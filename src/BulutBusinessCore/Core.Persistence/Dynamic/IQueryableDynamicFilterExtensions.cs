using System.Linq.Expressions;
using System.Reflection;

namespace BulutBusinessCore.Core.Persistence.Dynamic;

public static class IQueryableDynamicFilterExtensions
{
    public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQuery dynamicQuery)
    {
        if (dynamicQuery.Filter is not null)
            query = ApplyFilter(query, dynamicQuery.Filter);

        if (dynamicQuery.Sort is not null && dynamicQuery.Sort.Any())
            query = ApplySort(query, dynamicQuery.Sort);

        return query;
    }

    private static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, Filter filter)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var body = BuildExpressionTree<T>(filter, parameter);

        if (body == null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        return query.Where(lambda);
    }

    private static Expression? BuildExpressionTree<T>(Filter filter, ParameterExpression parameter)
    {
        if (string.IsNullOrWhiteSpace(filter.Field) || string.IsNullOrWhiteSpace(filter.Operator))
            return null;

        var property = typeof(T).GetProperty(filter.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
            throw new ArgumentException($"Property {filter.Field} not found on type {typeof(T).Name}");

        var left = Expression.Property(parameter, property);
        var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        object value = Convert.ChangeType(filter.Value, propertyType);

        Expression? comparison = filter.Operator.ToLower() switch
        {
            "eq" => Expression.Equal(left, Expression.Constant(value)),
            "neq" => Expression.NotEqual(left, Expression.Constant(value)),
            "lt" => Expression.LessThan(left, Expression.Constant(value)),
            "lte" => Expression.LessThanOrEqual(left, Expression.Constant(value)),
            "gt" => Expression.GreaterThan(left, Expression.Constant(value)),
            "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(value)),
            "contains" => Expression.Call(left, "Contains", null, Expression.Constant(value)),
            "startswith" => Expression.Call(left, "StartsWith", null, Expression.Constant(value)),
            "endswith" => Expression.Call(left, "EndsWith", null, Expression.Constant(value)),
            "isnull" => Expression.Equal(left, Expression.Constant(null)),
            "isnotnull" => Expression.NotEqual(left, Expression.Constant(null)),
            "in" => BuildInExpression(left, filter.Value, propertyType),
            "between" => BuildBetweenExpression(left, filter.Value, propertyType),
            _ => throw new NotSupportedException($"Operator {filter.Operator} is not supported")
        };

        // Eğer alt filtreler varsa, onları da işle:
        if (filter.Filters is { Count: > 0 })
        {
            var children = filter.Filters
                .Select(f => BuildExpressionTree<T>(f, parameter))
                .Where(e => e != null)
                .ToArray();

            if (children.Length == 0)
                return comparison;

            var logic = (filter.Logic ?? "and").ToLower();
            if (logic != "and" && logic != "or")
                throw new ArgumentException("Invalid logic. Must be 'and' or 'or'");

            Expression combined = children[0]!;
            for (int i = 1; i < children.Length; i++)
            {
                combined = logic == "and"
                    ? Expression.AndAlso(combined, children[i]!)
                    : Expression.OrElse(combined, children[i]!);
            }

            return comparison != null ? Expression.AndAlso(comparison, combined) : combined;
        }

        return comparison;
    }

    private static Expression BuildInExpression(Expression left, string value, Type propertyType)
    {
        var values = value.Split(',').Select(v => Convert.ChangeType(v.Trim(), propertyType)).ToArray();
        var constant = Expression.Constant(values);
        var containsMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(propertyType);
        return Expression.Call(containsMethod, constant, left);
    }

    private static Expression BuildBetweenExpression(Expression left, string value, Type propertyType)
    {
        var parts = value.Split(',');
        if (parts.Length != 2)
            throw new ArgumentException("Between operator needs 2 comma-separated values");

        var lower = Expression.GreaterThanOrEqual(left, Expression.Constant(Convert.ChangeType(parts[0], propertyType)));
        var upper = Expression.LessThanOrEqual(left, Expression.Constant(Convert.ChangeType(parts[1], propertyType)));

        return Expression.AndAlso(lower, upper);
    }

    private static IQueryable<T> ApplySort<T>(IQueryable<T> query, IEnumerable<Sort> sorts)
    {
        bool first = true;
        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sort in sorts)
        {
            var property = typeof(T).GetProperty(sort.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new ArgumentException($"Sort field '{sort.Field}' not found.");

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            string method = (sort.Dir.ToLower()) switch
            {
                "asc" => first ? "OrderBy" : "ThenBy",
                "desc" => first ? "OrderByDescending" : "ThenByDescending",
                _ => throw new ArgumentException($"Invalid sort direction: {sort.Dir}")
            };

            query = (IQueryable<T>)typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == method && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.PropertyType)
                .Invoke(null, new object[] { query, lambda })!;

            first = false;
        }

        return query;
    }
}

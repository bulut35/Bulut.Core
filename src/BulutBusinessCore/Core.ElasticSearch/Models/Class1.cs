using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulutBusinessCore.Core.ElasticSearch.Models;
public class ElasticSearchConfig
{
    public string ConnectionString { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public ElasticSearchConfig()
    {
        ConnectionString = string.Empty;
        UserName = string.Empty;
        Password = string.Empty;
    }

    public ElasticSearchConfig(string connectionString, string userName, string password)
    {
        ConnectionString = connectionString;
        UserName = userName;
        Password = password;
    }
}
public class ElasticSearchGetModel<T>
{
    public string ElasticId { get; set; }
    public T Item { get; set; }

    public ElasticSearchGetModel()
    {
        ElasticId = string.Empty;
        Item = default!;
    }

    public ElasticSearchGetModel(string elasticId, T item)
    {
        ElasticId = elasticId;
        Item = item;
    }
}
public class ElasticSearchInsertManyModel : ElasticSearchModel
{
    public object[] Items { get; set; }

    public ElasticSearchInsertManyModel(object[] items)
    {
        Items = items;
    }

    public ElasticSearchInsertManyModel(Id elasticId, string indexName, object[] items)
        : base(elasticId, indexName)
    {
        Items = items;
    }
}
public class ElasticSearchInsertUpdateModel : ElasticSearchModel
{
    public object Item { get; set; }

    public ElasticSearchInsertUpdateModel(object item)
    {
        Item = item;
    }

    public ElasticSearchInsertUpdateModel(Id elasticId, string indexName, object item)
        : base(elasticId, indexName)
    {
        Item = item;
    }
}
public class ElasticSearchModel
{
    public Id ElasticId { get; set; }
    public string IndexName { get; set; }

    public ElasticSearchModel()
    {
        ElasticId = null!;
        IndexName = string.Empty;
    }

    public ElasticSearchModel(Id elasticId, string indexName)
    {
        ElasticId = elasticId;
        IndexName = indexName;
    }
}
public class ElasticSearchResult : IElasticSearchResult
{
    public bool Success { get; }
    public string? Message { get; }

    public ElasticSearchResult()
    {
        Message = string.Empty;
    }

    public ElasticSearchResult(bool success, string? message = null)
    {
        Success = success;
        Message = message;
    }
}
public interface IElasticSearchResult
{
    public bool Success { get; }
    public string? Message { get; }
}
public class IndexModel
{
    public string IndexName { get; set; }
    public string AliasName { get; set; }
    public int NumberOfReplicas { get; set; } = 3;
    public int NumberOfShards { get; set; } = 3;

    public IndexModel()
    {
        IndexName = string.Empty;
        AliasName = string.Empty;
    }

    public IndexModel(string indexName, string aliasName)
    {
        IndexName = indexName;
        AliasName = aliasName;
    }
}
public class SearchByFieldParameters : SearchParameters
{
    public string FieldName { get; set; }
    public string Value { get; set; }

    public SearchByFieldParameters()
    {
        FieldName = string.Empty;
        Value = string.Empty;
    }

    public SearchByFieldParameters(string fieldName, string value)
    {
        FieldName = fieldName;
        Value = value;
    }
}
public class SearchByQueryParameters : SearchParameters
{
    public string QueryName { get; set; }
    public string Query { get; set; }
    public string[] Fields { get; set; }

    public SearchByQueryParameters()
    {
        QueryName = string.Empty;
        Query = string.Empty;
        Fields = Array.Empty<string>();
    }

    public SearchByQueryParameters(string queryName, string query, string[] fields)
    {
        QueryName = queryName;
        Query = query;
        Fields = fields;
    }
}
public class SearchParameters
{
    public string IndexName { get; set; }
    public int From { get; set; } = 0;
    public int Size { get; set; } = 10;

    public SearchParameters()
    {
        IndexName = string.Empty;
    }

    public SearchParameters(string indexName, int from, int size)
    {
        IndexName = indexName;
        From = from;
        Size = size;
    }
}
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BulutBusiness.Core.Persistence;
public static class ServiceCollectionRepositoriesAddExtensions
{
    public static IServiceCollection AddApplicationPersistenceRepositories(
        this IServiceCollection services,
        Assembly applicationAssembly,
        Assembly persistenceAssembly)
    {
        // Application katmanındaki tüm arayüzleri bul
        var interfaceTypes = applicationAssembly.GetTypes()
            .Where(t => t.IsInterface && t.Name.StartsWith("I") && t.Name.EndsWith("Repository"));

        // Persistence katmanındaki tüm implementasyonları bul
        var implementationTypes = persistenceAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"));

        foreach (var interfaceType in interfaceTypes)
        {
            // Persistence katmanındaki implementasyonları arayüzlere eşleştir
            var implementationType = implementationTypes
                .FirstOrDefault(t => $"I{t.Name}" == interfaceType.Name);

            if (implementationType != null)
            {
                // Scoped yaşam süresiyle DI'a ekle
                services.AddScoped(interfaceType, implementationType);
            }
        }

        return services;
    }
}
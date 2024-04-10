using API.Core.Services;
using API.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace API.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services
            .AddServices()
            .AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        Assembly coreAssembly = typeof(IService).Assembly;
        Assembly infrastructureAssembly = typeof(Service).Assembly;

        var abstractionTypes = coreAssembly.GetExportedTypes()
            .Where(t => t.IsInterface && t.Namespace != null && t.Namespace.EndsWith(".Services"));

        abstractionTypes = abstractionTypes.Where(t => t != typeof(IService));

        foreach (var abstractionType in abstractionTypes)
        {
            string concreteTypeName = $"{infrastructureAssembly.GetName().Name}.Services.{abstractionType.Name.Substring(1)}";
            var concreteType = infrastructureAssembly.GetType(concreteTypeName);

            if (concreteType != null)
            {
                services.AddTransient(abstractionType, concreteType);
            }
        }

        return services;
    }

}

using API.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Data.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<ApiContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("ApiContext"));
        });
    }
}

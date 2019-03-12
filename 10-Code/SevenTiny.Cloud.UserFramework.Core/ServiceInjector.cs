using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using SevenTiny.Cloud.UserFramework.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.UserFramework.Core
{
    public static class ServiceInjector
    {
        public static void InjectCore(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            services.AddScoped<UserFrameworkDbContext>();
        }
    }
}
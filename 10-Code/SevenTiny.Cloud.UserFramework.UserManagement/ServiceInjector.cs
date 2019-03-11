using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.UserFramework.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.UserFramework.UserManagement
{
    public static class ServiceInjector
    {
        public static void InjectDomain(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());
        }
    }
}
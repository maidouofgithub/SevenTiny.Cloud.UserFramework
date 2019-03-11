using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.UserFramework.Infrastructure.Configs;

namespace SevenTiny.Cloud.UserFramework.Core.Repository
{
    [DataBase("MultiTenantPlatformWeb")]
    public class UserFrameworkDbContext : MySqlDbContext<UserFrameworkDbContext>
    {
        public UserFrameworkDbContext() : base(ConnectionStringsConfig.Get("UserFrameworkDb"))
        {
            //开启一级缓存
            OpenQueryCache = true;
            CacheMediaType = Bantina.Bankinate.Cache.CacheMediaType.Local;
        }
    }
}

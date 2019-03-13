using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.UserFramework.Core.ServiceContract
{
    public interface IUserSecurityService : IRepository<UserSecurity>
    {
        /// <summary>
        /// 生成密钥,用于校验请求是否被篡改使用的盐值
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Result GenerateSecretKey(int userId);
        /// <summary>
        /// 根据用户账号信息获取Token
        /// RSA256(账号信息+过期时间)
        /// 可以直接使用RSA256解密进行获取账号信息，无需访问持久化数据，提高系统效率
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result GenerateToken(Account account);
        /// <summary>
        /// token解码
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Account TokenDecrypt(string token);
    }
}

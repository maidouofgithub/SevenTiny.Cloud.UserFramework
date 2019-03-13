using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.UserFramework.UserManagement.ServiceContract
{
    public interface IUserLoginService
    {
        /// <summary>
        /// 登陆并获取Token
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result LoginAndGetToken(Account account);
    }
}

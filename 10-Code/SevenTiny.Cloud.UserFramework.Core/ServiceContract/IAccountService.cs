using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.UserFramework.Core.ServiceContract
{
    public interface IAccountService : IUserCommonInfoRepository<Account>
    {
        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        bool VerifyPassword(int userId, string pwd);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        Result ChangePassword(int userId, string newPwd);
        /// <summary>
        /// 修改用户信息（特殊场景请直接调用修改单个字段的接口，不要直接调用这该接口）
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result ChangeEmail(int userId, string email);
        Result ChangePhone(int userId, string phone);
        /// <summary>
        /// 根据注册方式发送验证信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result SendRegisterMsgByRegisteredMedia(Account account);
        /// <summary>
        /// 根据注册方式校验验证信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="verificationCode">校验码，可以是各种字符串，如手机验证码，邮箱注册加密串等</param>
        /// <returns></returns>
        Result VerifyRegisterInfoByRegisteredMedia(Account account, string verificationCode);
    }
}

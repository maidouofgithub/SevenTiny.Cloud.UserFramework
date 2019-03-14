using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.UserFramework.Core.ServiceContract
{
    public interface IAccountService : IUserCommonInfoRepository<Account>
    {
        Result IsExist(int userId);
        Result ValidateRegisterd(string phone, string email);
        Result GetByPhoneOrEmail(string phone, string email);
        /// <summary>
        /// 正常登陆是不知道userid的，用下面这个对象承载登陆信息进行校验
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Result VerifyPassword(Account account);
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
        /// 通过短信或手机验证码校验
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Result VerifyRegisterMsgByPhoneCode(string phone, string verificationCode);
        /// <summary>
        /// 通过邮箱验证码校验
        /// </summary>
        /// <param name="email"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Result VerifyRegisterMsgByEmailCode(string email, string verificationCode);
        /// <summary>
        /// 根据邮箱注册链接的发送的验证码校验
        /// </summary>
        /// <param name="emailLinkVerificationCode"></param>
        /// <returns></returns>
        Result VerifyRegisterMsgByEmailLinkCode(string emailLinkVerificationCode);
    }
}

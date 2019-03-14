using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using SevenTiny.Cloud.UserFramework.UserManagement.ValueObject;

namespace SevenTiny.Cloud.UserFramework.UserManagement.ServiceContract
{
    public interface IUserRegisterService
    {
        /// <summary>
        /// 执行注册动作(发送注册验证码等）
        /// </summary>
        /// <param name="userInfoDTO"></param>
        /// <returns></returns>
        Result RegisterAction(UserInfoDTO userInfoDTO);

        Result VerifyPhoneAndAccomplish(string phone, string verificationCode);
        Result VerifyEmailCodeAndAccomplish(string email, string verificationCode);
        Result VerifyEmailLinkCodeAndAccomplish(string emailLinkVerificationCode);
    }
}

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
        /// <summary>
        /// 校验注册验证信息然后完成注册动作
        /// </summary>
        /// <param name="userInfoDTO"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Result VerifyRegisterInfoAndAccomplish(UserInfoDTO userInfoDTO, string verificationCode);
    }
}

using SevenTiny.Bantina;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using SevenTiny.Cloud.UserFramework.UserManagement.ServiceContract;
using SevenTiny.Cloud.UserFramework.UserManagement.ValueObject;

namespace SevenTiny.Cloud.UserFramework.UserManagement.Service
{
    public class UserRegisterService : IUserRegisterService
    {
        public UserRegisterService(
            IAccountService _accountService,
             IUserInfoService _userInfoService
            )
        {
            accountService = _accountService;
            userInfoService = _userInfoService;
        }

        private readonly IAccountService accountService;
        private readonly IUserInfoService userInfoService;

        public Result RegisterAction(UserInfoDTO userInfoDTO)
        {
            return Result.Success()
                .ContinueAssert(userInfoDTO == null, "注册信息不能为空")
                .ContinueAssert(userInfoDTO.RegisteredMedia == (int)Core.Enum.RegisteredMedia.UnKnown, "注册方式未确认")
                .Continue(() =>
                {
                    return accountService.SendRegisterMsgByRegisteredMedia(userInfoDTO.ToAccount());
                })
                ;
        }

        public Result VerifyRegisterInfoAndAccomplish(UserInfoDTO userInfoDTO, string verificationCode)
        {
            return Result.Success()
                .Continue(() =>
                {
                    //校验验证码
                    return accountService.VerifyRegisterInfoByRegisteredMedia(userInfoDTO.ToAccount(), verificationCode);
                })
                .Continue(() =>
                {
                    return TransactionHelper.Transaction<Result>(() =>
                    {
                        return userInfoService.Add(userInfoDTO.ToUserInfo())
                        .Continue(() =>
                        {
                            return accountService.Add(userInfoDTO.ToAccount());
                        });
                    });
                });
        }
    }
}

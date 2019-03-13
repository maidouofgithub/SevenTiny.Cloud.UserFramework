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

        public Result RegisterAction(Account account)
        {
            return Result.Success()
                .ContinueAssert(account == null, "注册信息不能为空")
                .ContinueAssert(account.RegisteredMedia == (int)Core.Enum.RegisteredMedia.UnKnown, "注册方式未确认")
                //校验是否已注册
                .Continue(accountService.ValidateRegisterd(account.Phone, account.Email))
                //发送注册验证信息
                .Continue(accountService.SendRegisterMsgByRegisteredMedia(account));
        }

        public Result VerifyRegisterInfoAndAccomplish(UserInfoDTO userInfoDTO)
        {
            var account = userInfoDTO.ToAccount();
            return Result.Success()
                .ContinueAssert(account == null, "注册信息不能为空")
                //校验是否已注册
                .Continue(accountService.ValidateRegisterd(account.Phone, account.Email))
                //校验验证码
                .Continue(result =>
                {
                    return accountService.VerifyRegisterInfoByRegisteredMedia(account, userInfoDTO.VerificationCode);
                })
                //添加注册信息
                .Continue(result =>
                {
                    //事务处理多条信息
                    return TransactionHelper.Transaction<Result>(() =>
                    {
                        return userInfoService.Add(userInfoDTO.ToUserInfo())
                        .Continue(re =>
                        {
                            return accountService.Add(account);
                        });
                    });
                });
        }
    }
}

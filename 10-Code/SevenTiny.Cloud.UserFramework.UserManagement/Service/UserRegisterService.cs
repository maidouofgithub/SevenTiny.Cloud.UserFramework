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
        public UserRegisterService(IAccountService _accountService)
        {
            accountService = _accountService;
        }

        private readonly IAccountService accountService;

        public Result RegisterAction(UserInfoDTO userInfoDTO)
        {
            return Result.Success()
                .ContinueAssert(userInfoDTO == null, "注册信息不能为空")
                .ContinueAssert(userInfoDTO.RegisteredMedia == 0 || userInfoDTO.RegisteredMedia == (int)Core.Enum.RegisteredMedia.UnKnown, "注册方式未确认")
                .Continue(() =>
                {
                    return accountService.SendRegisterMsgByRegisteredMedia(new Account
                    {
                        Name = userInfoDTO.Name,
                        RegisteredMedia = userInfoDTO.RegisteredMedia,
                        Email = userInfoDTO.Email,
                        Phone = userInfoDTO.Phone
                    });
                })
                ;
        }

        //这个方法的契约待定
        public Result ValidateRegisterInfoAndAccomplish(UserInfoDTO userInfoDTO)
        {
            return TransactionHelper.Transaction<Result>(() =>
            {
                //先校验注册校验信息
                return Result.Error();
            });
        }
    }
}

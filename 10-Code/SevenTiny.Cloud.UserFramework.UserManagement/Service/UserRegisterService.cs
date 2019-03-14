using SevenTiny.Bantina;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Infrastructure.Caching;
using SevenTiny.Cloud.UserFramework.Infrastructure.Const;
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

        private string GetRegisterUserInfoKey(string key)
        {
            return string.Concat(CacheKeyPrefixConst.REGISTER_USER_INFO_, key);
        }

        public Result RegisterAction(UserInfoDTO userInfoDTO)
        {
            var account = userInfoDTO.ToAccount();
            var userInfo = userInfoDTO.ToUserInfo();
            return Result.Success()
                .ContinueAssert(account.RegisteredMedia == (int)Core.Enum.RegisteredMedia.UnKnown, "注册方式未确认")
                //校验是否已注册
                .Continue(accountService.ValidateRegisterd(account.Phone, account.Email))
                //缓存注册信息
                .Continue(re =>
                {
                    //获取缓存key
                    string key;
                    switch ((Core.Enum.RegisteredMedia)account.RegisteredMedia)
                    {
                        case Core.Enum.RegisteredMedia.UnKnown:
                            return Result.Error("注册方式未确认");
                        case Core.Enum.RegisteredMedia.Phone:
                            key = account.Phone;
                            break;
                        case Core.Enum.RegisteredMedia.SMS:
                            key = account.Phone;
                            break;
                        case Core.Enum.RegisteredMedia.Email:
                            key = account.Email;
                            break;
                        default:
                            return Result.Error("注册方式未确认");
                    }
                    //缓存用户注册信息
                    LocalCacheHelper.AddOrUpdate(GetRegisterUserInfoKey(key), userInfoDTO, CacheExpiredConst.VERIFICATION_CODE_EXPIRED_SECOND);
                    return re;
                })
                //发送注册验证信息
                .Continue(accountService.SendRegisterMsgByRegisteredMedia(account));
        }

        public Result VerifyPhoneAndAccomplish(string phone, string verificationCode)
        {
            return Result.Success()
                 //校验是否注册
                 .Continue(accountService.ValidateRegisterd(phone, string.Empty))
                 //校验验证码
                 .Continue(accountService.VerifyRegisterMsgByPhoneCode(phone, verificationCode))
                 //添加用户
                 .Continue(re =>
                 {
                     var userInfo = LocalCacheHelper.Get<UserInfoDTO>(GetRegisterUserInfoKey(phone));
                     return re
                        .ContinueAssert(userInfo != null, "用户注册信息已过期，请重新注册")
                        .Continue(AddRegisterUserInfo(userInfo));
                 });
        }

        public Result VerifyEmailCodeAndAccomplish(string email, string verificationCode)
        {
            return Result.Success()
                 //校验是否注册
                 .Continue(accountService.ValidateRegisterd(email, string.Empty))
                 //校验验证码
                 .Continue(accountService.VerifyRegisterMsgByEmailCode(email, verificationCode))
                 //添加用户
                 .Continue(re =>
                 {
                     var userInfo = LocalCacheHelper.Get<UserInfoDTO>(GetRegisterUserInfoKey(email));
                     return re
                        .ContinueAssert(userInfo != null, "用户注册信息已过期，请重新注册")
                        .Continue(AddRegisterUserInfo(userInfo));
                 });
        }

        public Result VerifyEmailLinkCodeAndAccomplish(string emailLinkVerificationCode)
        {
            return Result.Success()
                 //校验验证码
                 .Continue(accountService.VerifyRegisterMsgByEmailLinkCode(emailLinkVerificationCode))
                 //添加用户
                 .Continue(re =>
                 {
                     //这里上一步的校验验证码要将email地址在Result中返回以供这里使用
                     var userInfo = LocalCacheHelper.Get<UserInfoDTO>(GetRegisterUserInfoKey(re.Data.ToString()));
                     return re
                        .ContinueAssert(userInfo != null, "用户注册信息已过期，请重新注册")
                        .Continue(AddRegisterUserInfo(userInfo));
                 });
        }

        private Result AddRegisterUserInfo(UserInfoDTO userInfoDTO)
        {
            var account = userInfoDTO.ToAccount();
            var userInfo = userInfoDTO.ToUserInfo();
            return Result.Success()
                .Continue(re =>
                {
                    return TransactionHelper.Transaction<Result>(() =>
                    {
                        return re
                        //添加Account
                         .Continue(accountService.Add(account))
                         //获取新的Account,回写UserId
                         .Continue(re2 =>
                         {
                             userInfo.UserId = ((Account)accountService.GetByPhoneOrEmail(account.Phone, account.Email).Data).UserId;
                             return re2;
                         })
                         //添加userInfo
                         .Continue(userInfoService.Add(userInfo));
                    });
                });
        }
    }
}

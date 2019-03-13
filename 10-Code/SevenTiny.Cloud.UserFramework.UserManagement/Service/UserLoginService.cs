using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using SevenTiny.Cloud.UserFramework.UserManagement.ServiceContract;
using SevenTiny.Cloud.UserFramework.UserManagement.ValueObject;

namespace SevenTiny.Cloud.UserFramework.UserManagement.Service
{
    public class UserLoginService : IUserLoginService
    {
        public UserLoginService(
            IAccountService _accountService,
            IUserSecurityService _userSecurityService
            )
        {
            accountService = _accountService;
            userSecurityService = _userSecurityService;
        }

        readonly IAccountService accountService;
        readonly IUserSecurityService userSecurityService;

        public Result LoginAndGetToken(Account account)
        {
            return Result.Success()
                //校验用户身份
                .Continue(accountService.VerifyPassword(account))
                //生成token
                .Continue(re =>
                {
                    re.Data = userSecurityService.GenerateToken(account);
                    return re;
                });
        }
    }
}

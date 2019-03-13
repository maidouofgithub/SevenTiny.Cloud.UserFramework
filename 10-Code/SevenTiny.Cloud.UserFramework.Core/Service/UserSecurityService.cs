using Newtonsoft.Json;
using SevenTiny.Bantina.Security;
using SevenTiny.Cloud.UserFramework.Core.Const;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Core.ValueObject;
using SevenTiny.Cloud.UserFramework.Infrastructure.Configs;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.UserFramework.Core.Service
{
    public class UserSecurityService : Repository<UserSecurity>, IUserSecurityService
    {
        public UserSecurityService(
            UserFrameworkDbContext _dbContext,
            IAccountService _accountService
            ) : base(_dbContext)
        {
            dbContext = _dbContext;
            accountService = _accountService;
        }

        private readonly UserFrameworkDbContext dbContext;
        private readonly IAccountService accountService;

        public Result GenerateSecretKey(int userId)
        {
            return Result.Success()
                //校验用户是否注册
                .Continue(accountService.IsExist(userId))
                //生成密钥
                .Continue(re =>
                {
                    string secretKey = MD5Helper.GetMd5Hash(string.Concat(SecretKeyConst.SaltBefore, "SecretKey", userId, DateTime.Now.ToString(), SecretKeyConst.SaltAfter));
                    if (!double.TryParse(UserFrameworkConfig.Get("SecretKeyEffectiveTime"), out double secretKeyEffectiveTime))
                        return Result.Error("获取密钥过期时间配置错误");
                    var expiredTime = DateTime.Now.AddSeconds(secretKeyEffectiveTime);
                    var userSecrity = dbContext.QueryOne<UserSecurity>(t => t.UserId == userId);
                    if (userSecrity == null)
                    {
                        dbContext.Add<UserSecurity>(new UserSecurity
                        {
                            UserId = userId,
                            SecretKey = secretKey,
                            ExpiredTime = expiredTime
                        });
                    }
                    else
                    {
                        userSecrity.SecretKey = secretKey;
                        userSecrity.ExpiredTime = expiredTime;
                        dbContext.Update(userSecrity);
                    }
                    re.Data = secretKey;
                    return re;
                });
        }

        public string GenerateToken(Account account)
        {
            return RSAHelper.Encrypt(JsonConvert.SerializeObject(new Token
            {
                ExpiredTime = DateTime.Now.AddSeconds(double.Parse(UserFrameworkConfig.Get("TokenEffectiveTime")))
            }.Attach(account)), UserFrameworkConfig.Get("RSA_PublicKey"));
        }

        public Account TokenDecrypt(string token)
        {
            return JsonConvert.DeserializeObject<Account>(RSAHelper.Decrypt(token, UserFrameworkConfig.Get("RSA_PrivateKey")));
        }
    }
}

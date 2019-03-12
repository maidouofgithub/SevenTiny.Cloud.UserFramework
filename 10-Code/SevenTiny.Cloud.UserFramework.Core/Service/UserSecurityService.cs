using Newtonsoft.Json;
using SevenTiny.Bantina.Security;
using SevenTiny.Cloud.UserFramework.Core.Const;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Core.ValueObject;
using SevenTiny.Cloud.UserFramework.Infrastructure.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.UserFramework.Core.Service
{
    public class UserSecurityService : Repository<UserSecurity>, IUserSecurityService
    {
        public UserSecurityService(UserFrameworkDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        UserFrameworkDbContext dbContext;

        public string GenerateSecretKey(int userId)
        {
            return MD5Helper.GetMd5Hash(string.Concat(SecretKeyConst.SaltBefore, "SecretKey", userId, SecretKeyConst.SaltAfter));
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

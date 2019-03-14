using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using SevenTiny.Bantina.Security;
using SevenTiny.Cloud.UserFramework.Infrastructure.Const;
using SevenTiny.Cloud.UserFramework.Infrastructure.Caching;
using SevenTiny.Cloud.UserFramework.Infrastructure.DataGenerator;
using SevenTiny.Cloud.UserFramework.Infrastructure.Configs;

namespace SevenTiny.Cloud.UserFramework.Core.Service
{
    public class AccountService : UserCommonInfoRepository<Account>, IAccountService
    {
        public AccountService(UserFrameworkDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        UserFrameworkDbContext dbContext;

        public Result IsExist(int userId)
        {
            return new Result { IsSuccess = dbContext.QueryExist<Account>(t => t.UserId == userId), Message = "该用户未注册" };
        }

        public Result ValidateRegisterd(string phone, string email)
        {
            return Result.Success()
                            .ContinueAssert(!string.IsNullOrEmpty(phone) || !string.IsNullOrEmpty(email), "邮箱或手机信息为空")
                            .Continue(re =>
                            {
                                bool exist = false;
                                if (!string.IsNullOrEmpty(phone))
                                    exist = dbContext.QueryExist<Account>(t => t.Phone == phone);
                                else if (!string.IsNullOrEmpty(email))
                                    exist = dbContext.QueryExist<Account>(t => t.Email == email);
                                return re.ContinueAssert(!exist, "已存在该用户的注册信息");
                            });
        }

        public Result GetByPhoneOrEmail(string phone, string email)
        {
            return Result.Success()
                            .ContinueAssert(!string.IsNullOrEmpty(phone) || !string.IsNullOrEmpty(email), "邮箱或手机信息为空")
                            .Continue(re =>
                            {
                                re.Data = null;
                                if (!string.IsNullOrEmpty(phone))
                                    re.Data = dbContext.QueryOne<Account>(t => t.Phone == phone);
                                else if (!string.IsNullOrEmpty(email))
                                    re.Data = dbContext.QueryExist<Account>(t => t.Email == email);
                                return re.ContinueAssert(re.Data != null, "该用户未注册");
                            });
        }

        /// <summary>
        /// 为密码加盐
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private string GetSaltPwd(string pwd)
        {
            //为降低暴力破解的可能，密码强制前后加盐
            return MD5Helper.GetMd5Hash(string.Concat(SecretKeyConst.SaltBefore, "account", pwd, SecretKeyConst.SaltAfter));
        }

        public new Result Add(Account account)
        {
            //密码加盐
            account.Password = GetSaltPwd(account.Password);
            base.Add(account);
            return Result.Success();
        }

        public new Result Update(Account account)
        {
            var old = base.GetByUserId(account.UserId);
            if (old != null)
            {
                old.Name = account.Name;
                old.Email = account.Email;
                old.Phone = account.Phone;
                return base.Update(old);
            }
            return Result.Error();
        }

        public Result VerifyPassword(Account account)
        {
            return Result.Success()
                .ContinueAssert(string.IsNullOrEmpty(account.Phone) || string.IsNullOrEmpty(account.Email), "邮箱或手机信息为空")
                .ContinueAssert(string.IsNullOrEmpty(account.Password), "用户密码信息为空")
                .Continue(re =>
                {
                    var entity = dbContext.QueryOne<Account>(t => t.Phone == account.Phone || t.Email == account.Email);
                    return re
                        .ContinueAssert(entity != null, "该用户未注册")
                        .ContinueAssert(entity.Password.Equals(GetSaltPwd(account.Password)), "账号或密码不正确");
                });
        }

        public Result ChangePassword(int userId, string newPwd)
        {
            var old = base.GetByUserId(userId);
            if (old != null)
            {
                //密码加盐
                old.Password = GetSaltPwd(newPwd);
                return base.Update(old);
            }
            return Result.Error();
        }

        public Result ChangeEmail(int userId, string email)
        {
            var old = base.GetByUserId(userId);
            if (old != null)
            {
                //修改邮箱逻辑
                //...
                old.Email = email;
                return base.Update(old);
            }
            return Result.Error();
        }

        public Result ChangePhone(int userId, string phone)
        {
            var old = base.GetByUserId(userId);
            if (old != null)
            {
                //修改手机号逻辑
                //...
                old.Phone = phone;
                return base.Update(old);
            }
            return Result.Error();
        }

        public Result SendRegisterMsgByRegisteredMedia(Account account)
        {
            switch ((Core.Enum.RegisteredMedia)account.RegisteredMedia)
            {
                case Core.Enum.RegisteredMedia.UnKnown:
                    return Result.Error("注册方式未确认");
                case Core.Enum.RegisteredMedia.Phone:
                    string randomCode = RandomNumberGenerator.NBitNumber(Convert.ToInt32(UserFrameworkConfig.Get("RandomNumberBit")));
                    return Result.Success();
                case Core.Enum.RegisteredMedia.SMS:
                    return Result.Success();
                case Core.Enum.RegisteredMedia.Email:
                    return Result.Success();
                default:
                    return Result.Error("注册方式未确认");
            }
        }

        public Result VerifyRegisterMsgByEmailLinkCode(string emailLinkVerificationCode)
        {
            //这里需要在此解析完链接后，校验用户是否被注册过
            //...

            //Result的Data要返回email地址，上游要用
            throw new NotImplementedException();
        }

        public Result VerifyRegisterMsgByPhoneCode(string phone, string verificationCode)
            => Result.Success()
                .Continue(re =>
                {
                    var code = LocalCacheHelper.Get<string>(GetRegisterVerificationCodeKey(phone));
                    return re
                        .ContinueAssert(!string.IsNullOrEmpty(code), "验证码已过期")
                        .ContinueAssert(code.Equals(verificationCode), "验证码不正确");
                });

        public Result VerifyRegisterMsgByEmailCode(string email, string verificationCode)
            => Result.Success()
                .Continue(re =>
                {
                    var code = LocalCacheHelper.Get<string>(GetRegisterVerificationCodeKey(email));
                    return re
                        .ContinueAssert(!string.IsNullOrEmpty(code), "验证码已过期")
                        .ContinueAssert(code.Equals(verificationCode), "验证码不正确");
                });

        private string GetRegisterVerificationCodeKey(string key)
        {
            return string.Concat(CacheKeyPrefixConst.REGISTER_VERIFICATION_CODE_, key);
        }
    }
}

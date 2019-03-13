using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using SevenTiny.Bantina.Security;
using SevenTiny.Cloud.UserFramework.Core.Const;

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
                            .ContinueAssert(string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email), "邮箱或手机信息为空")
                            .Continue(re =>
                            {
                                var exist = dbContext.QueryExist<Account>(t => t.Phone == phone || t.Email == email);
                                return re.ContinueAssert(!exist, "已存在该用户的注册信息");
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
                    return Result.Success();
                case Core.Enum.RegisteredMedia.SMS:
                    return Result.Success();
                case Core.Enum.RegisteredMedia.Email:
                    return Result.Success();
                default:
                    break;
            }
            return Result.Error();
        }

        public Result VerifyRegisterInfoByRegisteredMedia(Account account, string verificationCode)
        {
            switch ((Core.Enum.RegisteredMedia)account.RegisteredMedia)
            {
                case Core.Enum.RegisteredMedia.UnKnown:
                    return Result.Error("注册方式未确认");
                case Core.Enum.RegisteredMedia.Phone:
                    return Result.Success();
                case Core.Enum.RegisteredMedia.SMS:
                    return Result.Success();
                case Core.Enum.RegisteredMedia.Email:
                    return Result.Success();
                default:
                    break;
            }
            return Result.Error("验证注册验证码发生未知异常");
        }
    }
}

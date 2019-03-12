using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using SevenTiny.Bantina.Security;

namespace SevenTiny.Cloud.UserFramework.Core.Service
{
    public class AccountService : UserCommonInfoRepository<Account>, IAccountService
    {
        public AccountService(UserFrameworkDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        UserFrameworkDbContext dbContext;

        //密码加密的盐值
        private static readonly string saltBefore = "seventiny.cloud.";
        private static readonly string saltAfter = ".CYj(9yyz*8";

        /// <summary>
        /// 为密码加盐
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private string GetSaltPwd(string pwd)
        {
            //为降低暴力破解的可能，密码强制前后加盐
            //saltPwd="seventiny.cloud.123456.CYj(9yyz*8"
            return MD5Helper.GetMd5Hash(string.Concat(saltBefore, pwd, saltAfter));
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

        public bool VerifyPassword(int userId, string pwd)
        {
            var entity = base.GetByUserId(userId, t => new { t.UserId, t.Password });
            if (entity != null)
            {
                //存储的加盐密码和传入的密码加盐的值相比较
                if (entity.Password.Equals(GetSaltPwd(pwd)))
                {
                    return true;
                }
            }
            return false;
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

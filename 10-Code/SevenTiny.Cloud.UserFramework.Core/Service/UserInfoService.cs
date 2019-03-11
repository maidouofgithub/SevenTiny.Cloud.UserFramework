using SevenTiny.Cloud.UserFramework.Core.ServiceContract;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.UserFramework.Core.Service
{
    public class UserInfoService : UserCommonInfoRepository<UserInfo>, IUserInfoService
    {
        public UserInfoService(UserFrameworkDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        UserFrameworkDbContext dbContext;

        public Result AddUserInfo(UserInfo userInfo)
        {
            base.Add(userInfo);

            return Result.Success();
        }

        public Result UpdateUserInfo(UserInfo userInfo)
        {
            var old = base.GetByUserId(userInfo.UserId);
            if (old != null)
            {
                old.OfficePhone = userInfo.OfficePhone;
                old.QQ = userInfo.QQ;
                old.WeChat = userInfo.WeChat;
                return base.Update(old);
            }
            return Result.Error();
        }
    }
}

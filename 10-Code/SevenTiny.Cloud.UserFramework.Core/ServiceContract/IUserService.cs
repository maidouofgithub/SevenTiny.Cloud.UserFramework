using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.UserFramework.Core.ServiceContract
{
    public interface IUserService : IUserCommonInfoRepository<User>
    {
        Result AddUser(User user);
        Result UpdateUser(User user);
        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        bool VerifyPassword(int userId, string pwd);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        Result ChangePassword(int userId, string newPwd);
        /// <summary>
        /// 修改用户信息（特殊场景请直接调用修改单个字段的接口，不要直接调用这该接口）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Result ChangeEmail(int userId, string email);
        Result ChangePhone(int userId, string phone);
        Result SendRegisterMsgByRegisteredMedia(User user);
    }
}

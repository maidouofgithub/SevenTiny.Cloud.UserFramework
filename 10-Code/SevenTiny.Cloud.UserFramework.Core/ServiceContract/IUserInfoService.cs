using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.Repository;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.UserFramework.Core.ServiceContract
{
    public interface IUserInfoService : IUserCommonInfoRepository<UserInfo>
    {
    }
}

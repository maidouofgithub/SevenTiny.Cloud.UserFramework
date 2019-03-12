using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.UserFramework.Core.Entity
{
    /// <summary>
    /// 用户的主信息
    /// </summary>
    [Table]
    public class Account : UserCommonInfo
    {
        [Column]
        public int TenantId { get; set; } = 100000;//如果不是PaaS模式，默认100000
        [Column]
        public string Password { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Phone { get; set; }
        /// <summary>
        /// 注册媒介，标识是通过什么方式注册的。如手机/邮箱
        /// </summary>
        [Column]
        public int RegisteredMedia { get; set; } = (int)Enum.RegisteredMedia.UnKnown;

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <returns></returns>
        public Result ArgumentsCheck()
        {
            return Result.Success()
                .ContinueAssert(RegisteredMedia != (int)Enum.RegisteredMedia.UnKnown, "注册媒介不能为空")
                .Continue(re =>
                {
                    switch ((Enum.RegisteredMedia)this.RegisteredMedia)
                    {
                        case Enum.RegisteredMedia.UnKnown:
                            break;
                        case Enum.RegisteredMedia.Phone:
                            return re.ContinueAssert(string.IsNullOrEmpty(this.Phone), "手机号不能为空");
                        case Enum.RegisteredMedia.SMS:
                            return re.ContinueAssert(string.IsNullOrEmpty(this.Phone), "手机号不能为空");
                        case Enum.RegisteredMedia.Email:
                            return re.ContinueAssert(string.IsNullOrEmpty(this.Email), "邮箱不能为空");
                        default:
                            break;
                    }
                    return Result.Error("未知的注册媒介类型");
                });
        }
    }
}
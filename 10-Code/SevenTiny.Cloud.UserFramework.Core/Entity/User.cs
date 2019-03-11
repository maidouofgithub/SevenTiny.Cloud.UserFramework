using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.UserFramework.Core.Entity
{
    /// <summary>
    /// 用户的主信息
    /// </summary>
    [Table]
    public class User : UserCommonInfo
    {
        [Column]
        public int TenantId { get; set; }
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
    }
}

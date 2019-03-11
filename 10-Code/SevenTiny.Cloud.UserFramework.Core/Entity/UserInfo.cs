using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.UserFramework.Core.Entity
{
    /// <summary>
    /// 用户的扩展信息
    /// </summary>
    [Table]
    public class UserInfo : UserCommonInfo
    {
        [Column]
        public string OfficePhone { get; set; }
        [Column]
        public string QQ { get; set; }
        [Column]
        public string WeChat { get; set; }
    }
}

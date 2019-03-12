using SevenTiny.Cloud.UserFramework.Core.Entity;

namespace SevenTiny.Cloud.UserFramework.UserManagement.ValueObject
{
    public class UserInfoDTO
    {
        public int UserId { get; set; }
        public int TenantId { get; set; }
        public int CreateBy { get; set; } = -1;
        public int ModifyBy { get; set; } = -1;
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RegisteredMedia { get; set; } = (int)Core.Enum.RegisteredMedia.UnKnown;
        public string OfficePhone { get; set; }
        public string QQ { get; set; }
        public string WeChat { get; set; }

        public Account ToAccount() => new Account
        {
            UserId = this.UserId,
            TenantId = this.TenantId,
            Password = this.Password,
            Name = this.Name,
            Email = this.Email,
            Phone = this.Phone,
            RegisteredMedia = this.RegisteredMedia,
            CreateBy = this.CreateBy,
            ModifyBy = this.ModifyBy
        };

        public UserInfo ToUserInfo() => new UserInfo
        {
            UserId = this.UserId,
            CreateBy = this.CreateBy,
            ModifyBy = this.ModifyBy,
            OfficePhone = this.OfficePhone,
            QQ = this.QQ,
            WeChat = this.WeChat
        };
    }
}

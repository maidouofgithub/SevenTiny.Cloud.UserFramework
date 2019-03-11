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
    }
}

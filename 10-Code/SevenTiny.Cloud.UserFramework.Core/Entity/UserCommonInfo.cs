using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace SevenTiny.Cloud.UserFramework.Core.Entity
{
    public class UserCommonInfo
    {
        /// <summary>
        /// 用户Id是用户的主键
        /// </summary>
        [Key]
        [Column]
        [AutoIncrease]
        public int UserId { get; set; }
        [Column]
        public int IsDeleted { get; set; } = 0;
        [Column]
        public int CreateBy { get; set; } = -1;
        [Column("`CreateTime`")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [Column]
        public int ModifyBy { get; set; } = -1;
        [Column("`ModifyTime`")]
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}

using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.UserFramework.Core.Entity
{
    [Table]
    public class UserSecurity
    {
        [Key]
        [Column]
        public int UserId { get; set; }
        /// <summary>
        /// 用于验签的公钥，用户手动提交的
        /// </summary>
        [Column]
        public string PublicKey { get; set; }
        /// <summary>
        /// 用于签名的私钥，用户手动提交的
        /// </summary>
        [Column]
        public string PrivateKey { get; set; }
        [Column]
        public string SecretKey { get; set; }
        [Column]
        public DateTime ExpiredTime { get; set; }
    }
}

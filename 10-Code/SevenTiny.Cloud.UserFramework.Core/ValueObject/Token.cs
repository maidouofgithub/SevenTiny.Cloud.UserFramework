using SevenTiny.Cloud.UserFramework.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.UserFramework.Core.ValueObject
{
    /// <summary>
    /// Token传输对象实体
    /// </summary>
    public class Token
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime ExpiredTime { get; set; }

        public Token Attach(Account account)
        {
            this.UserId = account.UserId;
            this.Name = account.Name;
            this.Email = account.Email;
            this.Phone = account.Phone;
            return this;
        }
    }
}

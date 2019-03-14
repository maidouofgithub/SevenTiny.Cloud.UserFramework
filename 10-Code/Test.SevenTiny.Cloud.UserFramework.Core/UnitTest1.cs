using System;
using Xunit;
using SevenTiny.Cloud.UserFramework.Core.Enum;

namespace Test.SevenTiny.Cloud.UserFramework.Core
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            switch ((RegisteredMedia)5)
            {
                case RegisteredMedia.UnKnown:
                    break;
                case RegisteredMedia.Phone:
                    break;
                case RegisteredMedia.SMS:
                    break;
                case RegisteredMedia.Email:
                    break;
                default:
                    //没有匹配走这里
                    break;
            }
        }
    }
}

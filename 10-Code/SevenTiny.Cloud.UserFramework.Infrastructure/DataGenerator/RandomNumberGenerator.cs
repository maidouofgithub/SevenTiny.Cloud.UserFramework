using System;

namespace SevenTiny.Cloud.UserFramework.Infrastructure.DataGenerator
{
    public class RandomNumberGenerator
    {
        /// <summary>
        /// 获取N位随机数字符串
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static string NBitNumber(int bit)
        {
            if (bit <= 0)
                throw new ArgumentOutOfRangeException(nameof(bit), $"{nameof(bit)} arg must > 0");

            return new Random().Next((int)Math.Pow(10, bit - 1), (int)Math.Pow(10, bit) - 1).ToString();
        }
    }
}

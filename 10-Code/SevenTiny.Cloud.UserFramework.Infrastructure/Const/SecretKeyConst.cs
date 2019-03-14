namespace SevenTiny.Cloud.UserFramework.Infrastructure.Const
{
    /// <summary>
    /// 密钥常量
    /// </summary>
    public static class SecretKeyConst
    {
        /// <summary>
        /// 数值前面的盐值，建议后面再拼接用途
        /// </summary>
        public static readonly string SaltBefore = "seventiny.cloud.salt.";
        /// <summary>
        /// 数值后面的盐值
        /// </summary>
        public static readonly string SaltAfter = ".CYj(9yyz*8";
    }
}

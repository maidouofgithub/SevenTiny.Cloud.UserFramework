namespace SevenTiny.Cloud.UserFramework.Core.Enum
{
    /// <summary>
    /// 授权方式
    /// </summary>
    public enum AuthorizationType
    {
        Unknown = 0,
        /// <summary>
        /// 集中授权：该应用统一使用一个密钥进行授权验证，需要提供集中授权所用的用户id（数据库中密钥存在某个用户名下），密钥存在应用开发商端
        /// 适用于移动端，桌面应用
        /// </summary>
        CentralizedAuthorization = 1,
        /// <summary>
        /// 独立授权，每个应用使用服务端生成的密钥进行授权验证，密钥存在授权服务器的各自用户
        /// 适用于web端，密钥不能安全写死在代码中的场景
        /// </summary>
        IndependentAuthorization = 2
    }
}

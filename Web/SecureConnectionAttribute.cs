namespace System.Web
{
    /// <summary>
    /// 为控制器或操作方法提供安全连接标记设置
    /// <para>用于限制只有在使用安全的 HTTP 连接时才可以请求被标记的控制器或操作方法</para>
    /// </summary>
    public class SecureConnectionAttribute : AttributeBase
    {
        public override bool IsFatalError
        {
            get { return true; }
        }

        /// <summary>
        /// 声明只有在使用安全的 HTTP 连接时才可以请求该操作方法
        /// </summary>
        public SecureConnectionAttribute() { }

        /// <summary>
        /// 使用该 SecureConnection 标记设置验证当前 HTTP 请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Validate(HttpContextBase context)
        {
            return context.Request.IsSecureConnection;
        }
    }
}

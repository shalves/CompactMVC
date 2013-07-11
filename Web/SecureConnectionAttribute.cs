namespace System.Web
{
    /// <summary>
    /// 操作方法（Action）的 SecureConnection 标记类
    /// <para>用于限制只有在使用安全的Http连接时才可以请求被标记的操作方法</para>
    /// </summary>
    public class SecureConnectionAttribute : ActionAttribute
    {
        public override bool IsFatalError
        {
            get { return true; }
        }

        /// <summary>
        /// 声明只有在使用安全的Http连接时才可以请求该操作方法
        /// </summary>
        public SecureConnectionAttribute() { }

        /// <summary>
        /// 使用该SecureConnection标记设置验证当前Http请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Validate(HttpContextBase context)
        {
            return context.Request.IsSecureConnection;
        }
    }
}

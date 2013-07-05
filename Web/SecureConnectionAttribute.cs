namespace System.Web
{
    /// <summary>
    /// Action的SecureConnection标记类
    /// <para>用于限制只有在使用安全的Http连接时才可以请求被标记的Action</para>
    /// </summary>
    public class SecureConnectionAttribute : ActionAttribute
    {
        public override bool IsFatalError
        {
            get { return true; }
        }

        /// <summary>
        /// 声明只有在使用安全的Http连接时才可以请求该Action
        /// </summary>
        public SecureConnectionAttribute() { }

        public override bool Validate(HttpContextBase context)
        {
            return context.Request.IsSecureConnection;
        }
    }
}

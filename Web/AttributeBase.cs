namespace System.Web
{
    /// <summary>
    /// 为控制器和操作方法的标记类提供基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class AttributeBase : Attribute
    {
        /// <summary>
        /// 获取一个值，表示如果该标记验证失败是否会导致致命错误
        /// <para>当标记验证失败，应优先判断该值。当值为True时，应用程序应抛出异常或中止执行</para>
        /// </summary>
        public abstract bool IsFatalError { get; }

        /// <summary>
        /// 使用该标记设置验证当前HTTP请求
        /// </summary>
        /// <param name="context">HTTP请求的上下文信息</param>
        /// <returns></returns>
        public abstract bool Validate(HttpContextBase context);
    }
}

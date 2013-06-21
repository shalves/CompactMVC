namespace System.Web
{
    /// <summary>
    /// 为控制器Action的标记类提供基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ActionAttribute : Attribute
    {
        /// <summary>
        /// 获取一个值，表示如果该标记规则验证失败是否会导致致命错误
        /// <para>当标记验证失败，应优先判断该值。当值为True时，应用程序应抛出异常中止执行</para>
        /// </summary>
        public abstract bool IsFatalError { get; }

        /// <summary>
        /// 验证控制器的Action设定的标记
        /// </summary>
        /// <param name="controler"></param>
        /// <returns></returns>
        public abstract bool Validate(Controller controler);
    }
}

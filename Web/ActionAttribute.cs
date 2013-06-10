namespace System.Web
{
    /// <summary>
    /// 为控制器Action的标记类提供基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ActionAttribute : Attribute
    {
        /// <summary>
        /// 获取一个值，表示该标记规则判定失败后，应用程序是否应抛出异常从而中止执行
        /// </summary>
        public abstract bool IsFatalError { get; }

        /// <summary>
        /// 验证控制器的Action设定的标记
        /// </summary>
        /// <param name="controler"></param>
        /// <returns></returns>
        public abstract bool Validate(Controler controler);
    }
}

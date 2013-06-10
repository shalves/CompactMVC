namespace System.Web.UI
{
    /// <summary>
    /// 为客户端JS脚本提供基类
    /// </summary>
    public abstract class JavaScript
    {
        protected abstract string ScriptTextFormat { get; }
        public abstract override string ToString();
    }
}

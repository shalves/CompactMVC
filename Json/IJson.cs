namespace System.Json
{
    /// <summary>
    /// 用于实现Json对象和Json数组对象的约定
    /// </summary>
    public interface IJson
    {
        /// <summary>
        /// 获取或设置当前IJson对象在输出时是否引用属性名
        /// </summary>
        bool QuotePropertyName { get; set; }

        /// <summary>
        /// 获取该IJson对象的字符串表示
        /// </summary>
        string ToString();
    }
}

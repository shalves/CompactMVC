namespace System.Json
{
    /// <summary>
    /// JS对象或JS数组的标记接口
    /// </summary>
    public interface IJson
    {
        /// <summary>
        /// 获取该JS对象或数组的JSON文本形式
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}

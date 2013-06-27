namespace System.Json
{
    /// <summary>
    /// 用于实现Json对象或Json数组对象的约定
    /// </summary>
    public interface IJson
    {
        /// <summary>
        /// 获取该Json对象或Json数组对象的字符串表示
        /// </summary>
        /// <returns></returns>
        string GetJsonString();
    }
}

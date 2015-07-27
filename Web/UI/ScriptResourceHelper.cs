namespace System.Web.UI
{
    /// <summary>
    /// 提供对封装在控件中Script资源的辅助操作
    /// </summary>
    public class ScriptResourceHelper : ScriptReference
    {
        public ScriptResourceHelper(string scriptResName, string assembly)
            : base(scriptResName, assembly)
        { }

        /// <summary>
        /// 获取当前脚本的引用路径
        /// </summary>
        /// <param name="gzip">是否使用gzip格式压缩</param>
        /// <returns></returns>
        public string GetReferenceUrl(bool gzip)
        {
            return base.GetUrl(new ScriptManager(), gzip);
        }
    }
}

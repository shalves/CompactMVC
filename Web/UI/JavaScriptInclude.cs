namespace System.Web.UI
{
    /// <summary>
    /// 表示客户端JS脚本的引用
    /// </summary>
    public class JavaScriptInclude : JavaScript
    {
        protected override string ScriptTextFormat
        {
            get { return "<script src=\"{0}\" type=\"text/javascript\"></script>"; }
        }

        string _Src = string.Empty;

        public JavaScriptInclude(string src)
        {
            _Src = src;
        }

        public override string ToString()
        {
            return string.Format(ScriptTextFormat, _Src);
        }
    }
}

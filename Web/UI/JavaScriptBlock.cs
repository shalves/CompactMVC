using System.Text;

namespace System.Web.UI
{
    /// <summary>
    /// 表示客户端JS脚本块
    /// </summary>
    public class JavaScriptBlock : JavaScript
    {
        protected override string ScriptTextFormat
        {
            get { return "<script type=\"text/javascript\">\r\n//<![CDATA[\r\n{0}\r\n//]]>\r\n</script>"; }
        }

        StringBuilder _ScriptText;

        public JavaScriptBlock()
        {
            _ScriptText = new StringBuilder();
        }

        public JavaScriptBlock(string script)
        {
            _ScriptText = new StringBuilder(script);
        }

        public JavaScriptBlock Append(string script)
        {
            _ScriptText.Append(script);
            return this;
        }

        public JavaScriptBlock AppendFormat(string format, params object[] args)
        {
            _ScriptText.AppendFormat(format, args);
            return this;
        }

        public JavaScriptBlock AppendFormatLine(string format, params object[] args)
        {
            _ScriptText.AppendLine();
            _ScriptText.AppendFormat(format, args);
            return this;
        }

        public JavaScriptBlock AppendLine()
        {
            _ScriptText.AppendLine();
            return this;
        }

        public JavaScriptBlock AppendLine(string script)
        {
            _ScriptText.AppendLine(script);
            return this;
        }

        public override string ToString()
        {
            return string.Format(ScriptTextFormat, _ScriptText.ToString());
        }
    }
}

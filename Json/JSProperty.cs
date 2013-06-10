using System.Text;
using System.Extensions;
using System.Collections;

namespace System.Json
{
    /// <summary>
    /// 表示JS对象的值属性
    /// </summary>
    internal sealed class JSProperty
    {
        /// <summary>
        /// 获取该属性值的JS字符串形式
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string GetJSValue(object arg)
        {
            if (arg == null) return "null";

            string value = string.Empty;

            if (arg is char || arg is string || arg is DateTime)
            {
                value = "\"{0}\"".FormatWith(arg.ToString().JSEncode());
            }
            else if (arg is bool)
            {
                value = arg.ToString().ToLower();
            }
            else if (arg is IEnumerable)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                (arg as IEnumerable).Each((i, t) =>
                {
                    sb.Append(GetJSValue(t));
                    sb.Append(",");
                });
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                return sb.ToString();
            }
            else if (arg is IJson)
            {
                value = ((IJson)arg).ToString();
            }
            else
            {
                value = arg.ToString();
            }

            return value;
        }
    }
}

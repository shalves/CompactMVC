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
                value = string.Format("\"{0}\"", arg.ToString().JSEncode());
            }
            else if (arg is bool)
            {
                value = arg.ToString().ToLower();
            }
            else if (arg is IEnumerable)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (object obj in (IEnumerable)arg)
                {
                    sb.Append(GetJSValue(obj));
                    sb.Append(",");
                }
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

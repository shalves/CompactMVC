using System.Collections;
using System.Extensions;
using System.Text;

namespace System.Json
{
    /// <summary>
    /// 表示Json对象的属性
    /// </summary>
    internal sealed class JsonProperty
    {
        /// <summary>
        /// 获取指定对象在作为Json对象的属性时的字符串值
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string ValueOf(object arg)
        {
            if (arg == null) return "undefined";

            string value = string.Empty;

            if (arg is IJson)
            {
                value = ((IJson)arg).ToString();
            }
            else if (arg is char || arg is string || arg is DateTime)
            {
                value = string.Format("\"{0}\"", arg.ToString().JSEncode());
            }
            else if (arg is bool)
            {
                value = arg.ToString().ToLower();
            }
            else if (arg is IEnumerable)
            {
                int total = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                foreach (object obj in (IEnumerable)arg)
                {
                    sb.Append(", ");
                    sb.Append(ValueOf(obj));
                    total++;
                }
                if (total > 0) sb.Remove(1, 2);
                sb.Append("]");
                return sb.ToString();
            }
            else
            {
                value = arg.ToString();
            }

            return value;
        }
    }
}

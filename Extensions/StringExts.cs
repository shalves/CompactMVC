using System.Text;
using System.Web;

namespace System.Extensions
{
    public static partial class StringExts
    {
        /// <summary>
        /// 判断当前字符串对象是否为null或string.Empty值
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string arg)
        {
            return string.IsNullOrEmpty(arg);
        }

        /// <summary>
        /// 简化string.Format()的用法
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// 用指定数量的Unicode字符填充字符串的两侧
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="paddingChar"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Pad(this string arg, char paddingChar, int count)
        {
            string tmp = new string(paddingChar, count);
            return string.Format("{0}{1}{2}", tmp, arg, tmp);
        }

        /// <summary>
        /// 用指定的分隔符间隔，合并该字符串数组中所有的项
        /// </summary>
        /// <param name="array"></param>
        /// <param name="sparator">指定的分隔符</param>
        /// <returns></returns>
        public static string Merge(this string[] array, char sparator)
        {
            if (array == null || array.Length == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (string str in array)
            {
                sb.Append(sparator);
                sb.Append(str);
            }
            return sb.Remove(0, 1).ToString();
        }

        /// <summary>
        /// 复制并转换该字符串数组中的所有元素到int[]
        /// </summary>
        /// <param name="array">请确保元素中的所有项都可以转换为int类型</param>
        /// <exception cref="FormatException"></exception>
        /// <returns></returns>
        public static int[] AsIntArray(this string[] array)
        {
            int[] intArray = new int[array.Length];

            try
            {
                for (int i = 0; i < intArray.Length; i++)
                {
                    intArray[i] = int.Parse(array[i]);
                }
                return intArray;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 对字符串中的',"和\进行转码
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string JSEncode(this string arg)
        {
            if (string.IsNullOrEmpty(arg)) return "";

            return arg.
                Replace(@"\", @"\\").
                Replace("\'", "\\\'").
                Replace("\"", "\\\"");
        }

        /// <summary>
        /// 对字符串进行URL编码
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string UrlEncode(this string arg)
        {
            return Uri.EscapeDataString(arg);
        }

        /// <summary>
        /// 对URL编码的字符串进行解码
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string UrlDecode(this string arg)
        {
            return Uri.UnescapeDataString(arg);
        }
    }
}

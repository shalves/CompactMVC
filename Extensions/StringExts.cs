using System.Security.Cryptography;
using System.Text;

namespace System.Extensions
{
    public static partial class StringExts
    {
        /// <summary>
        /// 指示当前字符串对象是否为null或string.Empty值
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string arg)
        {
            return string.IsNullOrEmpty(arg);
        }

        /// <summary>
        /// 简化的string.Format()用法
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// 用指定数量的Unicode字符填充字符串的两端
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
        public static string Join(this string[] array, string sparator)
        {
            return string.Join(sparator, array);
        }

        /// <summary>
        /// 复制并转换该字符串数组中的所有元素到int[]
        /// <para>请确保元素中的所有项都可以转换为int类型</para>
        /// </summary>
        /// <param name="array"></param>
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
        /// 对字符串中的“'”，“"”和“\”进行转码
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string JSEncode(this string arg)
        {
            if (string.IsNullOrEmpty(arg)) return "";

            return arg.
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

        /// <summary>
        /// 返回当前字符串经过MD5加密后的结果
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string MD5Encrypt(this string arg)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            try
            {
                byte[] buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(arg));
                return BitConverter.ToString(buffer).Replace("-", "");
            }
            finally
            {
                md5.Clear();
            }
        }
    }
}

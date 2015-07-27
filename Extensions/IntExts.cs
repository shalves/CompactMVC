using System.Text;

namespace System.Extensions
{
    public static partial class IntExts
    {
        /// <summary>
        /// 返回由该int数组中所有元素组成的带有指定分隔符的字符串
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator">指定分隔符</param>
        /// <returns></returns>
        public static string Merge(this int[] array, char separator)
        {
            if (array == null || array.Length == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (int x in array)
            {
                sb.Append(separator);
                sb.Append(x);
            }
            return sb.Remove(0, 1).ToString();
        }
    }
}

using System.Text;

namespace System.Extensions
{
    public static partial class IntExts
    {
        /// <summary>
        /// 返回由该int[]中所有元素组成的带有指定分隔符的字符串形式
        /// </summary>
        /// <param name="array"></param>
        /// <param name="separator">指定的分隔符</param>
        /// <returns></returns>
        public static string Merge(this int[] array, char separator)
        {
            if (array == null || array.Length == 0) return string.Empty;
            StringBuilder tmp = new StringBuilder();
            array.Each<int>((i) =>
            {
                tmp.Append(separator);
                tmp.Append(i);
            });
            return tmp.Remove(0,1).ToString();
        }
    }
}

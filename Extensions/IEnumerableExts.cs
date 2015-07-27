using System.Collections;
using System.Collections.Generic;

namespace System.Extensions
{
    public static partial class IEnumerableExts
    {
        /// <summary>
        /// 遍历当前枚举对象，并执行指定的操作
        /// </summary>
        /// <param name="array"></param>
        /// <param name="action">要执行的操作（object：元素）</param>
        public static void Each(this IEnumerable array, Action<object> action)
        {
            foreach (object o in array)
            {
                action(o);
            }
        }

        /// <summary>
        /// 遍历当前枚举对象，并执行指定的操作
        /// </summary>
        /// <param name="array"></param>
        /// <param name="action">要执行的操作（int：索引，object：元素）</param>
        public static void Each(this IEnumerable array, Action<int, object> action)
        {
            IEnumerator t = array.GetEnumerator();
            int index = 0;
            while (t.MoveNext())
            {
                action(index++, t.Current);
            }
        }

        /// <summary>
        /// 遍历当前枚举对象，并执行指定的操作
        /// </summary>
        /// <param name="array"></param>
        /// <param name="func">要执行的操作（int：索引，object：元素，bool：返回false则中断）</param>
        public static void Each(this IEnumerable array, Func<int, object, bool> func)
        {
            IEnumerator t = array.GetEnumerator();
            int index = 0;
            while (t.MoveNext())
            {
                if (!func(index++, t.Current)) break;
            }
        }

        /// <summary>
        /// 遍历当前枚举对象，并执行指定的操作
        /// </summary>
        /// <typeparam name="T">枚举对象中元素的类型</typeparam>
        /// <param name="tarray"></param>
        /// <param name="action">要执行的操作（T：元素）</param>
        public static void Each<T>(this IEnumerable<T> tarray, Action<T> action)
        {
            foreach (T t in tarray)
            {
                action(t);
            }
        }

        /// <summary>
        /// 遍历当前枚举对象，并执行指定的操作
        /// </summary>
        /// <typeparam name="T">枚举对象中元素类型</typeparam>
        /// <param name="tarray"></param>
        /// <param name="action">要执行的操作（int：索引，T：元素）</param>
        public static void Each<T>(this IEnumerable<T> tarray, Action<int, T> action)
        {
            using (IEnumerator<T> t = tarray.GetEnumerator())
            {
                int index = 0;
                while (t.MoveNext())
                {
                    action(index++, t.Current);
                }
            }
        }

        /// <summary>
        /// 遍历当前枚举对象，并执行指定的操作
        /// </summary>
        /// <typeparam name="T">枚举对象中元素的类型</typeparam>
        /// <param name="tarray"></param>
        /// <param name="func">要执行的方法（int：索引，T：元素，bool：返回flase则中断）</param>
        public static void Each<T>(this IEnumerable<T> tarray, Func<int, T, bool> func)
        {
            using (IEnumerator<T> t = tarray.GetEnumerator())
            {
                int index = 0;
                while (t.MoveNext())
                {
                    if (!func(index++, t.Current)) break;
                }
            }
        }
    }
}

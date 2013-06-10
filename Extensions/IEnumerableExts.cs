using System.Collections;
using System.Collections.Generic;

namespace System.Extensions
{
    public static partial class IEnumerableExts
    {
        /// <summary>
        /// 遍历当前枚举对象,并执行指定的方法
        /// </summary>
        /// <param name="array"></param>
        /// <param name="action">要执行的方法(object元素)</param>
        public static void Each(this IEnumerable array, Action<object> action)
        {
            foreach (object o in array)
            {
                action(o);
            }
        }

        /// <summary>
        /// 遍历当前枚举对象,并执行指定的方法
        /// </summary>
        /// <param name="array"></param>
        /// <param name="action">要执行的方法(int索引, object元素)</param>
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
        /// 遍历当前枚举对象,并执行指定的方法
        /// </summary>
        /// <param name="arrar"></param>
        /// <param name="func">要执行的方法(int索引, object元素, bool返回false则中断遍历)</param>
        public static void Each(this IEnumerable arrar, Func<int, object, bool> func)
        {
            IEnumerator t = arrar.GetEnumerator();
            int index = 0;
            while (t.MoveNext())
            {
                if (!func(index++, t.Current)) break;
            }
        }

        /// <summary>
        /// 遍历当前枚举对象,并执行指定的方法
        /// </summary>
        /// <typeparam name="T">枚举对象中元素类型</typeparam>
        /// <param name="tArray"></param>
        /// <param name="action">要执行的方法(T元素)</param>
        public static void Each<T>(this IEnumerable<T> tArray, Action<T> action)
        {
            foreach (T t in tArray)
            {
                action(t);
            }
        }

        /// <summary>
        /// 遍历当前枚举对象,并执行指定的方法
        /// </summary>
        /// <typeparam name="T">枚举对象中元素类型</typeparam>
        /// <param name="tArray"></param>
        /// <param name="action">要执行的方法(int索引, T元素)</param>
        public static void Each<T>(this IEnumerable<T> tArray, Action<int, T> action)
        {
            using (IEnumerator<T> t = tArray.GetEnumerator())
            {
                int index = 0;
                while (t.MoveNext())
                {
                    action(index++, t.Current);
                }
            }
        }

        /// <summary>
        /// 遍历当前枚举对象,并执行指定的方法
        /// </summary>
        /// <typeparam name="T">枚举对象中元素类型</typeparam>
        /// <param name="tArray"></param>
        /// <param name="func">要执行的方法(int索引, T元素, bool返回flase则中断遍历)</param>
        public static void Each<T>(this IEnumerable<T> tArray, Func<int, T, bool> func)
        {
            using (IEnumerator<T> t = tArray.GetEnumerator())
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

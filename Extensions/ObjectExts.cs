using System.Reflection;

namespace System.Extensions
{
    public static partial class ObjectExts
    {
        /// <summary>
        /// 指示当前对象的值是否为null
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool IsNull(this object arg)
        {
            return arg == null;
        }

        /// <summary>
        /// 将指定的对象转换成与之等价的特定的值类型对象
        /// <para>如果可能</para>
        /// </summary>
        /// <typeparam name="T">指定的值类型</typeparam>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static T ParseTo<T>(this object arg)
        {
            if (arg == null) return default(T);

            try
            {
                return (T)Convert.ChangeType(arg, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 遍历匿名对象的公共属性,并对属性名称和值执行指定的方法
        /// </summary>
        /// <param name="anonymous"></param>
        /// <param name="action">指定执行的方法(string属性名,object属性值)</param>
        public static void EachProperty(this object anonymous, Action<string, object> action)
        {
            if (anonymous == null) return;

            Type t = anonymous.GetType();

            foreach (PropertyInfo property in t.GetProperties())
            {
                action(property.Name, property.GetValue(anonymous, null));
            }
        }
    }
}

using System.Reflection;

namespace System.Web
{
    /// <summary>
    /// 封装控制器操作方法的相关信息
    /// </summary>
    public class ActionMethodInfo
    {
        /// <summary>
        /// 获取该操作方法的基本方法信息的实例
        /// </summary>
        public MethodInfo BaseMethod {
            get; private set; 
        }

        /// <summary>
        /// 获取为该操作方法定义的Action标记的集合
        /// </summary>
        public ActionAttribute[] Attributes {
            get; private set; 
        }

        /// <summary>
        /// 获取该操作方法的名称
        /// </summary>
        public string Name { 
            get; private set; 
        }

        /// <summary>
        /// 获取该操作方法的参数类型的数组
        /// </summary>
        public Type[] ParameterTypes { 
            get; private set; 
        }

        /// <summary>
        /// 获取该操作方法的参数值的数组
        /// </summary>
        public object[] Parameters { 
            get; private set; 
        }

        /// <summary>
        /// 创建ActionMethodInfo类的新实例
        /// </summary>
        /// <param name="baseMethod"></param>
        /// <param name="name"></param>
        public ActionMethodInfo(MethodInfo baseMethod, string name)
        {
            BaseMethod = baseMethod;
            Name = name;
            ParameterTypes = Type.EmptyTypes;
            Parameters = null;
            SetActionAttributes();
        }

        /// <summary>
        /// 创建ActionMethodInfo类的新实例
        /// </summary>
        /// <param name="baseMethod"></param>
        /// <param name="name"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameters"></param>
        public ActionMethodInfo(
            MethodInfo baseMethod, string name, Type[] parameterTypes, object[] parameters) {
            BaseMethod = baseMethod;
            Name = name;
            ParameterTypes = parameterTypes;
            Parameters = parameters;
            SetActionAttributes();
        }

        /// <summary>
        /// 从基本方法信息的实例中获取所有Action标记并为Attributes属性赋值
        /// </summary>
        private void SetActionAttributes() {
            if (BaseMethod != null) {
                Attributes = BaseMethod.GetCustomAttributes(typeof(ActionAttribute), false) as ActionAttribute[];
            }
        }
    }
}

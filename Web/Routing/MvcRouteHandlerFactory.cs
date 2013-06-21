using System.Reflection;
using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 用于创建Mvc路由处理程序的工厂类
    /// </summary>
    public class MvcRouteHandlerFactory : IRouteHandlerFactory
    {
        readonly string _HandlersAssemblyName;
        /// <summary>
        /// 获取Mvc路由处理程序所在的程序集的名称
        /// </summary>
        public string HandlersAssemblyName 
        {
            get { return _HandlersAssemblyName; }
        }

        readonly string _HandlersNameSpace;
        /// <summary>
        /// 获取Mvc路由处理程序的统一名称空间
        /// </summary>
        public string HandlersNameSpace 
        {
            get { return _HandlersNameSpace; }
        }

        /// <summary>
        /// 初始化MvcRouteHandlerFactory的新实例
        /// </summary>
        /// <param name="handlersAssemblyName">指定Mvc路由处理程序所在的程序集的名称</param>
        public MvcRouteHandlerFactory(string handlersAssemblyName)
        {
            this._HandlersAssemblyName = handlersAssemblyName;
        }

        /// <summary>
        /// 初始化MvcRouteHandlerFactory的新实例
        /// </summary>
        /// <param name="handlersAssemblyName">指定Mvc路由处理程序所在的程序集的名称</param>
        /// <param name="handlersNameSpace">指定Mvc路由处理程序的统一名称空间</param>
        public MvcRouteHandlerFactory(string handlersAssemblyName, string handlersNameSpace)
        {
            this._HandlersAssemblyName = handlersAssemblyName;
            this._HandlersNameSpace = handlersNameSpace;
        }

        /// <summary>
        /// 创建Mvc路由处理程序的新实例
        /// </summary>
        /// <param name="handlerToken">指定处理程序的简要名称</param>
        /// <returns></returns>
        public IRouteHandler CreateRouteHandler(string handlerToken)
        {
            string prefix = 
                string.IsNullOrEmpty(HandlersNameSpace) ? HandlersAssemblyName : HandlersNameSpace;
            return new MvcRouteHandler(
                HandlersAssemblyName, string.Format("{0}.{1}Controller", prefix, handlerToken));
        }
    }
}

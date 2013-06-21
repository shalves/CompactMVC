using System.Reflection;

namespace System.Web.Routing
{
    /// <summary>
    /// 用于Mvc应用程序的路由请求处理程序
    /// </summary>
    internal sealed class MvcRouteHandler : IRouteHandler
    {
        readonly string _HandlersAssemblyName;
        /// <summary>
        /// 获取Mvc路由处理程序所在的程序集的名称
        /// </summary>
        public string HandlersAssemblyName
        {
            get { return _HandlersAssemblyName; }
        }

        readonly string _HandlerFullName;
        /// <summary>
        /// 获取Mvc路由处理程序的完整类型名
        /// </summary>
        public string HandlerFullName
        {
            get { return _HandlerFullName; }
        }

        static Assembly _HandlersAssembly;
        /// <summary>
        /// 获取Mvc路由处理程序所在的程序集的Assembly实例
        /// <para>单例模式</para>
        /// </summary>
        /// <returns></returns>
        internal Assembly GetHandlersAssembly()
        {
            if (MvcRouteHandler._HandlersAssembly == null)
                MvcRouteHandler._HandlersAssembly = Assembly.Load(this.HandlersAssemblyName);
            return MvcRouteHandler._HandlersAssembly;
        }

        /// <summary>
        /// 初始化MvcRouteHandler类的新实例
        /// </summary>
        /// <param name="handlersAssemblyName">Mvc路由处理程序所在的程序集的名称</param>
        /// <param name="handlerFullName">指定Mvc路由处理程序的完整类型名</param>
        public MvcRouteHandler(string handlersAssemblyName, string handlerFullName)
        { 
            this._HandlersAssemblyName = handlersAssemblyName;
            this._HandlerFullName = handlerFullName;
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            try
            {
                Assembly handlersAssembly = GetHandlersAssembly();
                IHttpHandler handler = handlersAssembly.CreateInstance(HandlerFullName) as IHttpHandler;

                if (handler == null) return null;

                ((IRouteable)handler).RequestContext = requestContext;

                return handler;
            }
            catch
            {
                throw;
            }
        }
    }
}

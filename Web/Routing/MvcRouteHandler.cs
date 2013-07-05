using System.Reflection;

namespace System.Web.Routing
{
    /// <summary>
    /// 用于在Web Mvc应用程序中处理Http路由请求
    /// </summary>
    internal sealed class MvcRouteHandler : IRouteHandler
    {
        readonly string _AssemblyName;
        readonly string _TypeName;

        /// <summary>
        /// 获取路由请求处理程序所在的程序集名称
        /// </summary>
        public string AssemblyName
        {
            get { return _AssemblyName; }
        }

        /// <summary>
        /// 获取路由请求处理程序的完整类型名
        /// </summary>
        public string TypeName
        {
            get { return _TypeName; }
        }

        static Assembly _Assembly;
        /// <summary>
        /// 获取路由请求处理程序所在程序集的Assembly实例
        /// <para>单例模式</para>
        /// </summary>
        /// <returns></returns>
        internal Assembly GetAssembly()
        {
            if (MvcRouteHandler._Assembly == null)
                MvcRouteHandler._Assembly = Assembly.Load(this.AssemblyName);
            return MvcRouteHandler._Assembly;
        }

        /// <summary>
        /// 初始化MvcRouteHandler类的新实例
        /// </summary>
        /// <param name="assemblyName">指定路由请求处理程序所在程序集的名称</param>
        /// <param name="typeName">指定路由请求处理程序的类型名</param>
        public MvcRouteHandler(string assemblyName, string typeName) { 
            this._AssemblyName = assemblyName;
            this._TypeName = typeName;
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext) {
            try
            {
                Assembly handlerAssembly = GetAssembly();

                IHttpHandler handler = handlerAssembly.CreateInstance(TypeName, true) as IHttpHandler;
                if (handler != null)
                {
                    if (handler is IController) ((IController)handler).Initialize(requestContext);
                }
                return handler;
            }
            catch
            {
                throw;
            }
        }
    }
}

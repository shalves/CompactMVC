using System.Reflection;

namespace System.Web.Routing
{
    /// <summary>
    /// 创建用于MvcApplication的路由请求处理程序
    /// </summary>
    internal sealed class MvcRouteHandler : IRouteHandler
    {
        /// <summary>
        /// 获取或设置处理程序所在的程序集名称
        /// </summary>
        public string AssemblyName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取处理程序的类型名称
        /// </summary>
        public string TypeName
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化MvcRouteHandler类的新实例
        /// </summary>
        /// <param name="assemblyName">处理程序所在的程序集名称</param>
        /// <param name="typeName">处理程序的类型名称</param>
        public MvcRouteHandler(string assemblyName, string typeName)
        { 
            this.AssemblyName = assemblyName;
            this.TypeName = typeName;
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            try
            {
                var assembly = Assembly.Load(AssemblyName);
                IHttpHandler handler = assembly.CreateInstance(
                    string.Format("{0}.{1}", AssemblyName, TypeName)) as IHttpHandler;

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

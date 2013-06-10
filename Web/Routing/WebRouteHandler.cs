using System.Web.Compilation;

namespace System.Web.Routing
{
    /// <summary>
    /// 创建用于Web应用程序的路由请求处理程序
    /// </summary>
    internal sealed class WebRouteHandler : IRouteHandler
    {
        /// <summary>
        /// 获取处理程序的虚拟路径
        /// </summary>
        public string VirtualPath
        {
            get; 
            private set; 
        }

        /// <summary>
        /// 初始化WebRouteHandler类的新实例
        /// </summary>
        /// <param name="virtualPath">指定处理程序的虚拟路径</param>
        public WebRouteHandler(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            try
            {
                IHttpHandler handler = BuildManager.
                    CreateInstanceFromVirtualPath(VirtualPath, typeof(IHttpHandler)) as IHttpHandler;

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
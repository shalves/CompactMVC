using System.Web.Compilation;

namespace System.Web.Routing
{
    /// <summary>
    /// 用于在Web应用程序中处理Http路由请求
    /// <para>可以通过Url直接请求的Http处理程序</para>
    /// </summary>
    internal class WebRouteHandler : IRouteHandler
    {
        readonly string _VirtualPath;
        /// <summary>
        /// 获取路由请求处理程序文件的完整虚拟路径
        /// </summary>
        public string VirtualPath
        {
            get { return _VirtualPath; }
        }

        /// <summary>
        /// 初始化WebRouteHandler类的新实例
        /// </summary>
        /// <param name="virtualPath">指定路由请求处理程序文件的完整虚拟路径</param>
        public WebRouteHandler(string virtualPath)
        {
            this._VirtualPath = virtualPath;
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            try
            {
                IHttpHandler handler = 
                    BuildManager.CreateInstanceFromVirtualPath(VirtualPath, typeof(IHttpHandler)) as IHttpHandler;

                if (handler != null && handler is IRouteable)
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
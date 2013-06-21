using System.Web.Compilation;

namespace System.Web.Routing
{
    /// <summary>
    /// 用于Web应用程序的路由请求处理程序
    /// <para>可以通过Url直接请求的IHttpHandler</para>
    /// </summary>
    internal sealed class WebRouteHandler : IRouteHandler
    {
        readonly string _HandlerVirtualPath;
        /// <summary>
        /// 获取Web路由处理程序的完整虚拟路径
        /// </summary>
        public string HandlerVirtualPath
        {
            get { return _HandlerVirtualPath; }
        }

        /// <summary>
        /// 初始化WebRouteHandler类的新实例
        /// </summary>
        /// <param name="handlerVirtualPath">指定Web路由处理程序的完整虚拟路径</param>
        public WebRouteHandler(string handlerVirtualPath)
        {
            this._HandlerVirtualPath = handlerVirtualPath;
        }

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            try
            {
                IHttpHandler handler = BuildManager.
                    CreateInstanceFromVirtualPath(HandlerVirtualPath, typeof(IHttpHandler)) as IHttpHandler;

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
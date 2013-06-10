using System.Web.Services.Protocols;

namespace System.Web.Routing
{
    /// <summary>
    /// Beta1
    /// </summary>
    public sealed class WebServiceRouteHandler : IRouteHandler
    {
        /// <summary>
        /// 获取WebService路由处理程序的虚拟路径
        /// </summary>
        public string VirtualPath
        {
            get; 
            private set; 
        }

        public WebServiceRouteHandler(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            WebServiceHandlerFactory wshFactory = new WebServiceHandlerFactory();

            return wshFactory.GetHandler(
                HttpContext.Current, "GET,POST", VirtualPath, HttpContext.Current.Server.MapPath(VirtualPath));
        }
    }
}

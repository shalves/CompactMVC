using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 封装有关当前Http路由请求和处理请求的控制器的信息
    /// </summary>
    public class ControllerContext
    {
        readonly RequestContext _RequestContext;
        readonly IController _Controller;
        readonly string _ActionName;

        /// <summary>
        /// 获取当前Http路由请求的上下文信息
        /// </summary>
        internal RequestContext RequestContext 
        {
            get { return _RequestContext; }
        }

        /// <summary>
        /// 获取处理当前Http路由请求的控制器实例
        /// </summary>
        public IController Controller 
        {
            get { return _Controller; }
        }

        /// <summary>
        /// 获取当前请求执行的Action名称
        /// <para>默认值：default</para>
        /// </summary>
        public string ActionName
        {
            get { return _ActionName ?? "default"; }
        }

        /// <summary>
        /// 获取当前Http请求的上下文件信息
        /// </summary>
        public HttpContextBase HttpContext
        {
            get { return RequestContext == null ? null : RequestContext.HttpContext; }
        }

        /// <summary>
        /// 从当前Http路由请求的上下文中获取路由数据
        /// </summary>
        public RouteData RouteData
        {
            get { return RequestContext == null ? null : RequestContext.RouteData; }
        }

        /// <summary>
        /// 初始化 ControllerContext 类的新实例
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controller"></param>
        public ControllerContext(RequestContext requestContext, IController controller) 
        {
            this._RequestContext = requestContext;
            this._Controller = controller;
            this._ActionName = GetRouteValue("action") as string;
        }

        /// <summary>
        /// 获取路由数据集合中指定名称的路由参数的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetRouteValue(string name)
        {
            if (RouteData == null) return null;
            object value = null;
            if (RouteData.Values.Count > 0)
                RouteData.Values.TryGetValue(name, out value);
            return value;
        }
    }
}

using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 封装有关当前Http路由请求和处理请求的控制器的信息
    /// </summary>
    public class ControllerContext {

        readonly RequestContext _RequestContext;
        readonly IController _Controller;

        /// <summary>
        /// 获取当前Http路由请求的上下文信息
        /// </summary>
        internal RequestContext RequestContext {
            get { return this._RequestContext; }
        }

        /// <summary>
        /// 获取处理当前Http路由请求的控制器实例
        /// </summary>
        public IController Controller {
            get { return this._Controller; }
        }

        /// <summary>
        /// 获取当前请求执行的Action名称
        /// <para>默认值：default</para>
        /// </summary>
        public string ActionName {
            get {
                string t = RouteData.Values["action"] as string;
                return t ?? "default";
            }
        }

        /// <summary>
        /// 获取当前Http请求的上下文件信息
        /// </summary>
        public HttpContextBase HttpContext {
            get { return this._RequestContext.HttpContext; }
        }

        /// <summary>
        /// 从当前Http路由请求的上下文中获取路由数据
        /// </summary>
        public RouteData RouteData {
            get { return this.RequestContext.RouteData; }
        }

        /// <summary>
        /// 初始化 ControllerContext 类的新实例
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controller"></param>
        public ControllerContext(RequestContext requestContext, IController controller) {
            this._RequestContext = requestContext;
            this._Controller = controller;
        }
    }
}

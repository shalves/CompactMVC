using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 用于在支持Http路由的Mvc Web应用程序中实现Controller的约定
    /// </summary>
    public interface IController : IActionExecutor
    {
        /// <summary>
        /// 获取控制的名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取有关当前Http路由请求和控制器的上下文信息
        /// </summary>
        ControllerContext ControllerContext { get; }

        /// <summary>
        /// 使用Http路由请求的上下文信息初始化控制器
        /// </summary>
        /// <param name="requestContext"></param>
        void Initialize(RequestContext requestContext);
    }
}

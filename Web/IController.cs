using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 用于在支持 HTTP 路由的 MVC Web 应用程序中实现控制器（Controller）的约定
    /// </summary>
    public interface IController : IActionExecutor
    {
        /// <summary>
        /// 获取控制品的名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取有关当前 HTTP 路由请求和控制器的上下文信息
        /// </summary>
        ControllerContext ControllerContext { get; }

        /// <summary>
        /// 使用 HTTP 路由请求的上下文信息初始化控制器
        /// </summary>
        /// <param name="requestContext"></param>
        void Initialize(RequestContext requestContext);

        /// <summary>
        /// 当前控制器的标记验证失败时引发的事件
        /// </summary>
        event EventHandler<ControllerAttributeValidateFailedEventArgs> ControllerAttributeValidateFailed;
    }
}

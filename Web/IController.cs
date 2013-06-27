using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 用于在支持Http路由的Mvc Web应用程序中实现Controller的约定
    /// </summary>
    internal interface IController : IHttpHandler, IRouteable
    {
        string Action { get; }
    }
}

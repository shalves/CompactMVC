namespace System.Web.Routing
{
    /// <summary>
    /// 定义ASP.NET Web应用程序为处理路由参数而实现的协定
    /// </summary>
    internal interface IRouteable
    {
        /// <summary>
        /// 获取或设置有关此次请求的上下文信息
        /// </summary>
        RequestContext RequestContext { get; set; }
    }
}

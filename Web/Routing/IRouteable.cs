namespace System.Web.Routing
{
    /// <summary>
    /// 用于实现处理Http路由参数的约定
    /// </summary>
    internal interface IRouteable
    {
        /// <summary>
        /// 获取或设置当前Http路由请求的上下文信息
        /// </summary>
        RequestContext RequestContext { get; set; }
    }
}

namespace System.Web.Routing
{
    /// <summary>
    /// 为实现对Http路由数据的处理提供接口支持
    /// </summary>
    internal interface IRouteable
    {
        /// <summary>
        /// 获取或设置当前Http路由请求的上下文信息
        /// </summary>
        RequestContext RequestContext { get; set; }
    }
}

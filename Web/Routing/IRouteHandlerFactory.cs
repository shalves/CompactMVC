namespace System.Web.Routing
{
    /// <summary>
    /// 用于实现Http路由请求处理程序工厂类的约定
    /// </summary>
    public interface IRouteHandlerFactory
    {
        /// <summary>
        /// 创建路由请求处理程序的新实例
        /// </summary>
        /// <param name="handlerToken"></param>
        /// <returns></returns>
        IRouteHandler CreateRouteHandler(string handlerToken);
    }
}

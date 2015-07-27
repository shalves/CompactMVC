namespace System.Web.Routing
{
    /// <summary>
    /// 为Http路由请求处理程序的工厂类提供基类
    /// </summary>
    public abstract class RouteHandlerFactory : IRouteHandlerFactory
    {
        static IRouteHandlerFactory _Current;

        /// <summary>
        /// 获取当前用于创建路由处理程序的工厂类对象
        /// </summary>
        public static IRouteHandlerFactory Current
        {
            get { return _Current; }
        }

        /// <summary>
        /// 指定用于创建路由处理程序的工厂类对象
        /// </summary>
        /// <param name="routeHandlerFactory"></param>
        public static void SetRouteHandlerFactory(IRouteHandlerFactory routeHandlerFactory)
        {
            _Current = routeHandlerFactory;
        }

        /// <summary>
        /// 创建Http路由请求处理程序的新实例
        /// </summary>
        /// <param name="handlerToken"></param>
        /// <returns></returns>
        public abstract IRouteHandler CreateRouteHandler(string handlerToken);
    }
}

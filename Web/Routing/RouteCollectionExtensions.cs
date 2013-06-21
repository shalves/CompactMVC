namespace System.Web.Routing
{
    public static class RouteCollectionExtensions
    {
        static IRouteHandlerFactory _RouteHandlerFactory;

        static IRouteHandler GetRouteHandler(string handlerToken)
        {
            if (_RouteHandlerFactory == null)
                throw new ArgumentException("未指定用于创建路由处理程序的工厂类对象", "IRouteHandlerFactory");
            return _RouteHandlerFactory.CreateRouteHandler(handlerToken);
        }

        /// <summary>
        /// 指定用于创建路由处理程序的工厂类对象
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="routeHandlerFactory"></param>
        public static void SetRouteHandlerFactory(this RouteCollection routes, IRouteHandlerFactory routeHandlerFactory)
        {
            _RouteHandlerFactory = routeHandlerFactory;
        }

        public static void MapRoute(this RouteCollection routes, string url, string handlerToken)
        {
            Route route = new Route(url, GetRouteHandler(handlerToken));
            routes.Add(route);
        }

        public static void MapRoute(this RouteCollection routes, string name, string url, string handlerToken)
        {
            Route route = new Route(url, GetRouteHandler(handlerToken));
            routes.Add(name, route);
        }

        public static void MapRoute(this RouteCollection routes, string url, RouteValueDictionary defaults, string handlerToken)
        {
            Route route = new Route(url, defaults, GetRouteHandler(handlerToken));
            routes.Add(route);
        }

        public static void MapRoute(this RouteCollection routes, string name, string url, RouteValueDictionary defaults, string handlerToken)
        {
            Route route = new Route(url, defaults, GetRouteHandler(handlerToken));
            routes.Add(name, route);
        }

        public static void MapRoute(this RouteCollection routes, 
            string url, RouteValueDictionary defaults, RouteValueDictionary constraints, string handlerToken)
        {
            Route route = new Route(url, defaults, constraints, GetRouteHandler(handlerToken));
            routes.Add(route);
        }

        public static void MapRoute(this RouteCollection routes,
            string name, string url, RouteValueDictionary defaults, RouteValueDictionary constraints, string handlerToken)
        {
            Route route = new Route(url, defaults, constraints, GetRouteHandler(handlerToken));
            routes.Add(name, route);
        }
    }
}

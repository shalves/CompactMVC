namespace System.Web.Routing
{
    public static class RouteCollectionExtensions
    {
        static IRouteHandler GetRouteHandler(string handlerToken)
        {
            if (RouteHandlerFactory.Current == null)
                throw new ArgumentException("未指定用于创建Http路由请求处理程序的工厂类对象", "RouteHandlerFactory.Current");
            return RouteHandlerFactory.Current.CreateRouteHandler(handlerToken);
        }

        /// <summary>
        /// 指定用于创建路由处理程序的工厂类对象
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="routeHandlerFactory"></param>
        public static void SetRouteHandlerFactory(this RouteCollection routes, IRouteHandlerFactory routeHandlerFactory)
        {
            RouteHandlerFactory.SetRouteHandlerFactory(routeHandlerFactory);
        }

        /// <summary>
        /// 忽略对指定路径规则的Http请求的路由
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="constraints"></param>
        public static void IgnoreRoute(this RouteCollection routes, params string[] constraints)
        {
            if (constraints != null && constraints.Length > 0)
            {
                foreach (string c in constraints)
                {
                    Route route = 
                        new Route("{Action}", null, new RouteValueDictionary { { "Action", c } }, new StopRoutingHandler());
                    routes.Add(route);
                }
            }
        }

        public static void MapRoute(this RouteCollection routes, string url, string handlerToken)
        {
            routes.MapRoute(url, handlerToken, new { Action = "Default" });
        }

        public static void MapRoute(this RouteCollection routes, string url, string handlerToken, object defaults)
        {
            Route route = new Route(url,
                new RouteValueDictionary(defaults), GetRouteHandler(handlerToken));
            routes.Add(route);
        }

        public static void MapRoute(this RouteCollection routes, string url, string handlerToken, object defaults, object constraints)
        {
            Route route = new Route(url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints), GetRouteHandler(handlerToken));
            routes.Add(route);
        }

        public static void MapRoute(this RouteCollection routes, string name, string url, string handlerToken)
        {
            routes.MapRoute(name, url, handlerToken, new { Action = "Default" });
        }

        public static void MapRoute(this RouteCollection routes, string name, string url, string handlerToken, object defaults)
        {
            Route route = new Route(url, 
                new RouteValueDictionary(defaults), GetRouteHandler(handlerToken));
            routes.Add(name, route);
        }

        public static void MapRoute(this RouteCollection routes, string name, string url, string handlerToken, object defaults, object constraints)
        {
            Route route = new Route(url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints), GetRouteHandler(handlerToken));
            routes.Add(name, route);
        }
    }
}

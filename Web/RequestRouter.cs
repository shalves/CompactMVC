using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 为在处理程序间转移Http路由请求提供支持
    /// </summary>
    public class RequestRouter
    {
        readonly HttpContextBase _Context;

        /// <summary>
        /// 获取当前Http请求的上下文
        /// </summary>
        protected internal HttpContextBase Context
        {
            get { return this._Context; }
        }

        /// <summary>
        /// 初始化 RequestRouter 类的新实例
        /// </summary>
        /// <param name="context">指定Http请求的上下文信息</param>
        protected internal RequestRouter(HttpContextBase context)
        {
            this._Context = context;
        }

        /// <summary>
        /// 保存当前请求的会话状态
        /// </summary>
        protected void SaveSessionState()
        {
            bool a, b;
            SessionState.SessionIDManager sidManager = new SessionState.SessionIDManager();
            sidManager.SaveSessionID(HttpContext.Current, Context.Session.SessionID, out a, out b);
        }

        protected virtual IRouteHandler GetRouteHandler(string handlerToken)
        {
            return RouteHandlerFactory.Current.CreateRouteHandler(handlerToken);
        }

        protected virtual void Route(RouteData routeData)
        {
            //先保存会话状态
            SaveSessionState();

            var reuqestContext = new RequestContext(Context, routeData);

            //重写请求路径
            var newPath = routeData.Route.GetVirtualPath(reuqestContext, null).VirtualPath;
            reuqestContext.HttpContext.RewritePath(newPath);

            //指定新的处理程序
            IHttpHandler handler = routeData.RouteHandler.GetHttpHandler(reuqestContext);
            reuqestContext.HttpContext.Handler = handler;

            if (handler == null)
                throw new Exception("未能从指定路由中获取到 IHttpHandler");

            handler.ProcessRequest(HttpContext.Current);
        }
        
        /// <summary>
        /// 将请求转发到指定名称的路由进行处理
        /// <para>处理程序可共享SessionState和Request.Params</para>
        /// </summary>
        /// <param name="routeName"></param>
        /// <param name="values"></param>
        public void ToRoute(string routeName, object values)
        {
            var route = RouteTable.Routes[routeName] as Route;
            if (route == null)
                throw new Exception(string.Format("路由表中不存在名为 \"{0}\" 的路由", routeName));
            RouteData routeData = new RouteData(route, route.RouteHandler);
            foreach (var pair in new RouteValueDictionary(values))
            {
                routeData.Values[pair.Key] = pair.Value;
            }
            Route(routeData);
        }

        /// <summary>
        /// 将请求转发到指定路径的路由进行处理
        /// <para>处理程序可共享SessionState和Request.Params</para>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="handlerToken"></param>
        /// <param name="values"></param>
        public void ToUrl(string url, string handlerToken, object values)
        {
            var routeHandler = GetRouteHandler(handlerToken);
            var routeData = new RouteData(new Route(url, routeHandler), routeHandler);
            foreach (var pair in new RouteValueDictionary(values))
            {
                routeData.Values[pair.Key] = pair.Value;
            }
            Route(routeData);
        }
    }
}

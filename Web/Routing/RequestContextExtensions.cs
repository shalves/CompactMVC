namespace System.Web.Routing
{
    internal static class RequestContextExtensions
    {
        /// <summary>
        /// 从Http路由请求的上下文中获取路由数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RouteData GetRouteData(this RequestContext context)
        {
            return context == null ? null : context.RouteData;
        }

        /// <summary>
        /// 从Http路由请求的上下文中获取指定名称的路由参数的值
        /// </summary>
        /// <param name="routeData"></param>
        /// <param name="name">路由参数的名称</param>
        /// <returns></returns>
        public static object GetRouteValue(this RequestContext context, string name)
        {
            object resultValue = null;
            var routeDate = GetRouteData(context);
            if (routeDate != null && routeDate.Values.Count > 0)
                routeDate.Values.TryGetValue(name, out resultValue);
            return resultValue;
        }
    }
}

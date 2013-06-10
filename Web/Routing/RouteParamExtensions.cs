namespace System.Web.Routing
{
    internal static class RouteParamExtensions
    {
        /// <summary>
        /// 从路由请求的上下文中获取路由参数的集合
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RouteValueDictionary GetRouteParamCollection(this RequestContext context)
        {
            return context == null ? null : context.RouteData.Values;
        }

        /// <summary>
        /// 获取路由参数集合中指定名称的参数的值
        /// </summary>
        /// <param name="valueDictionary"></param>
        /// <param name="key">路由参数的名称</param>
        /// <returns></returns>
        public static object GetRouteValue(this RouteValueDictionary valueDictionary, string key)
        {
            object resultValue = null;

            if (valueDictionary != null && valueDictionary.Count > 0)
                valueDictionary.TryGetValue(key, out resultValue);

            return resultValue;
        }
    }
}

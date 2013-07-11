using System.Web.Routing;

namespace System.Web.UI
{
    /// <summary>
    /// 表示可被路由的 Asp.NET WebForm 页面的类型
    /// </summary>
    public class RouteablePage : Page, IRouteable
    {
        RequestContext _RequestContext;
        RouteData _RouteData;

        #region IRouteable 成员
        RequestContext IRouteable.RequestContext 
        {
            get { return this._RequestContext; }
            set { this._RequestContext = value; }
        }
        #endregion

        /// <summary>
        /// 从当前Http路由请求的上下文中获取路由数据
        /// </summary>
        public RouteData RouteData
        {
            get { return _RequestContext == null ? null : _RequestContext.RouteData; }
        }

        /// <summary>
        /// 从当前Http路由请求的上下文中获取指定名称的路由参数的值
        /// </summary>
        /// <param name="name">路由参数的名称</param>
        /// <returns></returns>
        public object GetRouteValue(string name)
        {
            if (RouteData == null) return null;
            object value = null;
            if (RouteData.Values.Count > 0)
                RouteData.Values.TryGetValue(name, out value);
            return value;
        }
    }
}

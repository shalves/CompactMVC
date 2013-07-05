using System.Web.Routing;

namespace System.Web.UI
{
    /// <summary>
    /// 表示可被路由的 Asp.NET WebForm 页面的类型
    /// </summary>
    public class RouteablePage : Page, IRouteable
    {
        RequestContext IRouteable.RequestContext { get; set; }

        RouteData _RouteData;
        /// <summary>
        /// 从当前Http路由请求的上下文中获取路由数据
        /// </summary>
        public RouteData RouteData
        {
            get
            {
                if (_RouteData == null)
                    _RouteData = ((IRouteable)this).RequestContext.GetRouteData();
                return _RouteData;
            }
        }

        /// <summary>
        /// 从当前Http路由请求的上下文中获取指定名称的路由参数的值
        /// </summary>
        /// <param name="name">路由参数的名称</param>
        /// <returns></returns>
        public object GetRouteValue(string name)
        {
            return ((IRouteable)this).RequestContext.GetRouteValue(name);
        }
    }
}

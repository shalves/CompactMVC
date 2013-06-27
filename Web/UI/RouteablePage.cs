using System.Web.Routing;

namespace System.Web.UI
{
    /// <summary>
    /// 表示可被路由的WebForm页面的类型
    /// </summary>
    public class RouteablePage : Page, IRouteable
    {
        RequestContext IRouteable.RequestContext { get; set; }

        RouteValueDictionary _RouteValues;
        /// <summary>
        /// 获取路由中URL参数值和默认值的集合
        /// </summary>
        public RouteValueDictionary RouteValues
        {
            get
            {
                if (_RouteValues == null)
                    _RouteValues = ((IRouteable)this).RequestContext.GetRouteParamCollection();
                return _RouteValues;
            }
        }

        /// <summary>
        /// 获取路由中指定名称的URL参数的值
        /// <para>当指定的名称的路径值不存在时，不会引发异常</para>
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns></returns>
        public object GetRouteValue(string key)
        {
            return RouteValues.GetRouteValue(key);
        }
    }
}

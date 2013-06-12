using System;
using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 为路由请求的处理程序提供基类
    /// <para>用于Contorler和自定义的HttpHandler, 如(一般处理程序ashx)</para>
    /// </summary>
    public abstract class RouteableHttpHandler : IHttpHandler, IRouteable
    {
        bool _IsReusable = false;
        public virtual bool IsReusable
        {
            get { return _IsReusable; }
            protected set { _IsReusable = value; }
        }

        HttpContext _Context;
        /// <summary>
        /// 获取此次Http请求的上下文
        /// </summary>
        public HttpContext Context
        {
            get { return this._Context; }
        }

        /// <summary>
        /// 获取此次Http请求所使用的方法
        /// </summary>
        public HttpVerb HttpMethod
        {
            get { return Context.Request.HttpMethod.ToHttpVerb(); }
        }

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
        /// Http处理程序的主入口处
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            _Context = context;
            OnProcessRequest();
        }

        /// <summary>
        /// 重写此方法以实现对Http请求的处理和响应
        /// </summary>
        protected abstract void OnProcessRequest();

        /// <summary>
        /// 获取路由中指定名称的URL参数的值
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns></returns>
        public object GetRouteValue(string key)
        {
            return RouteValues.GetRouteValue(key);
        }
    }
}

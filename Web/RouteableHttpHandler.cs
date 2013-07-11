using System.Security.Principal;
using System.Web.Caching;
using System.Web.Routing;
using System.Web.SessionState;

namespace System.Web
{
    /// <summary>
    /// 为Http路由请求处理程序提供基类
    /// </summary>
    public abstract class RouteableHttpHandler : IHttpHandler, IRouteable
    {
        bool _IsReusable = false;
        HttpContext _Context;
        RequestContext _RequestContext;

        #region IHttpHandler 成员
        public virtual bool IsReusable
        {
            get { return _IsReusable; }
            protected set { _IsReusable = value; }
        }

        /// <summary>
        /// Http处理程序的主入口处
        /// </summary>
        /// <param name="context"></param>
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            this._Context = context;
            OnProcessRequest();
        }
        #endregion

        #region IRouteable 成员
        RequestContext IRouteable.RequestContext
        {
            get { return _RequestContext; }
            set { _RequestContext = value; }
        }
        #endregion

        #region 常用的ASP.NET对象
        /// <summary>
        /// 获取当前Http请求的上下文
        /// </summary>
        public HttpContext Context
        {
            get { return this._Context; }
        }

        /// <summary>
        /// 获取当前Http请求的Application对象
        /// </summary>
        public HttpApplicationState Application
        {
            get { return Context == null ? null : Context.Application; }
        }

        /// <summary>
        /// 获取当前应用程序域的Cache对象
        /// </summary>
        public Cache Cache
        {
            get { return Context == null ? null : Context.Cache; }
        }

        /// <summary>
        /// 获取当前Http请求的Request对象
        /// </summary>
        public HttpRequest Request
        {
            get { return Context == null ? null : Context.Request; }
        }

        /// <summary>
        /// 获取当前Http请求的Response对象
        /// </summary>
        public HttpResponse Response
        {
            get { return Context == null ? null : Context.Response; }
        }

        /// <summary>
        /// 获取提供用于处理Web请求的Server对象
        /// </summary>
        public HttpServerUtility Server
        {
            get { return Context == null ? null : Context.Server; }
        }

        /// <summary>
        /// 获取当前Http请求的Session对象
        /// </summary>
        public HttpSessionState Session
        {
            get { return Context == null ? null : Context.Session; }
        }

        /// <summary>
        /// 为当前HTTP请求获取或设置安全信息
        /// </summary>
        public IPrincipal User
        {
            get { return Context == null ? null : Context.User; }
            set { if (Context != null) Context.User = value; }
        }

        /// <summary>
        /// 获取当前Http请求所使用的方法
        /// </summary>
        public HttpVerb HttpMethod
        {
            get { return Request == null ? HttpVerb.NULL : Request.HttpMethod.ToHttpVerb(); }
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

        /// <summary>
        /// 重写此方法以实现对Http请求的处理和响应
        /// </summary>
        protected abstract void OnProcessRequest();
    }
}

using System.Collections.Specialized;
using System.Json;
using System.Security.Principal;
using System.Text;
using System.Web.Caching;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace System.Web
{
    /// <summary>
    /// 为支持Http路由的Mvc应用程序中的Controller提供基类
    /// </summary>
    public abstract class Controller : IController, IHttpHandler, IRequiresSessionState
    {
        HttpContextBase _HttpContext;
        ControllerContext _ControllerContext;
        FormsAuthenticationHelper _FormsAuthentication;

        #region IHttpHandler 成员
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            OnProcessRequest();
        }
        #endregion

        #region IController 成员
        /// <summary>
        /// 获取该控制器的名称
        /// <para>类型名</para>
        /// </summary>
        public virtual string Name
        {
            get { return this.GetType().Name; }
        }

        ControllerContext IController.ControllerContext 
        {
            get { return _ControllerContext; }
        }

        void IController.Initialize(RequestContext requestContext)
        {
            this._HttpContext = requestContext.HttpContext;
            this._ControllerContext = new ControllerContext(requestContext, this);
            ControllerInitializer.Default.InitializeController(this);
            this._ViewData = new ViewDataDictionary();
            this.Initialize(requestContext);
        }
        #endregion

        #region IActionExecutor 成员
        IActionMethodSelector IActionExecutor.ActionSelector { get; set; }

        event EventHandler _PreActionExecute;
        event EventHandler IActionExecutor.PreActionExecute
        {
            add { this._PreActionExecute += value; }
            remove { this._PreActionExecute -= value; }
        }

        event EventHandler<ActionEventArgs> ActionNotFound;
        event EventHandler<ActionEventArgs> IActionExecutor.ActionNotFound
        {
            add { this.ActionNotFound += value; }
            remove { this.ActionNotFound -= value; }
        }

        event EventHandler<ActionExecutionErrorEventArgs> ActionExecutionError;
        event EventHandler<ActionExecutionErrorEventArgs> IActionExecutor.ActionExecutionError
        {
            add { this.ActionExecutionError += value; }
            remove { this.ActionExecutionError -= value; }
        }

        /// <summary>
        /// 从当前控制器实例执行请求的操作方法
        /// </summary>
        void IActionExecutor.ExecuteAction(ActionMethodInfo action)
        {
            if (action == null)
            {
                if (ActionNotFound != null) ActionNotFound(this, new ActionEventArgs(Name, ControllerContext.ActionName));
                var op = OnActionNotFound();
                switch (op.ToDo)
                {
                    case DecisiveOperatingInstruction.Operation.Abort: return;
                    case DecisiveOperatingInstruction.Operation.ThrowException:
                        throw op.ExcpetionToThrow;
                    case DecisiveOperatingInstruction.Operation.NoOperation: break;
                }
            }
            else
            {
                try
                {
                    action.BaseMethod.Invoke(this, action.Parameters);
                }
                catch (Threading.ThreadAbortException)
                {
                    //此异常由HttpResponse.End()导致，无需处理
                }
                catch (Exception e)
                {
                    if (ActionExecutionError != null) ActionExecutionError(this, new ActionExecutionErrorEventArgs(Name, ControllerContext.ActionName, e));
                    var op = OnActionExecutionError(e);
                    switch (op.ToDo)
                    {
                        case DecisiveOperatingInstruction.Operation.Abort: return;
                        case DecisiveOperatingInstruction.Operation.ThrowException:
                            throw op.ExcpetionToThrow;
                        case DecisiveOperatingInstruction.Operation.NoOperation: break;
                    }
                    throw e;
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取有关处理当前Http路由请求的控制器上下文件信息
        /// </summary>
        protected internal ControllerContext ControllerContext
        {
            get { return this._ControllerContext; }
        }

        /// <summary>
        /// 从当前Http路由请求上下文中获取路由数据
        /// </summary>
        protected internal RouteData RouteData
        {
            get { return ControllerContext.RouteData; }
        }

        ViewDataDictionary _ViewData;
        /// <summary>
        /// 获取视图数据的集合
        /// <para>在呈现视图时，将会合并该集合到要呈现视图的数据集合中</para>
        /// </summary>
        protected internal ViewDataDictionary ViewData
        {
            get { return _ViewData; }
        }

        /// <summary>
        /// 为控制器提供Froms认证的原生支持
        /// </summary>
        protected internal FormsAuthenticationHelper FormsAuthentication
        {
            get
            {
                if (this._FormsAuthentication == null)
                    this._FormsAuthentication = new FormsAuthenticationHelper(Context);
                return this._FormsAuthentication;
            }
        }

        /// <summary>
        /// 使用Http路由请求的上下文信息初始化当前控制器的自定义设置
        /// </summary>
        /// <param name="requestContext"></param>
        protected internal virtual void Initialize(RequestContext requestContext) { }

        /// <summary>
        /// 控制器执行的主入口方法
        /// </summary>
        private void OnProcessRequest()
        {
            //调用预置方法
            if (_PreActionExecute != null)
                _PreActionExecute(this, new ActionEventArgs(Name, ControllerContext.ActionName));
            PreActionExecute();

            //获取操作方法
            ActionMethodInfo action = ((IActionExecutor)this).ActionSelector.GetActionMethod(ControllerContext);

            //执行操作方法
            ((IActionExecutor)this).ExecuteAction(action);
        }

        /// <summary>
        /// 在执行请求的操作方法之前要执行的操作
        /// </summary>
        protected virtual void PreActionExecute() { }

        /// <summary>
        /// 当请求的操作方法未找到时要执行的操作
        /// </summary>
        protected virtual DecisiveOperatingInstruction OnActionNotFound()
        {
            return DecisiveOperatingInstruction.ThrowException(
                new Exception(string.Format("控制器 \"{0}\" 中不存在名为 \"{1}\" 的 Action", Name, ControllerContext.ActionName)));
        }

        /// <summary>
        /// 在执行请求的操作方法出错时要执行的操作
        /// </summary>
        /// <param name="baseException"></param>
        /// <returns></returns>
        protected virtual DecisiveOperatingInstruction OnActionExecutionError(Exception baseException)
        {
            return DecisiveOperatingInstruction.ThrowException(
                new Exception(string.Format("在执行控制器 \"{0}\" 的 \"{1}\" Action 时遇到错误", Name, ControllerContext.ActionName), baseException));
        }

        #region 常用的ASP.NET对象
        /// <summary>
        /// 获取当前Http请求的上下文
        /// </summary>
        public HttpContextBase Context
        {
            get { return this._HttpContext; }
        }

        /// <summary>
        /// 获取当前Http请求的Application对象
        /// </summary>
        public HttpApplicationStateBase Application
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
        public HttpRequestBase Request
        {
            get { return Context == null ? null : Context.Request; }
        }

        /// <summary>
        /// 获取当前Http请求的Response对象
        /// </summary>
        public HttpResponseBase Response
        {
            get { return Context == null ? null : Context.Response; }
        }

        /// <summary>
        /// 获取提供用于处理Web请求的Server对象
        /// </summary>
        public HttpServerUtilityBase Server
        {
            get { return Context == null ? null : Context.Server; }
        }

        /// <summary>
        /// 获取当前Http请求的Session对象
        /// </summary>
        public HttpSessionStateBase Session
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
        /// 获取当前Http请求的类型
        /// </summary>
        public HttpVerb HttpMethod
        {
            get { return Request == null ? HttpVerb.None : Request.HttpMethod.ToHttpVerb(); }
        }
        #endregion

        #region 通用的响应
        /// <summary>
        /// 响应JS文本到客户端
        /// </summary>
        /// <param name="script"></param>
        protected internal void ResponseJavaScript(JavaScript script)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(script.ToString());
            Response.Flush();
        }

        /// <summary>
        /// 响应Json文本到客户端
        /// </summary>
        /// <param name="jsonResult"></param>
        protected internal void ResponseJson(IJson jsonResult)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(jsonResult.GetJsonString());
            Response.Flush();
        }

        /// <summary>
        /// 响应Jsonp文本到客户端
        /// </summary>
        /// <param name="callBack">客户端的js回调方法</param>
        /// <param name="jsonParam">json参数</param>
        protected internal void ResponseJsonp(string callBack, IJson jsonParam)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(string.Format("{0}({1});", callBack, jsonParam.GetJsonString()));
            Response.Flush();
        }
        #endregion

        #region 呈现视图的方法
        /// <summary>
        /// 呈现已经在视图集合中注册的默认视图(Action同名)
        /// </summary>
        protected internal void RenderView()
        {
            RenderView(this.ControllerContext.ActionName);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        protected internal void RenderView(string viewName)
        {
            RenderView(viewName, null);
        }

        /// <summary>
        /// 呈现已经在视图集合中注册的默认视图(Action同名)
        /// </summary>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        protected internal void RenderView(object viewModel)
        {
            RenderView(this.ControllerContext.ActionName, viewModel);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的型视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        protected internal void RenderView(string viewName, object viewModel)
        {
            RenderView(viewName, viewModel, true);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        /// <param name="preserveForm">是否将原始请求的QueryString和Form集合传给视图</param>
        protected internal void RenderView(string viewName, object viewModel, bool preserveForm)
        {
            var newPage = ViewManager.ResolveView(viewName, viewModel);
            newPage.ViewData.MergeFrom(ViewData);
            RenderView(newPage, preserveForm);
        }

        /// <summary>
        /// 呈现已经在视图集合中注册的默认视图(Action同名)
        /// <para>[强类型]</para>
        /// </summary>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        protected internal void RenderView<T>(T viewModel)
        {
            RenderView<T>(this.ControllerContext.ActionName, viewModel);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的视图
        /// <para>[强类型]</para>
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        protected internal void RenderView<T>(string viewName, T viewModel)
        {
            RenderView<T>(viewName, viewModel, true);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的视图
        /// <para>[强类型]</para>
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        /// <param name="preserveForm">是否将原始请求的QueryString和Form集合传给视图</param>
        protected internal void RenderView<T>(string viewName, T viewModel, bool preserveForm)
        {
            var newPage = ViewManager.ResolveView<T>(viewName, viewModel);
            newPage.ViewData.MergeFrom(ViewData);
            RenderView(newPage, preserveForm);
        }

        /// <summary>
        /// 呈现指定的视图实例
        /// </summary>
        /// <param name="view">要呈现的视图</param>
        /// <param name="preserveForm">是否将原始请求的QueryString和Form集合传给视图</param>
        protected internal void RenderView(ViewPage view, bool preserveForm)
        {
            //Context.RewritePath(view.VirtualPath);
            if (view is IRouteable)
                ((IRouteable)view).RequestContext = this.ControllerContext.RequestContext;
            view.ViewData["$_Controller"] = Name;
            view.ViewData["$_Action"] = this.ControllerContext.ActionName;
            //Server.Transfer(view, preserveForm);
            Server.Execute(view, null, preserveForm);
        }
        #endregion

        #region 特殊方法
        /// <summary>
        /// 获取路由参数集合中指定名称的参数的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected internal object GetRouteValue(string name)
        {
            return this.ControllerContext.RequestContext.GetRouteValue(name);
        }

        /// <summary>
        /// 将浏览器以HTTP POST的请求类型重定向到目标Url，并将formData数据附加到Request.Form集合中
        /// <para>不支持Ajax请求</para>
        /// </summary>
        /// <param name="targetUrl">目标url(支持"~/"的写法,支持url附带参数)</param>
        /// <param name="formData">要附加到Request.Form集合中的数据</param>
        /// <param name="preserveForm">是否保留原始的Request.QueryString和Request.Form集合</param>
        protected void PostRedirect(string targetUrl, NameValueCollection formData, bool preserveForm)
        {
            if (targetUrl == null)
                throw new ArgumentNullException("targetUrl");

            if (formData == null)
                formData = new NameValueCollection(0);

            StringBuilder target =
                new StringBuilder(targetUrl.Replace("~/", Request.ApplicationPath));

            if (preserveForm)
            {
                if (target.ToString().IndexOf('?') > -1)
                {
                    for (int i = 0; i < Request.QueryString.Count; i++)
                    {
                        target.AppendFormat("&{0}={1}", Request.QueryString.Keys[i], Request.QueryString[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < Request.QueryString.Count; i++)
                    {
                        target.AppendFormat(
                            "{0}{1}={2}", i == 0 ? "?" : "&", Request.QueryString.Keys[i], Request.QueryString[i]);
                    }
                }
            }

            StringBuilder html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine(" <title>PostRedirectPage</title>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendFormat(" <form id=\"form1\" action=\"{0}\" method=\"post\" {1}>\r\n", target.ToString());

            if (preserveForm)
            {
                for (int i = 0; i < Request.Form.Count; i++)
                {
                    html.AppendFormat(
                        "       <input id=\"{0}\" name=\"{0}\" value=\"{1}\" type=\"hidden\" />\r\n", Request.Form.Keys[i], Request.Form[i]);
                }
            }

            for (int i = 0; i < formData.Count; i++)
            {
                html.AppendFormat(
                    "       <input id=\"{0}\" name=\"{0}\" value=\"{1}\" type=\"hidden\" />\r\n", formData.Keys[i], formData[i]);
            }

            html.AppendLine("</form>");
            html.AppendLine("</body>");
            html.AppendLine("<script type=\"text/javascript\">");
            html.AppendLine("//<![CDATA[");
            html.AppendLine("   (function() {");
            html.AppendLine("       var form = document.getElementById(\"form1\");");
            html.AppendLine("       form.submit();");
            html.AppendLine("   })();");
            html.AppendLine("//]]>");
            html.AppendLine("</script>");
            html.AppendLine("</html>");

            Response.ContentType = "text/html; charset=utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(html.ToString());
            Response.End();
        }
        #endregion
    }
}

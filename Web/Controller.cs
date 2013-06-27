using System.Collections.Specialized;
using System.Json;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace System.Web
{
    /// <summary>
    /// 为支持Http路由的Mvc应用程序中的Controller提供基类
    /// </summary>
    public abstract class Controller : RouteableHttpHandler, IController, IRequiresSessionState
    {       
        Type _ThisType;
        /// <summary>
        /// 获取该控制器在运行时的类型实例
        /// </summary>
        internal Type ThisType
        {
            get
            {
                if (_ThisType == null)
                    _ThisType = this.GetType();
                return _ThisType;
            }
        }

        /// <summary>
        /// 获取当前请求执行的Action名称
        /// <para>默认值：default</para>
        /// </summary>
        public virtual string Action
        {
            get
            {
                string t = GetRouteValue("action") as string;
                return t ?? "default";
            }
        }

        ViewDataDictionary _ViewData;
        /// <summary>
        /// 获取视图数据的集合
        /// <para>在呈现视图时，将会合并该集合到要呈现视图的数据集合中</para>
        /// </summary>
        protected ViewDataDictionary ViewData
        {
            get { return _ViewData; }
        }

        /// <summary>
        /// 当请求执行的Action未找到时要执行的操作
        /// <para>默认抛出异常</para>
        /// </summary>
        protected virtual DecisiveOperatingInstruction OnActionNotFound()
        {
            return DecisiveOperatingInstruction.ThrowException(
                new Exception(string.Format("控制器 \"{0}\" 中没有名为 \"{1}\" 的 Action (Action是在控制器中定义的同名无参方法)", ThisType.Name, Action)));
        }

        /// <summary>
        /// 在验证当前请求的Action的某个标记规则失败后要执行的操作
        /// </summary>
        /// <param name="attr"></param>
        protected virtual DecisiveOperatingInstruction OnActionAttributeValidateFailed(ActionAttribute attr)
        {
            if (attr is HttpMethodAttribute)
            {
                var httpAttr = attr as HttpMethodAttribute;
                if (!string.IsNullOrEmpty(httpAttr.RedirectUrl))
                {
                    ResponseRedirect(httpAttr.RedirectUrl, true);
                    return DecisiveOperatingInstruction.Abort();
                }
                else
                {
                    return DecisiveOperatingInstruction.ThrowException(
                        new Exception(string.Format("方法 \"{0}\" 不接受类型为 \"{1}\" 的HTTP请求", Action, HttpMethod)));
                }
            }
            else if (attr is AuthenticationAttribute)
            {
                if (Request.IsAuthenticated)
                {
                    Response403();
                }
                else
                {
                    Response401();
                }
                return DecisiveOperatingInstruction.Abort();
            }
            return DecisiveOperatingInstruction.NoOperation();
        }

        /// <summary>
        /// 调用当前请求执行的Action
        /// </summary>
        void InvokeAction()
        {
            //指定Action方法应该具有的标志特性
            BindingFlags acionFlags =
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.InvokeMethod;
            //得到Action同名的无参方法的实例
            var action = ThisType.GetMethod(Action, acionFlags, null, Type.EmptyTypes, null);

            if (action == null)
            {
                var op = OnActionNotFound();
                switch (op.ToDo)
                { 
                    case DecisiveOperatingInstruction.Operation.Abort: return;
                    case DecisiveOperatingInstruction.Operation.ThrowException: 
                        throw op.ExcpetionToThrow;;
                    case DecisiveOperatingInstruction.Operation.NoOperation: break;
                }
            }
            else
            {
                var attrs = Attribute.GetCustomAttributes(action, typeof(ActionAttribute)) as ActionAttribute[];
                if (attrs != null)
                {
                    foreach (var attr in attrs)
                    {
                        if (!attr.Validate(this))
                        {
                            var op = OnActionAttributeValidateFailed(attr);
                            switch (op.ToDo)
                            {
                                case DecisiveOperatingInstruction.Operation.Abort: return;
                                case DecisiveOperatingInstruction.Operation.ThrowException:
                                    throw op.ExcpetionToThrow; ;
                                case DecisiveOperatingInstruction.Operation.NoOperation: break;
                            }
                        }
                    }
                }

                try
                {
                    action.Invoke(this, null);
                }
                catch (Threading.ThreadAbortException)
                {
                    //此异常由Response.End()导致，无需处理
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 控制器执行的主入口方法
        /// </summary>
        protected sealed override void OnProcessRequest()
        {
            //初始化视图数据集合
            _ViewData = new ViewDataDictionary();
            
            //调用预置方法
            BeforeProcessRequest();

            //执行当前请求的Action
            InvokeAction();
        }

        /// <summary>
        /// 控制器执行前的预置方法
        /// </summary>
        protected virtual void BeforeProcessRequest() { }

        #region 为Forms认证提供的方法
        /// <summary>
        /// 为用户创建Forms认证凭据
        /// <para>UserData以“name=value&amp;userdata=value1,value2&amp;roles=role1,role2”的格式存储</para>
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userData">用户数据（以“,”分隔）</param>
        /// <param name="roles">用户角色（以“,”分隔）</param>
        /// <param name="expiration">过期时间（分钟）</param>
        /// <param name="cookiePath">保存Forms认证凭据的Cookie的路径（以“/”开头）</param>
        public void CreateFormsAuthentication(
            string userName, string userData, string roles, double expiration, string cookiePath)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1, userName, DateTime.Now, DateTime.Now.AddMinutes(expiration), true, string.Format("name={0}&userdata={1}&roles={2}", userName, userData, roles));
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = string.IsNullOrEmpty(cookiePath) ? FormsAuthentication.FormsCookiePath : cookiePath;
            cookie.Expires = ticket.Expiration;
            Response.Cookies.Set(cookie);
            User = new GenericPrincipal(new FormsIdentity(ticket), roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// 为用户创建Forms认证凭据
        /// <para>UserData以“name=value&amp;userdata=value1,value2&amp;roles=role1,role2”的格式存储</para>
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userData">用户数据（以“,”分隔）</param>
        /// <param name="roles">用户角色（以“,”分隔）</param>
        /// <param name="expiration">过期时间（分钟）</param>
        public void CreateFormsAuthentication(
            string userName, string userData, string roles, double expiration)
        {
            CreateFormsAuthentication(
                userName, userData, roles, expiration, FormsAuthentication.FormsCookiePath);
        }

        /// <summary>
        /// 取消当前会话并删除用户的Forms认证凭据
        /// </summary>
        /// <param name="cookiePath">保存Forms认证凭据的Cookie的路径（以“/”开头）</param>
        public void SignOut(string cookiePath)
        {
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty); ;
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = string.IsNullOrEmpty(cookiePath) ? FormsAuthentication.FormsCookiePath : cookiePath;
            cookie.Expires = DateTime.Now.AddDays(-1D);
            Response.SetCookie(cookie);
            Session.Abandon();
            User = null;
        }

        /// <summary>
        /// 取消当前会话并删除用户的Forms认证凭据
        /// </summary>
        public void SignOut()
        {
            SignOut(FormsAuthentication.FormsCookiePath);
        }
        #endregion

        #region 通用的响应
        /// <summary>
        /// 响应JS脚本到客户端
        /// </summary>
        /// <param name="script"></param>
        public void ResponseJavaScript(JavaScript script)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(script.ToString());
            //Response.End();
            ResponseEnd();
        }

        /// <summary>
        /// 直接响应Json文本到客户端
        /// </summary>
        /// <param name="jsonResult"></param>
        public void ResponseJson(IJson jsonResult)
        {
            Response.ContentType = "text/plain; charset=utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(jsonResult.GetJsonString());
            //Response.End();
            ResponseEnd();
        }

        /// <summary>
        /// 发送301响应（永久性重定向）到客户端
        /// </summary>
        /// <param name="targetUrl">目标Url</param>
        public void Response301(string targetUrl)
        {
            Response.StatusCode = 301;
            Response.StatusDescription = "Moved Permanently";
            Response.AppendHeader("Location", targetUrl);
            //Response.End();
            ResponseEnd();
        }

        /// <summary>
        /// 发送401响应（请求未授权）到客户端
        /// </summary>
        public void Response401()
        {
            Response.StatusCode = 401;
            Response.StatusDescription = "Unauthorized";
            //Response.End();
            ResponseEnd();
        }

        /// <summary>
        /// 发送403响应（禁止访问）到客户端
        /// </summary>
        public void Response403()
        {
            Response.StatusCode = 403;
            Response.StatusDescription = "Forbidden";
            //Response.End();
            ResponseEnd();
        }

        /// <summary>
        /// 发送404响应（资源不存在）到客户端
        /// </summary>
        public void Response404()
        {
            Response.StatusCode = 404;
            Response.StatusDescription = "Not Found";
            //Response.End();
            ResponseEnd();
        }

        /// <summary>
        /// 使 ASP.NET 跳过 HTTP 执行管线链中的所有事件和筛选并直接执行 EndRequest 事件
        /// <para>不会引发“ThreadAbortException”，用于替代Response.End（）方法</para>
        /// </summary>
        public void ResponseEnd()
        {
            Context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 将客户端重定向到新的 URL 并指定该新 URL。
        /// <para>不会引发“ThreadAbortException”，用于替代Response.Redirect（string）方法</para>
        /// </summary>
        /// <param name="url">目标的位置。</param>
        public void ResponseRedirect(string url)
        {
            Response.Redirect(url, false);
            ResponseEnd();
        }

        /// <summary>
        /// 将客户端重定向到新的 URL 指定该新 URL 并指定当前页的执行是否终止。
        /// <para>不会引发“ThreadAbortException”，用于替代Response.Redirect（string，bool）方法</para>
        /// </summary>
        /// <param name="url">目标的位置。</param>
        /// <param name="endResponse">指示当前页的执行是否应终止。</param>
        public void ResponseRedirect(string url, bool endResponse)
        {
            Response.Redirect(url, false);
            if (endResponse) ResponseEnd();
        }
        #endregion

        #region 呈现视图的方法
        /// <summary>
        /// 呈现已经在视图集合中注册的默认视图(Action同名)
        /// </summary>
        public void RenderView()
        {
            RenderView(Action);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        public void RenderView(string viewName)
        {
            RenderView(viewName, null);
        }

        /// <summary>
        /// 呈现已经在视图集合中注册的默认视图(Action同名)
        /// </summary>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        public void RenderView(object viewModel)
        {
            RenderView(Action, viewModel);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的型视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        public void RenderView(string viewName, object viewModel)
        {
            RenderView(viewName, viewModel, true);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        /// <param name="preserveForm">是否将原始请求的QueryString和Form集合传给视图</param>
        public void RenderView(string viewName, object viewModel, bool preserveForm)
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
        public void RenderView<T>(T viewModel)
        {
            RenderView<T>(Action, viewModel);
        }

        /// <summary>
        /// 呈现已在视图集合中注册的指定名称的视图
        /// <para>[强类型]</para>
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="viewModel">要绑定的视图模型对象</param>
        public void RenderView<T>(string viewName, T viewModel)
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
        public void RenderView<T>(string viewName, T viewModel, bool preserveForm)
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
        public void RenderView(ViewPage view, bool preserveForm)
        {
            //Context.RewritePath(view.VirtualPath); --
            //if (view is IRouteable)
            //    ((IRouteable)view).RequestContext = ((IRouteable)this).RequestContext;
            //view.ViewData["$_Controller"] = ThisType.Name;
            //view.ViewData["$_Action"] = Action;
            //Server.Transfer(view, preserveForm);
            ExecuteView(view, preserveForm);
            ResponseEnd();
        }

        /// <summary>
        /// 执行指定的视图实例
        /// <para>执行完成后仍回到调用代码</para>
        /// </summary>
        /// <param name="view">要执行的视图</param>
        /// <param name="preserveForm">是否将原始请求的QueryString和Form集合传给视图</param>
        public void ExecuteView(ViewPage view, bool preserveForm)
        {
            //Context.RewritePath(view.VirtualPath);
            if (view is IRouteable)
                ((IRouteable)view).RequestContext = ((IRouteable)this).RequestContext;
            view.ViewData["$_Controller"] = ThisType.Name;
            view.ViewData["$_Action"] = Action;
            Server.Execute(view, null, preserveForm);
        }
        #endregion

        #region 特殊方法
        /// <summary>
        /// 重定向客户端到指定的Url，并将formData数据附加到Request.Form集合中
        /// <para>不适用于Ajax请求</para>
        /// </summary>
        /// <param name="targetUrl">目标url(支持"~/"的写法,支持url附带参数)</param>
        /// <param name="formData">要附加到Request.Form集合中的数据</param>
        /// <param name="preserveForm">是否保留原始的Request.QueryString和Request.Form集合</param>
        public void TransferRequest(string targetUrl, NameValueCollection formData, bool preserveForm)
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
            html.AppendLine(" <title>RedirectPage</title>");
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

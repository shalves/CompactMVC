using System.Collections.Specialized;
using System.Security.Principal;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;

namespace System.Web
{
    /// <summary>
    /// 为全局应用程序类提供基类
    /// </summary>
    public abstract class GlobalBase : HttpApplication
    {
        bool TryGetCurrentUser(out IPrincipal user)
        {
            user = null;
            HttpCookie hc = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (hc == null || string.IsNullOrEmpty(hc.Value)) return false;
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(hc.Value);
                FormsIdentity formsID = new FormsIdentity(ticket);
                NameValueCollection userData = HttpUtility.ParseQueryString(ticket.UserData);
                string[] userRoles = userData["roles"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                user = new GenericPrincipal(formsID, userRoles);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 注册路由规则
        /// </summary>
        /// <param name="routes"></param>
        protected abstract void RegisterRoutes(RouteCollection routes);

        /// <summary>
        /// 注册视图
        /// </summary>
        /// <param name="views"></param>
        protected abstract void RegisterViews(NameValueCollection views);

        /// <summary>
        /// 在应用程序启动时执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            RegisterViews(ViewManager.Views);
        }

        /// <summary>
        /// 在新会话启动时执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 在接收到新的请求时执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 在请求结束时执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        { 
        }

        /// <summary>
        /// 在接收到新的验证用户的请求时执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            IPrincipal user;
            if (TryGetCurrentUser(out user))
            {
                Context.User = user;
            }
        }

        /// <summary>
        /// 在向客户端发送内容之前执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_PreSendRequestContent(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 在出现未处理的错误时执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 在会话结束时执行的代码
        /// <para>注意: 只有在 Web.config 文件中的 sessionstate 模式设置为InProc,并且显式调用Session.Abandon方法时,
        /// 才会引发 Session_End 事件。如果会话模式设置为 StateServer 或 SQLServer,
        /// 则不会引发该事件。
        /// </para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 在应用程序关闭时执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_End(object sender, EventArgs e)
        {
        }
    }
}

using System.Security.Principal;

namespace System.Web.Security
{
    public class FormsAuthenticationHelper
    {
        readonly HttpContextBase _Context;
        string _RedirectUrl;

        protected HttpSessionStateBase Session 
        {
            get { return _Context == null ? null : _Context.Session; }
        }

        protected HttpRequestBase Request 
        {
            get { return _Context == null ? null : _Context.Request; }
        }

        protected HttpResponseBase Response
        {
            get { return _Context == null ? null : _Context.Response; }
        }

        public FormsIdentity UserIdentity { get; private set; }

        public bool IsAuthenticated 
        {
            get { return UserIdentity != null; }
        }

        #region FormsAuthentication 的静态属性
        public string DefaultUrl
        {
            get { return FormsAuthentication.DefaultUrl; }
        }

        public string LoginUrl
        {
            get { return FormsAuthentication.LoginUrl; }
        }

        public string FormsCookieName
        {
            get { return FormsAuthentication.FormsCookieName; }
        }

        public string FormsCookiePath
        {
            get { return FormsAuthentication.FormsCookiePath; }
        }
        #endregion

        /// <summary>
        /// 获取导致重定向到当前页的Url
        /// <para>优先级：?ReutrnUrl > UrlReferrer > DefaultUrl</para>
        /// </summary>
        public string RedirectUrl
        {
            get
            {
                if (_RedirectUrl == null)
                {
                    _RedirectUrl = Request.Params["ReturnUrl"];
                    if (string.IsNullOrEmpty(_RedirectUrl))
                    {
                        if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath != Request.Url.AbsolutePath)
                        {
                            _RedirectUrl = Request.UrlReferrer.ToString();
                        }
                        else
                        {
                            _RedirectUrl = DefaultUrl;
                        }
                    }
                }
                return _RedirectUrl;
            }
        }

        /// <summary>
        /// 使用当前Http请求的上下文信息初始化 FormsAuthenticationHelper 类的新实例
        /// </summary>
        /// <param name="context"></param>
        public FormsAuthenticationHelper(HttpContextBase context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            _Context = context;
            UserIdentity = context.User == null ? null : context.User.Identity as FormsIdentity;
        }

        /// <summary>
        /// 将浏览器重定向到登录Url
        /// </summary>
        public void RedirectToLoginUrl()
        {
            FormsAuthentication.RedirectToLoginPage();
        }

        /// <summary>
        /// 将浏览器重定向到登录Url
        /// </summary>
        /// <param name="extraQueryString">要包含在目标Url中的查询字符串</param>
        public void RedirectToLoginUrl(string extraQueryString)
        {
            FormsAuthentication.RedirectToLoginPage(extraQueryString);
        }

        /// <summary>
        /// 将浏览器重定向回导致重定向到当前页的Url
        /// <para>仅执行重定向操作</para>
        /// </summary>
        public void RedirectToRedirectUrl()
        {
            RedirectToRedirectUrl(null);
        }

        /// <summary>
        /// 将浏览器重定向回导致重定向到当前页的Url
        /// <para>仅执行重定向操作</para>
        /// </summary>
        public void RedirectToRedirectUrl(string extraQueryString)
        {
            string url = RedirectUrl;
            if (!string.IsNullOrEmpty(extraQueryString))
                url = string.Format("{0}{1}{2}", RedirectUrl, RedirectUrl.IndexOf('?') > -1 ? "&" : "?", extraQueryString);
            Response.Redirect(url);
        }

        /// <summary>
        /// 为用户创建Forms认证凭据
        /// <para>UserData以“name=value&amp;userdata=value1,value2&amp;roles=role1,role2”的格式存储</para>
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userData">用户数据（以“,”分隔）</param>
        /// <param name="roles">用户角色（以“,”分隔）</param>
        /// <param name="expiration">过期时间（分钟）</param>
        /// <param name="cookiePath">保存Forms认证凭据的Cookie的路径（以“/”开头）</param>
        public void Certificate(
            string userName, string userData, string roles, double expiration, string cookiePath)
        {
            if (!IsAuthenticated)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, userName, DateTime.Now, DateTime.Now.AddMinutes(expiration), true, string.Format("name={0}&userdata={1}&roles={2}", userName, userData, roles));
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                cookie.Domain = FormsAuthentication.CookieDomain;
                cookie.Path = string.IsNullOrEmpty(cookiePath) ? FormsAuthentication.FormsCookiePath : cookiePath;
                cookie.Expires = ticket.Expiration;
                Response.Cookies.Set(cookie);
                UserIdentity = new FormsIdentity(ticket);
                _Context.User = new GenericPrincipal(UserIdentity, roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// 为用户创建Forms认证凭据
        /// <para>UserData以“name=value&amp;userdata=value1,value2&amp;roles=role1,role2”的格式存储</para>
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userData">用户数据（以“,”分隔）</param>
        /// <param name="roles">用户角色（以“,”分隔）</param>
        /// <param name="expiration">过期时间（分钟）</param>
        public void Certificate(
            string userName, string userData, string roles, double expiration)
        {
            Certificate(userName, userData, roles, expiration, FormsAuthentication.FormsCookiePath);
        }

        /// <summary>
        /// 取消当前会话并删除用户的Forms认证凭据
        /// </summary>
        /// <param name="cookiePath">保存Forms认证凭据的Cookie的路径（以“/”开头）</param>
        public void SignOut(string cookiePath)
        {
            if (IsAuthenticated)
            {
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty); ;
                cookie.Domain = FormsAuthentication.CookieDomain;
                cookie.Path = string.IsNullOrEmpty(cookiePath) ? FormsAuthentication.FormsCookiePath : cookiePath;
                cookie.Expires = DateTime.Now.AddDays(-1D);
                Response.SetCookie(cookie);
                if (Session != null) Session.Abandon();
                UserIdentity = null;
                _Context.User = null;
            }
        }

        /// <summary>
        /// 取消当前会话并删除用户的Forms认证凭据
        /// </summary>
        public void SignOut()
        {
            SignOut(FormsAuthentication.FormsCookiePath);
        }
    }
}

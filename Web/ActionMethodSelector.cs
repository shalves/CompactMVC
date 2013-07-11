using System.Reflection;

namespace System.Web
{
    /// <summary>
    /// 基于无参和HttpMethod后缀原则实现操作方法的选择器
    /// </summary>
    public class ActionMethodSelector : IActionMethodSelector
    {
        readonly BindingFlags _ActionFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase;

        public virtual BindingFlags ActionFlags
        {
            get { return _ActionFlags; }
        }

        public virtual Binder ActionBinder
        {
            get { return Type.DefaultBinder; }
        }

        event EventHandler<ActionAttributeValidateFailedEventArgs> _ActionAttributeValidateFailed;
        event EventHandler<ActionAttributeValidateFailedEventArgs> IActionMethodSelector.ActionAttributeValidateFailed
        {
            add { this._ActionAttributeValidateFailed += value; }
            remove { this._ActionAttributeValidateFailed -= value; }
        }

        /// <summary>
        /// 初始化 ActionMethodSelector 类的新实例
        /// </summary>
        public ActionMethodSelector() 
        {
            this._ActionAttributeValidateFailed = 
                new EventHandler<ActionAttributeValidateFailedEventArgs>(OnActionAttributeValidateFailed);
        }

        /// <summary>
        /// 在请求的操作方法的Action标记验证失败时要执行的操作
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual void OnActionAttributeValidateFailed(
            object controller, ActionAttributeValidateFailedEventArgs e)
        {
            var Response = ((IController)controller).ControllerContext.HttpContext.Response;
            var Request = ((IController)controller).ControllerContext.HttpContext.Request;

            if (e.InvalidAttribute is HttpMethodAttribute)
            {
                var httpAttr = e.InvalidAttribute as HttpMethodAttribute;
                if (!string.IsNullOrEmpty(httpAttr.RedirectUrl))
                {
                    string url = 
                        httpAttr.ReserveQueryString ? httpAttr.RedirectUrl + Request.Url.Query : httpAttr.RedirectUrl;
                    Response.Redirect(url);
                }
                else
                {
                    Response.Send405(httpAttr.Allow);
                }
            }
            else if (e.InvalidAttribute is AuthenticationAttribute)
            {
                if (Request.IsAuthenticated)
                {
                    Response.Send403();
                }
                else
                {
                    Response.Send401();
                }
            }
            else if (e.InvalidAttribute is SecureConnectionAttribute)
            {
                throw new HttpException(
                    string.Format("只有在使用安全的HTTP连接时，才可以请求控制器 \"{0}\" 的 \"{1}\" Action", e.ControllerName, e.ActionName));
            }
        }

        /// <summary>
        /// 依据当前Http路由请求和处理请求的控制器信息获取操作方法信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual ActionMethodInfo GetActionMethod(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            else
            {
                var newSelector = new NonParameterActionMethodSelector(context);
                var action = newSelector.GetActionMethod(ActionFlags);
                if (action != null && action.Attributes != null)
                {
                    foreach (var attr in action.Attributes)
                    {
                        if (!attr.Validate(context.HttpContext))
                        {
                            var eventArgs =
                                new ActionAttributeValidateFailedEventArgs(context.Controller.Name, context.ActionName, attr);
                            this._ActionAttributeValidateFailed(context.Controller, eventArgs);
                            break;
                        }
                    }
                }
                return action;
            }
        }

        private class NonParameterActionMethodSelector
        {
            public ControllerContext Context { get; private set; }
            public Type ControllerType { get; private set; }
            public string ActionName { get; private set; }
            public string HttpMethod { get; private set; }

            public NonParameterActionMethodSelector(ControllerContext context)
            {
                this.Context = context;
                this.ControllerType = this.Context.Controller.GetType();
                this.ActionName = this.Context.ActionName;
                this.HttpMethod = this.Context.HttpContext.Request.HttpMethod;
            }

            public ActionMethodInfo GetActionMethod(BindingFlags actionFlags)
            {
                MethodInfo actionMethod;
                actionMethod = ControllerType.GetMethod(ActionName, actionFlags, null, Type.EmptyTypes, null);
                if (actionMethod == null)
                {
                    actionMethod = ControllerType.GetMethod(
                         string.Format("{0}{1}", ActionName, HttpMethod), actionFlags, null, Type.EmptyTypes, null);
                }
                if (actionMethod == null) return null;
                return new ActionMethodInfo(actionMethod, ActionName);
            }
        }
    }
}

using System.Reflection;
using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 为实现控制器在应用程序级别的全局初始化而定义的类型
    /// </summary>
    public class ControllerInitializer
    {
        static ControllerInitializer _Default;
        internal static ControllerInitializer Default
        {
            get
            {
                if (_Default == null)
                    _Default = new ControllerInitializer();
                return _Default;
            }
        }

        static IActionMethodSelector _ActionSelector;
        internal static IActionMethodSelector ActionSelector
        {
            get { return _ActionSelector; }
        }

        /// <summary>
        /// 设置默认的控制器初始化器
        /// </summary>
        /// <param name="initializer"></param>
        public static void SetDefaultInitializer(ControllerInitializer initializer)
        {
            _Default = initializer;
        }

        /// <summary>
        /// 设置默认的操作方法选择器
        /// </summary>
        /// <param name="selector"></param>
        public static void SetActionSelector(IActionMethodSelector selector)
        {
            _ActionSelector = selector;
        }

        protected internal ControllerInitializer() { }

        /// <summary>
        /// 用于检测是子类否重写了基类的某个方法
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        bool HasMethod(Type controllerType, string methodName)
        {
            var bindFlags = BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.InvokeMethod;
            return controllerType.GetMethod(methodName, bindFlags) != null;
        }

        /// <summary>
        /// 初始化指定的控制器实例
        /// </summary>
        /// <param name="controller">指定要初始化的控制器</param>
        protected internal virtual void InitializeController(IController controller)
        {
            if (ActionSelector == null)
            {
                SetActionSelector(new ActionMethodSelector());
                ActionSelector.ActionAttributeValidateFailed += 
                    new EventHandler<ActionAttributeValidateFailedEventArgs>(OnActionAttributeValidateFailed);
            }
            controller.ActionSelector = ActionSelector;
            controller.PreActionExecute += new EventHandler(PreActionExecute);
            controller.ActionNotFound += new EventHandler<ActionEventArgs>(OnActionNotFound);
            controller.ActionExecutionError += new EventHandler<ActionExecutionErrorEventArgs>(OnActionExecutionError);
        }

        /// <summary>
        /// 在执行请求的操作方法之前要执行的操作
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="e"></param>
        protected virtual void PreActionExecute(object controller, EventArgs e) { }

        /// 当获取的操作方法的Action标记验证失败时要执行的操作
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual void OnActionAttributeValidateFailed(object controller, ActionAttributeValidateFailedEventArgs e) { }

        /// <summary>
        /// 当请求的操作方法未找到时要执行的操作
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="e"></param>
        protected virtual void OnActionNotFound(object controller, ActionEventArgs e) { }

        /// <summary>
        /// 在执行请求的操作方法出错时要执行的操作
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="e"></param>
        protected virtual void OnActionExecutionError(object controller, ActionExecutionErrorEventArgs e) { }
    }
}

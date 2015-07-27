using System.Reflection;

namespace System.Web
{
    /// <summary>
    /// 为自定义操作方法的选择器提供接口支持
    /// </summary>
    public interface IActionMethodSelector
    {
        /// <summary>
        /// 获取操作方法应具有的标志
        /// </summary>
        BindingFlags ActionFlags { get; }

        /// <summary>
        /// 获取或指定操作方法的绑定设置
        /// </summary>
        Binder ActionBinder { get; }

        /// <summary>
        /// 当获取的操作方法的Action标记验证失败时引发的事件
        /// </summary>
        event EventHandler<ActionAttributeValidateFailedEventArgs> ActionAttributeValidateFailed;

        /// <summary>
        /// 从Http路由请求上下文信息的控制器实例中获取操作方法
        /// </summary>
        ActionMethodInfo GetActionMethod(ControllerContext context);
    }
}

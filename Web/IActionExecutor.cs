namespace System.Web
{
    /// <summary>
    /// 为自定义操作方法的执行器提供接口支持
    /// </summary>
    public interface IActionExecutor
    {
        /// <summary>
        /// 获取处理当前Http路由请求的控制器实例
        /// </summary>
        IController Controller { get; }

        /// <summary>
        /// 获取或设置操作方法的选择器
        /// </summary>
        IActionMethodSelector ActionSelector { get; set; }

        /// <summary>
        /// 执行指定的操作方法以完成对当前请求的处理
        /// </summary>
        /// <param name="action"></param>
        void ExecuteAction(ActionMethodInfo action);

        /// <summary>
        /// 在当前请求的操作方法被执行前引发的事件
        /// </summary>
        event EventHandler PreActionExecute;

        /// <summary>
        /// 在当前请求的操作方法未找时引发的事件
        /// </summary>
        event EventHandler<ActionEventArgs> ActionNotFound;

        /// <summary>
        /// 在当前请求的操作方法执行出错时引发的事件
        /// </summary>
        event EventHandler<ActionExecutionErrorEventArgs> ActionExecutionError;
    }
}

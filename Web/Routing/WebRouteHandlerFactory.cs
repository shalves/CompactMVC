namespace System.Web.Routing
{
    /// <summary>
    /// 用于创建Web路由处理程序的工厂类
    /// </summary>
    public sealed class WebRouteHandlerFactory : IRouteHandlerFactory
    {
        readonly string _HandlersDirectory;
        /// <summary>
        /// 获取Web路由处理程序文件所在的目录的虚拟路径
        /// </summary>
        public string HandlersDirectory
        {
            get { return _HandlersDirectory; }
        }

        readonly string _HandlersExtension;
        /// <summary>
        /// 获取Web路由处理程序文件的扩展名
        /// </summary>
        public string HandlerFilesExtension
        {
            get { return _HandlersExtension; }
        }

        /// <summary>
        /// 初始化WebRouteHandlerFactory的新实例
        /// </summary>
        /// <param name="handlersDirectory">必须是以~/或/开头的绝对路径</param>
        public WebRouteHandlerFactory(string handlersDirectory)
        {
            this._HandlersDirectory = handlersDirectory;
        }

        /// <summary>
        /// 初始化WebRouteHandlerFactory的新实例
        /// </summary>
        /// <param name="handlersDirectory">指定Web路由处理程序文件所在的目录的虚拟路径<para>必须是以~/或/开头的绝对路径</para></param>
        /// <param name="handlersExtension">指定Web路由处理程序文件的扩展名</param>
        public WebRouteHandlerFactory(string handlersDirectory, string handlersExtension)
        {
            this._HandlersDirectory = handlersDirectory;
            this._HandlersExtension = handlersExtension;
        }

        /// <summary>
        /// 根据文件名创建Web路由处理程序的新实例
        /// </summary>
        /// <param name="handlerToken">指定Web路由处理程序的文件名</param>
        /// <returns></returns>
        public IRouteHandler CreateRouteHandler(string handlerToken)
        {
            string postfix = string.IsNullOrEmpty(HandlerFilesExtension) ? "ashx" : HandlerFilesExtension;

            if (string.IsNullOrEmpty(HandlersDirectory))
            {
                return new WebRouteHandler(string.Format("~/{0}Controller.{1}", handlerToken, postfix));
            }
            else
            {
                return new WebRouteHandler(
                    string.Format("{0}/{1}Controller.{2}", HandlersDirectory.TrimEnd('/'), handlerToken, postfix));
            }
        }
    }
}

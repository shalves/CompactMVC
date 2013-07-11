namespace System.Web.Routing
{
    /// <summary>
    /// 用于创建Web路由处理程序的工厂类
    /// </summary>
    public sealed class WebRouteHandlerFactory : RouteHandlerFactory
    {
        readonly string _HandlerDirectory;
        readonly string _HandlerPostfix;

        /// <summary>
        /// 获取Http路由请求处理程序文件所在目录的虚拟路径
        /// </summary>
        public string HandlerDirectory
        {
            get { return _HandlerDirectory; }
        }

        /// <summary>
        /// 获取Http路由请求处理程序文件名的后缀部分
        /// </summary>
        public string HandlerPostfix
        {
            get { return _HandlerPostfix; }
        }

        /// <summary>
        /// 初始化 WebRouteHandlerFactory 类的新实例
        /// </summary>
        /// <param name="handlerDirectory">
        /// 指定路由请求处理程序文件所在目录的虚拟路径
        /// <para>必须是以~/或/开头的绝对路径</para>
        /// </param>
        public WebRouteHandlerFactory(string handlerDirectory)
        {
            this._HandlerDirectory = handlerDirectory;
        }

        /// <summary>
        /// 初始化 WebRouteHandlerFactory 类的新实例
        /// </summary>
        /// <param name="handlersDirectory">
        /// 指定Web路由处理程序文件所在的目录的虚拟路径
        /// <para>必须是以~/或/开头的绝对路径</para>
        /// </param>
        /// <param name="handlerPostfix">指定路由请求处理程序文件名的后缀部分</param>
        public WebRouteHandlerFactory(string handlersDirectory, string handlerPostfix)
        {
            this._HandlerDirectory = handlersDirectory;
            this._HandlerPostfix = handlerPostfix;
        }

        /// <summary>
        /// 创建Http路由请求处理程序的新实例
        /// </summary>
        /// <param name="handlerToken">指定Http路由请求处理程序的特征名</param>
        /// <returns></returns>
        public override IRouteHandler CreateRouteHandler(string handlerToken)
        {
            if (string.IsNullOrEmpty(HandlerDirectory))
            {
                return new WebRouteHandler(string.Format("~/{0}{1}.ashx", handlerToken, HandlerPostfix));
            }
            else
            {
                return new WebRouteHandler(
                    string.Format("{0}/{1}{2}.ashx", HandlerDirectory.TrimEnd('/'), handlerToken, HandlerPostfix));
            }
        }
    }
}

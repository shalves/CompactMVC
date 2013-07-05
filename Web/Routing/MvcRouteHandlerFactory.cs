namespace System.Web.Routing
{
    /// <summary>
    /// 用于创建Mvc路由请求处理程序的工厂类
    /// </summary>
    public class MvcRouteHandlerFactory : IRouteHandlerFactory
    {
        readonly string _HandlerAssemblyName;
        readonly string _HandlersNamespace;

        /// <summary>
        /// 获取路由请求处理程序所在程序集的名称
        /// </summary>
        public string HandlerAssemblyName 
        {
            get { return _HandlerAssemblyName; }
        }

        /// <summary>
        /// 获取路由请求处理程序的统一名称空间
        /// </summary>
        public string HandlersNamespace 
        {
            get { return _HandlersNamespace; }
        }

        /// <summary>
        /// 初始化MvcRouteHandlerFactory的新实例
        /// </summary>
        /// <param name="handlerAssemblyName">指定路由请求处理程序所在程序集的名称</param>
        public MvcRouteHandlerFactory(string handlerAssemblyName)
        {
            this._HandlerAssemblyName = handlerAssemblyName;
        }

        /// <summary>
        /// 初始化MvcRouteHandlerFactory的新实例
        /// </summary>
        /// <param name="handlerAssemblyName">指定路由请求处理程序所在程序集的名称</param>
        /// <param name="handlerNamespace">指定路由请求处理程序的统一名称空间</param>
        public MvcRouteHandlerFactory(string handlerAssemblyName, string handlerNamespace)
        {
            this._HandlerAssemblyName = handlerAssemblyName;
            this._HandlersNamespace = handlerNamespace;
        }

        /// <summary>
        /// 创建路由请求处理程序的新实例
        /// </summary>
        /// <param name="handlerToken">路由请求处理程序的特征名</param>
        /// <returns></returns>
        public IRouteHandler CreateRouteHandler(string handlerToken)
        {
            string prefix = 
                string.IsNullOrEmpty(HandlersNamespace) ? HandlerAssemblyName : HandlersNamespace;
            return new MvcRouteHandler(HandlerAssemblyName, string.Format("{0}.{1}Controller", prefix, handlerToken));
        }
    }
}

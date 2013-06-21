namespace System.Web.Routing
{
    /// <summary>
    /// 用于创建路由处理程序的工厂类的约定
    /// </summary>
    public interface IRouteHandlerFactory
    {
        IRouteHandler CreateRouteHandler(string handlerToken);
    }
}

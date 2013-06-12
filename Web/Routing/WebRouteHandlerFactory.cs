namespace System.Web.Routing
{
    public sealed class WebRouteHandlerFactory
    {
        public string HandlersDirectory
        {
            get;
            private set;
        }

        public WebRouteHandlerFactory(string handlersDirectory)
        {
            this.HandlersDirectory = handlersDirectory;
        }

        public IRouteHandler CreateRouteHandler(string handlerName)
        {
            if (string.IsNullOrEmpty(HandlersDirectory))
            {
                return new WebRouteHandler(string.Format("~/{0}", handlerName));
            }
            else
            {
                return new WebRouteHandler(
                    string.Format("{0}/{1}", HandlersDirectory.TrimEnd('/'), handlerName));
            }
        }
    }
}

using System.Extensions;

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
            if (HandlersDirectory.IsNullOrEmpty())
            {
                return new WebRouteHandler("~/{0}".FormatWith(handlerName));
            }
            else
            {
                return new WebRouteHandler("{0}/{1}".FormatWith(HandlersDirectory.TrimEnd('/'), handlerName));
            }
        }
    }
}

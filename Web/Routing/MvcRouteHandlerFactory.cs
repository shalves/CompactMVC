using System.Extensions;

namespace System.Web.Routing
{
    public sealed class MvcRouteHandlerFactory
    {
        public string AssemblyName
        {
            get;
            private set;
        }

        public string TypeNamePrefix
        {
            get;
            private set;
        }

        public MvcRouteHandlerFactory(string assemblyName, string typeNamePrefix)
        {
            this.AssemblyName = assemblyName;
            this.TypeNamePrefix = typeNamePrefix;
        }

        public IRouteHandler CreateRouteHandler(string typeName)
        {
            if (TypeNamePrefix.IsNullOrEmpty())
            {
                return new MvcRouteHandler(AssemblyName, typeName);
            }
            else
            {
                return new MvcRouteHandler(AssemblyName, "{0}.{1}".FormatWith(TypeNamePrefix, typeName));
            }
        }
    }
}

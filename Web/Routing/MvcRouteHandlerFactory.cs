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
            if (string.IsNullOrEmpty(TypeNamePrefix))
            {
                return new MvcRouteHandler(AssemblyName, typeName);
            }
            else
            {
                return new MvcRouteHandler(AssemblyName, string.Format("{0}.{1}", TypeNamePrefix, typeName));
            }
        }
    }
}

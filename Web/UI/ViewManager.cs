using System.Collections.Specialized;

namespace System.Web.UI
{
    /// <summary>
    /// 为视图提供统一的管理
    /// </summary>
    public static class ViewManager
    {
        static string _ViewDirectoryPath = "~/Views";

        /// <summary>
        /// 获取或设置视图文件夹在应用程序中的虚拟路径
        /// <para>默认值：~/Views</para>
        /// </summary>
        public static string ViewDirectoryPath
        {
            get { return _ViewDirectoryPath; }
            set { _ViewDirectoryPath = value; }
        }

        /// <summary>
        /// 映射新的视图到视图映射表
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewPath"></param>
        public static void MapView(string viewName, string viewPath)
        {
            ViewMappingStore.ViewMapping.Add(viewName, viewPath);
        }

        /// <summary>
        /// 映射新的视图到视图映射表
        /// </summary>
        /// <param name="viewMapping"></param>
        public static void MapView(NameValueCollection viewMapping)
        {
            ViewMappingStore.ViewMapping.Add(viewMapping);
        }

        public static string GetViewPathByViewName(string viewName)
        {
            return ViewMappingStore.ViewMapping[viewName];
        }

        public static string GetViewPathByConvention(string controllerToken, string actionName)
        {
            return string.Format("{0}/{1}/{2}.aspx", ViewDirectoryPath, controllerToken, actionName);
        }

        public static ViewPage ResolveView(string viewName, string controllerToken, string actionName, object viewModel)
        {
            string viewPath = null;
            if (!string.IsNullOrEmpty(viewName))
                viewPath = GetViewPathByViewName(viewName);

            if (string.IsNullOrEmpty(viewPath))
                viewPath = GetViewPathByConvention(controllerToken, actionName);
            
            return ViewBuilder.CreateView(viewPath, viewModel);
        }

        public static ViewPage<T> ResolveView<T>(string viewName, string controllerToken, string actionName, T viewModel)
        {
            string viewPath = null;
            if (!string.IsNullOrEmpty(viewName))
                viewPath = GetViewPathByViewName(viewName);

            if (string.IsNullOrEmpty(viewPath))
                viewPath = GetViewPathByConvention(controllerToken, actionName);

            return ViewBuilder.CreateView<T>(viewPath, viewModel);
        }
    }
}

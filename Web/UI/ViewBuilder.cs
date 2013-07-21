using System.Web.Compilation;

namespace System.Web.UI
{
    internal static class ViewBuilder
    {
        /// <summary>
        /// 从给定的虚拟路径创建视图实例，并为视图绑定视图模型
        /// </summary>
        /// <param name="viewPath"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ViewPage CreateView(string viewPath, object viewModel)
        {
            if (string.IsNullOrEmpty(viewPath)) throw new ArgumentNullException("viewPath");

            try
            {
                ViewPage newView = BuildManager.
                        CreateInstanceFromVirtualPath(viewPath, typeof(ViewPage)) as ViewPage;

                if (newView == null)
                    throw new Exception(string.Format("从路径 \"{0}\" 创建视图失败", viewPath));

                newView.SetVirtualPath(viewPath);
                newView.SetViewModel(viewModel);

                return newView;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 从给定的虚拟路径创建视图实例，并为视图绑定视图模型
        /// </summary>
        /// <param name="viewPath"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ViewPage<T> CreateView<T>(string viewPath, T viewModel)
        {
            if (string.IsNullOrEmpty(viewPath)) throw new ArgumentNullException("viewPath");

            try
            {
                ViewPage<T> newView = BuildManager.
                        CreateInstanceFromVirtualPath(viewPath, typeof(ViewPage<T>)) as ViewPage<T>;

                if (newView == null)
                    throw new Exception(string.Format("从路径 \"{0}\" 创建视图失败", viewPath));

                newView.SetVirtualPath(viewPath);
                newView.SetViewModel(viewModel);

                return newView;
            }
            catch
            {
                throw;
            }
        }
    }
}

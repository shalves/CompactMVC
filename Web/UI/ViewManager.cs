using System.Collections.Specialized;
using System.Extensions;
using System.Web.Compilation;

namespace System.Web.UI
{
    /// <summary>
    /// 为视图提供统一的管理
    /// </summary>
    public static class ViewManager
    {
        static NameValueCollection _ViewList;

        /// <summary>
        /// 视图名称与视图页面文件路径的集合
        /// </summary>
        public static NameValueCollection Views
        {
            get 
            {
                if (_ViewList == null)
                    _ViewList = new NameValueCollection();
                return _ViewList;
            }
        }

        /// <summary>
        /// 转换视图集合中指定名称的普通视图到实例
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static ViewPage ResolveGenericView(string viewName)
        {
            return ResolveView(viewName, null);
        }

        /// <summary>
        /// 转换视图集合中指定名称的普通视图到实例，并为视图绑定页面模型
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ViewPage ResolveView(string viewName, object viewModel)
        {
            string viewPath = Views[viewName];
            
            if (viewPath.IsNullOrEmpty())
                throw new Exception("视图 \"{0}\" 不存在".FormatWith(viewName));

            try
            {
                ViewPage newView = BuildManager.
                        CreateInstanceFromVirtualPath(viewPath, typeof(ViewPage)) as ViewPage;

                if (newView == null)
                    throw new Exception("创建视图 \"{0}\" 失败".FormatWith(viewName));

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
        /// 转换视图集合中指定名称的强类型视图到实例，并为视图绑定页面模型
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ViewPage<T> ResolveView<T>(string viewName, T viewModel)
        {
            string viewPath = Views[viewName];

            if (viewPath.IsNullOrEmpty())
                throw new Exception("视图 \"{0}\" 不存在".FormatWith(viewName));

            try
            {
                ViewPage<T> newView = BuildManager.
                        CreateInstanceFromVirtualPath(viewPath, typeof(ViewPage<T>)) as ViewPage<T>;

                if (newView == null)
                    throw new Exception("创建视图 \"{0}\" 失败".FormatWith(viewName));

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

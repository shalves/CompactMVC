namespace System.Web.UI
{
    /// <summary>
    /// 表示包含有ViewData和ViewModel属性的WebForm强类型视图页面
    /// </summary>
    public class ViewPage<T> : ViewPage
    {
        T _ViewModel;

        /// <summary>
        /// 获取当前视图页的页面模型
        /// </summary>
        public new T ViewModel
        {
            get { return _ViewModel; }
        }

        internal void SetViewModel(T viewModel)
        {
            _ViewModel = viewModel;
        }
    }
}

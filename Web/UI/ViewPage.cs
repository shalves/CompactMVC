namespace System.Web.UI
{
    /// <summary>
    /// 表示包含有ViewData和ViewModel属性的WebForm视图页面
    /// </summary>
    [FileLevelControlBuilder(typeof(ViewPageControlBuilder))]
    public class ViewPage : Page
    {
        /// <summary>
        /// 获取或设置页的标题
        /// </summary>
        public new string Title
        {
            get { return ViewData["Page_Title"].ToString(); }
            set { ViewData["Page_Title"] = value; }
        }

        string _VirtualPath;

        /// <summary>
        /// 获取当前视图页面文件的虚拟路径
        /// </summary>
        public string VirtualPath
        {
            get { return _VirtualPath; }
        }

        ViewDataDictionary _ViewData;

        /// <summary>
        /// 获取当前视图页的视图数据
        /// </summary>
        public ViewDataDictionary ViewData
        {
            get { return _ViewData; }
        }

        object _ViewModel;

        /// <summary>
        /// 获取当前视图页的页面模型
        /// </summary>
        public object ViewModel
        {
            get { return _ViewModel; }
        }

        /// <summary>
        /// 初始化ViewPage类的实例
        /// </summary>
        public ViewPage()
            : base()
        {
            _ViewData = new ViewDataDictionary
            { 
                {"Page_Title", "New View Page"},
                {"Meta_ContentType", "text/html; charset=utf-8"},
                {"Meta_Keywords", ""},
                {"Meta_Description", ""},
                {"Page_FrontObject", "null"}
            };
        }

        internal virtual void SetVirtualPath(string value)
        {
            _VirtualPath = value;
        }

        internal virtual void SetViewModel(object viewModel)
        {
            _ViewModel = viewModel;
        }
    }
}

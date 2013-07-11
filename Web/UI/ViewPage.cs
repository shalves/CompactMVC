using System.Web.UI.WebControls;
using System.Web.SessionState;

namespace System.Web.UI
{
    /// <summary>
    /// 表示包含有ViewData和ViewModel属性的WebForm视图页面
    /// </summary>
    [FileLevelControlBuilder(typeof(ViewPageControlBuilder))]
    public class ViewPage : RouteablePage
    {
        string _Name;
        string _VirtualPath;
        ViewDataDictionary _ViewData;
        object _ViewModel;

        /// <summary>
        /// 获取或设置页的标题
        /// </summary>
        public new string Title
        {
            get { return ViewData["$_Page_Title"].ToString(); }
            set { ViewData["$_Page_Title"] = value; }
        }

        /// <summary>
        /// 获取或设置页面Head元素中Meta ContentType标记的内容
        /// </summary>
        public string MetaContentType
        {
            get { return ViewData["$_Meta_ContentType"].ToString(); }
            set { ViewData["$_Meta_ContentType"] = value; }
        }

        /// <summary>
        /// 获取或设置页面Head元素中Meta Keywords标记的内容
        /// </summary>
        public string MetaKeywords
        {
            get { return ViewData["$_Meta_Keywords"].ToString(); }
            set { ViewData["$_Meta_Keywords"] = value; }
        }

        /// <summary>
        /// 获取或设置页面Head元素中Meta Description标记的内容
        /// </summary>
        public string MetaDescription
        {
            get { return ViewData["$_Meta_Description"].ToString(); }
            set { ViewData["$_Meta_Description"] = value; }
        }

        /// <summary>
        /// 获取当前视图页面在映射表中的名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// 获取当前视图页面文件的虚拟路径
        /// </summary>
        public string VirtualPath
        {
            get { return _VirtualPath; }
        }

        /// <summary>
        /// 获取当前视图页的视图数据
        /// </summary>
        public ViewDataDictionary ViewData
        {
            get { return _ViewData; }
        }

        /// <summary>
        /// 获取当前视图页的页面模型
        /// </summary>
        public object ViewModel
        {
            get { return _ViewModel; }
        }

        /// <summary>
        /// 初始化 ViewPage 类的实例
        /// </summary>
        public ViewPage()
            : base()
        {
            this._ViewData = new ViewDataDictionary
            { 
                {"$_Page_Title", "New View Page"},
                {"$_Meta_ContentType", "text/html; charset=utf-8"},
                {"$_Meta_Keywords", ""},
                {"$_Meta_Description", ""},
                {"$_Controller", ""},
                {"$_Action", ""}
            };
        }

        internal void SetName(string viewName)
        {
            _Name = viewName;
        }

        internal void SetVirtualPath(string value)
        {
            _VirtualPath = value;
        }

        internal virtual void SetViewModel(object viewModel)
        {
            _ViewModel = viewModel;
        }
    }
}

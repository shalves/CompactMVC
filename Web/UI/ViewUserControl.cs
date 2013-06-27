using System.Collections.Specialized;

namespace System.Web.UI
{
    [FileLevelControlBuilder(typeof(ViewUserControlControlBuilder))]
    public class ViewUserControl : UserControl
    {
        public new ViewPage Page
        {
            get { return base.Page as ViewPage; }
        }

        ViewDataDictionary _ViewData;
        public ViewDataDictionary ViewData
        {
            get { return _ViewData; }
        }

        public ViewUserControl()
            : base()
        {
            _ViewData = new ViewDataDictionary();
        }
    }
}

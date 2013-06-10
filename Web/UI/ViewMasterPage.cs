namespace System.Web.UI
{
    public class ViewMasterPage : MasterPage
    {
        public new ViewPage Page
        {
            get { return base.Page as ViewPage; }
        }
    }
}

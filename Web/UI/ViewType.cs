using System.ComponentModel;

namespace System.Web.UI
{
    //从mvc2_ms_pl中提取
    [ControlBuilder(typeof(ViewTypeControlBuilder))]
    [NonVisualControl]
    public class ViewType : Control
    {
        private string _typeName;

        [DefaultValue("")]
        public string TypeName
        {
            get
            {
                return _typeName ?? String.Empty;
            }
            set
            {
                _typeName = value;
            }
        }
    }
}

using System.Collections.ObjectModel;
using System.Text;

namespace System.Web.UI
{
    public class JavaScriptCollection : Collection<JavaScript>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var js in Items)
            {
                sb.AppendLine(js.ToString());
            }
            return sb.ToString();
        }
    }
}

using System.Collections.ObjectModel;
using System.Text;

namespace System.Web.UI
{
    public class JavaScriptCollection : Collection<JavaScript>
    {
        public override string ToString()
        {
            if (Count == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Items.Count; i++)
            {
                sb.Append(Items[i].ToString());
                if (i + 1 < Count) sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}

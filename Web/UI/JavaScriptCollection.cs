using System.Collections.ObjectModel;
using System.Extensions;
using System.Text;

namespace System.Web.UI
{
    public class JavaScriptCollection : Collection<JavaScript>
    {
        public override string ToString()
        {
            if (Count == 0) return string.Empty;

            StringBuilder tmp = new StringBuilder();

            Items.Each<JavaScript>((int i, JavaScript js) =>
            {
                tmp.Append(js.ToString());
                if (i + 1 < Count) tmp.AppendLine();
            });

            return tmp.ToString();
        }
    }
}

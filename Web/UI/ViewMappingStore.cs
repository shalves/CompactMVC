using System.Collections.Specialized;

namespace System.Web.UI
{
    internal static class ViewMappingStore
    {
        readonly static NameValueCollection _ViewMapping;

        /// <summary>
        /// 视图名称与视图页面文件路径的集合
        /// </summary>
        public static NameValueCollection ViewMapping
        {
            get { return _ViewMapping; }
        }

        static ViewMappingStore()
        {
            _ViewMapping = new NameValueCollection();
        }
    }
}

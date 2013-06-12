using System.Collections.Specialized;
using System.Text;

namespace System.Json
{
    /// <summary>
    /// 表示一个JS对象
    /// </summary>
    public sealed class JSObject : NameObjectCollectionBase, IPropertyNameQuotable, IJson
    {
        #region IPropertyNameQuotable成员
        bool IPropertyNameQuotable.QuotePropertyName { get; set; }
        void IPropertyNameQuotable.ByAdd()
        {
            foreach (var t in this)
            {
                if (t is IPropertyNameQuotable)
                {
                    ((IPropertyNameQuotable)t).QuotePropertyName = QuotePropertyName;
                    ((IPropertyNameQuotable)t).ByAdd();
                }
            }
        }
        #endregion

        bool QuotePropertyName
        {
            get { return ((IPropertyNameQuotable)this).QuotePropertyName; }
        }

        /// <summary>
        /// 获取对象中指定序号的元素的值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        object this[int i]
        {
            get { return base.BaseGet(i); }
        }

        /// <summary>
        /// 创建JS对象的实例
        /// </summary>
        public JSObject() { }

        /// <summary>
        /// 创建JS对象的实例
        /// </summary>
        /// <param name="quotePropertyName">设置在输出时是否引用属性名</param>
        public JSObject(bool quotePropertyName)
        {
            ((IPropertyNameQuotable)this).QuotePropertyName = quotePropertyName;
        }

        /// <summary>
        /// 集合初始化器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, object value)
        {
            if (QuotePropertyName && value is IPropertyNameQuotable)
            {
                ((IPropertyNameQuotable)value).QuotePropertyName = QuotePropertyName;
                ((IPropertyNameQuotable)value).ByAdd();
            }
            base.BaseAdd(name, value);
        }

        /// <summary>
        /// 获取该JS对象的字符串形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (this as IJson).ToString();
        }

        #region IJson成员
        string IJson.ToString()
        {
            string paireFormat = QuotePropertyName ? "\"{0}\": {1}" : "{0} : {1}";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < Count; i++)
            {
                sb.AppendFormat(paireFormat, Keys[i], JSProperty.GetJSValue(this[i]));
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.Text;

namespace System.Json
{
    /// <summary>
    /// 表示一个JS对象数组
    /// </summary>
    public sealed class JSArray : List<object>, IPropertyNameQuotable, IJson
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
        /// 创建JS对象数组的实例
        /// </summary>
        public JSArray() { }

        /// <summary>
        /// 创建JS对象数组的实例
        /// </summary>
        /// <param name="quotePropertyName">设置在输出时是否引用属性名</param>
        public JSArray(bool quotePropertyName)
        {
            ((IPropertyNameQuotable)this).QuotePropertyName = quotePropertyName;
        }

        /// <summary>
        /// 重写基类的Add方法,以便继承QuotePropertyName
        /// </summary>
        /// <param name="value"></param>
        public new void Add(object value)
        {
            if (QuotePropertyName && value is IPropertyNameQuotable)
            {
                ((IPropertyNameQuotable)value).QuotePropertyName = QuotePropertyName;
                ((IPropertyNameQuotable)value).ByAdd();
            }
            base.Add(value);
        }

        /// <summary>
        /// 获取该JS对象数组的字符形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (this as IJson).ToString();
        }

        #region IJson 成员
        string IJson.ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (object obj in this)
            {
                sb.Append(JSProperty.GetJSValue(obj));
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}

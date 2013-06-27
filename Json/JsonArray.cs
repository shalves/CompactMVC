using System.Collections.Generic;
using System.Text;
using System;

namespace System.Json
{
    /// <summary>
    /// 表示一个Json数组对象
    /// </summary>
    public sealed class JsonArray : List<object>, IPropertyNameQuotable, IJson
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
        /// 创建Json数组对象的实例
        /// </summary>
        public JsonArray() { }

        /// <summary>
        /// 创建Json数组对象的实例
        /// </summary>
        /// <param name="quotePropertyName">设置在输出时是否引用属性名</param>
        public JsonArray(bool quotePropertyName)
        {
            ((IPropertyNameQuotable)this).QuotePropertyName = quotePropertyName;
        }

        /// <summary>
        /// 重写基类的Add方法，以便继承QuotePropertyName
        /// </summary>
        /// <param name="value"></param>
        public new void Add(object value)
        {
            if (QuotePropertyName && value is IPropertyNameQuotable)
            {
                ((IPropertyNameQuotable)value).QuotePropertyName = QuotePropertyName;
                ((IPropertyNameQuotable)value).ByAdd();
            }
            if (value != null && value.GetType().Namespace == null)
            {
                base.Add(new JsonObject(QuotePropertyName, value));
            }
            else
            {
                base.Add(value);
            }
        }

        /// <summary>
        /// 获取该Json数组对象的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ((IJson)this).GetJsonString();
        }

        #region IJson 成员
        string IJson.GetJsonString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (object obj in this)
            {
                sb.Append(", ");
                sb.Append(JsonProperty.ValueOf(obj));
            }
            if (Count > 0) sb.Remove(1, 2);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}

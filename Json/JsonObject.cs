using System.Collections.Specialized;
using System.Text;
using System.Extensions;

namespace System.Json
{
    /// <summary>
    /// 表示一个Json对象
    /// </summary>
    public sealed class JsonObject : NameObjectCollectionBase, IPropertyNameQuotable, IJson
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
        /// 获取对象中指定序号的属性的值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        object this[int i]
        {
            get { return base.BaseGet(i); }
        }

        /// <summary>
        /// 获取对象中指定名称的属性的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object this[string name]
        {
            get { return base.BaseGet(name); }
        }

        /// <summary>
        /// 创建JS对象的实例
        /// </summary>
        public JsonObject() { }

        /// <summary>
        /// 创建JS对象的实例
        /// </summary>
        /// <param name="anonymous">指定用于初始化JsonObject的匿名对象</param>
        public JsonObject(object anonymous)
        {
            this.Add(anonymous);
        }

        /// <summary>
        /// 创建JS对象的实例
        /// </summary>
        /// <param name="quotePropertyName">设置在输出时是否引用属性名</param>
        public JsonObject(bool quotePropertyName)
        {
            ((IPropertyNameQuotable)this).QuotePropertyName = quotePropertyName;
        }

        /// <summary>
        /// 创建JS对象的实例
        /// </summary>
        /// <param name="quotePropertyName">设置在输出时是否引用属性名</param>
        /// <param name="anonymous">指定用于初始化JsonObject的匿名对象</param>
        public JsonObject(bool quotePropertyName, object anonymous)
        {
            ((IPropertyNameQuotable)this).QuotePropertyName = quotePropertyName;
            this.Add(anonymous);
        }

        /// <summary>
        /// 将匿名对象的属性和数据映射到当前Json对象
        /// </summary>
        /// <param name="anonymous"></param>
        public void Add(object anonymous)
        {
            anonymous.EachProperty((name, value) =>
            {
                this.Add(name, value);
            });
        }

        /// <summary>
        /// 为当前Json对象增加新的属性
        /// </summary>
        /// <param name="name">新属性的名称</param>
        /// <param name="value">新属性的值</param>
        public void Add(string name, object value)
        {
            if (QuotePropertyName && value is IPropertyNameQuotable)
            {
                ((IPropertyNameQuotable)value).QuotePropertyName = QuotePropertyName;
                ((IPropertyNameQuotable)value).ByAdd();
            }
            if (value != null && value.GetType().Namespace == null)
            {
                base.BaseAdd(name, new JsonObject(QuotePropertyName, value));
            }
            else
            {
                base.BaseAdd(name, value);
            }
        }

        

        /// <summary>
        /// 获取该Json对象的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ((IJson)this).GetJsonString();
        }

        
        #region IJson成员
        string IJson.GetJsonString()
        {
            string paireFormat = QuotePropertyName ? "\"{0}\": {1}" : "{0}: {1}";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < Count; i++)
            {
                sb.Append(", ");
                sb.AppendFormat(paireFormat, Keys[i], JsonProperty.ValueOf(this[i]));
            }
            if (Count > 0) sb.Remove(1, 2);
            sb.Append("}");
            return sb.ToString();
        }
        #endregion
    }
}

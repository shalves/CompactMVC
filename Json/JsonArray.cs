using System.Collections.Generic;
using System.Text;

namespace System.Json
{
    /// <summary>
    /// 表示一个Json数组对象
    /// </summary>
    public sealed class JsonArray : List<object>, IJson
    {
        bool _QuotePropertyName = true;

        bool IJson.QuotePropertyName
        {
            get { return _QuotePropertyName; }
            set { _QuotePropertyName = value; }
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
            this._QuotePropertyName = quotePropertyName;
        }

        /// <summary>
        /// 将新的元素添加到当前Json数组对象的末尾
        /// </summary>
        /// <param name="value"></param>
        public new void Add(object value)
        {
            if (value != null && value.GetType().Namespace == null)
            {
                base.Add(new JsonObject(_QuotePropertyName, value));
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
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (object obj in this)
            {
                sb.Append(", ");
                if (obj is IJson) 
                {
                    ((IJson)obj).QuotePropertyName = this._QuotePropertyName;
                }
                sb.Append(JsonProperty.ValueOf(obj));
            }
            if (Count > 0) sb.Remove(1, 2);
            sb.Append("]");
            return sb.ToString();
        }
    }
}

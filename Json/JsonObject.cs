using System.Collections.Specialized;
using System.Text;
using System.Extensions;

namespace System.Json
{
    /// <summary>
    /// 表示一个Json对象
    /// </summary>
    public sealed class JsonObject : NameObjectCollectionBase, IJson
    {
        bool _QuotePropertyName = true;
        bool IJson.QuotePropertyName
        {
            get { return this._QuotePropertyName; }
            set { this._QuotePropertyName = value; }
        }

        /// <summary>
        /// 获取或设置对象中指定索引处的属性
        /// </summary>
        /// <param name="index"></param>
        public object this[int index]
        {
            get { return base.BaseGet(index); }
            set { base.BaseSet(index, value); }
        }

        /// <summary>
        /// 获取或设置对象中指定名称的属性的值
        /// </summary>
        /// <param name="name"></param>
        public object this[string name]
        {
            get { return base.BaseGet(name); }
            set { base.BaseSet(name, value); }
        }

        /// <summary>
        /// 创建Json对象的实例
        /// </summary>
        public JsonObject() { }

        /// <summary>
        /// 创建Json对象的实例
        /// </summary>
        /// <param name="anonymous">指定用于初始化JsonObject的匿名对象</param>
        public JsonObject(object anonymous)
        {
            this.Add(anonymous);
        }

        /// <summary>
        /// 创建Json对象的实例
        /// </summary>
        /// <param name="quotePropertyName">设置在输出时是否引用属性名</param>
        public JsonObject(bool quotePropertyName)
        {
            this._QuotePropertyName = quotePropertyName;
        }

        /// <summary>
        /// 创建Json对象的实例
        /// </summary>
        /// <param name="quotePropertyName">设置在输出时是否引用属性名</param>
        /// <param name="anonymous">指定用于初始化Json对象的匿名对象</param>
        public JsonObject(bool quotePropertyName, object anonymous)
        {
            this._QuotePropertyName = quotePropertyName;
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
            if (value != null && value.GetType().Namespace == null)
            {
                base.BaseAdd(name, new JsonObject(this._QuotePropertyName, value));
            }
            else
            {
                base.BaseAdd(name, value);
            }
        }

        /// <summary>
        /// 移除当前Json对象中指定名称的属性
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        /// <summary>
        /// 移除当前Json对象中指定索引处的属性
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        /// <summary>
        /// 获取该Json对象的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string paireFormat = this._QuotePropertyName ? "\"{0}\": {1}" : "{0}: {1}";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < Count; i++)
            {
                sb.Append(", ");
                if (this[i] is IJson)
                {
                    ((IJson)this[i]).QuotePropertyName = this._QuotePropertyName;
                }
                sb.AppendFormat(paireFormat, Keys[i], JsonProperty.ValueOf(this[i]));
            }
            if (Count > 0) sb.Remove(1, 2);
            sb.Append("}");
            return sb.ToString();
        }
    }
}

using System.Extensions;

namespace System.Collections.Specialized
{
    /// <summary>
    /// 表示名称和System.Object类型的值的集合
    /// </summary>
    public class NameObjectCollection : NameObjectCollectionBase
    {
        public virtual object this[string name]
        {
            get { return BaseGet(name); }
            set { BaseSet(name, value); }
        }
        
        public virtual object this[int index]
        {
            get { return BaseGet(index); }
            set { BaseSet(index, value); }
        }

        public NameObjectCollection()
        { }

        /// <summary>
        /// 初始化NameObjectCollection的新实例
        /// </summary>
        /// <param name="anonymous">指定用于初始化集合的匿名对象</param>
        public NameObjectCollection(object anonymous)
        {
            this.Add(anonymous);
        }

        public virtual bool HasKeys()
        {
            return BaseHasKeys();
        }

        /// <summary>
        /// 获取指定name的索引值
        /// <para>双循环</para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual int IndexOf(string name)
        {
            int j = Keys.Count / 2;
            for (int i = 0; j < Keys.Count; i++, j++)
            {
                if (Keys[i].Equals(name)) return i;
                if (Keys[j].Equals(name)) return j;
            }
            return -1;
        }

        public virtual string GetKey(int index)
        {
            return BaseGetKey(index);
        }

        public virtual string[] GetAllKeys()
        {
            return BaseGetAllKeys();
        }

        public virtual object[] GetAllValues()
        {
            return BaseGetAllValues();
        }

        /// <summary>
        /// 将匿名对象的属性和数据映射到当前集合
        /// </summary>
        /// <param name="anonymous"></param>
        public virtual void Add(object anonymous)
        {
            anonymous.EachProperty((name, value) =>
            {
                this.Add(name, value);
            });
        }

        public virtual void Add(string name, object value)
        {
            BaseAdd(name, value);
        }

        /// <summary>
        /// 将指定的集合合并到当前集合
        /// </summary>
        /// <param name="newCollection">要合并到当前集合的新集合</param>
        public virtual void MergeFrom(NameObjectCollection newCollection)
        {
            if (newCollection == null || newCollection.Count == 0) return;

            for (int i = 0; i < newCollection.Count; i++)
            {
                this[newCollection.GetKey(i)] = newCollection[i];
            }
        }

        /// <summary>
        /// 遍历当前集合，并对集合中的每组键值对执行指定的方法
        /// </summary>
        /// <param name="func">匿名函数（int: Index, string: Key, object: Value, bool: 返回false时中断）</param>
        public void Each(Func<int, string, object, bool> func)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!func(i, GetKey(i), this[i])) break;
            }
        }

        public virtual void Remove(string name)
        {
            BaseRemove(name);
        }

        public virtual void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public virtual void Clear()
        {
            BaseClear();
        }
    }
}

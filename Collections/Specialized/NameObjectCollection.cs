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

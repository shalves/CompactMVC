using System;

namespace System.Collections.Generic
{
    /// <summary>
    /// 泛型 "Key/Value" 列表 [V1.25]
    /// <para>名称唯一</para>
    /// </summary>
    /// <typeparam name="TValue">集合中元素Value的类型</typeparam>
    public class KeyValueList<TValue> : IEnumerable<TValue>, IEnumerator<TValue>
    {
        /// <summary>
        /// 字典集合
        /// </summary>
        IList<KeyValueListItem<TValue>> _ITEM;

        /// <summary>
        /// 获取集合中包含的元素个数
        /// </summary>
        public int Count
        {
            get { return _ITEM.Count; }
        }

        /// <summary>
        /// 获取或设置指定索引处的元素的值
        /// </summary>
        /// <param name="index">从0开始的索引</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        public TValue this[int index]
        {
            get
            {
                return _ITEM[index].Value;
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", "索引超出界限");
                _ITEM[index].Value = value;
            }
        }

        /// <summary>
        /// 获取或设置指定名称的元素的值
        /// </summary>
        /// <param name="key">项名称</param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <returns></returns>
        public TValue this[string key]
        {
            get
            {
                int i = IndexOf(key);
                if (i == -1)
                    throw new KeyNotFoundException(string.Format("未找到名为\"{0}\"的元素", key));
                return this[i];
            }
            set
            {
                AddOrSet(key, value);
            }
        }

        void Init()
        {
            _ITEM = new List<KeyValueListItem<TValue>>();
        }

        public KeyValueList()
        {
            Init();
        }

        public KeyValueList(string key, TValue value)
        {
            Init();
            Add(key, value);
        }

        /// <summary>
        /// 获取指定名称的项在集合中的位置
        /// <para>截断式循环</para>
        /// </summary>
        /// <param name="key">项名称</param>
        /// <returns></returns>
        public virtual int IndexOf(string key)
        {
            int j = _ITEM.Count / 2;
            for (int i = 0; j < _ITEM.Count; i++, j++)
            {
                if (_ITEM[i].Key.Equals(key)) return i;
                if (_ITEM[j].Key.Equals(key)) return j;
            }
            return -1;
        }

        /// <summary>
        /// 获取集合中指定索引处的元素的名称
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetKey(int index)
        {
            return _ITEM[index].Key;
        }

        /// <summary>
        /// 向集合追加元素,如果存在则替换
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddOrSet(string key, TValue value)
        {
            int i = IndexOf(key);
            if (i == -1)
            {
                _ITEM.Add(new KeyValueListItem<TValue>(key, value));
            }
            else
            {
                this[i] = value;
            }
        }

        /// <summary>
        /// 设置指定名称的元素的值,或向集合的末尾追加元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, TValue value)
        {
            AddOrSet(key, value);
        }

        /// <summary>
        /// 将指定的集合合并到当前集合
        /// </summary>
        /// <param name="newCollection">要合并到当前集合的新集合</param>
        public void MergeFrom(KeyValueList<TValue> newCollection)
        {
            if (newCollection == null || newCollection.Count == 0) return;

            for (int i = 0; i < newCollection.Count; i++)
            {
                AddOrSet(newCollection.GetKey(i), newCollection[i]);
            }
        }

        /// <summary>
        /// 将一项插入集合中的指定索引处
        /// </summary>
        /// <param name="index">从0开始的索引</param>
        /// <param name="key">名称</param>
        /// <param name="value">值</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Insert(int index, string key, TValue value)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("index", "索引超出界限");

            int i = IndexOf(key);

            if (i > -1) _ITEM.RemoveAt(i);

            if (index >= Count-1)
            {
                _ITEM.Add(new KeyValueListItem<TValue>(key, value));
            }
            else
            {
                _ITEM.Insert(index, new KeyValueListItem<TValue>(key, value));
            }
        }

        /// <summary>
        /// 移除集合中指定位置的项
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _ITEM.RemoveAt(index);
        }

        /// <summary>
        /// 移除集合中指定名称的项
        /// </summary>
        /// <param name="key"></param>
        public void RemoveAs(string key)
        {
            _ITEM.RemoveAt(IndexOf(key));
        }

        /// <summary>
        /// 反转集合中所有项的排列顺序
        /// </summary>
        public void Reverse()
        {
            ((List<KeyValueListItem<TValue>>)_ITEM).Reverse();
        }

        /// <summary>
        /// 移除集中所有的项
        /// </summary>
        public void Clear()
        {
            _ITEM.Clear();
        }

        /// <summary>
        /// 遍历集合中所有元素,并对每个元素执行指定的方法
        /// </summary>
        /// <param name="func">匿名函数(int: Index, string: Key, T: Value, bool: 当返回false时中断)</param>
        public void Each(Func<int, string, TValue, bool> func)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!func(i, _ITEM[i].Key, _ITEM[i].Value)) break;
            }
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #region IEnumerator<T>
        int _PTR = -1;

        TValue IEnumerator<TValue>.Current
        {
            get { return _ITEM[_PTR].Value; }
        }
        
        object IEnumerator.Current
        {
            get { return _ITEM[_PTR].Value as object; }
        }

        bool IEnumerator.MoveNext()
        {
            ++_PTR;
            return _PTR < Count;
        }

        void IEnumerator.Reset()
        {
            _PTR = -1;
        }

        void IDisposable.Dispose() { }
        #endregion

        /// <summary>
        /// 表示KeyValueList集合中的元素的类型
        /// </summary>
        /// <typeparam name="T">元素中Value的类型</typeparam>
        protected class KeyValueListItem<T>
        {
            private string _Key;
            private T _Value;

            /// <summary>
            /// 获取该元素的名称
            /// </summary>
            public string Key
            {
                get { return _Key; }
            }

            /// <summary>
            /// 获取或设置该元素的值
            /// </summary>
            public T Value
            {
                get { return _Value; }
                set { _Value = value; }
            }

            /// <summary>
            /// 创建LiteDictionaryItem元素的新实例
            /// </summary>
            /// <param name="key">名称</param>
            /// <param name="value">值</param>
            public KeyValueListItem(string key, T value)
            {
                _Key = key;
                _Value = value;
            }
        }
    }
}

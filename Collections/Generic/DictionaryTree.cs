namespace System.Collections.Generic
{
    /// <summary>
    /// 用于指示递归如果进行后续操作的枚举
    /// </summary>
    public enum RecursionInstruction
    {
        /// <summary>
        /// 指示递归操作继续
        /// </summary>
        Continue = 0,

        /// <summary>
        /// 指示递归操作在本域内跳过当前项
        /// </summary>
        Next = 1,

        /// <summary>
        /// 指示递归操作在本域内跳过剩余项
        /// </summary>
        Break = 2,

        /// <summary>
        /// 指示递归操作中断
        /// </summary>
        Abort = 3
    }

    /// <summary>
    /// 泛型字典树类 v1.1
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class DictionaryTree<K, V> : IEnumerable<DictionaryTree<K, V>>
    {
        readonly K _Key;
        DictionaryTree<K, V> _Root;
        IDictionary<K, DictionaryTree<K, V>> _Branches;

        public K Key
        {
            get { return _Key; }
        }

        public V Value { get; set; }

        protected DictionaryTree<K, V> Root
        {
            get { return _Root; }
        }

        protected IDictionary<K, DictionaryTree<K, V>> Branches
        {
            get
            {
                if (_Branches == null)
                    _Branches = new Dictionary<K, DictionaryTree<K, V>>();
                return _Branches;
            }
        }

        public int LinealBranchesCount
        {
            get { return Branches.Count; }
        }

        public DictionaryTree<K, V> this[K key]
        {
            get { return Branches.ContainsKey(key) ? Branches[key] : null; }
        }

        public DictionaryTree(K key, V value)
        {
            this._Key = key; Value = value;
        }

        public void AddBranch(K key, V value)
        {
            var branch = new DictionaryTree<K, V>(key, value);
            branch._Root = this;
            Branches.Add(key, branch);
        }

        public void AddBranch(DictionaryTree<K, V> branch)
        {
            branch._Root = this;
            Branches.Add(branch.Key, branch);
        }

        public bool RemoveBranch(K key)
        {
            if (this[key] == null)
            {
                foreach (var branche in Branches)
                {
                    if (!branche.Value.RemoveBranch(key)) continue;
                    return true;
                }
                return false;
            }
            else
            {
                return Branches.Remove(key);
            }
        }

        public int AllBranchsCount()
        {
            int _BranchesCount = Branches.Count;
            foreach (var branch in Branches)
            {
                _BranchesCount += branch.Value.AllBranchsCount();
            }
            return _BranchesCount;
        }

        public DictionaryTree<K, V> FindBranch(K key)
        {
            if (this[key] == null)
            {
                foreach (var branch in Branches)
                {
                    var t = branch.Value.FindBranch(key);
                    if (t == null) continue;
                    return t;
                }
            }
            return this[key];
        }

        public Dictionary<K, DictionaryTree<K, V>> AllBranches()
        {
            var all = new Dictionary<K, DictionaryTree<K, V>>();
            foreach (var branche in Branches)
            {
                all.Add(branche.Key, branche.Value);
                if (branche.Value.Branches.Count > 0)
                {
                    foreach (var branche2 in branche.Value.AllBranches())
                    {
                        all.Add(branche2.Key, branche.Value);
                    }
                }
            }
            return all;
        }

        public void TraverseBranches(Func<DictionaryTree<K, V>, RecursionInstruction> func)
        {
            RecursiveBranchesTraversal(func);
        }

        RecursionInstruction RecursiveBranchesTraversal(Func<DictionaryTree<K, V>, RecursionInstruction> func)
        {
            foreach (var branche in Branches)
            {
                var r = func(branche.Value);
                if (r == RecursionInstruction.Next) continue;
                if (r == RecursionInstruction.Break) break;
                if (r == RecursionInstruction.Abort) return RecursionInstruction.Abort;
                if (r == RecursionInstruction.Continue)
                {
                    if (branche.Value.LinealBranchesCount > 0)
                    {
                        if (branche.Value.RecursiveBranchesTraversal(func) == RecursionInstruction.Abort)
                        {
                            return RecursionInstruction.Abort;
                        }
                    }
                }
            }
            return RecursionInstruction.Continue;
        }

        IEnumerator<DictionaryTree<K, V>> IEnumerable<DictionaryTree<K, V>>.GetEnumerator()
        {
            return Branches.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Branches.Values.GetEnumerator();
        }
    }
}

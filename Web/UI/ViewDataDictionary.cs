using System.Collections.Generic;

namespace System.Web.UI
{
    /// <summary>
    /// 视图数据的键/值对集合
    /// </summary>
    public class ViewDataDictionary : Dictionary<string, object>
    {
        /// <summary>
        /// 将指定的视图数据集合中的所有元素合并到当前集合
        /// </summary>
        /// <param name="newDict"></param>
        public void MergeFrom(ViewDataDictionary newDict)
        {
            if (newDict == null || newDict.Count == 0) return;
            foreach (var item in newDict)
            {
                if (ContainsKey(item.Key))
                {
                    this[item.Key] = item.Value;
                }
                else
                {
                    this.Add(item.Key, item.Value);
                }
            }
        }
    }
}

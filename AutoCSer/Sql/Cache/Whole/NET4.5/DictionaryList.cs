using System;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 分组列表缓存
    /// </summary>
    public partial class DictionaryList<valueType, modelType, keyType>
    {
        /// <summary>
        /// 获取匹配数量
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="isValue">数据匹配器,禁止锁操作</param>
        /// <returns>匹配数量</returns>
        public Threading.CountAwaiter CountAwaiter(keyType key, Func<valueType, bool> isValue)
        {
            ListArray<valueType> list;
            if (groups.TryGetValue(key, out list))
            {
                if (isValue == null) throw new ArgumentNullException();
                Threading.ListArrayCountAwaiter<valueType> task = new Threading.ListArrayCountAwaiter<valueType>(list, isValue);
                cache.SqlTable.AddQueue(task);
                return task;
            }
            return new Threading.CountAwaiter.NullValue();
        }
    }
}

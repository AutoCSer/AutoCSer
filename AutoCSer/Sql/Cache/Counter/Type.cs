using System;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 计数缓存类型
    /// </summary>
    public enum Type : byte
    {
        /// <summary>
        /// 未知/自定义缓存类型
        /// </summary>
        Unknown,
        /// <summary>
        /// ID 计数缓存，AutoCSer.Sql.Cache.Counter.Event.Identity&lt;valueType, modelType&gt;
        /// </summary>
        IdentityCounter,
        /// <summary>
        /// 关键字计数缓存，AutoCSer.Sql.Cache.Counter.Event.PrimaryKey&lt;valueType, modelType, primaryKey&gt;
        /// </summary>
        PrimaryKeyCounter,
        /// <summary>
        /// 先进先出优先队列缓存，AutoCSer.Sql.Cache.Counter.MemberQueue&lt;valueType, modelType, memberCacheType, int&gt;
        /// </summary>
        CreateIdentityCounterMemberQueue,
        /// <summary>
        /// 先进先出优先队列缓存，AutoCSer.Sql.Cache.Counter.Queue&lt;valueType, modelType, int&gt;
        /// </summary>
        CreateIdentityCounterQueue,
        /// <summary>
        /// 先进先出优先队列缓存，AutoCSer.Sql.Cache.Counter.QueueList&lt;valueType, modelType, counterKeyType, keyType&gt;
        /// </summary>
        CreateIdentityCounterQueueList,
        /// <summary>
        /// 先进先出优先队列缓存，AutoCSer.Sql.Cache.Counter.Queue&lt;valueType, modelType, primaryKey&gt;
        /// </summary>
        CreatePrimaryKeyCounterQueue,
        /// <summary>
        /// 先进先出优先队列缓存，AutoCSer.Sql.Cache.Counter.QueueList&lt;valueType, modelType, primaryKey, keyType&gt;
        /// </summary>
        CreatePrimaryKeyCounterQueueList,
        /// <summary>
        /// 先进先出优先队列缓存，AutoCSer.Sql.Cache.Counter.QueueDictionary&lt;valueType, modelType, primaryKey, keyType, dictionaryKeyType&gt;
        /// </summary>
        CreatePrimaryKeyCounterQueueDictionary
    }
}

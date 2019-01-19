using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列自定义缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="cacheValueType">目标缓存类型</typeparam>
    public sealed partial class MemberQueueCustom<valueType, modelType, memberCacheType, keyType, cacheValueType>
        : MemberQueue<memberCacheType, cacheValueType>
        where valueType : class, modelType
        where modelType : class
        where keyType : struct, IEquatable<keyType>
        where memberCacheType : class
#if NOJIT
        where cacheValueType : class, ICustom
#else
        where cacheValueType : class, ICustom<valueType>
#endif
    {
        /// <summary>
        /// SQL操作工具
        /// </summary>
        private readonly AutoCSer.Sql.Table<valueType, modelType> sqlTable;
        /// <summary>
        /// 缓存关键字获取器
        /// </summary>
        private readonly Func<modelType, keyType> getKey;
        /// <summary>
        /// 获取节点
        /// </summary>
        private readonly Func<keyType, memberCacheType> getTarget;
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        private readonly Func<keyType, cacheValueType> getCache;
        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        public cacheValueType this[keyType key]
        {
            get
            {
                memberCacheType node = getTarget(key);
                if (node != null)
                {
                    GetTask task = new GetTask(this, node, key);
                    sqlTable.AddQueue(task);
                    return task.Wait();
                }
                return null;
            }
        }
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="sqlTable">数据表格</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getTarget">获取节点</param>
        /// <param name="getCache">获取缓存数据</param>
        /// <param name="valueMember">节点成员</param>
        /// <param name="previousMember">前一个节点成员</param>
        /// <param name="nextMember">后一个节点成员</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public MemberQueueCustom(AutoCSer.Sql.Table<valueType, modelType> sqlTable, Func<modelType, keyType> getKey, Func<keyType, memberCacheType> getTarget
            , Func<keyType, cacheValueType> getCache, Expression<Func<memberCacheType, cacheValueType>> valueMember
            , Expression<Func<memberCacheType, memberCacheType>> previousMember, Expression<Func<memberCacheType, memberCacheType>> nextMember, int maxCount = 0)
            : base(valueMember, previousMember, nextMember, maxCount)
        {
            if (sqlTable == null) throw new ArgumentNullException("sqlTable is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            if (getTarget == null) throw new ArgumentNullException("getTarget is null");
            this.sqlTable = sqlTable;
            this.getKey = getKey;
            this.getTarget = getTarget;
            this.getCache = getCache;

            sqlTable.OnInserted += onInserted;
            sqlTable.OnUpdated += onUpdated;
            sqlTable.OnDeleted += onDeleted;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        protected override void removeCounter(memberCacheType node)
        {
            //--count;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onInserted(valueType value)
        {
            memberCacheType node = getTarget(getKey(value));
            if (node != null)
            {
                cacheValueType cache = get(node);
                if (cache != null) cache.Add(value);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onUpdated(valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            onInserted(value);
            onDeleted(oldValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onDeleted(valueType value)
        {
            memberCacheType node = getTarget(getKey(value));
            if (node != null)
            {
                cacheValueType cache = get(node);
                if (cache != null) cache.Remove(value);
            }
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(keyType key)
        {
            memberCacheType node = getTarget(key);
            if (node != null) removeNode(node);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private sealed class GetTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private MemberQueueCustom<valueType, modelType, memberCacheType, keyType, cacheValueType> queue;
            /// <summary>
            /// 成员缓存
            /// </summary>
            private memberCacheType node;
            /// <summary>
            /// 返回值
            /// </summary>
            private cacheValueType value;
            /// <summary>
            /// 关键字
            /// </summary>
            private keyType key;
            /// <summary>
            /// 等待缓存加载
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="queue"></param>
            /// <param name="node"></param>
            /// <param name="key"></param>
            internal GetTask(MemberQueueCustom<valueType, modelType, memberCacheType, keyType, cacheValueType> queue, memberCacheType node, keyType key)
            {
                this.queue = queue;
                this.node = node;
                this.key = key;
                wait.Set(0);
            }
            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    value = queue.get(node, key);
                }
                finally
                {
                    wait.Set();
                }
                return LinkNext;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal cacheValueType Wait()
            {
                wait.Wait();
                return value;
            }
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private cacheValueType get(memberCacheType node, keyType key)
        {
            cacheValueType cache = get(node);
            if (cache == null)
            {
                cache = getCache(key);
                appendNode(node, cache);
            }
            return cache;
        }
    }
}

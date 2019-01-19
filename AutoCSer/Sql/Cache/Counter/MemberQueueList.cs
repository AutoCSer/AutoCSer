using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;
using AutoCSer.Extension;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列缓存(不适应于update/delete)
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed partial class MemberQueueList<valueType, modelType, memberCacheType, keyType>
        : MemberQueue<memberCacheType, ListArray<valueType>>
        where valueType : class, modelType
        where modelType : class
        where keyType : struct, IEquatable<keyType>
        where memberCacheType : class
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
        /// 条件表达式获取器
        /// </summary>
        private readonly Func<keyType, Expression<Func<modelType, bool>>> getWhere;
        /// <summary>
        /// 缓存数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据集合</returns>
        public LeftArray<valueType> this[keyType key]
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
                return default(LeftArray<valueType>);
            }
        }
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="sqlTable">数据表格</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="getTarget">获取节点</param>
        /// <param name="getWhere">条件表达式获取器</param>
        /// <param name="valueMember">节点成员</param>
        /// <param name="previousMember">前一个节点成员</param>
        /// <param name="nextMember">后一个节点成员</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public MemberQueueList(AutoCSer.Sql.Table<valueType, modelType> sqlTable, Func<modelType, keyType> getKey, Func<keyType, memberCacheType> getTarget
            , Func<keyType, Expression<Func<modelType, bool>>> getWhere, Expression<Func<memberCacheType, ListArray<valueType>>> valueMember
            , Expression<Func<memberCacheType, memberCacheType>> previousMember, Expression<Func<memberCacheType, memberCacheType>> nextMember
            , int maxCount = 0)
            : base(valueMember, previousMember, nextMember, maxCount)
        {
            if (sqlTable == null) throw new ArgumentNullException("sqlTool is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            if (getTarget == null) throw new ArgumentNullException("getTarget is null");
            this.sqlTable = sqlTable;
            this.getKey = getKey;
            this.getTarget = getTarget;
            this.getWhere = getWhere;
            //this.isRemoveEnd = isRemoveEnd;

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
                ListArray<valueType> list = get(node);
                if (list != null) list.Add(value);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private void onUpdated(valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            keyType key = getKey(value), oldKey = getKey(oldValue);
            memberCacheType node = getTarget(key);
            if (node != null) removeNode(node);
            if (!key.Equals(oldKey))
            {
                node = getTarget(key);
                if (node != null) removeNode(node);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onDeleted(valueType value)
        {
            memberCacheType node = getTarget(getKey(value));
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
            private MemberQueueList<valueType, modelType, memberCacheType, keyType> queue;
            /// <summary>
            /// 成员缓存
            /// </summary>
            private memberCacheType node;
            /// <summary>
            /// 返回值
            /// </summary>
            private LeftArray<valueType> value;
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
            internal GetTask(MemberQueueList<valueType, modelType, memberCacheType, keyType> queue, memberCacheType node, keyType key)
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
                    value = queue.get(ref connection, node, key);
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
            internal LeftArray<valueType> Wait()
            {
                wait.Wait();
                return value;
            }
        }
        /// <summary>
        /// 获取缓存数据集合
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private LeftArray<valueType> get(ref DbConnection connection, memberCacheType node, keyType key)
        {
            ListArray<valueType> list = get(node);
            if (list == null)
            {
                list = new ListArray<valueType>(sqlTable.SelectQueue(ref connection, getWhere(key)));
                appendNode(node, list);
            }
            return new LeftArray<valueType>(list);
        }
    }
}

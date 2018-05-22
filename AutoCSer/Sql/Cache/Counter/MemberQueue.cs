using System;
using AutoCSer.Metadata;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Data.Common;

namespace AutoCSer.Sql.Cache.Counter
{
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="cacheValueType"></typeparam>
    public abstract class MemberQueue<memberCacheType, cacheValueType>
        where memberCacheType : class
        where cacheValueType : class
    {
        /// <summary>
        /// 获取节点数据
        /// </summary>
        protected readonly Func<memberCacheType, cacheValueType> getMemberValue;
        /// <summary>
        /// 设置节点数据
        /// </summary>
        protected readonly Action<memberCacheType, cacheValueType> setMemberValue;
        /// <summary>
        /// 获取前一个节点
        /// </summary>
        protected readonly Func<memberCacheType, memberCacheType> getPrevious;
        /// <summary>
        /// 设置前一个节点
        /// </summary>
        protected readonly Action<memberCacheType, memberCacheType> setPrevious;
        /// <summary>
        /// 获取后一个节点
        /// </summary>
        protected readonly Func<memberCacheType, memberCacheType> getNext;
        /// <summary>
        /// 设置后一个节点
        /// </summary>
        protected readonly Action<memberCacheType, memberCacheType> setNext;
        /// <summary>
        /// 头节点
        /// </summary>
        protected memberCacheType header;
        /// <summary>
        /// 尾节点
        /// </summary>
        protected memberCacheType end;
        /// <summary>
        /// 缓存默认最大容器大小
        /// </summary>
        protected readonly int maxCount;
        /// <summary>
        /// 数据数量
        /// </summary>
        protected int count;
        /// <summary>
        /// 先进先出优先队列缓存(非计数缓存)
        /// </summary>
        /// <param name="valueMember">节点成员</param>
        /// <param name="previousMember">前一个节点成员</param>
        /// <param name="nextMember">后一个节点成员</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        protected MemberQueue(Expression<Func<memberCacheType, cacheValueType>> valueMember, Expression<Func<memberCacheType, memberCacheType>> previousMember
            , Expression<Func<memberCacheType, memberCacheType>> nextMember, int maxCount)
        {
            if (valueMember == null) throw new ArgumentNullException("valueMember is null");
            if (previousMember == null) throw new ArgumentNullException("previousMember is null");
            if (nextMember == null) throw new ArgumentNullException("nextMember is null");
            MemberExpression<memberCacheType, cacheValueType> valueExpression = new MemberExpression<memberCacheType, cacheValueType>(valueMember);
            if (valueExpression.Field == null) throw new InvalidCastException("valueMember is not MemberExpression");
            MemberExpression<memberCacheType, memberCacheType> previousExpression = new MemberExpression<memberCacheType, memberCacheType>(previousMember);
            if (previousExpression.Field == null) throw new InvalidCastException("previousMember is not MemberExpression");
            MemberExpression<memberCacheType, memberCacheType> nextExpression = new MemberExpression<memberCacheType, memberCacheType>(nextMember);
            if (nextExpression.Field == null) throw new InvalidCastException("nextMember is not MemberExpression");
            getMemberValue = valueExpression.GetMember;
            setMemberValue = valueExpression.SetMember;
            getPrevious = previousExpression.GetMember;
            setPrevious = previousExpression.SetMember;
            getNext = nextExpression.GetMember;
            setNext = nextExpression.SetMember;
            this.maxCount = maxCount <= 0 ? ConfigLoader.Config.CacheMaxCount : maxCount;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        protected abstract void removeCounter(memberCacheType node);
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected cacheValueType get(memberCacheType node)
        {
            cacheValueType value = getMemberValue(node);
            if (value != null && node != end)
            {
                memberCacheType previous = getPrevious(node), next = getNext(node);
                if (previous == null) setPrevious(header = next, null);
                else
                {
                    setNext(previous, next);
                    setPrevious(next, previous);
                }
                setNext(end, node);
                setPrevious(node, end);
                setNext(node, null);
                end = node;
            }
            return value;
        }
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        protected void appendNode(memberCacheType node, cacheValueType value)
        {
            setMemberValue(node, value);
            if (end == null) header = end = node;
            else
            {
                setPrevious(node, end);
                setNext(end, node);
                end = node;
            }
            if (count == maxCount)
            {
                setMemberValue(node = header, null);
                setPrevious(header = getNext(header), null);
                setNext(node, null);
                removeCounter(node);
            }
            else ++count;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        protected void removeNode(memberCacheType node)
        {
            setMemberValue(node, null);
            memberCacheType previous = getPrevious(node), next = getNext(node);
            if (previous == null)
            {
                if (next == null) header = end = null;
                else
                {
                    setNext(node, null);
                    setPrevious(header = next, null);
                }
            }
            else
            {
                setPrevious(node, null);
                if (next == null) setNext(end = previous, null);
                else
                {
                    setNext(node, null);
                    setNext(previous, next);
                    setPrevious(next, previous);
                }
            }
            --count;
        }
    }
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="couterKeyType"></typeparam>
    /// <typeparam name="counterTargetType"></typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="cacheValueType"></typeparam>
    public abstract class MemberQueue<valueType, modelType, memberCacheType, couterKeyType, counterTargetType, keyType, cacheValueType>
        : MemberQueue<memberCacheType, cacheValueType>
        where valueType : class, modelType
        where modelType : class
        where couterKeyType : struct, IEquatable<couterKeyType>
        where counterTargetType : class
        where keyType : struct, IEquatable<keyType>
        where memberCacheType : class
        where cacheValueType : class
    {
        /// <summary>
        /// 缓存计数器
        /// </summary>
        protected readonly Event.Member<valueType, modelType, memberCacheType, couterKeyType, counterTargetType> counter;
        /// <summary>
        /// 缓存关键字获取器
        /// </summary>
        protected readonly Func<modelType, keyType> getKey;
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getKey">缓存关键字获取器</param>
        /// <param name="valueMember">节点成员</param>
        /// <param name="previousMember">前一个节点成员</param>
        /// <param name="nextMember">后一个节点成员</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        protected MemberQueue(Event.Member<valueType, modelType, memberCacheType, couterKeyType, counterTargetType> counter, Func<modelType, keyType> getKey
            , Expression<Func<memberCacheType, cacheValueType>> valueMember, Expression<Func<memberCacheType, memberCacheType>> previousMember
            , Expression<Func<memberCacheType, memberCacheType>> nextMember, int maxCount)
            : base(valueMember, previousMember, nextMember, maxCount)
        {
            if (counter == null) throw new ArgumentNullException("counter is null");
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            this.counter = counter;
            this.getKey = getKey;
        }
    }
    /// <summary>
    /// 先进先出优先队列缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed partial class MemberQueue<valueType, modelType, memberCacheType, keyType>
        : MemberQueue<valueType, modelType, memberCacheType, keyType, memberCacheType, keyType, valueType>
        where valueType : class, modelType
        where modelType : class
        where keyType : struct, IEquatable<keyType>
        where memberCacheType : class
    {
        /// <summary>
        /// 数据获取器
        /// </summary>
        private readonly Table<valueType, modelType, keyType>.GetValue getValue;
        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>缓存数据</returns>
        public valueType this[keyType key]
        {
            get
            {
                memberCacheType node = counter.GetByKey(key);
                if (node != null)
                {
                    GetTask task = new GetTask(this, node, key);
                    counter.SqlTable.AddQueue(task);
                    return task.Wait();
                }
                return null;
            }
        }
        /// <summary>
        /// 先进先出优先队列缓存
        /// </summary>
        /// <param name="counter">缓存计数器</param>
        /// <param name="getValue">数据获取器,禁止数据库与锁操作</param>
        /// <param name="valueMember">节点成员</param>
        /// <param name="previousMember">前一个节点成员</param>
        /// <param name="nextMember">后一个节点成员</param>
        /// <param name="maxCount">缓存默认最大容器大小</param>
        public MemberQueue(Event.Member<valueType, modelType, memberCacheType, keyType, memberCacheType> counter
            , Table<valueType, modelType, keyType>.GetValue getValue, Expression<Func<memberCacheType, valueType>> valueMember
            , Expression<Func<memberCacheType, memberCacheType>> previousMember, Expression<Func<memberCacheType, memberCacheType>> nextMember, int maxCount = 0)
            : base(counter, counter.GetKey, valueMember, previousMember, nextMember, maxCount)
        {
            if (getValue == null) throw new ArgumentNullException();
            this.getValue = getValue;

            //counter.OnReset += reset;
            counter.SqlTable.OnInserted += onInserted;
            counter.OnDeleted += onDeleted;
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        protected override void removeCounter(memberCacheType node)
        {
            counter.Remove(node);
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onInserted(valueType value)
        {
            memberCacheType node = counter.AddGetTarget(value);
            appendNode(node, counter.GetMember(node).Key);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void onDeleted(valueType value)
        {
            memberCacheType node = counter.GetValue(value);
            //memberCacheType node = counter.GetByKey(getKey(value));
            if (getMemberValue(node) != null) removeNode(node);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private sealed class GetTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 先进先出优先队列缓存
            /// </summary>
            private MemberQueue<valueType, modelType, memberCacheType, keyType> queue;
            /// <summary>
            /// 成员缓存
            /// </summary>
            private memberCacheType node;
            /// <summary>
            /// 返回值
            /// </summary>
            private valueType value;
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
            internal GetTask(MemberQueue<valueType, modelType, memberCacheType, keyType> queue, memberCacheType node, keyType key)
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
            internal valueType Wait()
            {
                wait.Wait();
                return value;
            }
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private valueType get(ref DbConnection connection, memberCacheType node, keyType key)
        {
            valueType value = get(node);
            if (value == null && (value = counter.Get(key)) == null)
            {
                if ((value = getValue(ref connection, key, counter.MemberMap)) != null) onInserted(value);
            }
            return value;
        }
    }
}

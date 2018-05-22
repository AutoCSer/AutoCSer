using System;
using System.Linq.Expressions;
using System.Reflection;
using AutoCSer.Extension;
using AutoCSer.Metadata;
using System.Threading;
using System.Runtime.InteropServices;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 计数成员（非精确计数）
    /// </summary>
    public class CountMember : AutoCSer.Threading.DoubleLink<CountMember>
    {
        /// <summary>
        /// 超时秒数
        /// </summary>
        protected int timeout;
        /// <summary>
        /// 当前超时秒数
        /// </summary>
        protected int second;
        /// <summary>
        /// 计数成员
        /// </summary>
        protected CountMember() { }
        /// <summary>
        /// 增加计数
        /// </summary>
        /// <param name="id">数据标识</param>
        public virtual void Inc(int id) { }
        /// <summary>
        /// 获取计数
        /// </summary>
        /// <param name="id">数据标识</param>
        /// <returns></returns>
        public virtual int Get(int id) { return -1; }
        /// <summary>
        /// 计数总量
        /// </summary>
        public virtual long TotalCount { get { return -1; } }
        /// <summary>
        /// 更新计数
        /// </summary>
        protected virtual void update() { }
        /// <summary>
        /// 定时器触发日志写入
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnTimer()
        {
            if (Interlocked.Decrement(ref second) == 0) update();
        }

        /// <summary>
        /// 默认空计数成员
        /// </summary>
        public static readonly CountMember Null = new CountMember();
        /// <summary>
        /// 计数成员链表
        /// </summary>
        internal static YieldLink CountMembers;
    }
    /// <summary>
    /// 计数成员（非精确计数）
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">扩展缓存绑定类型</typeparam>
    public sealed class CountMember<valueType, modelType, memberCacheType> : CountMember, IDisposable
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
    {
        /// <summary>
        /// 释放计数更新任务
        /// </summary>
        private sealed class FreeTask : Threading.LinkQueueTaskNode<FreeTask>
        {
            /// <summary>
            /// 计数成员
            /// </summary>
            private CountMember<valueType, modelType, memberCacheType> countMember;
            /// <summary>
            /// 更新数据
            /// </summary>
            private valueType value;
            /// <summary>
            /// 更新记录查询信息
            /// </summary>
            internal UpdateQuery<modelType> Query;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="countMember">计数成员</param>
            /// <param name="value">更新数据</param>
            internal void Set(CountMember<valueType, modelType, memberCacheType> countMember, valueType value)
            {
                this.countMember = countMember;
                this.value = value;
                Query.MemberMap = countMember.selectMemberMap;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                Threading.LinkQueueTaskNode next = LinkNext;
                Sql.Table<valueType, modelType> table = countMember.table;
                table.Client.Update(table, ref connection, value, countMember.memberMap, ref Query, true);
                LinkNext = null;
                Push();
                return next;
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push()
            {
                countMember.freeValue = value;
                Query.Clear();
                countMember = null;
                value = null;
                YieldPool.Default.PushNotNull(this);
            }
        }
        /// <summary>
        /// 更新计数任务
        /// </summary>
        private sealed class UpdateTask : Threading.LinkQueueTaskNode<UpdateTask>
        {
            /// <summary>
            /// 计数成员
            /// </summary>
            private CountMember<valueType, modelType, memberCacheType> countMember;
            /// <summary>
            /// 更新记录查询信息
            /// </summary>
            internal UpdateQuery<modelType> Query;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="countMember">计数成员</param>
            internal void Set(CountMember<valueType, modelType, memberCacheType> countMember)
            {
                this.countMember = countMember;
                Query.MemberMap = countMember.selectMemberMap;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                Threading.LinkQueueTaskNode next = LinkNext;
                try
                {
                    Sql.Table<valueType, modelType> table = countMember.table;
                    table.Client.Update(table, ref connection, countMember.updateValue, countMember.memberMap, ref Query, true);
                }
                finally { countMember.check(ref connection); }
                LinkNext = null;
                Push();
                return next;
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push()
            {
                countMember = null;
                Query.Clear();
                YieldPool.Default.PushNotNull(this);
            }
        }
        /// <summary>
        /// 数据表格
        /// </summary>
        private readonly Sql.Table<valueType, modelType> table;
        /// <summary>
        /// 待计数缓存
        /// </summary>
        private readonly Event.IdentityCache<valueType, modelType, memberCacheType> cache;
        /// <summary>
        /// 获取缓存计数
        /// </summary>
        private readonly Func<modelType, int> getCacheCount;
        /// <summary>
        /// 设置缓存计数
        /// </summary>
        private readonly Action<modelType, int> setCacheCount;
        /// <summary>
        /// 获取未处理计数
        /// </summary>
        private readonly Func<memberCacheType, int> getCount;
        /// <summary>
        /// 设置未处理计数
        /// </summary>
        private readonly Action<memberCacheType, int> setCount;
        /// <summary>
        /// 设置数据标识
        /// </summary>
        private readonly Action<valueType, long> setIdentity;
        /// <summary>
        /// 计数访问锁
        /// </summary>
        private readonly object counterLock;
        /// <summary>
        /// 查询成员
        /// </summary>
        private MemberMap<modelType> selectMemberMap;
        /// <summary>
        /// 计数成员
        /// </summary>
        private MemberMap<modelType> memberMap;
        /// <summary>
        /// 更新数据对象
        /// </summary>
        private valueType updateValue;
        /// <summary>
        /// 释放位置临时更新数据对象
        /// </summary>
        private valueType freeValue;
        /// <summary>
        /// 未处理计数数据标识列表
        /// </summary>
        private int[] ids;
        /// <summary>
        /// 未处理计数数据标识索引位置
        /// </summary>
        private int idIndex;
        /// <summary>
        /// 当前释放数据标识索引位置
        /// </summary>
        private int freeIdIndex;
        /// <summary>
        /// 当前更新数据标识索引位置
        /// </summary>
        private int updateIndex;
        /// <summary>
        /// 计数总量
        /// </summary>
        private long totalCount;
        /// <summary>
        /// 计数总量
        /// </summary>
        public override long TotalCount
        {
            get { return totalCount; }
        }
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 浏览计数
        /// </summary>
        /// <param name="cache">待计数缓存</param>
        /// <param name="member">计数成员</param>
        /// <param name="timeout">超时秒数</param>
        public CountMember(Event.IdentityCache<valueType, modelType, memberCacheType> cache, Expression<Func<modelType, int>> member, int timeout)
        {
            MemberExpression<modelType, int> memberExpression = new MemberExpression<modelType, int>(member);
            if (memberExpression.Field == null) throw new InvalidCastException("member is not MemberExpression");
            FieldInfo filed = typeof(memberCacheType).GetField(memberExpression.Field.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (filed == null) throw new FieldAccessException(typeof(memberCacheType).fullName() + " 没有找到计数字段 " + memberExpression.Field.Name);
            this.cache = cache;
            table = cache.SqlTable;
            this.timeout = second = Math.Max(timeout, 1);
            int size = table.Attribute.CountMemberCacheSize;
            ids = new int[size <= 0 ? TableAttribute.DefaultCountMemberCacheSize : size];
            getCacheCount = memberExpression.GetMember;
            setCacheCount = memberExpression.SetMember;
            getCount = AutoCSer.Emit.Field.UnsafeGetField<memberCacheType, int>(filed);
            setCount = AutoCSer.Emit.Field.UnsafeSetField<memberCacheType, int>(filed);
            setIdentity = DataModel.Model<modelType>.SetIdentity;
            updateValue = AutoCSer.Emit.Constructor<valueType>.New();
            (memberMap = new MemberMap<modelType>()).SetMember(memberExpression.Field.Name);
            selectMemberMap = table.GetSelectMemberMap(memberMap);
            foreach (valueType value in cache.Values) totalCount += getCacheCount(value);
            counterLock = new object();
            CountMembers.PushNotNull(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                CountMembers.PopNotNull(this);
                if (Interlocked.Exchange(ref second, 0) != 0) update();
            }
        }
        /// <summary>
        /// 获取计数
        /// </summary>
        /// <param name="id">数据标识</param>
        /// <returns></returns>
        public override int Get(int id)
        {
            valueType value = cache[id];
            return value == null ? 0 : getCacheCount(value) + getCount(cache.GetMemberCache(value));
        }
        /// <summary>
        /// 增加计数
        /// </summary>
        /// <param name="id">数据标识</param>
        public override void Inc(int id)
        {
            if (isDisposed == 0)
            {
                memberCacheType memberCache = cache.GetMemberCacheByKey(id);
                if (memberCache != null)
                {
                    Interlocked.Increment(ref totalCount);
                    Monitor.Enter(counterLock);
                    int count = getCount(memberCache);
                    if (count == 0)
                    {
                        setCount(memberCache, 1);
                        if (idIndex == ids.Length)
                        {
                            int freeId = ids[freeIdIndex];
                            valueType value = cache[freeId];
                            ids[freeIdIndex++] = id;
                            if (freeIdIndex == ids.Length) freeIdIndex = 0;
                            if (value == null) Monitor.Exit(counterLock);
                            else
                            {
                                memberCache = cache.GetMemberCache(value);
                                count = getCount(memberCache);
                                setCount(memberCache, 0);
                                Monitor.Exit(counterLock);
                                valueType freeValue = Interlocked.Exchange(ref this.freeValue, null) ?? AutoCSer.Emit.Constructor<valueType>.New();
                                setIdentity(freeValue, freeId);
                                setCacheCount(freeValue, getCacheCount(value) + count);
                                FreeTask freeTask = (FreeTask.YieldPool.Default.Pop() as FreeTask) ?? new FreeTask();
                                freeTask.Set(this, freeValue);
                                try
                                {
                                    if (table.Client.Update(table, freeValue, memberMap, ref freeTask.Query))
                                    {
                                        table.AddQueue(freeTask);
                                        freeTask = null;
                                    }
                                }
                                finally
                                {
                                    if (freeTask != null) freeTask.Push();
                                }
                            }
                        }
                        else
                        {
                            ids[idIndex++] = id;
                            Monitor.Exit(counterLock);
                        }
                    }
                    else
                    {
                        setCount(memberCache, count + 1);
                        Monitor.Exit(counterLock);
                    }
                }
            }
        }
        /// <summary>
        /// 更新计数
        /// </summary>
        protected override void update()
        {
            do
            {
                Monitor.Enter(counterLock);
                if (idIndex == 0)
                {
                    Monitor.Exit(counterLock);
                    second = timeout;
                    return;
                }
                if (updateIndex == 0) updateIndex = idIndex;
            NEXT:
                int id = ids[--updateIndex];
                valueType value = cache[id];
                ids[updateIndex] = ids[--idIndex];
                if (value == null)
                {
                    if (updateIndex == 0)
                    {
                        Monitor.Exit(counterLock);
                        second = timeout;
                        return;
                    }
                    goto NEXT;
                }
                memberCacheType memberCache = cache.GetMemberCache(value);
                int count = getCount(memberCache);
                setCount(memberCache, 0);
                Monitor.Exit(counterLock);

                setIdentity(updateValue, id);
                setCacheCount(updateValue, getCacheCount(value) + count);
                UpdateTask updateTask = (UpdateTask.YieldPool.Default.Pop() as UpdateTask) ?? new UpdateTask();
                updateTask.Set(this);
                try
                {
                    if (table.Client.Update(table, updateValue, memberMap, ref updateTask.Query))
                    {
                        table.AddQueue(updateTask);
                        updateTask = null;
                        return;
                    }
                }
                catch (Exception error)
                {
                    table.Log.Add(Log.LogType.Error, error);
                }
                finally
                {
                    if (updateTask != null) updateTask.Push();
                }
            }
            while (true);
        }
        /// <summary>
        /// 检测更新处理
        /// </summary>
        /// <param name="connection"></param>
        private void check(ref DbConnection connection)
        {
            if (isDisposed == 0)
            {
                second = timeout;
                if (isDisposed == 0 || Interlocked.Exchange(ref second, 0) == 0) return;
            }
            Sql.Client client = table.Client;
            UpdateQuery<modelType> query = new UpdateQuery<modelType> { MemberMap = selectMemberMap };
            do
            {
                Monitor.Enter(counterLock);
                if (idIndex == 0)
                {
                    Monitor.Exit(counterLock);
                    return;
                }
                if (updateIndex == 0) updateIndex = idIndex;
            NEXT:
                int id = ids[--updateIndex];
                valueType value = cache[id];
                ids[updateIndex] = ids[--idIndex];
                if (value == null)
                {
                    if (updateIndex == 0)
                    {
                        Monitor.Exit(counterLock);
                        return;
                    }
                    goto NEXT;
                }
                memberCacheType memberCache = cache.GetMemberCache(value);
                int count = getCount(memberCache);
                setCount(memberCache, 0);
                Monitor.Exit(counterLock);

                setIdentity(updateValue, id);
                setCacheCount(updateValue, getCacheCount(value) + count);
                try
                {
                    if (client.Update(table, updateValue, memberMap, ref query)) client.Update(table, ref connection, updateValue, memberMap, ref query, true);
                }
                catch (Exception error)
                {
                    table.Log.Add(Log.LogType.Error, error);
                }
                finally { query.ClearSql(); }
            }
            while (true);
        }
    }
}

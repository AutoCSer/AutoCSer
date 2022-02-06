using System;
using System.Linq.Expressions;
using System.Reflection;
using AutoCSer.Extensions;
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
    public class CountMember : AutoCSer.Threading.SecondTimerTaskNode
    {
        /// <summary>
        /// 计数成员
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        protected CountMember(int timeoutSeconds) : base(AutoCSer.Threading.SecondTimer.TaskArray, timeoutSeconds, AutoCSer.Threading.SecondTimerThreadMode.TinyBackgroundThreadPool, AutoCSer.Threading.SecondTimerKeepMode.After, timeoutSeconds) { }
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
        protected internal override void OnTimer() { }

        /// <summary>
        /// 默认空计数成员
        /// </summary>
        public static readonly new CountMember Null = new CountMember(0);
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
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                Sql.Table<valueType, modelType> table = countMember.table;
                table.Client.Update(table, ref connection, value, countMember.memberMap, ref Query, true);
                Push();
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
                YieldPool.Default.Push(this);
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
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    Sql.Table<valueType, modelType> table = countMember.table;
                    table.Client.Update(table, ref connection, countMember.updateValue, countMember.memberMap, ref Query, true);
                }
                finally { countMember.check(ref connection); }
                Push();
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push()
            {
                countMember = null;
                Query.Clear();
                YieldPool.Default.Push(this);
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
        /// 更新计数任务数量
        /// </summary>
        private volatile int updateTaskCount;
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
        /// <param name="timeoutSeconds">超时秒数</param>
        public CountMember(Event.IdentityCache<valueType, modelType, memberCacheType> cache, Expression<Func<modelType, int>> member, int timeoutSeconds)
            : base(Math.Max(timeoutSeconds, 1))
        {
            MemberExpression<modelType, int> memberExpression = new MemberExpression<modelType, int>(member);
            if (memberExpression.Field == null) throw new InvalidCastException("member is not MemberExpression");
            FieldInfo filed = typeof(memberCacheType).GetField(memberExpression.Field.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (filed == null) throw new FieldAccessException(typeof(memberCacheType).fullName() + " 没有找到计数字段 " + memberExpression.Field.Name);
            this.cache = cache;
            table = cache.SqlTable;
            int size = table.Attribute.CountMemberCacheSize;
            ids = new int[size <= 0 ? TableAttribute.DefaultCountMemberCacheSize : size];
            getCacheCount = memberExpression.GetMember;
            setCacheCount = memberExpression.SetMember;
            getCount = AutoCSer.Emit.Field.UnsafeGetField<memberCacheType, int>(filed);
            setCount = AutoCSer.Emit.Field.UnsafeSetField<memberCacheType, int>(filed);
            setIdentity = DataModel.Model<modelType>.SetIdentity;
            updateValue = AutoCSer.Metadata.DefaultConstructor<valueType>.Constructor();
            (memberMap = new MemberMap<modelType>()).SetMember(memberExpression.Field.Name);
            selectMemberMap = table.GetSelectMemberMap(memberMap);
            foreach (valueType value in cache.Values) totalCount += getCacheCount(value);
            counterLock = new object();
            AppendTaskArray();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                KeepMode = AutoCSer.Threading.SecondTimerKeepMode.Canceled;
                OnTimer();
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
                                valueType freeValue = Interlocked.Exchange(ref this.freeValue, null) ?? AutoCSer.Metadata.DefaultConstructor<valueType>.Constructor();
                                setIdentity(freeValue, freeId);
                                setCacheCount(freeValue, getCacheCount(value) + count);
                                FreeTask freeTask = (FreeTask.YieldPool.Default.Pop() as FreeTask) ?? new FreeTask();
                                freeTask.Set(this, freeValue);
                                try
                                {
                                    ReturnType returnType = table.Client.Update(table, freeValue, memberMap, ref freeTask.Query);
                                    if (returnType == ReturnType.Success)
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
        protected internal override void OnTimer()
        {
            while (updateTaskCount == 0)
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
                UpdateTask updateTask = (UpdateTask.YieldPool.Default.Pop() as UpdateTask) ?? new UpdateTask();
                updateTask.Set(this);
                try
                {
                    ReturnType returnType = table.Client.Update(table, updateValue, memberMap, ref updateTask.Query);
                    if (returnType == ReturnType.Success)
                    {
                        Interlocked.Increment(ref updateTaskCount);
                        table.AddQueue(updateTask);
                        updateTask = null;
                        return;
                    }
                }
                catch (Exception error)
                {
                    table.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                }
                finally
                {
                    if (updateTask != null) updateTask.Push();
                }
            }
        }
        /// <summary>
        /// 检测更新处理
        /// </summary>
        /// <param name="connection"></param>
        private void check(ref DbConnection connection)
        {
            //if (isDisposed == 0)
            //{
            //    second = timeout;
            //    if (isDisposed == 0 || Interlocked.Exchange(ref second, 0) == 0) return;
            //}
            if (Interlocked.Decrement(ref updateTaskCount) == 0)
            {
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
                        ReturnType returnType = client.Update(table, updateValue, memberMap, ref query);
                        if (returnType == ReturnType.Success) returnType = client.Update(table, ref connection, updateValue, memberMap, ref query, true);
                        //if (returnType != ReturnType.Success) table.Log.Add(LogLevel.Error, error);
                    }
                    catch (Exception error)
                    {
                        table.Log.Exception(error, null, LogLevel.Exception | LogLevel.AutoCSer);
                    }
                    finally { query.ClearSql(); }
                }
                while (true);
            }
        }
    }
}

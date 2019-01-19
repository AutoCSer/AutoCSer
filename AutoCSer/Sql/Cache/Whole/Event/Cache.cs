using System;
using System.Collections.Generic;
using AutoCSer.Metadata;
using System.Linq.Expressions;
using AutoCSer.Extension;
using System.Data.Common;
using System.Reflection;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 事件缓存
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    public abstract class Cache<valueType, modelType> : Copy<valueType, modelType>
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 重置缓存
        /// </summary>
        private sealed class ResetTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// SQL 表格缓存
            /// </summary>
            private Cache<valueType, modelType> cache;
            /// <summary>
            /// 等待缓存加载
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 异常信息
            /// </summary>
            private Exception exception;
            /// <summary>
            /// 查询信息
            /// </summary>
            internal SelectQuery<modelType> Query;
            /// <summary>
            /// 重置缓存
            /// </summary>
            /// <param name="cache"></param>
            /// <param name="where"></param>
            internal ResetTask(Cache<valueType, modelType> cache, Expression<Func<modelType, bool>> where)
            {
                byte isQuery = 0;
                try
                {
                    CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(where);
                    (this.cache = cache).SqlTable.GetSelect(cache.MemberMap, ref createQuery, ref Query);
                    isQuery = 1;
                    wait.Set(0);
                }
                finally
                {
                    if (isQuery == 0) Query.Free();
                }
            }
            /// <summary>
            /// 重置缓存
            /// </summary>
            /// <param name="connection"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    cache.Reset(ref connection, ref Query);
                }
                catch (Exception error)
                {
                    exception = error;
                }
                finally { wait.Set(); }
                return LinkNext;
            }
            /// <summary>
            /// 等待缓存加载
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Wait()
            {
                wait.Wait();
                if (exception != null) throw exception;
            }
        }
#if NOJIT
        /// <summary>
        /// 计算列加载完成字段信息
        /// </summary>
        private readonly FieldInfo logProxyLoadedField;
        /// <summary>
        /// 计算列加载完成
        /// </summary>
        private object isLogProxyLoaded;
#else
        /// <summary>
        /// 设置计算列加载完成
        /// </summary>
        private readonly Action<valueType> setIsLogProxyLoaded;
        /// <summary>
        /// 计算列加载完成
        /// </summary>
        private bool isLogProxyLoaded;
#endif
        /// <summary>
        /// 数据集合
        /// </summary>
        public abstract IEnumerable<valueType> Values { get; }
        /// <summary>
        /// 数据数量
        /// </summary>
        internal abstract int ValueCount { get; }
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="table">SQL操作工具</param>
        /// <param name="group">数据分组</param>
        protected Cache(Sql.Table<valueType, modelType> table, int group) : base(table, group)
        {
            table.IsOnlyQueue = true;
            if (group == 0)
            {
#if NOJIT
                logProxyLoadedField = typeof(modelType).GetField(LogStream.Log.IsSqlLogProxyLoadedName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (logProxyLoadedField != null) table.OnLogMemberLoaded += onLogMemberLoaded;
#else
                FieldInfo logProxyLoadedField = typeof(modelType).GetField(LogStream.Log.IsSqlLogProxyLoadedName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (logProxyLoadedField != null)
                {
                    DynamicMethod dynamicMethod = new DynamicMethod("SetIsLogProxyLoaded", null, new System.Type[] { typeof(modelType) }, typeof(modelType), true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.int32(1);
                    generator.Emit(OpCodes.Stfld, logProxyLoadedField);
                    generator.Emit(OpCodes.Ret);
                    setIsLogProxyLoaded = (Action<modelType>)dynamicMethod.CreateDelegate(typeof(Action<modelType>));
                    table.OnLogMemberLoaded += onLogMemberLoaded;
                }
#endif
            }
        }
        /// <summary>
        /// 重置缓存
        /// </summary>
        /// <param name="expression"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void reset(Expression<Func<modelType, bool>> expression)
        {
            ResetTask task = new ResetTask(this, expression);
            SqlTable.AddQueue(task);
            task.Wait();
        }
        /// <summary>
        /// 重置缓存
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        internal abstract void Reset(ref DbConnection connection, ref SelectQuery<modelType> query);
        /// <summary>
        /// 计算列加载完成事件
        /// </summary>
        private void onLogMemberLoaded()
        {
            isLogProxyLoaded = true;
            foreach (valueType value in Values) callSetIsLogProxyLoaded(value);
        }
        /// <summary>
        /// 设置计算列加载完成
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void callSetIsLogProxyLoaded(valueType value)
        {
            
#if NOJIT
            if (isLogProxyLoaded != null) logProxyLoadedField.SetValue(value, true);
#else
            if (isLogProxyLoaded) setIsLogProxyLoaded(value);
#endif
        }
        /// <summary>
        /// 添加记录事件
        /// </summary>
        public event Action<valueType> OnInserted;
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="value">新添加的对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void callOnInserted(valueType value)
        {
            if (OnInserted != null) OnInserted(value);
        }
        /// <summary>
        /// 更新记录事件 [缓存数据 + 更新后的数据 + 更新前的数据 + 更新数据成员]
        /// </summary>
        public event OnCacheUpdated OnUpdated;
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="cacheValue"></param>
        /// <param name="value">更新后的对象</param>
        /// <param name="oldValue">更新前的对象</param>
        /// <param name="memberMap"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void callOnUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            if (OnUpdated != null) OnUpdated(cacheValue, value, oldValue, memberMap);
        }
        /// <summary>
        /// 删除记录事件
        /// </summary>
        public event Action<valueType> OnDeleted;
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="value">被删除的对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void callOnDeleted(valueType value)
        {
            if (OnDeleted != null) OnDeleted(value);
        }
        /// <summary>
        /// 缓存数据加载完成
        /// </summary>
        /// <param name="onInserted">添加记录事件</param>
        /// <param name="onUpdated">更新记录事件</param>
        /// <param name="onDeleted">删除记录事件</param>
        /// <param name="isLoadMemberCache">是否加载缓存依赖类型</param>
        /// <param name="isSqlStreamTypeCount">是否日志流计数完成类型注册</param>
        public void Loaded(Action<valueType> onInserted = null, OnCacheUpdated onUpdated = null, Action<valueType> onDeleted = null, bool isLoadMemberCache = true, bool isSqlStreamTypeCount = true)
        {
            if (onInserted != null) OnInserted += onInserted;
            if (onUpdated != null) OnUpdated += onUpdated;
            if (onDeleted != null) OnDeleted += onDeleted;
            SqlTable.CacheLoaded(isLoadMemberCache, isSqlStreamTypeCount);
        }
        /// <summary>
        /// 成员绑定缓存集合
        /// </summary>
        protected LeftArray<object> memberCaches;
        /// <summary>
        /// 创建关键字整表缓存
        /// </summary>
        /// <typeparam name="primaryKey"></typeparam>
        /// <param name="getValue">根据关键字获取数据</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="group">数据分组</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public Event.MemberKey<valueType, modelType, primaryKey> CreateMemberPrimaryKey<primaryKey>(Func<primaryKey, valueType> getValue, Expression<Func<valueType, valueType>> member, int group = 1, bool isSave = true)
            where primaryKey : struct, IEquatable<primaryKey>
        {
            Event.MemberKey<valueType, modelType, primaryKey> cache = new Event.MemberKey<valueType, modelType, primaryKey>(this, ((Sql.Table<valueType, modelType, primaryKey>)SqlTable).GetPrimaryKey, getValue, member, group);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组列表缓存
        /// </summary>
        /// <typeparam name="keyType">分组字典关键字类型</typeparam>
        /// <typeparam name="targetType">目标表格类型</typeparam>
        /// <typeparam name="targetModelType">目标模型类型</typeparam>
        /// <typeparam name="targetMemberCacheType">目标缓存绑定类型</typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberList<valueType, modelType, keyType, targetMemberCacheType> CreateMemberList<keyType, targetType, targetModelType, targetMemberCacheType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey
            , Expression<Func<targetMemberCacheType, ListArray<valueType>>> member
            , bool isRemoveEnd = false, bool isReset = true, bool isSave = true)
            where keyType : IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
        {
            MemberList<valueType, modelType, keyType, targetMemberCacheType> cache = new MemberList<valueType, modelType, keyType, targetMemberCacheType>(this, getKey, targetCache.GetMemberCacheByKey, member, targetCache.GetAllMemberCache, isRemoveEnd, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组列表缓存
        /// </summary>
        /// <typeparam name="keyType">分组字典关键字类型</typeparam>
        /// <typeparam name="targetType">目标表格类型</typeparam>
        /// <typeparam name="targetModelType">目标模型类型</typeparam>
        /// <typeparam name="targetMemberCacheType">目标缓存绑定类型</typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberListWhere<valueType, modelType, keyType, targetMemberCacheType> CreateMemberListWhere<keyType, targetType, targetModelType, targetMemberCacheType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey
            , Expression<Func<targetMemberCacheType, ListArray<valueType>>> member, Func<valueType, bool> isValue
            , bool isRemoveEnd = false, bool isSave = true)
            where keyType : IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
        {
            MemberListWhere<valueType, modelType, keyType, targetMemberCacheType> cache = new MemberListWhere<valueType, modelType, keyType, targetMemberCacheType>(this, getKey, targetCache.GetMemberCacheByKey, member, targetCache.GetAllMemberCache, isValue, isRemoveEnd);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 分组列表 延时排序缓存
        /// </summary>
        /// <typeparam name="keyType">分组字典关键字类型</typeparam>
        /// <typeparam name="targetType">目标表格类型</typeparam>
        /// <typeparam name="targetModelType">目标模型类型</typeparam>
        /// <typeparam name="targetMemberCacheType">目标缓存绑定类型</typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="sorter">数据排序</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberLazyOrderArray<valueType, modelType, keyType, targetMemberCacheType> CreateMemberLazyOrderArray<keyType, targetType, targetModelType, targetMemberCacheType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey
            , Expression<Func<targetMemberCacheType, LazyOrderArray<valueType>>> member, Func<LeftArray<valueType>, LeftArray<valueType>> sorter
            , bool isSave = false)
            where keyType : IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
        {
            MemberLazyOrderArray<valueType, modelType, keyType, targetMemberCacheType> cache = new MemberLazyOrderArray<valueType, modelType, keyType, targetMemberCacheType>(this, getKey, targetCache.GetMemberCacheByKey, member, targetCache.GetAllMemberCache, sorter, true);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 分组列表 延时排序缓存
        /// </summary>
        /// <typeparam name="keyType">分组字典关键字类型</typeparam>
        /// <typeparam name="targetType">目标表格类型</typeparam>
        /// <typeparam name="targetModelType">目标模型类型</typeparam>
        /// <typeparam name="targetMemberCacheType">目标缓存绑定类型</typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="sorter">数据排序</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberLazyOrderArrayWhere<valueType, modelType, keyType, targetMemberCacheType> CreateMemberLazyOrderArrayWhere<keyType, targetType, targetModelType, targetMemberCacheType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey
            , Expression<Func<targetMemberCacheType, LazyOrderArray<valueType>>> member, Func<LeftArray<valueType>, LeftArray<valueType>> sorter, Func<valueType, bool> isValue
            , bool isSave = false)
            where keyType : IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
        {
            MemberLazyOrderArrayWhere<valueType, modelType, keyType, targetMemberCacheType> cache = new MemberLazyOrderArrayWhere<valueType, modelType, keyType, targetMemberCacheType>(this, getKey, targetCache.GetMemberCacheByKey, member, targetCache.GetAllMemberCache, sorter, isValue);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组列表 延时排序缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="targetType"></typeparam>
        /// <typeparam name="targetModelType"></typeparam>
        /// <typeparam name="targetMemberCacheType"></typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="sorter">排序器</param>
        /// <param name="isReset">是否初始化</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberOrderList<valueType, modelType, keyType, targetMemberCacheType> CreateMemberOrderList<keyType, targetType, targetModelType, targetMemberCacheType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey, Expression<Func<targetMemberCacheType, ListArray<valueType>>> member
            , Func<LeftArray<valueType>, LeftArray<valueType>> sorter, bool isReset, bool isSave = false)
            where keyType : IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
        {
            MemberOrderList<valueType, modelType, keyType, targetMemberCacheType> cache = new MemberOrderList<valueType, modelType, keyType, targetMemberCacheType>(this, getKey, targetCache.GetMemberCacheByKey, member, targetCache.GetAllMemberCache, sorter, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组字典缓存
        /// </summary>
        /// <typeparam name="keyType">分组字典关键字类型</typeparam>
        /// <typeparam name="targetType">目标表格类型</typeparam>
        /// <typeparam name="targetModelType">目标模型类型</typeparam>
        /// <typeparam name="targetMemberCacheType">目标缓存绑定类型</typeparam>
        /// <typeparam name="valueKeyType">目标数据关键字类型</typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="getValueKey">获取数据关键字委托</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberDictionary<valueType, modelType, keyType, valueKeyType, targetMemberCacheType> CreateMemberDictionary<keyType, targetType, targetModelType, targetMemberCacheType, valueKeyType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey
            , Expression<Func<targetMemberCacheType, Dictionary<RandomKey<valueKeyType>, valueType>>> member
            , Func<modelType, valueKeyType> getValueKey, bool isReset = true, bool isSave = true)
            where keyType : IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
            where valueKeyType : IEquatable<valueKeyType>
        {
            MemberDictionary<valueType, modelType, keyType, valueKeyType, targetMemberCacheType> cache = new MemberDictionary<valueType, modelType, keyType, valueKeyType, targetMemberCacheType>(this, getKey, targetCache.GetMemberCacheByKey, member, targetCache.GetAllMemberCache, getValueKey, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组列表缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="targetType"></typeparam>
        /// <typeparam name="targetModelType"></typeparam>
        /// <typeparam name="targetMemberCacheType"></typeparam>
        /// <typeparam name="memberKeyType"></typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getMemberKey">分组列表关键字获取器</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberDictionaryListWhere<valueType, modelType, keyType, memberKeyType, targetMemberCacheType> CreateMemberDictionaryListWhere<keyType, targetType, targetModelType, targetMemberCacheType, memberKeyType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey, Func<modelType, memberKeyType> getMemberKey
            , Expression<Func<targetMemberCacheType, Dictionary<RandomKey<memberKeyType>, ListArray<valueType>>>> member, Func<valueType, bool> isValue, bool isRemoveEnd, bool isSave = true)
            where keyType : struct, IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
            where memberKeyType : struct, IEquatable<memberKeyType>
        {
            MemberDictionaryListWhere<valueType, modelType, keyType, memberKeyType, targetMemberCacheType> cache = new MemberDictionaryListWhere<valueType, modelType, keyType, memberKeyType, targetMemberCacheType>(this, getKey, getMemberKey, targetCache.GetMemberCacheByKey, member, targetCache.GetAllMemberCache, isValue, isRemoveEnd);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组列表 延时排序缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="targetType"></typeparam>
        /// <typeparam name="targetModelType"></typeparam>
        /// <typeparam name="targetMemberCacheType"></typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getIndex">获取数组索引</param>
        /// <param name="arraySize">数组容器大小</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="sorter">排序器</param>
        /// <param name="isReset">是否初始化</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberArrayLazyOrderArray<valueType, modelType, keyType, targetMemberCacheType> CreateMemberArrayLazyOrderArray<keyType, targetType, targetModelType, targetMemberCacheType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey
            , Func<valueType, int> getIndex, int arraySize, Expression<Func<targetMemberCacheType, LazyOrderArray<valueType>[]>> member
            , Func<LeftArray<valueType>, LeftArray<valueType>> sorter, bool isReset = true, bool isSave = false)
            where keyType : IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
        {
            MemberArrayLazyOrderArray<valueType, modelType, keyType, targetMemberCacheType> cache = new MemberArrayLazyOrderArray<valueType, modelType, keyType, targetMemberCacheType>(this, getKey, targetCache.GetMemberCacheByKey, getIndex, arraySize, member, targetCache.GetAllMemberCache, sorter, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组列表缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <typeparam name="targetType"></typeparam>
        /// <typeparam name="targetModelType"></typeparam>
        /// <typeparam name="targetMemberCacheType"></typeparam>
        /// <param name="targetCache">目标缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getIndex">获取数组索引</param>
        /// <param name="arraySize">数组容器大小</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public MemberArrayList<valueType, modelType, keyType, targetMemberCacheType> CreateMemberArrayList<keyType, targetType, targetModelType, targetMemberCacheType>
            (Key<targetType, targetModelType, targetMemberCacheType, keyType> targetCache, Func<modelType, keyType> getKey, Func<valueType, int> getIndex, int arraySize
            , Expression<Func<targetMemberCacheType, ListArray<valueType>[]>> member, bool isRemoveEnd = false, bool isReset = true, bool isSave = true)
            where keyType : struct, IEquatable<keyType>
            where targetType : class, targetModelType
            where targetModelType : class
            where targetMemberCacheType : class
        {
            MemberArrayList<valueType, modelType, keyType, targetMemberCacheType> cache = new MemberArrayList<valueType, modelType, keyType, targetMemberCacheType>(this, getKey, getIndex, arraySize, targetCache.GetMemberCacheByKey, member, targetCache.GetAllMemberCache, isRemoveEnd, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }

        /// <summary>
        /// 创建分组字典缓存
        /// </summary>
        /// <typeparam name="groupKeyType"></typeparam>
        /// <typeparam name="keyType"></typeparam>
        /// <param name="getGroupKey">分组关键字获取器</param>
        /// <param name="getKey">字典关键字获取器</param>
        /// <param name="isReset">是否初始化数据</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public DictionaryDictionary<valueType, modelType, groupKeyType, keyType> CreateDictionaryDictionary<groupKeyType, keyType>(Func<valueType, groupKeyType> getGroupKey, Func<valueType, keyType> getKey, bool isReset = true, bool isSave = false)
            where groupKeyType : IEquatable<groupKeyType>
            where keyType : IEquatable<keyType>
        {
            DictionaryDictionary<valueType, modelType, groupKeyType, keyType> cache = new DictionaryDictionary<valueType, modelType, groupKeyType, keyType>(this, getGroupKey, getKey, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组字典缓存
        /// </summary>
        /// <typeparam name="keyType">分组关键字类型</typeparam>
        /// <typeparam name="sortType">排序关键字类型</typeparam>
        /// <param name="getKey">分组关键字获取器</param>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isReset">是否初始化数据</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public DictionarySearchTreeDictionary<valueType, modelType, keyType, sortType> CreateDictionarySearchTreeDictionary<keyType, sortType>(Func<valueType, keyType> getKey, Func<valueType, sortType> getSort, bool isReset = true, bool isSave = false)
            where keyType : IEquatable<keyType>
            where sortType : IComparable<sortType>
        {
            DictionarySearchTreeDictionary<valueType, modelType, keyType, sortType> cache = new DictionarySearchTreeDictionary<valueType, modelType, keyType, sortType>(this, getKey, getSort, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建分组列表缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <param name="getKey">字典关键字获取器</param>
        /// <param name="isRemoveEnd">分组关键字获取器</param>
        /// <param name="isReset">是否初始化数据</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public DictionaryList<valueType, modelType, keyType> CreateDictionaryList<keyType>(Func<valueType, keyType> getKey, bool isRemoveEnd = false, bool isReset = true, bool isSave = false)
            where keyType : IEquatable<keyType>
        {
            DictionaryList<valueType, modelType, keyType> cache = new DictionaryList<valueType, modelType, keyType>(this, getKey, isRemoveEnd, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建数组列表缓存
        /// </summary>
        /// <param name="getIndex">数组索引获取器</param>
        /// <param name="arraySize">数组容器大小</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public ArrayList<valueType, modelType> CreateArrayList(Func<valueType, int> getIndex, int arraySize, bool isRemoveEnd = false, bool isReset = true, bool isSave = false)
        {
            ArrayList<valueType, modelType> cache = new ArrayList<valueType, modelType>(this, getIndex, arraySize, isRemoveEnd, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建字典缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isReset">是否初始化</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public DictionaryArray<valueType, modelType, keyType> CreateDictionaryArray<keyType>(Func<valueType, keyType> getKey, bool isReset = true, bool isSave = false)
            where keyType : IEquatable<keyType>
        {
            DictionaryArray<valueType, modelType, keyType> cache = new DictionaryArray<valueType, modelType, keyType>(this, getKey, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建字典缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public DictionaryArrayWhere<valueType, modelType, keyType> CreateDictionaryArrayWhere<keyType>(Func<valueType, keyType> getKey, Func<valueType, bool> isValue, bool isSave = false)
            where keyType : IEquatable<keyType>
        {
            DictionaryArrayWhere<valueType, modelType, keyType> cache = new DictionaryArrayWhere<valueType, modelType, keyType>(this, getKey, isValue);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建字典缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isReset">是否初始化</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public Dictionary<valueType, modelType, keyType> CreateDictionary<keyType>(Func<valueType, keyType> getKey, bool isReset = true, bool isSave = false)
            where keyType : IEquatable<keyType>
        {
            Dictionary<valueType, modelType, keyType> cache = new Dictionary<valueType, modelType, keyType>(this, getKey, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建字典缓存
        /// </summary>
        /// <typeparam name="keyType"></typeparam>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public DictionaryWhere<valueType, modelType, keyType> CreateDictionaryWhere<keyType>(Func<valueType, keyType> getKey, Func<valueType, bool> isValue, bool isSave = false)
            where keyType : IEquatable<keyType>
        {
            DictionaryWhere<valueType, modelType, keyType> cache = new DictionaryWhere<valueType, modelType, keyType>(this, getKey, isValue);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建搜索树缓存
        /// </summary>
        /// <typeparam name="sortType">排序关键字类型</typeparam>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns></returns>
        public SearchTreeDictionaryWhere<valueType, modelType, sortType> CreateSearchTreeWhere<sortType>(Func<valueType, sortType> getSort, Func<valueType, bool> isValue)
            where sortType : IComparable<sortType>
        {
            return new SearchTreeDictionaryWhere<valueType, modelType, sortType>(this, getSort, isValue);
        }
        /// <summary>
        /// 创建搜索树缓存
        /// </summary>
        /// <typeparam name="sortType1">排序关键字类型</typeparam>
        /// <typeparam name="sortType2">排序关键字类型</typeparam>
        /// <param name="getSort1">排序关键字获取器</param>
        /// <param name="getSort2">排序关键字获取器</param>
        /// <param name="isValue">数据匹配器</param>
        /// <returns></returns>
        public SearchTreeDictionaryWhere<valueType, modelType, AutoCSer.Data.PrimaryKey<sortType1, sortType2>> CreateSearchTreeWhere<sortType1, sortType2>(Func<valueType, sortType1> getSort1, Func<valueType, sortType2> getSort2, Func<valueType, bool> isValue)
            where sortType1 : IEquatable<sortType1>, IComparable<sortType1>
            where sortType2 : IEquatable<sortType2>, IComparable<sortType2>
        {
            return new SearchTreeDictionaryWhere<valueType, modelType, AutoCSer.Data.PrimaryKey<sortType1, sortType2>>(this, value => new AutoCSer.Data.PrimaryKey<sortType1, sortType2> { Key1 = getSort1(value), Key2 = getSort2(value) }, isValue);
        }
        /// <summary>
        /// 创建数组+搜索树缓存
        /// </summary>
        /// <typeparam name="sortType"></typeparam>
        /// <param name="getIndex">数组索引获取器</param>
        /// <param name="arraySize">数组容器大小</param>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isReset">是否初始化</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public ArraySearchTreeDictionary<valueType, modelType, sortType> CreateArraySearchTreeDictionary<sortType>(Func<valueType, int> getIndex, int arraySize, Func<valueType, sortType> getSort, bool isReset = true, bool isSave = false)
            where sortType : IComparable<sortType>
        {
            ArraySearchTreeDictionary<valueType, modelType, sortType> cache = new ArraySearchTreeDictionary<valueType, modelType, sortType>(this, getIndex, arraySize, getSort, isReset);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
        /// <summary>
        /// 创建数组+搜索树缓存
        /// </summary>
        /// <typeparam name="sortType"></typeparam>
        /// <param name="getIndex">数组索引获取器</param>
        /// <param name="arraySize">数组容器大小</param>
        /// <param name="getSort">排序关键字获取器</param>
        /// <param name="isValue">缓存值判定</param>
        /// <param name="isSave">是否保存缓存对象防止被垃圾回收</param>
        /// <returns></returns>
        public ArraySearchTreeDictionaryWhere<valueType, modelType, sortType> CreateArraySearchTreeWhere<sortType>(Func<valueType, int> getIndex, int arraySize, Func<valueType, sortType> getSort, Func<valueType, bool> isValue, bool isSave = false)
            where sortType : IComparable<sortType>
        {
            ArraySearchTreeDictionaryWhere<valueType, modelType, sortType> cache = new ArraySearchTreeDictionaryWhere<valueType, modelType, sortType>(this, getIndex, arraySize, getSort, isValue);
            if (isSave) memberCaches.Add(cache);
            return cache;
        }
    }
    /// <summary>
    /// 事件缓存
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    public abstract class Cache<valueType, modelType, memberCacheType> : Cache<valueType, modelType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
    {
        /// <summary>
        /// 获取成员缓存
        /// </summary>
        public readonly Func<valueType, memberCacheType> GetMemberCache;
        /// <summary>
        /// 设置成员缓存
        /// </summary>
        private readonly Action<valueType, memberCacheType> setMemberCache;
        /// <summary>
        /// 设置成员缓存数据
        /// </summary>
        private readonly Action<memberCacheType, valueType> setMemberCacheValue;
        /// <summary>
        /// 获取所有成员缓存
        /// </summary>
        public readonly Func<IEnumerable<memberCacheType>> GetAllMemberCache;
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="table">SQL操作工具</param>
        /// <param name="memberCache"></param>
        /// <param name="group">数据分组</param>
        protected Cache(Sql.Table<valueType, modelType> table, Expression<Func<valueType, memberCacheType>> memberCache, int group)
            : base(table, group)
        {
            if (group == 0)
            {
                if (memberCache == null)
                {
                    if (typeof(valueType) != typeof(memberCacheType))
                    {
                        FieldIndex memberField = null;
                        foreach (FieldIndex field in MemberIndexGroup<valueType>.GetFields(MemberFilters.NonPublicInstance))
                        {
                            if (field.Member.FieldType == typeof(memberCacheType))
                            {
                                if(memberField == null) memberField = field;
                                if (field.IsAttribute<MemberCacheAttribute>(false))
                                {
                                    memberField = field;
                                    break;
                                }
                            }
                        }
                        if (memberField == null) throw new InvalidCastException(typeof(valueType).fullName() + " != " + typeof(memberCacheType).fullName());
                        else
                        {
                            if (AutoCSer.Emit.Constructor<memberCacheType>.New == null) throw new InvalidOperationException("找不到无参构造函数 " + typeof(memberCacheType).FullName);
                            GetMemberCache = AutoCSer.Emit.Field.UnsafeGetField<valueType, memberCacheType>(memberField.Member);
                            setMemberCache = AutoCSer.Emit.Field.UnsafeSetField<valueType, memberCacheType>(memberField.Member);
                            GetAllMemberCache = getAllMemberCache;
                            setMemberCacheValue = AutoCSer.Emit.Field.UnsafeSetField<memberCacheType, valueType>("Value");
                        }
                    }
                    //GetAllValue = getAllValue;
                }
                else
                {
                    if (AutoCSer.Emit.Constructor<memberCacheType>.New == null) throw new InvalidOperationException("找不到无参构造函数 " + typeof(memberCacheType).FullName);
                    MemberExpression<valueType, memberCacheType> expression = new MemberExpression<valueType, memberCacheType>(memberCache);
                    if (expression.Field == null) throw new InvalidCastException("memberCache is not MemberExpression");
                    GetMemberCache = expression.GetMember;
                    setMemberCache = expression.SetMember;
                    GetAllMemberCache = getAllMemberCache;
                    setMemberCacheValue = AutoCSer.Emit.Field.UnsafeSetField<memberCacheType, valueType>("Value");
                }
            }
        }
        /// <summary>
        /// 设置成员缓存与数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setMemberCacheAndValue(valueType value)
        {
            if (setMemberCache != null)
            {
                if (setMemberCacheValue == null) setMemberCache(value, AutoCSer.Emit.Constructor<memberCacheType>.New());
                else
                {
                    memberCacheType memberCache = AutoCSer.Emit.Constructor<memberCacheType>.New();
                    setMemberCache(value, memberCache);
                    setMemberCacheValue(memberCache, value);
                }
            }
            callSetIsLogProxyLoaded(value);
        }
        /// <summary>
        /// 所有成员缓存
        /// </summary>
        private IEnumerable<memberCacheType> getAllMemberCache()
        {
            foreach (valueType value in Values)
            {
                yield return GetMemberCache(value);
            }
        }
    }
}

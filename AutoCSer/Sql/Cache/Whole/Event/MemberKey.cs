using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 关键字整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed class MemberKey<valueType, modelType, keyType> : Key<valueType, modelType, valueType, keyType>
        where valueType : class, modelType
        where modelType : class
        where keyType : struct, IEquatable<keyType>
    {
        /// <summary>
        /// 事件缓存
        /// </summary>
        private readonly Cache<valueType, modelType> cache;
        /// <summary>
        /// 获取数据
        /// </summary>
        private readonly Func<keyType, valueType> getValue;
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        private readonly Func<valueType, valueType> getMember;
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        private readonly Action<valueType, valueType> setMember;
        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable<valueType> Values
        {
            get
            {
                foreach (valueType target in cache.Values)
                {
                    valueType value = getMember(target);
                    if (value != null) yield return value;
                }
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据</returns>
        public override valueType this[keyType key]
        {
            get
            {
                valueType target = getValue(key);
                return target != null ? getMember(target) : null;
            }
        }
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 数据数量
        /// </summary>
        internal override int ValueCount { get { return Count; } }
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="getValue">根据关键字获取数据</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="group">数据分组</param>
        public MemberKey(Cache<valueType, modelType> cache, Func<modelType, keyType> getKey
            , Func<keyType, valueType> getValue, Expression<Func<valueType, valueType>> member, int group = 1)
            : base(cache.SqlTable, null, getKey, group)
        {
            if (getValue == null) throw new ArgumentNullException("getValue is null");
            if (member == null) throw new ArgumentNullException("member is null");
            MemberExpression<valueType, valueType> expression = new MemberExpression<valueType, valueType>(member);
            if (expression.Field == null) throw new InvalidCastException("member is not MemberExpression");
            this.cache = cache;
            this.getValue = getValue;
            getMember = expression.GetMember;
            setMember = expression.SetMember;

            cache.SqlTable.OnInserted += onInserted;
            cache.SqlTable.OnUpdated += onUpdated;
            cache.SqlTable.OnDeleted += onDeleted;

            reset(null);
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        internal override void Reset(ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            foreach (valueType value in SqlTable.SelectQueue(ref connection, ref query)) insert(value);
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void insert(valueType value)
        {
            setMember(getValue(GetKey(value)), value);
            ++Count;
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void onInserted(valueType value)
        {
            valueType newValue = AutoCSer.Emit.Constructor<valueType>.New();
            AutoCSer.MemberCopy.Copyer<modelType>.Copy(newValue, value, MemberMap);
            insert(newValue);
            callOnInserted(newValue);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private void onUpdated(valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            valueType cacheValue = getMember(getValue(GetKey(value)));
            if (cacheValue != null)
            {
                update(cacheValue, value, oldValue, memberMap);
                callOnUpdated(cacheValue, value, oldValue, memberMap);
            }
            else SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            valueType cacheValue = getMember(getValue(GetKey(value)));
            if (cacheValue != null)
            {
                --Count;
                callOnDeleted(cacheValue);
            }
            else SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
    }
    /// <summary>
    /// 关键字整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="memberKeyType"></typeparam>
    /// <typeparam name="targetType"></typeparam>
    public class MemberKey<valueType, modelType, memberCacheType, keyType, memberKeyType, targetType> : Cache<valueType, modelType, memberCacheType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
        where keyType : struct, IEquatable<keyType>
        where memberKeyType : struct, IEquatable<memberKeyType>
        where targetType : class
    {
        /// <summary>
        /// 获取关键字
        /// </summary>
        private Func<modelType, keyType> getKey;
        /// <summary>
        /// 获取关键字
        /// </summary>
        private Func<modelType, memberKeyType> getMemberKey;
        /// <summary>
        /// 获取数据
        /// </summary>
        private Func<keyType, targetType> getValue;
        /// <summary>
        /// 获取所有节点集合
        /// </summary>
        private Func<IEnumerable<targetType>> getTargets;
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        private Func<targetType, Dictionary<RandomKey<memberKeyType>, valueType>> getMember;
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        private Action<targetType, Dictionary<RandomKey<memberKeyType>, valueType>> setMember;
        /// <summary>
        /// 数据集合
        /// </summary>
        public override IEnumerable<valueType> Values
        {
            get
            {
                foreach (targetType target in getTargets())
                {
                    Dictionary<RandomKey<memberKeyType>, valueType> cache = getMember(target);
                    if (cache != null)
                    {
                        foreach (valueType value in cache.Values) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 数据数量
        /// </summary>
        internal override int ValueCount { get { return Count; } }
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberCache">成员缓存</param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="getMemberKey">成员缓存键值获取器</param>
        /// <param name="getValue"></param>
        /// <param name="member">缓存成员</param>
        /// <param name="getTargets"></param>
        /// <param name="group">数据分组</param>
        public MemberKey(Sql.Table<valueType, modelType> sqlTool, Expression<Func<valueType, memberCacheType>> memberCache
            , Func<modelType, keyType> getKey, Func<modelType, memberKeyType> getMemberKey, Func<keyType, targetType> getValue
            , Expression<Func<targetType, Dictionary<RandomKey<memberKeyType>, valueType>>> member
            , Func<IEnumerable<targetType>> getTargets, int group = 0)
            : base(sqlTool, memberCache, group)
        {
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            if (getMemberKey == null) throw new ArgumentNullException("getMemberKey is null");
            if (getValue == null) throw new ArgumentNullException("getValue is null");
            if (getTargets == null) throw new ArgumentNullException("getTargets is null");
            if (member == null) throw new ArgumentNullException("member is null");
            MemberExpression<targetType, Dictionary<RandomKey<memberKeyType>, valueType>> expression = new MemberExpression<targetType, Dictionary<RandomKey<memberKeyType>, valueType>>(member);
            if (expression.Field == null) throw new InvalidCastException("member is not MemberExpression");
            this.getKey = getKey;
            this.getMemberKey = getMemberKey;
            this.getValue = getValue;
            this.getTargets = getTargets;
            getMember = expression.GetMember;
            setMember = expression.SetMember;

            sqlTool.OnInserted += onInserted;
            sqlTool.OnUpdated += onUpdated;
            sqlTool.OnDeleted += onDeleted;

            reset(null);
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        internal override void Reset(ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            foreach (valueType value in SqlTable.SelectQueue(ref connection, ref query)) insert(value);
        }
        /// <summary>
        /// 缺少目标数据错误数量
        /// </summary>
        public int MissTargetCount { get; private set; }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void insert(valueType value)
        {
            targetType target = getValue(getKey(value));
            if (target == null)
            {
                ++MissTargetCount;
                SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, "没有找到目标数据 " + typeof(targetType).FullName + "." + getKey(value).ToString());
            }
            else
            {
                Dictionary<RandomKey<memberKeyType>, valueType> cache = getMember(target);
                if (cache == null) cache = DictionaryCreator<RandomKey<memberKeyType>>.Create<valueType>();
                setMemberCacheAndValue(value);
                cache.Add(getMemberKey(value), value);
                setMember(target, cache);
                ++Count;
            }
        }
        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="value">新增的数据</param>
        private void onInserted(valueType value)
        {
            valueType newValue = AutoCSer.Emit.Constructor<valueType>.New();
            AutoCSer.MemberCopy.Copyer<modelType>.Copy(newValue, value, MemberMap);
            insert(newValue);
            callOnInserted(newValue);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        private void onUpdated(valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            Dictionary<RandomKey<memberKeyType>, valueType> cache = getMember(getValue(getKey(value)));
            valueType cacheValue;
            if (cache != null && cache.TryGetValue(getMemberKey(value), out cacheValue))
            {
                update(cacheValue, value, oldValue, memberMap);
                callOnUpdated(cacheValue, value, oldValue, memberMap);
            }
            else SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        private void onDeleted(valueType value)
        {
            Dictionary<RandomKey<memberKeyType>, valueType> cache = getMember(getValue(getKey(value)));
            memberKeyType memberKey = getMemberKey(value);
            valueType cacheValue;
            if (cache != null && cache.TryGetValue(memberKey, out cacheValue))
            {
                cache.Remove(memberKey);
                callOnDeleted(cacheValue);
                --Count;
            }
            else SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
    }
    /// <summary>
    /// 关键字整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="memberKeyType"></typeparam>
    /// <typeparam name="targetType"></typeparam>
    public sealed class MemberKey<valueType, modelType, keyType, memberKeyType, targetType> : MemberKey<valueType, modelType, valueType, keyType, memberKeyType, targetType>
        where valueType : class, modelType
        where modelType : class
        where keyType : struct, IEquatable<keyType>
        where memberKeyType : struct, IEquatable<memberKeyType>
        where targetType : class
    {
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="getKey">键值获取器</param>
        /// <param name="getMemberKey">成员缓存键值获取器</param>
        /// <param name="getValue"></param>
        /// <param name="member">缓存成员</param>
        /// <param name="getTargets"></param>
        /// <param name="group">数据分组</param>
        public MemberKey(Sql.Table<valueType, modelType> sqlTool, Func<modelType, keyType> getKey, Func<modelType, memberKeyType> getMemberKey, Func<keyType, targetType> getValue
            , Expression<Func<targetType, Dictionary<RandomKey<memberKeyType>, valueType>>> member
            , Func<IEnumerable<targetType>> getTargets, int group = 0)
            : base(sqlTool, null, getKey, getMemberKey, getValue, member, getTargets, group)
        {
        }
    }
}

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
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class PrimaryKeyArray<valueType, modelType, memberCacheType, keyType> : Key<valueType, modelType, memberCacheType, keyType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 字典缓存数据
        /// </summary>
        private DictionaryArray<RandomKey<keyType>, valueType> dictionaryArray;
        /// <summary>
        /// 数据集合
        /// </summary>
        public override IEnumerable<valueType> Values
        {
            get
            {
                return dictionaryArray.Values;
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
                do
                {
                    valueType value = dictionaryArray[key];
                    if (value == null) return null;
                    if (GetKey(value).Equals(key)) return value;
                }
                while (true);
            }
        }
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count { get { return dictionaryArray.Count; } }
        /// <summary>
        /// 数据数量
        /// </summary>
        internal override int ValueCount { get { return dictionaryArray.Count; } }
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberCache">成员缓存</param>
        /// <param name="group">数据分组</param>
        public PrimaryKeyArray(Sql.Table<valueType, modelType, keyType> sqlTool, Expression<Func<valueType, memberCacheType>> memberCache = null, int group = 0)
            : base(sqlTool, memberCache, sqlTool.GetPrimaryKey, group)
        {
            dictionaryArray.Reset();
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
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void insert(valueType value)
        {
            setMemberCacheAndValue(value);
            dictionaryArray.Add(GetKey(value), value);
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
            valueType cacheValue = dictionaryArray[GetKey(value)];
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
            dictionaryArray.Remove(GetKey(value), out value);
            if (value != null) callOnDeleted(value); 
            else SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
    }
    /// <summary>
    /// 关键字整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed class PrimaryKeyArray<valueType, modelType, keyType> : PrimaryKeyArray<valueType, modelType, valueType, keyType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="group">数据分组</param>
        public PrimaryKeyArray(Sql.Table<valueType, modelType, keyType> sqlTool, int group = 0)
            : base(sqlTool, null, group)
        {
        }
    }
}

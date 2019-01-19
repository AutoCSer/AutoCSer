using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Data.Common;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 关键字整表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="memberCacheType">成员缓存类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class PrimaryKey<valueType, modelType, memberCacheType, keyType> : Key<valueType, modelType, memberCacheType, keyType>
        where valueType : class, modelType
        where modelType : class
        where memberCacheType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 字典缓存数据
        /// </summary>
        private readonly Dictionary<RandomKey<keyType>, valueType> dictionary;
        /// <summary>
        /// 数据集合
        /// </summary>
        public override IEnumerable<valueType> Values
        {
            get
            {
                return dictionary.Values;
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
                    valueType value;
                    if (dictionary.TryGetValue(key, out value))
                    {
                        if (GetKey(value).Equals(key)) return value;
                    }
                    else return null;
                }
                while (true);
            }
        }
        /// <summary>
        /// 缓存数据数量
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        internal override int ValueCount { get { return Count; } }
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberCache">成员缓存</param>
        /// <param name="group">数据分组</param>
        public PrimaryKey(Sql.Table<valueType, modelType, keyType> sqlTool, Expression<Func<valueType, memberCacheType>> memberCache = null, int group = 0)
            : base(sqlTool, memberCache, sqlTool.GetPrimaryKey, group)
        {
            sqlTool.OnInserted += onInserted;
            sqlTool.OnUpdated += onUpdated;
            sqlTool.OnDeleted += onDeleted;

            dictionary = DictionaryCreator<RandomKey<keyType>>.Create<valueType>();
            reset(null);
        }
        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        internal override void Reset(ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            foreach (valueType value in SqlTable.SelectQueue(ref connection, ref query))
            {
                setMemberCacheAndValue(value);
                dictionary[GetKey(value)] = value;
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
            setMemberCacheAndValue(newValue);
            dictionary.Add(GetKey(value), newValue);
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
            valueType cacheValue;
            if (dictionary.TryGetValue(GetKey(value), out cacheValue))
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
            valueType cacheValue;
            RandomKey<keyType> key = GetKey(value);
            if (dictionary.TryGetValue(key, out cacheValue))
            {
                dictionary.Remove(key);
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
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed class PrimaryKey<valueType, modelType, keyType> : PrimaryKey<valueType, modelType, valueType, keyType>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 关键字整表缓存
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="group">数据分组</param>
        public PrimaryKey(Sql.Table<valueType, modelType, keyType> sqlTool, int group = 0)
            : base(sqlTool, null, group)
        {
        }
    }
}

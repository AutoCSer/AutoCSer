using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoCSer.Extension;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 分组字典缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    /// <typeparam name="valueKeyType">目标数据关键字类型</typeparam>
    /// <typeparam name="targetType">目标数据类型</typeparam>
    public class MemberDictionary<valueType, modelType, keyType, valueKeyType, targetType> : Member<valueType, modelType, keyType, targetType, Dictionary<RandomKey<valueKeyType>, valueType>>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
        where targetType : class
        where valueKeyType : IEquatable<valueKeyType>
    {
        /// <summary>
        /// 获取数据关键字委托
        /// </summary>
        private readonly Func<modelType, valueKeyType> getValueKey;
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueKey"></param>
        /// <returns></returns>
        public valueType this[keyType key, valueKeyType valueKey]
        {
            get
            {
                targetType target = getValue(key);
                if (target != null)
                {
                    Dictionary<RandomKey<valueKeyType>, valueType> values = getMember(target);
                    if (values != null)
                    {
                        valueType value;
                        if (values.TryGetValue(valueKey, out value)) return value;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 分组字典缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getValue">获取目标对象委托</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="getTargets"></param>
        /// <param name="getValueKey">获取数据关键字委托</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        public MemberDictionary(Event.Cache<valueType, modelType> cache, Func<modelType, keyType> getKey
            , Func<keyType, targetType> getValue, Expression<Func<targetType, Dictionary<RandomKey<valueKeyType>, valueType>>> member
            , Func<IEnumerable<targetType>> getTargets, Func<modelType, valueKeyType> getValueKey, bool isReset = true)
            : base(cache, getKey, getValue, member, getTargets)
        {
            if (getValueKey == null) throw new ArgumentNullException();
            this.getValueKey = getValueKey;

            if (isReset)
            {
                foreach (valueType value in cache.Values) onInserted(value);
                cache.OnInserted += onInserted;
                cache.OnUpdated += onUpdated;
                cache.OnDeleted += onDeleted;
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onInserted(valueType value)
        {
            keyType key = getKey(value);
            onInserted(value, ref key);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="key"></param>
        protected void onInserted(valueType value, ref keyType key)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
            else
            {
                Dictionary<RandomKey<valueKeyType>, valueType> dictionary = getMember(target);
                if (dictionary == null) setMember(target, dictionary = DictionaryCreator<RandomKey<valueKeyType>>.Create<valueType>());
                dictionary.Add(getValueKey(value), value);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue"></param>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap"></param>
        protected void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            keyType oldKey = getKey(oldValue), newKey = getKey(value);
            if (newKey.Equals(oldKey))
            {
                valueKeyType oldValueKey = getValueKey(oldValue), newValueKey = getValueKey(value);
                if (!oldValueKey.Equals(newValueKey))
                {
                    targetType target = getValue(newKey);
                    if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + newKey.ToString());
                    else
                    {
                        Dictionary<RandomKey<valueKeyType>, valueType> dictionary = getMember(target);
                        if (dictionary != null)
                        {
                            if (dictionary.Remove(oldValueKey))
                            {
                                dictionary.Add(newValueKey, cacheValue);
                                return;
                            }
                            dictionary.Add(newValueKey, cacheValue);
                        }
                        cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                    }
                }
            }
            else
            {
                onInserted(cacheValue, ref newKey);
                onDeleted(oldValue, ref oldKey);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="key">被删除的数据关键字</param>
        protected void onDeleted(valueType value, ref keyType key)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
            else
            {
                Dictionary<RandomKey<valueKeyType>, valueType> dictionary = getMember(target);
                if (dictionary == null || !dictionary.Remove(getValueKey(value)))
                {
                    cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, (typeof(valueType).FullName + " 缓存同步错误"));
                }
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            keyType key = getKey(value);
            onDeleted(value, ref key);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Dictionary<RandomKey<valueKeyType>, valueType>.ValueCollection getCache(keyType key)
        {
            targetType target = getValue(key);
            if (target != null)
            {
                Dictionary<RandomKey<valueKeyType>, valueType> dictionary = getMember(target);
                if (dictionary != null) return dictionary.Values;
            }
            return null;
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] GetArray(keyType key)
        {
            return getCache(key).getArray();
        }
    }
}

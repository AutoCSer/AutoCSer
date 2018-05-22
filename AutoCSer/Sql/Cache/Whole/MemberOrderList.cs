using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using AutoCSer.Extension;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 分组列表排序缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    /// <typeparam name="targetType">目标数据类型</typeparam>
    public class MemberOrderList<valueType, modelType, keyType, targetType> : Member<valueType, modelType, keyType, targetType, ListArray<valueType>>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
        where targetType : class
    {
        /// <summary>
        /// 排序器
        /// </summary>
        private readonly Func<LeftArray<valueType>, LeftArray<valueType>> sorter;
        /// <summary>
        /// 分组列表 延时排序缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getKey">分组字典关键字获取器</param>
        /// <param name="getValue">获取目标对象委托</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="getTargets"></param>
        /// <param name="sorter">排序器</param>
        /// <param name="isReset">是否初始化</param>
        public MemberOrderList(Event.Cache<valueType, modelType> cache, Func<modelType, keyType> getKey
            , Func<keyType, targetType> getValue, Expression<Func<targetType, ListArray<valueType>>> member
            , Func<IEnumerable<targetType>> getTargets, Func<LeftArray<valueType>, LeftArray<valueType>> sorter, bool isReset)
            : base(cache, getKey, getValue, member, getTargets)
        {
            if (sorter == null) throw new ArgumentNullException();
            this.sorter = sorter;

            if (isReset)
            {
                HashSet<keyType> keys = HashSetCreator<keyType>.Create();
                foreach (valueType value in cache.Values)
                {
                    keyType key = getKey(value);
                    targetType target = getValue(key);
                    if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
                    else
                    {
                        ListArray<valueType> list = getMember(target);
                        if (list == null)
                        {
                            (list = new ListArray<valueType>()).Add(value);
                            setMember(target, list);
                        }
                        else
                        {
                            list.Add(value);
                            keys.Add(key);
                        }
                    }
                }
                foreach (keyType key in keys)
                {
                    ListArray<valueType> list = getMember(getValue(key));
                    list.Array = sorter(list.ToLeftArray()).Array.notNull();
                }
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
            onInserted(value, getKey(value));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="key"></param>
        protected void onInserted(valueType value, keyType key)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
            else
            {
                ListArray<valueType> list = getMember(target);
                if (list == null)
                {
                    (list = new ListArray<valueType>()).Add(value);
                    setMember(target, list);
                }
                else
                {
                    LeftArray<valueType> array = list.ToLeftArray();
                    array.Add(value);
                    setMember(target, new ListArray<valueType>(sorter(array)));
                }
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
            keyType key = getKey(value), oldKey = getKey(oldValue);
            if (key.Equals(oldKey))
            {
                targetType target = getValue(key);
                if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
                else
                {
                    ListArray<valueType> list = getMember(target);
                    if (list == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                    else setMember(target, new ListArray<valueType>(sorter(list.ToLeftArray())));
                }
            }
            else
            {
                onInserted(cacheValue, key);
                onDeleted(cacheValue, oldKey);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="key">被删除数据的关键字</param>
        protected void onDeleted(valueType value, keyType key)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
            else
            {
                ListArray<valueType> list = getMember(target);
                if (list == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                else
                {
                    valueType[] array = list.Array;
                    int index = System.Array.IndexOf(array, value, 0, list.Length);
                    if (index == -1) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                    else if (list.Length == 1) setMember(target, null);
                    else
                    {
                        valueType[] newArray = new valueType[list.Length - 1];
                        Array.Copy(array, 0, newArray, 0, index);
                        Array.Copy(array, index + 1, newArray, index, newArray.Length - index);
                        setMember(target, new ListArray<valueType>(newArray, true));
                    }
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
            onDeleted(value, getKey(value));
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public LeftArray<valueType> GetCache(keyType key)
        {
            targetType target = getValue(key);
            if (target != null) return new LeftArray<valueType>(getMember(target));
            return default(LeftArray<valueType>);
        }
    }
}

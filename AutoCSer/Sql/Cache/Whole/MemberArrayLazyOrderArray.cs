using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using AutoCSer.Extension;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using System.Data.Common;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 分组列表 延时排序缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    /// <typeparam name="keyType">分组字典关键字类型</typeparam>
    /// <typeparam name="targetType">目标数据类型</typeparam>
    public partial class MemberArrayLazyOrderArray<valueType, modelType, keyType, targetType> : Member<valueType, modelType, keyType, targetType, LazyOrderArray<valueType>[]>
        where valueType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
        where targetType : class
    {
        /// <summary>
        /// 数组索引获取器
        /// </summary>
        protected readonly Func<valueType, int> getIndex;
        /// <summary>
        /// 数组容器大小
        /// </summary>
        protected readonly int arraySize;
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
        /// <param name="getIndex">获取数组索引</param>
        /// <param name="arraySize">数组容器大小</param>
        /// <param name="member">缓存字段表达式</param>
        /// <param name="getTargets"></param>
        /// <param name="sorter">排序器</param>
        /// <param name="isReset">是否初始化</param>
        public MemberArrayLazyOrderArray(Event.Cache<valueType, modelType> cache, Func<modelType, keyType> getKey
            , Func<keyType, targetType> getValue, Func<valueType, int> getIndex, int arraySize, Expression<Func<targetType, LazyOrderArray<valueType>[]>> member
            , Func<IEnumerable<targetType>> getTargets, Func<LeftArray<valueType>, LeftArray<valueType>> sorter, bool isReset)
            : base(cache, getKey, getValue, member, getTargets)
        {
            if (getIndex == null) throw new ArgumentNullException("getIndex is null");
            if (sorter == null) throw new ArgumentNullException("sorter is null");
            if (arraySize <= 0) throw new IndexOutOfRangeException("arraySize[" + arraySize.toString() + "] <= 0");
            this.getIndex = getIndex;
            this.arraySize = arraySize;
            this.sorter = sorter;

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
            onInserted(value, getKey(value), getIndex(value));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        protected void onInserted(valueType value, keyType key, int index)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
            else
            {
                LazyOrderArray<valueType>[] arrays = getMember(target);
                LazyOrderArray<valueType> array;
                if (arrays == null)
                {
                    setMember(target, arrays = new LazyOrderArray<valueType>[arraySize]);
                    array = null;
                }
                else array = arrays[index];
                if (array == null) arrays[index] = array = new LazyOrderArray<valueType>();
                array.Insert(value);
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
            int index = getIndex(value), oldIndex = getIndex(oldValue);
            if (key.Equals(oldKey))
            {
                if (index != oldIndex)
                {
                    targetType target = getValue(key);
                    if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
                    else
                    {
                        LazyOrderArray<valueType>[] arrays = getMember(target);
                        if (arrays != null)
                        {
                            LazyOrderArray<valueType> array = arrays[index];
                            if (array == null) arrays[index] = array = new LazyOrderArray<valueType>();
                            array.Insert(cacheValue);
                            if ((array = arrays[oldIndex]) != null)
                            {
                                array.Delete(cacheValue);
                                return;
                            }
                        }
                        cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
                    }
                }
            }
            else
            {
                onInserted(cacheValue, key, index);
                onDeleted(cacheValue, oldKey, getIndex(oldValue));
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="key">被删除数据的关键字</param>
        /// <param name="index"></param>
        protected void onDeleted(valueType value, keyType key, int index)
        {
            targetType target = getValue(key);
            if (target == null) cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, typeof(valueType).FullName + " 没有找到缓存目标对象 " + key.ToString());
            else
            {
                LazyOrderArray<valueType>[] arrays = getMember(target);
                if (arrays != null)
                {
                    LazyOrderArray<valueType> array = arrays[index];
                    if (array != null)
                    {
                        array.Delete(value);
                        return;
                    }
                }
                cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            onDeleted(value, getKey(value), getIndex(value));
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private LazyOrderArray<valueType> getCache(keyType key, int index)
        {
            targetType target = getValue(key);
            if (target != null)
            {
                LazyOrderArray<valueType>[] arrays = getMember(target);
                if (arrays != null) return arrays[index];
            }
            return null;
        }
        /// <summary>
        /// 获取分页数据集合
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="index">数组索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">数据总数</param>
        /// <param name="isDesc">是否逆序</param>
        /// <returns>分页数据集合</returns>
        public valueType[] GetPage(keyType key, int index, int pageSize, int currentPage, out int count, bool isDesc = false)
        {
            LazyOrderArray<valueType> array = getCache(key, index);
            if (array != null)
            {
                Threading.LazyOrderArrayPageTask<valueType> task = new Threading.LazyOrderArrayPageTask<valueType>(pageSize, currentPage, isDesc, array, sorter);
                cache.SqlTable.AddQueue(task);
                return task.Wait(out count);
            }
            count = 0;
            return NullValue<valueType>.Array;
        }
    }
}

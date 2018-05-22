using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Cache.Whole
{
    /// <summary>
    /// 数组列表缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
    /// <typeparam name="modelType">表格模型类型</typeparam>
    public class ArrayList<valueType, modelType>
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 整表缓存
        /// </summary>
        protected readonly Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 数组索引获取器
        /// </summary>
        protected readonly Func<valueType, int> getIndex;
        /// <summary>
        /// 分组数据
        /// </summary>
        protected readonly ListArray<valueType>[] array;
        /// <summary>
        /// 移除数据并使用最后一个数据移动到当前位置
        /// </summary>
        protected readonly bool isRemoveEnd;
        /// <summary>
        /// 分组列表缓存
        /// </summary>
        /// <param name="cache">整表缓存</param>
        /// <param name="getIndex">数组索引获取器</param>
        /// <param name="arraySize">数组容器大小</param>
        /// <param name="isRemoveEnd">移除数据并使用最后一个数据移动到当前位置</param>
        /// <param name="isReset">是否绑定事件并重置数据</param>
        public ArrayList(Event.Cache<valueType, modelType> cache, Func<valueType, int> getIndex, int arraySize, bool isRemoveEnd, bool isReset)
        {
            if (cache == null) throw new ArgumentNullException("cache is null");
            if (getIndex == null) throw new ArgumentNullException("getIndex is null");
            array = new ListArray<valueType>[arraySize];
            this.cache = cache;
            this.getIndex = getIndex;
            this.isRemoveEnd = isRemoveEnd;

            if (isReset)
            {
                foreach (valueType value in cache.Values)
                {
                    int index = getIndex(value);
                    ListArray<valueType> list = array[index];
                    if (list == null) array[index] = list = new ListArray<valueType>();
                    list.Add(value);
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
            onInserted(value, getIndex(value));
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">数据对象</param>
        /// <param name="index"></param>
        private void onInserted(valueType value, int index)
        {
            ListArray<valueType> list = array[index];
            if (list == null) array[index] = list = new ListArray<valueType>();
            list.Add(value);
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
            int index = getIndex(value), oldIndex = getIndex(oldValue);
            if (index != oldIndex)
            {
                onInserted(cacheValue, index);
                onDeleted(cacheValue, oldIndex);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        /// <param name="index"></param>
        protected void onDeleted(valueType value, int index)
        {
            ListArray<valueType> list = array[index];
            if (list != null)
            {
                int valueIndex = Array.LastIndexOf(list.Array, value, list.Length - 1);
                if (valueIndex != -1)
                {
                    if (isRemoveEnd) list.RemoveAtEnd(valueIndex);
                    else list.RemoveAt(valueIndex);
                    return;
                }
            }
            cache.SqlTable.Log.Add(AutoCSer.Log.LogType.Fatal, typeof(valueType).FullName + " 缓存同步错误");
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">被删除的数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void onDeleted(valueType value)
        {
            onDeleted(value, getIndex(value));
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="index">数组索引</param>
        /// <param name="pageSize">分页长度</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">记录总数</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] GetPage(int index, int pageSize, int currentPage, out int count)
        {
            return new LeftArray<valueType>(array[index]).GetPage(pageSize, currentPage, out count);
        }
        /// <summary>
        /// 获取逆序分页数据
        /// </summary>
        /// <param name="index">数组索引</param>
        /// <param name="pageSize">分页长度</param>
        /// <param name="currentPage">分页页号</param>
        /// <param name="count">记录总数</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] GetPageDesc(int index, int pageSize, int currentPage, out int count)
        {
            return new LeftArray<valueType>(array[index]).GetPageDesc(pageSize, currentPage, out count);
        }
    }
}

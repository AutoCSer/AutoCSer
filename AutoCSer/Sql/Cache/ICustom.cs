using System;
using System.Collections.Generic;

namespace AutoCSer.Sql.Cache
{
    /// <summary>
    /// 自定义缓存
    /// </summary>
    /// <typeparam name="valueType">表格绑定类型</typeparam>
#if NOJIT
    public interface ICustom
    {
        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <returns>是否添加数据</returns>
        bool Add(object value);
        /// <summary>
        /// 更新缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <param name="oldValue">旧数据</param>
        /// <returns>添加数据返回正数，删除数据返回负数，没有变化返回0</returns>
        int Update(object value, object oldValue);
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <returns>是否存在被删除数据</returns>
        bool Remove(object value);
        /// <summary>
        /// 所有缓存数据
        /// </summary>
        IEnumerable<object> Values { get; }
    }
#else
    public interface ICustom<valueType>
    {
        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <returns>是否添加数据</returns>
        bool Add(valueType value);
        /// <summary>
        /// 更新缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <param name="oldValue">旧数据</param>
        /// <returns>添加数据返回正数，删除数据返回负数，没有变化返回0</returns>
        int Update(valueType value, valueType oldValue);
        /// <summary>
        /// 删除缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <returns>是否存在被删除数据</returns>
        bool Remove(valueType value);
        /// <summary>
        /// 所有缓存数据
        /// </summary>
        IEnumerable<valueType> Values { get; }
    }
#endif
}

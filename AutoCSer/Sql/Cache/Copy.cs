using System;
using AutoCSer.Metadata;
using System.Threading;
using System.Linq.Expressions;
using System.Data.Common;

namespace AutoCSer.Sql.Cache
{
    /// <summary>
    /// SQL 表格缓存（成员位图复制操作）
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    public abstract class Copy<valueType, modelType> : Table<valueType, modelType>
        where valueType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// SQL操作缓存
        /// </summary>
        /// <param name="table">SQL操作工具</param>
        /// <param name="group">数据分组</param>
        protected Copy(Sql.Table<valueType, modelType> table, int group) : base(table, group) { }
        /// <summary>
        /// 更新缓存数据
        /// </summary>
        /// <param name="value">缓存数据</param>
        /// <param name="newValue">更新后的新数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="updateMemberMap">更新成员位图</param>
        protected void update(valueType value, valueType newValue, valueType oldValue, MemberMap<modelType> updateMemberMap)
        {
            using (MemberMap<modelType> memberMap = MemberMap.Copy())
            {
                memberMap.And(updateMemberMap);
                AutoCSer.MemberCopy.Copyer<modelType>.Copy(value, newValue, memberMap);
                memberMap.Xor(MemberMap);
                memberMap.And(MemberMap);
                AutoCSer.MemberCopy.Copyer<modelType>.Copy(oldValue, value, memberMap);
                AutoCSer.MemberCopy.Copyer<modelType>.Copy(newValue, value, memberMap);
            }
        }
    }
}

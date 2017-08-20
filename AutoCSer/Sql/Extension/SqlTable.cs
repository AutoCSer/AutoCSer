using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数据库表格扩展
    /// </summary>
    public static class SqlTable
    {
        /// <summary>
        /// 创建成员位图
        /// </summary>
        /// <typeparam name="valueType">表格模型类型</typeparam>
        /// <param name="sqlTable">数据库表格操作工具</param>
        /// <returns>创建成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberMap<valueType>.Builder CreateMemberMap<valueType>(this AutoCSer.Sql.Table<valueType> sqlTable)
            where valueType : class
        {
            return new MemberMap<valueType>.Builder(sqlTable != null);
        }
        /// <summary>
        /// 创建成员索引
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="returnType"></typeparam>
        /// <param name="sqlTable">数据库表格操作工具</param>
        /// <param name="member">字段表达式</param>
        /// <returns>成员索引</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MemberMap<valueType>.MemberIndex CreateMemberIndex<valueType, returnType>(this AutoCSer.Sql.Table<valueType> sqlTable, Expression<Func<valueType, returnType>> member)
            where valueType : class
        {
            return MemberMap<valueType>.MemberIndex.Create(member);
        }
    }
}

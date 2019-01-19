using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 添加数据查询信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct InsertQuery
    {
        /// <summary>
        /// 查询语句
        /// </summary>
        internal string Sql;
        /// <summary>
        /// 添加语句
        /// </summary>
        internal string InsertSql;
        /// <summary>
        /// 仅添加不查询
        /// </summary>
        internal bool NotQuery;
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            InsertSql = Sql = null;
        }
    }
}

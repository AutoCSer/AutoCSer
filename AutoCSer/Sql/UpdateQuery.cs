using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 更新记录查询信息
    /// </summary>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct UpdateQuery<modelType>
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap<modelType> MemberMap;
        /// <summary>
        /// 查询语句
        /// </summary>
        internal string Sql;
        /// <summary>
        /// 更新语句
        /// </summary>
        internal string UpdateSql;
        /// <summary>
        /// 仅更新不查询
        /// </summary>
        internal bool NotQuery;
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            MemberMap = null;
            UpdateSql = Sql = null;
        }
        /// <summary>
        /// 清空 Sql 语句
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ClearSql()
        {
            UpdateSql = Sql = null;
        }
        /// <summary>
        /// 释放成员位图
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            if (MemberMap != null)
            {
                UpdateSql = Sql = null;
                MemberMap.Dispose();
                MemberMap = null;
            }
        }
    }
}

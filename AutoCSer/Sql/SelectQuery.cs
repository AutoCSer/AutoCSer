using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 查询信息
    /// </summary>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SelectQuery<modelType>
    {
        /// <summary>
        /// 成员位图
        /// </summary>
        internal MemberMap<modelType> MemberMap;
        /// <summary>
        /// SQL 语句
        /// </summary>
        internal string Sql;
        /// <summary>
        /// 跳过记录数量
        /// </summary>
        internal int SkipCount;
        /// <summary>
        /// 索引列名称
        /// </summary>
        internal string IndexFieldName;
        /// <summary>
        /// 索引列 Sql 名称
        /// </summary>
        internal string IndexFieldSqlName;
        /// <summary>
        /// 释放成员位图
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            if (MemberMap != null)
            {
                MemberMap.Dispose();
                MemberMap = null;
            }
        }
        /// <summary>
        /// 设置索引列名称
        /// </summary>
        /// <param name="fieldName">索引列名称</param>
        /// <param name="fieldSqlName">索引列 Sql 名称</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetIndex(string fieldName, string fieldSqlName)
        {
            IndexFieldName = fieldName;
            IndexFieldSqlName = fieldSqlName;
        }
        /// <summary>
        /// 设置索引列名称
        /// </summary>
        /// <param name="fieldName">索引列名称</param>
        /// <param name="fieldSqlName">索引列 Sql 名称</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetIndex(out string fieldName, out string fieldSqlName)
        {
            fieldName = IndexFieldName;
            fieldSqlName = IndexFieldSqlName;
            IndexFieldName = null;
            IndexFieldSqlName = null;
        }
    }
}

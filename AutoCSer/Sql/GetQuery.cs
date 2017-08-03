using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 单条记录查询信息
    /// </summary>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct GetQuery<modelType>
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
        /// 释放成员位图
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            if (MemberMap != null)
            {
                Sql = null;
                MemberMap.Dispose();
                MemberMap = null;
            }
        }
    }
}

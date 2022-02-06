using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Sql.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct OleDbCommand
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 数据库命令
        /// </summary>
        [FieldOffset(0)]
        public System.Data.OleDb.OleDbCommand Value;
    }
}

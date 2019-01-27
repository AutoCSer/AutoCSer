using Oracle.ManagedDataAccess.Client;
using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Sql.Oracle
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 数据库命令
        /// </summary>
        [FieldOffset(0)]
        public OracleCommand OracleCommand;
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        [FieldOffset(0)]
        public OracleConnection OracleConnection;
    }
}

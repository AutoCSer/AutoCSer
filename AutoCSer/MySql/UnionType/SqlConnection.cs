using System;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace AutoCSer.Sql.MySql.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct SqlConnection
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Object;
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        [FieldOffset(0)]
        public MySqlConnection Value;
    }
}

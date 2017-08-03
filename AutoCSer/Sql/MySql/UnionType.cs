using System;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace AutoCSer.Sql.MySql
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
        public MySqlCommand MySqlCommand;
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        [FieldOffset(0)]
        public MySqlConnection MySqlConnection;
    }
}

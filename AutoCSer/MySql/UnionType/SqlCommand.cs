using System;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace AutoCSer.Sql.MySql.UnionType
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct SqlCommand
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
        public MySqlCommand Value;
    }
}

using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Sql.UnionType
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
        public System.Data.SqlClient.SqlConnection Value;
    }
}

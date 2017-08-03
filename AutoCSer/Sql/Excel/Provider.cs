using System;

namespace AutoCSer.Sql.Excel
{
    /// <summary>
    /// Excel接口类型
    /// </summary>
    public enum Provider : byte
    {
        /// <summary>
        /// 未知接口类型
        /// </summary>
        Unknown,
        /// <summary>
        /// 只能操作Excel2007之前的.xls文件
        /// </summary>
        [Provider(Name = "Microsoft.Jet.OleDb.4.0", Excel = "Excel 8.0")]
        Jet4,
        /// <summary>
        /// 
        /// </summary>
        [Provider(Name = "Microsoft.ACE.OLEDB.12.0", Excel = "Excel 12.0")]
        Ace12,
    }
}

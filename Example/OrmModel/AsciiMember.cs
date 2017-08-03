using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 字节字符串
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class AsciiMember
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 字节字符串
        /// </summary>
        [AutoCSer.Sql.Member(IsAscii = true, MaxStringLength = 256)]
        public string Url;
    }
}

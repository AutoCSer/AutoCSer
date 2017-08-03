using System;

namespace AutoCSer.Example.OrmModel
{
    /// <summary>
    /// 枚举类型支持
    /// </summary>
    [AutoCSer.Sql.Model]
    public partial class EnumMember
    {
        /// <summary>
        /// 默认自增标识
        /// </summary>
        public int Id;

        /// <summary>
        /// 默认映射到 int 的枚举类型
        /// </summary>
        public enum IntEnum
        {
            Int0,
            Int1
        }
        /// <summary>
        /// 默认映射到 int 的枚举类型
        /// </summary>
        public IntEnum Int = IntEnum.Int1;

        /// <summary>
        /// 映射到 byte 的枚举类型
        /// </summary>
        public enum ByteEnum : byte
        {
            Byte0,
            Byte1
        }
        /// <summary>
        /// 映射到 byte 的枚举类型
        /// </summary>
        public ByteEnum Byte;
    }
}

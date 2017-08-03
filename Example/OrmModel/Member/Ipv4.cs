using System;

namespace AutoCSer.Example.OrmModel.Member
{
    /// <summary>
    /// 基于强制类型转换的自定义数据类型
    /// </summary>
    [AutoCSer.Sql.Member(DataType = typeof(int))]
    public struct Ipv4
    {
        /// <summary>
        /// IP 地址
        /// </summary>
        public uint IpAddress;
        /// <summary>
        /// 强制类型转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Ipv4(int value) { return new Ipv4 { IpAddress = (uint)value }; }
        /// <summary>
        /// 强制类型转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator int(Ipv4 value) { return (int)value.IpAddress; }
    }
}

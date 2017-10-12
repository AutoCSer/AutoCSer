using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.DataSetSerialize
{
    /// <summary>
    /// 数据源
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal struct DataSource
    {
        /// <summary>
        /// 数据流
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// 字符串集合
        /// </summary>
        public string[] Strings;
        /// <summary>
        /// 字节数组集合
        /// </summary>
        public byte[][] Bytes;
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="data">数据流</param>
        /// <param name="strings">字符串集合</param>
        /// <param name="bytes">字节数组集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] data, string[] strings, byte[][] bytes)
        {
            Data = data;
            Strings = strings;
            Bytes = bytes;
        }
    }
}

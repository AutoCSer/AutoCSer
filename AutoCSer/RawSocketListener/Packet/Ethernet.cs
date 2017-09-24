using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// 以太网数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Ethernet
    {
        /// <summary>
        /// 以太网数据包头部长度
        /// </summary>
        public const int HeaderSize = 14;

        /// <summary>
        /// 数据
        /// </summary>
        private SubArray<byte> data;
        /// <summary>
        /// 数据包是否有效
        /// </summary>
        public bool IsPacket
        {
            get { return data.Array != null; }
        }
        /// <summary>
        /// 以太网目的地址
        /// </summary>
        public SubArray<byte> DestinationMac
        {
            get { return new SubArray<byte>(data.StartIndex, 6, data.Array); }
        }
        /// <summary>
        /// 以太网源地址
        /// </summary>
        public SubArray<byte> SourceMac
        {
            get { return new SubArray<byte>(data.StartIndex + 6, 6, data.Array); }
        }
        /// <summary>
        /// 帧类型
        /// </summary>
        public Frame Frame
        {
            get
            {
                return (Frame)(ushort)(((uint)data.Array[data.StartIndex + 12] << 8) + data.Array[data.StartIndex + 13]);
            }
        }
        /// <summary>
        /// 以太网数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Ethernet(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// 以太网数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Ethernet(ref SubArray<byte> data)
        {
            this.data = data.Length >= HeaderSize ? data : default(SubArray<byte>);
        }
    }
}

using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// 以太网会话点到点协议数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct EthernetSessionP2P
    {
#pragma warning disable
        /// <summary>
        /// 以太网会话点到点协议数据包代码
        /// </summary>
        public enum CodeEnum : byte
        {
            ActiveDiscoveryInitiation = 9,
            ActiveDiscoveryOffer = 7,
            ActiveDiscoveryTerminate = 0xa7,
            SessionStage = 0
        }
        /// <summary>
        /// 点到点协议
        /// </summary>
        private enum Protocol : byte
        {
            IPv4 = 0x21,
            IPv6 = 0x57,
            Padding = 1
        }
#pragma warning restore
        /// <summary>
        /// 数据包头长度
        /// </summary>
        public const int HeaderSize = 8;

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
        /// 版本号
        /// </summary>
        public int Version
        {
            get { return (data.Array[data.StartIndex] >> 4) & 240; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type
        {
            get { return data.Array[data.StartIndex] & 15; }
        }
        /// <summary>
        /// 代码类型
        /// </summary>
        public CodeEnum Code
        {
            get { return (CodeEnum)data.Array[data.StartIndex + 1]; }
        }
        /// <summary>
        /// 标识
        /// </summary>
        public ushort SessionId
        {
            get { return BitConverter.ToUInt16(data.Array, data.StartIndex + 2); }
        }
        /// <summary>
        /// 数据包长度(单位未知)
        /// </summary>
        public uint packetSize
        {
            get { return ((uint)data.Array[data.StartIndex + 4] << 8) + data.Array[data.StartIndex + 5]; }
        }
        /// <summary>
        /// 帧类型
        /// </summary>
        public Frame Frame
        {
            get
            {
                if (data.Array[data.StartIndex + 6] == 0)
                {
                    switch (data.Array[data.StartIndex + 7])
                    {
                        case (byte)Protocol.IPv4:
                            return Frame.IpV4;
                        case (byte)Protocol.IPv6:
                            return Frame.IpV6;
                    }
                }
                return Frame.None;
            }
        }
        /// <summary>
        /// 以太网会话点到点协议数据包
        /// </summary>
        /// <param name="data">数据</param>
        public EthernetSessionP2P(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// 以太网会话点到点协议数据包
        /// </summary>
        /// <param name="data">数据</param>
        public unsafe EthernetSessionP2P(ref SubArray<byte> data)
        {
            if (data.Length >= HeaderSize)
            {
                fixed (byte* dataFixed = data.Array)
                {
                    byte* start = dataFixed + data.StartIndex;
                    uint packetSize = ((uint)*(start + 4) << 8) + *(start + 5);
                    if (data.Length >= packetSize)
                    {
                        this.data = data;
                        return;
                    }
                }
            }
            this.data = default(SubArray<byte>);
        }
    }
}

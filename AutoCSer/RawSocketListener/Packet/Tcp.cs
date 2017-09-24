using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// TCP数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Tcp
    {
        /// <summary>
        /// TCP头默认长度
        /// </summary>
        public const int DefaultHeaderSize = 20;

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
        /// 源端口
        /// </summary>
        public uint SourcePort
        {
            get { return ((uint)data.Array[data.StartIndex] << 8) + data.Array[data.StartIndex + 1]; }
        }
        /// <summary>
        /// 目的端口
        /// </summary>
        public uint DestinationPort
        {
            get { return ((uint)data.Array[data.StartIndex + 2] << 8) + data.Array[data.StartIndex + 3]; }
        }
        /// <summary>
        /// 初始连接的请求号，即SEQ值
        /// </summary>
        public uint SequenceNumber
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 4); }
        }
        /// <summary>
        /// 对方的应答号，即ACK值
        /// </summary>
        public uint AnswerNumber
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 8); }
        }
        /// <summary>
        /// TCP头长度
        /// </summary>
        public int HeaderSize
        {
            get { return (int)((data.Array[data.StartIndex + 12] >> 4) << 2); }
        }
        /// <summary>
        /// 紧急数据标志URG
        /// </summary>
        public int IsUrgent
        {
            get { return data.Array[data.StartIndex + 13] & 0x20; }
        }
        /// <summary>
        /// 确认标志位ACK
        /// </summary>
        public int IsAffirmance
        {
            get { return data.Array[data.StartIndex + 13] & 0x10; }
        }
        /// <summary>
        /// PUSH标志位PSH
        /// </summary>
        public int IsPush
        {
            get { return data.Array[data.StartIndex + 13] & 8; }
        }
        /// <summary>
        /// 复位标志位RST
        /// </summary>
        public int IsReset
        {
            get { return data.Array[data.StartIndex + 13] & 4; }
        }
        /// <summary>
        /// 连接请求标志位SYN(同步)
        /// </summary>
        public int IsConnection
        {
            get { return data.Array[data.StartIndex + 13] & 2; }
        }
        /// <summary>
        /// 结束连接请求标志位FIN
        /// </summary>
        public int IsFinish
        {
            get { return data.Array[data.StartIndex + 13] & 1; }
        }
        /// <summary>
        /// 窗口大小
        /// </summary>
        public uint WindowSize
        {
            get { return ((uint)data.Array[data.StartIndex + 14] << 8) + data.Array[data.StartIndex + 15]; }
        }
        /// <summary>
        /// 校验和
        /// </summary>
        public uint CheckSum
        {
            get { return ((uint)data.Array[data.StartIndex + 16] << 8) + data.Array[data.StartIndex + 17]; }
        }
        /// <summary>
        /// 紧急指针，只有当URG标志置1时紧急指针才有效
        /// </summary>
        public uint UrgentPointer
        {
            get { return ((uint)data.Array[data.StartIndex + 18] << 8) + data.Array[data.StartIndex + 19]; }
        }
        /// <summary>
        /// TCP头扩展
        /// </summary>
        public SubArray<byte> Expand
        {
            get
            {
                int headerSize = HeaderSize;
                return headerSize > DefaultHeaderSize ? new SubArray<byte>(data.StartIndex + DefaultHeaderSize, DefaultHeaderSize - headerSize, data.Array) : default(SubArray<byte>);
            }
        }
        /// <summary>
        /// 下层应用数据包
        /// </summary>
        public SubArray<byte> Packet
        {
            get
            {
                return new SubArray<byte>(data.StartIndex + HeaderSize, data.Length - HeaderSize, data.Array);
            }
        }
        /// <summary>
        /// TCP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Tcp(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// TCP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Tcp(ref SubArray<byte> data)
        {
            if (data.Length >= DefaultHeaderSize && data.Length >= (uint)((data.Array[data.StartIndex + 12] >> 4) << 2))
            {
                this.data = data;
            }
            else this.data = default(SubArray<byte>);
        }
    }
}

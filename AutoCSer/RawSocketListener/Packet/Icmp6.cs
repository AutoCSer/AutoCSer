using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ICMP V6数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Icmp6
    {
        /// <summary>
        /// ICMP类型
        /// </summary>
        public enum TypeEnum : byte
        {
            /// <summary>
            /// 目的不可达
            /// </summary>
            Unreachable = 1,
            /// <summary>
            /// 分组报文太长
            /// </summary>
            Overflow = 2,
            /// <summary>
            /// 超时
            /// </summary>
            Timeout = 3,
            /// <summary>
            /// 参数错误
            /// </summary>
            ParameterError = 4,

            /// <summary>
            /// 回显(ping)请求
            /// </summary>
            EchoRequest = 128,
            /// <summary>
            /// 回显(Ping)应答
            /// </summary>
            EchoAnswer = 129,
            /// <summary>
            /// 路由器请求
            /// </summary>
            RouterRequest = 133,
            /// <summary>
            /// 路由器通告
            /// </summary>
            RouterAdvertisement = 134,
            /// <summary>
            /// 邻居请求
            /// </summary>
            NeighborRequest = 135,
            /// <summary>
            /// 邻居通告
            /// </summary>
            NeighborAdvertisement = 136,
            /// <summary>
            /// 重定向
            /// </summary>
            Redirect = 137,
        }
        /// <summary>
        /// ICMP类型相关代码
        /// </summary>
        public enum CodeEnum : byte
        {
            /// <summary>
            /// 无路由目的不可达
            /// </summary>
            Unreachable_Routing = 0,
            /// <summary>
            /// 与管理受禁的目的地通信
            /// </summary>
            Unreachable_Disable = 1,
            /// <summary>
            /// 超出了源地址的范围
            /// </summary>
            Unreachable_Range = 2,
            /// <summary>
            /// 地址不可达
            /// </summary>
            Unreachable_Address = 3,
            /// <summary>
            /// 端口不可达
            /// </summary>
            Unreachable_Port = 4,

            /// <summary>
            /// 传输过程超过了跳数限制
            /// </summary>
            Timeout_Hops = 0,
            /// <summary>
            /// 分装重组时间到期
            /// </summary>
            Timeout_Assembly = 1,

            /// <summary>
            /// 错误的首部字段
            /// </summary>
            ParameterError_Header = 0,
            /// <summary>
            /// 无法识别的首部类型
            /// </summary>
            ParameterError_Type = 1,
            /// <summary>
            /// 无法识别的IPv6选项
            /// </summary>
            ParameterError_Option = 2,
        }
        /// <summary>
        /// ICMP类型对应数据包最小长度
        /// </summary>
        private static readonly byte[] minTypeSize;
        /// <summary>
        /// ICMP类型对应数组长度
        /// </summary>
        private static readonly int typeCount = AutoCSer.EnumAttribute<TypeEnum>.GetMaxValue(-1) + 1;
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
        /// ICMP类型
        /// </summary>
        public TypeEnum Type
        {
            get { return (TypeEnum)data.Array[data.StartIndex]; }
        }
        /// <summary>
        /// 代码
        /// </summary>
        public CodeEnum Code
        {
            get { return (CodeEnum)data.Array[data.StartIndex + 1]; }
        }
        /// <summary>
        /// 校验和
        /// </summary>
        public uint CheckSum
        {
            get { return ((uint)data.Array[data.StartIndex + 2] << 8) + data.Array[data.StartIndex + 3]; }
        }

        /// <summary>
        /// 分组报文太长 发生差错的物理链路的MTU
        /// </summary>
        public uint OverflowMTU
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 4); }
        }
        /// <summary>
        /// 参数错误相对于原始数据的位置
        /// </summary>
        public uint ParameterErrorIndex
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 4); }
        }

        /// <summary>
        /// 回显(ping)通讯标识
        /// </summary>
        public uint EchoIdentity
        {
            get { return ((uint)data.Array[data.StartIndex + 4] << 8) + data.Array[data.StartIndex + 5]; }
        }
        /// <summary>
        /// 回显(ping)序列号
        /// </summary>
        public uint EchoSequence
        {
            get { return ((uint)data.Array[data.StartIndex + 6] << 8) + data.Array[data.StartIndex + 7]; }
        }

        /// <summary>
        /// 路由器通告 跳数限制
        /// </summary>
        public byte RouterAdvertisementHops
        {
            get { return data.Array[data.StartIndex + 4]; }
        }
        /// <summary>
        /// 路由器通告 受管理的地址配置标志(如果主机启用动态主机配置协议DHCP)
        /// </summary>
        public bool RouterAdvertisementManaged
        {
            get { return (data.Array[data.StartIndex + 5] & 0x80) != 0; }
        }
        /// <summary>
        /// 路由器通告 其它有状态的配置标志
        /// </summary>
        public bool RouterAdvertisementStateful
        {
            get { return (data.Array[data.StartIndex + 5] & 0x40) != 0; }
        }
        /// <summary>
        /// 路由器通告 路由器寿命
        /// </summary>
        public uint RouterAdvertisementLife
        {
            get { return ((uint)data.Array[data.StartIndex + 6] << 8) + data.Array[data.StartIndex + 7]; }
        }
        /// <summary>
        /// 路由器通告 可达时间
        /// </summary>
        public uint RouterAdvertisementTime
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 8); }
        }
        /// <summary>
        /// 路由器通告 重传定时器
        /// </summary>
        public uint RouterAdvertisementTimer
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 12); }
        }

        /// <summary>
        /// 邻居请求 目标地址
        /// </summary>
        public SubArray<byte> NeighborRequestDestination
        {
            get { return new SubArray<byte>(data.StartIndex + 8, 16, data.Array); }
        }

        /// <summary>
        /// 邻居通告 路由器标志
        /// </summary>
        public bool NeighborAdvertisementRouter
        {
            get { return (data.Array[data.StartIndex + 4] & 0x80) != 0; }
        }
        /// <summary>
        /// 邻居通告 响应请求标志
        /// </summary>
        public bool NeighborAdvertisementAnswer
        {
            get { return (data.Array[data.StartIndex + 4] & 0x40) != 0; }
        }
        /// <summary>
        /// 邻居通告 覆盖缓存标志
        /// </summary>
        public bool NeighborAdvertisementOver
        {
            get { return (data.Array[data.StartIndex + 4] & 0x20) != 0; }
        }
        /// <summary>
        /// 邻居通告 目标地址
        /// </summary>
        public SubArray<byte> NeighborAdvertisementDestination
        {
            get { return new SubArray<byte>(data.StartIndex + 8, 16, data.Array); }
        }

        /// <summary>
        /// 重定向 路由器地址
        /// </summary>
        public SubArray<byte> RedirectRouter
        {
            get { return new SubArray<byte>(data.StartIndex + 8, 16, data.Array); }
        }
        /// <summary>
        /// 重定向 目的地址
        /// </summary>
        public SubArray<byte> RedirectDestination
        {
            get { return new SubArray<byte>(data.StartIndex + 24, 16, data.Array); }
        }

        /// <summary>
        /// ICMP数据包扩展
        /// </summary>
        public SubArray<byte> Expand
        {
            get
            {
                int minSize = minTypeSize[data.Array[data.StartIndex]];
                return data.Length > minSize ? new SubArray<byte>(data.StartIndex + minSize, data.Length - minSize, data.Array) : default(SubArray<byte>);
            }
        }
        /// <summary>
        /// ICMP V6数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Icmp6(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// ICMP V6数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Icmp6(ref SubArray<byte> data)
        {
            if (data.Length >= 8)
            {
                byte type = data.Array[data.StartIndex];
                if (type < typeCount)
                {
                    int minSize = minTypeSize[type];
                    if (minSize != 0 && data.Length >= minSize)
                    {
                        this.data = data;
                        return;
                    }
                }
            }
            this.data = default(SubArray<byte>);
        }
        static Icmp6()
        {
            #region 初始化 ICMP类型对应数据包最小长度
            minTypeSize = new byte[typeCount];
            minTypeSize[(int)TypeEnum.Unreachable] = 8;
            minTypeSize[(int)TypeEnum.Overflow] = 8;
            minTypeSize[(int)TypeEnum.Timeout] = 8;
            minTypeSize[(int)TypeEnum.ParameterError] = 8;
            minTypeSize[(int)TypeEnum.EchoRequest] = 8;
            minTypeSize[(int)TypeEnum.EchoAnswer] = 8;
            minTypeSize[(int)TypeEnum.RouterRequest] = 8;
            minTypeSize[(int)TypeEnum.RouterAdvertisement] = 16;
            minTypeSize[(int)TypeEnum.NeighborRequest] = 24;
            minTypeSize[(int)TypeEnum.NeighborAdvertisement] = 24;
            minTypeSize[(int)TypeEnum.Redirect] = 40;
            #endregion
        }
    }
}

using System;
using AutoCSer.Extension;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ICMP数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Icmp
    {
        /// <summary>
        /// ICMP类型
        /// </summary>
        public enum TypeEnum : byte
        {
            /// <summary>
            /// 回显(Ping)应答
            /// </summary>
            EchoAnswer = 0,
            /// <summary>
            /// 目的不可达
            /// </summary>
            Unreachable = 3,
            /// <summary>
            /// 源端被关闭
            /// </summary>
            SourceClosed = 4,
            /// <summary>
            /// 重定向
            /// </summary>
            Redirect = 5,
            /// <summary>
            /// 回显(Ping)请求
            /// </summary>
            EchoRequest = 8,
            /// <summary>
            /// 路由器通告
            /// </summary>
            RouterAdvertisement = 9,
            /// <summary>
            /// 路由器请求
            /// </summary>
            RouterRequest = 10,
            /// <summary>
            /// 超时
            /// </summary>
            Timeout = 11,
            /// <summary>
            /// 参数错误
            /// </summary>
            ParameterError = 12,
            /// <summary>
            /// 时间戳请求
            /// </summary>
            TimeRequest = 13,
            /// <summary>
            /// 时间戳应答
            /// </summary>
            TimeAnswer = 14,
            /// <summary>
            /// 信息请求(已作废)
            /// </summary>
            InformationRequest = 15,
            /// <summary>
            /// 信息应答(已作废)
            /// </summary>
            InformationAnswer = 16,
            /// <summary>
            /// 地址掩码请求
            /// </summary>
            MaskRequest = 17,
            /// <summary>
            /// 地址掩码应答
            /// </summary>
            MaskAnswer = 18,
        }
        /// <summary>
        /// ICMP类型相关代码
        /// </summary>
        public enum CodeEnum : byte
        {
            /// <summary>
            /// 默认空值
            /// </summary>
            None = 0,
            /// <summary>
            /// 网络不可达
            /// </summary>
            Unreachable_Network = 0,
            /// <summary>
            /// 主机不可达
            /// </summary>
            Unreachable_Host = 1,
            /// <summary>
            /// 协议不可达
            /// </summary>
            Unreachable_Protocol = 2,
            /// <summary>
            /// 端口不可达
            /// </summary>
            Unreachable_Port = 3,
            /// <summary>
            /// 需要进行分片但设置了不分片比特
            /// </summary>
            Unreachable_Fragment = 4,
            /// <summary>
            /// 源站选路失败
            /// </summary>
            Unreachable_Routing = 5,
            /// <summary>
            /// 目的网络不认识
            /// </summary>
            Unreachable_NetworkUnknow = 6,
            /// <summary>
            /// 目的主机不认识
            /// </summary>
            Unreachable_HostUnknow = 7,
            /// <summary>
            /// 源主机被隔离(已作废)
            /// </summary>
            Unreachable_Isolated = 8,
            /// <summary>
            /// 目的网络被强制禁止
            /// </summary>
            Unreachable_NetworkDisable = 9,
            /// <summary>
            /// 目的主机被强制禁止
            /// </summary>
            Unreachable_HostDisable = 10,
            /// <summary>
            /// 由于服务类型TOS，网络不可达
            /// </summary>
            Unreachable_NetworkTOS = 11,
            /// <summary>
            /// 由于服务类型TOS，主机不可达
            /// </summary>
            Unreachable_HostTOS = 12,
            /// <summary>
            /// 由于过滤，通信被强制禁止
            /// </summary>
            Unreachable_Filter = 13,
            /// <summary>
            /// 主机越权
            /// </summary>
            Unreachable_UltraVires = 14,
            /// <summary>
            /// 优先权中止生效
            /// </summary>
            Unreachable_Priority = 15,

            /// <summary>
            /// 对网络重定向
            /// </summary>
            Redirect_Network = 0,
            /// <summary>
            /// 对主机重定向
            /// </summary>
            Redirect_Host = 1,
            /// <summary>
            /// 对服务类型和网络重定向
            /// </summary>
            Redirect_NetworkTOS = 2,
            /// <summary>
            /// 对服务类型和主机重定向
            /// </summary>
            Redirect_HostTOS = 3,

            /// <summary>
            /// 传输期间生存时间为0
            /// </summary>
            Timeout_Transmission = 0,
            /// <summary>
            /// 在数据报组装期间生存时间为0
            /// </summary>
            Timeout_Assembly = 1,

            /// <summary>
            /// 坏的IP首部（包括各种差错）
            /// </summary>
            ParameterError_IpHeader = 0,
            /// <summary>
            /// 缺少必需的选项
            /// </summary>
            ParameterError_Options = 1,
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
        /// 掩码通讯标识
        /// </summary>
        public uint MaskIdentity
        {
            get { return ((uint)data.Array[data.StartIndex + 4] << 8) + data.Array[data.StartIndex + 5]; }
        }
        /// <summary>
        /// 掩码序列号
        /// </summary>
        public uint MaskSequence
        {
            get { return ((uint)data.Array[data.StartIndex + 6] << 8) + data.Array[data.StartIndex + 7]; }
        }
        /// <summary>
        /// 掩码地址
        /// </summary>
        public uint MaskAddress
        {
            get { return BitConverter.ToUInt32(data.Array, data.StartIndex + 8); }
        }

        /// <summary>
        /// 时间戳通讯标识
        /// </summary>
        public uint TimeIdentity
        {
            get { return ((uint)data.Array[data.StartIndex + 4] << 8) + data.Array[data.StartIndex + 5]; }
        }
        /// <summary>
        /// 时间戳序列号
        /// </summary>
        public uint TimeSequence
        {
            get { return ((uint)data.Array[data.StartIndex + 6] << 8) + data.Array[data.StartIndex + 7]; }
        }
        /// <summary>
        /// 时间戳请求时间，请求端填写发起时间戳，然后发送报文。(返回的是自午夜开始记算的毫秒数)
        /// </summary>
        public uint TimeRequest
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 8); }
        }
        /// <summary>
        /// 时间戳接收时间，应答系统收到报文填写接收时间戳。
        /// </summary>
        public uint TimeReceive
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 12); }
        }
        /// <summary>
        /// 时间戳发送时间，发送应答时填写发送时间戳。
        /// </summary>
        public uint TimeSend
        {
            get { return data.Array.GetUIntBigEndian(data.StartIndex + 16); }
        }

        /// <summary>
        /// 下一站网络的MTU
        /// </summary>
        public uint UnreachableMTU
        {
            get { return ((uint)data.Array[data.StartIndex + 6] << 8) + data.Array[data.StartIndex + 7]; }
        }

        /// <summary>
        /// 重定向应该使用的路由器IP地址
        /// </summary>
        public uint RedirectRouterAddress
        {
            get { return BitConverter.ToUInt32(data.Array, data.StartIndex + 4); }
        }

        /// <summary>
        /// 路由器通告地址数
        /// </summary>
        public byte RouterAdvertisementCount
        {
            get { return data.Array[data.StartIndex + 4]; }
        }
        /// <summary>
        /// 路由器通告地址项长度
        /// </summary>
        public byte RouterAdvertisementAddressLength
        {
            get { return data.Array[data.StartIndex + 5]; }
        }
        /// <summary>
        /// 路由器通告生存时间
        /// </summary>
        public uint RouterAdvertisementLifeTime
        {
            get { return ((uint)data.Array[data.StartIndex + 6] << 8) + data.Array[data.StartIndex + 7]; }
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
        /*
         ICMP数据包扩展。整个I P首部最长只能包括15个32bit长的字（即60个字节）。由于IP首部固定长度为20字节，RR选项用去3个字节，这样只剩下37个字节来存放IP地址清单，也就是说只能存放9个IP地址。
         RR选项code值为7。len是RR选项总字节长度，最大39。ptr是一个基于1的指针，指向存放下一个IP地址的位置，它的最小值为4+4x。
         IP时间戳选code值为0x44。len一般是36或40。prt指向下一个时间，最小为5+4x。ooooabxd，oooo为溢出字段，a表示只记录时间(4x)，b表示记录时间与IP地址(8x)，d表示发送端初始化时间与IP地址(8x，只有路由IP匹配时才记录时间戳)。
         源站选路(宽松code值为0x83，严格code值为0x89)，len同RR，ptr同RR。
         如果在IP首部中的选项字段中有多个选项，在开始下一个选项之前必须填入空白字符，另外还可以用另一个值为1的特殊字符NOP。
        */
        /// <summary>
        /// ICMP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Icmp(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// ICMP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Icmp(ref SubArray<byte> data)
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
        static Icmp()
        {
            #region 初始化 ICMP类型对应数据包最小长度
            minTypeSize = new byte[typeCount];
            minTypeSize[(int)TypeEnum.EchoAnswer] = 8;
            minTypeSize[(int)TypeEnum.Unreachable] = 8;
            minTypeSize[(int)TypeEnum.SourceClosed] = 8;
            minTypeSize[(int)TypeEnum.Redirect] = 8;
            minTypeSize[(int)TypeEnum.EchoRequest] = 8;
            minTypeSize[(int)TypeEnum.RouterAdvertisement] = 8;
            minTypeSize[(int)TypeEnum.RouterRequest] = 8;
            minTypeSize[(int)TypeEnum.Timeout] = 8;
            //minTypeSize[(int)icmpType.ParameterError] = ;
            minTypeSize[(int)TypeEnum.TimeRequest] = 20;
            minTypeSize[(int)TypeEnum.TimeAnswer] = 20;
            minTypeSize[(int)TypeEnum.MaskRequest] = 12;
            minTypeSize[(int)TypeEnum.MaskAnswer] = 12;
            #endregion
        }
    }
}

using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// IPv4 数据包
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Ip
    {
#pragma warning disable
        /// <summary>
        /// IP协议
        /// </summary>
        public enum ProtocolEnum : byte//System.Net.Sockets.ProtocolType
        {
            /// <summary>
            /// 未知
            /// </summary>
            //Unknown = -1,
            /// <summary>
            /// 未指定
            /// </summary>
            Unspecified = 0,
            /// <summary>
            /// IP协议
            /// </summary>
            IP = 0,
            /// <summary>
            /// IPv6逐跳选项扩展
            /// </summary>
            IPv6HopByHopOptions = 0,
            /// <summary>
            /// 互联网控制报文协议
            /// </summary>
            Icmp = 1,
            /// <summary>
            /// 互联网组管理协议
            /// </summary>
            Igmp = 2,
            /// <summary>
            /// 网关到网关协议
            /// </summary>
            Ggp = 3,
            /// <summary>
            /// IPv4
            /// </summary>
            IPv4 = 4,
            /// <summary>
            /// 传输控制协议
            /// </summary>
            Tcp = 6,
            /// <summary>
            /// 外部网关协议
            /// </summary>
            Egp = 8,
            /// <summary>
            /// PUP协议
            /// </summary>
            Pup = 12,
            /// <summary>
            /// 用户数据报协议
            /// </summary>
            Udp = 17,
            /// <summary>
            /// XNS IDP 协议
            /// </summary>
            Hmp = 20,
            /// <summary>
            /// 互联网数据报协议
            /// </summary>
            Idp = 22,
            Rdp = 27,
            /// <summary>
            /// SO传输入协议
            /// </summary>
            TP = 29,
            /// <summary>
            /// IPv6头部
            /// </summary>
            IPv6 = 41,
            /// <summary>
            /// IPv6选路选项扩展
            /// </summary>
            IPv6RoutingHeader = 43,
            /// <summary>
            /// IPv6分片选项扩展
            /// </summary>
            IPv6FragmentHeader = 44,
            /// <summary>
            /// 资源预留协议
            /// </summary>
            Rsvp = 46,
            /// <summary>
            /// 通用路由封装
            /// </summary>
            Gre = 47,
            /// <summary>
            /// IPv6封装安全性选项扩展
            /// </summary>
            IPSecEncapsulatingSecurityPayload = 50,
            /// <summary>
            /// IPv6鉴别首部扩展
            /// </summary>
            IPSecAuthenticationHeader = 51,
            /// <summary>
            /// 互联网控制报文协议(IPv6)
            /// </summary>
            IcmpV6 = 58,
            /// <summary>
            /// IPv6无下一首部
            /// </summary>
            IPv6NoNextHeader = 59,
            /// <summary>
            /// IPv6目的地选项扩展
            /// </summary>
            IPv6DestinationOptions = 60,
            Rcd = 66,
            /// <summary>
            /// 网络硬盘协议(非官方)
            /// </summary>
            ND = 77,
            /// <summary>
            /// 多播传输协议
            /// </summary>
            Mtp = 92,
            /// <summary>
            /// 封装头
            /// </summary>
            Encap = 98,
            /// <summary>
            /// IPv6协议独立组播
            /// </summary>
            Pim = 103,
            /// <summary>
            /// 压缩头部协议
            /// </summary>
            Comp = 108,
            /// <summary>
            /// 原始IP数据包
            /// </summary>
            Raw = 255,
            ///// <summary>
            ///// 互联网数据包交换协议
            ///// </summary>
            //Ipx = 1000,
            ///// <summary>
            ///// 顺序包交换协议
            ///// </summary>
            //Spx = 1256,
            ///// <summary>
            ///// 顺序包交换协议版本2
            ///// </summary>
            //SpxII = 1257,
        }
        ///// <summary>
        ///// IP端口
        ///// </summary>
        //public enum IpPort : ushort
        //{
        //    Auth = 0x71,
        //    DayTime = 13,
        //    Echo = 7,
        //    Finger = 0x4f,
        //    Ftp = 0x15,
        //    FtpData = 20,
        //    Gopher = 70,
        //    Http = 80,
        //    Ident = 0x71,
        //    Imap = 0x8f,
        //    Kerberos = 0x58,
        //    Ntp = 0x7b,
        //    Pop3 = 110,
        //    PrivilegedPortLimit = 0x400,
        //    Sftp = 0x73,
        //    Smtp = 0x19,
        //    Snmp = 0xa1,
        //    Ssh = 0x16,
        //    Telnet = 0x17,
        //    Tftp = 0x45,
        //    Time = 0x25,
        //    Whois = 0x3f,
        //    Www = 80
        //}
#pragma warning restore
        /// <summary>
        /// IP标头默认字节数
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
        /// IP标头字节数
        /// </summary>
        public int HeaderSize
        {
            get { return (data.Array[data.StartIndex] & 15) << 2; }
        }
        /// <summary>
        /// IP版本号
        /// </summary>
        public int Version
        {
            get { return data.Array[data.StartIndex] >> 4; }
        }
        /// <summary>
        /// 优先权,现在已被忽略
        /// </summary>
        public int Priority
        {
            get { return data.Array[data.StartIndex + 1] >> 5; }
        }
        /// <summary>
        /// 服务类型TOS字段，暂时大多数TCP/IP实现都不支持TOS特性，4位中只能用1位，下面是推荐值。
        /// 最小时延：如 Telnet，Rlogin，FTP控制，TFTP，SMTP命令，DNS-UDP查询
        /// 最大吞吐量：如 FTP数据，SMTP数据，DNS区域传输
        /// 最高可靠性：如 SNMP，IGP
        /// 最小费用：如 NNTP
        /// </summary>
        public int ServiceType
        {
            get { return (data.Array[data.StartIndex + 1] >> 1) & 15; }
        }
        /// <summary>
        /// IP数据包总字节数(大多数的链路层都会对它进行分片，主机也要求不能接收超过576B[报文512B]的数据报。事实上现在大多数的实现（特别是那些支持网络文件系统NFS的实现）允许超过8192B的IP数据报。)
        /// </summary>
        public uint PacketSize
        {
            get { return ((uint)data.Array[data.StartIndex + 2] << 8) + data.Array[data.StartIndex + 3]; }
        }
        /// <summary>
        /// 唯一标识主机发送的每一个数据报，通常每发一份它的值就会加1。
        /// </summary>
        public uint Identity
        {
            get { return ((uint)data.Array[data.StartIndex + 4] << 8) + data.Array[data.StartIndex + 5]; }
        }
        /// <summary>
        /// 是否分片
        /// </summary>
        public bool IsFragment
        {
            get { return (data.Array[data.StartIndex + 6] & 0x40) != 0; }
        }
        /// <summary>
        /// 更多分片标志
        /// </summary>
        public bool MoreFragment
        {
            get { return (data.Array[data.StartIndex + 6] & 0x20) != 0; }
        }
        /// <summary>
        /// 分片偏移，该片偏移原始数据报开始处的位置
        /// </summary>
        public uint FragmentOffset
        {
            get { return (((uint)data.Array[data.StartIndex + 6] & 0x1f) << 8) + data.Array[data.StartIndex + 7]; }
        }
        /// <summary>
        /// 生存时间周期，一般为32或64，每经过一个路由器就减1，如果该字段为0，则该数据报被丢弃。
        /// </summary>
        public byte LifeTime
        {
            get { return data.Array[data.StartIndex + 8]; }
        }
        /// <summary>
        /// IP协议
        /// </summary>
        public ProtocolEnum Protocol
        {
            get { return (ProtocolEnum)data.Array[data.StartIndex + 9]; }
        }
        /// <summary>
        /// 校验和
        /// </summary>
        public uint CheckSum
        {
            get { return ((uint)data.Array[data.StartIndex + 10] << 8) + data.Array[data.StartIndex + 11]; }
        }
        /// <summary>
        /// 源IP地址
        /// </summary>
        public uint Source
        {
            get { return BitConverter.ToUInt32(data.Array, data.StartIndex + 12); }
        }
        /// <summary>
        /// 目的IP地址
        /// </summary>
        public uint Destination
        {
            get { return BitConverter.ToUInt32(data.Array, data.StartIndex + 16); }
        }
        /// <summary>
        /// IP头扩展
        /// </summary>
        public SubArray<byte> Expand
        {
            get
            {
                return HeaderSize > DefaultHeaderSize ?  new SubArray<byte>(data.StartIndex + DefaultHeaderSize, HeaderSize - DefaultHeaderSize, data.Array) : default(SubArray<byte>);
            }
        }
        /// <summary>
        /// 下层应用数据包
        /// </summary>
        public SubArray<byte> Packet
        {
            get
            {
                int headerSize = HeaderSize;
                return new SubArray<byte>(data.StartIndex + headerSize, data.Length - headerSize, data.Array);
            }
        }
        /// <summary>
        /// IP头校验和(应用于TCP,UDP等协议)
        /// </summary>
        private uint headerCheckSum
        {
            get
            {
                uint source = Source, destination = Destination, packetSize = (uint)(PacketSize - HeaderSize);
                return (source & 0xffff) + (source >> 16) + (destination & 0xffff) + (destination >> 16)
                    + (packetSize >> 8) + (((packetSize & 0xffU) + (uint)Protocol) << 8);
            }
        }
        /// <summary>
        /// 初始化IP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public unsafe Ip(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// 初始化IP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public unsafe Ip(ref SubArray<byte> data)
        {
            if (data.Length >= DefaultHeaderSize)
            {
                fixed (byte* dataFixed = data.Array)
                {
                    byte* start = dataFixed + data.StartIndex;
                    uint packetSize = ((uint)*(start + 2) << 8) + *(start + 3);
                    if (packetSize >= ((*start & 15) << 2) && packetSize <= data.Length)
                    {
                        this.data = new SubArray<byte>(data.StartIndex, (int)packetSize, data.Array);
                        return;
                    }
                }
            }
            this.data = default(SubArray<byte>);
        }
        /// <summary>
        /// 获取校验和，IP、ICMP、IGMP、TCP和UDP协议采用相同的检验和算法(对首部中每个16bit进行二进制反码求和，如果首部在传输过程中没有发生任何差错，那么接收方计算的结果应该为全1。)
        /// </summary>
        /// <param name="data">待校验数据</param>
        /// <param name="checkSum">校验和初始值,默认应为0</param>
        /// <returns>校验和</returns>
        public unsafe static ushort CreateCheckSum(ref SubArray<byte> data, uint checkSum = 0)
        {
            fixed (byte* fixedData = data.Array)
            {
                byte* start = fixedData + data.StartIndex, end = start + data.Length - 1;
                while (start < end)
                {
                    checkSum += *((ushort*)start);
                    start += 2;
                }
                if (start == end) checkSum += *start;
            }
            checkSum = (checkSum >> 16) + (checkSum & 0xffffU);
            return (ushort)(~(checkSum + (checkSum >> 16)));
        }
    }
}

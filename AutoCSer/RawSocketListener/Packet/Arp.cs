using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// ARP数据包(对于ARP和RARP请求/应答数据报大小只有28字节，为了达到46字节的最小长度，必须在后面添加18字节的填充字节)
    /// ARP查询/应答包(ARP是一个无状态的协议，只要有发往本机的ARP应答包，计算机都不加验证的接收，并更新自己的ARP缓存)
    /// 使用ARP欺骗功能前，必须安装winpcap驱动并启动ip路由功能，修改(添加)注册表选项：HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\IPEnableRouter = 0x1　
    /// 对于ARP和RARP请求/应答数据报大小只有28字节，为了达到46字节的最小长度，必须在后面添加18字节的填充字节。
    /// 使用sendarp有个问题，在远端主机不在线后（如拔掉网线后），使用该方法仍然能探测到在线。原因是ARP缓存还在，需要使用ARP -D来删除。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Arp
    {
        /// <summary>
        /// ARP数据包长度
        /// </summary>
        public const int PacketSize = 28;

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
        /// 硬件类型,0x0001表示10Mb以太网
        /// </summary>
        public uint Hardware
        {
            get { return ((uint)data.Array[data.StartIndex] << 8) + data.Array[data.StartIndex + 1]; }
        }
        /// <summary>
        /// 协议类型,为0x0800表示IP地址
        /// </summary>
        public uint Protocol
        {
            get { return ((uint)data.Array[data.StartIndex + 2] << 8) + data.Array[data.StartIndex + 3]; }
        }
        /// <summary>
        /// 硬件地址长度(即MAC地址长度),以太网为0x06
        /// </summary>
        public byte HardwareSize
        {
            get { return data.Array[data.StartIndex + 4]; }
        }
        /// <summary>
        /// 协议地址长度(即IP地址长度),以太网为0x04
        /// </summary>
        public byte AgreementSize
        {
            get { return data.Array[data.StartIndex + 5]; }
        }
        /// <summary>
        /// ARP请求包的OP值为1，ARP应答包的OP值为2，RARP请求包的OP值为3，RARP应答包的OP值为4
        /// </summary>
        public uint RequestOrResponse
        {
            get { return ((uint)data.Array[data.StartIndex + 6] << 8) + data.Array[data.StartIndex + 7]; }
        }
        /// <summary>
        /// 发送者以太网地址
        /// </summary>
        public SubArray<byte> SendMac
        {
            get { return new SubArray<byte>(data.StartIndex + 8, 6, data.Array); }
        }
        /// <summary>
        /// 发送者的IP地址
        /// </summary>
        public uint SendIp
        {
            get { return BitConverter.ToUInt32(data.Array, data.StartIndex + 14); }
        }
        /// <summary>
        /// 目的以太网地址
        /// </summary>
        public SubArray<byte> DestinationMac
        {
            get { return new SubArray<byte>(data.StartIndex + 18, 6, data.Array); }
        }
        /// <summary>
        /// 目的IP(查询MAC地址的IP)
        /// </summary>
        public uint destinationIp
        {
            get { return BitConverter.ToUInt32(data.Array, data.StartIndex + 24); }
        }
        /// <summary>
        /// ARP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Arp(SubArray<byte> data) : this(ref data) { }
        /// <summary>
        /// ARP数据包
        /// </summary>
        /// <param name="data">数据</param>
        public Arp(ref SubArray<byte> data)
        {
            this.data = data.Length >= PacketSize ? data : default(SubArray<byte>);
        }
    }
}

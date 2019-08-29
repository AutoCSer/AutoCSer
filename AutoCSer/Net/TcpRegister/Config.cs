using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpRegister
{
    /// <summary>
    /// TCP 内部注册服务配置
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        /// 端口分配起始位置，默认为 9000
        /// </summary>
        public int PortStart = (int)ServerPort.Reserved + 1;
        /// <summary>
        /// 端口分配结束位置（不包含），默认为 65536
        /// </summary>
        public int PortEnd = 65536;
        /// <summary>
        /// 服务注册保存间隔秒数，默认为 2 秒
        /// </summary>
        public int SaveSeconds = 2;
        /// <summary>
        /// 检测端口信息
        /// </summary>
        internal void CheckPort()
        {
            if (PortStart > PortEnd)
            {
                int port = PortStart;
                PortStart = PortEnd;
                PortEnd = port;
            }
            if (PortStart <= 0) PortStart = (int)ServerPort.Reserved + 1;
            if (PortEnd > 65536) PortEnd = 65536;
            if (PortStart == PortEnd) PortEnd = PortStart + 1;
        }
        /// <summary>
        /// 判断端口号是否在分配范围内
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsPort(int port)
        {
            return port >= PortStart && port < PortEnd;
        }
    }
}

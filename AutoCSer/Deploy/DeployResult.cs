using AutoCSer.Net.TcpServer;
using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署结果
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeployResult
    {
        /// <summary>
        /// 部署索引
        /// </summary>
        public int Index;
        /// <summary>
        /// 部署状态
        /// </summary>
        public DeployState State;
        /// <summary>
        /// 返回数据
        /// </summary>
        public byte[] Data;
        /// <summary>
        /// 远程状态
        /// </summary>
        public ReturnType ReturnType;
    }
}

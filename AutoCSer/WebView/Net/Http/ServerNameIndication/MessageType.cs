using System;

namespace AutoCSer.Net.Http.ServerNameIndication
{
    /// <summary>
    /// SSL通信哪个阶段 Byte[0]
    /// </summary>
    internal enum MessageType : byte
    {
        /// <summary>
        /// 加密传输
        /// </summary>
        ChangeCipherSpec = 0x14,
        /// <summary>
        /// 
        /// </summary>
        Alert = 0x15,
        /// <summary>
        /// 握手
        /// </summary>
        Handshake = 0x16,
        /// <summary>
        /// 正常通信
        /// </summary>
        Application = 0x17,
    }
}

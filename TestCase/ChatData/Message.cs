using System;

namespace AutoCSer.TestCase.ChatData
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct Message
    {
        /// <summary>
        /// 发送消息用户名称
        /// </summary>
        public string User;
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content;
        /// <summary>
        /// 服务端接收消息时间
        /// </summary>
        public DateTime Time;
    }
}

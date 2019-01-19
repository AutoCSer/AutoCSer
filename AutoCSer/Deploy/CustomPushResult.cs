using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 服务端推送结果
    /// </summary>
    public struct CustomPushResult
    {
        /// <summary>
        /// 部署服务名称
        /// </summary>
        public string ServerName;
        /// <summary>
        /// 自定义数据
        /// </summary>
        public byte[] Data;
    }
}

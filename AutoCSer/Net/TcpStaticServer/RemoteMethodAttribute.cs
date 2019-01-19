using System;

namespace AutoCSer.Net.TcpStaticServer
{
    /// <summary>
    /// 远程调用链目标函数配置
    /// </summary>
    public sealed class RemoteMethodAttribute : RemoteMemberAttribute
    {
        /// <summary>
        /// 是否生成函数
        /// </summary>
        internal override bool IsMethod
        {
            get { return true; }
        }
    }
}

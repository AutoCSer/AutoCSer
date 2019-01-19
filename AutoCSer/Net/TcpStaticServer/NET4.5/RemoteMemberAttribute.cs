using System;

namespace AutoCSer.Net.TcpStaticServer
{
    /// <summary>
    /// 远程调用链目标成员配置
    /// </summary>
    public partial class RemoteMemberAttribute
    {
        /// <summary>
        /// 默认为 true 表示生成 await 客户端函数
        /// </summary>
        public bool IsAwait = true;
    }
}

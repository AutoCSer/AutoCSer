using System;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// TCP 参数类型配置
    /// </summary>
    [Flags]
    internal enum ParameterFlag
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 是否检测相同的引用成员
        /// </summary>
        IsSerializeReferenceMember = 1,
        /// <summary>
        /// 是否序列化包装处理
        /// </summary>
        IsSerializeBox = 2,
    }
}

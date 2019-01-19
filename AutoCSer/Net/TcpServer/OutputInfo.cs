using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 服务端输出信息
    /// </summary>
    public sealed class OutputInfo
    {
        /// <summary>
        /// 输出参数编号
        /// </summary>
        public int OutputParameterIndex;
        /// <summary>
        /// 是否简单序列化输出参数
        /// </summary>
        public bool IsSimpleSerializeOutputParamter;
        /// <summary>
        /// 是否保持异步回调
        /// </summary>
        public byte IsKeepCallback;
        /// <summary>
        /// 客户端是否仅仅发送数据(不需要应答)
        /// </summary>
        public byte IsClientSendOnly;
        /// <summary>
        /// 是否采用 JSON 序列化
        /// </summary>
        internal byte IsJsonSerialize
        {
            get { return IsKeepCallback; }
        }
        /// <summary>
        /// 创建输出是否开启线程
        /// </summary>
        public bool IsBuildOutputThread;

        /// <summary>
        /// 最大输出数据字节长度
        /// </summary>
        internal int MaxDataSize;
        /// <summary>
        /// 更新最大输出数据字节长度
        /// </summary>
        /// <param name="size">输出数据字节长度</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CheckMaxDataSize(int size)
        {
            if (size > MaxDataSize) MaxDataSize = size;
        }
    }
}

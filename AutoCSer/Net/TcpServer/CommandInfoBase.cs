using System;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 客户端命令信息
    /// </summary>
    public class CommandInfoBase
    {
        /// <summary>
        /// TCP 调用命令
        /// </summary>
        public int Command;
        /// <summary>
        /// 命令参数
        /// </summary>
        public TcpServer.CommandFlags CommandFlags;
        /// <summary>
        /// 输入参数编号
        /// </summary>
        public int InputParameterIndex;
        /// <summary>
        /// 超时秒数
        /// </summary>
        public ushort TimeoutSeconds;
        /// <summary>
        /// 是否验证函数
        /// </summary>
        public bool IsVerifyMethod;
        /// <summary>
        /// 简单序列化参数
        /// </summary>
        protected byte SimpleSerializeFlags;
        /// <summary>
        /// 是否简单序列化输入参数
        /// </summary>
        public int SimpleSerializeInputParamter
        {
            get { return SimpleSerializeFlags & 1; }
        }
        /// <summary>
        /// 是否简单序列化输入参数
        /// </summary>
        public bool IsSimpleSerializeInputParamter
        {
            set
            {
                if (value) SimpleSerializeFlags |= 1;
                else SimpleSerializeFlags &= byte.MaxValue ^ 1;
            }
        }
        /// <summary>
        /// 是否简单序列化输出参数
        /// </summary>
        public int SimpleSerializeOutputParamter
        {
            get { return SimpleSerializeFlags & 2; }
        }
        /// <summary>
        /// 是否简单序列化输出参数
        /// </summary>
        public bool IsSimpleSerializeOutputParamter
        {
            set
            {
                if (value) SimpleSerializeFlags |= 2;
                else SimpleSerializeFlags &= byte.MaxValue ^ 2;
            }
        }
    }
}

using System;

namespace AutoCSer.DiskBlock
{
    /// <summary>
    /// 磁盘块服务客户端配置
    /// </summary>
    public class ClientConfig
    {
        /// <summary>
        /// 磁盘块服务数量
        /// </summary>
        public int Count = 1;

        /// <summary>
        /// 二进制序列化配置参数
        /// </summary>
        internal static readonly BinarySerialize.SerializeConfig BinarySerializeConfig;

        static ClientConfig()
        {
            BinarySerializeConfig = AutoCSer.MemberCopy.Copyer<BinarySerialize.SerializeConfig>.MemberwiseClone(BinarySerialize.Serializer.DefaultConfig);
        }
    }
}

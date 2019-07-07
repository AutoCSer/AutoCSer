using System;
using AutoCSer.Metadata;

namespace AutoCSer.Net.TcpOpenSimpleServer
{
    /// <summary>
    /// TCP 服务配置
    /// </summary>
    public unsafe class ServerAttribute : TcpSimpleServer.ServerAttribute
    {
        /// <summary>
        /// 成员选择类型
        /// </summary>
        public MemberFilters MemberFilters = MemberFilters.Instance;
        /// <summary>
        /// 成员选择类型
        /// </summary>
        internal override MemberFilters GetMemberFilters { get { return MemberFilters; } }
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数，默认为 8KB。
        /// </summary>
        public SubBuffer.Size SendBufferSize = SubBuffer.Size.Kilobyte8;
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override SubBuffer.Size GetSendBufferSize { get { return SendBufferSize; } }
        /// <summary>
        /// 服务器端发送数据缓冲区最大字节数
        /// </summary>
        public int ServerSendBufferMaxSize;
        /// <summary>
        /// 服务器端发送数据缓冲区最大字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetServerSendBufferMaxSize { get { return ServerSendBufferMaxSize; } }
        /// <summary>
        /// 最大输入数据字节数，默认为 16 KB，小于等于 0 表示不限
        /// </summary>
        public int MaxInputSize = (16 << 10) - (sizeof(uint) + sizeof(int) * 2);
        /// <summary>
        /// 客户端保持连接心跳包间隔时间默认为 59 秒，对于频率稳定可靠的服务类型可以设置为 0 禁用心跳包。
        /// </summary>
        public int CheckSeconds = 59;
        /// <summary>
        /// 客户端保持连接心跳包间隔时间
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetCheckSeconds { get { return CheckSeconds; } }
        /// <summary>
        /// 客户端接收命令超时为 9 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        public int ReceiveVerifyCommandSeconds = TcpOpenServer.ServerAttribute.DefaultReceiveVerifyCommandSeconds;
        /// <summary>
        /// 当需要将客户端提供给第三方使用的时候，可能不希望 dll 中同时包含服务端，默认为 true 会将客户端代码单独剥离出来生成一个代码文件 {项目名称}.tcpServer.服务名称.client.cs，当然你需要将服务中所有参数与返回值及其依赖的数据类型剥离出来。
        /// </summary>
        public bool IsSegmentation = true;
        /// <summary>
        /// 当需要将客户端提供给第三方使用的时候，可能不希望 dll 中同时包含服务端，设置为 true 会将客户端代码单独剥离出来生成一个代码文件 {项目名称}.tcpServer.服务名称.client.cs，当然你需要将服务中所有参数与返回值及其依赖的数据类型剥离出来。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsSegmentation { get { return IsSegmentation; } }
        /// <summary>
        /// 默认使用 JSON 序列化，适合数据类型不稳定的互联网服务。对于参数数据类型稳定的服务，或者可以同步部署服务器端与客户端的内部服务，可以考虑使用二进制序列化以提升性能（对于简单测试可能提升 100+% 性能）。
        /// </summary>
        public bool IsJsonSerialize = true;
        /// <summary>
        /// 是否使用 JSON 序列化
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsJsonSerialize { get { return IsJsonSerialize; } }
        /// <summary>
        /// 默认为 false 需要第一次调用触发，否则在创建客户端对象的时候自动启动连接
        /// </summary>
        public bool IsAutoClient;
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        public int BinaryDeSerializeMaxArraySize = AutoCSer.Net.TcpOpenServer.ServerAttribute.DefaultBinaryDeSerializeConfig.MaxArraySize;
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        internal override int GetBinaryDeSerializeMaxArraySize { get { return BinaryDeSerializeMaxArraySize; } }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="serviceName">TCP 调用服务名称</param>
        /// <param name="type">TCP 服务器类型</param>
        /// <returns>TCP 调用服务器端配置信息</returns>
        public static ServerAttribute GetConfig(string serviceName, Type type = null)
        {
            return GetConfig(serviceName, type, new UnionType { Value = AutoCSer.Config.Loader.GetObject(typeof(ServerAttribute), serviceName) }.ServerAttribute);
        }
    }
}

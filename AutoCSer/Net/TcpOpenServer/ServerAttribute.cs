using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpOpenServer
{
    /// <summary>
    /// TCP 服务配置
    /// </summary>
    public unsafe class ServerAttribute : TcpServer.ServerAttribute
    {
        /// <summary>
        /// 默认二进制反序列化配置参数名称
        /// </summary>
        public const string BinaryDeSerializeConfigName = "TcpOpenServer";
        /// <summary>
        /// 默认二进制反序列化配置参数
        /// </summary>
        internal static readonly AutoCSer.BinarySerialize.DeSerializeConfig DefaultBinaryDeSerializeConfig = AutoCSer.BinarySerialize.ConfigLoader.GetUnion(typeof(AutoCSer.BinarySerialize.DeSerializeConfig), BinaryDeSerializeConfigName).DeSerializeConfig ?? new AutoCSer.BinarySerialize.DeSerializeConfig { IsDisposeMemberMap = true, MaxArraySize = 1024 };
        /// <summary>
        /// 获取二进制反序列化配置参数
        /// </summary>
        /// <param name="maxArraySize"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static AutoCSer.BinarySerialize.DeSerializeConfig GetBinaryDeSerializeConfig(int maxArraySize)
        {
            if (maxArraySize == AutoCSer.BinarySerialize.DeSerializer.DefaultConfig.MaxArraySize) return AutoCSer.BinarySerialize.DeSerializer.DefaultConfig;
            if (maxArraySize == DefaultBinaryDeSerializeConfig.MaxArraySize) return DefaultBinaryDeSerializeConfig;
            return new AutoCSer.BinarySerialize.DeSerializeConfig { IsDisposeMemberMap = true, MaxArraySize = maxArraySize };
        }

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
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数，默认为 8KB。
        /// </summary>
        public SubBuffer.Size ReceiveBufferSize = SubBuffer.Size.Kilobyte8;
        /// <summary>
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override SubBuffer.Size GetReceiveBufferSize { get { return ReceiveBufferSize; } }
        /// <summary>
        /// 最大输入数据字节数，默认为 16 KB，小于等于 0 表示不限
        /// </summary>
        public int MaxInputSize = (16 << 10) - (sizeof(uint) + sizeof(int) * 2);
        ///// <summary>
        ///// 压缩启用最低字节数量，默认为 512 字节（压缩数据需要消耗一定的 CPU 资源降低带宽使用，在简单测试中可能降低 60% 的性能），设置为 0 表示不压缩数据（适合内网服务）。
        ///// </summary>
        //public int MinCompressSize = 512;
        ///// <summary>
        ///// 压缩启用最低字节数量
        ///// </summary>
        //[AutoCSer.Metadata.Ignore]
        //internal override int GetMinCompressSize { get { return MinCompressSize; } }
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
        /// 客户端接收命令默认超时为 9 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        internal const int DefaultReceiveVerifyCommandSeconds = 9;
        /// <summary>
        /// 客户端接收命令超时为 9 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        public int ReceiveVerifyCommandSeconds = DefaultReceiveVerifyCommandSeconds;
        /// <summary>
        /// 客户端最大自定义数据包字节大小，默认为 16KB，设置为 0 表示不限
        /// </summary>
        public int MaxCustomDataSize = (16 << 10) - (sizeof(uint) + sizeof(int) * 2);
        /// <summary>
        /// 客户端最大自定义数据包字节大小
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetMaxCustomDataSize { get { return MaxCustomDataSize; } }
        /// <summary>
        /// 客户端重建连接休眠毫秒数，默认为 1000
        /// </summary>
        public int ClientTryCreateSleep = 1000;
        /// <summary>
        /// 客户端重建连接休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetClientTryCreateSleep { get { return ClientTryCreateSleep; } }
        /// <summary>
        /// 客户端第一次重建连接休眠毫秒数，默认为 1000
        /// </summary>
        public int ClientFirstTryCreateSleep = 1000;
        /// <summary>
        /// 客户端第一次重建连接休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetClientFirstTryCreateSleep { get { return ClientFirstTryCreateSleep; } }
        /// <summary>
        /// 批量处理休眠毫秒数，默认为 -1 表示不等待，设置为 0 适应于延时要求不高只要求吞吐量的情况以减少套接字调用次数，设置为 0 适应于低延时高频串行调用
        /// </summary>
        public int ClientOutputSleep = -1;
        /// <summary>
        /// 批量处理休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetClientOutputSleep { get { return ClientOutputSleep; } }
        /// <summary>
        /// 批量处理休眠毫秒数，默认为 -1 表示不等待，设置为 0 适应于低延时高频串行调用，大于 0 适应于延时要求不高只要求吞吐量的情况以减少套接字调用次数
        /// </summary>
        public int ServerOutputSleep = -1;
        /// <summary>
        /// 批量处理休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetServerOutputSleep { get { return ServerOutputSleep; } }
        /// <summary>
        /// 命令池初始化二进制大小，默认为 3
        /// </summary>
        public byte CommandPoolBitSize = 3;
        /// <summary>
        /// 命令池初始化二进制大小 2^n
        /// </summary>
        internal override byte GetCommandPoolBitSize { get { return CommandPoolBitSize; } }
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
        /// 服务端创建输出是否开启线程，默认为 false
        /// </summary>
        public bool IsServerBuildOutputThread;
        /// <summary>
        /// 服务端创建输出是否开启线程
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsServerBuildOutputThread { get { return IsServerBuildOutputThread; } }
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        public int BinaryDeSerializeMaxArraySize = DefaultBinaryDeSerializeConfig.MaxArraySize;
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        internal override int GetBinaryDeSerializeMaxArraySize { get { return BinaryDeSerializeMaxArraySize; } }
        /// <summary>
        /// 默认为 false 需要第一次调用触发，否则在创建客户端对象的时候自动启动连接
        /// </summary>
        public bool IsAutoClient;

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

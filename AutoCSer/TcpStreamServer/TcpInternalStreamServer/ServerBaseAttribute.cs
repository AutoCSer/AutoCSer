using System;

namespace AutoCSer.Net.TcpInternalStreamServer
{
    /// <summary>
    /// TCP 内部服务配置
    /// </summary>
    public abstract class ServerBaseAttribute : TcpStreamServer.ServerAttribute, TcpRegister.IServerAttribute
    {
        /// <summary>
        /// 注册当前服务的 TCP 注册服务名称。
        /// </summary>
        public string TcpRegister;
        /// <summary>
        /// 注册当前服务的 TCP 注册服务名称。
        /// </summary>
        internal virtual string TcpRegisterName
        {
            get { return TcpRegister; }
        }
        /// <summary>
        /// 客户端访问的主机名称或者 IP 地址，用于需要使用端口映射服务。
        /// </summary>
        public string RegisterHost;
        /// <summary>
        /// 客户端访问的主机名称或者 IP 地址，用于需要使用端口映射服务。
        /// </summary>
        string TcpRegister.IServerAttribute.ClientRegisterHost
        {
            get { return RegisterHost; }
            set { RegisterHost = value; }
        }
        /// <summary>
        /// 客户端访问的监听端口，用于需要使用端口映射服务。
        /// </summary>
        public int RegisterPort;
        /// <summary>
        /// 客户端访问的监听端口，用于需要使用端口映射服务。
        /// </summary>
        int TcpRegister.IServerAttribute.ClientRegisterPort
        {
            get { return RegisterPort; }
            set { RegisterPort = value; }
        }
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数，默认为 128KB。
        /// </summary>
        public SubBuffer.Size SendBufferSize = SubBuffer.Size.Kilobyte128;
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override SubBuffer.Size GetSendBufferSize { get { return SendBufferSize; } }
        /// <summary>
        /// 服务器端发送数据缓冲区最大字节数，默认为 1MB。
        /// </summary>
        public int ServerSendBufferMaxSize = 1 << 20;
        /// <summary>
        /// 服务器端发送数据缓冲区最大字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetServerSendBufferMaxSize { get { return ServerSendBufferMaxSize; } }
        /// <summary>
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数，默认为 128KB。
        /// </summary>
        public SubBuffer.Size ReceiveBufferSize = SubBuffer.Size.Kilobyte128;
        /// <summary>
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override SubBuffer.Size GetReceiveBufferSize { get { return ReceiveBufferSize; } }
        /// <summary>
        /// 客户端保持连接心跳包间隔时间默认为 1 秒，对于频率稳定可靠的服务类型可以设置为 0 禁用心跳包。
        /// </summary>
        public int CheckSeconds = 1;
        /// <summary>
        /// 客户端保持连接心跳包间隔时间
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetCheckSeconds { get { return CheckSeconds; } }
        /// <summary>
        /// 客户端接收命令默认超时为 4 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        internal const int DefaultReceiveVerifyCommandSeconds = 4;
        /// <summary>
        /// 客户端接收命令超时为 4 秒，超时客户端将被当作攻击者被抛弃。
        /// </summary>
        public int ReceiveVerifyCommandSeconds = DefaultReceiveVerifyCommandSeconds;
        /// <summary>
        /// 客户端重建连接休眠毫秒数，默认为 10
        /// </summary>
        public int ClientTryCreateSleep = 10;
        /// <summary>
        /// 客户端重建连接休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetClientTryCreateSleep { get { return ClientTryCreateSleep; } }
        /// <summary>
        /// 客户端第一次重建连接休眠毫秒数，默认为 10
        /// </summary>
        public int ClientFirstTryCreateSleep = 10;
        /// <summary>
        /// 客户端第一次重建连接休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetClientFirstTryCreateSleep { get { return ClientFirstTryCreateSleep; } }
        /// <summary>
        /// 批量处理休眠毫秒数，默认为 0 适应于低延时高频串行调用，对于延时要求不高只要求吞吐量的情况可以设置为 1 以减少套接字调用次数
        /// </summary>
        public int ClientOutputSleep;
        /// <summary>
        /// 批量处理休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetClientOutputSleep { get { return ClientOutputSleep; } }
        /// <summary>
        /// 批量处理休眠毫秒数，默认为 0 适应于低延时高频串行调用，对于延时要求不高只要求吞吐量的情况可以设置为 1 以减少套接字调用次数
        /// </summary>
        public int ServerOutputSleep;
        /// <summary>
        /// 批量处理休眠毫秒数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetServerOutputSleep { get { return ServerOutputSleep; } }
        /// <summary>
        /// 当需要将客户端提供给第三方使用的时候，可能不希望 dll 中同时包含服务端，设置为 true 会将客户端代码单独剥离出来生成一个代码文件 {项目名称}.tcpServer.服务名称.client.cs，当然你需要将服务中所有参数与返回值及其依赖的数据类型剥离出来。
        /// </summary>
        public bool IsSegmentation;
        /// <summary>
        /// 当需要将客户端提供给第三方使用的时候，可能不希望 dll 中同时包含服务端，设置为 true 会将客户端代码单独剥离出来生成一个代码文件 {项目名称}.tcpServer.服务名称.client.cs，当然你需要将服务中所有参数与返回值及其依赖的数据类型剥离出来。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsSegmentation { get { return IsSegmentation; } }
        /// <summary>
        /// 默认使用二进制序列化，适合参数数据类型稳定的服务，或者可以同步部署服务器端与客户端的内部服务。对于数据类型不稳定的互联网服务应该使用 JSON 序列化。
        /// </summary>
        public bool IsJsonSerialize;
        /// <summary>
        /// 是否使用 JSON 序列化
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsJsonSerialize { get { return IsJsonSerialize; } }
        /// <summary>
        /// 服务端创建输出是否开启线程，默认为 true
        /// </summary>
        public bool IsServerBuildOutputThread = true;
        /// <summary>
        /// 服务端创建输出是否开启线程
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsServerBuildOutputThread { get { return IsServerBuildOutputThread; } }
        /// <summary>
        /// 默认为 true 表示在创建客户端对象的时候自动启动连接，否则需要第一次调用触发
        /// </summary>
        public bool IsAutoClient = true;
        /// <summary>
        /// 是否自动创建客户端对象
        /// </summary>
        public bool IsAuto
        {
            get { return IsAutoClient && TcpRegisterName == null; }
        }

        /// <summary>
        /// 默认为 true 表示生成记忆数字编号标识与长字符串名称标识之间对应关系的代码，用于保持多次代码生成的命令序号
        /// </summary>
        public bool IsRememberCommand = true;
        /// <summary>
        /// 默认为 true 表示只允许注册一个 TCP 服务实例（单例服务，其它服务的注册将失败），但 false 并不代表支持负载均衡（仅仅是在客户端访问某个服务端失败时可以切换到其他服务端连接）。
        /// </summary>
        public bool IsSingleRegister = true;
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        public int BinaryDeSerializeMaxArraySize = AutoCSer.BinarySerialize.DeSerializer.DefaultConfig.MaxArraySize;
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        internal override int GetBinaryDeSerializeMaxArraySize { get { return BinaryDeSerializeMaxArraySize; } }
    }
}

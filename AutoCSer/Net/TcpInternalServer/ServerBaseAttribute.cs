﻿using System;
using AutoCSer.Metadata;
using AutoCSer.Net.TcpRegister;

namespace AutoCSer.Net.TcpInternalServer
{
    /// <summary>
    /// TCP 内部服务配置
    /// </summary>
    public abstract class ServerBaseAttribute : TcpServer.ServerAttribute
    {
        /// <summary>
        /// 注册当前服务的 TCP 注册服务名称。
        /// </summary>
        public string TcpRegister;
        /// <summary>
        /// 注册当前服务的 TCP 注册服务名称。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override string TcpRegisterName
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
        [AutoCSer.Metadata.Ignore]
        internal override string ClientRegisterHost
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
        [AutoCSer.Metadata.Ignore]
        internal override int ClientRegisterPort
        {
            get { return RegisterPort; }
            set { RegisterPort = value; }
        }
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数，默认为 128KB。
        /// </summary>
        public AutoCSer.Memory.BufferSize SendBufferSize = AutoCSer.Memory.BufferSize.Kilobyte128;
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override AutoCSer.Memory.BufferSize GetSendBufferSize { get { return SendBufferSize; } }
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
        public AutoCSer.Memory.BufferSize ReceiveBufferSize = AutoCSer.Memory.BufferSize.Kilobyte128;
        /// <summary>
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override AutoCSer.Memory.BufferSize GetReceiveBufferSize { get { return ReceiveBufferSize; } }
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
        /// 客户端接收命令超时
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetReceiveVerifyCommandSeconds { get { return ReceiveVerifyCommandSeconds; } }
        /// <summary>
        /// 客户端等待连接毫秒数，默认为 0 表示等待直到成功或者失败
        /// </summary>
        public uint ClientWaitConnectedMilliseconds;
        /// <summary>
        /// 客户端等待连接毫秒数，默认为 0 表示等待直到成功或者失败
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override uint GetClientWaitConnectedMilliseconds { get { return ClientWaitConnectedMilliseconds; } }
        /// <summary>
        /// 客户端最大自定义数据包字节大小，默认为 0 表示不限
        /// </summary>
        public int MaxCustomDataSize;
        /// <summary>
        /// 客户端最大自定义数据包字节大小
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetMaxCustomDataSize { get { return MaxCustomDataSize; } }
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
        /// 客户端批量处理等待类型，默认为 ThreadYield
        /// </summary>
        public TcpServer.OutputWaitType ClientOutputWaitType = TcpServer.OutputWaitType.ThreadYield;
        /// <summary>
        /// 客户端批量处理等待类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override TcpServer.OutputWaitType GetClientOutputWaitType { get { return ClientOutputWaitType; } }
        /// <summary>
        /// 服务端批量处理等待类型，默认为 ThreadYield
        /// </summary>
        public TcpServer.OutputWaitType ServerOutputWaitType = TcpServer.OutputWaitType.ThreadYield;
        /// <summary>
        /// 服务端批量处理等待类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override TcpServer.OutputWaitType GetServerOutputWaitType { get { return ServerOutputWaitType; } }
        /// <summary>
        /// 客户端最大未处理命令数量，默认为 65536
        /// </summary>
        public int QueueCommandSize = 1 << 16;
        /// <summary>
        /// 客户端最大未处理命令数量
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetQueueCommandSize { get { return QueueCommandSize; } }
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
        /// 命令池初始化二进制大小，默认为 8
        /// </summary>
        public byte CommandPoolBitSize = 8;
        /// <summary>
        /// 命令池初始化二进制大小 2^n
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override byte GetCommandPoolBitSize { get { return CommandPoolBitSize; } }
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        public int BinaryDeSerializeMaxArraySize = AutoCSer.BinaryDeSerializer.DefaultConfig.MaxArraySize;
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override int GetBinaryDeSerializeMaxArraySize { get { return BinaryDeSerializeMaxArraySize; } }
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
        /// 默认为 true 表示只允许注册一个 TCP 服务实例（单例服务，其它服务的注册将失败），但 false 并不代表支持负载均衡（仅仅是在客户端访问某个服务端失败时可以切换到其他服务端连接）。
        /// </summary>
        public bool IsSingleRegister = true;
        /// <summary>
        /// true 表示只允许注册一个 TCP 服务实例（单例服务，其它服务的注册将失败），但 false 并不代表支持负载均衡（仅仅是在客户端访问某个服务端失败时可以切换到其他服务端连接）。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsSingleRegister
        {
            get { return IsSingleRegister; }
        }
        /// <summary>
        /// 默认为 false 表示为可替换服务，true 表示主服务
        /// </summary>
        public bool IsMainRegister = false;
        /// <summary>
        /// true 表示主服务，否则为可替换服务
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsMainRegister { get { return IsMainRegister; } }
        /// <summary>
        /// 服务端自定义队列类型，需要继承自 AutoCSer.Net.TcpServer.IServerCallQueueSet
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public Type ServerCallQueueType;
        /// <summary>
        /// 服务端自定义队列类型
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override Type GetServerCallQueueType { get { return ServerCallQueueType; } }
    }
}

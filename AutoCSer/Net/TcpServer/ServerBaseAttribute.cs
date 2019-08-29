using System;
using AutoCSer.Metadata;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务配置
    /// </summary>
    public abstract class ServerBaseAttribute : Attribute
    {
        /// <summary>
        /// 服务名称的唯一标识，默认匹配的配置文件 Key 的后缀（Net.Tcp.CommandServerAttribute.Service），TCP 注册服务中注册的当前服务名称。对于 tcpCall 必填，并且必须可以作为合法的 C# 类型名称使用。
        /// </summary>
        public string Name;
        /// <summary>
        /// 服务名称
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        public virtual string ServerName
        {
            get { return Name; }
        }
        /// <summary>
        /// 服务主机名称或者 IP 地址，无法解析时默认使用 IPAddress.Any，比如 "www.autocser.com" 或者 "127.0.0.1"
        /// </summary>
        public string Host;
        /// <summary>
        /// 服务监听端口(服务配置)
        /// </summary>
        public int Port;
        /// <summary>
        /// 服务默认验证字符串，AutoCSer.Net.Tcp.TimeVerifyServer 用到了该字符串。
        /// </summary>
        public string VerifyString;
        /// <summary>
        /// 附加验证字符串信息哈希值
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        private ulong? verifyHashCode;
        /// <summary>
        /// 附加验证字符串信息哈希值
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal unsafe ulong VerifyHashCode
        {
            get
            {
                if (verifyHashCode == null)
                {
                    fixed (char* verifyFixed = VerifyString) verifyHashCode = (ulong)Memory.GetHashCode64((byte*)verifyFixed, VerifyString.Length << 1);
                }
                return verifyHashCode.Value;
            }
        }
        /// <summary>
        /// 最大验证字节数量，默认为 256 字节
        /// </summary>
        public int MaxVerifyDataSize = 1 << 8;
        /// <summary>
        /// 成员选择类型
        /// </summary>
        internal abstract MemberFilters GetMemberFilters { get; }
        /// <summary>
        /// 当 IsSegmentation = true 时，对于剥离出来的客户端代码指定需要复制的目标路径，也就是你的客户端所在的项目路径。
        /// </summary>
        public string ClientSegmentationCopyPath;
        /// <summary>
        /// 当需要将客户端提供给第三方使用的时候，可能不希望 dll 中同时包含服务端，设置为 true 会将客户端代码单独剥离出来生成一个代码文件 {项目名称}.tcpServer.服务名称.client.cs，当然你需要将服务中所有参数与返回值及其依赖的数据类型剥离出来。
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract bool GetIsSegmentation { get; }
        /// <summary>
        /// 服务器端发送数据（包括客户端接受数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract SubBuffer.Size GetSendBufferSize { get; }
        /// <summary>
        /// 服务器端接受数据（包括客户端发送数据）缓冲区初始化字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract SubBuffer.Size GetReceiveBufferSize { get; }
        /// <summary>
        /// 服务器端发送数据缓冲区最大字节数
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract int GetServerSendBufferMaxSize { get; }
        /// <summary>
        /// 客户端发送数据缓冲区最大字节数，默认为 1MB。
        /// </summary>
        public int ClientSendBufferMaxSize = 1 << 20;
        /// <summary>
        /// 压缩启用最低字节数量，默认为 0 表示不压缩数据；压缩数据需要消耗一定的 CPU 资源降低带宽使用。
        /// </summary>
        public int MinCompressSize;
        /// <summary>
        /// 压缩启用最低字节数量
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal virtual int GetMinCompressSize { get { return MinCompressSize; } }
        /// <summary>
        /// 客户端保持连接心跳包间隔时间
        /// </summary>
        internal abstract int GetCheckSeconds { get; }
        /// <summary>
        /// 提供当前类型的一个泛型实例类型，用于获取命令序号记忆数据
        /// </summary>
        public Type GenericType;
        /// <summary>
        /// 是否使用 JSON 序列化
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal abstract bool GetIsJsonSerialize { get; }
        /// <summary>
        /// 默认为 true 表示在创建服务对象的时候自动启动监听，否则需要手动 Start
        /// </summary>
        public bool IsAutoServer = true;
        /// <summary>
        /// 成员是否匹配自定义属性类型，默认为 true 表示代码生成仅选择申明了 AutoCSer.Net.Tcp.MethodAttribute 的函数，否则选择所有函数。对于 tcpCall 有效域为当前 class。
        /// </summary>
        public bool IsAttribute = true;
        /// <summary>
        /// 指定是否搜索该成员的继承链以查找这些特性，参考System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)。对于 tcpCall 有效域为当前 class。
        /// </summary>
        public bool IsBaseTypeAttribute;
        /// <summary>
        /// 默认为 false 表示传输原始数据，否则传输简单变换处理后的数据，作用于继承自 AutoCSer.Net.Tcp.TimeVerifyServer 的服务端。
        /// </summary>
        public bool IsMarkData;
        /// <summary>
        /// 默认为 true 表示支持简单序列化操作
        /// </summary>
        public bool IsSimpleSerialize = true;
        /// <summary>
        /// 默认为 true 表示在初始化的时候启动序列化预编译任务
        /// </summary>
        public bool IsCompileSerialize = true;
        /// <summary>
        /// 默认为 false 表示不支持远程表达式
        /// </summary>
        public bool IsRemoteExpression;
        /// <summary>
        /// 默认为 false 表示客户端 API 公共可见，设置为 true 表示仅程序集可见
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal virtual bool GetIsInternalClient { get { return false; } }
        /// <summary>
        /// TCP 客户端路由类型，需要继承自 AutoCSer.Net.TcpServer.ClientRoute[]
        /// </summary>
        public Type ClientRouteType;
        /// <summary>
        /// 二进制反序列化数组最大长度
        /// </summary>
        internal abstract int GetBinaryDeSerializeMaxArraySize { get; }

        internal static attributeType GetConfig<attributeType>(string serviceName, Type type, attributeType attribute)
            where attributeType : ServerBaseAttribute
        {
            if (attribute == null && type != null)
            {
                attribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<attributeType>(type, false);
                if (attribute != null) attribute = AutoCSer.MemberCopy.Copyer<attributeType>.MemberwiseClone(attribute);
            }
            if (attribute.Name == null) attribute.Name = serviceName;
            return attribute;
        }
    }
}

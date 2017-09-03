using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpStaticStreamServer
{
    /// <summary>
    /// TCP 静态服务配置
    /// </summary>
    public class ServerAttribute : TcpInternalStreamServer.ServerBaseAttribute
    {
        /// <summary>
        /// 成员选择类型，为了防止调用者混淆了远程函数与本地函数在某些情况下产生误调用，默认只选择受保护的方法生成（包括 private / protected / internal）相关代码。
        /// </summary>
        public MemberFilters MemberFilters = MemberFilters.NonPublicStatic;
        /// <summary>
        /// 成员选择类型
        /// </summary>
        internal override MemberFilters GetMemberFilters { get { return MemberFilters; } }
        /// <summary>
        /// 是否 TCP 服务配置。一个静态服务只能存在一个 class 配置 IsServer = true，并且必须指定 Service，用于这个服务名称绑定 TCP 服务配置。
        /// </summary>
        public bool IsServer;
        /// <summary>
        /// 是否支持抽象类
        /// </summary>
        public bool IsAbstract;

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="serviceName">TCP 调用服务名称</param>
        /// <param name="type">TCP 服务器类型</param>
        /// <returns>TCP 调用服务器端配置信息</returns>
        public static TcpInternalStreamServer.ServerAttribute GetConfig(string serviceName, Type type)
        {
            TcpInternalStreamServer.ServerAttribute attribute = new TcpInternalStreamServer.UnionType { Value = AutoCSer.Config.Loader.GetObject(typeof(TcpInternalStreamServer.ServerAttribute), serviceName) }.ServerAttribute;
            if (attribute == null && type != null)
            {
                ServerAttribute staticAttribute = AutoCSer.Metadata.TypeAttribute.GetAttribute<ServerAttribute>(type, false);
                AutoCSer.MemberCopy.Copyer<TcpInternalStreamServer.ServerBaseAttribute>.Copy(attribute = new TcpInternalStreamServer.ServerAttribute(), staticAttribute);
            }
            if (attribute == null) attribute = new TcpInternalStreamServer.ServerAttribute();
            if (attribute.Name == null) attribute.Name = serviceName;
            return attribute;
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="serviceName">TCP 调用服务名称</param>
        /// <param name="type">TCP 服务器类型</param>
        /// <param name="isServer">是否服务端</param>
        /// <returns>TCP 调用服务器端配置信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static TcpInternalStreamServer.ServerAttribute GetConfig(string serviceName, Type type, bool isServer)
        {
            TcpInternalStreamServer.ServerAttribute attribute = GetConfig(serviceName, type);
            attribute.IsServer = isServer;
            return attribute;
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="serviceName">TCP 调用服务名称</param>
        /// <param name="isServer">是否服务端</param>
        /// <returns>TCP 调用服务器端配置信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static TcpInternalStreamServer.ServerAttribute GetConfig(string serviceName, bool isServer)
        {
            return GetConfig(serviceName, null, isServer);
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="serviceName">TCP 调用服务名称</param>
        /// <returns>TCP 调用服务器端配置信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static TcpInternalStreamServer.ServerAttribute GetConfig(string serviceName)
        {
            return GetConfig(serviceName, null);
        }
    }
}

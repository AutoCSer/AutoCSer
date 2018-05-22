using System;
using AutoCSer.Metadata;

namespace AutoCSer.Net.TcpInternalSimpleServer
{
    /// <summary>
    /// TCP 内部服务配置
    /// </summary>
    public class ServerAttribute : ServerBaseAttribute
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
        /// 用于在配置文件中标识当前程序是否服务端，当在标识为服务端的环境中使用客户端调用时会输出警告日志，提示用户判断是否混淆了客户端与服务端。
        /// </summary>
        public bool IsServer;
        /// <summary>
        /// 默认为 false 表示客户端 API 公共可见，设置为 true 表示仅程序集可见
        /// </summary>
        public bool IsInternalClient;
        /// <summary>
        /// 客户端 API 是否仅程序集可见
        /// </summary>
        [AutoCSer.Metadata.Ignore]
        internal override bool GetIsInternalClient { get { return IsInternalClient; } }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="serviceName">TCP 服务注册名称</param>
        /// <param name="type">TCP 服务器类型</param>
        /// <returns>TCP 调用服务器端配置信息</returns>
        public static ServerAttribute GetConfig(string serviceName, Type type = null)
        {
            return GetConfig(serviceName, type, new UnionType { Value = AutoCSer.Config.Loader.GetObject(typeof(ServerAttribute), serviceName) }.ServerAttribute);
        }
    }
}

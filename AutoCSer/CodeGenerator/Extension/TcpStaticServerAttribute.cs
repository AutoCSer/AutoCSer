using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extension
{
    /// <summary>
    /// TCP 静态服务配置相关操作
    /// </summary>
    public static class TcpStaticServerAttribute
    {
        /// <summary>
        /// 复制 TCP 服务配置成员位图
        /// </summary>
#if DOTNET2
        private static readonly MemberMap<AutoCSer.Net.TcpStaticServer.ServerAttribute> copyMemberMap = new MemberMap<AutoCSer.Net.TcpStaticServer.ServerAttribute>.Builder(MemberMap<AutoCSer.Net.TcpStaticServer.ServerAttribute>.NewFull()).Clear("Name").Clear("IsAttribute").Clear("IsBaseTypeAttribute");
#else
        private static readonly MemberMap<AutoCSer.Net.TcpStaticServer.ServerAttribute> copyMemberMap = new MemberMap<AutoCSer.Net.TcpStaticServer.ServerAttribute>.Builder(MemberMap<AutoCSer.Net.TcpStaticServer.ServerAttribute>.NewFull()).Clear(value => value.Name).Clear(value => value.IsAttribute).Clear(value => value.IsBaseTypeAttribute);
#endif
        /// <summary>
        /// 复制 TCP 服务配置
        /// </summary>
        /// <param name="value">TCP 服务配置</param>
        /// <param name="copyValue">TCP 服务配置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyFrom(this AutoCSer.Net.TcpStaticServer.ServerAttribute value, AutoCSer.Net.TcpStaticServer.ServerAttribute copyValue)
        {
            AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpStaticServer.ServerAttribute>.Copy(value, copyValue, copyMemberMap);
        }

        /// <summary>
        /// 复制 TCP 服务配置成员位图
        /// </summary>
#if DOTNET2
        private static readonly MemberMap<AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute> simpleCopyMemberMap = new MemberMap<AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute>.Builder(MemberMap<AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute>.NewFull()).Clear("Name").Clear("IsAttribute").Clear("IsBaseTypeAttribute");
#else
        private static readonly MemberMap<AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute> simpleCopyMemberMap = new MemberMap<AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute>.Builder(MemberMap<AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute>.NewFull()).Clear(value => value.Name).Clear(value => value.IsAttribute).Clear(value => value.IsBaseTypeAttribute);
#endif
        /// <summary>
        /// 复制 TCP 服务配置
        /// </summary>
        /// <param name="value">TCP 服务配置</param>
        /// <param name="copyValue">TCP 服务配置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyFrom(this AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute value, AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute copyValue)
        {
            AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute>.Copy(value, copyValue, simpleCopyMemberMap);
        }

        /// <summary>
        /// 复制 TCP 服务配置成员位图
        /// </summary>
#if DOTNET2
        private static readonly MemberMap<AutoCSer.Net.TcpStaticStreamServer.ServerAttribute> streamCopyMemberMap = new MemberMap<AutoCSer.Net.TcpStaticStreamServer.ServerAttribute>.Builder(MemberMap<AutoCSer.Net.TcpStaticStreamServer.ServerAttribute>.NewFull()).Clear("Name").Clear("IsAttribute").Clear("IsBaseTypeAttribute");
#else
        private static readonly MemberMap<AutoCSer.Net.TcpStaticStreamServer.ServerAttribute> streamCopyMemberMap = new MemberMap<AutoCSer.Net.TcpStaticStreamServer.ServerAttribute>.Builder(MemberMap<AutoCSer.Net.TcpStaticStreamServer.ServerAttribute>.NewFull()).Clear(value => value.Name).Clear(value => value.IsAttribute).Clear(value => value.IsBaseTypeAttribute);
#endif
        /// <summary>
        /// 复制 TCP 服务配置
        /// </summary>
        /// <param name="value">TCP 服务配置</param>
        /// <param name="copyValue">TCP 服务配置</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void CopyFrom(this AutoCSer.Net.TcpStaticStreamServer.ServerAttribute value, AutoCSer.Net.TcpStaticStreamServer.ServerAttribute copyValue)
        {
            AutoCSer.MemberCopy.Copyer<AutoCSer.Net.TcpStaticStreamServer.ServerAttribute>.Copy(value, copyValue, streamCopyMemberMap);
        }
    }
}

using System;

namespace AutoCSer.Example.TcpStaticServer
{
    /// <summary>
    /// 实例对象调用链映射 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = ServerName.Example1)]
    partial class RemoteKey
    {
        /// <summary>
        /// 实例对象定位关键字配置
        /// </summary>
        [AutoCSer.Net.TcpStaticServer.RemoteKey]
        public int Id;
        /// <summary>
        /// 实例对象定位函数（一般的实现可能是获取缓存对象）
        /// </summary>
        /// <param name="id">实例对象定位关键字</param>
        /// <returns>目标实例对象</returns>
        [AutoCSer.Net.TcpStaticServer.RemoteKey]
        private static RemoteKey getRemoteKey(int id)
        {
            return new RemoteKey { Id = id };
        }
        /// <summary>
        /// 调用链目标成员测试
        /// </summary>
        [AutoCSer.Net.TcpStaticServer.RemoteMember(IsAwait = false)]
        internal int NextId
        {
            get { return Id + 1; }
        }
        /// <summary>
        /// 调用链目标函数测试
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AutoCSer.Net.TcpStaticServer.RemoteMethod(IsAwait = false)]
        internal int AddId(int value)
        {
            return Id + value;
        }

        /// <summary>
        /// 远程调用链中间节点测试
        /// </summary>
        internal struct Link
        {
            /// <summary>
            /// 实例对象调用链映射
            /// </summary>
            public RemoteKey Value;
            /// <summary>
            /// 调用链目标成员测试
            /// </summary>
            [AutoCSer.Net.TcpStaticServer.RemoteMember(IsAwait = false)]
            internal int NextId
            {
                get { return Value.NextId; }
            }
            /// <summary>
            /// 调用链目标函数测试
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            [AutoCSer.Net.TcpStaticServer.RemoteMethod(IsAwait = false, NameType = AutoCSer.Net.TcpStaticServer.RemoteMemberAttribute.Type.Join)]
            internal int AddId(int value)
            {
                return Value.AddId(value);
            }
        }
        /// <summary>
        /// 远程调用链中间节点测试
        /// </summary>
        [AutoCSer.Net.TcpStaticServer.RemoteLink]
        internal Link RemoteLink
        {
            get
            {
                return new Link { Value = this };
            }
        }


        /// <summary>
        /// 实例对象调用链映射测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            RemoteKey value = new RemoteKey { Id = 1 };
#if NotSegmentation
            if (value.Remote.NextId != value.NextId)
            {
                return false;
            }

            if (value.Remote.AddId(2) != value.AddId(2))
            {
                return false;
            }

            if (value.Remote.RemoteLinkNextId != value.RemoteLink.NextId)
            {
                return false;
            }

            if (value.Remote.RemoteLink_AddId(3) != value.RemoteLink.AddId(3))
            {
                return false;
            }
#else
            TcpCall.RemoteKey.RemoteExtension remote = TcpCall.RemoteKey.Remote(value.Id);
            if (remote.NextId != value.NextId)
            {
                return false;
            }

            if (remote.AddId(2) != value.AddId(2))
            {
                return false;
            }

            if (remote.RemoteLinkNextId != value.RemoteLink.NextId)
            {
                return false;
            }

            if (remote.RemoteLink_AddId(3) != value.RemoteLink.AddId(3))
            {
                return false;
            }
#endif
            return true;
        }
    }
}

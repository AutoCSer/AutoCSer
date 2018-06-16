using System;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.ShortPath
{
    /// <summary>
    /// 短路径节点
    /// </summary>
    public abstract partial class Node
    {
        /// <summary>
        /// 数据结构定义节点
        /// </summary>
        protected readonly DataStructure.Abstract.Node node;
        /// <summary>
        /// 客户端
        /// </summary>
        internal readonly Client Client;
        /// <summary>
        /// 查询参数
        /// </summary>
        internal readonly DataStructure.Parameter.Value Parameter;
        /// <summary>
        /// 重建访问锁
        /// </summary>
        private readonly object createLock;
        /// <summary>
        /// 客户端套接字编号
        /// </summary>
        protected ulong socketIdentity;
        /// <summary>
        /// 短路径索引标识
        /// </summary>
        internal ReturnValue<ShortPathIdentity> Identity;
        /// <summary>
        /// 哈希表节点 短路径
        /// </summary>
        /// <param name="node"></param>
        protected Node(DataStructure.Abstract.Node node)
        {
            if (node.Parent != null)
            {
                this.node = node;
                Client = node.ClientDataStructure.Client;
                Parameter = new DataStructure.Parameter.Value(node.Parent);
                Parameter.Parameter = node.Parameter;
                Parameter.Parameter.OperationType = OperationParameter.OperationType.CreateShortPath;
                createLock = new object();
            }
        }
        /// <summary>
        /// 判断短路径是否创建成功
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool isCreate(ref AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            Identity = value.Value.Parameter.GetBinary<ShortPathIdentity>(value.Type);
            if (Identity.Type == ReturnType.Success)
            {
                if (Identity.Value.Identity != 0) return true;
                Identity.Type = ReturnType.NotFoundShortPathNode;
            }
            return false;
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        protected void create(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onCreated)
        {
            socketIdentity = Client.SocketIdentity;
            Client.Query(Parameter, value =>
            {
                if (socketIdentity == Client.SocketIdentity) onCreated(value);
                else create(onCreated);
            });
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        protected void createStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onCreated)
        {
            socketIdentity = Client.SocketIdentity;
            Client.QueryStream(Parameter, value =>
            {
                if (socketIdentity == Client.SocketIdentity) onCreated(value);
                else createStream(onCreated);
            });
        }

        /// <summary>
        /// 检测短路径的有效性
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnType Check(MasterServer.TcpInternalClient client)
        {
            switch (Identity.Type)
            {
                case ReturnType.NotFoundShortPathNode: return ReturnType.NotFoundShortPathNode;
                case ReturnType.NotFoundShortPath: return check(client);
            }
            if(socketIdentity != Client.SocketIdentity) return check(client);
            return Identity.Type;
        }
        /// <summary>
        /// 检测短路径的有效性
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private ReturnType check(MasterServer.TcpInternalClient client)
        {
            Monitor.Enter(createLock);
            switch (Identity.Type)
            {
                case ReturnType.NotFoundShortPathNode: Monitor.Exit(createLock); return ReturnType.NotFoundShortPathNode;
                case ReturnType.NotFoundShortPath: reCreate(client); return Identity.Type;
            }
            if (socketIdentity == Client.SocketIdentity) Monitor.Exit(createLock);
            else reCreate(client);
            return Identity.Type;
        }
        /// <summary>
        /// 重建短路径
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private bool reCreate(MasterServer.TcpInternalClient client)
        {
            try
            {
                do
                {
                    socketIdentity = Client.SocketIdentity;
                    AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = client.Query(new OperationParameter.QueryNode { Node = Parameter });
                    if (socketIdentity == Client.SocketIdentity) return isCreate(ref value);
                }
                while (true);
            }
            catch (Exception error)
            {
                client._TcpClient_.AddLog(error);
            }
            finally { Monitor.Exit(createLock); }
            return false;
        }
        /// <summary>
        /// 重建短路径
        /// </summary>
        /// <param name="client"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        internal bool ReCreate(MasterServer.TcpInternalClient client, ref ReturnType returnType)
        {
            switch (returnType)
            {
                case ReturnType.NotFoundShortPathNode: Identity.Type = ReturnType.NotFoundShortPathNode; return false;
                case ReturnType.NotFoundShortPath:
                    Monitor.Enter(createLock);
                    if (Identity.Type != ReturnType.NotFoundShortPathNode)
                    {
                        Identity.Type = ReturnType.NotFoundShortPath;
                        if (reCreate(client)) return true;
                    }
                    else Monitor.Exit(createLock);
                    break;
            }
            returnType = Identity.Type;
            return false;
        }

        /// <summary>
        /// 检测短路径的有效性
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnType Check(SlaveServer.TcpInternalClient client)
        {
            switch (Identity.Type)
            {
                case ReturnType.NotFoundShortPathNode: return ReturnType.NotFoundShortPathNode;
                case ReturnType.NotFoundShortPath: return check(client);
            }
            if (socketIdentity != Client.SocketIdentity) return check(client);
            return Identity.Type;
        }
        /// <summary>
        /// 检测短路径的有效性
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private ReturnType check(SlaveServer.TcpInternalClient client)
        {
            Monitor.Enter(createLock);
            switch (Identity.Type)
            {
                case ReturnType.NotFoundShortPathNode: Monitor.Exit(createLock); return ReturnType.NotFoundShortPathNode;
                case ReturnType.NotFoundShortPath: reCreate(client); return Identity.Type;
            }
            if (socketIdentity == Client.SocketIdentity) Monitor.Exit(createLock);
            else reCreate(client);
            return Identity.Type;
        }
        /// <summary>
        /// 重建短路径
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private bool reCreate(SlaveServer.TcpInternalClient client)
        {
            try
            {
                do
                {
                    socketIdentity = Client.SocketIdentity;
                    AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = client.Query(new OperationParameter.QueryNode { Node = Parameter });
                    if (socketIdentity == Client.SocketIdentity) return isCreate(ref value);
                }
                while (true);
            }
            catch (Exception error)
            {
                client._TcpClient_.AddLog(error);
            }
            finally { Monitor.Exit(createLock); }
            return false;
        }
        /// <summary>
        /// 重建短路径
        /// </summary>
        /// <param name="client"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        internal bool ReCreate(SlaveServer.TcpInternalClient client, ref ReturnType returnType)
        {
            switch (returnType)
            {
                case ReturnType.NotFoundShortPathNode: Identity.Type = ReturnType.NotFoundShortPathNode; return false;
                case ReturnType.NotFoundShortPath:
                    Monitor.Enter(createLock);
                    if (Identity.Type != ReturnType.NotFoundShortPathNode)
                    {
                        Identity.Type = ReturnType.NotFoundShortPath;
                        if (reCreate(client)) return true;
                    }
                    else Monitor.Exit(createLock);
                    break;
            }
            returnType = Identity.Type;
            return false;
        }
    }
    /// <summary>
    /// 短路径节点
    /// </summary>
    /// <typeparam name="nodeType">节点类型</typeparam>
    public abstract partial class Node<nodeType> : Node
        where nodeType : Node<nodeType>
    {
        /// <summary>
        /// 短路径节点
        /// </summary>
        /// <param name="node"></param>
        protected Node(DataStructure.Abstract.Node node) : base(node) { }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<nodeType> Create()
        {
            do
            {
                socketIdentity = Client.SocketIdentity;
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = Client.Query(Parameter);
                if (socketIdentity == Client.SocketIdentity)
                {
                    if(isCreate(ref value)) return (nodeType)this;
                    return new ReturnValue<nodeType> { Type = Identity.Type, TcpReturnType = Identity.TcpReturnType };
                }
            }
            while (true);
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Create(Action<ReturnValue<nodeType>> onCreated)
        {
            create(value =>
            {
                if (isCreate(ref value)) onCreated((nodeType)this);
                else onCreated(new ReturnValue<nodeType> { Type = Identity.Type, TcpReturnType = Identity.TcpReturnType });
            });
        }
        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="onCreated"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CreateStream(Action<ReturnValue<nodeType>> onCreated)
        {
            createStream(value =>
            {
                if (isCreate(ref value)) onCreated((nodeType)this);
                else onCreated(new ReturnValue<nodeType> { Type = Identity.Type, TcpReturnType = Identity.TcpReturnType });
            });
        }
    }
}

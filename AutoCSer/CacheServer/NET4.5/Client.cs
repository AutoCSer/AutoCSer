using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        public async Task<ReturnValue<bool>> RemoveDataStructureTask(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName)) return new ReturnValue<bool> { Type = ReturnType.NullArgument };
            IndexIdentity identity = default(IndexIdentity);
            identity.Set(await MasterClient.RemoveAwaiter(new OperationParameter.RemoveDataStructure { CacheName = cacheName }));
            if (identity.ReturnType == ReturnType.Success) removeDataStructure(cacheName, ref identity);
            return new ReturnValue<bool> { TcpReturnType = identity.TcpReturnType, Type = identity.ReturnType, Value = identity.ReturnType == ReturnType.Success };
        }

        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.Awaiter WriteFileAwaiter()
        {
            return MasterClient.WriteFileAwaiter();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.AwaiterBox<ReturnParameter> QueryAwaiter(DataStructure.Abstract.Node node)
        {
            if (isMasterClient) return MasterClient.QueryAwaiter(new OperationParameter.QueryNode { Node = node });
            return slaveClient.QueryAwaiter(new OperationParameter.QueryNode { Node = node });
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.AwaiterBox<ReturnParameter> QueryAsynchronousAwaiter(DataStructure.Abstract.Node node)
        {
            if (isMasterClient) return MasterClient.QueryAsynchronousAwaiter(new OperationParameter.QueryNode { Node = node });
            return slaveClient.QueryAsynchronousAwaiter(new OperationParameter.QueryNode { Node = node });
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.AwaiterBox<ReturnParameter> MasterQueryAsynchronousAwaiter(DataStructure.Abstract.Node node)
        {
            return MasterClient.QueryAsynchronousAwaiter(new OperationParameter.QueryNode { Node = node });
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.AwaiterBox<ReturnParameter> OperationAwaiter(DataStructure.Abstract.Node node)
        {
            return MasterClient.OperationAwaiter(new OperationParameter.OperationNode { Node = node });
        }
        /// <summary>
        /// 异步操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal AutoCSer.Net.TcpServer.AwaiterBox<ReturnParameter> OperationAsynchronousAwaiter(DataStructure.Abstract.Node node)
        {
            return MasterClient.OperationAsynchronousAwaiter(new OperationParameter.OperationNode { Node = node });
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal async Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> QueryAwaiter(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType;
            if (isMasterClient)
            {
                if ((returnType = shortPath.Check(MasterClient)) == ReturnType.Success)
                {
                    AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await MasterClient.QueryAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        return await MasterClient.QueryAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                    }
                    return value;
                }
            }
            else if ((returnType = shortPath.Check(slaveClient)) == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await slaveClient.QueryAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(slaveClient, ref value.Value.Parameter.ReturnType))
                {
                    return await slaveClient.QueryAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }
        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal async Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> QueryAsynchronousAwaiter(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType;
            if (isMasterClient)
            {
                if ((returnType = shortPath.Check(MasterClient)) == ReturnType.Success)
                {
                    AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await MasterClient.QueryAsynchronousAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                    if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                    {
                        return await MasterClient.QueryAsynchronousAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                    }
                    return value;
                }
            }
            else if ((returnType = shortPath.Check(slaveClient)) == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await slaveClient.QueryAsynchronousAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(slaveClient, ref value.Value.Parameter.ReturnType))
                {
                    return await slaveClient.QueryAsynchronousAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal async Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> OperationAwaiter(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await MasterClient.OperationAwaiter(new OperationParameter.ShortPathOperationNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                {
                    return await MasterClient.OperationAwaiter(new OperationParameter.ShortPathOperationNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }
        /// <summary>
        /// 异步操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal async Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> OperationAsynchronousAwaiter(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await MasterClient.OperationAsynchronousAwaiter(new OperationParameter.ShortPathOperationNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                {
                    return await MasterClient.OperationAsynchronousAwaiter(new OperationParameter.ShortPathOperationNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }

        /// <summary>
        /// 异步查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal async Task<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> MasterQueryAsynchronousAwaiter(ShortPath.Parameter.Node node)
        {
            ShortPath.Node shortPath = node.ShortPath;
            ReturnType returnType = shortPath.Check(MasterClient);
            if (returnType == ReturnType.Success)
            {
                AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value = await MasterClient.QueryAsynchronousAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                if (value.Type == Net.TcpServer.ReturnType.Success && shortPath.ReCreate(MasterClient, ref value.Value.Parameter.ReturnType))
                {
                    return await MasterClient.QueryAsynchronousAwaiter(new OperationParameter.ShortPathQueryNode { Node = node });
                }
                return value;
            }
            return new ReturnParameter(returnType);
        }
    }
}

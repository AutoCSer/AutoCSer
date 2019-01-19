using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.RemoteExpression
{
    /// <summary>
    /// 客户端检测服务端映射标识
    /// </summary>
    public abstract class ServerNodeIdChecker
    {
        /// <summary>
        /// 请求数据
        /// </summary>
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        internal struct Input
        {
            /// <summary>
            /// 远程表达式服务端节点标识解析命令信息
            /// </summary>
            internal static readonly TcpServer.CommandInfo CommandInfo = new TcpServer.CommandInfo { Command = TcpServer.Server.RemoteExpressionNodeIdCommandIndex, InputParameterIndex = -TcpServer.Server.RemoteExpressionNodeIdCommandIndex };
            /// <summary>
            /// 表达式服务端节点类型集合
            /// </summary>
            internal RemoteType[] Types;
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
        internal struct Output
        {
            /// <summary>
            /// 表达式服务端节点标识集合
            /// </summary>
            internal int[] Return;
            /// <summary>
            /// 远程表达式服务端节点标识解析输出参数信息
            /// </summary>
            internal static readonly AutoCSer.Net.TcpServer.OutputInfo OutputInfo = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = -TcpServer.Server.RemoteExpressionNodeIdCommandIndex };
            /// <summary>
            /// 远程表达式服务端节点标识解析输出参数信息
            /// </summary>
            internal static readonly AutoCSer.Net.TcpServer.OutputInfo OutputThreadInfo = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = -TcpServer.Server.RemoteExpressionNodeIdCommandIndex, IsBuildOutputThread = true };
        }
        /// <summary>
        /// 服务端映射标识集合
        /// </summary>
        internal readonly Dictionary<Type, int> ServerNodeIds = DictionaryCreator.CreateOnly<Type, int>();
        /// <summary>
        /// 服务端映射标识集合访问锁
        /// </summary>
        private readonly object serverNodeIdLock = new object();
        /// <summary>
        /// 获取服务端映射标识
        /// </summary>
        /// <param name="nodeType">远程表达式节点类型</param>
        /// <returns>服务端映射标识</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public int Get(Type nodeType)
        {
            return ServerNodeIds[nodeType];
        }
        /// <summary>
        /// 获取服务端映射标识集合
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        internal abstract AutoCSer.Net.TcpServer.ReturnValue<int[]> Get(RemoteType[] types);
        /// <summary>
        /// 服务端映射标识检测
        /// </summary>
        /// <param name="node"></param>
        /// <returns>是否映射成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe AutoCSer.Net.TcpServer.ReturnType Check(Node node)
        {
            LeftArray<Type> types = default(LeftArray<Type>);
            return Check(node, ref types);
        }
        /// <summary>
        /// 服务端映射标识检测
        /// </summary>
        /// <param name="node"></param>
        /// <param name="types"></param>
        /// <returns>是否映射成功</returns>
        internal unsafe AutoCSer.Net.TcpServer.ReturnType Check(Node node, ref LeftArray<Type> types)
        {
            node.CheckServerNodeId(this, ref types);
            if (types.Length == 0) return AutoCSer.Net.TcpServer.ReturnType.Success;
            int count = types.Length;
            LeftArray<RemoteType> remoteTypes = new LeftArray<RemoteType>(types.Length);
            types.Length = 0;
            AutoCSer.Net.TcpServer.ReturnType returnType = AutoCSer.Net.TcpServer.ReturnType.Success;
            Monitor.Enter(serverNodeIdLock);
            try
            {
                foreach (Type type in types.Array)
                {
                    if (!ServerNodeIds.ContainsKey(type))
                    {
                        types.UnsafeAdd(type);
                        remoteTypes.Add(type);
                    }
                    if (--count == 0) break;
                }
                if ((count = types.Length) != 0)
                {
                    AutoCSer.Net.TcpServer.ReturnValue<int[]> ids = Get(remoteTypes.ToArray());
                    if (ids.Type == TcpServer.ReturnType.Success)
                    {
                        fixed (int* idFixed = ids.Value)
                        {
                            int* idStart = idFixed;
                            foreach (Type type in types.Array)
                            {
                                if (*idStart == 0) returnType = TcpServer.ReturnType.RemoteExpressionServerNodeError;
                                else ServerNodeIds[type] = *idStart;
                                if (--count == 0) break;
                                ++idStart;
                            }
                        }
                    }
                    else returnType = ids.Type;
                }
            }
            finally { Monitor.Exit(serverNodeIdLock); }
            return returnType;
        }
    }
}

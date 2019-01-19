using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.RemoteExpression
{
    /// <summary>
    /// 客户端远程表达式节点
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    public struct ClientNode
    {
        /// <summary>
        /// 远程表达式命令信息
        /// </summary>
        internal static readonly TcpServer.CommandInfo CommandInfo = new TcpServer.CommandInfo { Command = TcpServer.Server.RemoteExpressionCommandIndex, InputParameterIndex = -TcpServer.Server.RemoteExpressionCommandIndex };
        /// <summary>
        /// 远程表达式节点
        /// </summary>
        internal Node Node;
        /// <summary>
        /// 客户端检测服务端映射标识
        /// </summary>
        internal ServerNodeIdChecker Checker;
        /// <summary>
        /// 客户端映射标识
        /// </summary>
        internal int ClientNodeId;
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetNull()
        {
            Node = null;
            Checker = null;
        }
        /// <summary>
        /// 服务端获取返回值
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue GetReturnValue()
        {
            return Node.Get(ClientNodeId);
        }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            serializer.Stream.Write(ClientNodeId);
            Node.Serialize(serializer, Checker);
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            ClientNodeId = deSerializer.ReadInt();
            Node.DeSerialize(deSerializer, out Node);
        }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.Json.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.Json.Serializer serializer)
        {
            serializer.CharStream.Write('[');
            serializer.Serialize(ClientNodeId);
            serializer.CharStream.Write(',');
            Node.Serialize(serializer, Checker);
            serializer.CharStream.Write(']');
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="parser"></param>
        [AutoCSer.Json.ParseCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.Json.Parser parser)
        {
            if (*parser.Current++ == '[')
            {
                parser.Parse(ref ClientNodeId);
                if (parser.State == Json.ParseState.Success)
                {
                    if (*parser.Current++ == ',')
                    {
                        Node.DeSerialize(parser, ref Node);
                        if (parser.State != Json.ParseState.Success || *parser.Current++ == ']') return;
                    }
                }
                else return;
            }
            parser.ParseState = Json.ParseState.Custom;
        }
    }
    /// <summary>
    /// 客户端远程表达式参数节点
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    public struct ClientNode<returnType>
    {
        /// <summary>
        /// 远程表达式节点
        /// </summary>
        internal Node<returnType> Node;
        /// <summary>
        /// 客户端检测服务端映射标识
        /// </summary>
        internal ServerNodeIdChecker Checker;
        /// <summary>
        /// 服务端获取返回值
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public returnType GetReturnValue()
        {
            return Node.GetValue();
        }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            Node.Serialize(serializer, Checker);
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            Node node;
            RemoteExpression.Node.DeSerialize(deSerializer, out node);
            Node = (Node<returnType>)node;
        }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.Json.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.Json.Serializer serializer)
        {
            Node.Serialize(serializer, Checker);
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="parser"></param>
        [AutoCSer.Json.ParseCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.Json.Parser parser)
        {
            Node node = null;
            RemoteExpression.Node.DeSerialize(parser, ref node);
            if (parser.State != Json.ParseState.Success)
            {
                Node = (Node<returnType>)node;
                return;
            }
            parser.ParseState = Json.ParseState.Custom;
        }
    }
}

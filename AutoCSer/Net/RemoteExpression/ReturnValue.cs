using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.RemoteExpression
{
    /// <summary>
    /// 返回值
    /// </summary>
    [AutoCSer.Json.Serialize]
    [AutoCSer.Json.Parse]
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    public class ReturnValue
    {
        /// <summary>
        /// 输出数据
        /// </summary>
        internal struct Output
        {
            /// <summary>
            /// 返回值
            /// </summary>
            internal ReturnValue Return;
            /// <summary>
            /// 远程表达式服务端节点标识解析输出参数信息
            /// </summary>
            internal static readonly AutoCSer.Net.TcpServer.OutputInfo OutputInfo = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = -TcpServer.Server.RemoteExpressionCommandIndex };
            /// <summary>
            /// 远程表达式服务端节点标识解析输出参数信息
            /// </summary>
            internal static readonly AutoCSer.Net.TcpServer.OutputInfo OutputThreadInfo = new AutoCSer.Net.TcpServer.OutputInfo { OutputParameterIndex = -TcpServer.Server.RemoteExpressionCommandIndex, IsBuildOutputThread = true };
        }
        /// <summary>
        /// 客户端映射标识
        /// </summary>
        internal int ClientNodeId;
        /// <summary>
        /// 尝试获取泛型数据
        /// </summary>
        /// <typeparam name="returnType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TryGetValue<returnType>(ref returnType value)
        {
            ReturnValue<returnType> returnValue = this as ReturnValue<returnType>;
            if (returnValue != null)
            {
                value = returnValue.Value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取泛型数据
        /// </summary>
        /// <typeparam name="returnType"></typeparam>
        /// <param name="nullValue">失败默认值</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public returnType GetValue<returnType>(returnType nullValue)
        {
            ReturnValue<returnType> returnValue = this as ReturnValue<returnType>;
            return returnValue != null ? returnValue.Value : nullValue;
        }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        protected virtual void serialize(AutoCSer.BinarySerialize.Serializer serializer) { }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(AutoCSer.BinarySerialize.Serializer serializer, ReturnValue value)
        {
            if(value == null) serializer.Stream.Write(AutoCSer.BinarySerialize.Serializer.NullValue);
            else value.serialize(serializer);
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="deSerializer"></param>
        protected virtual void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer) { }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <param name="value"></param>
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer, ref ReturnValue value)
        {
            value = createReturnValues.Array[deSerializer.ReadInt()]();
            value.deSerialize(deSerializer);
        }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        protected virtual void serialize(AutoCSer.Json.Serializer serializer) { }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [AutoCSer.Json.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(AutoCSer.Json.Serializer serializer, ReturnValue value)
        {
            if (value == null) serializer.CharStream.WriteJsonArray();
            else
            {
                CharStream jsonStream = serializer.CharStream;
                jsonStream.Write('[');
                serializer.Serialize(value.ClientNodeId);
                jsonStream.Write(',');
                value.serialize(serializer);
                jsonStream.Write(']');
            }
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="parser"></param>
        protected virtual void deSerialize(AutoCSer.Json.Parser parser) { }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        [AutoCSer.Json.ParseCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe static void deSerialize(AutoCSer.Json.Parser parser, ref ReturnValue value)
        {
            if (*parser.Current++ == '[')
            {
                if (*parser.Current == ']')
                {
                    ++parser.Current;
                    return;
                }
                int clientNodeId = 0;
                parser.Parse(ref clientNodeId);
                if (parser.State == Json.ParseState.Success)
                {
                    value = createReturnValues.Array[clientNodeId]();
                    value.deSerialize(parser);
                }
                return;
            }
            parser.ParseState = Json.ParseState.Custom;
        }

        /// <summary>
        /// 新建返回值的委托集合
        /// </summary>
        private static LeftArray<Func<ReturnValue>> createReturnValues;
        /// <summary>
        /// 新建返回值的委托集合访问锁
        /// </summary>
        private static readonly object createReturnValueLock = new object();
        /// <summary>
        /// 客户端表达式节点注册
        /// </summary>
        /// <param name="createReturnValue">新建一个返回值的委托</param>
        /// <returns>表达式节点映射编号</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int RegisterClient(Func<ReturnValue> createReturnValue)
        {
            Monitor.Enter(createReturnValueLock);
            int id = createReturnValues.Length;
            try
            {
                createReturnValues.Add(createReturnValue);
            }
            finally { Monitor.Exit(createReturnValueLock); }
            return id;
        }
        static ReturnValue()
        {
            createReturnValues.Add((Func<ReturnValue>)null);
        }
    }
    /// <summary>
    /// 返回值
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    public sealed class ReturnValue<returnType> : ReturnValue
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public returnType Value;
        /// <summary>
        /// 返回值隐式转换
        /// </summary>
        /// <param name="value">返回值</param>
        public static implicit operator ReturnValue<returnType>(returnType value)
        {
            return value != null ? new ReturnValue<returnType> { Value = value } : null;
        }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        protected override void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            serializer.Stream.Write(ClientNodeId);
            AutoCSer.BinarySerialize.TypeSerializer<returnType>.Serialize(serializer, Value);
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="deSerializer"></param>
        protected override void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            AutoCSer.BinarySerialize.TypeDeSerializer<returnType>.DeSerialize(deSerializer, ref Value);
        }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        protected override void serialize(AutoCSer.Json.Serializer serializer)
        {
            AutoCSer.Json.TypeSerializer<returnType>.Serialize(serializer, ref Value);
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="parser"></param>
        protected override void deSerialize(AutoCSer.Json.Parser parser)
        {
            AutoCSer.Json.TypeParser<returnType>.Parse(parser, ref Value);
        }
    }
}

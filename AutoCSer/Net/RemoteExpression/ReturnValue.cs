using System;
using System.Threading;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;

namespace AutoCSer.Net.RemoteExpression
{
    /// <summary>
    /// 返回值
    /// </summary>
    [AutoCSer.JsonSerialize]
    [AutoCSer.JsonDeSerialize]
    [AutoCSer.BinarySerialize(IsReferenceMember = false, IsMemberMap = false)]
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
        protected virtual void serialize(AutoCSer.BinarySerializer serializer) { }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(AutoCSer.BinarySerializer serializer, ReturnValue value)
        {
            if(value == null) serializer.Stream.Write(AutoCSer.BinarySerializer.NullValue);
            else value.serialize(serializer);
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="deSerializer"></param>
        protected virtual void deSerialize(AutoCSer.BinaryDeSerializer deSerializer) { }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <param name="value"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void deSerialize(AutoCSer.BinaryDeSerializer deSerializer, ref ReturnValue value)
        {
            value = createReturnValues.Array[deSerializer.ReadInt()]();
            value.deSerialize(deSerializer);
        }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        protected virtual void serialize(AutoCSer.JsonSerializer serializer) { }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [AutoCSer.JsonSerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void serialize(AutoCSer.JsonSerializer serializer, ReturnValue value)
        {
            if (value == null) serializer.CharStream.WriteJsonArray();
            else
            {
                CharStream jsonStream = serializer.CharStream;
                jsonStream.Write('[');
                serializer.CallSerialize(value.ClientNodeId);
                jsonStream.Write(',');
                value.serialize(serializer);
                jsonStream.Write(']');
            }
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        protected virtual void deSerialize(AutoCSer.JsonDeSerializer jsonDeSerializer) { }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        [AutoCSer.JsonDeSerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe static void deSerialize(AutoCSer.JsonDeSerializer jsonDeSerializer, ref ReturnValue value)
        {
            if (*jsonDeSerializer.Current++ == '[')
            {
                if (*jsonDeSerializer.Current == ']')
                {
                    ++jsonDeSerializer.Current;
                    return;
                }
                int clientNodeId = 0;
                jsonDeSerializer.CallSerialize(ref clientNodeId);
                if (jsonDeSerializer.State == Json.DeSerializeState.Success)
                {
                    value = createReturnValues.Array[clientNodeId]();
                    value.deSerialize(jsonDeSerializer);
                }
                return;
            }
            jsonDeSerializer.DeSerializeState = Json.DeSerializeState.Custom;
        }

        /// <summary>
        /// 新建返回值的委托集合
        /// </summary>
        private static LeftArray<Func<ReturnValue>> createReturnValues = new LeftArray<Func<ReturnValue>>(0);
        /// <summary>
        /// 新建返回值的委托集合访问锁
        /// </summary>
        private static AutoCSer.Threading.SleepFlagSpinLock createReturnValueLock;
        /// <summary>
        /// 客户端表达式节点注册
        /// </summary>
        /// <param name="createReturnValue">新建一个返回值的委托</param>
        /// <returns>表达式节点映射编号</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static int RegisterClient(Func<ReturnValue> createReturnValue)
        {
            createReturnValueLock.Enter();
            int id = createReturnValues.Length;
            try
            {
                if (createReturnValues.FreeCount == 0) createReturnValueLock.SleepFlag = 1;
                createReturnValues.Add(createReturnValue);
            }
            finally { createReturnValueLock.ExitSleepFlag(); }
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
        protected override void serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.Stream.Write(ClientNodeId);
            AutoCSer.BinarySerialize.TypeSerializer<returnType>.Serialize(serializer, ref Value);
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="deSerializer"></param>
        protected override void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
        {
            AutoCSer.BinarySerialize.TypeDeSerializer<returnType>.DeSerialize(deSerializer, ref Value);
        }
        /// <summary>
        /// 服务端序列化返回值
        /// </summary>
        /// <param name="serializer"></param>
        protected override void serialize(AutoCSer.JsonSerializer serializer)
        {
            AutoCSer.Json.TypeSerializer<returnType>.Serialize(serializer, ref Value);
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        protected override void deSerialize(AutoCSer.JsonDeSerializer jsonDeSerializer)
        {
            AutoCSer.Json.TypeDeSerializer<returnType>.DeSerialize(jsonDeSerializer, ref Value);
        }
    }
}

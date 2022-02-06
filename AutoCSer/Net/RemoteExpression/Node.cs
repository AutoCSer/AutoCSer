using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.RemoteExpression
{
    /// <summary>
    /// 远程表达式节点
    /// </summary>
    public unsafe class Node
    {
        /// <summary>
        /// 远程表达式容器类型名称
        /// </summary>
        internal const string RemoteExpressionTypeName = "RemoteExpression";
        /// <summary>
        /// 父节点
        /// </summary>
        protected internal Node Parent;
        /// <summary>
        /// 客户端映射标识
        /// </summary>
        internal int ClientNodeId;
        /// <summary>
        /// 远程表达式节点
        /// </summary>
        public Node() { }
        /// <summary>
        /// 远程表达式节点
        /// </summary>
        /// <param name="clientNodeId">客户端映射标识</param>
        protected Node(int clientNodeId)
        {
            ClientNodeId = clientNodeId;
        }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        protected virtual void serializeParameter(AutoCSer.BinarySerializer serializer) { }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="checker"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Serialize(AutoCSer.BinarySerializer serializer, ServerNodeIdChecker checker)
        {
            serializer.Stream.Write(checker.ServerNodeIds[GetType()]);
            serializeParameter(serializer);
            serializeParent(serializer, checker);
        }
        /// <summary>
        /// 序列化父节点
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="checker"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void serializeParent(AutoCSer.BinarySerializer serializer, ServerNodeIdChecker checker)
        {
            if (Parent == null) serializer.Stream.Write(AutoCSer.BinarySerializer.NullValue);
            else Parent.Serialize(serializer, checker);
        }
        /// <summary>
        /// 参数序列化
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="parameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void serializeParameter<parameterType>(AutoCSer.BinarySerializer serializer, parameterType parameter)
        {
            if (parameter == null) serializer.Stream.Write(AutoCSer.BinarySerializer.NullValue);
            else AutoCSer.BinarySerialize.TypeSerializer<parameterType>.ClassSerialize(serializer, parameter);
        }
        /// <summary>
        /// 参数序列化
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="parameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void serializeParameterStruct<parameterType>(AutoCSer.BinarySerializer serializer, ref parameterType parameter)
        {
            AutoCSer.BinarySerialize.TypeSerializer<parameterType>.StructSerialize(serializer, ref parameter);
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        protected virtual void deSerializeParameter(AutoCSer.BinaryDeSerializer deSerializer) { }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
        {
            deSerializeParameter(deSerializer);
            deSerializeParent(deSerializer);
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        /// <param name="value"></param>
        internal static void DeSerialize(AutoCSer.BinaryDeSerializer deSerializer, out Node value)
        {
            (value = createNodes.Array[deSerializer.ReadInt()]()).deSerialize(deSerializer);
        }
        /// <summary>
        /// 反序列化父节点
        /// </summary>
        /// <param name="deSerializer"></param>
        protected void deSerializeParent(AutoCSer.BinaryDeSerializer deSerializer)
        {
            if (deSerializer.CheckNullValue() != 0) DeSerialize(deSerializer, out Parent);
        }
        /// <summary>
        /// 参数反序列化
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="parameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void deSerializeParameter<parameterType>(AutoCSer.BinaryDeSerializer deSerializer, ref parameterType parameter)
        {
            if (deSerializer.CheckNullValue() != 0) AutoCSer.BinarySerialize.TypeDeSerializer<parameterType>.DeSerialize(deSerializer, ref parameter);
        }
        /// <summary>
        /// 参数反序列化
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <param name="deSerializer"></param>
        /// <param name="parameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void deSerializeParameterStruct<parameterType>(AutoCSer.BinaryDeSerializer deSerializer, ref parameterType parameter)
        {
            AutoCSer.BinarySerialize.TypeDeSerializer<parameterType>.StructDeSerialize(deSerializer, ref parameter);
        }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        protected virtual void serializeParameter(AutoCSer.JsonSerializer serializer) { }
        /// <summary>
        /// 客户端序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="checker"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Serialize(AutoCSer.JsonSerializer serializer, ServerNodeIdChecker checker)
        {
            serializeStart(serializer, checker);
            serializeParameter(serializer);
            serializeParent(serializer, checker);
        }
        /// <summary>
        /// JSON 序列化开始
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="checker"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void serializeStart(AutoCSer.JsonSerializer serializer, ServerNodeIdChecker checker)
        {
            serializer.CharStream.Write('[');
            serializer.CallSerialize(checker.ServerNodeIds[GetType()]);
        }
        /// <summary>
        /// JSON 序列化父节点并结束
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="checker"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void serializeParent(AutoCSer.JsonSerializer serializer, ServerNodeIdChecker checker)
        {
            if (Parent != null)
            {
                serializer.CharStream.Write(',');
                Parent.Serialize(serializer, checker);
            }
            serializer.CharStream.Write(']');
        }
        /// <summary>
        /// 参数序列化
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="parameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void serializeParameter<parameterType>(AutoCSer.JsonSerializer serializer, parameterType parameter)
        {
            if (parameter == null) serializer.CharStream.WriteJsonNull();
            else AutoCSer.Json.TypeSerializer<parameterType>.ClassSerialize(serializer, parameter);
        }
        /// <summary>
        /// 参数序列化
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="parameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void serializeParameterStruct<parameterType>(AutoCSer.JsonSerializer serializer, ref parameterType parameter)
        {
            AutoCSer.Json.TypeSerializer<parameterType>.StructSerialize(serializer, ref parameter);
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        protected virtual void deSerializeParameter(AutoCSer.JsonDeSerializer jsonDeSerializer) { }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void deSerialize(AutoCSer.JsonDeSerializer jsonDeSerializer)
        {
            deSerializeParameter(jsonDeSerializer);
            deSerializeParent(jsonDeSerializer);
        }
        /// <summary>
        /// 服务端反序列化
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        internal static void DeSerialize(AutoCSer.JsonDeSerializer jsonDeSerializer, ref Node value)
        {
            if (*jsonDeSerializer.Current++ == '[')
            {
                int serverNodeId = 0;
                jsonDeSerializer.CallSerialize(ref serverNodeId);
                if (jsonDeSerializer.State == Json.DeSerializeState.Success) (value = createNodes.Array[serverNodeId]()).deSerialize(jsonDeSerializer);
                return;
            }
            jsonDeSerializer.DeSerializeState = Json.DeSerializeState.Custom;
        }
        /// <summary>
        /// 客户端反序列化返回值
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        protected void deSerializeParent(AutoCSer.JsonDeSerializer jsonDeSerializer)
        {
            if (jsonDeSerializer.DeSerializeState == Json.DeSerializeState.Success)
            {
                char split = *jsonDeSerializer.Current++;
                if ((split & 1) == 0)
                {
                    if (split == ',')
                    {
                        DeSerialize(jsonDeSerializer, ref Parent);
                        if (jsonDeSerializer.State != Json.DeSerializeState.Success || *jsonDeSerializer.Current++ == ']') return;
                    }
                }
                else if (split == ']') return;
                jsonDeSerializer.DeSerializeState = Json.DeSerializeState.Custom;
            }
        }
        /// <summary>
        /// 参数反序列化
        /// </summary>
        /// <typeparam name="parameterType"></typeparam>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="parameter"></param>
        protected void deSerializeParameter<parameterType>(AutoCSer.JsonDeSerializer jsonDeSerializer, ref parameterType parameter)
        {
            if (jsonDeSerializer.DeSerializeState == Json.DeSerializeState.Success)
            {
                if (*jsonDeSerializer.Current++ == ',')
                {
                    AutoCSer.Json.TypeDeSerializer<parameterType>.DeSerialize(jsonDeSerializer, ref parameter);
                    return;
                }
                jsonDeSerializer.DeSerializeState = Json.DeSerializeState.Custom;
            }
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        protected virtual ReturnValue getReturn() { return null; }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="clientNodeId">客户端映射标识</param>
        /// <returns>返回值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue Get(int clientNodeId)
        {
            ReturnValue value = getReturn();
            if (value != null)
            {
                value.ClientNodeId = clientNodeId;
                return value;
            }
            return null;
        }
        /// <summary>
        /// 服务端映射标识检测
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="checkTypes"></param>
        internal void CheckServerNodeId(ServerNodeIdChecker checker, ref LeftArray<Type> checkTypes)
        {
            Type nodeType = GetType();
            if (!checker.ServerNodeIds.ContainsKey(nodeType)) checkTypes.Add(nodeType);
            if (Parent != null) Parent.CheckServerNodeId(checker, ref checkTypes);
            checkParameterServerNodeId(checker, ref checkTypes);
        }
        /// <summary>
        /// 参数服务端映射标识检测
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="checkTypes"></param>
        protected virtual void checkParameterServerNodeId(ServerNodeIdChecker checker, ref LeftArray<Type> checkTypes) { }
        /// <summary>
        /// 设置远程表达式节点参数
        /// </summary>
        /// <typeparam name="returnType"></typeparam>
        /// <param name="clientNode"></param>
        /// <param name="node"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setParameter<returnType>(ref ClientNode<returnType> clientNode, Node<returnType> node)
        {
            clientNode.Node = node;
        }
        /// <summary>
        /// 用于代码生成
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="clientNode"></param>
        /// <param name="node"></param>
        internal void setParameter<valueType>(ref valueType clientNode, object node) { }
        /// <summary>
        /// 服务端映射标识检测
        /// </summary>
        /// <typeparam name="returnType"></typeparam>
        /// <param name="checker"></param>
        /// <param name="checkTypes"></param>
        /// <param name="clientNode"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void checkServerNodeId<returnType>(ServerNodeIdChecker checker, ref LeftArray<Type> checkTypes, ref ClientNode<returnType> clientNode)
        {
            clientNode.Node.CheckServerNodeId(clientNode.Checker = checker, ref checkTypes);
        }
        /// <summary>
        /// 用于代码生成
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="checker"></param>
        /// <param name="checkTypes"></param>
        /// <param name="clientNode"></param>
        internal void checkServerNodeId<valueType>(ServerNodeIdChecker checker, ref LeftArray<Type> checkTypes, ref valueType clientNode) { }
        /// <summary>
        /// 远程表达式泛型节点类型转换
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Cast(Node value)
        {
            Parent = value;
        }

        /// <summary>
        /// 客户端表达式节点注册
        /// </summary>
        /// <param name="createReturnValue">新建一个返回值的委托</param>
        /// <returns>表达式节点映射编号</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected static int registerClient(Func<ReturnValue> createReturnValue)
        {
            return ReturnValue.RegisterClient(createReturnValue);
        }
        /// <summary>
        /// 服务端节点类型集合
        /// </summary>
        private static readonly Dictionary<Type, int> types = DictionaryCreator.CreateOnly<Type, int>();
        /// <summary>
        /// 服务端节点类型集合访问锁
        /// </summary>
        private static AutoCSer.Threading.SleepFlagSpinLock typeLock;
        /// <summary>
        /// 创建服务端表达式委托集合
        /// </summary>
        protected static LeftArray<Func<Node>> createNodes = new LeftArray<Func<Node>>(0);
        /// <summary>
        /// 获取服务端节点标识
        /// </summary>
        /// <param name="remoteTypes"></param>
        /// <returns></returns>
        internal unsafe static int[] Get(AutoCSer.Reflection.RemoteType[] remoteTypes)
        {
            int[] ids = new int[remoteTypes.Length];
            fixed (int* idFixed = ids)
            {
                int* idStart = idFixed;
                foreach (AutoCSer.Reflection.RemoteType remoteType in remoteTypes)
                {
                    Type type;
                    if (remoteType.TryGet(out type))
                    {
                        if (types.TryGetValue(type, out *idStart)) ++idStart;
                        else
                        {
                            typeLock.Enter();
                            if (types.TryGetValue(type, out *idStart))
                            {
                                typeLock.Exit();
                                ++idStart;
                            }
                            else
                            {
                                typeLock.SleepFlag = 1;
                                try
                                {
                                    Func<Node> createNode = (Func<Node>)Delegate.CreateDelegate(typeof(Func<Node>), createNodeMethod.MakeGenericMethod(type));
                                    int id = createNodes.Length;
                                    createNodes.Add(createNode);
                                    types.Add(type, id);
                                    *idStart++ = id;
                                }
                                finally { typeLock.ExitSleepFlag(); }
                            }
                        }
                    }
                    else *idStart++ = 0;
                }
            }
            return ids;
        }
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static Node createNew<valueType>()
            where valueType : Node
        {
            return AutoCSer.Metadata.DefaultConstructor<valueType>.Constructor();
        }
        /// <summary>
        /// 创建节点函数信息
        /// </summary>
        private static readonly MethodInfo createNodeMethod = typeof(Node).GetMethod("createNew", BindingFlags.Static | BindingFlags.NonPublic);
        static Node()
        {
            createNodes.Add((Func<Node>)null);
        }
    }
    /// <summary>
    /// 远程表达式节点
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    public abstract class Node<returnType> : Node
    {
        /// <summary>
        /// 客户端返回值映射标识
        /// </summary>
        public static class ReturnClientNodeId
        {
            /// <summary>
            /// 客户端返回值映射标识
            /// </summary>
            public static readonly int Id = registerClient(createReturnValue);
            /// <summary>
            /// 创建客户端返回值类型
            /// </summary>
            /// <returns>创建客户端返回值类型</returns>
            private static AutoCSer.Net.RemoteExpression.ReturnValue createReturnValue() { return new AutoCSer.Net.RemoteExpression.ReturnValue<returnType>(); }
        }
        /// <summary>
        /// 远程表达式节点
        /// </summary>
        protected Node() : base() { }
        /// <summary>
        /// 远程表达式节点
        /// </summary>
        /// <param name="clientNodeId">客户端映射标识</param>
        protected Node(int clientNodeId) : base(clientNodeId) { }
        /// <summary>
        /// 服务端获取数据
        /// </summary>
        /// <returns>返回值</returns>
        protected abstract returnType getValue();
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        protected override ReturnValue getReturn() { return (ReturnValue<returnType>)getValue(); }
        /// <summary>
        /// 服务端获取数据
        /// </summary>
        /// <returns>返回值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public returnType GetValue()
        {
            return getValue();
        }
    }
}

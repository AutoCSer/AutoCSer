using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.RemoteExpression
{
    /// <summary>
    /// 远程表达式泛型节点
    /// </summary>
    public abstract class GenericNode : Node
    {
        /// <summary>
        /// 远程表达式泛型节点
        /// </summary>
        protected GenericNode() : base() { }
        /// <summary>
        /// 远程表达式泛型节点
        /// </summary>
        /// <param name="clientNodeId">客户端映射标识</param>
        protected GenericNode(int clientNodeId) : base(clientNodeId) { }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="nodeType"></typeparam>
        /// <returns>远程表达式节点</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public nodeType Cast<nodeType>()
            where nodeType : Node
        {
            if (typeof(nodeType).Name != RemoteExpressionTypeName) throw new InvalidCastException();
            nodeType value = AutoCSer.Emit.Constructor<nodeType>.New();
            value.Parent = this;
            value.ClientNodeId = 0;
            return value;
        }
    }
    /// <summary>
    /// 远程表达式泛型节点
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    public abstract class GenericNode<returnType> : Node<returnType>
    {
        /// <summary>
        /// 远程表达式泛型节点
        /// </summary>
        protected GenericNode() : base() { }
        /// <summary>
        /// 远程表达式泛型节点
        /// </summary>
        /// <param name="clientNodeId">客户端映射标识</param>
        protected GenericNode(int clientNodeId) : base(clientNodeId) { }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="nodeType"></typeparam>
        /// <returns>远程表达式节点</returns>
        public nodeType Cast<nodeType>()
            where nodeType : Node
        {
            if (typeof(nodeType).DeclaringType != typeof(returnType) || typeof(nodeType).Name != RemoteExpressionTypeName) throw new InvalidCastException();
            nodeType value = AutoCSer.Emit.Constructor<nodeType>.New();
            value.Parent = this;
            return value;
        }
    }
}

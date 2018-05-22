using System;

namespace AutoCSer.Example.TcpStaticServer
{
    /// <summary>
    /// 远程表达式测试
    /// </summary>
    [AutoCSer.Net.RemoteExpression.Type]
    static partial class Expression
    {
        /// <summary>
        /// 远程表达式测试
        /// </summary>
        [AutoCSer.Net.RemoteExpression.Type]
        internal partial class Node1
        {
            /// <summary>
            /// 远程表达式实例字段测试
            /// </summary>
            [AutoCSer.Net.RemoteExpression.Member]
            public int Value;
            /// <summary>
            /// 远程表达式实例属性测试
            /// </summary>
            [AutoCSer.Net.RemoteExpression.Member]
            public Node2 NextNode
            {
                get { return new Node2 { Value = Value + 1 }; }
            }
            /// <summary>
            /// 远程表达式实例参数属性测试
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            [AutoCSer.Net.RemoteExpression.Member]
            public int this[int value]
            {
                get { return Value + value; }
            }
            /// <summary>
            /// 远程表达式实例方法测试
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            [AutoCSer.Net.RemoteExpression.Member]
            public Node2 GetNextNode(int value)
            {
                return new Node2 { Value = Value + value };
            }
            /// <summary>
            /// 远程表达式实例泛型方法测试
            /// </summary>
            /// <typeparam name="valueType"></typeparam>
            /// <param name="value"></param>
            /// <returns></returns>

            [AutoCSer.Net.RemoteExpression.Member]
            public int GenericMethod<valueType>(valueType value)
            {
                Node1 node = value as Node1;
                if (node == null) return Value - (value as Node2).Value;
                return Value + node.Value;
            }
            /// <summary>
            /// 远程表达式泛型实例成员测试
            /// </summary>
            [AutoCSer.Net.RemoteExpression.Member(IsGenericTypeInstantiation = true)]
            public GenericNode<Node1> Generic1
            {
                get
                {
                    return new GenericNode<Node1> { Value = new Node1 { Value = 1 } };
                }
            }
            /// <summary>
            /// 远程表达式泛型实例成员测试
            /// </summary>
            [AutoCSer.Net.RemoteExpression.Member]
            public GenericNode<Node2> Generic2
            {
                get
                {
                    return new GenericNode<Node2> { Value = new Node2 { Value = 2 } };
                }
            }
        }
        /// <summary>
        /// 远程表达式测试
        /// </summary>
        [AutoCSer.Net.RemoteExpression.Type]
        internal partial class Node2
        {
            /// <summary>
            /// 远程表达式实例字段测试
            /// </summary>
            [AutoCSer.Net.RemoteExpression.Member]
            public int Value;
            /// <summary>
            /// 远程表达式实例属性测试
            /// </summary>
            [AutoCSer.Net.RemoteExpression.Member]
            public Node1 LastNode
            {
                get { return new Node1 { Value = Value - 1 }; }
            }
            /// <summary>
            /// 远程表达式实例参数属性测试
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            [AutoCSer.Net.RemoteExpression.Member]
            public int this[int value]
            {
                get { return Value - value; }
            }
            /// <summary>
            /// 远程表达式实例方法测试
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            [AutoCSer.Net.RemoteExpression.Member]
            public Node1 GetLastNode(int value)
            {
                return new Node1 { Value = Value - value };
            }
        }
        /// <summary>
        /// 远程表达式泛型节点测试
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        [AutoCSer.Net.RemoteExpression.Type]
        internal partial class GenericNode<valueType>
        {
            /// <summary>
            /// 泛型数据
            /// </summary>
            [AutoCSer.Net.RemoteExpression.Member]
            public valueType Value;
        }

        /// <summary>
        /// 远程表达式静态字段测试
        /// </summary>
        [AutoCSer.Net.RemoteExpression.Member]
        internal static readonly Node1 StaticField = new Node1 { Value = 1 };
        /// <summary>
        /// 远程表达式静态属性测试
        /// </summary>
        [AutoCSer.Net.RemoteExpression.Member]
        internal static Node1 StaticProperty
        {
            get { return new Node1 { Value = 2 }; }
        }
        /// <summary>
        /// 远程表达式静态方法测试
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AutoCSer.Net.RemoteExpression.Member]
        internal static Node1 StaticMethod(int value)
        {
            return new Node1 { Value = value };
        }
        /// <summary>
        /// 远程表达式泛型静态成员测试
        /// </summary>
        [AutoCSer.Net.RemoteExpression.Member(IsGenericTypeInstantiation = true)]
        internal static readonly GenericNode<Node1> Generic1 = new GenericNode<Node1> { Value = new Node1 { Value = 1 } };
        /// <summary>
        /// 远程表达式泛型静态成员测试
        /// </summary>
        [AutoCSer.Net.RemoteExpression.Member]
        internal static GenericNode<Node2> Generic2
        {
            get
            {
                return new GenericNode<Node2> { Value = new Node2 { Value = 2 } };
            }
        }
        /// <summary>
        /// 远程表达式静态泛型方法测试
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>

        [AutoCSer.Net.RemoteExpression.Member]
        internal static int GenericStaticMethod<valueType>(valueType value)
        {
            Node1 node = value as Node1;
            if (node == null) return (value as Node2).Value - 1;
            return node.Value + 1;
        }
        /// <summary>
        /// 远程表达式参数测试
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AutoCSer.Net.RemoteExpression.Member]
        internal static Node1 RemoteExpressionParameter(AutoCSer.Net.RemoteExpression.ClientNode<Node1> value)
        {
            return new Node1 {  Value = value.GetReturnValue().Value + 1 };
        }

        /// <summary>
        /// 远程表达式测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField).Value.Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticProperty).Value.Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(3)).Value.Value != 3)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.GenericStaticMethod(new Node1 { Value = 2 })).Value != 2 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.GenericStaticMethod(new Node2 { Value = 2 })).Value != 2 - 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.RemoteExpressionParameter(RemoteExpression.StaticField)).Value.Value != 1 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.Generic1).Value.Value.Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.Generic2).Value.Value.Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.Value).Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticProperty.Value).Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(3).Value).Value != 3)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(4)[3]).Value != 4 + 3)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(4).GetNextNode(2)).Value.Value != 4 + 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(4).GenericMethod(new Node1 { Value = 2 })).Value != 4 + 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(4).GenericMethod(new Node2 { Value = 2 })).Value != 4 - 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.Generic1.Value).Value.Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.Generic2.Value).Value.Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.Generic1.Value.Value).Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.Generic2.Value.Cast<Expression.Node2.RemoteExpression>()).Value.Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.Generic2.Value.Cast<Expression.Node2.RemoteExpression>().Value).Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.Generic1).Value.Value.Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.Generic1.Value).Value.Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.Generic1.Value.Value).Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.Generic2).Value.Value.Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.Generic2.Value).Value.Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.Generic2.Value.Cast<Expression.Node2.RemoteExpression>()).Value.Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.Generic2.Value.Cast<Expression.Node2.RemoteExpression>().Value).Value != 2)
            {
                return false;
            }

            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.NextNode).Value.Value != 1 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode).Value.Value != 2 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode).Value.Value != 3 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.NextNode.Value).Value != 1 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.Value).Value != 2 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.Value).Value != 3 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(4).NextNode[3]).Value != 4 + 1 - 3)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(4).NextNode.GetLastNode(2)).Value.Value != 4 + 1 - 2)
            {
                return false;
            }

            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.NextNode.LastNode).Value.Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.LastNode).Value.Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.LastNode).Value.Value != 3)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.NextNode.LastNode.Value).Value != 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.LastNode.Value).Value != 2)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.LastNode.Value).Value != 3)
            {
                return false;
            }

            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.NextNode.LastNode.NextNode).Value.Value != 1 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.LastNode.NextNode).Value.Value != 2 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.LastNode.NextNode).Value.Value != 3 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticField.NextNode.LastNode.NextNode.Value).Value != 1 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.LastNode.NextNode.Value).Value != 2 + 1)
            {
                return false;
            }
            if (TcpStaticClient.Example1.TcpClient.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.LastNode.NextNode.Value).Value != 3 + 1)
            {
                return false;
            }
            return true;
        }
    }
}

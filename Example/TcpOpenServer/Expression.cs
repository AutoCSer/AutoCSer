using System;

namespace AutoCSer.Example.TcpOpenServer
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
        /// 远程表达式测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            using (AutoCSer.Example.TcpOpenServer.RefOut.TcpOpenServer server = new AutoCSer.Example.TcpOpenServer.RefOut.TcpOpenServer())
            {
                if (server.IsListen)
                {
                    using (AutoCSer.Example.TcpOpenServer.TcpClient.RefOut.TcpOpenClient client = new AutoCSer.Example.TcpOpenServer.TcpClient.RefOut.TcpOpenClient())
                    {
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticField).Value.Value != 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticProperty).Value.Value != 2)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(3)).Value.Value != 3)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticField.Value).Value != 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticProperty.Value).Value != 2)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(3).Value).Value != 3)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(4)[3]).Value != 4 + 3)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(4).GetNextNode(2)).Value.Value != 4 + 2)
                        {
                            return false;
                        }

                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticField.NextNode).Value.Value != 1 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode).Value.Value != 2 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode).Value.Value != 3 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticField.NextNode.Value).Value != 1 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.Value).Value != 2 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.Value).Value != 3 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(4).NextNode[3]).Value != 4 + 1 - 3)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(4).NextNode.GetLastNode(2)).Value.Value != 4 + 1 - 2)
                        {
                            return false;
                        }

                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticField.NextNode.LastNode).Value.Value != 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.LastNode).Value.Value != 2)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.LastNode).Value.Value != 3)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticField.NextNode.LastNode.Value).Value != 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.LastNode.Value).Value != 2)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.LastNode.Value).Value != 3)
                        {
                            return false;
                        }

                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticField.NextNode.LastNode.NextNode).Value.Value != 1 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.LastNode.NextNode).Value.Value != 2 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.LastNode.NextNode).Value.Value != 3 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticField.NextNode.LastNode.NextNode.Value).Value != 1 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticProperty.NextNode.LastNode.NextNode.Value).Value != 2 + 1)
                        {
                            return false;
                        }
                        if (client._TcpClient_.GetRemoteExpression(RemoteExpression.StaticMethod(3).NextNode.LastNode.NextNode.Value).Value != 3 + 1)
                        {
                            return false;
                        }

                        return true;
                    }
                }
            }
            return false;
        }
    }
}

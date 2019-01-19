//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 实例对象调用链映射 示例
            /// </summary>
            public partial class RemoteKey
            {
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                public partial struct RemoteExtension
                {
                    /// <summary>
                    /// 实例对象定位关键字配置
                    /// </summary>
                    public int Id;
                    /// <summary>
                    /// 调用链目标成员测试
                    /// </summary>
                    public int NextId
                    {
                        get
                        {
                            return TcpCall.RemoteKey.getNextId(Id);
                        }
                    }
                    /// <summary>
                    /// 调用链目标成员测试
                    /// </summary>
                    public int RemoteLinkNextId
                    {
                        get
                        {
                            return TcpCall.RemoteKey.get_RemoteLink_NextId(Id);
                        }
                    }
                    /// <summary>
                    /// 调用链目标函数测试
                    /// </summary>
                    public int AddId(int value)
                    {
                        
                        return  TcpCall.RemoteKey.remote_AddId(Id, value);
                    }
                    /// <summary>
                    /// 调用链目标函数测试
                    /// </summary>
                    public int RemoteLink_AddId(int value)
                    {
                        
                        return  TcpCall.RemoteKey.remote_RemoteLink_AddId(Id, value);
                    }
                }
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                /// <param name="Id">实例对象定位关键字配置</param>
                /// <returns>远程对象扩展</returns>
                public static RemoteExtension Remote(int Id)
                {
                    return new RemoteExtension { Id = Id };
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 远程调用连类型映射测试
            /// </summary>
            public partial struct RemoteLinkType
            {
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                public partial struct RemoteExtension
                {
                    /// <summary>
                    /// 远程调用连类型映射测试
                    /// </summary>
                    public AutoCSer.Example.TcpStaticServer.RemoteLinkType Value;
                    /// <summary>
                    /// 调用链目标成员测试
                    /// </summary>
                    public int NextId
                    {
                        get
                        {
                            return TcpCall.RemoteLinkType.getNextId(Value);
                        }
                    }
                    /// <summary>
                    /// 调用链目标成员测试
                    /// </summary>
                    public int RemoteLinkNextId
                    {
                        get
                        {
                            return TcpCall.RemoteLinkType.get_RemoteLink_NextId(Value);
                        }
                    }
                    /// <summary>
                    /// 调用链目标函数测试
                    /// </summary>
                    /// <param name="value">远程调用连类型映射测试</param>
                    public int AddId(int value)
                    {
                        
                        return  TcpCall.RemoteLinkType.remote_AddId(Value, value);
                    }
                    /// <summary>
                    /// 调用链目标函数测试
                    /// </summary>
                    public int RemoteLink_AddId(int value)
                    {
                        
                        return  TcpCall.RemoteLinkType.remote_RemoteLink_AddId(Value, value);
                    }
                }
                /// <summary>
                /// 远程对象扩展
                /// </summary>
                /// <param name="value">远程调用连类型映射测试</param>
                /// <returns>远程对象扩展</returns>
                public static RemoteExtension RemoteType(AutoCSer.Example.TcpStaticServer.RemoteLinkType value)
                {
                    return new RemoteExtension { Value = value };
                }
            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// ref / out 参数测试 示例
            /// </summary>
            public partial class RefOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c1 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 0 + 128, InputParameterIndex = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// ref / out 参数测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                /// <param name="product">乘积</param>
                /// <returns>和</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> Add1(int left, ref int right, out int product)
                {
                    AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p2> _wait_ = AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p2>.Pop();
                    try
                    {
                        AutoCSer.Net.TcpInternalServer.ClientSocketSender _socket_ = AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender;
                        if (_socket_ != null)
                        {
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p1 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p1
                            {
                                
                                p0 = left,
                                
                                p1 = right,
                            };
                            
                            AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p2 _outputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p2
                            {
                                
                                p0 = right,
                            };
                            AutoCSer.Net.TcpServer.ReturnType _returnType_ = _socket_.WaitGet<AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p1, AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p2>(_c1, ref _wait_, ref _inputParameter_, ref _outputParameter_);
                            
                            right = _outputParameter_.p0;
                            
                            product = _outputParameter_.p1;
                            return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = _returnType_, Value = _outputParameter_.Return };
                        }
                    }
                    finally
                    {
                        if (_wait_ != null) AutoCSer.Net.TcpServer.AutoWaitReturnValue<AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p2>.PushNotNull(_wait_);
                    }
                    product = default(int);
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = AutoCSer.Net.TcpServer.ReturnType.ClientException };
                }

            }
        }
}namespace AutoCSer.Example.TcpStaticServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCall
        {
            /// <summary>
            /// 仅发送请求测试 示例
            /// </summary>
            public partial class SendOnly
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfo _c2 = new AutoCSer.Net.TcpServer.CommandInfo { Command = 1 + 128, InputParameterIndex = 3, IsSendOnly = 1, TaskType = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// 仅发送请求测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
                public static void SetSum1(int left, int right)
                {
                    
                    AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p3 _inputParameter_ = new AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p3
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    
                    AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/.TcpClient.Sender.CallOnly(_c2, ref _inputParameter_);
                }

            }
        }
}
namespace AutoCSer.Example.TcpStaticServer.TcpStaticClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class Example1
        {
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p1
            {
                public int p0;
                public int p1;
                public int p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p2
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<AutoCSer.Net.TcpServer.ReturnValue<int>>
#endif
            {
                public int p0;
                public int p1;
                [AutoCSer.Json.IgnoreMember]
                public AutoCSer.Net.TcpServer.ReturnValue<int> Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public AutoCSer.Net.TcpServer.ReturnValue<int> Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (AutoCSer.Net.TcpServer.ReturnValue<int>)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p3
            {
                public int p0;
                public int p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p4
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public int Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public int Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (int)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<bool>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public bool Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public bool Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (bool)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p6
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p7
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
#if NOJIT
                     : AutoCSer.Net.IReturnParameter
#else
                     : AutoCSer.Net.IReturnParameter<int>
#endif
            {
                [AutoCSer.Json.IgnoreMember]
                public int Ret;
                [AutoCSer.IOS.Preserve(Conditional = true)]
                public int Return
                {
                    get { return Ret; }
                    set { Ret = value; }
                }
#if NOJIT
                [AutoCSer.Metadata.Ignore]
                public object ReturnObject
                {
                    get { return Ret; }
                    set { Ret = (int)value; }
                }
#endif
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p9
            {
                public int p0;
                public int p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p10
            {
                public AutoCSer.Example.TcpStaticServer.RemoteLinkType p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p11
            {
                public AutoCSer.Example.TcpStaticServer.RemoteLinkType p0;
                public int p1;
            }
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 自定义数据包处理
                /// </summary>
                public Action<AutoCSer.SubArray<byte>> OnCustomData;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// TCP 客户端路由
                /// </summary>
                public AutoCSer.Net.TcpServer.ClientLoadRoute<AutoCSer.Net.TcpInternalServer.ClientSocketSender> ClientRoute;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<AutoCSer.Net.TcpInternalServer.ClientSocketSender, bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticServer.Client TcpClient;
            static Example1()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticServer.ServerAttribute.GetConfig("Example1", typeof(AutoCSer.Example.TcpStaticServer.RefOut), false);
                }
                TcpClient = new AutoCSer.Net.TcpStaticServer.Client(config.ServerAttribute, config.OnCustomData, config.Log, config.ClientRoute, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p1), typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p3), typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p6), typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p7), typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p9), null }
                    , new System.Type[] { typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p4), typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p5), typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p8), null }
                    , new System.Type[] { typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p10), typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p11), null }
                    , new System.Type[] { typeof(AutoCSer.Example.TcpStaticServer.TcpStaticClient/**/.Example1/**/._p2), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
#endif
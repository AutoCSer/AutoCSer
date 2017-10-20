//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Example.TcpStaticSimpleServer
{
        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public static partial class TcpCallSimple
        {
            /// <summary>
            /// ref / out 参数测试 示例
            /// </summary>
            public partial class RefOut
            {
                private static readonly AutoCSer.Net.TcpServer.CommandInfoBase _c10 = new AutoCSer.Net.TcpServer.CommandInfoBase { Command = 7 + 128, InputParameterIndex = 8, IsSimpleSerializeInputParamter = true };

                /// <summary>
                /// ref / out 参数测试
                /// </summary>
                /// <param name="left">加法左值</param>
                /// <param name="right">加法右值</param>
                /// <param name="product">乘积</param>
                /// <returns>和</returns>
                public static AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> Add1(int left, ref int right, out int product)
                {
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p8 _inputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p8
                    {
                        
                        p0 = left,
                        
                        p1 = right,
                    };
                    
                    AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p9 _outputParameter_ = new AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p9
                    {
                        
                        p0 = right,
                    };
                    AutoCSer.Net.TcpServer.ReturnType _returnType_ = AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/.TcpClient.Get<AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p8, AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p9>(_c10, ref _inputParameter_, ref _outputParameter_);
                    
                    right = _outputParameter_.p0;
                    
                    product = _outputParameter_.p1;
                    return new AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.TcpServer.ReturnValue<int>> { Type = _returnType_, Value = _outputParameter_.Return };
                }

            }
        }
}
namespace AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient
{

        /// <summary>
        /// TCP调用客户端
        /// </summary>
        public class Example1
        {
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p5
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
            internal struct _p6
            {
                public int p0;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p7
            {
                public int p0;
                public int p1;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p8
            {
                public int p0;
                public int p1;
                public int p2;
            }
            [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false)]
            [AutoCSer.Metadata.BoxSerialize]
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct _p9
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
            internal struct _p10
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
            /// <summary>
            /// TCP 静态调用客户端参数
            /// </summary>
            public sealed class ClientConfig
            {
                /// <summary>
                /// TCP 内部服务配置
                /// </summary>
                public AutoCSer.Net.TcpInternalSimpleServer.ServerAttribute ServerAttribute;
                /// <summary>
                /// 日志接口
                /// </summary>
                public AutoCSer.Log.ILog Log;
                /// <summary>
                /// 验证委托
                /// </summary>
                public Func<bool> VerifyMethod;
            }
            /// <summary>
            /// 默认客户端TCP调用
            /// </summary>
            public static readonly AutoCSer.Net.TcpStaticSimpleServer.Client TcpClient;
            static Example1()
            {
                ClientConfig config = (ClientConfig)AutoCSer.Config.Loader.GetObject(typeof(ClientConfig)) ?? new ClientConfig();
                if (config.ServerAttribute == null)
                {
                    config.ServerAttribute = AutoCSer.Net.TcpStaticSimpleServer.ServerAttribute.GetConfig("Example1", typeof(AutoCSer.Example.TcpStaticSimpleServer.RefOut), false);
                }
                TcpClient = new AutoCSer.Net.TcpStaticSimpleServer.Client(config.ServerAttribute, config.Log, config.VerifyMethod);
                TcpClient.ClientCompileSerialize(new System.Type[] { typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p6), typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p7), typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p8), null }
                    , new System.Type[] { typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p5), typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p10), null }
                    , new System.Type[] { null }
                    , new System.Type[] { typeof(AutoCSer.Example.TcpStaticSimpleServer.TcpStaticSimpleClient/**/.Example1/**/._p9), null }
                    , new System.Type[] { null }
                    , new System.Type[] { null });
            }
        }
}
#endif
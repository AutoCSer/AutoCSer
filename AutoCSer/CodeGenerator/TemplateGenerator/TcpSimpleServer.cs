using System;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// TCP 服务代码生成基类
    /// </summary>
    internal abstract partial class TcpSimpleServer
    {
        /// <summary>
        /// TCP 服务代码生成
        /// </summary>
        /// <typeparam name="attributeType">TCP 服务配置</typeparam>
        /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
        /// <typeparam name="serverSocketType"></typeparam>
        internal abstract class Generator<attributeType, methodAttributeType, serverSocketType> : TcpServer.GeneratorBase<attributeType, methodAttributeType, serverSocketType>
            where attributeType : AutoCSer.Net.TcpServer.ServerBaseAttribute
            where methodAttributeType : AutoCSer.Net.TcpServer.MethodBaseAttribute
            where serverSocketType : AutoCSer.Net.TcpSimpleServer.ServerSocket
        {
            /// <summary>
            /// 方法索引信息
            /// </summary>
            public new sealed class TcpMethod : TcpMethod<TcpMethod>
            {
                /// <summary>
                /// 验证方法是否支持异步
                /// </summary>
                public override bool IsVerifyMethodAsynchronous { get { return true; } }
                /// <summary>
                /// 空方法索引信息
                /// </summary>
                private static readonly TcpMethod nullMethod = new TcpMethod { IsNullMethod = true };
                /// <summary>
                /// 检测方法序号
                /// </summary>
                /// <param name="methodIndexs">方法集合</param>
                /// <param name="rememberIdentityCommand">命令序号记忆数据</param>
                /// <param name="getMethodKeyName">获取命令名称的委托</param>
                /// <returns>方法集合,失败返回null</returns>
                public static TcpMethod[] CheckIdentity(TcpMethod[] methodIndexs, Dictionary<HashString, int> rememberIdentityCommand, Func<TcpMethod, string> getMethodKeyName)
                {
                    return CheckIdentity(methodIndexs, rememberIdentityCommand, getMethodKeyName, nullMethod);
                }
            }
            /// <summary>
            /// 方法索引集合
            /// </summary>
            public TcpMethod[] MethodIndexs;
            /// <summary>
            /// 简单序列化方法集合
            /// </summary>
            public TcpMethod[] SimpleSerializeMethods
            {
                get
                {
                    return getSimpleSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// 简单反序列化方法集合
            /// </summary>
            public TcpMethod[] SimpleDeSerializeMethods
            {
                get
                {
                    return getSimpleDeSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// 二进制序列化方法集合
            /// </summary>
            public TcpMethod[] SerializeMethods
            {
                get
                {
                    return getSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// 二进制反序列化方法集合
            /// </summary>
            public TcpMethod[] DeSerializeMethods
            {
                get
                {
                    return getDeSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// JSON 序列化方法集合
            /// </summary>
            public TcpMethod[] JsonSerializeMethods
            {
                get
                {
                    return getJsonSerializeMethods(MethodIndexs);
                }
            }
            /// <summary>
            /// JSON 序列化方法集合
            /// </summary>
            public TcpMethod[] JsonDeSerializeMethods
            {
                get
                {
                    return getJsonDeSerializeMethods(MethodIndexs);
                }
            }
        }
    }
}

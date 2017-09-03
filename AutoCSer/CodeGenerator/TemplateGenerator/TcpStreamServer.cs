using System;
using System.Collections.Generic;
using AutoCSer.CodeGenerator.Metadata;

namespace AutoCSer.CodeGenerator.TemplateGenerator
{
    /// <summary>
    /// TCP 服务代码生成基类
    /// </summary>
    internal abstract partial class TcpStreamServer
    {
        /// <summary>
        /// TCP 服务代码生成
        /// </summary>
        /// <typeparam name="attributeType">TCP 服务配置</typeparam>
        /// <typeparam name="methodAttributeType">TCP 调用函数配置</typeparam>
        /// <typeparam name="serverSocketSenderType">TCP 服务套接字数据发送</typeparam>
        internal abstract class Generator<attributeType, methodAttributeType, serverSocketSenderType> : TcpServer.GeneratorBase<attributeType, methodAttributeType, serverSocketSenderType>
            where attributeType : AutoCSer.Net.TcpStreamServer.ServerAttribute
            where methodAttributeType : AutoCSer.Net.TcpStreamServer.MethodAttribute
            where serverSocketSenderType : AutoCSer.Net.TcpServer.ServerSocketSenderBase
        {
            /// <summary>
            /// 方法索引信息
            /// </summary>
            public new sealed class TcpMethod : TcpMethod<TcpMethod>
            {
                /// <summary>
                /// 是否定义服务器端调用
                /// </summary>
                public override bool IsMethodServerCall
                {
                    get
                    {
                        return ServiceAttribute.ServerTaskType != Net.TcpStreamServer.ServerTaskType.Synchronous && !Attribute.IsVerifyMethod;
                    }
                }

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
        }
    }
}

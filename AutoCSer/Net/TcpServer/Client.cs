using System;
using AutoCSer.Log;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Net;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端
    /// </summary>
    /// <typeparam name="attributeType">TCP 服务配置类型</typeparam>
    public abstract class Client<attributeType> : ClientBase<attributeType>
        where attributeType : ServerAttribute
    {
        /// <summary>
        /// 客户端最大自定义数据包字节大小
        /// </summary>
        protected readonly int maxCustomDataSize;
        /// <summary>
        /// 自定义数据命令信息
        /// </summary>
        private CommandInfo customDataCommandInfo;
        /// <summary>
        /// 自定义数据命令信息
        /// </summary>
        internal CommandInfo CustomDataCommandInfo
        {
            get
            {
                if (customDataCommandInfo == null) customDataCommandInfo = new CommandInfo { IsSendOnly = 1, MaxDataSize = sizeof(uint) + sizeof(int) * 2 };
                return customDataCommandInfo;
            }
        }
#if !NOJIT
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        internal Client() : base() { }
#endif
        /// <summary>
        /// TCP 服务客户端
        /// </summary>
        /// <param name="attribute">TCP服务调用配置</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        public Client(attributeType attribute, Action<SubArray<byte>> onCustomData, ILog log)
            : base(attribute, log, onCustomData)
        {
            maxCustomDataSize = attribute.GetMaxCustomDataSize <= 0 ? int.MaxValue : attribute.GetMaxCustomDataSize;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (IsDisposed == 0)
            {
                base.Dispose();
                DisposeSocket();
                SocketWait.Set();
            }
        }
        /// <summary>
        /// 释放套接字
        /// </summary>
        internal abstract void DisposeSocket();

        ///// <summary>
        ///// 获取异步回调
        ///// </summary>
        ///// <param name="callback">回调委托</param>
        ///// <returns>异步回调</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public Callback<ReturnValue<outputParameterType>> GetCallback<outputParameterType>(Action<ReturnValue> callback)
        //{
        //    return callback != null ? new CallbackReturnValue<outputParameterType>(callback) : (Callback<ReturnValue<outputParameterType>>)NullCallbackReturnValue<outputParameterType>.Default;
        //}
    }
}

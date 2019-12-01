using System;
using AutoCSer.Log;
using System.Threading;
using AutoCSer.Extension;
using System.Net;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// TCP 服务客户端
    /// </summary>
    public abstract class Client : ClientBase
    {
        /// <summary>
        /// 客户端最大自定义数据包字节大小
        /// </summary>
        protected readonly int maxCustomDataSize;
        /// <summary>
        /// 最大超时秒数
        /// </summary>
        private readonly ushort maxTimeoutSeconds;
        /// <summary>
        /// 最大超时秒数
        /// </summary>
        internal override ushort MaxTimeoutSeconds { get { return maxTimeoutSeconds; } }
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
        /// <summary>
        /// 命令超时触发事件
        /// </summary>
        public event Action OnTimeout;
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
        /// <param name="maxTimeoutSeconds">最大超时秒数</param>
        /// <param name="onCustomData">自定义数据包处理</param>
        /// <param name="log">日志接口</param>
        internal Client(ServerBaseAttribute attribute, ushort maxTimeoutSeconds, Action<SubArray<byte>> onCustomData, ILog log)
            : base(attribute, log, onCustomData)
        {
            maxCustomDataSize = attribute.GetMaxCustomDataSize <= 0 ? int.MaxValue : attribute.GetMaxCustomDataSize;
            this.maxTimeoutSeconds = maxTimeoutSeconds;
        }
        /// <summary>
        /// 命令超时触发事件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnTimeout()
        {
            if (OnTimeout != null) OnTimeout();
        }

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

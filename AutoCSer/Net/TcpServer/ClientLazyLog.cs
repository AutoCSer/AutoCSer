using AutoCSer.Threading;
using System;
using System.Runtime.InteropServices;
using AutoCSer.Extensions;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 客户端延迟日志，用于跟踪异常客户端最后的操作行为
    /// </summary>
    internal sealed class ClientLazyLog : LazyEvent
    {
        private readonly ClientSocketBase clientSocket;
        /// <summary>
        /// 日志数据
        /// </summary>
        private LogData log;
        /// <summary>
        /// 当前日志位置
        /// </summary>
        private int logByte
        {
            get { return reserve; }
            set { reserve = value; }
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        internal enum LogType : byte
        {
            /// <summary>
            /// 
            /// </summary>
            None = 0,
            /// <summary>
            /// 套接字初始化连接
            /// </summary>
            SocketConnect = 1,
            /// <summary>
            /// 创建套接字以后发起异步接收数据请求
            /// </summary>
            CreateSocketReceiveAsync = 2,
            /// <summary>
            /// 创建套接字以后发起异步接收数据请求同步完成错误状态
            /// </summary>
            CreateSocketReceiveSynchronous = 3,
            /// <summary>
            /// 创建套接字初始化失败并停止，等待接收数据回调重建连接
            /// </summary>
            CreateSocketError = 4,
            /// <summary>
            /// 创建套接字初始化失败并等待休眠循环重建连接
            /// </summary>
            CreateSocketErrorSleep = 5,
            /// <summary>
            /// 获取到命令回调序号
            /// </summary>
            OnReceiveTypeCommandIdentity = 6,
            /// <summary>
            /// 获取到命令回调序号
            /// </summary>
            OnReceiveTypeCommandIdentityAgain = 7,
            /// <summary>
            /// 获取到数据
            /// </summary>
            OnReceiveTypeData = 8,
            /// <summary>
            /// 获取到临时数据
            /// </summary>
            OnReceiveTypeBigData = 9,
            /// <summary>
            /// 获取数据错误或者处理异常
            /// </summary>
            OnReceiveError = 0xA,
            /// <summary>
            /// 关闭套接字
            /// </summary>
            OnClose = 0xB,
            /// <summary>
            /// 套接字关闭事件
            /// </summary>
            OnSocketLock = 0xC,
            /// <summary>
            /// 关闭套接字以后创建新的套接字
            /// </summary>
            OnCreateNew = 0xD,
        }
        /// <summary>
        /// 日志数据
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        internal partial struct LogData
        {
            /// <summary>
            /// 日志数据
            /// </summary>
            [FieldOffset(0)]
            public ulong Data;

            [FieldOffset(0)]
            internal byte Byte0;
            [FieldOffset(1)]
            internal byte Byte1;
            [FieldOffset(2)]
            internal byte Byte2;
            [FieldOffset(3)]
            internal byte Byte3;
            [FieldOffset(4)]
            internal byte Byte4;
            [FieldOffset(5)]
            internal byte Byte5;
            [FieldOffset(6)]
            internal byte Byte6;
            [FieldOffset(7)]
            internal byte Byte7;
        }
        /// <summary>
        /// 客户端延迟日志
        /// </summary>
        /// <param name="seconds">延迟秒数</param>
        /// <param name="clientSocket">TCP 服务客户端套接字</param>
        internal ClientLazyLog(int seconds, ClientSocketBase clientSocket) : base(seconds)
        {
            if (clientSocket != null)
            {
                this.clientSocket = clientSocket;
                AppendTaskArray();
            }
        }
        /// <summary>
        /// 添加日志类型
        /// </summary>
        /// <param name="type"></param>
        internal void Append(LogType type)
        {
            setTimeout();
            switch (logByte)
            {
                case 0: log.Byte0 = (byte)type; logByte = 1; return;
                case 1: log.Byte1 = (byte)type; logByte = 2; return;
                case 2: log.Byte2 = (byte)type; logByte = 3; return;
                case 3: log.Byte3 = (byte)type; logByte = 4; return;
                case 4: log.Byte4 = (byte)type; logByte = 5; return;
                case 5: log.Byte5 = (byte)type; logByte = 6; return;
                case 6: log.Byte6 = (byte)type; logByte = 7; return;
                case 7: log.Byte7 = (byte)type; logByte = 0; return;
            }
        }
        /// <summary>
        /// 超时检测
        /// </summary>
        protected internal override void OnTimer()
        {
            if (AutoCSer.Threading.SecondTimer.CurrentSeconds >= timeoutSeconds)
            {
                KeepMode = SecondTimerKeepMode.Canceled;
                //clientSocket.Log.Wait(LogLevel.Error, clientSocket.ClientCreator.Attribute.ServerName + "[" + clientSocket.ClientCreator.CommandClient.IsDisposed.toString() + "] 客户端延时调试日志 " + clientSocket.IpAddress.ToString() + ":" + clientSocket.Port.toString() + "." + clientSocket.CreateVersion.toString() + "[" + clientSocket.ClientCreator.CreateVersion.toString() + "]" + " " + logByte.toString() + "." + log.Data.toHex(), Log.CacheType.None);
                if (clientSocket.ClientCreator.CommandClient.IsDisposed == 0 && clientSocket.CreateVersion == clientSocket.ClientCreator.CreateVersion)
                {
                    clientSocket.Log.Debug(clientSocket.ClientCreator.Attribute.ServerName + " 客户端延时日志 " + clientSocket.IpAddress.ToString() + ":" + clientSocket.Port.toString() + "." + clientSocket.CreateVersion.toString() + " " + logByte.toString() + "." + log.Data.toHex(), LogLevel.Debug  | LogLevel.AutoCSer);
                }
            }
        }
        //private void output()
        //{
        //    clientSocket.Log.Wait(LogLevel.Error, clientSocket.ClientCreator.Attribute.ServerName + "[" + clientSocket.ClientCreator.CommandClient.IsDisposed.toString() + "] 客户端延时调试日志 " + clientSocket.IpAddress.ToString() + ":" + clientSocket.Port.toString() + "." + clientSocket.CreateVersion.toString() + "[" + clientSocket.ClientCreator.CreateVersion.toString() + "]" + " " + logByte.toString() + "." + log.Data.toHex(), Log.CacheType.None);
        //    if (clientSocket.ClientCreator.CommandClient.IsDisposed == 0 && clientSocket.CreateVersion == clientSocket.ClientCreator.CreateVersion)
        //    {
        //        AutoCSer.Threading.TimerTask.Default.Add(output, DateTime.Now.AddSeconds(seconds), TimerTaskThreadType.ThreadPool);
        //    }
        //}

        /// <summary>
        /// 默认空客户端延迟日志
        /// </summary>
        internal new static readonly ClientLazyLog Null = new ClientLazyLog(int.MaxValue, null);
    }
}

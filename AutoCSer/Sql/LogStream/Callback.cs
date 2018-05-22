using System;
using System.Threading;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 日志流回调
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public sealed class Callback<valueType>
    {
        /// <summary>
        /// 回调委托集合
        /// </summary>
        private LeftArray<Func<AutoCSer.Net.TcpServer.ReturnValue<valueType>, bool>> callbacks;
        /// <summary>
        /// 回调委托集合访问锁
        /// </summary>
        private readonly object callbackLock = new object();
        /// <summary>
        /// 添加日志流
        /// </summary>
        /// <param name="log">日志数据</param>
        public void Call(valueType log)
        {
            if (callbacks.Length != 0)
            {
                Func<AutoCSer.Net.TcpServer.ReturnValue<valueType>, bool>[] callbackArray = callbacks.Array;
                Func<AutoCSer.Net.TcpServer.ReturnValue<valueType>, bool> removeCallback = null;
                int removeIndex = -1, index = 0;
                AutoCSer.Net.TcpServer.ReturnValue<valueType> value = log;
                foreach (Func<AutoCSer.Net.TcpServer.ReturnValue<valueType>, bool> callback in callbackArray)
                {
                    if (callback != null)
                    {
                        if (!callback(value))
                        {
                            removeCallback = callback;
                            removeIndex = index;
                        }
                        ++index;
                    }
                    else break;
                }
                if (removeCallback != null)
                {
                    Monitor.Enter(callbackLock);
                    if (callbackArray[removeIndex] == removeCallback)
                    {
                        if (--callbacks.Length != removeIndex) callbackArray[removeIndex] = callbackArray[callbacks.Length];
                        callbackArray[callbacks.Length] = null;
                    }
                    Monitor.Exit(callbackLock);
                }
            }
        }
        /// <summary>
        /// 添加日志回调委托
        /// </summary>
        /// <param name="callback">日志回调委托</param>
        public void Append(Func<AutoCSer.Net.TcpServer.ReturnValue<valueType>, bool> callback)
        {
            Monitor.Enter(callbackLock);
            try
            {
                callbacks.Add(callback);
            }
            finally { Monitor.Exit(callbackLock); }
        }
    }
}

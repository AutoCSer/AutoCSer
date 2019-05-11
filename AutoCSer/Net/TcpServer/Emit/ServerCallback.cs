using System;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// 服务端回调转换
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    public sealed class ServerCallback<returnType>
    {
        /// <summary>
        /// 服务端回调委托
        /// </summary>
        private Func<ReturnValue<returnType>, bool> callback;
        /// <summary>
        /// 服务端回调转换
        /// </summary>
        /// <param name="callback">服务端回调委托</param>
        private ServerCallback(Func<ReturnValue<returnType>, bool> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 服务端回调
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool call(returnType value)
        {
            return callback(value);
        }
        /// <summary>
        /// 服务端回调转换
        /// </summary>
        /// <param name="callback">服务端回调委托</param>
        /// <returns>服务端回调转换</returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public static Func<returnType, bool> Get(Func<ReturnValue<returnType>, bool> callback)
        {
            return new ServerCallback<returnType>(callback).call;
        }
    }
}

using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// 客户端回调转换
    /// </summary>
    public sealed class ClientCallback
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        private Func<ReturnValue, bool> callback;
        /// <summary>
        /// 客户端回调转换
        /// </summary>
        /// <param name="callback">客户端回调委托</param>
        private ClientCallback(Func<ReturnValue, bool> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 客户端回调
        /// </summary>
        /// <param name="value"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void call(ReturnValue value)
        {
            callback(value);
        }
        /// <summary>
        /// 客户端回调转换
        /// </summary>
        /// <param name="callback">客户端回调委托</param>
        /// <returns>客户端回调转换</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public static Action<ReturnValue> Get(Func<ReturnValue, bool> callback)
        {
            return callback == null ? (Action<ReturnValue>)null : new ClientCallback(callback).call;
        }
    }
    /// <summary>
    /// 客户端回调转换
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    public sealed class ClientCallback<returnType>
    {
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        private Func<ReturnValue<returnType>, bool> callback;
        /// <summary>
        /// 客户端回调转换
        /// </summary>
        /// <param name="callback">客户端回调委托</param>
        private ClientCallback(Func<ReturnValue<returnType>, bool> callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 客户端回调
        /// </summary>
        /// <param name="value"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void call(ReturnValue<returnType> value)
        {
            callback(value);
        }
        /// <summary>
        /// 客户端回调转换
        /// </summary>
        /// <param name="callback">客户端回调委托</param>
        /// <returns>客户端回调转换</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public static Action<ReturnValue<returnType>> Get(Func<ReturnValue<returnType>, bool> callback)
        {
            return callback == null ? (Action<ReturnValue<returnType>>)null : new ClientCallback<returnType>(callback).call;
        }
    }
}

using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 异步等待
    /// </summary>
    public sealed class TaskAsyncReturnValue : Callback<ReturnValue>, INotifyCompletion
    {
        /// <summary>
        /// 输出参数
        /// </summary>
        private ReturnType outputParameter;
        /// <summary>
        /// 异步回调
        /// </summary>
        private Action continuation;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return false; } }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        public override void Call(ref ReturnValue outputParameter)
        {
            this.outputParameter = outputParameter.Type;
            if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Call(ReturnValue outputParameter)
        {
            this.outputParameter = outputParameter.Type;
            if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnType GetResult()
        {
            return outputParameter;
        }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TaskAsyncReturnValue GetAwaiter()
        {
            return this;
        }
    }
    /// <summary>
    /// 异步等待
    /// </summary>
    /// <typeparam name="outputParameterType"></typeparam>
    public sealed class TaskAsyncReturnValue<outputParameterType> : Callback<ReturnValue<outputParameterType>>, INotifyCompletion
    {
        /// <summary>
        /// 输出参数
        /// </summary>
        private ReturnValue<outputParameterType> outputParameter;
        /// <summary>
        /// 异步回调
        /// </summary>
        private Action continuation;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return false; } }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        public override void Call(ref ReturnValue<outputParameterType> outputParameter)
        {
            this.outputParameter = outputParameter;
            if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<outputParameterType> GetResult()
        {
            return outputParameter;
        }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public TaskAsyncReturnValue<outputParameterType> GetAwaiter()
        {
            return this;
        }
    }
}

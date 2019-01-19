using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 异步等待
    /// </summary>
    public sealed partial class Awaiter : INotifyCompletion
    {
        /// <summary>
        /// 等待返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<ReturnValue> Wait()
        {
            return await this;
        }
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Call(ReturnValue outputParameter)
        {
            returnValue = outputParameter;
            if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        /// <param name="type">返回值类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Call(ReturnType type)
        {
            returnValue.Type = type;
            continuation = Pub.EmptyAction;
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue GetResult()
        {
            return returnValue;
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
        public Awaiter GetAwaiter()
        {
            return this;
        }
    }
    /// <summary>
    /// 异步等待
    /// </summary>
    public abstract partial class Awaiter<returnType, awaiterReturnValueType> : INotifyCompletion
	{
        /// <summary>
        /// 等待返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public async Task<ReturnValue<returnType>> Wait()
        {
            return await this;
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        private Action continuation;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return false; } }
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        /// <param name="type">返回值类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Call(ReturnType type)
        {
            returnValue.Type = type;
            continuation = Pub.EmptyAction;
        }
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        /// <param name="returnValue">输出参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void call(ReturnType returnType, ref returnType returnValue)
        {
            this.returnValue.Type = returnType;
            this.returnValue.Value = returnValue;
            if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<returnType> GetResult()
        {
            return returnValue;
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
        public Awaiter<returnType, awaiterReturnValueType> GetAwaiter()
        {
            return this;
        }
    }
}

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
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 异步回调
        /// </summary>
        private Action continuation;
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Call(ReturnValue outputParameter)
        {
            returnType = outputParameter.Type;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        /// <param name="type">返回值类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Call(ReturnType type)
        {
            returnType = type;
            continuation = Common.EmptyAction;
            IsCompleted = true;
        }
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        /// <param name="awaiter"></param>
        /// <param name="type">返回值类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static void Call(Awaiter awaiter, ReturnType type)
        {
            awaiter.Call(type);
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue GetResult()
        {
            return new ReturnValue { Type = returnType };
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
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 异步回调
        /// </summary>
        private Action continuation;
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        /// <param name="type">返回值类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Call(ReturnType type)
        {
            returnValueType = type;
            continuation = Common.EmptyAction;
            IsCompleted = true;
        }
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        /// <param name="returnValue">输出参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void call(ReturnType returnType, ref returnType returnValue)
        {
            returnValueType = returnType;
            this.returnValue = returnValue;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<returnType> GetResult()
        {
            return new ReturnValue<returnType> { Type = returnValueType, Value = returnValue };
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

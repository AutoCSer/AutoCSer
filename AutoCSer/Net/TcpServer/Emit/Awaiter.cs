using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.TcpServer.Emit
{
    /// <summary>
    /// 异步等待
    /// </summary>
    /// <typeparam name="returnType">返回值类型</typeparam>
    public sealed class Awaiter<returnType> : Callback<ReturnValue<AwaiterReturnValue<returnType>>>, INotifyCompletion
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
        /// 返回值
        /// </summary>
        private returnType returnValue;
        /// <summary>
        /// 返回值类型
        /// </summary>
        private ReturnType returnValueType;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; set; }
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        /// <param name="type">返回值类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Call(ReturnType type)
        {
            returnValueType = type;
            continuation = Pub.EmptyAction;
            IsCompleted = true;
        }
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="returnValue">输出参数</param>
        public override void Call(ref ReturnValue<AwaiterReturnValue<returnType>> returnValue)
        {
            returnValueType = returnValue.Type;
            this.returnValue = returnValue.Value.Return;
            if (System.Threading.Interlocked.CompareExchange(ref continuation, Pub.EmptyAction, null) != null) continuation();
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
        public Awaiter<returnType> GetAwaiter()
        {
            return this;
        }
    }
}

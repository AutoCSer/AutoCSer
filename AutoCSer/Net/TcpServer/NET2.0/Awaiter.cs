using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 异步等待
    /// </summary>
    public sealed partial class Awaiter
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        private AutoCSer.Threading.WaitHandle waitHandle;
        /// <summary>
        /// 异步等待
        /// </summary>
        public Awaiter()
        {
            waitHandle.Set(0);
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Call(ReturnValue outputParameter)
        {
            returnValue = outputParameter;
            waitHandle.Set();
        }
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        /// <param name="type">返回值类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Call(ReturnType type)
        {
            returnValue.Type = type;
            waitHandle.Set();
        }
        /// <summary>
        /// 等待返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AwaiterResult<ReturnValue> Wait()
        {
            waitHandle.Wait();
            return new AwaiterResult<ReturnValue> { Result = returnValue };
        }
    }
    /// <summary>
    /// 异步等待
    /// </summary>
    public abstract partial class Awaiter<returnType, awaiterReturnValueType>
    {
        /// <summary>
        /// 等待事件
        /// </summary>
        private AutoCSer.Threading.WaitHandle waitHandle;
        /// <summary>
        /// 异步等待
        /// </summary>
        public Awaiter()
        {
            waitHandle.Set(0);
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
            waitHandle.Set();
        }
        /// <summary>
        /// 设置错误返回值类型
        /// </summary>
        /// <param name="type">返回值类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Call(ReturnType type)
        {
            returnValue.Type = type;
            waitHandle.Set();
        }
        /// <summary>
        /// 等待返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AwaiterResult<ReturnValue<returnType>> Wait()
        {
            waitHandle.Wait();
            return new AwaiterResult<ReturnValue<returnType>> { Result = returnValue };
        }
	}
}

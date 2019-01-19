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
        /// 返回值
        /// </summary>
        private ReturnValue returnValue;
    }
    /// <summary>
    /// 异步等待
    /// </summary>
    /// <typeparam name="returnType"></typeparam>
    /// <typeparam name="awaiterReturnValueType"></typeparam>
    public abstract partial class Awaiter<returnType, awaiterReturnValueType> : Callback<ReturnValue<awaiterReturnValueType>>
        where awaiterReturnValueType : struct
    {
        /// <summary>
        /// 返回值
        /// </summary>
        private ReturnValue<returnType> returnValue;
    }
    /// <summary>
    /// 异步等待
    /// </summary>
    /// <typeparam name="returnType"></typeparam>
    public sealed class Awaiter<returnType> : Awaiter<returnType, AwaiterReturnValue<returnType>>
    {
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="returnValue">输出参数</param>
        public override void Call(ref ReturnValue<AwaiterReturnValue<returnType>> returnValue)
        {
            call(returnValue.Type, ref returnValue.Value.Ret);
        }
    }
    /// <summary>
    /// 异步等待
    /// </summary>
    /// <typeparam name="returnType"></typeparam>
    public sealed class AwaiterBox<returnType> : Awaiter<returnType, AwaiterReturnValueBox<returnType>>
    {
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="returnValue">输出参数</param>
        public override void Call(ref ReturnValue<AwaiterReturnValueBox<returnType>> returnValue)
        {
            call(returnValue.Type, ref returnValue.Value.Ret);
        }
    }
    /// <summary>
    /// 异步等待
    /// </summary>
    /// <typeparam name="returnType"></typeparam>
    public sealed class AwaiterReference<returnType> : Awaiter<returnType, AwaiterReturnValueReference<returnType>>
    {
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="returnValue">输出参数</param>
        public override void Call(ref ReturnValue<AwaiterReturnValueReference<returnType>> returnValue)
        {
            call(returnValue.Type, ref returnValue.Value.Ret);
        }
    }
    /// <summary>
    /// 异步等待
    /// </summary>
    /// <typeparam name="returnType"></typeparam>
    public sealed class AwaiterBoxReference<returnType> : Awaiter<returnType, AwaiterReturnValueBoxReference<returnType>>
    {
        /// <summary>
        /// 异步回调返回值
        /// </summary>
        /// <param name="returnValue">输出参数</param>
        public override void Call(ref ReturnValue<AwaiterReturnValueBoxReference<returnType>> returnValue)
        {
            call(returnValue.Type, ref returnValue.Value.Ret);
        }
    }
}

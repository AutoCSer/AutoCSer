using System;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpServer
{
    /// <summary>
    /// 同步等待调用
    /// </summary>
    public sealed class AutoWaitReturnValue : Callback<ReturnValue>
    {
        /// <summary>
        /// 回调处理
        /// </summary>
        internal readonly Action<ReturnValue> CallbackHandle;
        /// <summary>
        /// 同步等待
        /// </summary>
        private AutoCSer.Threading.AutoWaitHandle waitHandle;

        /// <summary>
        /// 下一个节点
        /// </summary>
        private AutoWaitReturnValue next;
        /// <summary>
        /// 输出参数
        /// </summary>
        private ReturnType outputParameter;
        /// <summary>
        /// 调用返回值（警告：每次调用只能使用一次）
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Get(out ReturnValue value)
        {
            waitHandle.Wait();
            value.Type = outputParameter;
            outputParameter = ReturnType.Unknown;
            PushNotNull(this);
        }
        /// <summary>
        /// 等待返回
        /// </summary>
        /// <returns>是否存在返回值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnType Wait()
        {
            waitHandle.Wait();
            ReturnType outputParameter = this.outputParameter;
            this.outputParameter = ReturnType.Unknown;
            PushNotNull(this);
            return outputParameter;
        }
        /// <summary>
        /// 同步等待调用
        /// </summary>
        internal AutoWaitReturnValue()
        {
            waitHandle.Set(0);
            CallbackHandle = CallbackSet;
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="outputParameter">是否调用成功</param>
        public override void Call(ref ReturnValue outputParameter)
        {
            CallbackSet(outputParameter);
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="outputParameter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallbackSet(ReturnValue outputParameter)
        {
            this.outputParameter = outputParameter.Type;
            waitHandle.Set();
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Action<ReturnValue>(AutoWaitReturnValue value)
        {
            return value.CallbackHandle;
        }

        /// <summary>
        /// 缓存数量
        /// </summary>
        private readonly static int poolMaxCount = AutoCSer.Common.Config.GetYieldPoolCount(typeof(AutoWaitReturnValue));
        /// <summary>
        /// 链表头部
        /// </summary>
        private static AutoWaitReturnValue poolHead;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock popLock;
        /// <summary>
        /// 缓存数量
        /// </summary>
        private static int poolCount;
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public static void PushNotNull(AutoWaitReturnValue value)
        {
            if (poolCount >= poolMaxCount) return;
            System.Threading.Interlocked.Increment(ref poolCount);
            AutoWaitReturnValue headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    value.next = null;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, null) == null) return;
                }
                else
                {
                    value.next = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public static AutoWaitReturnValue Pop()
        {
            popLock.EnterYield();
            AutoWaitReturnValue headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    popLock.Exit();
                    return new AutoWaitReturnValue();
                }
                if (System.Threading.Interlocked.CompareExchange(ref poolHead, headValue.next, headValue) == headValue)
                {
                    popLock.Exit();
                    System.Threading.Interlocked.Decrement(ref poolCount);
                    headValue.next = null;
                    return headValue;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 添加链表
        /// </summary>
        /// <param name="value">链表头部</param>
        /// <param name="end">链表尾部</param>
        /// <param name="count">数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void pushLink(AutoWaitReturnValue value, AutoWaitReturnValue end, int count)
        {
            System.Threading.Interlocked.Add(ref poolCount, count);
            AutoWaitReturnValue headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    end.next = null;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, null) == null) return;
                }
                else
                {
                    end.next = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            AutoWaitReturnValue headValue = System.Threading.Interlocked.Exchange(ref poolHead, null);
            poolCount = 0;
            if (headValue != null && count != 0)
            {
                int pushCount = count;
                AutoWaitReturnValue end = headValue;
                while (--count != 0)
                {
                    if (end.next == null)
                    {
                        pushLink(headValue, end, pushCount - count);
                        return;
                    }
                    end = end.next;
                }
                end.next = null;
                pushLink(headValue, end, pushCount);
            }
        }
        static AutoWaitReturnValue()
        {
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(AutoWaitReturnValue));
        }
    }
    /// <summary>
    /// 同步等待调用
    /// </summary>
    /// <typeparam name="outputParameterType">输出参数类型</typeparam>
    public sealed class AutoWaitReturnValue<outputParameterType> : Callback<ReturnValue<outputParameterType>>
    {
        /// <summary>
        /// 同步等待
        /// </summary>
        private AutoCSer.Threading.AutoWaitHandle waitHandle;
        /// <summary>
        /// 下一个节点
        /// </summary>
        private AutoWaitReturnValue<outputParameterType> next;
        /// <summary>
        /// 输出参数
        /// </summary>
        private ReturnValue<outputParameterType> outputParameter;
        /// <summary>
        /// 调用返回值（警告：每次调用只能使用一次）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<outputParameterType> Get()
        {
            waitHandle.Wait();
            ReturnValue<outputParameterType> value = outputParameter;
            outputParameter.Null();
            PushNotNull(this);
            return value;
        }
        /// <summary>
        /// 调用返回值（警告：每次调用只能使用一次）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnType Get(out outputParameterType value)
        {
            waitHandle.Wait();
            value = outputParameter.Value;
            ReturnType type = outputParameter.Type;
            outputParameter.Null();
            PushNotNull(this);
            return type;
        }
        /// <summary>
        /// 调用返回值（警告：每次调用只能使用一次）
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Get(out ReturnValue<outputParameterType> value)
        {
            waitHandle.Wait();
            value = outputParameter;
            outputParameter.Null();
            PushNotNull(this);
        }
        /// <summary>
        /// 等待返回(无意义)
        /// </summary>
        /// <returns>是否存在返回值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnType Wait()
        {
            return ReturnType.Unknown;
        }
        /// <summary>
        /// 同步等待调用
        /// </summary>
        internal AutoWaitReturnValue()
        {
            waitHandle.Set(0);
        }
        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="outputParameter">输出参数</param>
        public override void Call(ref ReturnValue<outputParameterType> outputParameter)
        {
            this.outputParameter = outputParameter;
            waitHandle.Set();
        }

        /// <summary>
        /// 缓存数量
        /// </summary>
        private readonly static int poolMaxCount = AutoCSer.Common.Config.GetYieldPoolCount(typeof(AutoWaitReturnValue<outputParameterType>));
        /// <summary>
        /// 链表头部
        /// </summary>
        private static AutoWaitReturnValue<outputParameterType> poolHead;
        /// <summary>
        /// 弹出节点访问锁
        /// </summary>
        private static AutoCSer.Threading.SpinLock popLock;
        /// <summary>
        /// 缓存数量
        /// </summary>
        private static int poolCount;
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public static void PushNotNull(AutoWaitReturnValue<outputParameterType> value)
        {
            if (poolCount >= poolMaxCount) return;
            System.Threading.Interlocked.Increment(ref poolCount);
            AutoWaitReturnValue<outputParameterType> headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    value.next = null;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, null) == null) return;
                }
                else
                {
                    value.next = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 弹出节点
        /// </summary>
        /// <returns></returns>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        public static AutoWaitReturnValue<outputParameterType> Pop()
        {
            popLock.EnterYield();
            AutoWaitReturnValue<outputParameterType> headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    popLock.Exit();
                    return new AutoWaitReturnValue<outputParameterType>();
                }
                if (System.Threading.Interlocked.CompareExchange(ref poolHead, headValue.next, headValue) == headValue)
                {
                    popLock.Exit();
                    System.Threading.Interlocked.Decrement(ref poolCount);
                    headValue.next = null;
                    return headValue;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 添加链表
        /// </summary>
        /// <param name="value">链表头部</param>
        /// <param name="end">链表尾部</param>
        /// <param name="count">数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void pushLink(AutoWaitReturnValue<outputParameterType> value, AutoWaitReturnValue<outputParameterType> end, int count)
        {
            System.Threading.Interlocked.Add(ref poolCount, count);
            AutoWaitReturnValue<outputParameterType> headValue;
            do
            {
                if ((headValue = poolHead) == null)
                {
                    end.next = null;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, null) == null) return;
                }
                else
                {
                    end.next = headValue;
                    if (System.Threading.Interlocked.CompareExchange(ref poolHead, value, headValue) == headValue) return;
                }
                AutoCSer.Threading.ThreadYield.Yield();
            }
            while (true);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            AutoWaitReturnValue<outputParameterType> headValue = System.Threading.Interlocked.Exchange(ref poolHead, null);
            poolCount = 0;
            if (headValue != null && count != 0)
            {
                int pushCount = count;
                AutoWaitReturnValue<outputParameterType> end = headValue;
                while (--count != 0)
                {
                    if (end.next == null)
                    {
                        pushLink(headValue, end, pushCount - count);
                        return;
                    }
                    end = end.next;
                }
                end.next = null;
                pushLink(headValue, end, pushCount);
            }
        }

        static AutoWaitReturnValue()
        {
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(AutoWaitReturnValue<outputParameterType>));
        }
    }
}

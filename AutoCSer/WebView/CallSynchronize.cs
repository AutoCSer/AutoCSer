using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTTP 同步调用
    /// </summary>
    public abstract unsafe class CallSynchronize : CallBase
    {
        /// <summary>
        /// 获取 WEB 调用输出
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public CallResponse GetResponse()
        {
            return CallResponse;
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(string value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(ref SubString value)
        {
            CallResponse.Write(ref value);
        }
        /// <summary>
        /// 输出字节数组
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(byte[] value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出字节数组
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(ref SubArray<byte> value)
        {
            CallResponse.Write(ref value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(byte value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(sbyte value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(ushort value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(short value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(uint value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(int value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(long value)
        {
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(ulong value)
        {
            CallResponse.Write(value);
        }
    }
}

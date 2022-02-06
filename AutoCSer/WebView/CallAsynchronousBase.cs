using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.WebView
{
    /// <summary>
    /// HTTP 异步调用
    /// </summary>
    public abstract unsafe class CallAsynchronousBase : CallBase
    {
        /// <summary>
        /// 是否创建了异步输出
        /// </summary>
        internal bool IsCreateResponse;
        /// <summary>
        /// 获取 WEB 调用输出
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public CallResponse GetResponse()
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            return CallResponse;
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(string value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(ref SubString value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(ref value);
        }
        /// <summary>
        /// 输出字节数组
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(byte[] value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出字节数组
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(ref SubArray<byte> value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(ref value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(byte value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(sbyte value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(ushort value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(short value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(uint value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(int value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(long value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 输出数字字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Response(ulong value)
        {
            if (!IsCreateResponse) throw new InvalidOperationException();
            CallResponse.Write(value);
        }
        /// <summary>
        /// 创建 WEB 异步调用输出
        /// </summary>
        /// <param name="size"></param>
        /// <returns>是否创建成功</returns>
        public bool CreateResponse(int size = UnmanagedPool.TinySize)
        {
            if (IsCreateResponse || SocketIdentity != Socket.Identity) return false;
            IsCreateResponse = true;
            UnmanagedStream responseStream = Interlocked.Exchange(ref ResponseStream, null);
            if (responseStream == null) responseStream = new UnmanagedStream(size);
            else
            {
                AutoCSer.Memory.Pointer buffer = Unmanaged.GetPointer(size, false);
                responseStream.Reset(ref buffer);
                responseStream.IsUnmanaged = true;
            }
            CallResponse.Set(responseStream, ref DomainServer.ResponseEncoding);
            return true;
        }
        /// <summary>
        /// 创建 WEB 异步调用输出
        /// </summary>
        /// <param name="response"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CreateResponse(ref CallResponse response, int size = UnmanagedPool.TinySize)
        {
            if (CreateResponse(size))
            {
                response = CallResponse;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 创建 WEB 异步调用输出（需要自己负责释放内存）
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>是否创建成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CreateResponse(ref AutoCSer.Memory.Pointer buffer)
        {
            if (IsCreateResponse || SocketIdentity != Socket.Identity) return false;
            IsCreateResponse = true;
            UnmanagedStream responseStream = Interlocked.Exchange(ref ResponseStream, null);
            if (responseStream == null) responseStream = new UnmanagedStream(ref buffer);
            else responseStream.Reset(ref buffer);
            CallResponse.Set(responseStream, ref DomainServer.ResponseEncoding);
            return true;
        }
        /// <summary>
        /// 创建 WEB 异步调用输出（需要自己负责释放内存）
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="response"></param>
        /// <returns>是否创建成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CreateResponse(ref AutoCSer.Memory.Pointer buffer, ref CallResponse response)
        {
            if (CreateResponse(ref buffer))
            {
                response = CallResponse;
                return true;
            }
            return false;
        }
    }
}

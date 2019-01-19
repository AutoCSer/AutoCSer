using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB 调用输出
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct CallResponse
    {
        /// <summary>
        /// 页面输出数据流
        /// </summary>
        internal UnmanagedStream ResponseStream;
        /// <summary>
        /// 字符编码
        /// </summary>
        private EncodingCache encoding;
        /// <summary>
        /// 设置同步输出文本
        /// </summary>
        /// <param name="responseStream"></param>
        /// <param name="encoding"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(UnmanagedStream responseStream, ref EncodingCache encoding)
        {
            this.ResponseStream = responseStream;
            this.encoding = encoding;
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(string value)
        {
            encoding.WriteBytes(value, ResponseStream);
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(SubString value)
        {
            encoding.WriteBytes(ref value, ResponseStream);
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ref SubString value)
        {
            encoding.WriteBytes(ref value, ResponseStream);
        }
        /// <summary>
        /// 输出字节数组
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(byte[] value)
        {
            ResponseStream.Write(value);
        }
        /// <summary>
        /// 输出字节数组
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(SubArray<byte> value)
        {
            ResponseStream.Write(ref value);
        }
        /// <summary>
        /// 输出字节数组
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ref SubArray<byte> value)
        {
            ResponseStream.Write(ref value);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(byte value)
        {
            encoding.Write(value, ResponseStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(sbyte value)
        {
            encoding.Write(value, ResponseStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ushort value)
        {
            encoding.Write(value, ResponseStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(short value)
        {
            encoding.Write(value, ResponseStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(uint value)
        {
            encoding.Write(value, ResponseStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(int value)
        {
            encoding.Write(value, ResponseStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(ulong value)
        {
            encoding.Write(value, ResponseStream);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Write(long value)
        {
            encoding.Write(value, ResponseStream);
        }
    }
}

using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// 创建输出信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ServerBuffer
    {
        /// <summary>
        /// 发送/接收数据
        /// </summary>
        internal SubArray<byte> Data;
        /// <summary>
        /// 复制数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull CopyBuffer;
        /// <summary>
        /// 压缩数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull CompressBuffer;
        /// <summary>
        /// 套接字错误
        /// </summary>
        internal SocketError SocketError;
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            CopyBuffer.TryFree();
            CompressBuffer.TryFree();
        }
        /// <summary>
        /// 设置发送数据
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetSendDataCopyBuffer(int size)
        {
            Data.Set(CopyBuffer.Buffer, CopyBuffer.StartIndex, size);
        }
        /// <summary>
        /// 压缩发送数据
        /// </summary>
        /// <param name="dataLength"></param>
        /// <param name="sendMarkData"></param>
        internal unsafe bool CompressSendData(int dataLength, ulong sendMarkData)
        {
            if (AutoCSer.IO.Compression.DeflateCompressor.Get(Data.Array, Data.Start + sizeof(int), dataLength, ref CompressBuffer, ref Data, sizeof(int) * 2, sizeof(int) * 2))
            {
                int compressionDataSize = Data.Length;
                Data.MoveStart(-(sizeof(int) * 2));
                fixed (byte* sendDataFixed = Data.Array)
                {
                    byte* dataStart = sendDataFixed + Data.Start;
                    *(int*)dataStart = -compressionDataSize;
                    *(int*)(dataStart + sizeof(int)) = dataLength;
                    if (sendMarkData != 0) TcpServer.CommandBase.Mark64(dataStart + sizeof(int) * 2, sendMarkData, (compressionDataSize + 3) & (int.MaxValue - 3));
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 压缩发送数据
        /// </summary>
        /// <param name="dataLength"></param>
        /// <param name="sendMarkData"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void MarkSendData(int dataLength, ulong sendMarkData)
        {
            fixed (byte* sendDataFixed = Data.Array)
            {
                TcpServer.CommandBase.Mark32(sendDataFixed + (Data.Start + sizeof(int)), sendMarkData, (dataLength + 3) & (int.MaxValue - 3));
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        internal bool Send(Socket socket)
        {
            int count, sendSizeLessCount = 0;
            do
            {
                Data.MoveStart(count = socket.Send(Data.Array, Data.Start, Data.Length, SocketFlags.None, out SocketError));
                if (Data.Length == 0) return true;
            }
            while (SocketError == SocketError.Success && (count >= TcpServer.Server.MinSocketSize || (count > 0 && sendSizeLessCount++ == 0)));
            return false;
        }
#if !DOTNET2
        /// <summary>
        /// 设置发送数据缓冲区
        /// </summary>
        /// <param name="asyncEventArgs"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetBuffer(SocketAsyncEventArgs asyncEventArgs)
        {
            asyncEventArgs.SetBuffer(Data.Array, Data.Start, Data.Length);
        }
        /// <summary>
        /// 设置发送数据缓冲区
        /// </summary>
        /// <param name="asyncEventArgs"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetBufferNext(SocketAsyncEventArgs asyncEventArgs)
        {
            asyncEventArgs.SetBuffer(Data.Start, Data.Length);
        }
#endif
        /// <summary>
        /// 发送数据后设置数据位置
        /// </summary>
        /// <param name="count"></param>
        /// <returns>未完成数据长度</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int OnSend(int count)
        {
            Data.MoveStart(count);
            return Data.Length;
        }
    }
}

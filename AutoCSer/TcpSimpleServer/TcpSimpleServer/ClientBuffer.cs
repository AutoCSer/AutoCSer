using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpSimpleServer
{
    /// <summary>
    /// 创建命令信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ClientBuffer
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
        /// 返回值类型
        /// </summary>
        internal TcpServer.ReturnType ReturnType;
        /// <summary>
        /// 套接字操作是否错误
        /// </summary>
        internal bool IsError;
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
            SubArray<byte> oldSendData = Data;
            if (AutoCSer.IO.Compression.DeflateCompressor.Get(Data.Array, Data.Start + (sizeof(uint) + sizeof(int)), dataLength, ref CompressBuffer, ref Data, sizeof(uint) + sizeof(int) * 2, sizeof(uint) + sizeof(int) * 2))
            {
                int compressionDataSize = Data.Length;
                Data.MoveStart(-(sizeof(uint) + sizeof(int) * 2));
                fixed (byte* sendDataFixed = Data.Array, oldSendDataFixed = oldSendData.Array)
                {
                    byte* dataStart = sendDataFixed + Data.Start;
                    *(int*)dataStart = *(int*)(oldSendDataFixed + oldSendData.Start);
                    *(int*)(dataStart + sizeof(uint)) = -compressionDataSize;
                    *(int*)(dataStart + (sizeof(uint) + sizeof(int))) = dataLength;
                    if (sendMarkData != 0) TcpServer.CommandBase.Mark32(dataStart + (sizeof(uint) + sizeof(int) * 2), sendMarkData, (compressionDataSize + 3) & (int.MaxValue - 3));
                }
                return IsError = true;
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
                TcpServer.CommandBase.Mark64(sendDataFixed + (Data.Start + (sizeof(uint) + sizeof(int))), sendMarkData, (dataLength + 3) & (int.MaxValue - 3));
            }
            IsError = true;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal bool Send(Client client)
        {
            if (IsError)
            {
                Socket socket = client.Socket;
                int sendCount = 0, count;
                do
                {
                    Data.MoveStart(count = socket.Send(Data.Array, Data.Start, Data.Length, SocketFlags.None, out SocketError));
                    ++sendCount;
                    if (Data.Length == 0)
                    {
                        //CompressBuffer.TryFree();
                        //CopyBuffer.TryFree();
                        client.SendCount += sendCount;
                        return true;
                    }
                }
                while (count > 0 && SocketError == SocketError.Success);
                client.SendCount += sendCount;
                ReturnType = TcpServer.ReturnType.ClientSendError;
            }
            else ReturnType = TcpServer.ReturnType.ClientBuildError;
            return false;
        }
        /// <summary>
        /// 设置返回值类型
        /// </summary>
        /// <param name="returnType"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetReturnType(TcpServer.ReturnType returnType)
        {
            ReturnType = returnType;
            IsError = false;
        }
        /// <summary>
        /// 设置接收数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetReceiveData(ref SubBuffer.PoolBufferFull buffer, int size)
        {
            Data.Set(buffer.Buffer, buffer.StartIndex + sizeof(int), size);
        }
        /// <summary>
        /// 复制缓冲区数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="copySize"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CopyBufferData(ref SubBuffer.PoolBufferFull buffer, int size, int copySize)
        {
            SubBuffer.Pool.GetBuffer(ref CopyBuffer, size);
            System.Buffer.BlockCopy(buffer.Buffer, buffer.StartIndex, CopyBuffer.Buffer, CopyBuffer.StartIndex, copySize);
        }
        /// <summary>
        /// 接收数据解压缩
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool DeCompressReceiveData(ref SubBuffer.PoolBufferFull buffer, int size, int dataSize)
        {
            CompressBuffer.StartIndex = dataSize;
            AutoCSer.IO.Compression.DeflateDeCompressor.Get(buffer.Buffer, buffer.StartIndex + sizeof(int) * 2, size, ref CompressBuffer);
            if (CompressBuffer.Buffer != null)
            {
                Data.Set(CompressBuffer.Buffer, CompressBuffer.StartIndex, dataSize);
                return true;
            }
            ReturnType = TcpServer.ReturnType.ClientDeSerializeError;
            return false;
        }
        /// <summary>
        /// 设置接收数据
        /// </summary>
        /// <param name="size"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetReceiveData(int size)
        {
            Data.Set(CopyBuffer.Buffer, CopyBuffer.StartIndex + sizeof(int), size);
        }
        /// <summary>
        /// 接收数据解压缩
        /// </summary>
        /// <param name="size"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool DeCompressReceiveData(int size, int dataSize)
        {
            CompressBuffer.StartIndex = dataSize;
            AutoCSer.IO.Compression.DeflateDeCompressor.Get(CopyBuffer.Buffer, CopyBuffer.StartIndex + sizeof(int) * 2, size, ref CompressBuffer);
            if (CompressBuffer.Buffer != null)
            {
                Data.Set(CompressBuffer.Buffer, CompressBuffer.StartIndex, dataSize);
                return true;
            }
            ReturnType = TcpServer.ReturnType.ClientDeSerializeError;
            return false;
        }
    }
}

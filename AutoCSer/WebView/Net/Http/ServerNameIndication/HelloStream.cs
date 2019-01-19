using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Net.Http.ServerNameIndication
{
    /// <summary>
    /// SSL 客户端 Hello 流，用于获取请求的 domain
    /// </summary>
    internal sealed partial class HelloStream : Stream, IAsyncResult
    {
        /// <summary>
        /// 等待异步操作完成
        /// </summary>
        private readonly ManualResetEvent asyncWaitHandle = new ManualResetEvent(true);

        /// <summary>
        /// HTTP 套接字安全流
        /// </summary>
        internal readonly SslSocket Socket;
        /// <summary>
        /// 当前 Hello 数据读取位置
        /// </summary>
        private int readIndex;
        /// <summary>
        /// 未读取 Hello 数据长度
        /// </summary>
        private int helloSize;
        /// <summary>
        /// 异步读取数据长度
        /// </summary>
        private int asyncReadSize;
        /// <summary>
        /// 数据写入超时毫秒
        /// </summary>
        public override int WriteTimeout
        {
            get { return Socket.NetworkStream.WriteTimeout; }
            set { Socket.NetworkStream.WriteTimeout = value; }
        }
        /// <summary>
        /// 数据读取超时毫秒
        /// </summary>
        public override int ReadTimeout
        {
            get { return Socket.NetworkStream.ReadTimeout; }
            set { Socket.NetworkStream.ReadTimeout = value; }
        }
        /// <summary>
        /// 超时设置是否有效
        /// </summary>
        public override bool CanTimeout
        {
            get { return Socket.NetworkStream.CanTimeout; }
        }
        /// <summary>
        /// 是否支持读取
        /// </summary>
        public override bool CanRead
        {
            get { return helloSize != 0 || Socket.NetworkStream.CanRead; }
        }
        /// <summary>
        /// 是否支持查找，始终返回 false。
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }
        /// <summary>
        /// 是否支持写入
        /// </summary>
        public override bool CanWrite
        {
            get { return Socket.NetworkStream.CanWrite; }
        }
        /// <summary>
        /// System.NotSupportedException
        /// </summary>
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }
        /// <summary>
        /// System.NotSupportedException
        /// </summary>
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
        /// <summary>
        /// 是否同步完成
        /// </summary>
        bool IAsyncResult.CompletedSynchronously { get { return true; } }
        /// <summary>
        /// 异步操作的信息
        /// </summary>
        public object AsyncState { get; private set; }
        /// <summary>
        /// 等待异步操作完成
        /// </summary>
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return asyncWaitHandle; } }
        /// <summary>
        /// 操作是否已完成
        /// </summary>
        bool IAsyncResult.IsCompleted { get { return true; } }
        /// <summary>
        /// SSL 客户端 Hello 流
        /// </summary>
        /// <param name="socket">HTTP 套接字安全流</param>
        internal HelloStream(SslSocket socket)
        {
            Socket = socket;
        }
        /// <summary>
        /// 读取 Hello 数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ReadHello()
        {
            AsyncState = null;
            if (!Socket.NetworkStream.BeginRead(Socket.Header.Buffer.Buffer, readIndex = Socket.Header.Buffer.StartIndex, Socket.Header.Buffer.Length, OnRead, this).CompletedSynchronously) Header.ReceiveTimeout.Push(Socket, Socket.Socket);
        }
        /// <summary>
        /// 读取 Hello 数据
        /// </summary>
        /// <param name="result"></param>
        private void OnRead(IAsyncResult result)
        {
            if (!result.CompletedSynchronously) Header.ReceiveTimeout.Cancel(Socket);
            try
            {
                helloSize = Socket.NetworkStream.EndRead(result);
                if (helloSize > 43)
                {
                    SubArray<byte> domain = getDomain();
                    if (domain.Length != 0)
                    {
                        HttpRegister.SslCertificate certificate = new AutoCSer.Net.Http.UnionType { Value = Socket.Server }.SslServer.GetCertificate(ref domain);
                        if (certificate != null)
                        {
                            Socket.SslStream = certificate.CreateSslStream(this);
                            return;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Socket.Server.RegisterServer.TcpServer.Log.Add(Log.LogType.Debug, error);
            }
            Socket.HeaderError();
        }
        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <returns></returns>
        private unsafe SubArray<byte> getDomain()
        {
            fixed (byte* bufferFixed = Socket.Header.Buffer.Buffer)
            {
                byte* start = bufferFixed + readIndex;
                if (*start == (byte)MessageType.Handshake && start[1] == 3 && start[5] == (byte)HandshakeType.ClientHello)
                {
                    switch (start[2])
                    {
                        case (byte)MinorVersion.SSL3:
                        case (byte)MinorVersion.TLS1:
                        case (byte)MinorVersion.TLS11:
                        case (byte)MinorVersion.TLS12:
                            if (helloSize >= ((int)start[7] << 8) + start[8] + 9)
                            {
                                int sessionIndex = 43, cipherSuitesIndex = start[sessionIndex] + (43 + 1);
                                if (helloSize > cipherSuitesIndex)
                                {
                                    int compressionMethodIndex = ((int)start[cipherSuitesIndex] << 8) + (int)start[cipherSuitesIndex + 1] + (cipherSuitesIndex + 2);
                                    if ((uint)helloSize > (uint)compressionMethodIndex)
                                    {
                                        int extensionsIndex = start[compressionMethodIndex] + (compressionMethodIndex + 1);
                                        if (helloSize > extensionsIndex)
                                        {
                                            int endIndex = ((int)start[extensionsIndex] << 8) + start[extensionsIndex + 1] + (extensionsIndex + 2);
                                            if ((uint)helloSize >= (uint)helloSize)
                                            {
                                                extensionsIndex += 2;
                                                while (extensionsIndex < helloSize)
                                                {
                                                    int extensionIndex = extensionsIndex + ((int)start[extensionsIndex + sizeof(ushort)] << 8) + start[extensionsIndex + (sizeof(ushort) + 1)] + 4;
                                                    if ((uint)endIndex < (uint)extensionIndex || extensionIndex < extensionsIndex) return default(SubArray<byte>);
                                                    if (*(ushort*)(start + extensionsIndex) == 0)
                                                    {//Server Name
                                                        for (extensionsIndex += 4; extensionsIndex < extensionIndex;)
                                                        {
                                                            int serverNameEndIndex = extensionsIndex + ((int)start[extensionsIndex] << 8) + start[extensionsIndex + 1] + 2;
                                                            if ((uint)extensionIndex < (uint)serverNameEndIndex || serverNameEndIndex < extensionsIndex) return default(SubArray<byte>);
                                                            if (start[extensionsIndex + 2] == 0)
                                                            {//Host Name
                                                                return new SubArray<byte>(Socket.Header.Buffer.Buffer, Socket.Header.Buffer.StartIndex + extensionsIndex + 5, ((int)start[extensionsIndex + 3] << 8) + start[extensionsIndex + 4]);
                                                            }
                                                            extensionsIndex = serverNameEndIndex;
                                                        }
                                                    }
                                                    extensionsIndex = extensionIndex;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            return default(SubArray<byte>);
        }
        /// <summary>
        /// 刷新流中的数据
        /// </summary>
        public override void Flush()
        {
            Socket.NetworkStream.Flush();
        }
        /// <summary>
        /// System.NotSupportedException
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// System.NotSupportedException
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new System.NotSupportedException();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (helloSize == 0) return Socket.NetworkStream.Read(buffer, offset, count);
            if (helloSize >= count)
            {
                Buffer.BlockCopy(Socket.Header.Buffer.Buffer, readIndex, buffer, offset, count);
                readIndex += count;
                helloSize -= count;
                return count;
            }
            Buffer.BlockCopy(Socket.Header.Buffer.Buffer, readIndex, buffer, offset, helloSize);
            int size = Socket.NetworkStream.Read(buffer, offset + helloSize, count - helloSize) + helloSize;
            helloSize = 0;
            return size;
        }
        /// <summary>
        /// 异步读取数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            if (helloSize == 0)
            {
                asyncReadSize = 0;
                AsyncState = null;
                return Socket.NetworkStream.BeginRead(buffer, offset, size, callback, state);
            }
            if (size > helloSize) size = helloSize;
            Buffer.BlockCopy(Socket.Header.Buffer.Buffer, readIndex, buffer, offset, size);
            readIndex += size;
            helloSize -= size;
            AsyncState = state;
            asyncReadSize = size;
            callback(this);
            return this;
        }
        /// <summary>
        /// 异步读取的结束
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public override int EndRead(IAsyncResult asyncResult)
        {
            if (asyncReadSize == 0) return Socket.NetworkStream.EndRead(asyncResult);
            return asyncReadSize;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            Socket.NetworkStream.Write(buffer, offset, count);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
        {
            return Socket.NetworkStream.BeginWrite(buffer, offset, size, callback, state);
        }
        /// <summary>
        /// 处理异步写入结束
        /// </summary>
        /// <param name="asyncResult"></param>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            Socket.NetworkStream.EndWrite(asyncResult);
        }
    }
}

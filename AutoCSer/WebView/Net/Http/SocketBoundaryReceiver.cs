using System;
using System.Net.Sockets;
using AutoCSer.Extension;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 套接字数据接收器
    /// </summary>
    internal sealed unsafe class SocketBoundaryReceiver : BoundaryReceiver<Socket>, AutoCSer.Threading.ILinkTask
    {
#if DOTNET2
        /// <summary>
        /// 接收数据异步回调
        /// </summary>
        private AsyncCallback onReceiveAsyncCallback;
#else
        /// <summary>
        /// 异步套接字操作
        /// </summary>
        private SocketAsyncEventArgs socketAsyncEventArgs;
#endif
        /// <summary>
        /// HTTP 套接字数据接收器
        /// </summary>
        /// <param name="socket">HTTP套接字</param>
        internal SocketBoundaryReceiver(Socket socket)
            : base(socket)
        {
#if DOTNET2
            onReceiveAsyncCallback = onReceive;
#else
            socketAsyncEventArgs = SocketAsyncEventArgsPool.Get();
            socketAsyncEventArgs.Completed += onReceive;
#endif
        }
#if !DOTNET2
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            socketAsyncEventArgs.Completed -= onReceive;
            SocketAsyncEventArgsPool.PushNotNull(ref socketAsyncEventArgs);
        }
#endif
        /// <summary>
        /// 数据接收错误
        /// </summary>
        private void error()
        {
            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
            buffer.Free();
            httpSocket.HeaderError();
        }
        /// <summary>
        /// 开始接收表单数据
        /// </summary>
        internal void Receive()
        {
            try
            {
                SubBuffer.Size size = httpSocket.GetFormPage.MaxMemoryStreamSize;
                if ((contentLength = header.ContentLength) + sizeof(int) > (int)size >> 1) SubBuffer.Pool.GetPool(size).Get(ref buffer);
                else SubBuffer.Pool.GetBuffer(ref buffer, contentLength + sizeof(int));
                contentLength -= (receiveEndIndex = header.CopyToFormData(ref buffer));
                bufferSize = buffer.Length - sizeof(int);
#if !DOTNET2
                socketAsyncEventArgs.SocketError = SocketError.Success;
                socketAsyncEventArgs.SetBuffer(buffer.Buffer, receiveEndIndex, Math.Min(bufferSize - receiveEndIndex, contentLength));
#endif
                boundary = header.BoundaryIndex;
                httpSocket.SetTimeout(contentLength);
                if (onFirstBoundary()) return;
            }
            catch (Exception error)
            {
                httpSocket.DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.AutoCSer, error);
            }
            this.error();
        }
        /// <summary>
        /// 开始接收表单数据
        /// </summary>
        /// <returns></returns>
        private bool receive()
        {
            System.Net.Sockets.Socket socket = this.httpSocket.Socket;
            if (socket != null)
            {
#if DOTNET2
                SocketError socketError;
                IAsyncResult async = socket.BeginReceive(buffer.Buffer, buffer.StartIndex + receiveEndIndex, Math.Min(bufferSize - receiveEndIndex, contentLength), SocketFlags.None, out socketError, onReceiveAsyncCallback, socket);
                if (socketError == SocketError.Success)
                {
                    if (!async.CompletedSynchronously) Header.ReceiveTimeout.Push(this.httpSocket, socket);
                    return true;
                }
#else
                while (Interlocked.CompareExchange(ref httpSocket.ReceiveAsyncLock, 1, 0) != 0) Thread.Sleep(0);
                socketAsyncEventArgs.SetBuffer(buffer.StartIndex + receiveEndIndex, Math.Min(bufferSize - receiveEndIndex, contentLength));
                if (socket.ReceiveAsync(socketAsyncEventArgs))
                {
                    Header.ReceiveTimeout.Push(this.httpSocket, socket);
                    Interlocked.Exchange(ref httpSocket.ReceiveAsyncLock, 0);
                    return true;
                }
                Interlocked.Exchange(ref httpSocket.ReceiveAsyncLock, 0);
#endif
#if !DOTNET2
                if (socketAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = socketAsyncEventArgs.BytesTransferred;
                    if (count > 0)
                    {
                        receiveEndIndex += count;
                        contentLength -= count;
                        return onReceive();
                    }
                }
#endif
            }
            return false;
        }
#if DOTNET2
        /// <summary>
        /// 接收表单数据处理
        /// </summary>
        /// <param name="async">异步回调参数</param>
        private void onReceive(IAsyncResult async)
#else
        /// <summary>
        /// 接收表单数据处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="async">异步回调参数</param>
        private void onReceive(object sender, SocketAsyncEventArgs async)
#endif
        {
#if DOTNET2
            try
            {
                if (!async.CompletedSynchronously) Header.ReceiveTimeout.Cancel(httpSocket);
                System.Net.Sockets.Socket socket = new Net.UnionType { Value = async.AsyncState }.Socket;
                if (socket == httpSocket.Socket)
                {
                    SocketError socketError;
                    int count = socket.EndReceive(async, out socketError);
                    if (count > 0 && socketError == SocketError.Success)
                    {
                        receiveEndIndex += count;
                        contentLength -= count;
                        if (onReceive()) return;
                    }
                }
            }
            catch (Exception error)
            {
                httpSocket.DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.AutoCSer, error);
            }
#else
            try
            {
                Header.ReceiveTimeout.Cancel(httpSocket);
                if (socketAsyncEventArgs.SocketError == SocketError.Success)
                {
                    int count = socketAsyncEventArgs.BytesTransferred;
                    if (count > 0)
                    {
                        receiveEndIndex += count;
                        contentLength -= count;
                        if (onReceive()) return;
                    }
                }
            }
            catch (Exception error)
            {
                httpSocket.DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.AutoCSer, error);
            }
#endif
            this.error();
        }
        /// <summary>
        /// 接收表单数据处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onReceive()
        {
            switch (receiveType)
            {
                case BoundaryReceiveType.OnFirstBoundary: return onFirstBoundary();
                case BoundaryReceiveType.OnEnter: return onEnter();
                case BoundaryReceiveType.OnValue: return onValue();
                default: return false;
            }
        }
        /// <summary>
        /// 接收第一个分割符
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onFirstBoundary()
        {
            if (receiveEndIndex >= boundary.Length + 4) return checkFirstBoundary();
            receiveType = BoundaryReceiveType.OnFirstBoundary;
            return receive();
        }
        /// <summary>
        /// 检测第一个分隔符
        /// </summary>
        /// <returns></returns>
        private bool checkFirstBoundary()
        {
            int boundaryLength4 = boundary.Length + 4;
            fixed (byte* bufferFixed = buffer.Buffer, boundaryFixed = header.Buffer.Buffer)
            {
                byte* bufferStart = bufferFixed + buffer.StartIndex;
                if (*(short*)bufferStart == '-' + ('-' << 8) && Memory.EqualNotNull(boundaryFixed + (header.Buffer.StartIndex + boundary.StartIndex), bufferStart + 2, boundary.Length))
                {
                    int endValue = (int)*(short*)(bufferStart + 2 + boundary.Length);
                    if (endValue == 0x0a0d)
                    {
                        startIndex = currentIndex = boundaryLength4;
                        return onEnter();
                    }
                    if (((endValue ^ ('-' + ('-' << 8))) | (receiveEndIndex ^ boundaryLength4)) == 0) return boundaryReceiverFinally();
                }
            }
            return false;
        }
        /// <summary>
        /// 查找换行处理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onEnter()
        {
            return receiveEndIndex - currentIndex > sizeof(int) ? checkEnter() : receiveEnter();
        }
        /// <summary>
        /// 继续接收换行
        /// </summary>
        /// <returns></returns>
        private bool receiveEnter()
        {
            int length = receiveEndIndex - startIndex;
            if (length <= 1 << 10)
            {
                if (receiveEndIndex == bufferSize)
                {
                    fixed (byte* bufferFixed = buffer.Buffer)
                    {
                        byte* bufferStart = bufferFixed + buffer.StartIndex;
                        Memory.CopyNotNull(bufferStart + startIndex, bufferStart, length);
                    }
                    currentIndex -= startIndex;
                    receiveEndIndex = length;
                    startIndex = 0;
                }
                receiveType = BoundaryReceiveType.OnEnter;
                return receive();
            }
            return false;
        }
        /// <summary>
        /// 查找换行
        /// </summary>
        /// <returns></returns>
        private bool checkEnter()
        {
            int searchEndIndex = receiveEndIndex - sizeof(int);
            fixed (byte* dataFixed = buffer.Buffer)
            {
                byte* dataStart = dataFixed + buffer.StartIndex, start = dataStart + currentIndex, searchEnd = dataStart + searchEndIndex, end = dataStart + receiveEndIndex;
                *end = 13;
                do
                {
                    while (*start != 13) ++start;
                    if (start <= searchEnd)
                    {
                        if (*(int*)start == 0x0a0d0a0d)
                        {
                            currentIndex = (int)(start - dataStart);
                            return parseName();
                        }
                        ++start;
                    }
                    else
                    {
                        currentIndex = (int)(start - dataStart);
                        return receiveEnter();
                    }
                }
                while (true);
            }
        }
        /// <summary>
        /// 解析表单名称
        /// </summary>
        /// <returns></returns>
        private bool parseName()
        {
            currentName = currentValue = currentFileName = null;
            saveFileName = null;
            fixed (byte* dataFixed = buffer.Buffer)
            {
                byte* dataStart = dataFixed + buffer.StartIndex, start = dataStart + startIndex, end = dataStart + currentIndex;
                *end = (byte)';';
                do
                {
                    while (*start == ' ') ++start;
                    if (start == end) break;
                    if (*(int*)start == ('n' | ('a' << 8) | ('m' << 16) | ('e' << 24)))
                    {
                        currentName = getName(dataFixed, start += sizeof(int), end);
                        start += currentName.length() + 3;
                    }
                    else if (((*(int*)start ^ ('f' | ('i' << 8) | ('l' << 16) | ('e' << 24)))
                        | (*(int*)(start + sizeof(int)) ^ ('n' | ('a' << 8) | ('m' << 16) | ('e' << 24)))) == 0)
                    {
                        currentFileName = getName(dataFixed, start += sizeof(int) * 2, end);
                        start += currentFileName.length() + 3;
                    }
                    for (*end = (byte)';'; *start != ';'; ++start) ;
                }
                while (start++ != end);
                *end = 13;
            }
            if (currentName != null)
            {
                startIndex = valueEnterIndex = (currentIndex += 4);
                return onValue();
            }
            return false;
        }
        /// <summary>
        /// 接收表单值处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool onValue()
        {
            if (valueEnterIndex >= 0 ? receiveEndIndex - valueEnterIndex >= (boundary.Length + 4) : (receiveEndIndex - currentIndex >= (boundary.Length + 6)))
            {
                return checkValue();
            }
            return receiveValue();
        }
        /// <summary>
        /// 继续接收数据
        /// </summary>
        /// <returns></returns>
        private bool receiveValue()
        {
            if (receiveEndIndex == bufferSize)
            {
                if (startIndex == 0)
                {
                    linkTaskType = BoundaryReceiveLinkTaskType.WriteFile;
                    AutoCSer.Threading.LinkTask.Task.Add(this);
                    return true;
                }
                int length = receiveEndIndex - startIndex;
                fixed (byte* bufferFixed = buffer.Buffer)
                {
                    byte* bufferStart = bufferFixed + buffer.StartIndex;
                    Memory.CopyNotNull(bufferStart + startIndex, bufferStart, length);
                }
                currentIndex -= startIndex;
                valueEnterIndex -= startIndex;
                receiveEndIndex = length;
                startIndex = 0;
            }
            receiveType = BoundaryReceiveType.OnValue;
            return receive();
        }
        /// <summary>
        /// 接收表单值处理
        /// </summary>
        /// <returns></returns>
        private bool checkValue()
        {
            int boundaryLength2 = boundary.Length + 2;
            fixed (byte* bufferFixed = buffer.Buffer, boundaryFixed = header.Buffer.Buffer)
            {
                byte* bufferStart = bufferFixed + buffer.StartIndex, boundaryStart = boundaryFixed + (header.Buffer.StartIndex + boundary.StartIndex);
                byte* start = bufferStart + currentIndex, end = bufferStart + receiveEndIndex, last = bufferStart + valueEnterIndex;
                *end-- = 13;
                do
                {
                    while (*start != 13) ++start;
                    if (start >= end) break;
                    if ((int)(start - last) == boundaryLength2 && *(short*)last == ('-') + ('-' << 8)
                        && Memory.EqualNotNull(boundaryStart, last + 2, boundary.Length) && *(start + 1) == 10)
                    {
                        currentIndex = (int)(last - bufferStart) - 2;
                        if (fileStream == null)
                        {
                            getValue();
                            startIndex = currentIndex = (int)(start - bufferStart) + 2;
                            return onEnter();
                        }
                        valueEnterIndex = (int)(start - bufferStart) + 2;
                        linkTaskType = BoundaryReceiveLinkTaskType.GetFile;
                        AutoCSer.Threading.LinkTask.Task.Add(this);
                        return true;
                    }
                    last = *++start == 10 ? ++start : (bufferStart - buffer.Length);
                }
                while (true);
                int hash = (*(int*)(end -= 3) ^ ('-') + ('-' << 8) + 0x0a0d0000);
                if ((hash | (*(int*)(end -= boundary.Length + sizeof(int)) ^ 0x0a0d + ('-' << 16) + ('-' << 24))) == 0
                     && Memory.EqualNotNull(boundaryStart, end + sizeof(int), boundary.Length))
                {
                    currentIndex = (int)(end - bufferStart);
                    if (fileStream == null)
                    {
                        getValue();
                        return boundaryReceiverFinally();
                    }
                    linkTaskType = BoundaryReceiveLinkTaskType.GetFileFinally;
                    AutoCSer.Threading.LinkTask.Task.Add(this);
                    return true;
                }
                valueEnterIndex = (int)(last - bufferStart);
                currentIndex = (int)(start - bufferStart);
            }
            return receiveValue();
        }
        /// <summary>
        /// 表单数据接收完成
        /// </summary>
        /// <returns></returns>
        private bool boundaryReceiverFinally()
        {
            if (contentLength == 0)
            {
                long identity = httpSocket.Identity;
                try
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                    buffer.Free();
                    if (httpSocket.OnGetForm()) return true;
                }
                catch (Exception error)
                {
                    httpSocket.DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.AutoCSer, error);
                }
                httpSocket.ResponseError(identity, ResponseState.ServerError500);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        public AutoCSer.Threading.ILinkTask SingleRunLinkTask()
        {
            AutoCSer.Threading.ILinkTask next = NextLinkTask;
            NextLinkTask = null;
            try
            {
                switch (linkTaskType)
                {
                    case BoundaryReceiveLinkTaskType.WriteFile:
                        if(writeFile()) return next;
                        break;
                    case BoundaryReceiveLinkTaskType.GetFile:
                        if(getFile()) return next;
                        break;
                    case BoundaryReceiveLinkTaskType.GetFileFinally:
                        if(getFileFinally()) return next;
                        break;
                }
            }
            catch (Exception error)
            {
                httpSocket.DomainServer.RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            this.error();
            return next;
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <returns></returns>
        private bool writeFile()
        {
            if (fileStream == null)
            {
                fileStream = new FileStream(saveFileName = httpSocket.GetFormPage.GetSaveFileName(currentName) ?? (cacheFileName + ((ulong)AutoCSer.Pub.Identity).toHex()), FileMode.CreateNew, FileAccess.Write, FileShare.None);
            }
            if (valueEnterIndex > 0)
            {
                fileStream.Write(buffer.Buffer, buffer.StartIndex, valueEnterIndex);
                fixed (byte* bufferFixed = buffer.Buffer)
                {
                    byte* bufferStart = bufferFixed + buffer.StartIndex;
                    Memory.CopyNotNull(bufferStart + valueEnterIndex, bufferStart, receiveEndIndex -= valueEnterIndex);
                }
                valueEnterIndex = 0;
            }
            else
            {
                fileStream.Write(buffer.Buffer, buffer.StartIndex, receiveEndIndex);
                receiveEndIndex = 0;
                valueEnterIndex = -buffer.Length;
            }
            startIndex = 0;
            currentIndex = receiveEndIndex;
            receiveType = BoundaryReceiveType.OnValue;
            return receive();
        }
        /// <summary>
        /// 获取文件表单值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool getFile()
        {
            addFile();
            startIndex = currentIndex = valueEnterIndex;
            return onEnter();
        }
        /// <summary>
        /// 获取文件表单值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool getFileFinally()
        {
            addFile();
            return boundaryReceiverFinally();
        }
    }
}

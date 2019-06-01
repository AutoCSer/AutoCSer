using System;
using System.Threading;
using System.Net.Sockets;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.TcpInternalStreamServer
{
    /// <summary>
    /// TCP 内部服务套接字数据发送
    /// </summary>
    public sealed class ServerSocketSender : TcpStreamServer.ServerSocketSender<ServerAttribute, Server, ServerSocket, ServerSocketSender>
    {
#if !NOJIT
        /// <summary>
        /// TCP 内部服务套接字数据发送
        /// </summary>
        internal ServerSocketSender() : base() { }
#endif
        /// <summary>
        /// TCP 内部服务套接字数据发送
        /// </summary>
        /// <param name="socket">TCP 内部服务套接字</param>
        internal ServerSocketSender(ServerSocket socket) : base(socket) { }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Close()
        {
            if (!isClose && Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) close();
        }
        /// <summary>
        /// 释放接收数据缓冲区与异步事件对象
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void closeSocket()
        {
            close();
        }
        /// <summary>
        /// 释放接收数据缓冲区与异步事件对象
        /// </summary>
        private void close()
        {
            if (isClose) freeOutput();
            else
            {
                isClose = true;
                try
                {
                    ServerSocket.DisposeSocket();
                }
                catch (Exception error)
                {
                    Server.AddLog(error);
                }
                freeBuffer();
                callOnClose();
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal override void VirtualBuildOutput()
        {
            BuildOutput();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal unsafe void BuildOutput()
        {
            if (IsSocket)
            {
                TcpStreamServer.ServerOutput.OutputLink head = null, end;
                TcpServer.SenderBuildInfo buildInfo = new TcpServer.SenderBuildInfo { SendBufferSize = Server.SendBufferPool.Size, IsClientAwaiter = Server.Attribute.IsClientAwaiter };
                UnmanagedStream outputStream;
                SocketError socketError;
                try
                {
                    if (Buffer.Buffer == null) Server.SendBufferPool.Get(ref Buffer);
                    if (OutputSerializer == null) outputStream = (OutputSerializer = BinarySerialize.Serializer.YieldPool.Default.Pop() ?? new BinarySerialize.Serializer()).SetTcpServer();
                    else outputStream = OutputSerializer.Stream;
                    int outputSleep = OutputSleep, currentOutputSleep;
                    do
                    {
                        buildInfo.IsNewBuffer = 0;
                        fixed (byte* dataFixed = Buffer.Buffer)
                        {
                            byte* start = dataFixed + Buffer.StartIndex;
                            currentOutputSleep = outputSleep;
                            STREAM:
                            using (outputStream)
                            {
                                RESET:
                                if (outputStream.Data.Byte != start) outputStream.Reset(start, Buffer.Length);
                                buildInfo.Clear();
                                outputStream.ByteSize = 0;
                                if ((head = Outputs.GetClear(out end)) == null)
                                {
                                    if (currentOutputSleep < 0) goto CHECK;
                                    Thread.Sleep(currentOutputSleep);
                                    if (Outputs.IsEmpty) goto CHECK;
                                    head = Outputs.GetClear(out end);
                                    currentOutputSleep = 0;
                                }
                                LOOP:
                                do
                                {
                                    head = head.Build(this, ref buildInfo);
                                    if (buildInfo.IsSend != 0 || !ServerSocket.IsVerifyMethod) goto SETDATA;
                                }
                                while (head != null);
                                if (!Outputs.IsEmpty)
                                {
                                    head = Outputs.GetClear(out end);
                                    goto LOOP;
                                }
                                if (!buildInfo.IsClientAwaiter)
                                {
                                    if (currentOutputSleep >= 0)
                                    {
                                        Thread.Sleep(currentOutputSleep);
                                        if (!Outputs.IsEmpty)
                                        {
                                            head = Outputs.GetClear(out end);
                                            currentOutputSleep = 0;
                                            goto LOOP;
                                        }
                                    }
                                }
                                else
                                {
                                    if (currentOutputSleep == int.MinValue) currentOutputSleep = outputSleep;
                                    if (currentOutputSleep >= 0) Thread.Sleep(currentOutputSleep);
                                    else AutoCSer.Threading.ThreadYield.YieldOnly();
                                    if (!Outputs.IsEmpty)
                                    {
                                        head = Outputs.GetClear(out end);
                                        currentOutputSleep = 0;
                                        goto LOOP;
                                    }
                                }
                                CHECK:
                                if (outputStream.ByteSize == 0) goto DISPOSE;
                                SETDATA:
                                buildInfo.IsNewBuffer = setSendData(start);
                                SEND:
                                if (IsSocket)
                                {
                                    int sendSize = Socket.Send(sendData.Array, sendData.Start, sendData.Length, SocketFlags.None, out socketError);
                                    sendData.MoveStart(sendSize);
                                    if (sendData.Length == 0)
                                    {
                                        buildInfo.SendSizeLessCount = 0;
                                        if (buildInfo.IsNewBuffer == 0)
                                        {
                                            freeCopyBuffer();
                                            if (head == null)
                                            {
                                                currentOutputSleep = int.MinValue;
                                                goto RESET;
                                            }
                                            if (outputStream.Data.Byte != start) outputStream.Reset(start, Buffer.Length);
                                            buildInfo.Clear();
                                            outputStream.ByteSize = 0;
                                            //currentOutputSleep = outputSleep;
                                            goto LOOP;
                                        }
                                        CompressBuffer.TryFree();
                                        if (head != null) Outputs.PushHead(ref head, end);
                                        //currentOutputSleep = outputSleep;
                                        goto DISPOSE;
                                    }
                                    if (socketError == SocketError.Success && (sendSize >= TcpServer.Server.MinSocketSize || (sendSize > 0 && buildInfo.SendSizeLessCount++ == 0))) goto SEND;
                                    buildInfo.IsError = true;
                                }
                                buildInfo.IsNewBuffer = 0;
                                DISPOSE:;
                            }
                            if (buildInfo.IsNewBuffer == 0)
                            {
                                if (!buildInfo.IsError)
                                {
                                    if (IsSocket)
                                    {
                                        //IsOutput = 0;
                                        Interlocked.Exchange(ref IsOutput, 0);
                                        if (!Outputs.IsEmpty)
                                        {
                                            if (Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) goto STREAM;
                                        }
                                        else if (!IsSocket && Interlocked.CompareExchange(ref IsOutput, 1, 0) == 0) buildInfo.IsClose = true;
                                    }
                                    else buildInfo.IsClose = true;
                                }
                                return;
                            }
                        }
                    }
                    while (buildInfo.IsNewBuffer != 0);
                }
                catch (Exception error)
                {
                    Server.Log.Add(AutoCSer.Log.LogType.Error, error);
                    buildInfo.IsError = true;
                }
                finally
                {
                    if (buildInfo.IsError | buildInfo.IsClose)
                    {
                        if (buildInfo.IsError) closeSocket();
                        else close();
                        TcpStreamServer.ServerOutput.OutputLink.CancelLink(head);
                    }
                }
            }
            else close();
        }
    }
}

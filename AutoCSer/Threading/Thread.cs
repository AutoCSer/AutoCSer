using System;
using AutoCSer.Extension;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程操作
    /// </summary>
    public unsafe partial class Thread : Link<Thread>
    {
        /// <summary>
        /// 线程集合
        /// </summary>
        private static readonly Dictionary<int, System.Threading.Thread> threads = DictionaryCreator.CreateInt<System.Threading.Thread>();
        /// <summary>
        /// 线程集合访问锁
        /// </summary>
        private static readonly object threadsLock = new object();
        /// <summary>
        /// 获取线程集合
        /// </summary>
        /// <returns></returns>
        public static LeftArray<System.Threading.Thread> GetThreads()
        {
            LeftArray<System.Threading.Thread> threadArray = new LeftArray<System.Threading.Thread>(threads.Count);
            Monitor.Enter(threadsLock);
            try
            {
                foreach (System.Threading.Thread thread in threads.Values) threadArray.Add(thread);
            }
            finally { Monitor.Exit(threadsLock); }
            return threadArray;
        }
        /// <summary>
        /// 调用类型
        /// </summary>
        internal enum CallType : byte
        {
            None,
            /// <summary>
            /// 委托
            /// </summary>
            Action,
            /// <summary>
            /// 二进制序列化预编译
            /// </summary>
            CompileBinarySerialize,
            /// <summary>
            /// 二进制反序列化预编译
            /// </summary>
            CompileBinaryDeSerialize,
            /// <summary>
            /// JSON 序列化预编译
            /// </summary>
            CompileJsonSerialize,
            /// <summary>
            /// JSON 反序列化预编译
            /// </summary>
            CompileJsonDeSerialize,
            /// <summary>
            /// 简单序列化预编译
            /// </summary>
            CompileSimpleSerialize,
            /// <summary>
            /// 简单反序列化预编译
            /// </summary>
            CompileSimpleDeSerialize,
            /// <summary>
            /// 删除应用程序卸载处理
            /// </summary>
            DomainUnloadRemoveLast,
            /// <summary>
            /// 删除应用程序卸载处理
            /// </summary>
            DomainUnloadRemoveLastRun,
            /// <summary>
            /// 关闭文件流写入器
            /// </summary>
            FileStreamWriterDispose,
            /// <summary>
            /// 文件流写入器写入文件数据
            /// </summary>
            FileStreamWriteFile,
            /// <summary>
            /// 链表任务
            /// </summary>
            LinkTaskRun,
            /// <summary>
            /// 进程文件复制
            /// </summary>
            ProcessCopyer,
            /// <summary>
            /// 创建 TCP 服务客户端套接字
            /// </summary>
            TcpClientSocketBaseCreate,
            /// <summary>
            /// TCP 客户端套接字创建数据发送
            /// </summary>
            TcpClientSocketSenderVirtualBuildOutput,
            /// <summary>
            /// TCP 内部服务客户端套接字创建数据发送
            /// </summary>
            TcpInternalClientSocketSenderBuildOutput,
            /// <summary>
            /// TCP 内部服务获取客户端请求
            /// </summary>
            TcpInternalServerGetSocket,
            /// <summary>
            /// TCP 内部服务套接字创建数据发送
            /// </summary>
            TcpInternalServerSocketSenderBuildOutput,
            /// <summary>
            /// TCP 服务客户端套接字创建数据发送
            /// </summary>
            TcpOpenClientSocketSenderBuildOutput,
            /// <summary>
            /// TCP 服务获取客户端请求
            /// </summary>
            TcpOpenServerGetSocket,
            /// <summary>
            /// TCP 服务套接字处理
            /// </summary>
            TcpOpenServerOnSocket,
            /// <summary>
            /// TCP 服务套接字创建数据发送
            /// </summary>
            TcpOpenServerSocketSenderBuildOutput,
            /// <summary>
            /// TCP 服务套接字创建数据发送
            /// </summary>
            TcpServerSocketSenderVirtualBuildOutput,
        }
        /// <summary>
        /// 调用信息
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct CallInfo
        {
            /// <summary>
            /// 任务委托
            /// </summary>
            public object Value;
            /// <summary>
            /// 调用类型
            /// </summary>
            public CallType Type;
            /// <summary>
            /// 调用信息
            /// </summary>
            /// <param name="value"></param>
            /// <param name="type"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Set(object value, CallType type)
            {
                Value = value;
                Type = type;
            }
            /// <summary>
            /// 任务调用
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Call()
            {
                switch (Type)
                {
                    case CallType.Action: new UnionType { Value = Value }.Action(); return;
                    case CallType.CompileBinarySerialize: AutoCSer.BinarySerialize.Serializer.Compile(new UnionType { Value = Value }.TypeArray); return;
                    case CallType.CompileBinaryDeSerialize: AutoCSer.BinarySerialize.DeSerializer.Compile(new UnionType { Value = Value }.TypeArray); return;
                    case CallType.CompileJsonSerialize: AutoCSer.Json.Serializer.Compile(new UnionType { Value = Value }.TypeArray); return;
                    case CallType.CompileJsonDeSerialize: AutoCSer.Json.Parser.Compile(new UnionType { Value = Value }.TypeArray); return;
                    case CallType.CompileSimpleSerialize: AutoCSer.Net.SimpleSerialize.Serializer.Compile(new UnionType { Value = Value }.TypeArray); return;
                    case CallType.CompileSimpleDeSerialize: AutoCSer.Net.SimpleSerialize.DeSerializer.Compile(new UnionType { Value = Value }.TypeArray); return;
                    case CallType.DomainUnloadRemoveLast: new AutoCSer.DomainUnload.UnionType { Value = Value }.UnloadObject.RemoveLast(); return;
                    case CallType.DomainUnloadRemoveLastRun: new AutoCSer.DomainUnload.UnionType { Value = Value }.UnloadObject.RemoveLastRun(); return;
                    case CallType.FileStreamWriterDispose: new AutoCSer.IO.UnionType { Value = Value }.FileStreamWriter.Dispose(); return;
                    case CallType.FileStreamWriteFile: new AutoCSer.IO.UnionType { Value = Value }.FileStreamWriter.WriteFile(); return;
                    case CallType.LinkTaskRun: new UnionType { Value = Value }.LinkTask.Run(); return;
                    case CallType.ProcessCopyer: new AutoCSer.Diagnostics.UnionType { Value = Value }.ProcessCopyer.Copy(); return;
                    case CallType.TcpClientSocketBaseCreate: new AutoCSer.Net.TcpServer.UnionType { Value = Value }.ClientSocketBase.Create(); return;
                    case CallType.TcpClientSocketSenderVirtualBuildOutput: new AutoCSer.Net.TcpServer.UnionType { Value = Value }.ClientSocketSenderBase.VirtualBuildOutput(); return;
                    case CallType.TcpInternalClientSocketSenderBuildOutput: new AutoCSer.Net.TcpInternalServer.UnionType { Value = Value }.ClientSocketSender.BuildOutput(); return;
                    case CallType.TcpInternalServerGetSocket: new AutoCSer.Net.TcpInternalServer.UnionType { Value = Value }.Server.GetSocket(); return;
                    case CallType.TcpInternalServerSocketSenderBuildOutput: new AutoCSer.Net.TcpInternalServer.UnionType { Value = Value }.ServerSocketSender.BuildOutput(); return;
                    case CallType.TcpOpenClientSocketSenderBuildOutput: new AutoCSer.Net.TcpOpenServer.UnionType { Value = Value }.ClientSocketSender.BuildOutput(); return;
                    case CallType.TcpOpenServerGetSocket: new AutoCSer.Net.TcpOpenServer.UnionType { Value = Value }.Server.GetSocket(); return;
                    case CallType.TcpOpenServerOnSocket: new AutoCSer.Net.TcpOpenServer.UnionType { Value = Value }.Server.OnSocket(); return;
                    case CallType.TcpOpenServerSocketSenderBuildOutput: new AutoCSer.Net.TcpOpenServer.UnionType { Value = Value }.ServerSocketSender.BuildOutput(); return;
                    case CallType.TcpServerSocketSenderVirtualBuildOutput: new AutoCSer.Net.TcpServer.UnionType { Value = Value }.ServerSocketSenderBase.VirtualBuildOutput(); return;
                }
            }
        }
        /// <summary>
        /// 一次性等待锁
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        internal struct AutoWaitHandle
        {
            /// <summary>
            /// 同步等待锁
            /// </summary>
            private object waitLock;
            /// <summary>
            /// 是否等待中
            /// </summary>
            private volatile int isWait;
            /// <summary>
            /// 初始化数据
            /// </summary>
            /// <param name="isWait">是否等待中</param>
            internal void Set(int isWait)
            {
                waitLock = new object();
                this.isWait = isWait;
            }
            /// <summary>
            /// 等待结束
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Wait()
            {
                Monitor.Enter(waitLock);
                if (isWait == 0)
                {
                    isWait = 1;
                    Monitor.Wait(waitLock);
                }
                isWait = 0;
                Monitor.Exit(waitLock);
            }
            /// <summary>
            /// 结束等待
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Set()
            {
                Monitor.Enter(waitLock);
                if (isWait == 0) isWait = 1;
                else Monitor.Pulse(waitLock);
                Monitor.Exit(waitLock);
            }
        }
        /// <summary>
        /// 线程池
        /// </summary>
        private readonly ThreadPool threadPool;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 线程是否已经退出
        /// </summary>
        internal bool IsAborted
        {
            get
            {
                return threadHandle.ThreadState == System.Threading.ThreadState.Aborted;
            }
        }
        /// <summary>
        /// 任务委托
        /// </summary>
        private CallInfo task;
        ///// <summary>
        ///// 应用程序退出处理
        ///// </summary>
        //private DomainUnload.UnloadInfo domainUnload;
        ///// <summary>
        ///// 应用程序退出处理
        ///// </summary>
        //private Action<Exception> onError;
        /// <summary>
        /// 等待事件
        /// </summary>
        private AutoWaitHandle waitHandle;
        /// <summary>
        /// 线程池线程
        /// </summary>
        /// <param name="threadPool">线程池</param>
        internal Thread(ThreadPool threadPool)
        {
            waitHandle.Set(0);
            this.threadPool = threadPool;
            threadHandle = new System.Threading.Thread(exitTest, threadPool.StackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 线程池线程
        /// </summary>
        /// <param name="threadPool">线程池</param>
        /// <param name="task">任务委托</param>
        /// <param name="taskType">任务委托调用类型</param>
        internal Thread(ThreadPool threadPool, object task, CallType taskType)
        {
            this.task.Set(task, taskType);
            waitHandle.Set(0);
            this.threadPool = threadPool;
            threadHandle = threadPool.IsBackground ? new System.Threading.Thread(runBackground, threadPool.StackSize) : new System.Threading.Thread(run, threadPool.StackSize);
            threadHandle.IsBackground = true;
            //threadHandle.IsBackground = threadPool.IsBackground;
            appendThread();
            threadHandle.Start();
        }
        /// <summary>
        /// 添加线程集合
        /// </summary>
        private void appendThread()
        {
            Monitor.Enter(threadsLock);
            try
            {
                threads[threadHandle.ManagedThreadId] = threadHandle;
            }
            finally { Monitor.Exit(threadsLock); }
        }
        /// <summary>
        /// 移除线程集合
        /// </summary>
        private void removeThread()
        {
            Monitor.Enter(threadsLock);
            threads.Remove(threadHandle.ManagedThreadId);
            Monitor.Exit(threadsLock);
        }
        /// <summary>
        /// 退出测试线程
        /// </summary>
        private void exitTest()
        {
            if (threadPool.Push(this)) return;
            waitHandle.Wait();
            if (task.Type != CallType.None) runBackground();
        }
        /// <summary>
        /// 运行线程
        /// </summary>
        private void runBackground()
        {
            try
            {
                do
                {
                    try
                    {
                        do
                        {
                            //if (domainUnload.Type == AutoCSer.DomainUnload.Type.None) task.Call();
                            //else
                            //{
                            //    AutoCSer.DomainUnload.Unloader.Add(ref domainUnload);
                            task.Call();
                            //    domainUnload.Type = AutoCSer.DomainUnload.Type.None;
                            //    AutoCSer.DomainUnload.Unloader.Remove(ref domainUnload);
                            //}
                            task.Value = null;
                            //onError = null;
                            if (threadPool.Push(this)) return;
                            waitHandle.Wait();
                        }
                        while (task.Type != CallType.None);
                        return;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
                        //try
                        //{
                        //    if (onError == null) AutoCSer.Log.Pub.Log.add(AutoCSer.Log.LogType.Error, error);
                        //    else onError(error);
                        //}
                        //catch (Exception error1)
                        //{
                        //    AutoCSer.Log.Pub.Log.add(AutoCSer.Log.LogType.Debug, error1);
                        //}
                    }
                    finally
                    {
                        task.Value = null;
                        //onError = null;
                        //if (domainUnload.Type != AutoCSer.DomainUnload.Type.None) AutoCSer.DomainUnload.Unloader.Remove(ref domainUnload);
                    }
                    if (threadPool.Push(this)) return;
                    waitHandle.Wait();
                }
                while (task.Type != CallType.None);
            }
            finally
            {
                removeThread();
            }
        }
        /// <summary>
        /// 运行线程
        /// </summary>
        private void run()
        {
            try
            {
                do
                {
                    try
                    {
                        do
                        {
                            //if (domainUnload.Type == AutoCSer.DomainUnload.Type.None) task.Call();
                            //else
                            //{
                            //    AutoCSer.DomainUnload.Unloader.Add(ref domainUnload);
                            task.Call();
                            //    domainUnload.Type = AutoCSer.DomainUnload.Type.None;
                            //    AutoCSer.DomainUnload.Unloader.Remove(ref domainUnload);
                            //}
                            task.Value = null;
                            //onError = null;
                            if (threadPool.Push(this)) return;
                            waitHandle.Wait();
                        }
                        while (task.Type != CallType.None);
                        return;
                    }
                    catch (Exception error)
                    {
                        AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, error);
                        //try
                        //{
                        //    if (onError == null) AutoCSer.Log.Pub.Log.add(AutoCSer.Log.LogType.Error, error);
                        //    else onError(error);
                        //}
                        //catch (Exception error1)
                        //{
                        //    AutoCSer.Log.Pub.Log.add(AutoCSer.Log.LogType.Debug, error1);
                        //}
                    }
                    finally
                    {
                        task.Value = null;
                        //onError = null;
                        //if (domainUnload.Type != AutoCSer.DomainUnload.Type.None) AutoCSer.DomainUnload.Unloader.Remove(ref domainUnload);
                    }
                    if (threadPool.Push(this)) return;
                    waitHandle.Wait();
                }
                while (task.Type != CallType.None);
            }
            finally { removeThread(); }
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="task">任务委托</param>
        /// <param name="taskType">任务委托调用类型</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RunTask(object task, CallType taskType)
        {
            //this.domainUnload.Type = AutoCSer.DomainUnload.Type.None;
            //this.onError = null;
            this.task.Set(task, taskType);
            waitHandle.Set();
        }
        /// <summary>
        /// 结束线程
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Thread StopLink()
        {
            Thread next = LinkNext;
            task.Type = CallType.None;
            LinkNext = null;
            waitHandle.Set();
            return next;
        }
    }
}

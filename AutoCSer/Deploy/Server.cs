using System;
using AutoCSer.Net;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署服务
    /// </summary>
    [AutoCSer.Net.TcpInternalServer.Server(Name = Server.ServerName, Host = "127.0.0.1", Port = (int)ServerPort.Deploy, MinCompressSize = 1024)]
    public partial class Server : AutoCSer.Net.TcpInternalServer.TimeVerifyServer
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public const string ServerName = "Deploy";

        /// <summary>
        /// 自定义任务集合
        /// </summary>
        internal ServerCustomTask CustomTask = ServerCustomTask.Null;
        /// <summary>
        /// 切换服务前的调用
        /// </summary>
        public event Action BeforeSwitch;
        /// <summary>
        /// 客户端信息池
        /// </summary>
        private AutoCSer.Threading.IndexValuePool<ClientIdentity> clientPool;
        /// <summary>
        /// 部署信息池
        /// </summary>
        private AutoCSer.Threading.IndexValuePool<DeployInfo> deployPool;
        /// <summary>
        /// 服务端推送委托
        /// </summary>
        private Dictionary<int, Func<AutoCSer.Net.TcpServer.ReturnValue<byte[]>, bool>> onPushs = DictionaryCreator.CreateInt<Func<AutoCSer.Net.TcpServer.ReturnValue<byte[]>, bool>>();
        /// <summary>
        /// 服务端推送委托编号
        /// </summary>
        private int onPushIdentity;
        /// <summary>
        /// 服务端推送委托数量
        /// </summary>
        public int OnPushCount
        {
            get { return onPushs.Count; }
        }
        /// <summary>
        /// 新的获取服务端推送委托
        /// </summary>
        public event Action<Func<AutoCSer.Net.TcpServer.ReturnValue<byte[]>, bool>, int> OnGetPush;
        /// <summary>
        /// 部署服务
        /// </summary>
        public Server()
        {
            clientPool.Reset(16);
            deployPool.Reset(4);
        }
        /// <summary>
        /// 切换服务
        /// </summary>
        /// <param name="SwitchFile"></param>
        public void Switch(FileInfo SwitchFile)
        {
            this.TcpServer.StopListen();
            try
            {
                if (BeforeSwitch != null) BeforeSwitch();
            }
            finally { SwitchFile.StartProcessDirectory(); }
        }
        /// <summary>
        /// 设置自定义任务处理类型
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value">任务目标对象</param>
        public void SetCustomTask<valueType>(valueType value)
            where valueType : class
        {
            CustomTask = new ServerCustomTask<valueType>(value);
        }
        /// <summary>
        /// 注册客户端
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox, IsClientAwaiter = false)]
        private IndexIdentity register()
        {
            int index, identity;
            object arrayLock = clientPool.ArrayLock;
            Monitor.Enter(arrayLock);
            try
            {
                index = clientPool.GetIndexContinue();//不能写成一行，可能造成Pool先入栈然后被修改，导致索引溢出
                identity = clientPool.Array[index].Identity;
            }
            finally { Monitor.Exit(arrayLock); }
            return new IndexIdentity { Index = index, Identity = identity };
        }
        /// <summary>
        /// 清除客户端信息
        /// </summary>
        /// <param name="clientId"></param>
        internal void ClearClient(ref IndexIdentity clientId)
        {
            object arrayLock = clientPool.ArrayLock;
            Monitor.Enter(arrayLock);
            clientPool.Array[clientId.Index].Clear(clientId.Identity);
            Monitor.Exit(arrayLock);
        }
        /// <summary>
        /// 部署任务状态轮询
        /// </summary>
        /// <param name="clientId">服务端标识</param>
        /// <param name="onLog">部署任务状态更新回调</param>
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private void getLog(IndexIdentity clientId, Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> onLog)
        {
            object arrayLock = clientPool.ArrayLock;
            Monitor.Enter(arrayLock);
            clientPool.Array[clientId.Index].Set(clientId.Identity, onLog);
            Monitor.Exit(arrayLock);
        }
        /// <summary>
        /// 获取部署任务状态更新回调
        /// </summary>
        /// <param name="deployIdentity"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        internal Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> GetLog(ref IndexIdentity deployIdentity, ref IndexIdentity clientId)
        {
            object arrayLock = deployPool.ArrayLock;
            Monitor.Enter(arrayLock);
            if (deployPool.Array[deployIdentity.Index].GetClientId(deployIdentity.Identity, ref clientId))
            {
                Monitor.Exit(arrayLock);
                arrayLock = clientPool.ArrayLock;
                Monitor.Enter(arrayLock);
                Func<AutoCSer.Net.TcpServer.ReturnValue<Log>, bool> onLog = clientPool.Array[clientId.Index].GetLog(clientId.Identity);
                Monitor.Exit(arrayLock);
                return onLog;
            }
            else Monitor.Exit(arrayLock);
            return null;
        }
        /// <summary>
        /// 清除所有部署任务
        /// </summary>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous)]
        private void clear()
        {
            object arrayLock = deployPool.ArrayLock;
            Monitor.Enter(arrayLock);
            DeployInfo[] deployArray = deployPool.Array;
            int poolIndex = deployPool.PoolIndex;
            while (poolIndex != 0) deployArray[--poolIndex].Clear();
            deployPool.ClearIndexContinue();
            Monitor.Exit(arrayLock);
            GC.Collect();
        }
        /// <summary>
        /// 创建部署
        /// </summary>
        /// <param name="clientId">部署服务端标识</param>
        /// <returns>部署信息索引标识</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private IndexIdentity create(IndexIdentity clientId)
        {
            int index, identity;
            object arrayLock = deployPool.ArrayLock;
            Monitor.Enter(arrayLock);
            try
            {
                index = deployPool.GetIndexContinue();
                identity = deployPool.Array[index].Set(ref clientId);
            }
            finally { Monitor.Exit(arrayLock); }
            return new IndexIdentity { Index = index, Identity = identity };
        }
        /// <summary>
        /// 清除部署信息
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>       
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void clear(IndexIdentity identity)
        {
            Clear(ref identity);
        }
        /// <summary>
        /// 清除部署信息
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>       
        internal void Clear(ref IndexIdentity identity)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                if (deployPool.Array[identity.Index].Clear(identity.Identity))
                {
                    deployPool.FreeExit(identity.Index);
                    GC.Collect();
                }
                else Monitor.Exit(arrayLock);
            }
        }
        /// <summary>
        /// 启动部署
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>
        /// <param name="time">启动时间</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.ThreadPool, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private DeployState start(IndexIdentity identity, DateTime time)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    return deployPool.Array[identity.Index].Start(identity.Identity, time, new Timer { Server = this, Identity = identity });
                }
                finally { Monitor.Exit(arrayLock); }
            }
            return DeployState.IdentityError;
        }
        /// <summary>
        /// 设置文件数据源
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>
        /// <param name="files">文件数据源</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private bool setFileSource(IndexIdentity identity, byte[][] files)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                if (deployPool.Array[identity.Index].SetFiles(identity.Identity, files))
                {
                    Monitor.Exit(arrayLock);
                    return true;
                }
                Monitor.Exit(arrayLock);
            }
            return false;
        }
        /// <summary>
        /// 比较文件最后修改时间
        /// </summary>
        /// <param name="directory">目录信息</param>
        /// <param name="serverPath">服务器端路径</param>
        /// <returns></returns>
        [AutoCSer.Net.TcpServer.Method(ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private Directory getFileDifferent(Directory directory, string serverPath)
        {
            directory.Different(new DirectoryInfo(serverPath));
            return directory;
        }
        /// <summary>
        /// 添加web任务(css/js/html)
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>
        /// <param name="directory">目录信息</param>
        /// <param name="webFile">写文件任务信息</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private int addFiles(IndexIdentity identity, Directory directory, ClientTask.WebFile webFile, TaskType taskType)
        {
            //System.IO.FileInfo file = new System.IO.FileInfo(@"ServerDeSerializeError" + AutoCSer.Date.NowTime.Set().Ticks.ToString());
            //System.IO.File.WriteAllBytes(file.FullName, data.ToArray());
            //Console.WriteLine(file.FullName);
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    return deployPool.Array[identity.Index].AddTask(identity.Identity, new Task { Directory = directory, ServerDirectory = new DirectoryInfo(webFile.ServerPath), Type = taskType });
                }
                finally { Monitor.Exit(arrayLock); }
            }
            return -1;
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>
        /// <param name="files">文件集合</param>
        /// <param name="assemblyFile">写文件 exe/dll/pdb 任务信息</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private int addAssemblyFiles(IndexIdentity identity, KeyValue<string, int>[] files, ClientTask.AssemblyFile assemblyFile)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    return deployPool.Array[identity.Index].AddTask(identity.Identity, new Task { FileIndexs = files, ServerDirectory = new DirectoryInfo(assemblyFile.ServerPath), Type = TaskType.AssemblyFile });
                }
                finally { Monitor.Exit(arrayLock); }
            }
            return -1;
        }
        /// <summary>
        /// 写文件并运行程序
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>
        /// <param name="files">文件集合</param>
        /// <param name="run">写文件并运行程序 任务信息</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private int addRun(IndexIdentity identity, KeyValue<string, int>[] files, ClientTask.Run run)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    return deployPool.Array[identity.Index].AddTask(identity.Identity, new Task { FileIndexs = files, ServerDirectory = new DirectoryInfo(run.ServerPath), Type = TaskType.Run, RunFileName = run.FileName, RunSleep = run.Sleep, IsWaitRun = run.IsWait });
                }
                finally { Monitor.Exit(arrayLock); }
            }
            return -1;
        }
        /// <summary>
        /// 等待运行程序切换结束
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>
        /// <param name="taskIndex">任务索引位置</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private int addWaitRunSwitch(IndexIdentity identity, int taskIndex)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    return deployPool.Array[identity.Index].AddTask(identity.Identity, new Task { TaskIndex = taskIndex, Type = TaskType.WaitRunSwitch });
                }
                finally { Monitor.Exit(arrayLock); }
            }
            return -1;
        }
        /// <summary>
        /// 添加自定义任务
        /// </summary>
        /// <param name="identity">部署信息索引标识</param>
        /// <param name="custom">自定义任务处理 任务信息</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private int addCustom(IndexIdentity identity, ClientTask.Custom custom)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    return deployPool.Array[identity.Index].AddTask(identity.Identity, new Task { RunFileName = custom.CallName, CustomData = custom.CustomData, Type = TaskType.Custom });
                }
                finally { Monitor.Exit(arrayLock); }
            }
            return -1;
        }
        /// <summary>
        /// 自定义服务端推送
        /// </summary>
        /// <param name="onPush">推送委托</param>
        [AutoCSer.Net.TcpServer.KeepCallbackMethod(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Queue, ClientTask = AutoCSer.Net.TcpServer.ClientTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void customPush(Func<AutoCSer.Net.TcpServer.ReturnValue<byte[]>, bool> onPush)
        {
            onPushs.Add(++onPushIdentity, onPush);
            if (OnGetPush != null) OnGetPush(onPush, onPushIdentity);
        }
        /// <summary>
        /// 服务端推送
        /// </summary>
        /// <param name="customData"></param>
        /// <param name="onPushIdentity">0 表示所有客户端</param>
        public void CustomPush(byte[] customData, int onPushIdentity = 0)
        {
            TcpServer.CallQueue.Add(new CustomPushServerCall(this, customData, onPushIdentity));
        }
        /// <summary>
        /// 服务端推送
        /// </summary>
        /// <param name="customData"></param>
        /// <param name="onPushIdentity"></param>
        internal void CallCustomPush(byte[] customData, int onPushIdentity)
        {
            if (onPushIdentity == 0)
            {
                LeftArray<int> removeIdentitys = default(LeftArray<int>);
                foreach (KeyValuePair<int, Func<AutoCSer.Net.TcpServer.ReturnValue<byte[]>, bool>> onPush in onPushs)
                {
                    if (!onPush.Value(customData)) removeIdentitys.Add(onPush.Key);
                }
                foreach (int removeIdentity in removeIdentitys) onPushs.Remove(removeIdentity);
            }
            else
            {
                Func<AutoCSer.Net.TcpServer.ReturnValue<byte[]>, bool> onPush;
                if (onPushs.TryGetValue(onPushIdentity, out onPush))
                {
                    if (!onPush(customData)) onPushs.Remove(onPushIdentity);
                }
            }
        }

        /// <summary>
        /// 默认切换服务相对目录名称
        /// </summary>
        public const string DefaultSwitchDirectoryName = "Switch";
        /// <summary>
        /// 默认更新服务相对目录名称
        /// </summary>
        public const string DefaultUpdateDirectoryName = "Update";
        /// <summary>
        /// 发布服务更新以后的后续处理
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void OnDeployServerUpdated()
        {
            OnDeployServerUpdated(null);
        }
        /// <summary>
        /// 发布服务更新以后的后续处理
        /// </summary>
        /// <param name="deployServerFileName">发布服务文件名称</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称</param>
        /// <param name="updateDirectoryName">更新服务相对目录名称</param>
        public void OnDeployServerUpdated(string deployServerFileName, string switchDirectoryName = DefaultSwitchDirectoryName, string updateDirectoryName = DefaultUpdateDirectoryName)
        {
            DirectoryInfo CurrentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory), SwitchDirectory, UpdateDirectory;
            if (CurrentDirectory.Name == switchDirectoryName)
            {
                SwitchDirectory = CurrentDirectory.Parent;
                UpdateDirectory = new DirectoryInfo(Path.Combine(SwitchDirectory.FullName, updateDirectoryName));
            }
            else
            {
                SwitchDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory.FullName, switchDirectoryName));
                if (!SwitchDirectory.Exists) SwitchDirectory.Create();
                UpdateDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory.FullName, updateDirectoryName));
            }
            if (deployServerFileName == null) deployServerFileName = new FileInfo(deployServerFileName = Assembly.GetEntryAssembly().Location).Name;
            FileInfo UpdateFile = new FileInfo(Path.Combine(UpdateDirectory.FullName, deployServerFileName)), SwitchFile = new FileInfo(Path.Combine(SwitchDirectory.FullName, deployServerFileName));
            if (UpdateFile.LastWriteTimeUtc > SwitchFile.LastWriteTimeUtc)
            {
                foreach (FileInfo File in UpdateDirectory.GetFiles())
                {
                    FileInfo RemoveFile = new FileInfo(Path.Combine(SwitchDirectory.FullName, File.Name));
                    if (RemoveFile.Exists) RemoveFile.Delete();
                    File.MoveTo(RemoveFile.FullName);
                }
                Console.WriteLine(SwitchFile.FullName);
                Switch(new FileInfo(SwitchFile.FullName));
            }
        }
        /// <summary>
        /// 初始化时获取切换服务文件
        /// </summary>
        /// <param name="deployServerFileName">发布服务文件名称</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称</param>
        /// <returns>切换服务文件</returns>
        public static FileInfo GetSwitchFile(string deployServerFileName = null, string switchDirectoryName = DefaultSwitchDirectoryName)
        {
            DirectoryInfo CurrentDirectory = new DirectoryInfo(AutoCSer.PubPath.ApplicationPath), SwitchDirectory;
            if (CurrentDirectory.Name == switchDirectoryName)
            {
                SwitchDirectory = CurrentDirectory.Parent;
            }
            else
            {
                SwitchDirectory = new DirectoryInfo(Path.Combine(CurrentDirectory.FullName, switchDirectoryName));
            }
            if (SwitchDirectory.Exists)
            {
                if (deployServerFileName == null) deployServerFileName = new FileInfo(deployServerFileName = Assembly.GetEntryAssembly().Location).Name;
                FileInfo SwitchFile = new FileInfo(Path.Combine(SwitchDirectory.FullName, deployServerFileName));
                if (SwitchFile.Exists)
                {
                    FileInfo CurrentFile = new FileInfo(Path.Combine(CurrentDirectory.FullName, deployServerFileName));
                    if (SwitchFile.LastWriteTimeUtc > CurrentFile.LastWriteTimeUtc) return SwitchFile;
                }
            }
            return null;
        }
        /// <summary>
        /// 发布切换更新，返回当前检测文件
        /// </summary>
        /// <param name="deployPath">发布目标路径</param>
        /// <param name="checkFileName">检测文件名称</param>
        /// <param name="switchDirectoryName">切换服务相对目录名称</param>
        /// <param name="updateDirectoryName">更新服务相对目录名称</param>
        /// <returns>当前检测文件</returns>
        public static FileInfo UpdateSwitchFile(string deployPath, string checkFileName, string switchDirectoryName = DefaultSwitchDirectoryName, string updateDirectoryName = DefaultUpdateDirectoryName)
        {
            DirectoryInfo Directory = new DirectoryInfo(deployPath), MoveToDirectory = Directory;
            DirectoryInfo SwitchDirectory = new DirectoryInfo(Path.Combine(Directory.FullName, switchDirectoryName));
            FileInfo FileInfo = new FileInfo(Path.Combine(Directory.FullName, checkFileName));
            if (FileInfo.Exists)
            {
                if (SwitchDirectory.Exists)
                {
                    FileInfo SwitchFileInfo = new FileInfo(Path.Combine(SwitchDirectory.FullName, FileInfo.Name));
                    if (!SwitchFileInfo.Exists || SwitchFileInfo.LastWriteTimeUtc < FileInfo.LastWriteTimeUtc) MoveToDirectory = SwitchDirectory;
                }
                else
                {
                    SwitchDirectory.Create();
                    MoveToDirectory = SwitchDirectory;
                }
            }
            foreach (FileInfo File in new DirectoryInfo(Path.Combine(Directory.FullName, updateDirectoryName)).GetFiles())
            {
                FileInfo RemoveFile = new FileInfo(Path.Combine(MoveToDirectory.FullName, File.Name));
                if (RemoveFile.Exists) RemoveFile.Delete();
                File.MoveTo(RemoveFile.FullName);
            }
            return new FileInfo(Path.Combine(MoveToDirectory.FullName, FileInfo.Name));
        }
#if !DotNetStandard
        /// <summary>
        /// 检测日志输出
        /// </summary>
        public static void CheckThreadLog()
        {
            LeftArray<Thread> threads = AutoCSer.Threading.Thread.GetThreads();
            AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Debug, "活动线程数量 " + threads.Length.toString(), new StackFrame());
            int currentId = Thread.CurrentThread.ManagedThreadId;
            foreach (Thread thread in threads)
            {
                if (thread.ManagedThreadId != currentId)
                {
                    StackTrace stack = null;
                    Exception exception = null;
                    bool isSuspend = false;
                    try
                    {
#pragma warning disable 618
                        if ((thread.ThreadState & (System.Threading.ThreadState.StopRequested | System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Stopped | System.Threading.ThreadState.WaitSleepJoin | System.Threading.ThreadState.Suspended | System.Threading.ThreadState.AbortRequested | System.Threading.ThreadState.Aborted)) == 0)
                        {
                            thread.Suspend();
                            isSuspend = true;
                        }
                        stack = new StackTrace(thread, true);
#pragma warning restore 618
                        //if (stack.FrameCount == AutoCSer.Threading.Thread.DefaultFrameCount) stack = null;
                    }
                    catch (Exception error)
                    {
                        exception = error;
                    }
                    finally
                    {
#pragma warning disable 618
                        if (isSuspend) thread.Resume();
#pragma warning restore 618
                    }
                    if (exception != null)
                    {
                        try
                        {
                            AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Debug, exception);
                        }
                        catch { }
                    }
                    if (stack != null)
                    {
                        try
                        {
                            AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Debug, stack.ToString(), new StackFrame());
                        }
                        catch { }
                    }
                }
            }
        }
#endif
        /// <summary>
        /// 获取申请进程排他锁名称
        /// </summary>
        /// <returns></returns>
        private static string GetProcessEventWaitHandleName()
        {
            Assembly Assembly = Assembly.GetEntryAssembly();
            if (Assembly == null) throw new ArgumentNullException("Name is null");
            return Assembly.FullName;
        }
        /// <summary>
        /// 尝试申请进程排他锁
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static EventWaitHandle TryCreateProcessEventWaitHandle(string Name = null)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, Name ?? GetProcessEventWaitHandleName(), out createdProcessWait);
            return createdProcessWait ? processWait : null;
        }
        /// <summary>
        /// 设置申请进程排他锁
        /// </summary>
        /// <param name="Name"></param>
        public static void SetProcessEventWaitHandle(string Name = null)
        {
            using (EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, Name ?? GetProcessEventWaitHandleName()))
            {
                processWait.Set();
            }
        }
    }
}

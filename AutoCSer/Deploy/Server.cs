using System;
using AutoCSer.Net;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;

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
        /// 客户端信息池
        /// </summary>
        private AutoCSer.Threading.IndexValuePool<ClientIdentity> clientPool;
        /// <summary>
        /// 部署信息池
        /// </summary>
        private AutoCSer.Threading.IndexValuePool<DeployInfo> deployPool;
        /// <summary>
        /// 部署服务
        /// </summary>
        public Server()
        {
            clientPool.Reset(16);
            deployPool.Reset(4);
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
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private bool start(IndexIdentity identity, DateTime time)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    if (deployPool.Array[identity.Index].Start(identity.Identity, time, new Timer { Server = this, Identity = identity })) return true;
                }
                finally { Monitor.Exit(arrayLock); }
            }
            return false;
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
        /// <param name="serverPath">服务器端路径</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private int addFiles(IndexIdentity identity, Directory directory, string serverPath, TaskType taskType)
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
                    return deployPool.Array[identity.Index].AddTask(identity.Identity, new Task { Directory = directory, ServerDirectory = new DirectoryInfo(serverPath), Type = taskType });
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
        /// <param name="serverPath">服务器端路径</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private int addAssemblyFiles(IndexIdentity identity, KeyValue<string, int>[] files, string serverPath)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    return deployPool.Array[identity.Index].AddTask(identity.Identity, new Task { FileIndexs = files, ServerDirectory = new DirectoryInfo(serverPath), Type = TaskType.AssemblyFile });
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
        /// <param name="serverPath">服务器端路径</param>
        /// <param name="runSleep">运行前休眠</param>
        /// <returns>任务索引编号,-1表示失败</returns>
        [AutoCSer.Net.TcpServer.Method(ServerTask = AutoCSer.Net.TcpServer.ServerTaskType.Synchronous, ParameterFlags = AutoCSer.Net.TcpServer.ParameterFlags.SerializeBox)]
        private int addRun(IndexIdentity identity, KeyValue<string, int>[] files, string serverPath, int runSleep)
        {
            if ((uint)identity.Index < (uint)deployPool.PoolIndex)
            {
                object arrayLock = deployPool.ArrayLock;
                Monitor.Enter(arrayLock);
                try
                {
                    return deployPool.Array[identity.Index].AddTask(identity.Identity, new Task { FileIndexs = files, ServerDirectory = new DirectoryInfo(serverPath), Type = TaskType.Run, RunSleep = runSleep });
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
    }
}

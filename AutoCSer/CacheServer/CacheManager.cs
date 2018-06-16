using System;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    internal sealed unsafe partial class CacheManager
    {
        /// <summary>
        /// 缓存从服务
        /// </summary>
        internal readonly SlaveServer SlaveServer;
        /// <summary>
        /// 缓存服务配置
        /// </summary>
        internal readonly ServerConfig Config;
        /// <summary>
        /// 缓存主服务配置
        /// </summary>
        internal MasterServerConfig MasterConfig
        {
            get { return new UnionType { Value = Config }.MasterServerConfig; }
        }
        /// <summary>
        /// 缓存服务
        /// </summary>
        internal readonly AutoCSer.Net.TcpInternalServer.Server TcpServer;
        /// <summary>
        /// TCP 服务器端同步调用队列处理
        /// </summary>
        private readonly AutoCSer.Net.TcpServer.ServerCallCanDisposableQueue tcpQueue;
        /// <summary>
        /// 查询数据缓冲区
        /// </summary>
        private readonly Buffer queryBuffer = new Buffer();
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        private readonly Buffer loadBuffer;
        /// <summary>
        /// 释放资源
        /// </summary>
        private readonly Action disposeHandle;
        /// <summary>
        /// 数据结构定义集合
        /// </summary>
        internal readonly Dictionary<HashString, ServerDataStructure> DataStructures = DictionaryCreator<HashString>.Create<ServerDataStructure>();
        /// <summary>
        /// 短路径集合
        /// </summary>
        private readonly ShortPathItem[] shortPaths;
        /// <summary>
        /// 服务启动时间
        /// </summary>
        private readonly long startTicks = Date.NowTime.Set().Ticks;
        /// <summary>
        /// 文件流写入器
        /// </summary>
        internal FileStreamWriter File;
        /// <summary>
        /// 当前创建中的文件流写入器
        /// </summary>
        internal FileStreamWriter NewFile;
        /// <summary>
        /// 获取缓存数据链表
        /// </summary>
        private CacheGetter getterLink;
        /// <summary>
        /// 获取缓存数据链表
        /// </summary>
        private CacheGetter waitGetterLink;
        /// <summary>
        /// 当前获取缓存数据
        /// </summary>
        private CacheGetter currentGetter;
        /// <summary>
        /// 数据结构数组集合
        /// </summary>
        internal DataStructureItem[] Array = NullValue<DataStructureItem>.Array;
        /// <summary>
        /// 数据结构空闲索引
        /// </summary>
        internal LeftArray<int> FreeIndexs;
        /// <summary>
        /// 加载中的消息队列集合
        /// </summary>
        internal LeftArray<Cache.MessageQueue.Node> LoadMessageQueues;
        /// <summary>
        /// 创建缓存节点后的回调操作
        /// </summary>
        internal Action<IndexIdentity> OnDataStructureCreated;
        /// <summary>
        /// 当前短路径索引位置
        /// </summary>
        private int shortPathIndex;
        /// <summary>
        /// 是否允许写操作
        /// </summary>
        internal bool CanWrite;
        /// <summary>
        /// 是否重建中
        /// </summary>
        private bool isRebuild;
        /// <summary>
        /// 是否已经加载完成
        /// </summary>
        internal bool IsLoaded;
        /// <summary>
        /// 是否写文件持久化
        /// </summary>
        internal bool IsFile;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private volatile int isDisposed;
        /// <summary>
        /// 缓存管理
        /// </summary>
        /// <param name="config">缓存服务配置</param>
        /// <param name="tcpServer">缓存服务</param>
        internal CacheManager(MasterServerConfig config, AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            Config = config;
            TcpServer = tcpServer;
            tcpQueue = tcpServer.CallQueue;
            shortPaths = new ShortPathItem[config.GetShortPathCount];
            if (config.IsFile)
            {
                disposeHandle = Dispose;
                IsFile = true;
                File = new FileStreamWriter(this, config);
                foreach (Cache.MessageQueue.Node messageQueue in LoadMessageQueues) messageQueue.Start();
                LoadMessageQueues.SetNull();
                IsLoaded = CanWrite = true;
                AutoCSer.DomainUnload.Unloader.Add(disposeHandle, DomainUnload.Type.Action);
            }
            else IsLoaded = CanWrite = true;
        }
        /// <summary>
        /// 缓存管理
        /// </summary>
        /// <param name="slaveServer">缓存从服务</param>
        /// <param name="tcpServer">缓存服务</param>
        internal CacheManager(SlaveServer slaveServer, AutoCSer.Net.TcpInternalServer.Server tcpServer)
        {
            shortPaths = new ShortPathItem[slaveServer.Config.GetShortPathCount];
            loadBuffer = new Buffer();
            SlaveServer = slaveServer;
            TcpServer = tcpServer;
            Config = slaveServer.Config;
            CanWrite = false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0)
            {
                //timeout.Free();
                if (File != null) AutoCSer.DomainUnload.Unloader.Remove(disposeHandle, DomainUnload.Type.Action, false);
                tcpQueue.Add(new ServerCall.CacheManager(this, ServerCall.CacheManagerServerCallType.Dispose));
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void DisposeTcpQueue()
        {
            CanWrite = false;
            if (File != null)
            {
                File.Close();
                File = null;
            }
            CacheGetter getter = waitGetterLink;
            waitGetterLink = null;
            while (getter != null) getter = getter.Cancel();
            getter = getterLink;
            getterLink = currentGetter = null;
            while (getter != null) getter = getter.Cancel();
        }
        /// <summary>
        /// 重建文件流
        /// </summary>
        /// <returns></returns>
        internal ReturnType NewFileStream()
        {
            if (isDisposed == 0)
            {
                if (File != null)
                {
                    if (NewFile == null)
                    {
                        NewFile = new FileStreamWriter(File);
                        return currentGetter == null ? NewFile.Start(File) : ReturnType.Success;
                    }
                    return ReturnType.FileStreamExists;
                }
                return ReturnType.NotFoundFileStream;
            }
            return ReturnType.Disposed;
        }
        /// <summary>
        /// 重建文件流失败
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CreateNewFileStreamError()
        {
            Buffer.DisposeLink(NewFile.BufferLink.GetClear());
            NewFile = null;
        }
        /// <summary>
        /// 重建文件流
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OnCreatedNewFileStream()
        {
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(File.Close);
            File = NewFile;
            NewFile = null;
        }
        /// <summary>
        /// 下一个获取缓存数据
        /// </summary>
        internal void NextGetter()
        {
            if (NewFile == null)
            {
                if ((currentGetter = waitGetterLink) != null)
                {
                    waitGetterLink = currentGetter.LinkNext;
                    currentGetter.Start();
                    Append(currentGetter);
                }
            }
            else
            {
                currentGetter = null;
                NewFile.Start(File);
            }
        }
        /// <summary>
        /// 添加获取缓存数据
        /// </summary>
        /// <param name="getter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Append(CacheGetter getter)
        {
            getter.LinkNext = getterLink;
            getterLink = getter;
        }
        /// <summary>
        /// 添加获取缓存数据
        /// </summary>
        /// <param name="getter"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AppendWait(CacheGetter getter)
        {
            if (IsLoaded && currentGetter == null)
            {
                currentGetter = getter;
                getter.Start();
                Append(getter);
            }
            else
            {
                getter.LinkNext = waitGetterLink;
                waitGetterLink = getter;
            }
        }

        /// <summary>
        /// 数据立即写入文件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void WriteFile()
        {
            if (File != null) File.Write();
        }

        /// <summary>
        /// 加载操作数据
        /// </summary>
        /// <param name="loadData">加载数据</param>
        /// <returns>是否加载成功</returns>
        internal bool Load(ref LoadData loadData)
        {
            OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, ref loadData);
            do
            {
                if (parser.Load(ref loadData))
                {
                    switch (parser.OperationType)
                    {
                        case OperationParameter.OperationType.LoadArraySize:
                            int size = parser.ReadInt();
                            if (parser.IsEnd)
                            {
                                Array = new DataStructureItem[size];
                                isRebuild = true;
                                break;
                            }
                            return false;
                        case OperationParameter.OperationType.GetOrCreateDataStructure:
                            if(loadDataStructure(loadData.Buffer)) break;
                            return false;
                        case OperationParameter.OperationType.RemoveDataStructure:
                            if(loadRemoveDataStructure(loadData.Buffer)) break;
                            return false;
                        case OperationParameter.OperationType.LoadIndexIdentity:
                            if (loadIndexIdentity(ref parser)) break;
                            return false;
                        default:
                            ServerDataStructure dataStructure = parser.Get(Array);
                            if (dataStructure != null) dataStructure.Node.Operation(ref parser);
                            break;
                    }
                }
                else return false;
            }
            while (loadData.IsNext);
            return true;
        }
        /// <summary>
        /// 添加数据结构定义
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns></returns>
        private bool loadDataStructure(Buffer buffer)
        {
            DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
            ServerDataStructure dataStructure = new ServerDataStructure(this, buffer, ref dataStructureBuffer);
            int index = dataStructureBuffer.Identity.Index;
            if (loadBuffer == null)
            {
                if (!isRebuild && index != getFreeIndex()) return false;
            }
            else if (index >= Array.Length) Array = Array.copyNew(Math.Max(Array.Length << 1, Math.Max(sizeof(int), index + 1)));
            DataStructures.Add(dataStructureBuffer.CacheName, dataStructure);
            Array[index].Load(dataStructure);
            return true;
        }
        /// <summary>
        /// 删除数据结构定义
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns></returns>
        private bool loadRemoveDataStructure(Buffer buffer)
        {
            DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
            HashString name = dataStructureBuffer.CacheName;
            ServerDataStructure dataStructure;
            if (DataStructures.TryGetValue(name, out dataStructure))
            {
                int index = dataStructure.Identity.Index;
                if (dataStructureBuffer.Identity.Index == index)
                {
                    if (!isRebuild && loadBuffer == null) FreeIndexs.Add(index);
                    Array[index].LoadFree(dataStructureBuffer.Identity.Identity);
                    DataStructures.Remove(name);
                    dataStructure.Node.OnRemoved();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 加载数据结构索引标识
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        private bool loadIndexIdentity(ref OperationParameter.NodeParser parser)
        {
            if (parser.ReadInt() == Array.Length)
            {
                FreeIndexs.PrepLength(Array.Length);
                ServerDataStructure dataStructure;
                for (int index = Array.Length; index != 0; )
                {
                    if (!Array[--index].Load((ulong)parser.ReadLong(), out dataStructure)) return false;
                    if (dataStructure == null) FreeIndexs.UnsafeAdd(index);
                }
                isRebuild = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取空闲索引
        /// </summary>
        /// <returns></returns>
        private int getFreeIndex()
        {
            if (FreeIndexs.Length != 0) return FreeIndexs.UnsafePopOnly();
            int index = Array.Length;
            Array = Array.copyNew(Math.Max(index << 1, sizeof(int)));
            FreeIndexs.PrepLength(Array.Length - index);
            for (int freeIndex = Array.Length - 1; freeIndex != index; FreeIndexs.UnsafeAdd(freeIndex--)) ;
            return index;
        }
        /// <summary>
        /// 数据操作完以后的持久化处理
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        private void onOperation(Buffer buffer)
        {
            if (File != null)
            {
                buffer.Reference();
                File.BufferLink.Push(buffer);
                if (NewFile != null)
                {
                    buffer.Reference();
                    NewFile.BufferLink.Push(buffer);
                }
            }
            CacheGetter getter = getterLink;
            if (getter != null)
            {
                CacheGetter father = null;
                do
                {
                    if (getter.Append(buffer))
                    {
                        father = getter;
                        getter = getter.LinkNext;
                    }
                    else
                    {
                        if (getter == currentGetter) NextGetter();
                        getter = getter.LinkNext;
                        if (father == null) getterLink = getter;
                        else father.LinkNext = getter;
                    }
                }
                while (getter != null);
            }
            buffer.FreeReference();
        }
        /// <summary>
        /// 添加数据结构定义
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>数据结构索引标识</returns>
        internal IndexIdentity GetOrCreateDataStructure(Buffer buffer)
        {
            if (CanWrite)
            {
                DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
                HashString name = dataStructureBuffer.CacheName;
                ServerDataStructure dataStructure;
                if (DataStructures.TryGetValue(name, out dataStructure))
                {
                    if (dataStructure.IsNodeData(ref dataStructureBuffer.Data))
                    {
                        buffer.FreeReference();
                        return dataStructure.Identity;
                    }
                    buffer.FreeReference();
                    return new IndexIdentity { ReturnType = ReturnType.DataStructureNameExists };
                }
                OnDataStructureCreated = onDataStructureCreated;
                if ((dataStructure = new ServerDataStructure(this, buffer, ref dataStructureBuffer)).Node != null)
                {
                    int index = getFreeIndex();
                    DataStructures.Add(name, dataStructure);
                    Array[index].Set(index, dataStructure);
                    buffer.SetIdentity(ref dataStructure.Identity);
                    onOperation(buffer);
                    OnDataStructureCreated(dataStructure.Identity);
                    OnDataStructureCreated = onDataStructureCreated;
                    return dataStructure.Identity;
                }
                buffer.FreeReference();
                return new IndexIdentity { ReturnType = dataStructure.ReturnType };
            }
            buffer.FreeReference();
            return new IndexIdentity { ReturnType = ReturnType.CanNotWrite };
        }
        /// <summary>
        /// 删除数据结构定义
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>被删除数据结构索引标识</returns>
        internal IndexIdentity RemoveDataStructure(Buffer buffer)
        {
            if (CanWrite)
            {
                DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
                HashString name = dataStructureBuffer.CacheName;
                ServerDataStructure dataStructure;
                if (DataStructures.TryGetValue(name, out dataStructure))
                {
                    int index = dataStructure.Identity.Index;
                    FreeIndexs.Add(index);
                    buffer.SetIdentity(new IndexIdentity { Index = index, Identity = Array[index].Free() });
                    DataStructures.Remove(name);
                    onOperation(buffer);
                    dataStructure.Node.OnRemoved();
                    return dataStructure.Identity;
                }
                buffer.FreeReference();
                return new IndexIdentity { ReturnType = ReturnType.DataStructureNameNotFound };
            }
            buffer.FreeReference();
            return new IndexIdentity { ReturnType = ReturnType.CanNotWrite };
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>返回参数</returns>
        internal ValueData.Data Operation(Buffer buffer)
        {
            if (CanWrite)
            {
                fixed (byte* dataFixed = buffer.Array.Array)
                {
                    OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, buffer, dataFixed);
                    ServerDataStructure dataStructure = parser.Get(Array);
                    if (dataStructure != null)
                    {
                        dataStructure.Node.Operation(ref parser);
                        if (parser.IsOperation) onOperation(buffer);
                        else buffer.FreeReference();
                        return parser.ReturnParameter;
                    }
                }
                buffer.FreeReference();
                return new ValueData.Data(ReturnType.DataStructureIdentityError);
            }
            buffer.FreeReference();
            return new ValueData.Data(ReturnType.CanNotWrite);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="onOperation"></param>
        internal void Operation(Buffer buffer, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation)
        {
            int isReturn = 0;
            try
            {
                if (CanWrite)
                {
                    fixed (byte* dataFixed = buffer.Array.Array)
                    {
                        OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, buffer, dataFixed);
                        ServerDataStructure dataStructure = parser.Get(Array);
                        if (dataStructure != null)
                        {
                            parser.OnReturn = onOperation;
                            try
                            {
                                dataStructure.Node.Operation(ref parser);
                            }
                            finally
                            {
                                if (parser.OnReturn == null) isReturn = 1;
                            }
                            if (parser.IsOperation) this.onOperation(buffer);
                            else buffer.FreeReference();
                            return;
                        }
                    }
                    buffer.FreeReference();
                    isReturn = 1;
                    onOperation(new ReturnParameter(ReturnType.DataStructureIdentityError));
                    return;
                }
                buffer.FreeReference();
            }
            finally
            {
                if (isReturn == 0) onOperation(new ReturnParameter(ReturnType.CanNotWrite));
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryData">查询数据</param>
        /// <returns>返回参数</returns>
        internal ValueData.Data Query(ref SubArray<byte> queryData)
        {
            queryBuffer.Set(ref queryData);
            fixed (byte* dataFixed = queryData.Array)
            {
                OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, queryBuffer, dataFixed);
                ServerDataStructure dataStructure = parser.Get(Array);
                if (dataStructure != null)
                {
                    dataStructure.Node.Query(ref parser);
                    return parser.ReturnParameter;
                }
            }
            return new ValueData.Data(ReturnType.DataStructureIdentityError);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryData">查询数据</param>
        /// <param name="onQuery"></param>
        /// <param name="isDeSerializeStream">是否反序列化网络流，否则需要 Copy 数据</param>
        internal void Query(ref SubArray<byte> queryData, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery, bool isDeSerializeStream)
        {
            try
            {
                queryBuffer.Set(ref queryData);
                fixed (byte* dataFixed = queryData.Array)
                {
                    OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, queryBuffer, dataFixed);
                    ServerDataStructure dataStructure = parser.Get(Array);
                    if (dataStructure != null)
                    {
                        parser.SetOnReturn(onQuery, isDeSerializeStream);
                        onQuery = null;
                        try
                        {
                            dataStructure.Node.Query(ref parser);
                        }
                        finally
                        {
                            parser.CallOnReturn();
                        }
                    }
                }
            }
            finally
            {
                if (onQuery != null) onQuery(new ReturnParameter(ReturnType.DataStructureIdentityError));
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryData">查询数据</param>
        /// <param name="onQuery"></param>
        /// <param name="isDeSerializeStream">是否反序列化网络流，否则需要 Copy 数据</param>
        internal void Query(ref SubArray<byte> queryData, Func<AutoCSer.Net.TcpServer.ReturnValue<IdentityReturnParameter>, bool> onQuery, bool isDeSerializeStream)
        {
            try
            {
                queryBuffer.Set(ref queryData);
                fixed (byte* dataFixed = queryData.Array)
                {
                    OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, queryBuffer, dataFixed);
                    ServerDataStructure dataStructure = parser.Get(Array);
                    if (dataStructure != null)
                    {
                        parser.SetOnReturn(onQuery, isDeSerializeStream);
                        onQuery = null;
                        try
                        {
                            dataStructure.Node.Query(ref parser);
                        }
                        finally
                        {
                            parser.CallOnReturnDistributionMessage();
                        }
                    }
                }
            }
            finally
            {
                if (onQuery != null) onQuery(new IdentityReturnParameter(ReturnType.DataStructureIdentityError));
            }
        }

        /// <summary>
        /// 创建短路径
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parser"></param>
        internal void CreateShortPath(Cache.Value.Node node, ref OperationParameter.NodeParser parser)
        {
            byte[] packet = parser.CreateReadPacket(OperationParameter.Serializer.HeaderSize);
            ulong identity = shortPaths[shortPathIndex].Set(node, packet);
            parser.ReturnParameter.ReturnParameterSetBinary(new ShortPathIdentity { Index = shortPathIndex, Identity = identity, Ticks = startTicks, PacketSize = packet.Length });
            if (++shortPathIndex == shortPaths.Length) shortPathIndex = 0;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parameter">短路径操作参数</param>
        /// <returns>返回参数</returns>
        internal ValueData.Data Operation(ref OperationParameter.ShortPathOperationNode parameter)
        {
            if (CanWrite)
            {
                if (parameter.Buffer != null)
                {
                    if (parameter.Identity.Ticks == startTicks)
                    {
                        byte[] packet;
                        Cache.Value.Node node = shortPaths[parameter.Identity.Index].Get(parameter.Identity.Identity, out packet);
                        if (packet.Length == parameter.Identity.PacketSize)
                        {
                            if (node.IsNode)
                            {
                                byte[] bufferData = parameter.Buffer.Array.Array;
                                System.Buffer.BlockCopy(packet, 0, bufferData, parameter.Buffer.Array.Start + OperationParameter.Serializer.HeaderSize, parameter.Identity.PacketSize);
                                fixed (byte* bufferFixed = bufferData)
                                {
                                    OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, parameter.Buffer, bufferFixed);
                                    parser.Read += parameter.Identity.PacketSize;
                                    node.Operation(ref parser);
                                    if (parser.IsOperation) onOperation(parameter.Buffer);
                                    else parameter.Buffer.FreeReference();
                                    return parser.ReturnParameter;
                                }
                            }
                            return new ValueData.Data(ReturnType.NotFoundShortPathNode);
                        }
                    }
                    return new ValueData.Data(ReturnType.NotFoundShortPath);
                }
                return new ValueData.Data(ReturnType.ServerDeSerializeError);
            }
            return new ValueData.Data(ReturnType.CanNotWrite);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parameter">短路径操作参数</param>
        /// <param name="onOperation"></param>
        internal void Operation(ref OperationParameter.ShortPathOperationNode parameter, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onOperation)
        {
            ReturnType returnType = ReturnType.CanNotWrite;
            try
            {
                if (CanWrite)
                {
                    if (parameter.Buffer != null)
                    {
                        returnType = ReturnType.NotFoundShortPath;
                        if (parameter.Identity.Ticks == startTicks)
                        {
                            byte[] packet;
                            Cache.Value.Node node = shortPaths[parameter.Identity.Index].Get(parameter.Identity.Identity, out packet);
                            if (packet.Length == parameter.Identity.PacketSize)
                            {
                                if (node.IsNode)
                                {
                                    returnType = ReturnType.Unknown;
                                    byte[] bufferData = parameter.Buffer.Array.Array;
                                    System.Buffer.BlockCopy(packet, 0, bufferData, parameter.Buffer.Array.Start + OperationParameter.Serializer.HeaderSize, parameter.Identity.PacketSize);
                                    fixed (byte* bufferFixed = bufferData)
                                    {
                                        OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, parameter.Buffer, bufferFixed);
                                        parser.Read += parameter.Identity.PacketSize;
                                        parser.OnReturn = onOperation;
                                        try
                                        {
                                            node.Operation(ref parser);
                                        }
                                        finally
                                        {
                                            if (parser.OnReturn == null) returnType = ReturnType.Success;
                                        }
                                        if (parser.IsOperation) this.onOperation(parameter.Buffer);
                                        else parameter.Buffer.FreeReference();
                                        return;
                                    }
                                }
                                else returnType = ReturnType.NotFoundShortPathNode;
                            }
                        }
                    }
                    else returnType = ReturnType.ServerDeSerializeError;
                }
                parameter.Buffer.FreeReference();
            }
            finally
            {
                if (returnType != ReturnType.Success) onOperation(new ReturnParameter(returnType));
            }
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryData">查询数据</param>
        /// <returns>返回参数</returns>
        internal ValueData.Data ShortPathQuery(ref SubArray<byte> queryData)
        {
            byte[] data = queryData.Array;
            ReturnType returnType = ReturnType.NotFoundShortPath;
            fixed (byte* dataFixed = queryData.Array)
            {
                byte* start = dataFixed + queryData.Start;
                ShortPathIdentity identity = new ShortPathIdentity(start + OperationParameter.Serializer.HeaderSize);
                if (identity.Ticks == startTicks)
                {
                    byte[] packet;
                    Cache.Value.Node node = shortPaths[identity.Index].Get(identity.Identity, out packet);
                    if (packet.Length == identity.PacketSize)
                    {
                        if (node.IsNode)
                        {
                            returnType = ReturnType.Unknown;
                            if ((*(int*)start = queryData.Length + identity.PacketSize - ShortPathIdentity.SerializeSize) <= queryData.Length)
                            {
                                System.Buffer.BlockCopy(packet, 0, data, queryData.Start + OperationParameter.Serializer.HeaderSize, identity.PacketSize);
                                System.Buffer.BlockCopy(data, queryData.Start + (OperationParameter.Serializer.HeaderSize + ShortPathIdentity.SerializeSize), data, queryData.Start + (OperationParameter.Serializer.HeaderSize + identity.PacketSize), queryData.Length - (OperationParameter.Serializer.HeaderSize + ShortPathIdentity.SerializeSize));
                                queryData.Length = *(int*)start;

                                queryBuffer.Set(ref queryData);
                                OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, queryBuffer, dataFixed);
                                parser.Read += identity.PacketSize;
                                node.Query(ref parser);
                                return parser.ReturnParameter;
                            }
                            else
                            {
                                Buffer buffer = BufferCount.GetBuffer(*(int*)start);
                                try
                                {
                                    SubArray<byte> bufferSubArray = buffer.Array;
                                    byte[] bufferData = bufferSubArray.Array;
                                    fixed (byte* bufferFixed = bufferData)
                                    {
                                        *(ulong*)(bufferFixed + bufferSubArray.Start) = *(ulong*)start;
                                        System.Buffer.BlockCopy(packet, 0, bufferData, bufferSubArray.Start + OperationParameter.Serializer.HeaderSize, identity.PacketSize);
                                        System.Buffer.BlockCopy(data, queryData.Start + (OperationParameter.Serializer.HeaderSize + ShortPathIdentity.SerializeSize), bufferData, bufferSubArray.Start + (OperationParameter.Serializer.HeaderSize + identity.PacketSize), queryData.Length - (OperationParameter.Serializer.HeaderSize + ShortPathIdentity.SerializeSize));

                                        OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, buffer, bufferFixed);
                                        parser.Read += identity.PacketSize;
                                        node.Query(ref parser);
                                        return parser.ReturnParameter;
                                    }
                                }
                                finally { buffer.Dispose(); }
                            }
                        }
                        else returnType = ReturnType.NotFoundShortPathNode;
                    }
                }
            }
            return new ValueData.Data(returnType);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryData">查询数据</param>
        /// <param name="onQuery"></param>
        /// <param name="isDeSerializeStream">是否反序列化网络流，否则需要 Copy 数据</param>
        internal void ShortPathQuery(ref SubArray<byte> queryData, Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, bool> onQuery, bool isDeSerializeStream)
        {
            byte[] data = queryData.Array;
            ReturnType returnType = ReturnType.ServerDeSerializeError;
            try
            {
                fixed (byte* dataFixed = queryData.Array)
                {
                    byte* start = dataFixed + queryData.Start;
                    ShortPathIdentity identity = new ShortPathIdentity(start + OperationParameter.Serializer.HeaderSize);
                    returnType = ReturnType.NotFoundShortPath;
                    if (identity.Ticks == startTicks)
                    {
                        byte[] packet;
                        Cache.Value.Node node = shortPaths[identity.Index].Get(identity.Identity, out packet);
                        if (packet.Length == identity.PacketSize)
                        {
                            if (node.IsNode)
                            {
                                returnType = ReturnType.Unknown;
                                if ((*(int*)start = queryData.Length + identity.PacketSize - ShortPathIdentity.SerializeSize) <= queryData.Length)
                                {
                                    System.Buffer.BlockCopy(packet, 0, data, queryData.Start + OperationParameter.Serializer.HeaderSize, identity.PacketSize);
                                    System.Buffer.BlockCopy(data, queryData.Start + (OperationParameter.Serializer.HeaderSize + ShortPathIdentity.SerializeSize), data, queryData.Start + (OperationParameter.Serializer.HeaderSize + identity.PacketSize), queryData.Length - (OperationParameter.Serializer.HeaderSize + ShortPathIdentity.SerializeSize));
                                    queryData.Length = *(int*)start;

                                    queryBuffer.Set(ref queryData);
                                    OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, queryBuffer, dataFixed);
                                    parser.Read += identity.PacketSize;
                                    parser.SetOnReturn(onQuery, isDeSerializeStream);
                                    onQuery = null;
                                    try
                                    {
                                        node.Query(ref parser);
                                    }
                                    finally
                                    {
                                        parser.CallOnReturn();
                                    }
                                }
                                else
                                {
                                    Buffer buffer = BufferCount.GetBuffer(*(int*)start);
                                    try
                                    {
                                        SubArray<byte> bufferSubArray = buffer.Array;
                                        byte[] bufferData = bufferSubArray.Array;
                                        fixed (byte* bufferFixed = bufferData)
                                        {
                                            *(ulong*)(bufferFixed + bufferSubArray.Start) = *(ulong*)start;
                                            System.Buffer.BlockCopy(packet, 0, bufferData, bufferSubArray.Start + OperationParameter.Serializer.HeaderSize, identity.PacketSize);
                                            System.Buffer.BlockCopy(data, queryData.Start + (OperationParameter.Serializer.HeaderSize + ShortPathIdentity.SerializeSize), bufferData, bufferSubArray.Start + (OperationParameter.Serializer.HeaderSize + identity.PacketSize), queryData.Length - (OperationParameter.Serializer.HeaderSize + ShortPathIdentity.SerializeSize));

                                            OperationParameter.NodeParser parser = new OperationParameter.NodeParser(this, buffer, bufferFixed);
                                            parser.Read += identity.PacketSize;
                                            parser.SetOnReturn(onQuery, isDeSerializeStream);
                                            onQuery = null;
                                            try
                                            {
                                                node.Query(ref parser);
                                            }
                                            finally
                                            {
                                                parser.CallOnReturn();
                                            }
                                        }
                                    }
                                    finally { buffer.Dispose(); }
                                }
                            }
                            else returnType = ReturnType.NotFoundShortPathNode;
                        }
                    }
                }
            }
            finally
            {
                if (onQuery != null) onQuery(new ReturnParameter(returnType));
            }
        }

        /// <summary>
        /// 获取数据结构索引标识
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <returns>数据结构索引标识</returns>
        internal IndexIdentity GetDataStructure(Buffer buffer)
        {
            DataStructureBuffer dataStructureBuffer = new DataStructureBuffer(buffer);
            HashString name = dataStructureBuffer.CacheName;
            ServerDataStructure dataStructure;
            if (DataStructures.TryGetValue(name, out dataStructure))
            {
                if (dataStructure.IsNodeData(ref dataStructureBuffer.Data))
                {
                    buffer.FreeReference();
                    return dataStructure.Identity;
                }
                buffer.FreeReference();
                return new IndexIdentity { ReturnType = ReturnType.DataStructureNameExists };
            }
            buffer.FreeReference();
            return new IndexIdentity { ReturnType = ReturnType.DataStructureNameNotFound };
        }
        /// <summary>
        /// 加载缓存数据
        /// </summary>
        /// <param name="parameter"></param>
        internal void Load(AutoCSer.Net.TcpServer.ReturnValue<CacheReturnParameter> parameter)
        {
            if (parameter.Type == AutoCSer.Net.TcpServer.ReturnType.Success) load(ref parameter.Value.LoadData);
        }
        /// <summary>
        /// 加载缓存数据
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe void load(ref SubArray<byte> data)
        {
            if (data.Array != null)
            {
                if (data.Length != 0)
                {
                    LoadData loadData = new LoadData { Buffer = loadBuffer };
                    fixed (byte* dataFixed = data.Array)
                    {
                        loadData.Set(ref data, dataFixed);
                        if (Load(ref loadData)) return;
                    }
                    TcpServer.Log.Add(Log.LogType.Fatal, "缓存数据解析失败");
                }
                else
                {
                    SlaveServer.Cache = this;
                    IsLoaded = true;
                    NextGetter();
                }
            }
        }

        /// <summary>
        /// 创建缓存节点后的回调操作
        /// </summary>
        private static readonly Action<IndexIdentity> onDataStructureCreated = onDataStructureCreatedNull;
        /// <summary>
        /// 创建缓存节点后的回调操作
        /// </summary>
        /// <param name="identity"></param>
        private static void onDataStructureCreatedNull(IndexIdentity identity) { }
    }
}

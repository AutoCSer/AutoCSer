using System;
using AutoCSer.Metadata;
using System.Threading;
using System.Linq.Expressions;
using System.Data.Common;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 日志
    /// </summary>
    public abstract class Log : Threading.LinkQueueTaskNode
    {
        /// <summary>
        /// 计算列加载完成字段名称
        /// </summary>
        public const string IsSqlLogProxyLoadedName = "_IsSqlLogProxyLoaded_";
        /// <summary>
        /// 数据表格
        /// </summary>
        protected readonly Table table;
        /// <summary>
        /// 等待事件
        /// </summary>
        private AutoCSer.Threading.WaitHandle wait;
        /// <summary>
        /// 等待加载成员位图
        /// </summary>
        private byte[] waitMap;
        /// <summary>
        /// 等待加载成员数量
        /// </summary>
        private int waitCount;
        /// <summary>
        /// 等待加载成员位图访问锁
        /// </summary>
        private int waitMapLock;
        /// <summary>
        /// 日志回调委托索引
        /// </summary>
        protected int onLogIndex;
        /// <summary>
        /// 日志回调委托访问锁
        /// </summary>
        protected int onLogLock;
        /// <summary>
        /// 是否开始处理日志
        /// </summary>
        protected int isStart;
        /// <summary>
        /// 是否加载完成
        /// </summary>
        protected bool isLoaded;
        /// <summary>
        /// 是否支持成员位图
        /// </summary>
        protected bool isMemberMap;
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="table"></param>
        /// <param name="memberIndexs"></param>
        protected Log(Table table, int[] memberIndexs)
        {
            wait.Set(0);
            if ((waitCount = memberIndexs.Length) > 0)
            {
                int maxIndex = memberIndexs.max(0);
                waitMap = new byte[(maxIndex >> 3) + 1];
                foreach (int memberIndex in memberIndexs) waitMap[memberIndex >> 3] |= (byte)(1 << (int)(memberIndex & 7));
            }
            waitCount += 3;
            (this.table = table).OnCacheLoaded += cacheLoaded;
        }
        /// <summary>
        /// 缓存加载完毕事件
        /// </summary>
        private void cacheLoaded()
        {
            table.OnCacheLoaded -= cacheLoaded;
            loadCount();
        }
        /// <summary>
        /// 加载成员处理
        /// </summary>
        /// <param name="memberIndex"></param>
        public void LoadMember(int memberIndex)
        {
            if ((uint)memberIndex < waitMap.Length << 3)
            {
                while (System.Threading.Interlocked.CompareExchange(ref waitMapLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.SqlLogStreamLoadMember);
                if ((waitMap[memberIndex >> 3] & (1 << (int)(memberIndex & 7))) == 0) System.Threading.Interlocked.Exchange(ref waitMapLock, 0);
                else
                {
                    waitMap[memberIndex >> 3] ^= (byte)(1 << (int)(memberIndex & 7));
                    if (--waitCount == 0)
                    {
                        System.Threading.Interlocked.Exchange(ref waitMapLock, 0);
                        loaded();
                        waitMap = null;
                    }
                    else System.Threading.Interlocked.Exchange(ref waitMapLock, 0);
                }
            }
        }
        /// <summary>
        /// 判断成员是否已经加载
        /// </summary>
        /// <param name="memberIndex"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WaitMember(int memberIndex)
        {
            byte[] waitMap = this.waitMap;
            if (waitMap != null && (waitMap[memberIndex >> 3] & (1 << (int)(memberIndex & 7))) != 0) wait.Wait();
        }
        /// <summary>
        /// 加载完成检测
        /// </summary>
        protected void loadCount()
        {
            while (System.Threading.Interlocked.CompareExchange(ref waitMapLock, 1, 0) != 0) AutoCSer.Threading.ThreadYield.Yield(AutoCSer.Threading.ThreadYield.Type.SqlLogStreamLoadMember);
            if (--waitCount == 0)
            {
                System.Threading.Interlocked.Exchange(ref waitMapLock, 0);
                loaded();
            }
            else System.Threading.Interlocked.Exchange(ref waitMapLock, 0);
        }
        /// <summary>
        /// 开始处理日志
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Start()
        {
            if (Interlocked.CompareExchange(ref isStart, 1, 0) == 0) loadCount();
        }
        /// <summary>
        /// 加载完成
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void loaded()
        {
            wait.Set();
            table.AddQueue(this);
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="connection"></param>
        internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
        {
            onLoaded();
            return LinkNext;
        }
        /// <summary>
        /// 缓存加载完毕
        /// </summary>
        protected abstract void onLoaded();
    }
    /// <summary>
    /// 日志
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    public class Log<valueType, modelType> : Log
        where valueType : class, modelType, IMemberMapValueLink<valueType>
        where modelType : class
    {
        /// <summary>
        /// 日志数据
        /// </summary>
        [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        public struct Data
        {
            /// <summary>
            /// 数据库更新记录
            /// </summary>
            internal MemberMapValue<modelType, valueType> Value;
            /// <summary>
            /// 日志类型
            /// </summary>
            internal LogType Type;
        }
        /// <summary>
        /// 添加日志处理委托
        /// </summary>
        private sealed class AddOnLogTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 日志
            /// </summary>
            internal Log<valueType, modelType> Log;
            /// <summary>
            /// 日志处理委托
            /// </summary>
            internal Func<AutoCSer.Net.TcpServer.ReturnValue<Data>, bool> OnLog;
            /// <summary>
            /// 是否仅队列处理
            /// </summary>
            internal bool IsQueue;
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                Log.add(OnLog, IsQueue);
                return LinkNext;
            }
        }
        /// <summary>
        /// 手动更新数据
        /// </summary>
        private sealed class UpdateTask : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 日志
            /// </summary>
            internal Log<valueType, modelType> Log;
            /// <summary>
            /// 数据
            /// </summary>
            internal valueType Value;
            /// <summary>
            /// 成员位图
            /// </summary>
            internal MemberMap<modelType> MemberMap;
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override Threading.LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                Log.update(Value, MemberMap);
                return LinkNext;
            }
        }
        /// <summary>
        /// 缓存
        /// </summary>
        private Cache.Whole.Event.Cache<valueType, modelType> cache;
        /// <summary>
        /// 日志回调委托集合
        /// </summary>
        private Func<AutoCSer.Net.TcpServer.ReturnValue<Data>, bool>[] onLogs;
        /// <summary>
        /// 添加数据日志
        /// </summary>
        private AutoCSer.Net.TcpServer.ReturnValue<Data> insertLog;
        /// <summary>
        /// 更新数据日志
        /// </summary>
        private AutoCSer.Net.TcpServer.ReturnValue<Data> updateLog;
        /// <summary>
        /// 更新数据日志
        /// </summary>
        private AutoCSer.Net.TcpServer.ReturnValue<Data> updateMemberMapLog;
        /// <summary>
        /// 删除数据日志
        /// </summary>
        private AutoCSer.Net.TcpServer.ReturnValue<Data> deleteLog;
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="table"></param>
        /// <param name="memberIndexs"></param>
        public Log(Table<valueType, modelType> table, params int[] memberIndexs)
            : base(table, memberIndexs ?? NullValue<int>.Array)
        {
            onLogs = new Func<Net.TcpServer.ReturnValue<Data>, bool>[table.Attribute.MaxLogStreamCount <= 0 ? TableAttribute.DefaultLogStreamCount : table.Attribute.MaxLogStreamCount];
            insertLog.Type = updateLog.Type = updateMemberMapLog.Type = deleteLog.Type = AutoCSer.Net.TcpServer.ReturnType.Success;
            insertLog.Value.Type = LogType.Insert;
            updateLog.Value.Type = updateMemberMapLog.Value.Type = LogType.Update;
            deleteLog.Value.Type = LogType.Delete;
            Start();
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="isMemberMap"></param>
        public void Set(Cache.Whole.Event.Cache<valueType, modelType> cache, bool isMemberMap)
        {
            if (cache == null) throw new ArgumentNullException();
            if (Interlocked.CompareExchange(ref this.cache, cache, null) == null)
            {
                this.isMemberMap = isMemberMap;
                if (isMemberMap)
                {
                    insertLog.Value.Value.MemberMap = updateLog.Value.Value.MemberMap = cache.MemberMap;
                    deleteLog.Value.Value.MemberMap = DataModel.Model<modelType>.GetIdentityOrPrimaryKeyMemberMap();
                }
                loadCount();
            }
            else throw new InvalidOperationException();
        }
        /// <summary>
        /// 创建数据更新成员位图
        /// </summary>
        /// <typeparam name="memberType"></typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public MemberMap<modelType> CreateMemberMap<memberType>(Expression<Func<modelType, memberType>> member)
        {
            if (member == null) throw new ArgumentNullException();
            MemberMap<modelType> memberMap = new MemberMap<modelType>();
            memberMap.SetMember(member);
            DataModel.Model<modelType>.SetIdentityOrPrimaryKeyMemberMap(memberMap);
            return memberMap;
        }
        /// <summary>
        /// 缓存加载完毕
        /// </summary>
        protected override void onLoaded()
        {
            table.CallOnLogMemberLoaded();
            for (int writeIndex = 0; writeIndex != onLogIndex; )
            {
                if (start(onLogs[writeIndex])) ++writeIndex;
                else
                {
                    for (int readIndex = writeIndex + 1; readIndex != onLogIndex; ++readIndex)
                    {
                        if (start(onLogs[readIndex])) onLogs[writeIndex++] = onLogs[readIndex];
                    }
                    for (int readIndex = writeIndex; readIndex != onLogIndex; onLogs[readIndex++] = null) ;
                    onLogIndex = writeIndex;
                }
            }
            cache.OnInserted += onInserted;
            cache.OnUpdated += onUpdated;
            cache.OnDeleted += onDeleted;
            isLoaded = true;
        }
        /// <summary>
        /// 日志委托初始化处理
        /// </summary>
        /// <param name="onLog"></param>
        /// <returns></returns>
        private bool start(Func<AutoCSer.Net.TcpServer.ReturnValue<Data>, bool> onLog)
        {
            int count = table.Attribute.OnLogStreamCount;
            foreach (valueType value in cache.Values)
            {
                insertLog.Value.Value.Value = value;
                if (!onLog(insertLog)) return false;
                if (--count == 0)
                {
                    Thread.Sleep(1);
                    count = table.Attribute.OnLogStreamCount;
                }
            }
            return onLog(new Data { Type = LogType.Loaded });
        }
        /// <summary>
        /// 添加日志处理委托
        /// </summary>
        /// <param name="onLog"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(Func<AutoCSer.Net.TcpServer.ReturnValue<Data>, bool> onLog)
        {
            if (onLog != null) table.AddQueue(new AddOnLogTask { Log = this, OnLog = onLog });
        }
        /// <summary>
        /// 添加日志处理委托
        /// </summary>
        /// <param name="onLog"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AddQueue(Func<AutoCSer.Net.TcpServer.ReturnValue<Data>, bool> onLog)
        {
            if (onLog != null) table.AddQueue(new AddOnLogTask { Log = this, OnLog = onLog, IsQueue = true });
        }
        /// <summary>
        /// 添加日志处理委托
        /// </summary>
        /// <param name="onLog"></param>
        /// <param name="isQueue"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void add(Func<AutoCSer.Net.TcpServer.ReturnValue<Data>, bool> onLog, bool isQueue)
        {
            if (onLogIndex == onLogs.Length) onLog(new Data { Type = LogType.CountError });
            else if (isLoaded || isQueue || start(onLog)) onLogs[onLogIndex++] = onLog;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        private void onInserted(valueType value)
        {
            insertLog.Value.Value.Value = value;
            onLog(ref insertLog);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cacheValue"></param>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        /// <param name="memberMap"></param>
        private void onUpdated(valueType cacheValue, valueType value, valueType oldValue, MemberMap<modelType> memberMap)
        {
            if (onLogIndex != 0)
            {
                if (isMemberMap)
                {
                    (memberMap = memberMap.Copy()).And(cache.MemberMap);
                    if (memberMap.IsAnyMember)
                    {
                        DataModel.Model<modelType>.SetIdentityOrPrimaryKeyMemberMap(memberMap);
                        updateMemberMapLog.Value.Value.Set(cacheValue, memberMap);
                        onLog(ref updateMemberMapLog);
                    }
                    else memberMap.Dispose();
                }
                else
                {
                    updateLog.Value.Value.Value = value;
                    onLog(ref updateLog);
                }
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        private void onDeleted(valueType value)
        {
            deleteLog.Value.Value.Value = value;
            onLog(ref deleteLog);
        }
        /// <summary>
        /// 日志处理
        /// </summary>
        /// <param name="data"></param>
        private void onLog(ref AutoCSer.Net.TcpServer.ReturnValue<Data> data)
        {
            for (int writeIndex = 0; writeIndex != onLogIndex; )
            {
                if (onLogs[writeIndex](data)) ++writeIndex;
                else
                {
                    for (int readIndex = writeIndex + 1; readIndex != onLogIndex; ++readIndex)
                    {
                        if (onLogs[readIndex](data)) onLogs[writeIndex++] = onLogs[readIndex];
                    }
                    for (int readIndex = writeIndex; readIndex != onLogIndex; onLogs[readIndex++] = null) ;
                    onLogIndex = writeIndex;
                }
            }
        }
        /// <summary>
        /// 手动更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Update(valueType value, MemberMap<modelType> memberMap)
        {
            table.AddQueue(new UpdateTask { Log = this, Value = value, MemberMap = memberMap });
        }
        /// <summary>
        /// 手动更新数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void update(valueType value, MemberMap<modelType> memberMap)
        {
            updateMemberMapLog.Value.Value.Set(value, memberMap);
            onLog(ref updateMemberMapLog);
        }
    }
}

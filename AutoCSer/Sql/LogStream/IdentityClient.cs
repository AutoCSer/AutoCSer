using System;
using AutoCSer.Log;
using AutoCSer.Extension;
using System.Threading;
using AutoCSer.Metadata;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 自增标识客户端
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    public sealed partial class IdentityClient<valueType, modelType> : Client<valueType, modelType, int>
        where valueType : class, modelType, IMemberMapValueLink<valueType>
        where modelType : class
    {
        /// <summary>
        /// 虚拟空日志流客户端
        /// </summary>
        public static readonly IdentityClient<valueType, modelType> Null = new IdentityClient<valueType, modelType>();
        /// <summary>
        /// 获取数据
        /// </summary>
        private new sealed class Getter : Client.Getter
        {
            /// <summary>
            /// 自增标识客户端
            /// </summary>
            private readonly IdentityClient<valueType, modelType> client;
            /// <summary>
            /// 加载数据缓存
            /// </summary>
            private LeftArray<valueType> array;
            /// <summary>
            /// 自增标识客户端
            /// </summary>
            /// <param name="client"></param>
            public Getter(IdentityClient<valueType, modelType> client)
            {
                this.client = client;
                try
                {
                    keepCallback = client.getLog(onLog);
                    if (keepCallback != null) return;
                }
                catch (Exception error)
                {
                    client.log.Add(AutoCSer.Log.LogType.Error, error);
                }
                this.error();
            }
            /// <summary>
            /// 错误处理
            /// </summary>
            private void error()
            {
                if (Interlocked.CompareExchange(ref isError, 1, 0) == 0)
                {
                    client.isLoaded = false;
                    AutoCSer.Threading.ThreadPool.TinyBackground.Start(client.onError);
                    array.SetNull();
                    if (keepCallback != null) keepCallback.Dispose();
                }
            }
            /// <summary>
            /// 日志流数据处理
            /// </summary>
            /// <param name="data"></param>
            private void onLog(AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data> data)
            {
                if (isError == 0)
                {
                    if (data.Type == Net.TcpServer.ReturnType.Success)
                    {
                        try
                        {
                            if (isLoaded)
                            {
                                if (client.onLog(ref data.Value)) return;
                            }
                            else
                            {
                                switch (data.Value.Type)
                                {
                                    case LogType.Insert: array.Add(data.Value.Value.Value); return;
                                    case LogType.Loaded: client.load(ref array); isLoaded = true; return;
                                }
                            }
                        }
                        catch (Exception error)
                        {
                            client.log.Add(AutoCSer.Log.LogType.Error, error);
                        }
                    }
                    this.error();
                }
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private Getter getter;
        /// <summary>
        /// 缓存数据
        /// </summary>
        private Cache.IdentityArray<valueType> array;
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public valueType this[int identity]
        {
            get
            {
                if (identity <= maxIdentity) return identity > 0 ? array[identity] : null;
                return getValue(identity);
            }
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 最大自增标识
        /// </summary>
        private int maxIdentity;
        /// <summary>
        /// 虚拟空日志流客户端
        /// </summary>
        private IdentityClient() : base() { }
        /// <summary>
        /// 日志流客户端
        /// </summary>
        /// <param name="getLog">获取日志数据委托</param>
        /// <param name="getValue">获取数据委托</param>
        /// <param name="getValueAwaiter">获取数据委托</param>
        /// <param name="custom">客户端自定义绑定</param>
        /// <param name="log">日志处理</param>
        public IdentityClient(Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback> getLog
                , Func<int, AutoCSer.Net.TcpServer.ReturnValue<valueType>> getValue, Func<int, AutoCSer.Net.TcpServer.AwaiterBox<valueType>> getValueAwaiter, Custom custom = null, ILog log = null)
            : base(getLog, DataModel.Model<modelType>.GetIdentity32, getValue, getValueAwaiter, custom, log)
        {
            getter = new Getter(this);
        }
        /// <summary>
        /// 虚拟客户端创建自增标识客户端
        /// </summary>
        /// <param name="tcpCallType">TCP 调用类型</param>
        /// <param name="custom">客户端自定义绑定</param>
        /// <param name="log">日志处理</param>
        /// <returns></returns>
        public IdentityClient<valueType, modelType> CreateNull(Type tcpCallType, Custom custom = null, ILog log = null)
        {
            if (this == Null)
            {
                Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback> getLog = (Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback>)Delegate.CreateDelegate(typeof(Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback>), tcpCallType.GetMethod("onSqlLog", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>) }, null));
                Func<int, AutoCSer.Net.TcpServer.ReturnValue<valueType>> getValue = (Func<int, AutoCSer.Net.TcpServer.ReturnValue<valueType>>)Delegate.CreateDelegate(typeof(Func<int, AutoCSer.Net.TcpServer.ReturnValue<valueType>>), tcpCallType.GetMethod("getSqlCache", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(int) }, null));
                Func<int, AutoCSer.Net.TcpServer.AwaiterBox<valueType>> getValueAwaiter = (Func<int, AutoCSer.Net.TcpServer.AwaiterBox<valueType>>)Delegate.CreateDelegate(typeof(Func<int, AutoCSer.Net.TcpServer.AwaiterBox<valueType>>), tcpCallType.GetMethod("getSqlCacheAwaiter", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(int) }, null));
                return new IdentityClient<valueType, modelType>(getLog, getValue, getValueAwaiter, custom, log);
            }
            return this;
        }
        /// <summary>
        /// 错误处理
        /// </summary>
        private void onError()
        {
            Thread.Sleep(1000);
            getter = new Getter(this);
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="array"></param>
        private void load(ref LeftArray<valueType> array)
        {
            int maxIdentity = array.maxKey(value => getKey(value), 0);
            Cache.IdentityArray<valueType> newArray = new Cache.IdentityArray<valueType>(maxIdentity + 1);
            foreach (valueType value in array) newArray[getKey(value)] = value;
            this.array = newArray;
            Count = array.Length;
            this.maxIdentity = maxIdentity;
            isLoaded = true;
            custom.Load(array);
        }
        /// <summary>
        /// 日志流数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool onLog(ref Log<valueType, modelType>.Data data)
        {
            valueType value = data.Value.Value;
            int identity = getKey(value);
            switch (data.Type)
            {
                case LogType.Insert:
                    if (identity >= array.Length) array.ToSize(identity + 1);
                    if (array[identity] == null) ++Count;
                    if (identity > maxIdentity) maxIdentity = identity;
                    custom.Insert(array[identity] = value);
                    return true;
                case LogType.Update:
                    if (identity < array.Length)
                    {
                        valueType clientValue = array[identity];
                        if (clientValue != null)
                        {
                            MemberMap<modelType> memberMap = data.Value.MemberMap;
                            custom.Update(clientValue, value, memberMap);
                            AutoCSer.MemberCopy.Copyer<modelType>.Copy(clientValue, value, memberMap);
                        }
                    }
                    MemberMapValueLinkPool<valueType>.PushNotNull(value);
                    return true;
                case LogType.Delete:
                    MemberMapValueLinkPool<valueType>.PushNotNull(value);
                    if (identity < array.Length)
                    {
                        valueType clientValue = array[identity];
                        if (clientValue != null)
                        {
                            array[identity] = null;
                            --Count;
                            custom.Delete(clientValue);
                        }
                    }
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取数据，尽量不要在 .NET 4.0 及以下版本中使用
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="value"></param>
        /// <returns>null 表示已经同步获取数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.AwaiterBox<valueType> Get(int identity, ref valueType value)
        {
            if (identity <= maxIdentity)
            {
                value = identity > 0 ? array[identity] : null;
                return null;
            }
            return getValueAwaiter(identity);
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="identitys"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType[] Get(AutoCSer.Net.TcpServer.ReturnValue<int[]> identitys)
        {
            if (identitys.Type == AutoCSer.Net.TcpServer.ReturnType.Success) return Get(identitys.Value);
            return new AutoCSer.Net.TcpServer.ReturnValue<valueType[]> { Type = identitys.Type };
        }
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <param name="identitys"></param>
        /// <returns></returns>
        public valueType[] Get(int[] identitys)
        {
            valueType[] values = new valueType[identitys.Length];
            int index = 0;
            foreach (int identity in identitys) values[index++] = this[identity];
            return values;
        }
    }
}

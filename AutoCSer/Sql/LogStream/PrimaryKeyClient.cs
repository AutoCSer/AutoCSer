using System;
using System.Reflection;
using AutoCSer.Extension;
using AutoCSer.Log;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 自增标识客户端
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public sealed class PrimaryKeyClient<valueType, modelType, keyType> : Client<valueType, modelType, keyType>
        where valueType : class, modelType, IMemberMapValueLink<valueType>
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 虚拟空日志流客户端
        /// </summary>
        public static readonly PrimaryKeyClient<valueType, modelType, keyType> Null = new PrimaryKeyClient<valueType, modelType, keyType>();
        /// <summary>
        /// 获取数据
        /// </summary>
        private new sealed class Getter : Client.Getter
        {
            /// <summary>
            /// 自增标识客户端
            /// </summary>
            private readonly PrimaryKeyClient<valueType, modelType, keyType> client;
            /// <summary>
            /// 加载数据缓存
            /// </summary>
            private Dictionary<RandomKey<keyType>, valueType> dictionary;
            /// <summary>
            /// 自增标识客户端
            /// </summary>
            /// <param name="client"></param>
            public Getter(PrimaryKeyClient<valueType, modelType, keyType> client)
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
                    dictionary = null;
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
                                    case LogType.Insert:
                                        if (dictionary == null) dictionary = DictionaryCreator<RandomKey<keyType>>.Create<valueType>();
                                        valueType value = data.Value.Value.Value;
                                        dictionary[client.getKey(value)] = value;
                                        return;
                                    case LogType.Loaded: client.load(dictionary ?? DictionaryCreator<RandomKey<keyType>>.Create<valueType>()); isLoaded = true; return;
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
        /// 数据集合
        /// </summary>
        private Dictionary<RandomKey<keyType>, valueType> dictionary;
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public valueType this[keyType key]
        {
            get
            {
                if (dictionary != null)
                {
                    valueType value;
                    if (dictionary.TryGetValue(key, out value)) return value;
                }
                return getValue(key);
            }
        }
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get { return dictionary == null ? 0 : dictionary.Count; }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        private Getter getter;
        /// <summary>
        /// 虚拟空日志流客户端
        /// </summary>
        private PrimaryKeyClient() : base() { }
        /// <summary>
        /// 日志流客户端
        /// </summary>
        /// <param name="getLog">获取日志数据委托</param>
        /// <param name="getValue">获取数据委托</param>
        /// <param name="getValueAwaiter">获取数据委托</param>
        /// <param name="custom">客户端自定义绑定</param>
        /// <param name="log">日志处理</param>
        public PrimaryKeyClient(Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback> getLog
                , Func<keyType, AutoCSer.Net.TcpServer.ReturnValue<valueType>> getValue, Func<keyType, AutoCSer.Net.TcpServer.AwaiterBox<valueType>> getValueAwaiter, Custom custom = null, ILog log = null)
            : base(getLog, AutoCSer.Data.Model<modelType>.GetPrimaryKeyGetter<keyType>("GetSqlPrimaryKey", DataModel.Model<modelType>.PrimaryKeys.getArray(value => value.FieldInfo)), getValue, getValueAwaiter, custom, log)
        {
            getter = new Getter(this);
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
        /// <param name="dictionary">数据集合</param>
        private void load(Dictionary<RandomKey<keyType>, valueType> dictionary)
        {
            this.dictionary = dictionary;
            isLoaded = true;
            custom.Load(dictionary.Values);
        }
        /// <summary>
        /// 日志流数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool onLog(ref Log<valueType, modelType>.Data data)
        {
            valueType value = data.Value.Value, clientValue;
            keyType key = getKey(value);
            switch (data.Type)
            {
                case LogType.Insert:
                    if (dictionary.TryGetValue(key, out clientValue)) dictionary[key] = value;
                    else dictionary.Add(key, value);
                    custom.Insert(value);
                    return true;
                case LogType.Update:
                    if (dictionary.TryGetValue(key, out clientValue))
                    {
                        MemberMap<modelType> memberMap = data.Value.MemberMap;
                        custom.Update(clientValue, value, memberMap);
                        AutoCSer.MemberCopy.Copyer<modelType>.Copy(clientValue, value, memberMap);
                    }
                    MemberMapValueLinkPool<valueType>.PushNotNull(value);
                    return true;
                case LogType.Delete:
                    MemberMapValueLinkPool<valueType>.PushNotNull(value);
                    if (dictionary.TryGetValue(key, out clientValue)) custom.Delete(clientValue);
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取数据，尽量不要在 .NET 4.0 及以下版本中使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>null 表示已经同步获取数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Net.TcpServer.AwaiterBox<valueType> Get(keyType key, ref valueType value)
        {
            return dictionary != null && !dictionary.TryGetValue(key, out value) ? null : getValueAwaiter(key);
        }
    }
}

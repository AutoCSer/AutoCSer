using System;
using AutoCSer.Log;
using AutoCSer.Metadata;
using System.Collections.Generic;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 日志流客户端
    /// </summary>
    public abstract class Client
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        protected abstract class Getter
        {
            /// <summary>
            /// 日志流处理保持回调
            /// </summary>
            protected AutoCSer.Net.TcpServer.KeepCallback keepCallback;
            /// <summary>
            /// 是否错误
            /// </summary>
            protected volatile int isError;
            /// <summary>
            /// 数据是否加载完成
            /// </summary>
            protected bool isLoaded;
        }
        /// <summary>
        /// 日志处理
        /// </summary>
        protected readonly ILog log;
        /// <summary>
        /// 数据是否加载完成
        /// </summary>
        protected bool isLoaded;
        /// <summary>
        /// 日志流客户端
        /// </summary>
        /// <param name="log">日志处理</param>
        protected Client(ILog log)
        {
            log = this.log;
        }
    }
    /// <summary>
    /// 日志流客户端
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    public abstract class Client<valueType, modelType> : Client
        where valueType : class, modelType, IMemberMapValueLink<valueType>
        where modelType : class
    {
        /// <summary>
        /// 客户端自定义绑定
        /// </summary>
        public class Custom
        {
            /// <summary>
            /// 重新加载数据
            /// </summary>
            /// <param name="values">初始化数据</param>
            public virtual void Load(ICollection<valueType> values) { }
            /// <summary>
            /// 添加数据
            /// </summary>
            /// <param name="value"></param>
            public virtual void Insert(valueType value) { }
            /// <summary>
            /// 更新数据
            /// </summary>
            /// <param name="value">更新之前的数据</param>
            /// <param name="updateValue">准备更新的数据</param>
            /// <param name="memberMap">准备更新的数据成员</param>
            public virtual void Update(valueType value, valueType updateValue, MemberMap<modelType> memberMap) { }
            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="value"></param>
            public virtual void Delete(valueType value) { }
            /// <summary>
            /// 虚拟客户端自定义绑定
            /// </summary>
            internal static readonly Custom Null = new Custom();
        }
        /// <summary>
        /// 获取日志数据委托
        /// </summary>
        protected readonly Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback> getLog;
        /// <summary>
        /// 客户端自定义绑定
        /// </summary>
        protected readonly Custom custom;
        /// <summary>
        /// 日志流客户端
        /// </summary>
        protected Client() : base(null) { }
        /// <summary>
        /// 日志流客户端
        /// </summary>
        /// <param name="getLog">获取日志数据委托</param>
        /// <param name="custom">客户端自定义绑定</param>
        /// <param name="log">日志处理</param>
        protected Client(Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback> getLog, Custom custom, ILog log)
            : base(log ?? AutoCSer.Log.Pub.Log)
        {
            if (getLog == null) throw new ArgumentNullException();
            this.getLog = getLog;
            this.custom = custom ?? Custom.Null;
        }
    }
    /// <summary>
    /// 日志流客户端
    /// </summary>
    /// <typeparam name="valueType">表格类型</typeparam>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public abstract class Client<valueType, modelType, keyType> : Client<valueType, modelType>
        where valueType : class, modelType, IMemberMapValueLink<valueType>
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 获取数据委托
        /// </summary>
        protected readonly Func<keyType, AutoCSer.Net.TcpServer.ReturnValue<valueType>> getValue;
        /// <summary>
        /// 获取数据委托
        /// </summary>
        protected readonly Func<keyType, AutoCSer.Net.TcpServer.AwaiterBox<valueType>> getValueAwaiter;
        /// <summary>
        /// 获取关键字委托
        /// </summary>
        protected readonly Func<modelType, keyType> getKey;
        /// <summary>
        /// 日志流客户端
        /// </summary>
        protected Client() : base() { }
        /// <summary>
        /// 日志流客户端
        /// </summary>
        /// <param name="getLog">获取日志数据委托</param>
        /// <param name="getKey">获取关键字委托</param>
        /// <param name="getValue">获取数据委托</param>
        /// <param name="getValueAwaiter">获取数据委托</param>
        /// <param name="custom">客户端自定义绑定</param>
        /// <param name="log">日志处理</param>
        protected Client(Func<Action<AutoCSer.Net.TcpServer.ReturnValue<Log<valueType, modelType>.Data>>, AutoCSer.Net.TcpServer.KeepCallback> getLog
            , Func<modelType, keyType> getKey, Func<keyType, AutoCSer.Net.TcpServer.ReturnValue<valueType>> getValue, Func<keyType, AutoCSer.Net.TcpServer.AwaiterBox<valueType>> getValueAwaiter, Custom custom, ILog log)
            : base(getLog, custom, log)
        {
            if (getKey == null) throw new ArgumentNullException("getKey is null");
            if (getValue == null) throw new ArgumentNullException("getValue is null");
            if (getValueAwaiter == null) throw new ArgumentNullException("getValueAwaiter is null");
            this.getKey = getKey;
            this.getValue = getValue;
            this.getValueAwaiter = getValueAwaiter;
        }
    }
}

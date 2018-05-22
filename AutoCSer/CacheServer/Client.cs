using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public abstract partial class Client
    {
        /// <summary>
        /// 数据结构定义信息集合
        /// </summary>
        internal readonly Dictionary<HashString, ClientDataStructure> CacheNames = AutoCSer.DictionaryCreator.CreateHashString<ClientDataStructure>();
        /// <summary>
        /// 数据结构定义信息集合访问锁
        /// </summary>
        protected readonly object cacheNameLock = new object();
        /// <summary>
        /// 获取或者创建数据结构信息
        /// </summary>
        /// <typeparam name="valueType">数据结构定义节点类型</typeparam>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns>数据结构定义节点</returns>
        public ReturnValue<valueType> GetOrCreateDataStructure<valueType>(string cacheName)
            where valueType : DataStructure.Abstract.Node
        {
            if (string.IsNullOrEmpty(cacheName)) cacheName = typeof(valueType).FullName;
            HashString hashName = cacheName;
            ClientDataStructure dataStructure;
            Monitor.Enter(cacheNameLock);
            try
            {
                if (CacheNames.TryGetValue(hashName, out dataStructure))
                {
                    if (dataStructure.NodeType == typeof(valueType)) return (valueType)dataStructure.Node;
                    return new ReturnValue<valueType> { Type = ReturnType.DataStructureNameExists };
                }
                Func<DataStructure.Abstract.Node, valueType> constructor = (Func<DataStructure.Abstract.Node, valueType>)DataStructure.Abstract.Node.GetConstructor(typeof(valueType));
                if (constructor != null)
                {
                    valueType node = constructor(null);
                    dataStructure = new ClientDataStructure(this, cacheName, typeof(valueType), node);
                    IndexIdentity identity = dataStructure.Identity;
                    if (identity.ReturnType == ReturnType.Success)
                    {
                        CacheNames.Add(hashName, dataStructure);
                        return node;
                    }
                    return new ReturnValue<valueType> { Type = identity.ReturnType, TcpReturnType = identity.TcpReturnType };
                }
            }
            finally { Monitor.Exit(cacheNameLock); }
            return new ReturnValue<valueType> { Type = ReturnType.DataStructureNotFoundConstructor };
        }
        /// <summary>
        /// 获取或者创建数据结构信息
        /// </summary>
        /// <param name="dataStructure"></param>
        /// <returns></returns>
        internal abstract AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.IndexIdentity> GetOrCreate(ClientDataStructure dataStructure);
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <returns></returns>
        public abstract ReturnValue<bool> RemoveDataStructure(string cacheName);
        /// <summary>
        /// 删除数据结构信息
        /// </summary>
        /// <param name="cacheName">缓存名称标识</param>
        /// <param name="onRemove"></param>
        public abstract void RemoveDataStructure(string cacheName, Action<ReturnValue<bool>> onRemove);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        internal abstract AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Query(DataStructure.Abstract.Node node);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal abstract void Query(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal abstract void QueryStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query(DataStructure.Abstract.Node node, Action<ReturnValue<int>> onGet)
        {
            Query(node, value => onGet(value.Value.GetInt(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream(DataStructure.Abstract.Node node, Action<ReturnValue<int>> onGet)
        {
            QueryStream(node, value => onGet(value.Value.GetInt(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onGet)
        {
            Query(node, value => onGet(value.Value.GetBool(value.Type)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onGet)
        {
            QueryStream(node, value => onGet(value.Value.GetBool(value.Type)));
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Query<nodeType>(DataStructure.Abstract.Node node, Action<ReturnValueNode<nodeType>> onGet)
            where nodeType : DataStructure.Abstract.Node, DataStructure.Abstract.IValue
        {
            Query(node, value => onGet(new ReturnValueNode<nodeType>(value)));
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void QueryStream<nodeType>(DataStructure.Abstract.Node node, Action<ReturnValueNode<nodeType>> onGet)
            where nodeType : DataStructure.Abstract.Node, DataStructure.Abstract.IValue
        {
            QueryStream(node, value => onGet(new ReturnValueNode<nodeType>(value)));
        }

        ///// <summary>
        ///// 查询数据
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="onGet"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void Query<valueType>(DataStructure.Abstract.Node node, Action<DataStructure.Abstract.ReturnValueNew<valueType>> onGet)
        //{
        //    Query(node, value => onGet(new DataStructure.Abstract.ReturnValueNew<valueType>(value)));
        //}
        ///// <summary>
        ///// 查询数据
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="onGet"></param>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal void QueryStream<valueType>(DataStructure.Abstract.Node node, Action<DataStructure.Abstract.ReturnValueNew<valueType>> onGet)
        //{
        //    QueryStream(node, value => onGet(new DataStructure.Abstract.ReturnValueNew<valueType>(value)));
        //}

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        internal abstract void OperationOnly(DataStructure.Abstract.Node node);
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal abstract AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> Operation(DataStructure.Abstract.Node node);
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal abstract void Operation(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn);
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        internal abstract void OperationStream(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn);

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationReturnParameter(DataStructure.Abstract.Node node, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onReturn)
        {
            if (onReturn != null) Operation(node, onReturn);
            else OperationOnly(node);
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Operation(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            if (onReturn != null) Operation(node, value => onReturn(value.Value.GetBool(value.Type)));
            else OperationOnly(node);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onReturn"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationStream(DataStructure.Abstract.Node node, Action<ReturnValue<bool>> onReturn)
        {
            OperationStream(node, value => onReturn(value.Value.GetBool(value.Type)));
        }

        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Operation<nodeType>(DataStructure.Abstract.Node node, Action<ReturnValueNode<nodeType>> onGet)
            where nodeType : DataStructure.Abstract.Node, DataStructure.Abstract.IValue
        {
            if (onGet != null) Operation(node, value => onGet(new ReturnValueNode<nodeType>(value)));
            else OperationOnly(node);
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="onGet"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void OperationStream<nodeType>(DataStructure.Abstract.Node node, Action<ReturnValueNode<nodeType>> onGet)
            where nodeType : DataStructure.Abstract.Node, DataStructure.Abstract.IValue
        {
            OperationStream(node, value => onGet(new ReturnValueNode<nodeType>(value)));
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<int> GetInt(AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> value)
        {
            return value.Value.GetInt(value.Type);
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<bool> GetBool(AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> value)
        {
            return value.Value.GetBool(value.Type);
        }
    }
}

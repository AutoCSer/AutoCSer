using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure
{
    /// <summary>
    /// 搜索树字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">数据节点类型</typeparam>
    public sealed partial class SearchTreeDictionary<keyType, nodeType> : Abstract.Dictionary<keyType, nodeType>
        where keyType : IEquatable<keyType>, IComparable<keyType>
        where nodeType : Abstract.Node
    {
        /// <summary>
        /// 获取元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public nodeType this[keyType key]
        {
            get
            {
                nodeType node = nodeConstructor(this);
                ValueData.Data<keyType>.SetData(ref node.Parameter, key);
                return node;
            }
        }
        /// <summary>
        /// 搜索树字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
#if !NOJIT
        [AutoCSer.IOS.Preserve(Conditional = true)]
#endif
        private SearchTreeDictionary(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.SearchTreeDictionary);
            stream.Write((byte)ValueData.Data<keyType>.DataType);
            serializeParentDataStructure(stream);
        }

        /// <summary>
        /// 获取操作元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        private Parameter.OperationBool getOperation(keyType key, OperationParameter.OperationType operationType)
        {
            Parameter.OperationBool node = new Parameter.OperationBool(this, operationType);
            ValueData.Data<keyType>.SetData(ref node.Parameter, key);
            return node;
        }

        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public Parameter.OperationBool GetOrCreateNode(keyType key)
        {
            return getOperation(key, OperationParameter.OperationType.GetOrCreateNode);
        }
        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AutoCSer.CacheServer.ReturnValue<nodeType> GetOrCreate(keyType key)
        {
            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.CacheServer.ReturnParameter> value = ClientDataStructure.Client.Operation(GetOrCreateNode(key));
            return value.Value.Parameter.GetBool(value.Type, value.Value.Parameter.Int64.Bool ? this[key] : null);
        }
        /// <summary>
        /// 获取或者创建元素节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onGet"></param>
        /// <returns></returns>
        public void GetOrCreate(keyType key, Action<AutoCSer.CacheServer.ReturnValue<nodeType>> onGet)
        {
            if (onGet != null)
            {
                ClientDataStructure.Client.Operation(GetOrCreateNode(key),
                    value => value.Value.Parameter.GetBool(value.Type, value.Value.Parameter.Int64.Bool ? this[key] : null));
            }
            else ClientDataStructure.Client.OperationOnly(GetOrCreateNode(key));
        }

        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public Parameter.OperationBool GetRemoveNode(keyType key)
        //{
        //    return getOperation(key, OperationParameter.OperationType.Remove);
        //}
        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public AutoCSer.CacheServer.ReturnValue<bool> Remove(keyType key)
        //{
        //    return Client.GetBool(ClientDataStructure.Client.Operation(GetRemoveNode(key)));
        //}
        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="onRemove"></param>
        ///// <returns></returns>
        //public void Remove(keyType key, Action<AutoCSer.CacheServer.ReturnValue<bool>> onRemove)
        //{
        //    ClientDataStructure.Client.Operation(GetRemoveNode(key), onRemove);
        //}
        ///// <summary>
        ///// 删除元素节点
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="onRemove">直接在 Socket 接收数据的 IO 线程中处理以避免线程调度，适应于快速结束的非阻塞函数；需要知道的是这种模式下如果产生阻塞会造成 Socket 停止接收数据甚至死锁</param>
        ///// <returns></returns>
        //public void RemoveStream(keyType key, Action<AutoCSer.CacheServer.ReturnValue<bool>> onRemove)
        //{
        //    ClientDataStructure.Client.OperationStream(GetRemoveNode(key), onRemove);
        //}

#if NOJIT
        /// <summary>
        /// 创建搜索树字典节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static SearchTreeDictionary<keyType, nodeType> create(Abstract.Node parent)
        {
            return new SearchTreeDictionary<keyType, nodeType>(parent);
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly Func<Abstract.Node, SearchTreeDictionary<keyType, nodeType>> constructor;
        static SearchTreeDictionary()
        {
            if (!ValueData.Data<keyType>.IsSortKey) throw new InvalidCastException("不支持排序关键字类型 " + typeof(keyType).fullName());
#if NOJIT
            constructor = (Func<Abstract.Node, SearchTreeDictionary<keyType, nodeType>>)Delegate.CreateDelegate(typeof(Func<Abstract.Node, SearchTreeDictionary<keyType, nodeType>>), typeof(SearchTreeDictionary<keyType, nodeType>).GetMethod(Cache.Node.CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null));
#else
            constructor = (Func<Abstract.Node, SearchTreeDictionary<keyType, nodeType>>)AutoCSer.Emit.Constructor.CreateDataStructure(typeof(SearchTreeDictionary<keyType, nodeType>), NodeConstructorParameterTypes);
#endif
        }
    }
}

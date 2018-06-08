using System;
using System.Threading;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数据结构定义节点
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// 构造函数字段名称
        /// </summary>
        internal const string ConstructorFieldName = "constructor";

        /// <summary>
        /// 父节点
        /// </summary>
        internal Node Parent;
        /// <summary>
        /// 参数数据
        /// </summary>
        internal ValueData.Data Parameter;
        /// <summary>
        /// 客户端数据结构定义信息
        /// </summary>
        internal ClientDataStructure ClientDataStructure
        {
            get { return Parent != null ? Parent.ClientDataStructure : new UnionType { Value = Parameter.Value }.ClientDataStructure; }
        }
        ///// <summary>
        ///// 操作类型
        ///// </summary>
        //public OperationParameter.OperationType OperationType
        //{
        //    get { return Parameter.OperationType; }
        //}
        /// <summary>
        /// 数据结构定义节点
        /// </summary>
        protected Node() { }
        /// <summary>
        /// 数据结构定义节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected Node(Node parent)
        {
            Parent = parent;
        }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal abstract Node CreateValueNode();
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal abstract void SerializeDataStructure(UnmanagedStream stream);
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void serializeParentDataStructure(UnmanagedStream stream)
        {
            if (Parent == null) stream.Write((byte)Abstract.NodeType.Unknown);
            else Parent.SerializeDataStructure(stream);
        }
        /// <summary>
        /// 序列化参数信息
        /// </summary>
        /// <param name="stream"></param>
        internal void SerializeParameter(UnmanagedStream stream)
        {
            if (Parent != null)
            {
                Parent.SerializeParameter(stream);
                //serializeParameter(stream);
                Parameter.Serialize(stream);
            }
            else ClientDataStructure.Identity.UnsafeSerialize(stream);
        }
        /// <summary>
        /// 尝试设置父节点
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool TrySetParent(Node parent)
        {
            Node oldParent = Interlocked.CompareExchange(ref Parent, parent, null);
            return oldParent == null || oldParent == parent;
        }

        /// <summary>
        /// 节点构造函数参数类型集合
        /// </summary>
        internal static readonly Type[] NodeConstructorParameterTypes = new Type[] { typeof(Node) };
        /// <summary>
        /// 构造函数集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, Delegate> constructors = new AutoCSer.Threading.LockDictionary<Type, Delegate>();
        /// <summary>
        /// 获取构造函数
        /// </summary>
        /// <param name="type">节点类型</param>
        /// <returns></returns>
        internal static Delegate GetConstructor(Type type)
        {
            Delegate constructor;
            if (!constructors.TryGetValue(type, out constructor)) constructors.Set(type, constructor = (Delegate)type.GetField(ConstructorFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null));
            return constructor;
        }
    }
}

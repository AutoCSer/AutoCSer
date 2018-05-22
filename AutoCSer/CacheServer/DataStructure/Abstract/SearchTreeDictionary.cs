using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 搜索树字典节点
    /// </summary>
    /// <typeparam name="keyType">关键字类型</typeparam>
    /// <typeparam name="nodeType">数据节点类型</typeparam>
    public abstract class SearchTreeDictionary<keyType, nodeType> : Dictionary<keyType>
        where keyType : IEquatable<keyType>, IComparable<keyType>
        where nodeType : Node
    {
        /// <summary>
        /// 搜索树字典节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected SearchTreeDictionary(Node parent) : base(parent) { }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return nodeConstructor(this).CreateValueNode();
        }

        /// <summary>
        /// 子节点构造函数
        /// </summary>
        protected static readonly Func<Node, nodeType> nodeConstructor;

        static SearchTreeDictionary()
        {
            if (!ValueData.Data<keyType>.IsSortKey) throw new InvalidCastException("不支持排序关键字类型 " + typeof(keyType).fullName());
            nodeConstructor = (Func<Node, nodeType>)typeof(nodeType).GetField(Cache.Node.ConstructorFieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(null);
        }
    }
}

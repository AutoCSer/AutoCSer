using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace AutoCSer.CacheServer.DataStructure.Abstract
{
    /// <summary>
    /// 数据节点
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public abstract class Value<valueType> : Node
    {
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="parent">父节点</param>
        protected Value(Abstract.Node parent) : base(parent) { }
        /// <summary>
        /// 数据节点
        /// </summary>
        /// <param name="value">数据</param>
        protected Value(valueType value)
        {
            ValueData.Data<valueType>.SetData(ref Parameter, value);
        }
        /// <summary>
        /// 创建数据节点
        /// </summary>
        /// <returns></returns>
        internal override Abstract.Node CreateValueNode()
        {
            return this;
        }
        /// <summary>
        /// 序列化数据结构定义信息
        /// </summary>
        /// <param name="stream"></param>
        internal override void SerializeDataStructure(UnmanagedStream stream)
        {
            stream.Write((byte)Abstract.NodeType.Value);
            stream.Write((byte)ValueData.Data<valueType>.DataType);
            serializeParentDataStructure(stream);
        }

        static Value()
        {
            if (ValueData.Data<valueType>.DataType == ValueData.DataType.Null) throw new InvalidCastException("不支持数据类型 " + typeof(valueType).fullName() + "，请考虑序列化类型如 Json<valueType> 或者 Binary<valueType>");
        }
    }
}

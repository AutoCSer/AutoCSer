using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 简单逻辑条件
    /// </summary>
    /// <typeparam name="valueType">运算数据类型</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct Logic<valueType>
    {
        /// <summary>
        /// 运算数据
        /// </summary>
        private valueType value;
        /// <summary>
        /// 更新条件逻辑类型
        /// </summary>
        private LogicType type;
        /// <summary>
        /// 简单逻辑条件
        /// </summary>
        /// <param name="type">更新条件逻辑类型</param>
        /// <param name="value">运算数据</param>
        internal Logic(LogicType type, valueType value)
        {
            this.type = type;
            this.value = value;
        }
        /// <summary>
        /// 获取更新数据节点
        /// </summary>
        /// <param name="node">数据参数节点</param>
        /// <param name="type">更新类型</param>
        /// <returns>更新数据节点</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal DataStructure.Parameter.Value GetNode(DataStructure.Parameter.Value node, OperationType type)
        {
            node = new DataStructure.Parameter.Value(new DataStructure.Parameter.Value(node, (uint)(ushort)type + ((uint)(byte)this.type << 16)), OperationParameter.OperationType.Update);
            ValueData.Data<valueType>.SetData(ref node.Parameter, value);
            return node;
        }

        /// <summary>
        /// 获取等于更新条件
        /// </summary>
        /// <param name="logic">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator ==(Logic<valueType> logic, valueType value)
        {
            return new Logic<valueType>(LogicType.Equal, value);
        }
        /// <summary>
        /// 获取不等于更新条件
        /// </summary>
        /// <param name="logic">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator !=(Logic<valueType> logic, valueType value)
        {
            return new Logic<valueType>(LogicType.NotEqual, value);
        }
        /// <summary>
        /// 获取大于等于更新条件
        /// </summary>
        /// <param name="logic">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator >=(Logic<valueType> logic, valueType value)
        {
            return new Logic<valueType>(LogicType.MoreOrEqual, value);
        }
        /// <summary>
        /// 获取大于更新条件
        /// </summary>
        /// <param name="logic">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator >(Logic<valueType> logic, valueType value)
        {
            return new Logic<valueType>(LogicType.More, value);
        }
        /// <summary>
        /// 获取小于等于更新条件
        /// </summary>
        /// <param name="logic">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator <=(Logic<valueType> logic, valueType value)
        {
            return new Logic<valueType>(LogicType.LessOrEqual, value);
        }
        /// <summary>
        /// 获取小于更新条件
        /// </summary>
        /// <param name="logic">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator <(Logic<valueType> logic, valueType value)
        {
            return new Logic<valueType>(LogicType.Less, value);
        }
        /// <summary>
        /// 仅用于消除 == 警告
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            Logic<valueType> value = ((Logic<valueType>)other);
            return type == value.type && this.value.Equals(value.value);
        }
        /// <summary>
        /// 仅用于消除 == 警告
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value.GetHashCode() ^ (int)(byte)type;
        }
        /// <summary>
        /// 默认简单逻辑条件，仅用于重载运算符产生目标逻辑条件
        /// </summary>
        public static Logic<valueType> Default = default(Logic<valueType>);
    }
}

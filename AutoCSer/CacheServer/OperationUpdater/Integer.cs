using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 整数更新
    /// </summary>
    /// <typeparam name="valueType">整数类型</typeparam>
    public partial struct Integer<valueType>
    {
        /// <summary>
        /// 数据参数节点
        /// </summary>
        private DataStructure.Parameter.Value node;
        /// <summary>
        /// 数字更新
        /// </summary>
        public Number<valueType> Number
        {
            get { return new Number<valueType>(node); }
        }
        /// <summary>
        /// 整数更新
        /// </summary>
        /// <param name="node">数据参数节点</param>
        internal Integer(DataStructure.Parameter.Value node)
        {
            this.node = node;
        }

        /// <summary>
        /// 获取等于更新条件
        /// </summary>
        /// <param name="integer">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator ==(Integer<valueType> integer, valueType value)
        {
            return new Logic<valueType>(LogicType.Equal, value);
        }
        /// <summary>
        /// 获取不等于更新条件
        /// </summary>
        /// <param name="integer">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator !=(Integer<valueType> integer, valueType value)
        {
            return new Logic<valueType>(LogicType.NotEqual, value);
        }
        /// <summary>
        /// 获取大于等于更新条件
        /// </summary>
        /// <param name="integer">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator >=(Integer<valueType> integer, valueType value)
        {
            return new Logic<valueType>(LogicType.MoreOrEqual, value);
        }
        /// <summary>
        /// 获取大于更新条件
        /// </summary>
        /// <param name="integer">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator >(Integer<valueType> integer, valueType value)
        {
            return new Logic<valueType>(LogicType.More, value);
        }
        /// <summary>
        /// 获取小于等于更新条件
        /// </summary>
        /// <param name="integer">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator <=(Integer<valueType> integer, valueType value)
        {
            return new Logic<valueType>(LogicType.LessOrEqual, value);
        }
        /// <summary>
        /// 获取小于更新条件
        /// </summary>
        /// <param name="integer">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator <(Integer<valueType> integer, valueType value)
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
            return node.Equals(((Integer<valueType>)other).node);
        }
        /// <summary>
        /// 仅用于消除 == 警告
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return node.GetHashCode();
        }

        /// <summary>
        /// 获取更新数据节点
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private DataStructure.Parameter.Value getNode(valueType value, OperationType type)
        {
            DataStructure.Parameter.Value node = new DataStructure.Parameter.Value(this.node);
            ValueData.Data<valueType>.SetData(ref node.Parameter, value);
            node = new DataStructure.Parameter.Value(node, OperationParameter.OperationType.Update);
            node.Parameter.Set((uint)(ushort)type);
            return node;
        }
        /// <summary>
        /// 获取更新数据节点
        /// </summary>
        /// <param name="value"></param>
        /// <param name="logic"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private DataStructure.Parameter.Value getNode(valueType value, Logic<valueType> logic, OperationType type)
        {
            DataStructure.Parameter.Value node = new DataStructure.Parameter.Value(this.node);
            ValueData.Data<valueType>.SetData(ref node.Parameter, value);
            return logic.GetNode(node, type);
        }

        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="integer">整数更新</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator ^(Integer<valueType> integer, valueType value)
        {
            return integer.Xor(value);
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Xor(valueType value)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Xor)));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Xor(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Xor), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Xor));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Xor(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Xor), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Xor));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void XorStream(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Xor), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void XorStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Xor), onUpdated);
        }

        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Xor(valueType value, Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Xor)));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Xor(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Xor), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Xor));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Xor(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Xor), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Xor));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void XorStream(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Xor), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 逻辑异或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void XorStream(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Xor), onUpdated);
        }

        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="integer">整数更新</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator &(Integer<valueType> integer, valueType value)
        {
            return integer.And(value);
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> And(valueType value)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, OperationType.And)));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void And(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.And), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.And));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void And(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.And), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.And));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void AndStream(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.And), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void AndStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.And), onUpdated);
        }

        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> And(valueType value, Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.And)));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void And(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.And), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.And));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void And(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.And), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.And));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void AndStream(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.And), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 逻辑与运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void AndStream(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.And), onUpdated);
        }

        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="integer">整数更新</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator |(Integer<valueType> integer, valueType value)
        {
            return integer.Or(value);
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Or(valueType value)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Or)));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Or(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Or), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Or));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Or(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Or), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Or));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void OrStream(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Or), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void OrStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Or), onUpdated);
        }

        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Or(valueType value, Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Or)));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Or(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Or), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Or));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Or(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Or), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Or));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void OrStream(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Or), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 逻辑或运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void OrStream(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Or), onUpdated);
        }

        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="integer">整数更新</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator !(Integer<valueType> integer)
        {
            return integer.Not();
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Not()
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(default(valueType), OperationType.Not)));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Not(Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(default(valueType), OperationType.Not), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(default(valueType), OperationType.Not));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Not(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(default(valueType), OperationType.Not), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(default(valueType), OperationType.Not));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="onUpdated">计算结果</param>
        public void NotStream(Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(default(valueType), OperationType.Not), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="onUpdated">计算结果</param>
        public void NotStream(Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(default(valueType), OperationType.Not), onUpdated);
        }

        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Not(Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(default(valueType), logic, OperationType.Not)));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Not(Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(default(valueType), logic, OperationType.Not), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(default(valueType), logic, OperationType.Not));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Not(Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(default(valueType), logic, OperationType.Not), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(default(valueType), logic, OperationType.Not));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void NotStream(Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(default(valueType), logic, OperationType.Not), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 逻辑取反运算
        /// </summary>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void NotStream(Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(default(valueType), logic, OperationType.Not), onUpdated);
        }

        /// <summary>
        /// 获取返回值数据委托
        /// </summary>
        internal static readonly Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>> GetValue;
        static Integer()
        {
            if (!ValueData.Data<valueType>.IsInteger) throw new InvalidCastException("不支持整数类型 " + typeof(valueType).fullName());
            GetValue = Number<valueType>.GetValue;
        }
    }
}

using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 数字更新
    /// </summary>
    /// <typeparam name="valueType">数字类型</typeparam>
    public partial struct Number<valueType>
    {
        /// <summary>
        /// 数据参数节点
        /// </summary>
        private DataStructure.Parameter.Value node;
        /// <summary>
        /// 数字更新
        /// </summary>
        /// <param name="node">数据参数节点</param>
        internal Number(DataStructure.Parameter.Value node)
        {
            this.node = node;
        }

        /// <summary>
        /// 获取等于更新条件
        /// </summary>
        /// <param name="number">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator ==(Number<valueType> number, valueType value)
        {
            return new Logic<valueType>(LogicType.Equal, value);
        }
        /// <summary>
        /// 获取不等于更新条件
        /// </summary>
        /// <param name="number">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator !=(Number<valueType> number, valueType value)
        {
            return new Logic<valueType>(LogicType.NotEqual, value);
        }
        /// <summary>
        /// 获取大于等于更新条件
        /// </summary>
        /// <param name="number">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator >=(Number<valueType> number, valueType value)
        {
            return new Logic<valueType>(LogicType.MoreOrEqual, value);
        }
        /// <summary>
        /// 获取大于更新条件
        /// </summary>
        /// <param name="number">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator >(Number<valueType> number, valueType value)
        {
            return new Logic<valueType>(LogicType.More, value);
        }
        /// <summary>
        /// 获取小于等于更新条件
        /// </summary>
        /// <param name="number">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator <=(Number<valueType> number, valueType value)
        {
            return new Logic<valueType>(LogicType.LessOrEqual, value);
        }
        /// <summary>
        /// 获取小于更新条件
        /// </summary>
        /// <param name="number">无意义</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Logic<valueType> operator <(Number<valueType> number, valueType value)
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
            return node.Equals(((Number<valueType>)other).node);
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
        /// 加法运算
        /// </summary>
        /// <param name="number">数字更新</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator +(Number<valueType> number, valueType value)
        {
            return number.Add(value);
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Add(valueType value)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Add)));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Add), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Add));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Add), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Add));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void AddStream(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Add), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void AddStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Add), onUpdated);
        }

        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Add(valueType value, Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Add)));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Add), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Add));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Add(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Add), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Add));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void AddStream(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Add), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 加法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void AddStream(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Add), onUpdated);
        }

        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="number">数字更新</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator -(Number<valueType> number, valueType value)
        {
            return number.Sub(value);
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Sub(valueType value)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Sub)));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Sub(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Sub), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Sub));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Sub(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Sub), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Sub));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void SubStream(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Sub), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void SubStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Sub), onUpdated);
        }

        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Sub(valueType value, Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Sub)));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Sub(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Sub), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Sub));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Sub(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Sub), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Sub));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void SubStream(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Sub), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 减法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void SubStream(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Sub), onUpdated);
        }

        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="number">数字更新</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator *(Number<valueType> number, valueType value)
        {
            return number.Mul(value);
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Mul(valueType value)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Mul)));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Mul(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Mul), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Mul));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Mul(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Mul), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Mul));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void MulStream(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Mul), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void MulStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Mul), onUpdated);
        }

        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Mul(valueType value, Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Mul)));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Mul(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Mul), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Mul));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Mul(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Mul), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Mul));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void MulStream(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Mul), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 乘法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void MulStream(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Mul), onUpdated);
        }

        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="number">数字更新</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator /(Number<valueType> number, valueType value)
        {
            return number.Div(value);
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Div(valueType value)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Div)));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Div(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Div), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Div));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Div(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Div), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Div));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void DivStream(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Div), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void DivStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Div), onUpdated);
        }

        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Div(valueType value, Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Div)));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Div(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Div), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Div));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Div(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Div), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Div));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void DivStream(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Div), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 除法运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void DivStream(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Div), onUpdated);
        }

        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="number">数字更新</param>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> operator %(Number<valueType> number, valueType value)
        {
            return number.Mod(value);
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Mod(valueType value)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Mod)));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Mod(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Mod), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Mod));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Mod(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, OperationType.Mod), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, OperationType.Mod));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void ModStream(valueType value, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Mod), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="onUpdated">计算结果</param>
        public void ModStream(valueType value, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, OperationType.Mod), onUpdated);
        }

        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <returns>计算结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<valueType> Mod(valueType value, Logic<valueType> logic)
        {
            return GetValue(node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Mod)));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Mod(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Mod), returnValue => onUpdated(GetValue(returnValue)));
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Mod));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Mod(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated != null) node.ClientDataStructure.Client.Operation(getNode(value, logic, OperationType.Mod), onUpdated);
            else node.ClientDataStructure.Client.OperationOnly(getNode(value, logic, OperationType.Mod));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void ModStream(valueType value, Logic<valueType> logic, Action<ReturnValue<valueType>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Mod), returnValue => onUpdated(GetValue(returnValue)));
        }
        /// <summary>
        /// 取余运算
        /// </summary>
        /// <param name="value">运算数据</param>
        /// <param name="logic">简单逻辑条件</param>
        /// <param name="onUpdated">计算结果</param>
        public void ModStream(valueType value, Logic<valueType> logic, Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> onUpdated)
        {
            if (onUpdated == null) throw new ArgumentNullException();
            node.ClientDataStructure.Client.OperationStream(getNode(value, logic, OperationType.Mod), onUpdated);
        }

        /// <summary>
        /// 获取返回值数据委托
        /// </summary>
        internal static readonly Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>> GetValue;

        static Number()
        {
            if (!ValueData.Data<valueType>.IsNumber) throw new InvalidCastException("不支持数字类型 " + typeof(valueType).fullName());

            Type type = typeof(valueType);
            if (type == typeof(int))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<int>>)Client.GetInt;
                return;
            }
            if (type == typeof(long))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<long>>)Client.GetLong;
                return;
            }
            if (type == typeof(ulong))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<ulong>>)Client.GetULong;
                return;
            }
            if (type == typeof(uint))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<uint>>)Client.GetUInt;
                return;
            }
            if (type == typeof(short))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<short>>)Client.GetShort;
                return;
            }
            if (type == typeof(ushort))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<ushort>>)Client.GetUShort;
                return;
            }
            if (type == typeof(byte))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<byte>>)Client.GetByte;
                return;
            }
            if (type == typeof(sbyte))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<sbyte>>)Client.GetSByte;
                return;
            }
            if (type == typeof(float))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<float>>)Client.GetFloat;
                return;
            }
            if (type == typeof(double))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<double>>)Client.GetDouble;
                return;
            }
            if (type == typeof(decimal))
            {
                GetValue = (Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<valueType>>)(object)(Func<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>, ReturnValue<decimal>>)Client.GetDecimal;
                return;
            }
        }
    }
}

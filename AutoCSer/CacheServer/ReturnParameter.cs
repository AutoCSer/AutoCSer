using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 返回值参数
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    public struct ReturnParameter
    {
        /// <summary>
        /// 返回值参数
        /// </summary>
        internal ValueData.Data Parameter;
        /// <summary>
        /// 返回值类型
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        internal ReturnParameter(ReturnType returnType)
        {
            //Parameter = new ValueData.Data(returnType);
            Parameter = default(ValueData.Data);
            Parameter.ReturnType = returnType;
        }
        /// <summary>
        /// 返回值类型
        /// </summary>
        /// <param name="parameter">返回值参数</param>
        internal ReturnParameter(ValueData.Data parameter)
        {
            Parameter = parameter;
        }
        /// <summary>
        /// 返回值类型
        /// </summary>
        /// <param name="parameter">返回值参数</param>
        internal ReturnParameter(ref ValueData.Data parameter)
        {
            Parameter = parameter;
        }
        /// <summary>
        /// 返回值类型
        /// </summary>
        /// <param name="parameter">返回值参数</param>
        /// <param name="isDeSerializeStream">是否反序列化网络流，否则需要 Copy 数据</param>
        internal ReturnParameter(ValueData.Data parameter, bool isDeSerializeStream)
        {
            parameter.IsReturnDeSerializeStream = isDeSerializeStream;
            Parameter = parameter;
        }
        /// <summary>
        /// 设置返回值参数
        /// </summary>
        /// <param name="parameter">返回值参数</param>
        /// <param name="isDeSerializeStream">是否反序列化网络流，否则需要 Copy 数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ValueData.Data parameter, bool isDeSerializeStream)
        {
            parameter.IsReturnDeSerializeStream = isDeSerializeStream;
            Parameter = parameter;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerialize.Serializer serializer)
        {
            Parameter.SerializeReturnParameter(serializer.Stream);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.BinarySerialize.SerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void deSerialize(AutoCSer.BinarySerialize.DeSerializer deSerializer)
        {
            Parameter.DeSerializeReturnParameter(deSerializer);
        }

        /// <summary>
        /// 获取回调委托包装
        /// </summary>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        public static Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> GetCallback(Action<ReturnValue<bool>> onReturn)
        {
            if (onReturn != null) return value => onReturn(value.Value.Parameter.GetBool(value.Type));
            return null;
        }
        /// <summary>
        /// 获取回调委托包装
        /// </summary>
        /// <param name="onReturn"></param>
        /// <returns></returns>
        public static Action<AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter>> GetCallback<valueType>(Action<ReturnValue<valueType>> onReturn)
        {
            if (onReturn != null) return value => onReturn(new ReturnValue<valueType>(ref value));
            return null;
        }
    }
}

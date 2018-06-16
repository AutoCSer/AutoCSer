using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 附带标识的返回值参数
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    public struct IdentityReturnParameter
    {
        /// <summary>
        /// 消息标识
        /// </summary>
        internal ulong Identity;
        /// <summary>
        /// 返回值参数
        /// </summary>
        internal ValueData.Data Parameter;
        /// <summary>
        /// 附带标识的返回值参数
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="parameter"></param>
        internal IdentityReturnParameter(ulong identity, ref ValueData.Data parameter)
        {
            Identity = identity;
            Parameter = parameter;
        }
        /// <summary>
        /// 返回值类型
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        internal IdentityReturnParameter(ReturnType returnType)
        {
            Parameter = default(ValueData.Data);
            Identity = 0;
            Parameter.ReturnType = returnType;
        }
        /// <summary>
        /// 返回值类型
        /// </summary>
        /// <param name="parameter">返回值参数</param>
        internal IdentityReturnParameter(ref ValueData.Data parameter)
        {
            Identity = 0;
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
            UnmanagedStream stream = serializer.Stream;
            stream.Write(Identity);
            Parameter.SerializeReturnParameter(stream);
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
            Identity = deSerializer.ReadULong();
            Parameter.DeSerializeReturnParameter(deSerializer);
        }
    }
}

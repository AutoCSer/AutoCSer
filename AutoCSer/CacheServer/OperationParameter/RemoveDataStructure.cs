﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.OperationParameter
{
    /// <summary>
    /// 删除数据结构操作参数
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct RemoveDataStructure
    {
        /// <summary>
        /// 缓存名称标识
        /// </summary>
        internal string CacheName;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal Buffer Buffer;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void serialize(AutoCSer.BinarySerializer serializer)
        {
            Serializer operationSerializer = new Serializer(serializer.Stream);
            operationSerializer.Stream.Data.CurrentIndex += IndexIdentity.SerializeSize;
            fixed (char* nameFixed = CacheName) AutoCSer.BinarySerializer.Serialize(nameFixed, operationSerializer.Stream, CacheName.Length);
            operationSerializer.End(OperationType.RemoveDataStructure);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deSerializer"></param>
        [AutoCSer.BinarySerializeCustom]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private unsafe void deSerialize(AutoCSer.BinaryDeSerializer deSerializer)
        {
            Buffer = Serializer.GetOperationData(deSerializer);
        }
    }
}

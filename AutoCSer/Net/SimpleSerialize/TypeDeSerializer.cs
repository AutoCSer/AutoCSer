using System;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 简单反序列化
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    internal unsafe static partial class TypeDeSerializer<valueType> where valueType : struct
    {
#if NOJIT
        /// <summary>
        /// 固定分组填充字节数
        /// </summary>
        private static readonly int fixedFillSize;
        /// <summary>
        /// 固定分组成员反序列化
        /// </summary>
        private static readonly Action<DeSerializer, object> fixedDeSerializer;
        /// <summary>
        /// 成员反序列化
        /// </summary>
        private static readonly Action<DeSerializer, object> deSerializer;
        /// <summary>
        /// 简单反序列化委托
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value">目标数据</param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal static byte* DeSerialize(byte* start, ref valueType value, byte* end)
        {
            DeSerializer pointer = new DeSerializer { Read = start, End = end };
            object objectValue = value;
            fixedDeSerializer(pointer, objectValue);
            pointer.Read += fixedFillSize;
            if (deSerializer != null) deSerializer(pointer, objectValue);
            value = (valueType)objectValue;
            return pointer.Read;
        }
#else
        /// <summary>
        /// 简单反序列化委托
        /// </summary>
        /// <param name="start"></param>
        /// <param name="value">目标数据</param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal delegate byte* SimpleDeSerializer(byte* start, ref valueType value, byte* end);
        /// <summary>
        /// 简单反序列化委托
        /// </summary>
        internal static readonly SimpleDeSerializer DeSerialize;
#endif
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Compile() { }

        static TypeDeSerializer()
        {
            Type type = typeof(valueType);
            int memberCountVerify;
            BinarySerialize.Fields<BinarySerialize.FieldSize> fields = BinarySerialize.SerializeMethodCache.GetFields(MemberIndexGroup<valueType>.GetFields(MemberFilters.PublicInstanceField), false, out memberCountVerify);
#if NOJIT
            fixedFillSize = -fields.FixedSize & 3;
            fixedDeSerializer = new FieldDeSerializer(ref fields.FixedFields).DeSerialize;
            if (fields.FieldArray.Length != 0) deSerializer = new FieldDeSerializer(ref fields.FieldArray).DeSerialize;
#else
            DeSerializeDynamicMethod dynamicMethod = new DeSerializeDynamicMethod(type);
            foreach (BinarySerialize.FieldSize member in fields.FixedFields) dynamicMethod.Push(member);
            dynamicMethod.FixedFill(-fields.FixedSize & 3);
            foreach (BinarySerialize.FieldSize member in fields.FieldArray) dynamicMethod.Push(member);
            DeSerialize = (SimpleDeSerializer)dynamicMethod.Create<SimpleDeSerializer>();
#endif
        }
    }
}
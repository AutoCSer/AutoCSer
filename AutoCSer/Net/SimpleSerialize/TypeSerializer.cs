using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.SimpleSerialize
{
    /// <summary>
    /// 简单序列化
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    internal unsafe static partial class TypeSerializer<valueType> where valueType : struct
    {
#if NOJIT
        /// <summary>
        /// 固定分组字节数
        /// </summary>
        private static readonly int fixedSize;
        /// <summary>
        /// 固定分组填充字节数
        /// </summary>
        private static readonly int fixedFillSize;
        /// <summary>
        /// 固定分组成员序列化
        /// </summary>
        private static readonly Action<UnmanagedStream, object> fixedSerializer;
        /// <summary>
        /// 成员序列化
        /// </summary>
        private static readonly Action<UnmanagedStream, object> serializer;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Serializer(UnmanagedStream stream, ref valueType value)
        {
            object objectValue = value;
            stream.PrepLength(fixedSize);
            fixedSerializer(stream, objectValue);
            stream.ByteSize += fixedFillSize;
            if (serializer != null) serializer(stream, objectValue);
        }
#else
        /// <summary>
        /// 简单序列化委托
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value">目标数据</param>
        internal delegate void SimpleSerializer(UnmanagedStream stream, ref valueType value);
        /// <summary>
        /// 成员序列化
        /// </summary>
        internal static readonly SimpleSerializer Serializer;
#endif
        /// <summary>
        /// 预编译
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Compile() { }

        static TypeSerializer()
        {
            Type type = typeof(valueType);
            int memberCountVerify;
            BinarySerialize.Fields<BinarySerialize.FieldSize> fields = BinarySerialize.SerializeMethodCache.GetFields(MemberIndexGroup<valueType>.GetFields(MemberFilters.PublicInstanceField), false, out memberCountVerify);
#if NOJIT
            fixedFillSize = -fields.FixedSize & 3;
            fixedSize = (fields.FixedSize + fields.FieldArray.Length * sizeof(int) + 3) & (int.MaxValue - 3);
            fixedSerializer = new FieldFerializer(ref fields.FixedFields).Serialize;
            if (fields.FieldArray.Length != 0) serializer = new FieldFerializer(ref fields.FieldArray).Serialize;
#else
            SerializeDynamicMethod dynamicMethod = new SerializeDynamicMethod(type, (fields.FixedSize + fields.FieldArray.Length * sizeof(int) + 3) & (int.MaxValue - 3));
            foreach (BinarySerialize.FieldSize member in fields.FixedFields) dynamicMethod.Push(member);
            dynamicMethod.FixedFill(-fields.FixedSize & 3);
            foreach (BinarySerialize.FieldSize member in fields.FieldArray) dynamicMethod.Push(member);
            Serializer = (SimpleSerializer)dynamicMethod.Create<SimpleSerializer>();
#endif
        }
    }
}

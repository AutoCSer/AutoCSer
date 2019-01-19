using System;
using System.Reflection;
using AutoCSer.Metadata;
using System.Collections.Generic;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 字段信息
    /// </summary>
    internal unsafe class FieldSize
    {
        /// <summary>
        /// 字段信息
        /// </summary>
        public FieldInfo Field;
        /// <summary>
        /// 成员索引
        /// </summary>
        public int MemberIndex;
        /// <summary>
        /// 固定分组排序字节数
        /// </summary>
        internal byte FixedSize;
        /// <summary>
        /// 字段信息
        /// </summary>
        /// <param name="field"></param>
        internal FieldSize(FieldIndex field)
        {
            Field = field.Member;
            MemberIndex = field.MemberIndex;
            if (Field.FieldType.IsEnum) fixedSizes.TryGetValue(System.Enum.GetUnderlyingType(Field.FieldType), out FixedSize);
            else fixedSizes.TryGetValue(Field.FieldType, out FixedSize);
        }
        /// <summary>
        /// 固定分组排序字节数排序比较
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int FixedSizeSort(FieldSize left, FieldSize right)
        {
            return (int)((uint)right.FixedSize & (0U - (uint)right.FixedSize)) - (int)((uint)left.FixedSize & (0U - (uint)left.FixedSize));
        }

        /// <summary>
        /// 固定类型字节数
        /// </summary>
        private static readonly Dictionary<Type, byte> fixedSizes;

        static FieldSize()
        {
            fixedSizes = DictionaryCreator.CreateOnly<Type, byte>();
            fixedSizes.Add(typeof(bool), sizeof(bool));
            fixedSizes.Add(typeof(byte), sizeof(byte));
            fixedSizes.Add(typeof(sbyte), sizeof(sbyte));
            fixedSizes.Add(typeof(short), sizeof(short));
            fixedSizes.Add(typeof(ushort), sizeof(ushort));
            fixedSizes.Add(typeof(int), sizeof(int));
            fixedSizes.Add(typeof(uint), sizeof(uint));
            fixedSizes.Add(typeof(long), sizeof(long));
            fixedSizes.Add(typeof(ulong), sizeof(ulong));
            fixedSizes.Add(typeof(char), sizeof(char));
            fixedSizes.Add(typeof(DateTime), sizeof(long));
            fixedSizes.Add(typeof(float), sizeof(float));
            fixedSizes.Add(typeof(double), sizeof(double));
            fixedSizes.Add(typeof(decimal), sizeof(decimal));
            fixedSizes.Add(typeof(Guid), (byte)sizeof(Guid));
            fixedSizes.Add(typeof(bool?), sizeof(byte));
            fixedSizes.Add(typeof(byte?), sizeof(ushort));
            fixedSizes.Add(typeof(sbyte?), sizeof(ushort));
            fixedSizes.Add(typeof(short?), sizeof(uint));
            fixedSizes.Add(typeof(ushort?), sizeof(uint));
            fixedSizes.Add(typeof(int?), sizeof(int) + sizeof(int));
            fixedSizes.Add(typeof(uint?), sizeof(uint) + sizeof(int));
            fixedSizes.Add(typeof(long?), sizeof(long) + sizeof(int));
            fixedSizes.Add(typeof(ulong?), sizeof(ulong) + sizeof(int));
            fixedSizes.Add(typeof(char?), sizeof(uint));
            fixedSizes.Add(typeof(DateTime?), sizeof(long) + sizeof(int));
            fixedSizes.Add(typeof(float?), sizeof(float) + sizeof(int));
            fixedSizes.Add(typeof(double?), sizeof(double) + sizeof(int));
            fixedSizes.Add(typeof(decimal?), sizeof(decimal) + sizeof(int));
            fixedSizes.Add(typeof(Guid?), (byte)(sizeof(Guid) + sizeof(int)));
        }
    }
}

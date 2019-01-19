using System;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 字段集合信息
    /// </summary>
    /// <typeparam name="fieldType"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Fields<fieldType> where fieldType : FieldSize
    {
        /// <summary>
        /// 固定序列化字段
        /// </summary>
        public LeftArray<fieldType> FixedFields;
        /// <summary>
        /// 非固定序列化字段
        /// </summary>
        public LeftArray<fieldType> FieldArray;
        /// <summary>
        /// JSON 混合序列化字段
        /// </summary>
        public LeftArray<FieldIndex> JsonFields;
        /// <summary>
        /// 固定序列化字段字节数
        /// </summary>
        public int FixedSize;
        /// <summary>
        /// 字段集合信息
        /// </summary>
        /// <param name="fixedFields">固定序列化字段</param>
        /// <param name="fields">非固定序列化字段</param>
        /// <param name="jsonFields">JSON 混合序列化字段</param>
        /// <param name="fixedSize">固定序列化字段字节数</param>
        /// <param name="isJson"></param>
        /// <param name="memberCountVerify">序列化成员数量</param>
        internal Fields(ref LeftArray<fieldType> fixedFields, ref LeftArray<fieldType> fields, ref LeftArray<FieldIndex> jsonFields, int fixedSize, bool isJson, out int memberCountVerify)
        {
            memberCountVerify = fixedFields.Length + fields.Length + 0x40000000;
            if (isJson || jsonFields.Length != 0) memberCountVerify |= 0x20000000;

            FixedFields = fixedFields.Sort(FieldSize.FixedSizeSort);
            FieldArray = fields;
            JsonFields = jsonFields;
            FixedSize = fixedSize;
        }
    }
}

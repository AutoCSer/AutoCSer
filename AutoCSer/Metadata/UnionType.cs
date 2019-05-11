using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal partial struct UnionType
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public GenericType GenericType;
        /// <summary>
        /// 结构体泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public StructGenericType StructGenericType;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public GenericType2 GenericType2;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public DictionaryGenericType3 DictionaryGenericType3;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public EnumerableGenericType2 EnumerableGenericType2;
    }
}

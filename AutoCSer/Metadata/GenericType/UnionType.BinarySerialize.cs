using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 类型转换
    /// </summary>
    internal partial struct UnionType
    {
        /// <summary>
        /// 引用泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public ClassGenericType ClassGenericType;
        /// <summary>
        /// 泛型类型元数据
        /// </summary>
        [FieldOffset(0)]
        public CollectionGenericType2 CollectionGenericType2;
    }
}

using System;

namespace fastCSharp
{
    /// <summary>
    /// 静态哈希基类
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    public abstract class StaticHash<valueType> : StaticHashIndex
    {
        /// <summary>
        /// 哈希数据数组
        /// </summary>
        protected valueType[] array;
        /// <summary>
        /// 是否空集合
        /// </summary>
        /// <returns>是否空集合</returns>
        public unsafe bool IsEmpty()
        {
            byte* indexFixed = indexs.Byte;
            for (Range* index = (Range*)(indexFixed + indexs.ByteSize); index != indexFixed; )
            {
                --index;
                if ((*index).StartIndex != (*index).EndIndex) return false;
            }
            return true;
        }
    }
}

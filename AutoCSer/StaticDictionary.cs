using System;
using fastCSharp.Extension;

namespace fastCSharp
{
    /// <summary>
    /// 静态哈希字典
    /// </summary>
    /// <typeparam name="keyType">键值类型</typeparam>
    /// <typeparam name="valueType">数据类型</typeparam>
    public sealed class StaticDictionary<keyType, valueType> : StaticHash<KeyValue<keyType, valueType>> where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 静态哈希字典
        /// </summary>
        /// <param name="values">初始化键值对集合</param>
        public unsafe StaticDictionary(KeyValue<keyType, valueType>[] values)
        {
            if (values.isEmpty())
            {
                indexs = defaultIndexs;
                array = NullValue<KeyValue<keyType, valueType>>.Array;
            }
            else
            {
                int length = ((values.Length + 1) >> 1) * (sizeof(int) * 2);
                UnmanagedPool pool = fastCSharp.UnmanagedPool.GetDefaultPool(length);
                Pointer.Size data = pool.GetSize64(length);
                try
                {
                    getValues(values, data.Int);
                }
                finally { pool.PushOnly(ref data); }
            }
        }
        /// <summary>
        /// 获取哈希数据数组
        /// </summary>
        /// <param name="values">数据集合</param>
        /// <param name="hashs">哈希集合</param>
        private unsafe void getValues(KeyValue<keyType, valueType>[] values, int* hashs)
        {
            int* hash = hashs;
            foreach (KeyValue<keyType, valueType> value in values) *hash++ = getHashCode(value.Key);
            array = base.getValues(values, hashs);
        }
        /// <summary>
        /// 获取匹配数据
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <param name="nullValue">默认空值</param>
        /// <returns>匹配数据,失败返回默认空值</returns>
        [System.Runtime.CompilerServices.MethodImpl((System.Runtime.CompilerServices.MethodImplOptions)fastCSharp.Pub.MethodImplOptionsAggressiveInlining)]
        public valueType Get(keyType key, valueType nullValue)
        {
            int index = indexOf(key);
            return index != -1 ? array[index].Value : nullValue;
        }
        /// <summary>
        /// 获取键值匹配数组位置
        /// </summary>
        /// <param name="key">哈希键值</param>
        /// <returns>数组位置</returns>
        private unsafe int indexOf(keyType key)
        {
            for (Range range = ((Range*)indexs.Data)[getHashCode(key) & indexAnd]; range.StartIndex != range.EndIndex; ++range.StartIndex)
            {
                if (array[range.StartIndex].Key.Equals(key)) return range.StartIndex;
            }
            return -1;
        }
    }
}

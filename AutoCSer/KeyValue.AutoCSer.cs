using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 键值对
    /// </summary>
    public partial struct KeyValue<keyType, valueType>
    {
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(ref keyType key, valueType value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Null()
        {
            Key = default(keyType);
            Value = default(valueType);
        }
    }
}

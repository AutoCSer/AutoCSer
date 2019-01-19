using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 键值对
    /// </summary>
    /// <typeparam name="keyType">键类型</typeparam>
    /// <typeparam name="valueType">值类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct KeyValue<keyType, valueType>
    {
        /// <summary>
        /// 键
        /// </summary>
        public keyType Key;
        /// <summary>
        /// 值
        /// </summary>
        public valueType Value;
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(keyType key, valueType value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(ref keyType key, ref valueType value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 重置键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(keyType key, valueType value)
        {
            Key = key;
            Value = value;
        }
    }
}

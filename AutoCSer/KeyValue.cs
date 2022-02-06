using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 键值对
    /// </summary>
    /// <typeparam name="KT">键类型</typeparam>
    /// <typeparam name="VT">值类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct KeyValue<KT, VT>
    {
        /// <summary>
        /// 键
        /// </summary>
        public KT Key;
        /// <summary>
        /// 值
        /// </summary>
        public VT Value;
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(KT key, VT value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(ref KT key, VT value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// 键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public KeyValue(ref KT key, ref VT value)
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
        public void Set(KT key, VT value)
        {
            Key = key;
            Value = value;
        }
    }
}

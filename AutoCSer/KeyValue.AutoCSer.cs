using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 键值对
    /// </summary>
    public partial struct KeyValue<KT, VT>
    {
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Null()
        {
            Key = default(KT);
            Value = default(VT);
        }
    }
}

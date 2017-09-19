using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 取消操作数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct AnyCancel
    {
        /// <summary>
        /// 是否取消操作
        /// </summary>
        internal bool IsCancelValue;
        /// <summary>
        /// 是否取消操作
        /// </summary>
        public bool IsCancel
        {
            get { return IsCancelValue; }
        }
        /// <summary>
        /// 取消操作
        /// </summary>
        [MethodImpl(MethodImpl.AggressiveInlining)]
        public void Cancel()
        {
            IsCancelValue = true;
        }
    }
}

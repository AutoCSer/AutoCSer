using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 视图错误重定向路径
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ViewLocationPath
    {
        /// <summary>
        /// 错误重定向路径
        /// </summary>
        internal string ErrorPath;
        /// <summary>
        /// 返回路径
        /// </summary>
        internal string ReturnPath;
        /// <summary>
        /// 重定向路径
        /// </summary>
        internal string LocationPath;
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            ErrorPath = ReturnPath = LocationPath = null;
        }
        /// <summary>
        /// 设置重定向路径
        /// </summary>
        /// <param name="locationPath">重定向路径</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetLocation(string locationPath)
        {
            LocationPath = locationPath;
            ErrorPath = ReturnPath = null; 
        }
        /// <summary>
        /// 设置错误路径
        /// </summary>
        /// <param name="errorPath">错误重定向路径</param>
        /// <param name="returnPath">返回路径</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetError(string errorPath, string returnPath)
        {
            ErrorPath = errorPath;
            ReturnPath = returnPath;
            LocationPath = null;
        }
    }
}

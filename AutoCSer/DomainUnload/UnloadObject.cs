using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.DomainUnload
{
    /// <summary>
    /// 委托回调
    /// </summary>
    internal sealed class UnloadObject
    {
        /// <summary>
        /// 卸载处理对象
        /// </summary>
        public object Unload;
        /// <summary>
        /// 卸载处理类型
        /// </summary>
        public Type Type;
        /// <summary>
        /// 删除卸载处理函数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RemoveLast()
        {
            Unloader.RemoveLast(new UnloadInfo { Unload = Unload, Type = Type });
        }
        /// <summary>
        /// 删除卸载处理函数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void RemoveLastRun()
        {
            Unloader.RemoveLastRun(new UnloadInfo { Unload = Unload, Type = Type });
        }
    }
}

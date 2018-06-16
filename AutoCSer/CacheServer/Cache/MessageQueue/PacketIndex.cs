using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 数据包索引信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct PacketIndex
    {
        /// <summary>
        /// 数据标识
        /// </summary>
        internal ulong Identity;
        /// <summary>
        /// 数据文件内部标识
        /// </summary>
        internal uint FileIdentity
        {
            get { return (uint)Identity & (FileWriter.DataCountPerFile - 1); }
        }
        /// <summary>
        /// 文件位置
        /// </summary>
        internal long FileIndex;
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="identity"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ulong identity)
        {
            Identity = identity;
            FileIndex = 0;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fileIndex"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ulong identity, long fileIndex)
        {
            Identity = identity;
            FileIndex = fileIndex;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="fileIndex"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Get(out ulong identity, out long fileIndex)
        {
            identity = Identity;
            fileIndex = FileIndex;
        }
    }
}

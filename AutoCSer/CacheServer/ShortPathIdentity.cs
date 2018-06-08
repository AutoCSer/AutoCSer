using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 短路径索引标识
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct ShortPathIdentity
    {
        /// <summary>
        /// 序列化大小
        /// </summary>
        internal const int SerializeSize = sizeof(int) * 2 + sizeof(ulong) + sizeof(long);
        /// <summary>
        /// 服务启动时间
        /// </summary>
        internal long Ticks;
        /// <summary>
        /// 短路径标识
        /// </summary>
        internal ulong Identity;
        /// <summary>
        /// 短路径索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 短路径数据包大小
        /// </summary>
        internal int PacketSize;
        /// <summary>
        /// 短路径索引标识
        /// </summary>
        /// <param name="read"></param>
        internal ShortPathIdentity(byte* read)
        {
            Ticks = *(long*)read;
            Identity = *(ulong*)(read + sizeof(long));
            Index = *(int*)(read + (sizeof(long) + sizeof(ulong)));
            PacketSize = *(int*)(read + (sizeof(long) + sizeof(ulong) + sizeof(int)));
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeSerialize(UnmanagedStream stream)
        {
            UnsafeSerialize(stream.CurrentData);
            stream.ByteSize += SerializeSize;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="write"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void UnsafeSerialize(byte* write)
        {
            *(long*)write = Ticks;
            *(ulong*)(write + sizeof(long)) = Identity;
            *(int*)(write + (sizeof(long) + sizeof(ulong))) = Index;
            *(int*)(write + (sizeof(long) + sizeof(ulong) + sizeof(int))) = PacketSize;
        }
    }
}

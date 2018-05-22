using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 服务端数据结构索引标识
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false, IsMemberMap = false)]
    [StructLayout(LayoutKind.Auto)]
    internal unsafe struct IndexIdentity
    {
        /// <summary>
        /// 序列化大小
        /// </summary>
        internal const int SerializeSize = sizeof(int) + sizeof(ulong);
        /// <summary>
        /// TCP 操作返回值类型
        /// </summary>
        internal AutoCSer.Net.TcpServer.ReturnType TcpReturnType;
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal ReturnType ReturnType;
        /// <summary>
        /// 数据结构索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 数据结构标识
        /// </summary>
        internal ulong Identity;
        /// <summary>
        /// 设置服务端数据结构索引标识
        /// </summary>
        /// <param name="read"></param>
        internal IndexIdentity(byte* read)
        {
            Index = *(int*)read;
            Identity = *(ulong*)(read + sizeof(int));
            ReturnType = ReturnType.Unknown;
            TcpReturnType = AutoCSer.Net.TcpServer.ReturnType.Unknown;
        }
        /// <summary>
        /// 相等返回 0
        /// </summary>
        /// <param name="other"></param>
        /// <returns>相等返回 0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ulong Equals(ref IndexIdentity other)
        {
            return (uint)(other.Index ^ Index) | (other.Identity ^ Identity);
        }
        /// <summary>
        /// 设置服务端数据结构索引标识
        /// </summary>
        /// <param name="identity"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void set(ref IndexIdentity identity)
        {
            Index = identity.Index;
            Identity = identity.Identity;
        }
        /// <summary>
        /// 设置数据结构索引标识
        /// </summary>
        /// <param name="index">数据结构索引</param>
        /// <param name="identity">数据结构标识</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(int index, ulong identity)
        {
            Index = index;
            Identity = identity;
            ReturnType = ReturnType.Success;
        }
        /// <summary>
        /// 设置成功返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ulong SetSuccess()
        {
            ReturnType = ReturnType.Success;
            return Identity;
        }
        /// <summary>
        /// 设置服务端数据结构索引标识
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        internal bool Set(AutoCSer.Net.TcpServer.ReturnValue<IndexIdentity> identity)
        {
            TcpReturnType = identity.Type;
            if (identity.Value.ReturnType == ReturnType.Success)
            {
                set(ref identity.Value);
                ReturnType = ReturnType.Success;
                return true;
            }
            ReturnType = identity.Value.ReturnType == ReturnType.Success ? identity.Value.ReturnType : ReturnType.TcpError;
            return false;
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
            *(int*)write = Index;
            *(ulong*)(write + sizeof(int)) = Identity;
        }
    }
}

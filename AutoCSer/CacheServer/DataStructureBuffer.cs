using System;
using System.Runtime.InteropServices;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 服务端数据结构定义数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct DataStructureBuffer
    {
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubArray<byte> Data;
        /// <summary>
        /// 服务端数据结构索引标识
        /// </summary>
        internal IndexIdentity Identity;
        /// <summary>
        /// 缓存名称标识
        /// </summary>
        internal readonly string CacheName;
        /// <summary>
        /// 服务端数据结构定义数据
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        internal unsafe DataStructureBuffer(Buffer buffer)
        {
            Data = buffer.Array;
            fixed (byte* dataFixed = Data.Array)
            {
                byte* start = dataFixed + Data.Start, read = start + (OperationParameter.Serializer.HeaderSize + IndexIdentity.SerializeSize);
                Identity = new IndexIdentity(start + OperationParameter.Serializer.HeaderSize);
                CacheName = AutoCSer.BinarySerialize.DeSerializer.DeSerializeString(ref read, start + *(int*)start);
                Data.MoveStart((int)(read - start));
            }
        }
    }
}

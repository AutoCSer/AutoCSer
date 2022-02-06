using AutoCSer.Expand.Win32;
using System;
using System.Collections.Generic;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 簇流数据集合
    /// </summary>
    public struct ClusterStreamAttribute
    {
        /// <summary>
        /// 数据
        /// </summary>
        private readonly byte[] buffer;
        /// <summary>
        /// 起始位置
        /// </summary>
        private readonly int startIndex;
        /// <summary>
        /// 索引扩展
        /// </summary>
        /// <param name="attribute"></param>
        public ClusterStreamAttribute(MainFileTableAttribute attribute)
        {
            buffer = attribute.Buffer;
            startIndex = attribute.DataStartIndex;
        }
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="size"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private ulong getCount(int size, int startIndex)
        {
            switch (size)
            {
                case 1: return buffer[startIndex];
                case 2: return BitConverter.ToUInt16(buffer, startIndex);
                case 3: return BitConverter.ToUInt32(buffer, startIndex) & 0xffffffU;
                case 4: return BitConverter.ToUInt32(buffer, startIndex);
                case 5: return BitConverter.ToUInt64(buffer, startIndex) & 0xffffffffffUL;
                case 6: return BitConverter.ToUInt64(buffer, startIndex) & 0xffffffffffffUL;
                case 7: return BitConverter.ToUInt64(buffer, startIndex) & 0xffffffffffffffUL;
                case 8: return BitConverter.ToUInt64(buffer, startIndex);
            }
            throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// 获取偏移
        /// </summary>
        /// <param name="size"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private long getOffset(int size, int startIndex)
        {
            switch (size)
            {
                case 1: return (sbyte)buffer[startIndex];
                case 2: return BitConverter.ToInt16(buffer, startIndex);
                case 3: return getOffset(BitConverter.ToUInt32(buffer, startIndex), 24);
                case 4: return BitConverter.ToInt32(buffer, startIndex);
                case 5: return getOffset(BitConverter.ToUInt64(buffer, startIndex), 40);
                case 6: return getOffset(BitConverter.ToUInt64(buffer, startIndex), 48);
                case 7: return getOffset(BitConverter.ToUInt64(buffer, startIndex), 56);
                case 8: return BitConverter.ToInt64(buffer, startIndex);
            }
            throw new IndexOutOfRangeException();
        }
        /// <summary>
        /// 获取偏移
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        private long getOffset(ulong offset, int bits)
        {
            ulong andValue = (1UL << bits) - 1;
            offset &= andValue;
            if (offset >> (bits - 1) != 0) offset |= andValue ^ ulong.MaxValue;
            return (long)offset;
        }
        /// <summary>
        /// 获取簇流数据集合
        /// </summary>
        public IEnumerable<ClusterStream> ClusterStreams
        {
            get
            {
                int startIndex = this.startIndex, clusterCountSize = buffer[startIndex] & 15, offsetSize = buffer[startIndex] >> 4;
                ClusterStream clusterStream = new ClusterStream((long)getCount(clusterCountSize, startIndex + 1), (long)getCount(offsetSize, startIndex + 1 + clusterCountSize));
                yield return clusterStream;
                while (buffer[startIndex += clusterCountSize + offsetSize + 1] != 0)
                {
                    clusterCountSize = buffer[startIndex] & 15;
                    offsetSize = buffer[startIndex] >> 4;
                    clusterStream.ClusterCount = (long)getCount(clusterCountSize, startIndex + 1);
                    clusterStream.Offset += getOffset(offsetSize, startIndex + 1 + clusterCountSize);
                    yield return clusterStream;
                }
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dirverHandle"></param>
        /// <param name="newTechnologyFileSystem"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public IEnumerable<byte[]> ReadData(Microsoft.Win32.SafeHandles.SafeFileHandle dirverHandle, NewTechnologyFileSystem newTechnologyFileSystem, byte[] buffer = null)
        {
            int bytePerCluster = (int)newTechnologyFileSystem.BytePerCluster;
            foreach (ClusterStream clusterStream in ClusterStreams)
            {
                if (!Kernel32.SetFilePointer(dirverHandle, clusterStream.Offset * bytePerCluster))
                {
                    yield return null;
                    break;
                }
                for (long ClusterCount = clusterStream.ClusterCount; ClusterCount > 0; --ClusterCount)
                {
                    if (buffer == null || buffer.Length < bytePerCluster) buffer = new byte[bytePerCluster];
                    if (Kernel32.ReadFile(dirverHandle, buffer, bytePerCluster) != bytePerCluster)
                    {
                        yield return null;
                        goto RETURN;
                    }
                    yield return buffer;
                }
            }
        RETURN:;
        }
    }
}

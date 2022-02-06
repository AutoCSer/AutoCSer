using System;
using System.Collections.Generic;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 文件索引，索引区域占用了若干个簇，每一个簇都包含了两部分：一个标准索引头和若干个标准索引项
    /// </summary>
    internal struct FileIndex
    {
        /// <summary>
        /// 数据
        /// </summary>
        internal readonly byte[] Buffer;
        /// <summary>
        /// 文件索引
        /// </summary>
        /// <param name="buffer"></param>
        internal FileIndex(byte[] buffer)
        {
            Buffer = buffer;
            if (BitConverter.ToInt32(buffer, 0) != 'I' + ('N' << 8) + ('D' << 16) + ('X' << 24)
                || (int)IndexEntryOffset < 0 || (int)IndexEntrySize <= 0)
            {
                Buffer = null;
            }
        }
        /// <summary>
        /// 更新序列号偏移
        /// </summary>
        public ushort UpdateSerialNumberOffset
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
        }
        /// <summary>
        /// 更新序列号和更新数组大小
        /// </summary>
        public ushort UpdateSerialNumberSize
        {
            get { return BitConverter.ToUInt16(Buffer, 6); }
        }
        /// <summary>
        /// 日志文件序列号(LSN)
        /// </summary>
        public ulong LogSerialNumber
        {
            get { return BitConverter.ToUInt64(Buffer, 8); }
        }
        /// <summary>
        /// 本索引缓冲区在索引分配中的VCN
        /// </summary>
        public ulong IndexCacheVCN
        {
            get { return BitConverter.ToUInt64(Buffer, 0x10); }
        }
        /// <summary>
        /// 索引项的偏移 相对于当前位置
        /// </summary>
        public uint IndexEntryOffset
        {
            get { return BitConverter.ToUInt32(Buffer, 0x18); }
        }
        /// <summary>
        /// 索引项和索引头的总字节数
        /// </summary>
        public uint IndexEntrySize
        {
            get { return BitConverter.ToUInt32(Buffer, 0x1C); }
        }
        /// <summary>
        /// 索引项分配的大小
        /// </summary>
        public uint IndexEntryAllocSize
        {
            get { return BitConverter.ToUInt32(Buffer, 0x20); }
        }
        /// <summary>
        /// 1 表示有子节点
        /// </summary>
        public bool HasLeafNode
        {
            get { return Buffer[0x24] == 1; }
        }
        /// <summary>
        /// 更新序列号
        /// </summary>
        public ushort UpdateSerialNumber
        {
            get { return BitConverter.ToUInt16(Buffer, 0x28); }
        }
        /// <summary>
        /// 标准索引项集合
        /// </summary>
        public IEnumerable<FileIndexEntry> IndexEntrys
        {
            get
            {
                int startIndex = (int)IndexEntryOffset;
                while (startIndex < IndexEntrySize)
                {
                    FileIndexEntry fileIndexEntry = new FileIndexEntry(Buffer, startIndex + 0x18);
                    yield return fileIndexEntry;
                    startIndex += fileIndexEntry.IndexEntrySize;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// B+树索引根节点
    /// </summary>
    internal struct IndexRootAttribute
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
        /// B+树索引根节点
        /// </summary>
        /// <param name="attribute"></param>
        internal IndexRootAttribute(MainFileTableAttribute attribute)
        {
            buffer = attribute.Buffer;
            startIndex = attribute.DataStartIndex;
            if ((int)EntryOffset < 0 || (int)TotalSize <= 0) buffer = null;
        }
        /// <summary>
        /// 属性的类型
        /// </summary>
        public uint AttributeType
        {
            get { return BitConverter.ToUInt32(buffer, startIndex); }
        }
        /// <summary>
        /// 排序规则
        /// </summary>
        public uint ColRule
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 4); }
        }
        /// <summary>
        /// 索引项分配尺寸
        /// </summary>
        public uint EntrySize
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 8); }
        }
        /// <summary>
        /// 每个索引项占用的簇数
        /// </summary>
        public byte ClusterPerList
        {
            get { return buffer[startIndex + 0xC]; }
        }

        #region 索引头
        /// <summary>
        /// 第一个索引项的偏移
        /// </summary>
        public uint EntryOffset
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x10); }
        }
        /// <summary>
        /// 索引项的总尺寸(包括索引头和下面的索引项)
        /// </summary>
        public uint TotalSize
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x14); }
        }
        /// <summary>
        /// 索引项分配的尺寸
        /// </summary>
        public uint AllocSize
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x18); }
        }
        /// <summary>
        /// 标志位，0x00 小目录(数据存放在根节点的数据区中)，0x01 大目录(需要索引项存储区和索引项位图)
        /// </summary>
        public byte Flags
        {
            get { return buffer[startIndex + 0x1C]; }
        }
        #endregion

        /// <summary>
        /// 索引项集合
        /// </summary>
        internal IEnumerable<FileIndexEntry> IndexEntrys
        {
            get
            {
                int startIndex = (int)EntryOffset;
                while (startIndex < TotalSize)
                {
                    FileIndexEntry fileIndexEntry = new FileIndexEntry(buffer, startIndex + this.startIndex + 0x10);
                    yield return fileIndexEntry;
                    startIndex += fileIndexEntry.IndexEntrySize;
                }
            }
        }
    }
}

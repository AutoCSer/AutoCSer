using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 标准索引项
    /// </summary>
    public struct FileIndexEntry
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
        /// 标准索引项
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        internal FileIndexEntry(byte[] buffer, int startIndex)
        {
            this.buffer = buffer;
            this.startIndex = startIndex;
        }
        /// <summary>
        /// 文件的MFT参考号
        /// </summary>
        public ulong MainFileTableReferNumber
        {
            get { return BitConverter.ToUInt64(buffer, startIndex); }
        }
        /// <summary>
        ///  MFT 文件记录编号，每一项占用2个扇区
        /// </summary>
        public ulong MainFileTableIndex
        {
            get { return MainFileTableReferNumber & 0xffffffffffffUL; }
        }
        /// <summary>
        /// 获取文件记录的起始扇区号，每一项占用2个扇区
        /// </summary>
        /// <param name="NTFS"></param>
        /// <returns></returns>
        public ulong GetSectorIndex(NewTechnologyFileSystem NTFS)
        {
            return NTFS.MainFileTableSectorIndex + (MainFileTableIndex << 1);
        }
        /// <summary>
        /// 索引项的大小
        /// </summary>
        public ushort IndexEntrySize
        {
            get { return BitConverter.ToUInt16(buffer, startIndex + 8); }
        }
        /// <summary>
        /// 文件名偏移
        /// </summary>
        public ushort FileNameOffset
        {
            get { return BitConverter.ToUInt16(buffer, startIndex + 0xA); }
        }
        /// <summary>
        /// 索引标志
        /// </summary>
        public ushort IndexFlag
        {
            get { return BitConverter.ToUInt16(buffer, startIndex + 0xC); }
        }
        /// <summary>
        /// 父目录MFT文件参考号
        /// </summary>
        public ulong FatherDirMainFileTableReferNumber
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x10); }
        }
        /// <summary>
        /// 文件创建时间
        /// </summary>
        public DateTime CreatTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x18)); }
        }
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        public DateTime AlterTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x20)); }
        }
        /// <summary>
        /// 文件记录最后修改时间
        /// </summary>
        public DateTime MainFileTableChangeTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x28)); }
        }
        /// <summary>
        /// 文件最后访问时间
        /// </summary>
        public DateTime ReadTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x30)); }
        }
        /// <summary>
        /// 文件分配大小
        /// </summary>
        public ulong FileAllocSize
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x38); }
        }
        /// <summary>
        /// 文件实际大小
        /// </summary>
        public ulong FileRealSize
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x40); }
        }
        /// <summary>
        /// 文件标志
        /// </summary>
        public ulong FileFlag
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x48); }
        }
        /// <summary>
        /// 文件名长度
        /// </summary>
        public byte FileNameSize
        {
            get { return buffer[startIndex + 0x50]; }
        }
        /// <summary>
        /// 文件命名空间
        /// </summary>
        public FileNamespace Namespace
        {
            get { return (FileNamespace)buffer[startIndex + 0x51]; }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return System.Text.Encoding.Unicode.GetString(buffer, startIndex + 0x52, FileNameSize << 1); }
        }
        /// <summary>
        /// 判断文件名称是否匹配
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public unsafe bool IsFileName(string fileName)
        {
            if (FileNameSize == FileName.Length)
            {
                fixed(byte* bufferFixed = buffer)
                fixed (char* fileNameFixed = fileName)
                {
                    return AutoCSer.Memory.Common.EqualNotNull(bufferFixed + (startIndex + 0x52), fileNameFixed, fileName.Length << 1);
                }
            }
            return false;
        }
    }
}

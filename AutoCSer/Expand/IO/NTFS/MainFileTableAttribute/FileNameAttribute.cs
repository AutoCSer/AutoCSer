using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 文件名
    /// </summary>
    public struct FileNameAttribute
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
        /// 文件名（Unicode）
        /// </summary>
        /// <param name="attribute"></param>
        public FileNameAttribute(MainFileTableAttribute attribute)
        {
            buffer = attribute.Buffer;
            startIndex = attribute.DataStartIndex;
        }
        /// <summary>
        /// 父目录的MFT记录的记录索引。注意：该值的低6字节是MFT记录号，高2字节是该MFT记录的序列号
        /// </summary>
        public ulong ParentFR
        {
            get { return BitConverter.ToUInt64(buffer, startIndex); }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 8)); }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime AlterTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x10)); }
        }
        /// <summary>
        /// 文件记录最后修改时间
        /// </summary>
        public DateTime MainFileTableChangeTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x18)); }
        }
        /// <summary>
        /// 文件记录最后访问时间
        /// </summary>
        public DateTime ReadTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x20)); }
        }
        /// <summary>
        /// 占用空间大小
        /// </summary>
        public ulong AllocSize
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x28); }
        }
        /// <summary>
        /// 文件的真实尺寸
        /// </summary>
        public ulong ValidSize
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x30); }
        }
        /// <summary>
        /// DOS文件属性
        /// </summary>
        public FileAttributes Attribute
        {
            get { return (FileAttributes)BitConverter.ToUInt32(buffer, startIndex + 0x38); }
        }
        /// <summary>
        /// 扩展属性与链接，用于 EAS 和重解析点
        /// </summary>
        public uint Reparse
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x3C); }
        }
        /// <summary>
        /// 文件名的字符数
        /// </summary>
        public byte NameSize
        {
            get { return buffer[startIndex + 0x40]; }
        }
        /// <summary>
        /// 命名空间
        /// </summary>
        public FileNamespace Namespace
        {
            get { return (FileNamespace)buffer[startIndex + 0x41]; }
        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get
            {
                return System.Text.Encoding.Unicode.GetString(buffer, startIndex + 0x42, NameSize << 1);
            }
        }
        /// <summary>
        /// 判断是否文件目录根节点文件
        /// </summary>
        public bool IsRoot
        {
            get { return NameSize == 1 && BitConverter.ToInt16(buffer, startIndex + 0x42) == '.'; }
        }
    }
}

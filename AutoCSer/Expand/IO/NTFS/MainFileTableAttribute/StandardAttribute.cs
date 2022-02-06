using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 标准属性
    /// </summary>
    public struct StandardAttribute
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
        /// 标准属性
        /// </summary>
        /// <param name="attribute"></param>
        public StandardAttribute(MainFileTableAttribute attribute)
        {
            buffer = attribute.Buffer;
            startIndex = attribute.DataStartIndex;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex)); }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime AlterTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 8)); }
        }
        /// <summary>
        /// 文件记录最后修改时间
        /// </summary>
        public DateTime MainFileTableChangeTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x10)); }
        }
        /// <summary>
        /// 文件记录最后访问时间
        /// </summary>
        public DateTime ReadTime
        {
            get { return MainFileTableAttribute.GetFileTime(BitConverter.ToUInt64(buffer, startIndex + 0x18)); }
        }
        /// <summary>
        /// 文件属性
        /// </summary>
        public FileAttributes Attributes
        {
            get { return (FileAttributes)BitConverter.ToUInt32(buffer, startIndex + 0x20); }
        }
        /// <summary>
        /// 文件允许的最大版本号
        /// </summary>
        public uint MaxFileVersion
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x24); }
        }
        /// <summary>
        /// 文件版本号
        /// </summary>
        public uint FileVersion
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x28); }
        }
        /// <summary>
        /// 类ID（一个双向的类索引）
        /// </summary>
        public uint ClassIdentity
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x2C); }
        }
        /// <summary>
        /// 所有者ID，0 表示未使用配额
        /// </summary>
        public uint OwnerID
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x30); }
        }
        /// <summary>
        /// 安全ID
        /// </summary>
        public uint SecureID
        {
            get { return BitConverter.ToUInt32(buffer, startIndex + 0x34); }
        }
        /// <summary>
        /// 文件占用字节数
        /// </summary>
        public ulong AllocSize
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x38); }
        }
        /// <summary>
        /// 文件更新序列化
        /// </summary>
        public ulong UpdateSerialNumber
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x40); }
        }
    }
}

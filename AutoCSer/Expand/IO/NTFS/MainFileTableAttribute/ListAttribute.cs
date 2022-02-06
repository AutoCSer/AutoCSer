using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 属性列表
    /// </summary>
    public struct ListAttribute
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
        /// 属性列表
        /// </summary>
        /// <param name="attribute"></param>
        public ListAttribute(MainFileTableAttribute attribute)
        {
            buffer = attribute.Buffer;
            startIndex = attribute.DataStartIndex;
        }
        /// <summary>
        /// 类型
        /// </summary>
        public uint Type
        {
            get { return BitConverter.ToUInt32(buffer, startIndex); }
        }
        /// <summary>
        /// 记录长度
        /// </summary>
        public ushort Size
        {
            get { return BitConverter.ToUInt16(buffer, startIndex  + 4); }
        }
        /// <summary>
        /// 属性名称长度
        /// </summary>
        public byte NameSize
        {
            get { return buffer[startIndex + 6]; }
        }
        /// <summary>
        /// 属性名称偏移
        /// </summary>
        public byte NameOffset
        {
            get { return buffer[startIndex + 7]; }
        }
        /// <summary>
        /// 起始VCN
        /// </summary>
        public ulong StartVCN
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 8); }
        }
        /// <summary>
        /// 基础文件记录的参考号
        /// </summary>
        public ulong BaseFileRecord
        {
            get { return BitConverter.ToUInt64(buffer, startIndex + 0x10); }
        }
        /// <summary>
        /// 属性ID
        /// </summary>
        public ushort AttributeID
        {
            get { return BitConverter.ToUInt16(buffer, startIndex + 0x18); }
        }
    }
}

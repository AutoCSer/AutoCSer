using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 主文件表属性
    /// </summary>
    public struct MainFileTableAttribute
    {
        /// <summary>
        /// 数据
        /// </summary>
        internal readonly byte[] Buffer;
        /// <summary>
        /// 起始位置
        /// </summary>
        private readonly int startIndex;
        /// <summary>
        /// 属性数据偏移
        /// </summary>
        internal int DataStartIndex { get { return startIndex + DataOffset; } }
        /// <summary>
        /// 主文件表属性
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        internal MainFileTableAttribute(byte[] buffer, int startIndex)
        {
            this.Buffer = buffer;
            this.startIndex = startIndex;
            //uint.MaxValue 为结束标志
            if ((uint)Type == uint.MaxValue || (int)Size <= 0) this.Buffer = null;
        }
        /// <summary>
        /// 属性类型
        /// </summary>
        public MainFileTableAttributeType Type
        {
            get { return (MainFileTableAttributeType)BitConverter.ToUInt32(Buffer, startIndex); }
        }
        /// <summary>
        /// 属性头和属性体的总长度
        /// </summary>
        public uint Size
        {
            get { return BitConverter.ToUInt32(Buffer, startIndex + 4); }
        }
        /// <summary>
        /// 是否是常驻属性（0常驻 1非常驻）
        /// </summary>
        public bool ResidentFlag
        {
            get { return Buffer[startIndex + 8] != 0; }
        }
        /// <summary>
        /// 属性名的长度
        /// </summary>
        public byte NameSize
        {
            get { return Buffer[startIndex + 9]; }
        }
        /// <summary>
        /// 属性名的偏移 相对于属性头
        /// </summary>
        public ushort NameOffset
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0xA); }
        }

        #region 常驻属性
        /// <summary>
        /// 属性数据的长度
        /// </summary>
        public uint DataSize
        {
            get { return BitConverter.ToUInt32(Buffer, startIndex + 0x10); }
        }
        /// <summary>
        /// 属性数据相对于属性头的偏移 / 簇流列表相对于属性头的偏移
        /// </summary>
        public ushort DataOffset
        {
            get { return ResidentFlag ? BitConverter.ToUInt16(Buffer, startIndex + 0x20) : BitConverter.ToUInt16(Buffer, startIndex + 0x14); }
        }
        /// <summary>
        /// 索引
        /// </summary>
        public byte Index
        {
            get { return Buffer[startIndex + 0x16]; }
        }
        /// <summary>
        /// 保留
        /// </summary>
        public byte Reserve
        {
            get { return Buffer[startIndex + 0x17]; }
        }
        /// <summary>
        /// 获取属性数据
        /// </summary>
        public SubArray<byte> Data
        {
            get
            {
                if(!ResidentFlag) return new SubArray<byte>(startIndex + DataOffset, (int)DataSize, Buffer);
                return default(SubArray<byte>);
            }
        }
        #endregion

        #region 非常驻属性
        /// <summary>
        /// 标志（0x0001压缩 0x4000加密 0x8000稀疏）
        /// </summary>
        public ushort Flags
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0xC); }
        }
        /// <summary>
        /// 属性唯一ID
        /// </summary>
        public ushort Id
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0xE); }
        }
        /// <summary>
        /// 本属性中数据流起始虚拟簇号
        /// </summary>
        public ulong StartVCN
        {
            get { return BitConverter.ToUInt64(Buffer, startIndex + 0x10); }
        }
        /// <summary>
        /// 属性中数据流终止虚拟簇号
        /// </summary>
        public ulong EndVCN
        {
            get { return BitConverter.ToUInt64(Buffer, startIndex + 0x18); }
        }
        /// <summary>
        /// 压缩单位 2的N次方
        /// </summary>
        public ushort CompressSize
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0x22); }
        }
        /// <summary>
        /// 属性分配的大小
        /// </summary>
        public ulong AllocSize
        {
            get { return BitConverter.ToUInt64(Buffer, startIndex + 0x28); }
        }
        /// <summary>
        /// 属性的实际大小
        /// </summary>
        public ulong ValidSize
        {
            get { return BitConverter.ToUInt64(Buffer, startIndex + 0x30); }
        }
        /// <summary>
        /// 属性的初始大小
        /// </summary>
        public ulong InitedSize
        {
            get { return BitConverter.ToUInt64(Buffer, startIndex + 0x38); }
        }
        #endregion

        /// <summary>
        /// 文件时间
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        internal static DateTime GetFileTime(ulong Value)
        {
            return new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks((long)Value);
        }
    }
}

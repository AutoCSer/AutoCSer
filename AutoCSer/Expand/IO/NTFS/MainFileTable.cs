using AutoCSer.Expand.Win32;
using System;
using System.Collections.Generic;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// MFT 主文件表
    /// </summary>
    public struct MainFileTable
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
        /// 文件位置
        /// </summary>
        private readonly long fileIndex;
        /// <summary>
        /// 是否成功读取数据
        /// </summary>
        public bool IsBuffer { get { return Buffer != null; } }
        /// <summary>
        /// MFT 主文件表
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <param name="fileIndex"></param>
        internal MainFileTable(byte[] buffer, int startIndex, long fileIndex)
        {
            this.Buffer = buffer;
            this.startIndex = startIndex;
            this.fileIndex = fileIndex;
            //0x454C4946 == "FILE"
            if (BitConverter.ToInt32(buffer, startIndex) != 0x454C4946 || (int)BytesAllocated <= 0) this.Buffer = null;
        }
        /// <summary>
        /// 更新序列号偏移, 与操作系统有关
        /// </summary>
        public ushort USNOffset
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 4); }
        }
        /// <summary>
        /// 更新序列号与更新数组字节大小
        /// </summary>
        public ushort USNCount
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 6); }
        }
        /// <summary>
        /// 日志文件序列号(LSN)
        /// </summary>
        public ulong LogSerialNumber
        {
            get { return BitConverter.ToUInt64(Buffer, startIndex + 8); }
        }
        /// <summary>
        /// 序列号(用于记录文件被反复使用的次数)
        /// </summary>
        public ushort SequenceNumber
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0x10); }
        }
        /// <summary>
        /// 硬连接数
        /// </summary>
        public ushort LinkCount
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0x12); }
        }
        /// <summary>
        /// 第一个属性偏移
        /// </summary>
        public ushort AttributeOffset
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0x14); }
        }
        /// <summary>
        /// 文件标记
        /// </summary>
        public unsafe FileFlags Flags
        {
            get { return (FileFlags)BitConverter.ToUInt16(Buffer, startIndex + 0x16); }
            set
            {
                fixed (byte* bufferFixed = Buffer)
                {
                    *(ushort*)(bufferFixed + (startIndex + 0x16)) = (ushort)value;
                }
            }
        }
        /// <summary>
        /// 文件记录实时大小(字节) 当前MFT表项长度,到FFFFFF的长度+4
        /// </summary>
        public uint BytesInUse
        {
            get { return BitConverter.ToUInt32(Buffer, startIndex + 0x18); }
        }
        /// <summary>
        /// 文件记录分配大小(字节)
        /// </summary>
        public uint BytesAllocated
        {
            get { return BitConverter.ToUInt32(Buffer, startIndex + 0x1C); }
        }
        /// <summary>
        /// 基础文件记录的参考号
        /// </summary>
        public ulong BaseFileRecord
        {
            get { return BitConverter.ToUInt64(Buffer, startIndex + 0x20); }
        }
        /// <summary>
        /// 下一个自由ID号
        /// </summary>
        public ushort NextAttributeNumber
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0x28); }
        }
        /// <summary>
        /// 边界
        /// </summary>
        public ushort Pading
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0x2A); }
        }
        /// <summary>
        /// windows xp中使用,本MFT记录号
        /// </summary>
        public uint RecordNumber
        {
            get { return BitConverter.ToUInt32(Buffer, startIndex + 0x2C); }
        }
        /// <summary>
        /// 更新序列号
        /// </summary>
        public ushort UpdateSerialNumber
        {
            get { return BitConverter.ToUInt16(Buffer, startIndex + 0x30); }
        }
        /// <summary>
        /// 更新数组字节长度
        /// </summary>
        public int UpdateArraySize
        {
            get { return AttributeOffset - 0x32; }
        }
        /// <summary>
        /// 枚举属性集合
        /// </summary>
        public IEnumerable<MainFileTableAttribute> Attributes
        {
            get
            {
                int startIndex = AttributeOffset;
                do
                {
                    MainFileTableAttribute attribute = new MainFileTableAttribute(Buffer, startIndex + this.startIndex);
                    if (attribute.Buffer == null || attribute.Size > BytesInUse) break;
                    startIndex += (int)attribute.Size;
                    if (startIndex > BytesInUse) break;
                    yield return attribute;
                }
                while (true);
            }
        }
        /// <summary>
        /// 枚举索引项集合
        /// </summary>
        /// <param name="dirverHandle"></param>
        /// <param name="newTechnologyFileSystem"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public IEnumerable<FileIndexEntry> GetIndexEntrys(Microsoft.Win32.SafeHandles.SafeFileHandle dirverHandle, NewTechnologyFileSystem newTechnologyFileSystem, byte[] buffer = null)
        {
            foreach (MainFileTableAttribute mainFileTableAttribute in Attributes)
            {
                switch (mainFileTableAttribute.Type)
                {
                    case MainFileTableAttributeType.IndexRoot:
                        IndexRootAttribute IndexRootAttribute = new IndexRootAttribute(mainFileTableAttribute);
                        foreach (FileIndexEntry fileIndexEntry in IndexRootAttribute.IndexEntrys) yield return fileIndexEntry;
                        break;
                    case MainFileTableAttributeType.IndexAllocation:
                        ClusterStreamAttribute indexAllocationAttribute = new ClusterStreamAttribute(mainFileTableAttribute);
                        foreach (byte[] data in indexAllocationAttribute.ReadData(dirverHandle, newTechnologyFileSystem, buffer))
                        {
                            if (data != null)
                            {
                                FileIndex fileIndex = new FileIndex(data);
                                if (fileIndex.Buffer == null) break;
                                foreach (FileIndexEntry fileIndexEntry in fileIndex.IndexEntrys) yield return fileIndexEntry;
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 调试检查第一个扇区数据（基本数据）是否和内存数据是否一致
        /// </summary>
        /// <param name="dirverHandle"></param>
        /// <param name="newTechnologyFileSystem"></param>
        /// <returns></returns>
        public unsafe bool CheckSector(Microsoft.Win32.SafeHandles.SafeFileHandle dirverHandle, NewTechnologyFileSystem newTechnologyFileSystem)
        {
            if (Kernel32.SetFilePointer(dirverHandle, fileIndex + startIndex))
            {
                byte[] buffer = new byte[newTechnologyFileSystem.BytePerSector];
                if (Kernel32.ReadFile(dirverHandle, buffer) == buffer.Length)
                {
                    fixed(byte* bufferFixed = Buffer, readBufferFixed = buffer)
                    {
                        return AutoCSer.Memory.Common.EqualNotNull(bufferFixed + startIndex, readBufferFixed, buffer.Length);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 保存第一个扇区数据（基本数据）
        /// </summary>
        /// <param name="dirverHandle"></param>
        /// <param name="newTechnologyFileSystem"></param>
        /// <returns></returns>
        public bool WriteSector(Microsoft.Win32.SafeHandles.SafeFileHandle dirverHandle, NewTechnologyFileSystem newTechnologyFileSystem)
        {
            if (Kernel32.SetFilePointer(dirverHandle, fileIndex + startIndex) && Kernel32.GetFilePointer(dirverHandle) == fileIndex + startIndex)
            {
                byte[] buffer = Buffer;
                if (startIndex != 0)
                {
                    buffer = new byte[newTechnologyFileSystem.BytePerSector];
                    System.Buffer.BlockCopy(Buffer, startIndex, buffer, 0, buffer.Length);
                }
                int writeSize = Kernel32.WriteFileWithLockVolume(dirverHandle, buffer, newTechnologyFileSystem.BytePerSector);
                return writeSize == newTechnologyFileSystem.BytePerSector;
            }
            return false;
        }
    }
}

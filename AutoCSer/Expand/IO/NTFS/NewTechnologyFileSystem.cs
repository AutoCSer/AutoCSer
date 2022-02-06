using System;
using AutoCSer.Expand.Win32;
using System.Collections.Generic;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// NTFS https://blog.csdn.net/Hilavergil/article/details/82622470
    /// </summary>
    public struct NewTechnologyFileSystem
    {
        /// <summary>
        /// 数据
        /// </summary>
        private readonly byte[] buffer;
        /// <summary>
        /// 是否成功读取数据
        /// </summary>
        public bool IsBuffer { get { return buffer != null; } }
        /// <summary>
        /// NTFS
        /// </summary>
        /// <param name="dirverHandle">逻辑驱动器句柄</param>
        /// <param name="buffer"></param>
        public NewTechnologyFileSystem(Microsoft.Win32.SafeHandles.SafeFileHandle dirverHandle, byte[] buffer = null)
        {
            this.buffer = buffer;
            if (Kernel32.SetFilePointer(dirverHandle, 0))
            {
                if (this.buffer == null || this.buffer.Length < 512) this.buffer = new byte[512];
                if (Kernel32.ReadFile(dirverHandle, this.buffer, 512) == 512
                    && BitConverter.ToInt64(this.buffer, 3) == 0x202020205346544EL)
                {
                    return;
                }
            }
            this.buffer = null;
        }
        /// <summary>
        /// 每扇区字节数
        /// </summary>
        public ushort BytePerSector
        {
            get { return BitConverter.ToUInt16(buffer, 0xB); }
        }
        /// <summary>
        /// 每簇扇区数
        /// </summary>
        public byte SectorPerCluster
        {
            get { return buffer[0xD]; }
        }
        /// <summary>
        /// 每簇字节数
        /// </summary>
        public uint BytePerCluster
        {
            get { return (uint)BytePerSector * SectorPerCluster; }
        }
        /// <summary>
        /// 保留扇区
        /// </summary>
        public ushort ReserveSector
        {
            get { return BitConverter.ToUInt16(buffer, 0xE); }
        }
        /// <summary>
        /// 介质描述，硬盘为 0xF8
        /// </summary>
        public byte MediumDescription
        {
            get { return buffer[0x15]; }
        }
        /// <summary>
        /// 每磁头扇区数
        /// </summary>
        public ushort SectorPerHead
        {
            get { return BitConverter.ToUInt16(buffer, 0x18); }
        }
        /// <summary>
        /// 每柱面磁头数
        /// </summary>
        public ushort HeadPerCylinder
        {
            get { return BitConverter.ToUInt16(buffer, 0x1A); }
        }
        /// <summary>
        /// 隐藏扇区数，从 MBR 到 DBR 的扇区数总数
        /// </summary>
        public uint HideSector
        {
            get { return BitConverter.ToUInt32(buffer, 0x1C); }
        }
        /// <summary>
        /// 分区扇区总数
        /// </summary>
        public ulong PartitionTotalSector
        {
            get { return BitConverter.ToUInt64(buffer, 0x28); }
        }
        /// <summary>
        /// MFT 主文件表起始簇号
        /// </summary>
        public ulong MainFileTableClusterIndex
        {
            get { return BitConverter.ToUInt64(buffer, 0x30); }
        }
        /// <summary>
        /// MFT 主文件表起始扇区号
        /// </summary>
        public ulong MainFileTableSectorIndex
        {
            get { return MainFileTableClusterIndex * SectorPerCluster; }
        }
        /// <summary>
        /// $MFTmirr 镜像起始簇号
        /// </summary>
        public ulong MainFileTableMirrorClusterIndex
        {
            get { return BitConverter.ToUInt64(buffer, 0x38); }
        }
        /// <summary>
        /// 每个主文件表记录的簇数
        /// </summary>
        public uint SectorPerMainFileTable
        {
            get { return BitConverter.ToUInt32(buffer, 0x40); }
        }
        /// <summary>
        /// 每个索引的簇数
        /// </summary>
        public uint SectorPerFileIndex
        {
            get { return BitConverter.ToUInt32(buffer, 0x44); }
        }
        /// <summary>
        /// 分区逻辑序列号（卷标）
        /// </summary>
        public ulong PartitionLogicalSerialNumber
        {
            get { return BitConverter.ToUInt64(buffer, 0x48); }
        }

        /// <summary>
        /// 数据保持到指定文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveFile(string fileName)
        {
            System.IO.File.WriteAllBytes(fileName, buffer);
        }

        /// <summary>
        /// 获取跟目录的主文件表
        /// </summary>
        /// <param name="dirverHandle"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public MainFileTable FindRoot(Microsoft.Win32.SafeHandles.SafeFileHandle dirverHandle, byte[] buffer = null)
        {
            int bytePerCluster = (int)BytePerCluster;
            long fileIndex = (long)(MainFileTableClusterIndex * (uint)bytePerCluster);
            if (Kernel32.SetFilePointer(dirverHandle, fileIndex))
            {
                if (buffer == null || buffer.Length < bytePerCluster) buffer = new byte[bytePerCluster];
                do
                {
                    if (Kernel32.ReadFile(dirverHandle, buffer, bytePerCluster) == bytePerCluster)
                    {
                        int bufferIndex = 0;
                        do
                        {
                            MainFileTable mainFileTable = new MainFileTable(buffer, bufferIndex, fileIndex);
                            if (mainFileTable.Buffer != null)
                            {
                                foreach (MainFileTableAttribute mainFileTableAttribute in mainFileTable.Attributes)
                                {
                                    if (mainFileTableAttribute.Type == MainFileTableAttributeType.FileName && !mainFileTableAttribute.ResidentFlag)
                                    {
                                        FileNameAttribute fileNameAttribute = new FileNameAttribute(mainFileTableAttribute);
                                        if (fileNameAttribute.IsRoot) return mainFileTable;
                                    }
                                }
                                bufferIndex += (int)mainFileTable.BytesAllocated;
                            }
                            else return default(MainFileTable);
                        }
                        while (bufferIndex < bytePerCluster);
                        fileIndex += bytePerCluster;
                    }
                    else return default(MainFileTable);
                }
                while (true);
            }
            return default(MainFileTable);
        }
        /// <summary>
        /// 获取指定文件的主文件表
        /// </summary>
        /// <param name="dirverHandle"></param>
        /// <param name="fileIndexEntry"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public MainFileTable Get(Microsoft.Win32.SafeHandles.SafeFileHandle dirverHandle, FileIndexEntry fileIndexEntry, byte[] buffer = null)
        {
            long fileIndex = (long)(fileIndexEntry.GetSectorIndex(this) * BytePerSector);
            if (Kernel32.SetFilePointer(dirverHandle, fileIndex))
            {
                int size = BytePerSector << 1;
                if (buffer == null || buffer.Length < size) buffer = new byte[size];
                if (Kernel32.ReadFile(dirverHandle, buffer, size) == size) return new MainFileTable(buffer, 0, fileIndex);
            }
            return default(MainFileTable);
        }
        /// <summary>
        /// 搜索文件或者目录的主文件表
        /// </summary>
        /// <param name="dirverHandle"></param>
        /// <param name="paths"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public MainFileTable SearchPath(Microsoft.Win32.SafeHandles.SafeFileHandle dirverHandle, IEnumerable<string> paths, byte[] buffer = null)
        {
            int bufferSize = BytePerSector << 1;
            MainFileTable mainFileTable = FindRoot(dirverHandle);
            foreach (string path in paths)
            {
                if (!mainFileTable.IsBuffer) return default(MainFileTable);
                bool isPath = false;
                foreach (FileIndexEntry fileIndexEntry in mainFileTable.GetIndexEntrys(dirverHandle, this))
                {
                    if (fileIndexEntry.IsFileName(path))
                    {
                        if (buffer == null || buffer.Length < bufferSize) buffer = new byte[bufferSize]; 
                        mainFileTable = Get(dirverHandle, fileIndexEntry, buffer);
                        isPath = true;
                        break;
                    }
                }
                if (!isPath) return default(MainFileTable);
            }
            return mainFileTable;
        }
    }
}

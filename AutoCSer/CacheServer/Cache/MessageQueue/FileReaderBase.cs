using System;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.Cache.MessageQueue
{
    /// <summary>
    /// 队列数据 读文件
    /// </summary>
    internal abstract unsafe class FileReaderBase
    {
        /// <summary>
        /// 状态缓存区大小
        /// </summary>
        protected const int stateBufferSize = sizeof(ulong);

        /// <summary>
        /// 队列数据 写文件
        /// </summary>
        internal readonly FileWriter Writer;
        /// <summary>
        /// 队列数据 读取配置
        /// </summary>
        internal readonly ReaderConfig Config;
        /// <summary>
        /// 保存已完成消息标识
        /// </summary>
        private readonly Action saveStateHandle;
        /// <summary>
        /// 状态文件
        /// </summary>
        protected FileStream stateFileStream;
        /// <summary>
        /// 数据文件
        /// </summary>
        protected FileStream dataFileStream;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        protected byte[] buffer;
        /// <summary>
        /// 数据缓冲区起始位置
        /// </summary>
        protected int bufferIndex;
        /// <summary>
        /// 数据缓冲区数据长度
        /// </summary>
        protected int bufferSize;
        /// <summary>
        /// 大数据缓冲区
        /// </summary>
        protected byte[] bigBuffer = new byte[stateBufferSize];
        /// <summary>
        /// 第一个消息索引位置
        /// </summary>
        protected int messageIndex;
        /// <summary>
        /// 当前写入消息索引位置
        /// </summary>
        protected int writeMessageIndex;
        /// <summary>
        /// 数据文件当前读取数据标识
        /// </summary>
        protected ulong dataFileIdentity;
        /// <summary>
        /// 消息首节点数据标识
        /// </summary>
        internal ulong Identity;
        /// <summary>
        /// 下一个写入消息数据标识
        /// </summary>
        protected ulong writeIdentity;
        /// <summary>
        /// 写文件下一个添加数据标识
        /// </summary>
        protected ulong writerAppendIdentity;
        /// <summary>
        /// 内存消息结束数据标识
        /// </summary>
        protected ulong memoryEndIdentity;
        /// <summary>
        /// 当前发送消息索引位置
        /// </summary>
        protected int sendMessageIndex;
        /// <summary>
        /// 下一个发送消息数据标识
        /// </summary>
        protected ulong sendIdentity;
        /// <summary>
        /// 设置已完成消息标识时间
        /// </summary>
        private DateTime flushIdentityTime;
        /// <summary>
        /// 需要保存的已完成消息标识
        /// </summary>
        private ulong saveIdentity;
        /// <summary>
        /// 已经保存的已完成消息标识
        /// </summary>
        private ulong savedIdentity;
        /// <summary>
        /// 已完成消息标识访问锁
        /// </summary>
        private int saveStateLock;
        /// <summary>
        /// 状态文件名称
        /// </summary>
        protected virtual string stateFileName
        {
            get { return Writer.FilePath + "reader"; }
        }
        /// <summary>
        /// 状态备份文件名称
        /// </summary>
        protected virtual string stateBackupFileName
        {
            get { return Writer.FilePath + "reader_backup"; }
        }
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        protected volatile int isDisposed;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        internal bool IsDisposed
        {
            get { return isDisposed != 0; }
        }
        /// <summary>
        /// 队列数据 读文件
        /// </summary>
        /// <param name="writer">队列数据 写文件</param>
        /// <param name="config">队列数据 读取配置</param>
        internal FileReaderBase(FileWriter writer, ReaderConfig config)
        {
            saveStateHandle = saveState;
            Writer = writer;
            Config = config;
            config.Format();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        protected void dispose()
        {
            --Writer.ReadCount;
            if (saveIdentity > savedIdentity && System.Threading.Interlocked.CompareExchange(ref saveStateLock, 1, 0) == 0)
            {
                try
                {
                    saveState();
                }
                catch { }
            }
            else if (stateFileStream != null)
            {
                stateFileStream.Dispose();
                stateFileStream = null;
            }
            if (dataFileStream != null)
            {
                dataFileStream.Dispose();
                dataFileStream = null;
            }
            buffer = bigBuffer = null;
        }
        /// <summary>
        /// 初始化加载状态文件
        /// </summary>
        protected void loadStateFile()
        {
            if (checkStateFile())
            {
                stateFileStream = new FileStream(stateFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, 256, FileOptions.None);
                stateFileStream.Seek(-stateBufferSize, SeekOrigin.End);
                stateFileStream.Read(bigBuffer, 0, stateBufferSize);
                fixed (byte* stateDataFixed = bigBuffer) Identity = *(ulong*)stateDataFixed;
            }
            else stateFileStream = new FileStream(stateFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, 256, FileOptions.None);
        }
        /// <summary>
        /// 检测状态文件是否存在
        /// </summary>
        /// <returns></returns>
        protected bool checkStateFile()
        {
            FileInfo stateFileInfo = new FileInfo(stateFileName);
            if (stateFileInfo.Exists)
            {
                if (stateFileInfo.Length >= stateBufferSize) return true;
                stateFileInfo.Delete();
            }
            FileInfo stateBackupFileInfo = new FileInfo(stateBackupFileName);
            if (stateBackupFileInfo.Exists && stateBackupFileInfo.Length >= stateBufferSize)
            {
                stateBackupFileInfo.MoveTo(stateFileName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 初始化加载索引信息
        /// </summary>
        /// <param name="fileIndex"></param>
        /// <returns></returns>
        protected bool loadIndex(ref long fileIndex)
        {
            if (Writer.GetIndex(Identity, ref dataFileIdentity, ref fileIndex))
            {
                writeIdentity = Identity;
                memoryEndIdentity = Identity + Config.MemoryCacheNodeCount;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 加载数据文件
        /// </summary>
        /// <param name="fileIndex"></param>
        protected void loadFile(long fileIndex)
        {
            if (buffer == null) buffer = new byte[(int)Writer.Config.BufferSize];
            bufferSize = 0;
            dataFileStream = new FileStream(Writer.GetFileName(dataFileIdentity), FileMode.Open, FileAccess.Read, FileShare.ReadWrite, buffer.Length, FileOptions.None);
            dataFileStream.Seek(fileIndex, SeekOrigin.Begin);
        }
        /// <summary>
        /// 继续读取数据文件
        /// </summary>
        /// <returns></returns>
        protected bool readFile()
        {
            int endIndex = bufferIndex + bufferSize;
            if ((bufferSize += dataFileStream.Read(buffer, endIndex, buffer.Length - endIndex)) < sizeof(int) * 3)
            {
                dataFileStream.Dispose();
                if (((uint)bufferSize | (dataFileIdentity & (FileWriter.DataCountPerFile - 1))) != 0) return false;
                dataFileStream = new FileStream(Writer.GetFileName(dataFileIdentity), FileMode.Open, FileAccess.Read, FileShare.ReadWrite, buffer.Length, FileOptions.None);
                bufferSize = dataFileStream.Read(buffer, 0, buffer.Length);
                if (bufferSize < sizeof(int) * 3) return false;
                bufferIndex = 0;
            }
            return true;
        }
        /// <summary>
        /// 保存已完成消息标识
        /// </summary>
        /// <param name="identity"></param>
        protected void saveState(ulong identity)
        {
            saveIdentity = identity;
            if (System.Threading.Interlocked.CompareExchange(ref saveStateLock, 1, 0) == 0)
            {
                if (saveIdentity == savedIdentity) System.Threading.Interlocked.Exchange(ref saveStateLock, 0);
                else AutoCSer.Threading.ThreadPool.Tiny.Start(saveStateHandle);
            }
        }
        /// <summary>
        /// 保存已完成消息标识
        /// </summary>
        private void saveState()
        {
            do
            {
                try
                {
                    SAVE:
                    System.Threading.Thread.Sleep(0);
                    if (stateFileStream.Length >= FileWriter.MaxStateFileSize)
                    {
                        stateFileStream.Dispose();
                        stateFileStream = null;
                        FileInfo bakFile = new FileInfo(stateBackupFileName);
                        if (bakFile.Exists) bakFile.Delete();
                        System.IO.File.Move(stateFileName, stateBackupFileName);
                        stateFileStream = new FileStream(stateFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read, 256, FileOptions.None);
                    }
                    ulong identity = saveIdentity;
                    fixed (byte* stateDataFixed = bigBuffer) *(ulong*)stateDataFixed = identity;
                    stateFileStream.Write(bigBuffer, 0, stateBufferSize);
                    savedIdentity = identity;
                    if (flushIdentityTime != Date.NowTime.Now)
                    {
                        stateFileStream.Flush(true);
                        flushIdentityTime = Date.NowTime.Now;
                    }
                    if (identity != saveIdentity) goto SAVE;
                    if (isDisposed != 0)
                    {
                        stateFileStream.Dispose();
                        stateFileStream = null;
                        return;
                    }
                }
                finally { System.Threading.Interlocked.Exchange(ref saveStateLock, 0); }
            }
            while (saveIdentity != savedIdentity && System.Threading.Interlocked.CompareExchange(ref saveStateLock, 1, 0) == 0);
        }
    }
}

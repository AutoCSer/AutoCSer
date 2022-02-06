using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 文件标记
    /// </summary>
    [Flags]
    public enum FileFlags : ushort
    {
        /// <summary>
        /// 是否使用中
        /// </summary>
        UsedFlag = 1,
        /// <summary>
        /// 是否目录
        /// </summary>
        DirectoryFlag = 2,

        /// <summary>
        /// 已删除文件
        /// </summary>
        DeleteFile = 0,
        /// <summary>
        /// 正常文件
        /// </summary>
        File = UsedFlag,
        /// <summary>
        /// 已删除目录
        /// </summary>
        DeleteDirectory = 2,
        /// <summary>
        /// 正常目录
        /// </summary>
        Directory = UsedFlag | DirectoryFlag
    }
}

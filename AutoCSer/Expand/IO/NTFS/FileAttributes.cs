using System;

namespace AutoCSer.IO.NTFS
{
    /// <summary>
    /// 文件属性
    /// </summary>
    [Flags]
    public enum FileAttributes : uint
    {
        /// <summary>
        /// 只读
        /// </summary>
        ReadOnly = 1,
        /// <summary>
        /// 隐藏
        /// </summary>
        Hide = 2,
        /// <summary>
        /// 系统
        /// </summary>
        System = 4,
        /// <summary>
        /// 存档
        /// </summary>
        Archive = 0x20,
        /// <summary>
        /// 设备
        /// </summary>
        Device = 0x40,
        /// <summary>
        /// 常规
        /// </summary>
        Normal = 0x80,
        /// <summary>
        /// 临时
        /// </summary>
        Temporary = 0x100,
        /// <summary>
        /// 稀疏
        /// </summary>
        Sparse = 0x200,
        /// <summary>
        /// 重解析点
        /// </summary>
        ReparsePoint = 0x400,
        /// <summary>
        /// 压缩
        /// </summary>
        Compression = 0x800,
        /// <summary>
        /// 脱机
        /// </summary>
        Offline = 0x1000,
        /// <summary>
        /// 未编入索引
        /// </summary>
        NotIndexed = 0x2000,
        /// <summary>
        /// 加密
        /// </summary>
        Encryption = 0x4000
    }
}

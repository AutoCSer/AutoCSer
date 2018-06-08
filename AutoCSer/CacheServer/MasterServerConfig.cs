using System;

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 缓存主服务配置
    /// </summary>
    public sealed class MasterServerConfig : ServerConfig
    {
        /// <summary>
        /// 物理文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 物理文件名称 AutoCSer Memory Cache
        /// </summary>
        internal string GetFileName
        {
            get
            {
                return FileName + ".amc";
            }
        }
        /// <summary>
        /// 物理文件名称 AutoCSer Memory Cache Switch
        /// </summary>
        internal string GetSwitchFileName
        {
            get
            {
                return FileName + ".amcs";
            }
        }
        /// <summary>
        /// 是否写文件持久化
        /// </summary>
        internal bool IsFile { get { return !string.IsNullOrEmpty(FileName); } }
        /// <summary>
        /// 物理文件刷新秒数默认为 1
        /// </summary>
        public int FileFlushSeconds = 1;
        /// <summary>
        /// 默认为 true 表示重命名不可识别的物理文件
        /// </summary>
        public bool IsMoveBakUnknownFile = true;
        /// <summary>
        /// 默认为 false 表示不忽略文件数据的完整性
        /// </summary>
        public bool IsIgnoreFileEndError = false;
        /// <summary>
        /// 压缩启用最低字节数量，默认为 1KB，小于等于 0 表示不压缩。压缩数据可以减低硬盘负载与冷启动时间，但是需要消耗一定的 CPU 资源。
        /// </summary>
        public int MinCompressSize = 1 << 10;
        /// <summary>
        /// 文件缓冲区大小默认为 128KB
        /// </summary>
        public SubBuffer.Size BufferSize = SubBuffer.Size.Kilobyte128;
        /// <summary>
        /// 文件读取缓冲区大小默认为 16KB
        /// </summary>
        public SubBuffer.Size ReaderBufferSize = SubBuffer.Size.Kilobyte16;

        /// <summary>
        /// 消息队列路径
        /// </summary>
        public string MessageQueuePath = @"MessageQueue\";
    }
}

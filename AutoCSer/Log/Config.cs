using System;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志配置
    /// </summary>
    //public class Config
    {
        /// <summary>
        /// 日志文件目录
        /// </summary>
        public string FilePath = PubPath.ApplicationPath;
        /// <summary>
        /// 日志文件默认最大字节数
        /// </summary>
        public int MaxFileSize = 1 << 20;
        /// <summary>
        /// 最大缓存日志数量
        /// </summary>
        public int MaxCacheCount = 1 << 4;
        /// <summary>
        /// 日志处理类型
        /// </summary>
        public LogType Type = LogType.All ^ LogType.AutoCSer ^ LogType.Debug ^ LogType.Info ^ LogType.Warn;
        /// <summary>
        /// 日志处理接口
        /// </summary>
        public ILog Log;
    }
}

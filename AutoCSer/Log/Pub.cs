using System;

namespace AutoCSer.Log
{
    /// <summary>
    /// 日志公共
    /// </summary>
    public static class Pub
    {
        /// <summary>
        /// 默认日志配置
        /// </summary>
        public static readonly Config Config = ConfigLoader.GetUnion(typeof(Config)).Config ?? new Config();
        /// <summary>
        /// 信息处理接口，一般用于辅助定位BUG
        /// </summary>
        public static readonly ILog Log = Config.Log ?? new AutoCSer.Log.File(Config.FilePath + File.DefaultFilePrefix + "default.txt", Config.MaxCacheCount, Config.Type);
    }
}

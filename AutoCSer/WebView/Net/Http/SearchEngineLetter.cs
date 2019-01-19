using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// 搜索引擎首字母
    /// </summary>
    internal enum SearchEngineLetter : byte
    {
        /// <summary>
        /// 未知字符
        /// </summary>
        Unknown,
        /// <summary>
        /// bingbot
        /// </summary>
        b,
        /// <summary>
        /// DotBot
        /// </summary>
        D,
        /// <summary>
        /// Googlebot,GeoHasher
        /// </summary>
        G,
        /// <summary>
        /// ia_archiver
        /// </summary>
        i,
        /// <summary>
        /// Mediapartners-Google,MJ12bot
        /// </summary>
        M,
        /// <summary>
        /// msnbot
        /// </summary>
        m,
        /// <summary>
        /// R6_CommentReader
        /// </summary>
        R,
        /// <summary>
        /// renren share slurp
        /// </summary>
        r,
        /// <summary>
        /// Sogou,SiteBot
        /// </summary>
        S,
        /// <summary>
        /// spider
        /// </summary>
        s,
        /// <summary>
        /// Twiceler
        /// </summary>
        T,
        /// <summary>
        /// Yandex,YoudaoBot,Yahoo! Slurp
        /// </summary>
        Y,
        /// <summary>
        /// ZhihuExternalHit
        /// </summary>
        Z
    }
}

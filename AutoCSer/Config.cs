using System;
using System.Text;
using System.IO;
using AutoCSer.Extensions;
using AutoCSer.Threading;

namespace AutoCSer
{
    /// <summary>
    /// 公共全局配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 全局编码
        /// </summary>
        public Encoding Encoding = Encoding.UTF8;
        ///// <summary>
        ///// 编码
        ///// </summary>
        //private EncodingCache encodingCache;
        ///// <summary>
        ///// 编码
        ///// </summary>
        //internal EncodingCache EncodingCache
        //{
        //    get
        //    {
        //        if (encodingCache.Encoding == null && Encoding != null) encodingCache = new EncodingCache(Encoding);
        //        return encodingCache;
        //    }
        //}
        /// <summary>
        /// 缓存文件主目录
        /// </summary>
        public string CachePath = AutoCSer.Config.ApplicationPath;
        /// <summary>
        /// 二维秒级定时器数组容器二进制位长度，默认为最小值 8，最大值为 12
        /// </summary>
        public byte TimeoutCapacityBitSize = 8;
        /// <summary>
        /// 日志文件操作异常处理（因为没法写日志），注意应用层也不要调用该日志记录这个异常，也不要让两个异常的日志相互调用避免死循环
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="exception"></param>
        public virtual void OnLogFileException(ILog log, Exception exception)
        {
            Console.WriteLine(exception.ToString());
        }
        /// <summary>
        /// 默认链表缓存池参数
        /// </summary>
        /// <param name="type">缓存数据类型</param>
        /// <returns>链表缓存池参数</returns>
        public virtual LinkPoolParameter GetLinkPoolParameter(Type type)
        {
            return LinkPoolParameter.Default;
        }
        /// <summary>
        /// 数据格式化
        /// </summary>
        internal void Format()
        {
            if (Encoding == null) Encoding = Encoding.UTF8;
            if (TimeoutCapacityBitSize < 8) TimeoutCapacityBitSize = 8;
            else if (TimeoutCapacityBitSize > 12) TimeoutCapacityBitSize = 12;

            if (CachePath != ApplicationPath)
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(CachePath);
                    if (!directory.Exists) directory.Create();
                    CachePath = directory.fullName();
                }
                catch (Exception error)
                {
                    CachePath = AutoCSer.Config.ApplicationPath;
                    AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(error, AutoCSer.Threading.ThreadTaskType.ConfigFormatException);
                }
            }
        }
        /// <summary>
        /// 数据格式化
        /// </summary>
        /// <param name="exception"></param>
        internal static void ConfigFormatException(Exception exception)
        {
            AutoCSer.LogHelper.Exception(exception, null, LogLevel.Exception | LogLevel.AutoCSer);
        }


        /// <summary>
        /// 进程内默认自增标识
        /// </summary>
        private static long identity = AutoCSer.Date.StartTime.Ticks;
        /// <summary>
        /// 进程内默认自增标识
        /// </summary>
        public static long Identity
        {
            get { return System.Threading.Interlocked.Increment(ref identity); }
        }
        /// <summary>
        /// AutoCSer 爬虫标识
        /// </summary>
        public const string HttpSpiderUserAgent = "AutoCSer spider";
        /// <summary>
        /// 缓存池默认缓存数量
        /// </summary>
        public const int DefaultPoolCount = 1 << 12;
        /// <summary>
        /// 获取链表缓存池默认缓存数量
        /// </summary>
        /// <param name="type">缓存数据类型</param>
        /// <returns>链表缓存池默认缓存数量</returns>
        public virtual int GetYieldPoolCount(Type type)
        {
            return DefaultPoolCount;
        }
#if !Serialize
        /// <summary>
        /// 是否调试模式
        /// </summary>
        public bool IsDebug;
        /// <summary>
        /// 是否 window 服务模式
        /// </summary>
        public bool IsService;
#endif
        /// <summary>
        /// 程序执行主目录，以 System.IO.Path.DirectorySeparatorChar 结尾
        /// </summary>
        public static readonly string ApplicationPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory).fullName();
    }
}
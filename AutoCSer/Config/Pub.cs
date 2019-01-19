using System;
using System.Text;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.Config
{
    /// <summary>
    /// 公用全局配置
    /// </summary>
    public class Pub
    {
#if Serialize
        /// <summary>
        /// 链表缓存池默认缓存数量
        /// </summary>
        public int YieldPoolCount = 1 << 12;
        /// <summary>
        /// 获取链表缓存池默认缓存数量
        /// </summary>
        /// <param name="type">缓存数据类型</param>
        /// <returns>链表缓存池默认缓存数量</returns>
        public virtual int GetYieldPoolCount(Type type)
        {
            return YieldPoolCount;
        }

        /// <summary>
        /// 默认全局配置
        /// </summary>
        public static readonly Pub Default = UnionLoader.GetUnion(typeof(Pub)).Pub ?? new Pub();
#else
        /// <summary>
        /// 是否调试模式
        /// </summary>
        public bool IsDebug;
        /// <summary>
        /// 全局编码
        /// </summary>
        public Encoding Encoding = Encoding.UTF8;
        /// <summary>
        /// 编码
        /// </summary>
        private EncodingCache encodingCache;
        /// <summary>
        /// 编码
        /// </summary>
        internal EncodingCache EncodingCache
        {
            get
            {
                if (encodingCache.Encoding == null && Encoding != null) encodingCache = new EncodingCache(Encoding);
                return encodingCache;
            }
        }
        ///// <summary>
        ///// 程序工作主目录
        ///// </summary>
        //public string WorkPath = AutoCSer.PubPath.ApplicationPath;
        /// <summary>
        /// 缓存文件主目录
        /// </summary>
        public string CachePath = AutoCSer.PubPath.ApplicationPath;
        /// <summary>
        /// 是否 window 服务模式
        /// </summary>
        public bool IsService;
        /// <summary>
        /// 缓存池默认缓存数量
        /// </summary>
        public const int DefaultPoolCount = 1 << 12;
        /// <summary>
        /// 链表缓存池默认缓存数量
        /// </summary>
        public int YieldPoolCount = DefaultPoolCount;
        /// <summary>
        /// 获取链表缓存池默认缓存数量
        /// </summary>
        /// <param name="type">缓存数据类型</param>
        /// <returns>链表缓存池默认缓存数量</returns>
        public virtual int GetYieldPoolCount(Type type)
        {
            return YieldPoolCount;
        }

        /// <summary>
        /// 默认全局配置
        /// </summary>
        public static readonly Pub Default;
        static Pub()
        {
            Default = UnionLoader.GetUnion(typeof(Pub)).Pub ?? new Pub();
            //if (Default.WorkPath != AutoCSer.PubPath.ApplicationPath)
            //{
            //    try
            //    {
            //        DirectoryInfo directory = new DirectoryInfo(Default.WorkPath);
            //        if (!directory.Exists) directory.Create();
            //        Default.WorkPath = directory.fullName().fileNameToLower();
            //    }
            //    catch (Exception error)
            //    {
            //        AutoCSer.Log.Pub.Log.add(Log.Type.Error, error);
            //        Default.WorkPath = AutoCSer.PubPath.ApplicationPath;
            //    }
            //}
            if (Default.CachePath != AutoCSer.PubPath.ApplicationPath)
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(Default.CachePath);
                    if (!directory.Exists) directory.Create();
                    Default.CachePath = directory.fullName().fileNameToLower();
                }
                catch (Exception error)
                {
                    AutoCSer.Log.Trace.Console(error.ToString());
                    Default.CachePath = AutoCSer.PubPath.ApplicationPath;
                }
            }
        }
#endif
    }
}

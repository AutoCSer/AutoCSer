using System;
using System.Text;

namespace AutoCSer.WebView
{
    /// <summary>
    /// 网站生成配置
    /// </summary>
    public abstract class Config
    {
        /// <summary>
        /// 文件编码
        /// </summary>
        public virtual Encoding Encoding
        {
            get { return AutoCSer.Config.Pub.Default.Encoding; }
        }
        /// <summary>
        /// Session类型
        /// </summary>
        public virtual Type SessionType
        {
            get { return typeof(int); }
        }
        /// <summary>
        /// 默认主域名
        /// </summary>
        public virtual string MainDomain { get { return "127.0.0.1"; } }
        /// <summary>
        /// TCP 服务主机与端口信息
        /// </summary>
        public AutoCSer.Net.HostPort MainHostPort
        {
            get { return getHostPort(MainDomain); }
        }
        /// <summary>
        /// 静态文件网站域名，用于在浏览器客户端永久性缓存的资源
        /// </summary>
        public virtual string StaticFileDomain { get { return MainDomain; } }
        /// <summary>
        /// 图片文件域名
        /// </summary>
        public virtual string ImageDomain { get { return StaticFileDomain; } }
        /// <summary>
        /// 轮询网站域名
        /// </summary>
        public virtual string PollDomain { get { return MainDomain; } }
        /// <summary>
        /// 视图加载失败重定向，默认为 "/"
        /// </summary>
        public virtual string NoViewLocation { get { return "/"; } }
        /// <summary>
        /// 是否对 css 缺省域名替换为静态文件域名，默认为 true
        /// </summary>
        public virtual bool IsCssStaticFileDomain
        {
            get { return true; }
        }
        /// <summary>
        /// 是否进行WebView前期处理，默认为 true（格式化HTML、CSS）
        /// </summary>
        public virtual bool IsWebView
        {
            get { return true; }
        }
        /// <summary>
        /// 是否复制 js 脚本文件，默认为 true
        /// </summary>
        public virtual bool IsCopyScript
        {
            get { return true; }
        }
        /// <summary>
        /// WEB Path 导出引导类型
        /// </summary>
        public virtual Type ExportPathType
        {
            get { return null; }
        }
        /// <summary>
        /// WEB视图扩展默认目录名称，默认为 viewJs
        /// </summary>
        public virtual string ViewJsDirectory
        {
            get { return "viewJs"; }
        }
        /// <summary>
        /// 附加导入 HTML/JS 目录集合
        /// </summary>
        public virtual string[] IncludeDirectories { get { return null; } }
        /// <summary>
        /// 默认为 true 表示导出 TypeScript，否则导出 JavaScript
        /// </summary>
        public virtual bool IsExportPathTypeScript
        {
            get { return true; }
        }
        /// <summary>
        /// 本地 HTML 文件链接是否添加版本号，默认为 false
        /// </summary>
        public virtual bool IsHtmlLinkVersion { get { return false; } }
#if MONO
        /// <summary>
        /// 是否忽略大小写
        /// </summary>
        internal bool IgnoreCase;
#else
        /// <summary>
        /// 是否忽略大小写，默认为 false
        /// </summary>
        public virtual bool IgnoreCase
        {
            get { return false; }
        }
#endif
        /// <summary>
        /// 默认为 true 表示 GZip 启用快速压缩，否则使用默认压缩
        /// </summary>
        public virtual bool IsFastestCompressionLevel
        {
            get { return true; }
        }
        /// <summary>
        /// TCP 服务主机与端口信息
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        private static AutoCSer.Net.HostPort getHostPort(string domain)
        {
            int index = domain.LastIndexOf(':');
            if (index != -1)
            {
                ushort port;
                if (ushort.TryParse(domain.Substring(index + 1), out port)) return new AutoCSer.Net.HostPort { Host = domain.Substring(0, index), Port = (int)(uint)port };
            }
            return new AutoCSer.Net.HostPort { Host = domain, Port = 80 };
        }
        /// <summary>
        /// 默认网站生成配置
        /// </summary>
        internal sealed class Null : Config
        {
            /// <summary>
            /// 网站生成配置
            /// </summary>
            internal static readonly Null Default = new Null();
        }
    }
}

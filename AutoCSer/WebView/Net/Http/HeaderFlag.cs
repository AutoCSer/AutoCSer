using System;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 头部标志位
    /// </summary>
    [Flags]
    public enum HeaderFlag : uint
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 是否安全连接
        /// </summary>
        IsSsl = 1,
        /// <summary>
        /// 是否保持连接状态
        /// </summary>
        IsKeepAlive = 2,
        /// <summary>
        /// 客户端是否支持 GZip 压缩
        /// </summary>
        IsGZip = 4,
        /// <summary>
        /// 是否设置 HTTP 请求内容类型
        /// </summary>
        IsSetContentType = 8,
        /// <summary>
        /// 是否设置 Cookie
        /// </summary>
        IsSetCookie = 0x10,
        /// <summary>
        /// 是否设置访问来源
        /// </summary>
        IsSetReferer = 0x20,
        /// <summary>
        /// 是否设置浏览器参数
        /// </summary>
        IsSetUserAgent = 0x40,
        /// <summary>
        /// 是否设置请求编码
        /// </summary>
        IsSetRequestEncoding = 0x80,
        /// <summary>
        /// 是否设置客户端文档时间标识
        /// </summary>
        IsSetIfModifiedSince = 0x100,
        /// <summary>
        /// 是否设置客户端缓存有效标识
        /// </summary>
        IsSetIfNoneMatch = 0x200,
        /// <summary>
        /// 是否设置转发信息
        /// </summary>
        IsSetXProwardedFor = 0x400,
        /// <summary>
        /// 是否 100 Continue 确认
        /// </summary>
        Is100Continue = 0x800,
        /// <summary>
        /// 是否设置提交数据分隔符
        /// </summary>
        IsSetBoundary = 0x1000,
        /// <summary>
        /// 是否设置访问来源
        /// </summary>
        IsSetOrigin = 0x2000,
        /// <summary>
        /// 是否设置来源页是否合法
        /// </summary>
        IsSetIsReferer = 0x4000,
        /// <summary>
        /// 来源页是否合法
        /// </summary>
        IsReferer = 0x8000,
        /// <summary>
        /// 是否存在请求范围
        /// </summary>
        IsRange = 0x10000,
        /// <summary>
        /// 是否已经格式化请求范围
        /// </summary>
        IsFormatRange = 0x20000,
        /// <summary>
        /// 请求范围是否错误
        /// </summary>
        IsRangeError = 0x40000,
        /// <summary>
        /// 是否 ajax 请求
        /// </summary>
        IsAjax = 0x80000,
        /// <summary>
        /// URL中是否包含 #
        /// </summary>
        IsHash = 0x100000,
        /// <summary>
        /// 是否设置 URL中是否包含 #
        /// </summary>
        IsSetHash = 0x200000,
        /// <summary>
        /// 是否设置搜索引擎
        /// </summary>
        IsSearchEngine = 0x400000,
        /// <summary>
        /// 是否设置搜索引擎
        /// </summary>
        IsSetSearchEngine = 0x800000,
        /// <summary>
        /// 是否设置查询 Json 字符串
        /// </summary>
        IsSetQueryJson = 0x1000000,
        /// <summary>
        /// 是否设置查询 XML 字符串
        /// </summary>
        IsSetQueryXml = 0x2000000,
        /// <summary>
        /// 是否重新加载视图
        /// </summary>
        IsReView = 0x4000000,
        /// <summary>
        /// 是否重新加载视图（忽略主列表）
        /// </summary>
        IsMobileReView = 0x8000000,
        /// <summary>
        /// 是否第一次加载页面缓存
        /// </summary>
        IsLoadPageCache = 0x10000000,
        /// <summary>
        /// 是否设置 AJAX 调用函数名称
        /// </summary>
        IsSetAjaxCallName = 0x20000000,
        /// <summary>
        /// 是否设置 AJAX 回调函数名称
        /// </summary>
        IsSetAjaxCallBackName = 0x40000000,
        /// <summary>
        /// 是否设置 URL 资源版本查询名称
        /// </summary>
        IsVersion = 0x80000000,
        /// <summary>
        /// 所有标志位
        /// </summary>
        All = 0xffffffffU,
    }
}

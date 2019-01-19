using System;
using AutoCSer.Extension;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 头部
    /// </summary>
    public abstract unsafe class Header
    {
        /// <summary>
        /// 套接字
        /// </summary>
        internal System.Net.Sockets.Socket socket;
        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull Buffer;
        /// <summary>
        /// 接收数据起始位置
        /// </summary>
        protected int receiveIndex;
        /// <summary>
        /// 接收数据字节数
        /// </summary>
        protected int receiveCount;
        /// <summary>
        /// HTTP 头部结束位置
        /// </summary>
        internal int HeaderEndIndex;
        /// <summary>
        /// HTTP 头部标志位
        /// </summary>
        internal HeaderFlag Flag;
        /// <summary>
        /// HTTP 头部标志位
        /// </summary>
        internal HeaderWebSocketFlag WebSocketFlag;
        /// <summary>
        /// 请求域名
        /// </summary>
        internal BufferIndex HostIndex;
        /// <summary>
        /// 请求域名
        /// </summary>
        internal SubArray<byte> Host
        {
            get { return new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + HostIndex.StartIndex, Length = HostIndex.Length }; }
        }
        /// <summary>
        /// 请求URI
        /// </summary>
        internal BufferIndex UriIndex;
        /// <summary>
        /// 请求路径
        /// </summary>
        internal BufferIndex PathIndex;
        /// <summary>
        /// 请求路径
        /// </summary>
        internal SubArray<byte> Path
        {
            get { return new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + PathIndex.StartIndex, Length = PathIndex.Length }; }
        }
        /// <summary>
        /// Cookie
        /// </summary>
        private BufferIndex cookie;
        /// <summary>
        /// 提交数据分隔符
        /// </summary>
        internal BufferIndex BoundaryIndex;
        /// <summary>
        /// HTTP 请求内容类型
        /// </summary>
        private BufferIndex contentType;
        /// <summary>
        /// 访问来源
        /// </summary>
        private BufferIndex referer;
        /// <summary>
        /// 访问来源
        /// </summary>
        internal SubArray<byte> Referer
        {
            get { return new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + referer.StartIndex, Length = referer.Length }; }
        }
        /// <summary>
        /// 浏览器参数
        /// </summary>
        private BufferIndex userAgent;
        /// <summary>
        /// 客户端文档时间标识
        /// </summary>
        internal BufferIndex IfModifiedSinceIndex;
        /// <summary>
        /// 客户端文档时间标识
        /// </summary>
        internal SubArray<byte> IfModifiedSince
        {
            get { return new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + IfModifiedSinceIndex.StartIndex, Length = IfModifiedSinceIndex.Length }; }
        }
        /// <summary>
        /// 客户端缓存有效标识
        /// </summary>
        private BufferIndex ifNoneMatch;
        /// <summary>
        /// 转发信息
        /// </summary>
        private BufferIndex xProwardedFor;
        /// <summary>
        /// WebSocket 确认连接值
        /// </summary>
        private BufferIndex secWebSocketKey;
        /// <summary>
        /// 访问来源
        /// </summary>
        internal BufferIndex webSocketOrigin;
        /// <summary>
        /// 访问来源
        /// </summary>
        internal BufferIndex OriginIndex;
        /// <summary>
        /// 访问来源
        /// </summary>
        internal SubArray<byte> Origin
        {
            get { return new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + OriginIndex.StartIndex, Length = OriginIndex.Length }; }
        }
        /// <summary>
        /// AJAX 调用函数名称
        /// </summary>
        internal BufferIndex AjaxCallNameIndex;
        /// <summary>
        /// AJAX 调用函数名称
        /// </summary>
        internal SubArray<byte> AjaxCallNameData
        {
            get { return new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + AjaxCallNameIndex.StartIndex, Length = AjaxCallNameIndex.Length }; }
        }
        /// <summary>
        /// AJAX 回调函数名称
        /// </summary>
        internal BufferIndex AjaxCallBackNameIndex;
        /// <summary>
        /// AJAX 回调函数名称
        /// </summary>
        internal SubArray<byte> AjaxCallBackName
        {
            get { return new SubArray<byte> { Array = Buffer.Buffer, Start = Buffer.StartIndex + AjaxCallBackNameIndex.StartIndex, Length = AjaxCallBackNameIndex.Length }; }
        }
        /// <summary>
        /// 查询 Json 字符串
        /// </summary>
        private BufferIndex queryJson;
        /// <summary>
        /// Json字符串
        /// </summary>
        internal string QueryJson
        {
            get
            {
                if ((Flag & HeaderFlag.IsSetQueryJson) != 0 && queryJson.Length != 0)
                {
                    int index = Buffer.StartIndex + queryJson.StartIndex;
                    fixed (byte* bufferFixed = Buffer.Buffer) return UnescapeUtf8(bufferFixed + index, queryJson.Length, Buffer.Buffer, index);
                }
                return null;
            }
        }
        /// <summary>
        /// 查询 XML 字符串
        /// </summary>
        private BufferIndex queryXml;
        /// <summary>
        /// XML字符串
        /// </summary>
        internal string QueryXml
        {
            get
            {
                if ((Flag & HeaderFlag.IsSetQueryXml) != 0 && queryXml.Length != 0)
                {
                    int index = Buffer.StartIndex + queryXml.StartIndex;
                    fixed (byte* bufferFixed = Buffer.Buffer) return UnescapeUtf8(bufferFixed + index, queryXml.Length, Buffer.Buffer, index);
                }
                return null;
            }
        }
        /// <summary>
        /// 请求内容编码
        /// </summary>
        private EncodingCache requestEncoding;
        /// <summary>
        /// 请求内容编码
        /// </summary>
        internal EncodingCache RequestEncoding
        {
            get { return (Flag & HeaderFlag.IsSetRequestEncoding) == 0 ? EncodingCache.Utf8 : requestEncoding; }
        }
        /// <summary>
        /// WebSocket 数据
        /// </summary>
        internal SubString WebSocketData;
        /// <summary>
        /// 是否保持连接状态
        /// </summary>
        internal HeaderFlag IsKeepAlive
        {
            get { return Flag & HeaderFlag.IsKeepAlive; }
        }
        /// <summary>
        /// 客户端是否支持 GZip 压缩
        /// </summary>
        internal HeaderFlag IsGZip
        {
            get { return Flag & HeaderFlag.IsGZip; }
        }
        /// <summary>
        /// 是否存在请求范围
        /// </summary>
        internal HeaderFlag IsRange
        {
            get { return Flag & HeaderFlag.IsRange; }
        }
        /// <summary>
        /// 是否已经格式化请求范围
        /// </summary>
        internal HeaderFlag IsFormatRange
        {
            get { return Flag & HeaderFlag.IsFormatRange; }
        }
        /// <summary>
        /// 是否设置提交数据分隔符
        /// </summary>
        internal HeaderFlag IsBoundary
        {
            get { return Flag & HeaderFlag.IsSetBoundary; }
        }
        /// <summary>
        /// 是否 ajax 请求
        /// </summary>
        internal HeaderFlag IsAjax
        {
            get { return Flag & HeaderFlag.IsAjax; }
        }
        /// <summary>
        /// 是否搜索引擎
        /// </summary>
        [AutoCSer.WebView.OutputAjax(IsIgnoreCurrent = true)]
        internal HeaderFlag IsSearchEngine
        {
            get
            {
                if ((Flag & HeaderFlag.IsSetSearchEngine) == 0)
                {
                    if (userAgent.Length >= 5 && checkSearchEngine())
                    {
                        Flag |= HeaderFlag.IsSetSearchEngine | HeaderFlag.IsSearchEngine;
                        return HeaderFlag.IsSearchEngine;
                    }
                    Flag |= HeaderFlag.IsSetSearchEngine;
                    return HeaderFlag.None;
                }
                return Flag & HeaderFlag.IsSearchEngine;
            }
        }
        /// <summary>
        /// 判断来源页是否合法
        /// </summary>
        internal HeaderFlag IsReferer
        {
            get
            {
                if ((Flag & HeaderFlag.IsSetIsReferer) == 0)
                {
                    if (HostIndex.Length != 0)
                    {
                        if ((Flag & HeaderFlag.IsSetReferer) != 0)
                        {
                            SubArray<byte> domain = DomainParser.GetMainDomainByUrl(Referer);
                            if (DomainParser.GetMainDomain(Host).equal(ref domain)) Flag |= HeaderFlag.IsReferer;
                        }
                        else if ((Flag & HeaderFlag.IsSetOrigin) != 0)
                        {
                            SubArray<byte> domain = DomainParser.GetMainDomainByUrl(Origin);
                            if (DomainParser.GetMainDomain(Host).equal(ref domain)) Flag |= HeaderFlag.IsReferer;
                        }
                    }
                    Flag |= HeaderFlag.IsSetIsReferer;
                }
                return Flag & HeaderFlag.IsReferer;
            }
        }
        /// <summary>
        /// 是否 100 Continue 确认
        /// </summary>
        internal HeaderFlag Is100Continue
        {
            get
            {
                HeaderFlag flag = Flag & HeaderFlag.Is100Continue;
                Flag &= HeaderFlag.All ^ HeaderFlag.Is100Continue;
                return flag;
            }
        }
        ///// <summary>
        ///// URL中的#!是否需要转换成?
        ///// </summary>
        //internal bool IsHashQueryUri
        //{
        //    get
        //    {
        //        return (Flag & (HeaderFlag.IsSetHash | HeaderFlag.IsHash)) != (HeaderFlag.IsSetHash | HeaderFlag.IsHash) && IsSearchEngine != 0;
        //    }
        //}
        /// <summary>
        /// 是否 WebSocket 连接
        /// </summary>
        internal bool IsWebSocket
        {
            get { return (WebSocketFlag & HeaderWebSocketFlag.IsWebSocket) == HeaderWebSocketFlag.IsWebSocket; }
        }
        /// <summary>
        /// HTTP头部名称数据
        /// </summary>
        internal int HeaderCount;
        /// <summary>
        /// 查询参数数量
        /// </summary>
        internal int QueryCount;
        /// <summary>
        /// 请求内容字节长度
        /// </summary>
        internal int ContentLength;
        /// <summary>
        /// 请求范围起始位置
        /// </summary>
        internal long RangeStart;
        /// <summary>
        /// 请求范围结束位置
        /// </summary>
        internal long RangeEnd;
        /// <summary>
        /// 请求范围长度
        /// </summary>
        internal long RangeSize
        {
            get { return RangeEnd - RangeStart + 1; }
        }
        /// <summary>
        /// 查询模式类型
        /// </summary>
        internal MethodType Method;
        /// <summary>
        /// 提交数据类型
        /// </summary>
        internal PostType PostType;
        /// <summary>
        /// 是否第二次接收数据
        /// </summary>
        internal bool IsKeepAliveReceive;
        /// <summary>
        /// 接收数据量过低次数
        /// </summary>
        internal byte ReceiveSizeLessCount;
        /// <summary>
        /// 格式化请求范围
        /// </summary>
        /// <param name="contentLength">内容字节长度</param>
        /// <returns>范围是否有效</returns>
        internal bool FormatRange(long contentLength)
        {
            Flag |= HeaderFlag.IsFormatRange;
            if (contentLength == 0)
            {
                RangeStart = RangeEnd = long.MinValue;
                Flag &= HeaderFlag.All ^ HeaderFlag.IsRange;
            }
            else if (RangeStart == 0)
            {
                if (RangeEnd >= contentLength - 1 || RangeEnd < 0) RangeEnd = contentLength - 1;
            }
            else if (RangeStart > 0)
            {
                if (RangeStart >= contentLength || (ulong)RangeEnd < (ulong)RangeStart)
                {
                    Flag |= HeaderFlag.IsRangeError;
                    return false;
                }
                if (RangeEnd >= contentLength || RangeEnd < 0) RangeEnd = contentLength - 1;
            }
            else if (RangeEnd >= 0)
            {
                RangeStart = 0;
                if (RangeEnd >= contentLength) RangeEnd = contentLength - 1;
            }
            return true;
        }
        /// <summary>
        /// HTTP 头部
        /// </summary>
        protected Header()
        {
            BufferPool.Get(ref Buffer);
        }
        /// <summary>
        /// HTTP头部解析
        /// </summary>
        /// <returns>是否成功</returns>
        protected bool parse()
        {
            HostIndex.Value = 0;
            Flag &= isKeepAliveDomainServer ? HeaderFlag.IsKeepAlive | HeaderFlag.IsSsl | HeaderFlag.IsGZip : (HeaderFlag.IsKeepAlive | HeaderFlag.IsSsl);
            WebSocketFlag = HeaderWebSocketFlag.None;
            fixed (byte* bufferFixed = Buffer.Buffer)
            {
                byte* bufferStart = bufferFixed + Buffer.StartIndex, current = bufferStart + receiveIndex;
                if ((Method = GetMethod(current)) == MethodType.Unknown) return false;
                byte* end = bufferStart + HeaderEndIndex;
                for (*end = 32; *current != 32; ++current) ;
                *end = 13;
                if (current == end) return false;
                while (*++current == 32) ;
                if (current == end) return false;
                byte* start = current;
                while (*current != 32 && *current != 13) ++current;
                UriIndex.Set(start - bufferStart, current - start);
                if (UriIndex.Length == 0) return false;
                while (*current != 13) ++current;

                byte* headerIndex = bufferStart + NameStartIndex;
                HeaderCount = ContentLength = 0;
                PostType = PostType.None;
                while (current != end)
                {
                    if ((current += 2) >= end) return false;
                    byte* nameStart = current;
                    for (*end = (byte)':'; *current != (byte)':'; ++current) ;
                    int nameSize = (int)(current - nameStart), isParse = 0;
                    //subArray<byte> name = subArray<byte>.Unsafe(Buffer, (int)(start - bufferStart), (int)(current - start));
                    *end = 13;
                    if (current == end || *++current != ' ') return false;
                    for (start = ++current; *current != 13; ++current) ;
                    //Action<requestHeader, bufferIndex> parseHeaderName = parses.Get(name, null);
                    switch ((((*(nameStart + (nameSize >> 1)) | 0x20) >> 2) ^ (*(nameStart + nameSize - 3) << 1)) & ((1 << 5) - 1))
                    {
                        case (('s' >> 2) ^ ('o' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 4) | ((*(int*)nameStart | 0x20202020) ^ ('h' + ('o' << 8) + ('s' << 16) + ('t' << 24)))) == 0)
                            {
                                HostIndex.Set(start - bufferStart, current - start);
                                //flag |= HeaderFlag.IsSetHost;
                                isParse = 1;
                            }
                            break;
                        case ((('-' | 0x20) >> 2) ^ ('g' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 14) | ((*(int*)nameStart | 0x20202020) ^ ('c' + ('o' << 8) + ('n' << 16) + ('t' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x202020) ^ ('e' + ('n' << 8) + ('t' << 16) + ('-' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 2) | 0x20202020) ^ ('l' + ('e' << 8) + ('n' << 16) + ('g' << 24)))
                                | ((*(short*)(nameStart + sizeof(int) * 3) | 0x2020) ^ ('t' + ('h' << 8)))) == 0)
                            {
                                while (start != current)
                                {
                                    ContentLength *= 10;
                                    ContentLength += *start++ - '0';
                                }
                                if (ContentLength < 0) return false;
                                //flag |= HeaderFlag.IsSetContentLength;
                                isParse = 1;
                            }
                            break;
                        case (('e' >> 2) ^ ('i' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 15) | ((*(int*)nameStart | 0x20202020) ^ ('a' + ('c' << 8) + ('c' << 16) + ('e' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x20002020) ^ ('p' + ('t' << 8) + ('-' << 16) + ('e' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 2) | 0x20202020) ^ ('n' + ('c' << 8) + ('o' << 16) + ('d' << 24)))
                                | (int)((*(uint*)(nameStart + sizeof(int) * 3) | 0xff202020U) ^ ('i' + ('n' << 8) + ('g' << 16) + 0xff000000U))) == 0)
                            {
                                if (current - start >= 4)
                                {
                                    *current = (byte)'g';
                                    while (true)
                                    {
                                        while (*start != 'g') ++start;
                                        if (start != current)
                                        {
                                            if ((*(int*)start | 0x20202020) == ('g' | ('z' << 8) | ('i' << 16) | ('p' << 24)))
                                            {
                                                Flag |= HeaderFlag.IsGZip;
                                                break;
                                            }
                                            else ++start;
                                        }
                                        else break;
                                    }
                                    *current = 13;
                                }
                                isParse = 1;
                            }
                            break;
                        case (('c' >> 2) ^ ('i' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 10) | ((*(int*)nameStart | 0x20202020) ^ ('c' + ('o' << 8) + ('n' << 16) + ('n' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x20202020) ^ ('e' + ('c' << 8) + ('t' << 16) + ('i' << 24)))
                                | ((*(short*)(nameStart + sizeof(int) * 2) | 0x2020) ^ ('o' + ('n' << 8)))) == 0)
                            {
                                switch ((int)(current - start) - 5)
                                {
                                    case 5 - 5:
                                        if ((((*(int*)start | 0x20202020) ^ ('c' | ('l' << 8) | ('o' << 16) | ('s' << 24)))
                                            | ((*(start + sizeof(int)) | 0x20) ^ 'e')) == 0)
                                        {
                                            Flag &= (HeaderFlag.All ^ HeaderFlag.IsKeepAlive);
                                        }
                                        break;
                                    case 7 - 5:
                                        if ((((*(int*)start | 0x20202020) ^ ('u' | ('p' << 8) | ('g' << 16) | ('r' << 24)))
                                            | ((*(int*)(start + sizeof(int)) | 0x202020) ^ ('a' | ('d' << 8) | ('e' << 16) | 0xd000000))) == 0)
                                        {
                                            WebSocketFlag |= HeaderWebSocketFlag.IsConnectionUpgrade;
                                        }
                                        break;
                                    case 10 - 5:
                                        if ((((*(int*)start | 0x20202020) ^ ('k' | ('e' << 8) | ('e' << 16) | ('p' << 24)))
                                            | ((*(int*)(start + sizeof(int)) | 0x20202000) ^ ('-' | ('a' << 8) | ('l' << 16) | ('i' << 24)))
                                            | ((*(short*)(start + sizeof(int) * 2) | 0x2020) ^ ('v' | ('e' << 8)))) == 0)
                                        {
                                            Flag |= HeaderFlag.IsKeepAlive;
                                        }
                                        break;
                                }
                                isParse = 1;
                            }
                            break;
                        case (('t' >> 2) ^ ('y' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 12) | ((*(int*)nameStart | 0x20202020) ^ ('c' + ('o' << 8) + ('n' << 16) + ('t' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x202020) ^ ('e' + ('n' << 8) + ('t' << 16) + ('-' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 2) | 0x20202020) ^ ('t' + ('y' << 8) + ('p' << 16) + ('e' << 24)))) == 0)
                            {
                                contentType.Set(start - bufferStart, current - start);
                                if (contentType.Length == 0) Flag &= HeaderFlag.All ^ HeaderFlag.IsSetContentType;
                                else Flag |= HeaderFlag.IsSetContentType;
                                isParse = 1;
                            }
                            break;
                        case (('k' >> 2) ^ ('k' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 6) | ((*(int*)nameStart | 0x20202020) ^ ('c' + ('o' << 8) + ('o' << 16) + ('k' << 24)))
                                | ((*(short*)(nameStart + sizeof(int)) | 0x2020) ^ ('i' + ('e' << 8)))) == 0)
                            {
                                cookie.Set(start - bufferStart, current - start);
                                if (cookie.Length == 0) Flag &= HeaderFlag.All ^ HeaderFlag.IsSetCookie;
                                else Flag |= HeaderFlag.IsSetCookie;
                                isParse = 1;
                            }
                            break;
                        case (('e' >> 2) ^ ('r' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 7) | ((*(int*)nameStart | 0x20202020) ^ ('r' + ('e' << 8) + ('f' << 16) + ('e' << 24)))
                                | (int)((*(uint*)(nameStart + sizeof(int)) | 0xff202020U) ^ ('r' + ('e' << 8) + ('r' << 16) + 0xff000000U))) == 0)
                            {
                                referer.Set(start - bufferStart, current - start);
                                if (referer.Length == 0) Flag &= HeaderFlag.All ^ HeaderFlag.IsSetReferer;
                                else Flag |= HeaderFlag.IsSetReferer;
                                isParse = 1;
                            }
                            break;
                        case (('n' >> 2) ^ ('n' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 5) | ((*(int*)nameStart | 0x20202020) ^ ('r' + ('a' << 8) + ('n' << 16) + ('g' << 24)))
                                | ((*(nameStart + sizeof(int)) | 0x20) ^ 'e')) == 0)
                            {
                                if ((int)(current - start) > 6 && ((*(int*)start ^ ('b' + ('y' << 8) + ('t' << 16) + ('e' << 24))) | (*(short*)(start + 4) ^ ('s' + ('=' << 8)))) == 0)
                                {
                                    parseRange(start, current);
                                    if ((Flag & HeaderFlag.IsRange) == 0)
                                    {
                                        Flag |= HeaderFlag.IsRangeError;
                                        return false;
                                    }
                                }
                                isParse = 1;
                            }
                            break;
                        case (('a' >> 2) ^ ('e' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 10) | ((*(int*)nameStart | 0x20202020) ^ ('u' + ('s' << 8) + ('e' << 16) + ('r' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x20202000) ^ ('-' + ('a' << 8) + ('g' << 16) + ('e' << 24)))
                                | ((*(short*)(nameStart + sizeof(int) * 2) | 0x2020) ^ ('n' + ('t' << 8)))) == 0)
                            {
                                userAgent.Set(start - bufferStart, current - start);
                                if (userAgent.Length == 0) Flag &= HeaderFlag.All ^ HeaderFlag.IsSetUserAgent;
                                else Flag |= HeaderFlag.IsSetUserAgent;
                                isParse = 1;
                            }
                            break;
                        case (('i' >> 2) ^ ('n' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 17) | ((*(int*)nameStart | 0x20002020) ^ ('i' + ('f' << 8) + ('-' << 16) + ('m' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x20202020) ^ ('o' + ('d' << 8) + ('i' << 16) + ('f' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 2) | 0x202020) ^ ('i' + ('e' << 8) + ('d' << 16) + ('-' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 3) | 0x20202020) ^ ('s' + ('i' << 8) + ('n' << 16) + ('c' << 24)))
                                | ((*(nameStart + sizeof(int) * 4) | 0x20) ^ 'e')) == 0)
                            {
                                IfModifiedSinceIndex.Set(start - bufferStart, current - start);
                                if (IfModifiedSinceIndex.Length == 0) Flag &= HeaderFlag.All ^ HeaderFlag.IsSetIfModifiedSince;
                                else Flag |= HeaderFlag.IsSetIfModifiedSince;
                                isParse = 1;
                            }
                            break;
                        case (('e' >> 2) ^ ('t' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 13) | ((*(int*)nameStart | 0x20002020) ^ ('i' + ('f' << 8) + ('-' << 16) + ('n' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x202020) ^ ('o' + ('n' << 8) + ('e' << 16) + ('-' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 2) | 0x20202020) ^ ('m' + ('a' << 8) + ('t' << 16) + ('c' << 24)))
                                | ((*(nameStart + sizeof(int) * 3) | 0x20) ^ 'h')) == 0)
                            {
                                if (*(current - 1) == '"' && (nameSize = (int)(current - start)) >= 3)
                                {
                                    if (*start == '"')
                                    {
                                        ifNoneMatch.Set((int)(start - bufferStart) + 1, nameSize - 2);
                                        Flag |= HeaderFlag.IsSetIfNoneMatch;
                                    }
                                    else if ((*(int*)start & 0xffffff) == ('W' + ('/' << 8) + ('"' << 16)) && nameSize >= 5)
                                    {
                                        ifNoneMatch.Set(((int)(start - bufferStart) + 3), nameSize - 4);
                                        Flag |= HeaderFlag.IsSetIfNoneMatch;
                                    }
                                }
                                isParse = 1;
                            }
                            break;
                        case (('r' >> 2) ^ ('F' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 15) | ((*(int*)nameStart | 0x20200020) ^ ('x' + ('-' << 8) + ('p' << 16) + ('r' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x20202020) ^ ('o' + ('w' << 8) + ('a' << 16) + ('r' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 2) | 0x202020) ^ ('d' + ('e' << 8) + ('d' << 16) + ('-' << 24)))
                                | (int)((*(uint*)(nameStart + sizeof(int) * 3) | 0xff202020U) ^ ('f' + ('o' << 8) + ('r' << 16) + 0xff000000U))) == 0)
                            {
                                xProwardedFor.Set(start - bufferStart, current - start);
                                if (xProwardedFor.Length == 0) Flag &= HeaderFlag.All ^ HeaderFlag.IsSetXProwardedFor;
                                else Flag |= HeaderFlag.IsSetXProwardedFor;
                                isParse = 1;
                            }
                            break;
                        case (('e' >> 2) ^ ('e' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 6) | ((*(int*)nameStart | 0x20202020) ^ ('e' + ('x' << 8) + ('p' << 16) + ('e' << 24)))
                                | ((*(short*)(nameStart + sizeof(int)) | 0x2020) ^ ('c' + ('t' << 8)))) == 0)
                            {
                                if ((int)(current - start) == 12)
                                {
                                    if (((*(int*)start ^ ('1' | ('0' << 8) | ('0' << 16) | ('-' << 24)))
                                        | ((*(int*)(start + sizeof(int)) | 0x20202020) ^ ('c' | ('o' << 8) | ('n' << 16) | ('t' << 24)))
                                        | ((*(int*)(start + sizeof(int) * 2) | 0x20202020) ^ ('i' | ('n' << 8) | ('u' << 16) | ('e' << 24)))) == 0)
                                    {
                                        Flag |= HeaderFlag.Is100Continue;
                                    }
                                }
                                isParse = 1;
                            }
                            break;
                        case (('r' >> 2) ^ ('a' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 7) | ((*(int*)nameStart | 0x20202020) ^ ('u' + ('p' << 8) + ('g' << 16) + ('r' << 24)))
                                | (int)((*(uint*)(nameStart + sizeof(int)) | 0xff202020U) ^ ('a' + ('d' << 8) + ('e' << 16) + 0xff000000U))) == 0)
                            {
                                if ((int)(current - start) == 9)
                                {
                                    if ((((*(int*)start | 0x20202020) ^ ('w' | ('e' << 8) | ('b' << 16) | ('s' << 24)))
                                        | ((*(int*)(start + sizeof(int)) | 0x20202020) ^ ('o' | ('c' << 8) | ('k' << 16) | ('e' << 24)))
                                        | ((*(start + sizeof(int) * 2) | 0x20) ^ 't')) == 0)
                                    {
                                        WebSocketFlag |= HeaderWebSocketFlag.IsUpgrade;
                                    }
                                }
                                isParse = 1;
                            }
                            break;
                        case (('o' >> 2) ^ ('K' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 17) | ((*(int*)nameStart | 0x202020) ^ ('s' + ('e' << 8) + ('c' << 16) + ('-' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x20202020) ^ ('w' + ('e' << 8) + ('b' << 16) + ('s' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 2) | 0x20202020) ^ ('o' + ('c' << 8) + ('k' << 16) + ('e' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 3) | 0x20200020) ^ ('t' + ('-' << 8) + ('k' << 16) + ('e' << 24)))
                                | ((*(nameStart + sizeof(int) * 4) | 0x20) ^ 'y')) == 0)
                            {
                                int keySize = (int)(current - start);
                                if (keySize != 0 && keySize <= 32)
                                {
                                    secWebSocketKey.Set((short)(start - bufferStart), (short)keySize);
                                    WebSocketFlag |= HeaderWebSocketFlag.IsSetSecKey;
                                }
                                isParse = 1;
                            }
                            break;
                        case (('k' >> 2) ^ ('g' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 20) | ((*(int*)nameStart | 0x202020) ^ ('s' + ('e' << 8) + ('c' << 16) + ('-' << 24)))
                                | ((*(int*)(nameStart + sizeof(int)) | 0x20202020) ^ ('w' + ('e' << 8) + ('b' << 16) + ('s' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 2) | 0x20202020) ^ ('o' + ('c' << 8) + ('k' << 16) + ('e' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 3) | 0x20200020) ^ ('t' + ('-' << 8) + ('o' << 16) + ('r' << 24)))
                                | ((*(int*)(nameStart + sizeof(int) * 4) | 0x20202020) ^ ('i' + ('g' << 8) + ('i' << 16) + ('n' << 24)))) == 0)
                            {
                                webSocketOrigin.Set(start - bufferStart, current - start);
                                if (webSocketOrigin.Length == 0) WebSocketFlag &= HeaderWebSocketFlag.All ^ HeaderWebSocketFlag.IsSetOrigin;
                                else WebSocketFlag |= HeaderWebSocketFlag.IsSetOrigin;
                                isParse = 1;
                            }
                            break;
                        case (('g' >> 2) ^ ('g' << 1)) & ((1 << 5) - 1):
                            if (((nameSize ^ 6) | ((*(int*)nameStart | 0x20202020) ^ ('o' + ('r' << 8) + ('i' << 16) + ('g' << 24)))
                                | ((*(short*)(nameStart + sizeof(int)) | 0x2020) ^ ('i' + ('n' << 8)))) == 0)
                            {
                                OriginIndex.Set(start - bufferStart, current - start);
                                if (OriginIndex.Length == 0) Flag &= HeaderFlag.All ^ HeaderFlag.IsSetOrigin;
                                else Flag |= HeaderFlag.IsSetOrigin;
                                isParse = 1;
                            }
                            break;
                    }
                    if (isParse == 0 && HeaderCount != maxHeaderCount)
                    {
                        (*(BufferIndex*)headerIndex).Set((int)(nameStart - bufferStart), nameSize);
                        (*(BufferIndex*)(headerIndex + sizeof(BufferIndex))).Set(start - bufferStart, current - start);
                        ++HeaderCount;
                        headerIndex += sizeof(BufferIndex) * 2;
                    }
                }
                if (HostIndex.Length == 0) return false;
                if ((WebSocketFlag & HeaderWebSocketFlag.IsWebSocket) == HeaderWebSocketFlag.IsWebSocket)
                {
                    if (Method != MethodType.GET || (Flag & HeaderFlag.IsSetIfModifiedSince) != 0) return false;
                }
                if ((Flag & HeaderFlag.IsSetContentType) != 0 && contentType.Length != 0)
                {
                    start = bufferStart + contentType.StartIndex;
                    end = start + contentType.Length;
                    current = AutoCSer.Memory.Find(start, end, (byte)';');
                    switch ((current == null ? contentType.Length : (int)(current - start)) - 8)
                    {
                        case 8 - 8://text/xml; charset=utf-8
                            if ((((*(int*)start | 0x20202020) ^ ('t' | ('e' << 8) | ('x' << 16) | ('t' << 24)))
                                | ((*(int*)(start + sizeof(int)) | 0x20202000) ^ ('/' | ('x' << 8) | ('m' << 16) | ('l' << 24)))) == 0)
                            {
                                if (*(start += 8) == ';') parseCharset(start, end);
                                PostType = PostType.Xml;
                            }
                            break;
                        case 9 - 8://text/json; charset=utf-8
                            if ((((*(int*)start | 0x20202020) ^ ('t' | ('e' << 8) | ('x' << 16) | ('t' << 24)))
                                | ((*(int*)(start + sizeof(int)) | 0x20202000) ^ ('/' | ('j' << 8) | ('s' << 16) | ('o' << 24)))
                                | ((*(start + sizeof(int) * 2) | 0x20) ^ 'n')) == 0)
                            {
                                if (*(start += 9) == ';') parseCharset(start, end);
                                PostType = PostType.Json;
                            }
                            break;
                        case 15 - 8://application/xml; charset=utf-8
                            if ((((*(int*)start | 0x20202020) ^ ('a' | ('p' << 8) | ('p' << 16) | ('l' << 24)))
                                | ((*(int*)(start + sizeof(int)) | 0x20202020) ^ ('i' | ('c' << 8) | ('a' << 16) | ('t' << 24)))
                                | ((*(int*)(start + sizeof(int) * 2) | 0x00202020) ^ ('i' | ('o' << 8) | ('n' << 16) | ('/' << 24)))
                                | (int)((*(uint*)(start + sizeof(int) * 3) | 0xff202020U) ^ ('x' | ('m' << 8) | ('l' << 16) | 0xff000000U))) == 0)
                            {
                                if (*(start += 15) == ';') parseCharset(start, end);
                                PostType = PostType.Xml;
                            }
                            break;
                        case 16 - 8://application/json; charset=utf-8
                            if ((((*(int*)start | 0x20202020) ^ ('a' | ('p' << 8) | ('p' << 16) | ('l' << 24)))
                                | ((*(int*)(start + sizeof(int)) | 0x20202020) ^ ('i' | ('c' << 8) | ('a' << 16) | ('t' << 24)))
                                | ((*(int*)(start + sizeof(int) * 2) | 0x00202020) ^ ('i' | ('o' << 8) | ('n' << 16) | ('/' << 24)))
                                | ((*(int*)(start + sizeof(int) * 3) | 0x20202020) ^ ('j' | ('s' << 8) | ('o' << 16) | ('n' << 24)))) == 0)
                            {
                                if (*(start += 16) == ';') parseCharset(start, end);
                                PostType = PostType.Json;
                            }
                            break;
                        case 19 - 8://multipart/form-data; boundary=---------------------------xxxxxxxxxxxxx
                            if (contentType.Length > 30)
                            {
                                if ((((*(int*)start | 0x20202020) ^ ('m' | ('u' << 8) | ('l' << 16) | ('t' << 24)))
                                    | ((*(int*)(start + sizeof(int)) | 0x20202020) ^ ('i' | ('p' << 8) | ('a' << 16) | ('r' << 24)))
                                    | ((*(int*)(start + sizeof(int) * 2) | 0x20200020) ^ ('t' | ('/' << 8) | ('f' << 16) | ('o' << 24)))
                                    | ((*(int*)(start + sizeof(int) * 3) | 0x20002020) ^ ('r' | ('m' << 8) | ('-' << 16) | ('d' << 24)))
                                    | ((*(int*)(start + sizeof(int) * 4) | 0x00202020) ^ ('a' | ('t' << 8) | ('a' << 16) | (';' << 24)))
                                    | ((*(int*)(start + sizeof(int) * 5) | 0x20202000) ^ (' ' | ('b' << 8) | ('o' << 16) | ('u' << 24)))
                                    | ((*(int*)(start + sizeof(int) * 6) | 0x20202020) ^ ('n' | ('d' << 8) | ('a' << 16) | ('r' << 24)))
                                    | ((*(short*)(start += sizeof(int) * 7) | 0x20) ^ ('y' | ('=' << 8)))) == 0)
                                {
                                    BoundaryIndex.Set(contentType.StartIndex + sizeof(int) * 7 + 2, contentType.Length - (sizeof(int) * 7 + 2));
                                    if (BoundaryIndex.Length > maxBoundaryLength) return false;
                                    Flag |= HeaderFlag.IsSetBoundary;
                                    PostType = PostType.FormData;
                                }
                            }
                            break;
                        case 33 - 8://application/x-www-form-urlencoded
                            if ((((*(int*)start | 0x20202020) ^ ('a' | ('p' << 8) | ('p' << 16) | ('l' << 24)))
                                | ((*(int*)(start + sizeof(int)) | 0x20202020) ^ ('i' | ('c' << 8) | ('a' << 16) | ('t' << 24)))
                                | ((*(int*)(start + sizeof(int) * 2) | 0x00202020) ^ ('i' | ('o' << 8) | ('n' << 16) | ('/' << 24)))
                                | ((*(int*)(start + sizeof(int) * 3) | 0x20200020) ^ ('x' | ('-' << 8) | ('w' << 16) | ('w' << 24)))
                                | ((*(int*)(start + sizeof(int) * 4) | 0x20200020) ^ ('w' | ('-' << 8) | ('f' << 16) | ('o' << 24)))
                                | ((*(int*)(start + sizeof(int) * 5) | 0x20002020) ^ ('r' | ('m' << 8) | ('-' << 16) | ('u' << 24)))
                                | ((*(int*)(start + sizeof(int) * 6) | 0x20202020) ^ ('r' | ('l' << 8) | ('e' << 16) | ('n' << 24)))
                                | ((*(int*)(start + sizeof(int) * 7) | 0x20202020) ^ ('c' | ('o' << 8) | ('d' << 16) | ('e' << 24)))
                                | ((*(start + sizeof(int) * 8) | 0x20) ^ 'd')) == 0)
                            {
                                PostType = PostType.Form;
                            }
                            break;
                    }
                }
                if (Method == MethodType.POST)
                {
                    if (PostType == PostType.None)
                    {
                        if ((WebSocketFlag &= HeaderWebSocketFlag.IsWebSocket) == HeaderWebSocketFlag.IsWebSocket) return false;
                        PostType = PostType.Data;
                    }
                    if ((Flag & HeaderFlag.IsKeepAlive) == 0 && ContentLength < (int)(receiveCount - (HeaderEndIndex + sizeof(int)))) return false;
                }
                else
                {
                    if (PostType != PostType.None) return false;
                    if ((Flag & HeaderFlag.IsKeepAlive) == 0 && receiveCount - HeaderEndIndex != sizeof(int)) return false;
                    if ((ContentLength | (int)(Flag & HeaderFlag.IsSetBoundary)) != 0) return false;
                }
                int isValue = QueryCount = 0;
                if ((WebSocketFlag & HeaderWebSocketFlag.IsWebSocket) != HeaderWebSocketFlag.IsWebSocket)
                {
                    start = bufferStart + UriIndex.StartIndex;
                    end = AutoCSer.Memory.Find(start, start + UriIndex.Length, (byte)'?');
                    if (end == null)
                    {
                        end = AutoCSer.Memory.Find(start, start + UriIndex.Length, (byte)'#');
                        if (end != null) Flag |= HeaderFlag.IsHash | HeaderFlag.IsSetHash | HeaderFlag.IsSetSearchEngine | HeaderFlag.IsSearchEngine;
                    }
                    else if (*(end + 1) == '_')
                    {
                        if (Memory.SimpleEqualNotNull(googleFragmentName.Byte, end + 2, googleFragmentNameSize))
                        {
                            Flag |= HeaderFlag.IsHash | HeaderFlag.IsSetHash | HeaderFlag.IsSetSearchEngine | HeaderFlag.IsSearchEngine;
                            byte* write = end + 1, urlEnd = start + UriIndex.Length;
                            current = write + googleFragmentNameSize + 1;
                            byte endValue = *urlEnd;
                            *urlEnd = (byte)'%';
                            do
                            {
                                while (*current != '%') *write++ = *current++;
                                if (current == urlEnd) break;
                                uint code = (uint)(*++current - '0'), number = (uint)(*++current - '0');
                                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                                code = (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
                                *write++ = code == 0 ? (byte)' ' : (byte)code;
                            }
                            while (++current < urlEnd);
                            *urlEnd = endValue;
                            UriIndex.Length = (short)((int)(write - bufferStart) - UriIndex.StartIndex);
                        }
                    }
                    if (end == null) PathIndex = UriIndex;
                    else
                    {
                        PathIndex.Set(UriIndex.StartIndex, (short)(end - start));
                        BufferIndex* nameIndex = (BufferIndex*)(bufferStart + QueryStartIndex);
                        current = end;
                        byte endValue = *(end = start + UriIndex.Length);
                        *end = (byte)'&';
                        if ((Flag | HeaderFlag.IsSetHash) != 0)
                        {
                            if (*current == '!') ++current;
                            else if (*current == '%' && *(short*)(current + 1) == '2' + ('1' << 8)) current += 3;
                        }
                        do
                        {
                            byte isDefaultQuery = 0;
                            nameIndex->StartIndex = (short)(++current - bufferStart);
                            while (*current != '&' && *current != '=') ++current;
                            BufferIndex* valueIndex = nameIndex + 1;
                            nameIndex->Length = (short)((int)(current - bufferStart) - nameIndex->StartIndex);
                            if (*current == '=')
                            {
                                isValue = 1;
                                valueIndex->StartIndex = (short)(++current - bufferStart);
                                while (*current != '&') ++current;
                                valueIndex->Length = (short)((int)(current - bufferStart) - valueIndex->StartIndex);
                            }
                            else valueIndex->Value = 0;
                            if (nameIndex->Length == 1 && valueIndex->Length != 0)
                            {
                                switch (*(bufferStart + nameIndex->StartIndex) - 'a')
                                {
                                    case AjaxCallNameChar - 'a':
                                        AjaxCallNameIndex = *valueIndex;
                                        Flag |= HeaderFlag.IsSetAjaxCallName;
                                        if (PathIndex.Length == 5)
                                        {
                                            byte* viewPath = bufferStart + PathIndex.StartIndex;
                                            if (*(int*)viewPath == '/' + ('A' << 8) + ('j' << 16) + ('a' << 24) && *(viewPath + sizeof(int)) == 'x') Flag |= HeaderFlag.IsAjax;
                                        }
                                        isDefaultQuery = 1;
                                        break;
                                    case AjaxCallBackNameChar - 'a':
                                        AjaxCallBackNameIndex = *valueIndex;
                                        Flag |= HeaderFlag.IsSetAjaxCallBackName;
                                        isDefaultQuery = 1;
                                        break;
                                    case ReViewNameChar - 'a':
                                        Flag |= HeaderFlag.IsReView;
                                        isDefaultQuery = 1;
                                        break;
                                    case VersionNameChar - 'a':
                                        Flag |= HeaderFlag.IsVersion;
                                        isDefaultQuery = 1;
                                        break;
                                    case LoadPageCacheChar - 'a':
                                        Flag |= HeaderFlag.IsLoadPageCache;
                                        isDefaultQuery = 1;
                                        break;
                                    case MobileReViewNameChar - 'a':
                                        Flag |= HeaderFlag.IsMobileReView;
                                        isDefaultQuery = 1;
                                        break;
                                    case QueryJsonNameChar - 'a':
                                        queryJson = *valueIndex;
                                        Flag |= HeaderFlag.IsSetQueryJson;
                                        isDefaultQuery = 1;
                                        break;
                                    case QueryXmlNameChar - 'a':
                                        queryXml = *valueIndex;
                                        Flag |= HeaderFlag.IsSetQueryXml;
                                        isDefaultQuery = 1;
                                        break;
                                    case EncodingNameChar - 'a':
                                        if (requestEncoding.Encoding == null)
                                        {
                                            start = bufferStart + valueIndex->StartIndex;
                                            switch (*(start + 2) & 15)
                                            {
                                                case ('2' & 15)://gb2312
                                                    if (*(int*)(start + 2) == ('2' | ('3' << 8) | ('1' << 16) | ('2' << 24)))
                                                    {
                                                        requestEncoding = EncodingCacheOther.Gb2312;
                                                        Flag |= HeaderFlag.IsSetRequestEncoding;
                                                    }
                                                    break;
                                                case ('f' & 15)://utf-8
                                                    if ((*(int*)(start + 1) | 0x2020) == ('t' | ('f' << 8) | ('-' << 16) | ('8' << 24)))
                                                    {
                                                        requestEncoding = EncodingCache.Utf8;
                                                        Flag |= HeaderFlag.IsSetRequestEncoding;
                                                    }
                                                    break;
                                                case ('k' & 15)://gbk
                                                    if ((*(int*)(start - 1) | 0x20202000) == ('=' | ('g' << 8) | ('b' << 16) | ('k' << 24)))
                                                    {
                                                        requestEncoding = EncodingCacheOther.Gbk;
                                                        Flag |= HeaderFlag.IsSetRequestEncoding;
                                                    }
                                                    break;
                                                case ('g' & 15)://big5
                                                    if ((*(int*)start | 0x00202020) == ('b' | ('i' << 8) | ('g' << 16) | ('5' << 24)))
                                                    {
                                                        requestEncoding = EncodingCacheOther.Big5;
                                                        Flag |= HeaderFlag.IsSetRequestEncoding;
                                                    }
                                                    break;
                                                case ('1' & 15)://gb18030
                                                    if (*(int*)(start + 3) == ('8' | ('0' << 8) | ('3' << 16) | ('0' << 24)))
                                                    {
                                                        requestEncoding = EncodingCacheOther.Gb18030;
                                                        Flag |= HeaderFlag.IsSetRequestEncoding;
                                                    }
                                                    break;
                                                case ('i' & 15)://unicode
                                                    if ((*(int*)(start + 3) | 0x20202020) == ('c' | ('o' << 8) | ('d' << 16) | ('e' << 24)))
                                                    {
                                                        requestEncoding = EncodingCache.Unicode;
                                                        Flag |= HeaderFlag.IsSetRequestEncoding;
                                                    }
                                                    break;
                                            }
                                            isDefaultQuery = 1;
                                        }
                                        break;
                                }
                            }
                            if (isDefaultQuery == 0)
                            {
                                if (QueryCount == maxQueryCount) return false;
                                nameIndex += 2;
                                ++QueryCount;
                            }
                        }
                        while (current != end);
                        *end = endValue;
                    }
                }
                else
                {
                    start = bufferStart + UriIndex.StartIndex;
                    end = AutoCSer.Memory.Find(start, start + UriIndex.Length, (byte)'?');
                    if (end == null) PathIndex = UriIndex;
                    else
                    {
                        PathIndex.Set(UriIndex.StartIndex, (short)(end - start));
                        BufferIndex* nameIndex = (BufferIndex*)(bufferStart + QueryStartIndex);
                        current = end;
                        byte endValue = *(end = start + UriIndex.Length);
                        *end = (byte)'&';
                        do
                        {
                            byte isDefaultQuery = 0;
                            nameIndex->StartIndex = (short)(++current - bufferStart);
                            while (*current != '&' && *current != '=') ++current;
                            BufferIndex* valueIndex = nameIndex + 1;
                            nameIndex->Length = (short)((int)(current - bufferStart) - nameIndex->StartIndex);
                            if (*current == '=')
                            {
                                isValue = 1;
                                valueIndex->StartIndex = (short)(++current - bufferStart);
                                while (*current != '&') ++current;
                                valueIndex->Length = (short)((int)(current - bufferStart) - valueIndex->StartIndex);
                            }
                            else valueIndex->Value = 0;
                            if (nameIndex->Length == 1 && valueIndex->Length != 0)
                            {
                                switch (*(bufferStart + nameIndex->StartIndex) - 'a')
                                {
                                    case QueryJsonNameChar - 'a':
                                        queryJson = *valueIndex;
                                        Flag |= HeaderFlag.IsSetQueryJson;
                                        isDefaultQuery = 1;
                                        break;
                                    case QueryXmlNameChar - 'a':
                                        queryXml = *valueIndex;
                                        Flag |= HeaderFlag.IsSetQueryXml;
                                        isDefaultQuery = 1;
                                        break;
                                }
                            }
                            if (isDefaultQuery == 0)
                            {
                                if (QueryCount == maxQueryCount) return false;
                                nameIndex += 2;
                                ++QueryCount;
                            }
                        }
                        while (current != end);
                        *end = endValue;
                    }
                }
                if (((QueryCount ^ 1) | isValue) == 0)
                {
                    BufferIndex* nameIndex = (BufferIndex*)(bufferStart + QueryStartIndex);
                    (nameIndex + 1)->Value = nameIndex->Value;
                    nameIndex->StartIndex = 1;
                    nameIndex->Length = 0;
                }
            }
            return true;
        }
        /// <summary>
        /// 请求字节范围解析
        /// </summary>
        /// <param name="start">数据起始位置</param>
        /// <param name="end">数据结束未知</param>
        private void parseRange(byte* start, byte* end)
        {
            if (*(start += 6) == '-')
            {
                long rangeEnd = 0;
                while (++start != end)
                {
                    rangeEnd *= 10;
                    rangeEnd += *start - '0';
                }
                if (rangeEnd > 0)
                {
                    this.RangeStart = long.MinValue;
                    this.RangeEnd = rangeEnd;
                    Flag |= HeaderFlag.IsRange;
                    return;
                }
            }
            else
            {
                long rangeStart = 0;
                do
                {
                    int number = *start - '0';
                    if ((uint)number > 9) break;
                    rangeStart *= 10;
                    ++start;
                    rangeStart += number;
                }
                while (true);
                if (rangeStart >= 0 && *start == '-')
                {
                    if (++start == end)
                    {
                        this.RangeStart = rangeStart;
                        this.RangeEnd = long.MinValue;
                        Flag |= HeaderFlag.IsRange;
                        return;
                    }
                    long rangeEnd = *start - '0';
                    while (++start != end)
                    {
                        rangeEnd *= 10;
                        rangeEnd += *start - '0';
                    }
                    if (rangeStart < rangeEnd)
                    {
                        this.RangeStart = rangeStart;
                        this.RangeEnd = rangeEnd;
                        Flag |= HeaderFlag.IsRange;
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 编码解析
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private void parseCharset(byte* start, byte* end)
        {
            *end = (byte)'c';
            while (*++start != 'c') ;
            if (start != end && (((*(int*)start | 0x20202020) ^ ('c' | ('h' << 8) | ('a' << 16) | ('r' << 24)))
                | ((*(int*)(start + sizeof(int)) | 0x00202020) ^ ('s' | ('e' << 8) | ('t' << 16) | ('=' << 24)))) == 0)
            {
                switch (*(start + 10) & 15)
                {
                    case ('2' & 15)://gb2312
                        if (*(int*)(start + 10) == ('2' | ('3' << 8) | ('1' << 16) | ('2' << 24)))
                        {
                            requestEncoding = EncodingCacheOther.Gb2312;
                            Flag |= HeaderFlag.IsSetRequestEncoding;
                        }
                        break;
                    case ('f' & 15)://utf-8
                        if ((*(int*)(start + 9) | 0x2020) == ('t' | ('f' << 8) | ('-' << 16) | ('8' << 24)))
                        {
                            requestEncoding = EncodingCache.Utf8;
                            Flag |= HeaderFlag.IsSetRequestEncoding;
                        }
                        break;
                    case ('k' & 15)://gbk
                        if ((*(int*)(start + 7) | 0x20202000) == ('=' | ('g' << 8) | ('b' << 16) | ('k' << 24)))
                        {
                            requestEncoding = EncodingCacheOther.Gbk;
                            Flag |= HeaderFlag.IsSetRequestEncoding;
                        }
                        break;
                    case ('g' & 15)://big5
                        if ((*(int*)(start + 8) | 0x00202020) == ('b' | ('i' << 8) | ('g' << 16) | ('5' << 24)))
                        {
                            requestEncoding = EncodingCacheOther.Big5;
                            Flag |= HeaderFlag.IsSetRequestEncoding;
                        }
                        break;
                    case ('1' & 15)://gb18030
                        if (*(int*)(start + 11) == ('8' | ('0' << 8) | ('3' << 16) | ('0' << 24)))
                        {
                            requestEncoding = EncodingCacheOther.Gb18030;
                            Flag |= HeaderFlag.IsSetRequestEncoding;
                        }
                        break;
                    case ('i' & 15)://unicode
                        if ((*(int*)(start + 11) | 0x20202020) == ('c' | ('o' << 8) | ('d' << 16) | ('e' << 24)))
                        {
                            requestEncoding = EncodingCache.Unicode;
                            Flag |= HeaderFlag.IsSetRequestEncoding;
                        }
                        break;
                }
            }
            *end = 13;
        }
        /// <summary>
        /// 判断是否搜索引擎
        /// </summary>
        /// <returns></returns>
        private bool checkSearchEngine()
        {
            fixed (byte* bufferFixed = Buffer.Buffer)
            {
                byte* start = bufferFixed + Buffer.StartIndex + userAgent.StartIndex, letterTable = (byte*)searchEngineLetterTable.Data, end = start + userAgent.Length - 4;
                do
                {
                    switch (letterTable[*start])
                    {
                        case (byte)SearchEngineLetter.b://bingbot
                            if (*(short*)(start + 1) == 'i' + ('n' << 8) && *(int*)(start + 3) == 'g' + ('b' << 8) + ('o' << 16) + ('t' << 24)) return true;
                            break;
                        case (byte)SearchEngineLetter.D://DotBot
                            if (*(start + 1) == 'o' && *(int*)(start + 2) == 't' + ('B' << 8) + ('o' << 16) + ('t' << 24)) return true;
                            break;
                        case (byte)SearchEngineLetter.G:
                            if (*(start + 1) == 'o')
                            {//Googlebot
                                if (*(int*)(start + 1) == 'o' + ('o' << 8) + ('g' << 16) + ('l' << 24) && *(int*)(start + 5) == 'e' + ('b' << 8) + ('o' << 16) + ('t' << 24)) return true;
                            }
                            else if (*(start + 1) == 'e')
                            {//GeoHasher
                                if (*(int*)(start + 1) == 'e' + ('o' << 8) + ('H' << 16) + ('a' << 24) && *(int*)(start + 5) == 's' + ('h' << 8) + ('e' << 16) + ('r' << 24)) return true;
                            }
                            break;
                        case (byte)SearchEngineLetter.i://ia_archiver
                            if (*(short*)(start + 1) == 'a' + ('_' << 8) && *(int*)(start + 3) == 'a' + ('r' << 8) + ('c' << 16) + ('h' << 24) && *(int*)(start + 7) == 'i' + ('v' << 8) + ('e' << 16) + ('r' << 24)) return true;
                            break;
                        case (byte)SearchEngineLetter.M:
                            if (*(start + 1) == 'e')
                            {//Mediapartners-Google
                                if (*(short*)(start + 2) == 'd' + ('i' << 8) && *(int*)(start + 4) == 'a' + ('p' << 8) + ('a' << 16) + ('r' << 24)
                                    && *(int*)(start + 8) == 't' + ('n' << 8) + ('e' << 16) + ('r' << 24) && *(int*)(start + 12) == 's' + ('-' << 8) + ('G' << 16) + ('o' << 24)
                                    && *(int*)(start + 16) == 'o' + ('g' << 8) + ('l' << 16) + ('e' << 24)) return true;
                            }
                            else if (*(short*)(start + 1) == 'J' + ('1' << 8))
                            {//MJ12bot
                                if (*(int*)(start + 3) == '2' + ('b' << 8) + ('o' << 16) + ('t' << 24)) return true;
                            }
                            break;
                        case (byte)SearchEngineLetter.m://msnbot
                            if (*(start + 1) == 's' && *(int*)(start + 2) == 'n' + ('b' << 8) + ('o' << 16) + ('t' << 24)) return true;
                            break;
                        case (byte)SearchEngineLetter.R://R6_CommentReader
                            if (*(int*)start == 'R' + ('6' << 8) + ('_' << 16) + ('C' << 24) && *(int*)(start + 4) == 'o' + ('m' << 8) + ('m' << 16) + ('e' << 24)
                                && *(int*)(start + 8) == 'n' + ('t' << 8) + ('R' << 16) + ('e' << 24) && *(int*)(start + 12) == 'a' + ('d' << 8) + ('e' << 16) + ('r' << 24)) return true;
                            break;
                        case (byte)SearchEngineLetter.r://renren share slurp
                            if (*(start + 1) == 'e' && *(int*)(start + 2) == 'n' + ('r' << 8) + ('e' << 16) + ('n' << 24) && *(int*)(start + 6) == ' ' + ('s' << 8) + ('h' << 16) + ('a' << 24)
                                && *(int*)(start + 10) == 'r' + ('e' << 8) + (' ' << 16) + ('s' << 24) && *(int*)(start + 14) == 'l' + ('u' << 8) + ('r' << 16) + ('p' << 24)) return true;
                            break;
                        case (byte)SearchEngineLetter.S://
                            if (*(start + 1) == 'p')
                            {//Spider
                                if (*(int*)(start + 2) == 'i' + ('d' << 8) + ('e' << 16) + ('r' << 24)) return true;
                            }
                            else if (*(start + 1) == 'o')
                            {//Sogou
                                if (*(int*)(start + 1) == 'o' + ('g' << 8) + ('o' << 16) + ('u' << 24)) return true;
                            }
                            else if (*(short*)(start + 1) == 'i' + ('t' << 8))
                            {//SiteBot
                                if (*(int*)(start + 3) == 'e' + ('B' << 8) + ('o' << 16) + ('t' << 24)) return true;
                            }
                            break;
                        case (byte)SearchEngineLetter.s://spider
                            if (*(start + 1) == 'p' && *(int*)(start + 2) == 'i' + ('d' << 8) + ('e' << 16) + ('r' << 24)) return true;
                            break;
                        case (byte)SearchEngineLetter.T://Twiceler
                            if (*(int*)start == 'T' + ('w' << 8) + ('i' << 16) + ('c' << 24) && *(int*)(start + 4) == 'e' + ('l' << 8) + ('e' << 16) + ('r' << 24)) return true;
                            break;
                        case (byte)SearchEngineLetter.Y:
                            if (*(start + 1) == 'a')
                            {
                                if (*(short*)(start + 2) == 'h' + ('o' << 8))
                                {//Yahoo! Slurp
                                    if (*(int*)(start + 4) == 'o' + ('!' << 8) + (' ' << 16) + ('S' << 24) && *(int*)(start + 8) == 'l' + ('u' << 8) + ('r' << 16) + ('p' << 24)) return true;
                                }
                                else
                                {//Yandex
                                    if (*(int*)(start + 2) == 'n' + ('d' << 8) + ('e' << 16) + ('x' << 24)) return true;
                                }
                            }
                            else if (*(int*)(start + 1) == 'o' + ('u' << 8) + ('d' << 16) + ('a' << 24))
                            {//YoudaoBot
                                if (*(int*)(start + 5) == 'o' + ('B' << 8) + ('o' << 16) + ('t' << 24)) return true;
                            }
                            break;
                        case (byte)SearchEngineLetter.Z://ZhihuExternalHit
                            if (*(int*)start == 'Z' + ('h' << 8) + ('i' << 16) + ('h' << 24) && *(int*)(start + 4) == 'u' + ('E' << 8) + ('x' << 16) + ('t' << 24)
                                 && *(int*)(start + 8) == 'e' + ('r' << 8) + ('n' << 16) + ('a' << 24) && *(int*)(start + 16) == 'l' + ('H' << 8) + ('i' << 16) + ('t' << 24)) return true;
                            break;
                    }
                }
                while (++start != end);
            }
            return false;
        }
        /// <summary>
        /// 处理忽略的请求内容字节长度
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void IgnoreContentLength()
        {
            int receiveSize = Math.Min(receiveCount - receiveIndex, ContentLength);
            receiveIndex += receiveSize;
            ContentLength -= receiveSize;
        }
        /// <summary>
        /// 复制接收数据到表单缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        internal void CopyToForm(ref SubArray<byte> buffer)
        {
            int receiveSize = Math.Min(receiveCount - receiveIndex, ContentLength);
            if (receiveSize != 0)
            {
                System.Buffer.BlockCopy(Buffer.Buffer, Buffer.StartIndex + receiveIndex, buffer.Array, buffer.Start, receiveSize);
                receiveIndex += receiveSize;
                buffer.MoveStart(receiveSize);
            }
        }
        /// <summary>
        /// 复制接收数据到数据缓冲区
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal int CopyToFormData(ref SubBuffer.PoolBufferFull buffer)
        {
            int receiveSize = Math.Min(receiveCount - receiveIndex, ContentLength);
            if (receiveSize != 0)
            {
                System.Buffer.BlockCopy(Buffer.Buffer, Buffer.StartIndex + receiveIndex, buffer.Buffer, buffer.StartIndex, receiveSize);
                receiveIndex += receiveSize;
            }
            return receiveSize;
        }
        /// <summary>
        /// 查询解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool ParseQuery<valueType>(ref valueType value)
        {
            return QueryCount == 0 || HeaderQueryParser.Parse(this, ref value);
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length">数据长度</param>
        /// <param name="bufferStart"></param>
        /// <returns>unescape解码后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal string UnescapeUtf8(byte* start, int length, byte* bufferStart)
        {
            return UnescapeUtf8(start, length, Buffer.Buffer, Buffer.StartIndex + (int)(start - bufferStart));
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="bufferStart"></param>
        /// <param name="index"></param>
        /// <param name="length">数据长度</param>
        /// <returns>unescape解码后的字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal string UnescapeUtf8(byte* bufferStart, int index, int length)
        {
            return UnescapeUtf8(bufferStart + index, length, Buffer.Buffer, Buffer.StartIndex + index);
        }
        /// <summary>
        /// 模拟javascript解码函数unescape
        /// </summary>
        /// <param name="current"></param>
        /// <param name="length">数据长度</param>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="bufferIndex">数据起始位置</param>
        /// <returns>unescape解码后的字符串</returns>
        internal static string UnescapeUtf8(byte* current, int length, byte[] buffer, int bufferIndex)
        {
            byte* start = current, end = current + length, escape = null;
            uint escapeCode = 0, unicode = 0;
            do
            {
                if ((*start & 0x80) == 0)
                {
                    if (*start == '%')
                    {
                        if (escape == null) escape = start;
                        if (*++start == 'u')
                        {
                            unicode = 1;
                            break;
                        }
                        uint code = (uint)(*start++ - '0');
                        escapeCode |= code > 9 ? 8 : code;
                    }
                }
                else escapeCode = 8;
            }
            while (++start < end);
            if (unicode != 0 || (escapeCode & 8) == 0)
            {
                if (escape == null) AutoCSer.Memory.ToString(current, (int)(end - current));
                length = (int)(++escape - current);
                for (start = escape + (*escape == 'u' ? 5 : 2); start < end; ++length)
                {
                    if (*start == '%') start += *(start + 1) == 'u' ? 6 : 3;
                    else ++start;
                }
                string value = AutoCSer.Extension.StringExtension.FastAllocateString(length);
                fixed (char* valueFixed = value)
                {
                    start = current;
                    char* write = valueFixed, writeEnd = valueFixed + length;
                    do
                    {
                        if (*start == '%')
                        {
                            if (*++start == 'u')
                            {
                                uint code = (uint)(*++start - '0'), number = (uint)(*++start - '0');
                                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                                if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                                code <<= 12;
                                code += (number << 8);
                                if ((number = (uint)(*++start - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
                                code += (number << 4);
                                number = (uint)(*++start - '0');
                                code += (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
                                *write = (char)code;
                            }
                            else
                            {
                                uint code = (uint)(*start - '0'), number = (uint)(*++start - '0');
                                if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
                                code = (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
                                *write = (char)code;
                            }
                        }
                        else *write = (char)*start;
                        ++start;
                    }
                    while (++write != writeEnd);
                }
                return value;
            }
            if (escape != null)
            {
                byte* write = escape;
            NEXT:
                unicode = (uint)(*++escape - '0');
                escapeCode = (uint)(*++escape - '0');
                if (unicode > 9) unicode = ((unicode - ('A' - '0')) & 0xffdfU) + 10;
                unicode = (escapeCode > 9 ? (((escapeCode - ('A' - '0')) & 0xffdfU) + 10) : escapeCode) + (unicode << 4);
                *write++ = (byte)unicode;
                while (++escape < end)
                {
                    if (*escape == '%') goto NEXT;
                    *write++ = *escape;
                }
                end = write;
            }
            return Encoding.UTF8.GetString(buffer, bufferIndex, (int)(end - current));
        }
        /// <summary>
        /// 设置输出重定向
        /// </summary>
        /// <param name="path"></param>
        /// <param name="location"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetResponseLocationRange(string path, ref SubArray<byte> location)
        {
            if (HeaderEndIndex <= path.Length) location.Set(Buffer.Buffer, Buffer.StartIndex, path.Length);
            else location.Set(new byte[path.Length], 0, path.Length);
        }
        /// <summary>
        /// 设置输出重定向
        /// </summary>
        /// <param name="length"></param>
        /// <param name="location"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetResponseLocationRange(int length, ref SubArray<byte> location)
        {
            if (HeaderEndIndex <= length) location.Set(Buffer.Buffer, Buffer.StartIndex, length);
            else location.Set(new byte[length], 0, length);
        }
        /// <summary>
        /// 获取 Cookie 值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="nameLength">名称长度</param>
        /// <returns>值</returns>
        private BufferIndex getCookie(byte* name, int nameLength)
        {
            fixed (byte* bufferFixed = Buffer.Buffer)
            {
                byte* bufferStart = bufferFixed + Buffer.StartIndex, start = bufferStart + cookie.StartIndex, end = start + cookie.Length, searchEnd = end - nameLength;
                *end = (byte)';';
                do
                {
                    while (*start == ' ') ++start;
                    if (start >= searchEnd) break;
                    if (*(start + nameLength) == '=')
                    {
                        if (AutoCSer.Memory.SimpleEqualNotNull(name, start, nameLength))
                        {
                            for (start += nameLength + 1; *start == ' '; ++start) ;
                            int startIndex = (int)(start - bufferStart);
                            while (*start != ';') ++start;
                            return new BufferIndex { StartIndex = (short)startIndex, Length = (short)((int)(start - bufferStart) - startIndex) };
                        }
                        start += nameLength + 1;
                    }
                    while (*start != ';') ++start;
                }
                while (++start < searchEnd);
            }
            return default(BufferIndex);
        }
        /// <summary>
        /// 获取 Cookie 值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetCookie(byte[] name, ref SubArray<byte> value)
        {
            if (cookie.Length > name.Length)
            {
                fixed (byte* nameFixed = name)
                {
                    BufferIndex index = getCookie(nameFixed, name.Length);
                    if (index.StartIndex != 0) value.Set(Buffer.Buffer, Buffer.StartIndex + index.StartIndex, index.Length);
                }
            }
        }
        ///// <summary>
        ///// 获取Cookie值
        ///// </summary>
        ///// <param name="name">名称</param>
        ///// <param name="value">值</param>
        //internal void GetCookie(string name, ref SubArray<byte> value)
        //{
        //    if (cookie.Length > name.Length && name.Length <= UnmanagedPool.TinySize)
        //    {
        //        BufferIndex index;
        //        fixed (char* nameFixed = name)
        //        {
        //            byte* cookieNameBuffer = UnmanagedPool.Tiny.Get();
        //            AutoCSer.Extension.StringExtension.WriteBytes(nameFixed, name.Length, cookieNameBuffer);
        //            index = getCookie(cookieNameBuffer, name.Length);
        //            UnmanagedPool.Tiny.Push(cookieNameBuffer);
        //        }
        //        if (index.StartIndex != 0) value.Set(Buffer.Buffer, Buffer.StartIndex + index.StartIndex, index.Length);
        //    }
        //}
        /// <summary>
        /// 判断是否存在Cookie值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>是否存在Cookie值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe bool IsCookie(byte[] name)
        {
            if (cookie.Length > name.Length)
            {
                fixed (byte* nameFixed = name)
                {
                    return getCookie(nameFixed, name.Length).StartIndex != 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 最大数据分隔符长度
        /// </summary>
        private const int maxBoundaryLength = 128;
        /// <summary>
        /// AJAX调用名称
        /// </summary>
        internal const string AjaxCallName = "/Ajax";
        /// <summary>
        /// web视图重新加载禁用输出成员名称
        /// </summary>
        internal const string ViewOnlyName = "ViewOnly";
        /// <summary>
        /// 公用错误处理函数名称
        /// </summary>
        internal const string PubErrorCallName = "Pub.Error";
        /// <summary>
        /// AJAX回调函数名称
        /// </summary>
        internal const char AjaxCallBackNameChar = 'c';
        /// <summary>
        /// 查询编码名称
        /// </summary>
        internal const char EncodingNameChar = 'e';
        /// <summary>
        /// json查询对象名称
        /// </summary>
        internal const char QueryJsonNameChar = 'j';
        /// <summary>
        /// 第一次加载页面缓存名称
        /// </summary>
        internal const char LoadPageCacheChar = 'l';
        /// <summary>
        /// 重新加载视图查询名称（忽略主列表）
        /// </summary>
        internal const char MobileReViewNameChar = 'm';
        /// <summary>
        ///AJAX调用函数名称
        /// </summary>
        internal const char AjaxCallNameChar = 'n';
        /// <summary>
        /// 重新加载视图查询名称
        /// </summary>
        internal const char ReViewNameChar = 'r';
        /// <summary>
        /// URL 资源版本查询名称
        /// </summary>
        internal const char VersionNameChar = 'v';
        /// <summary>
        /// XML查询对象名称
        /// </summary>
        internal const char QueryXmlNameChar = 'x';
        /// <summary>
        /// 保存当前版本的文件名称
        /// </summary>
        internal const string VersionFileName = "AutoCSer.Web.Version.html";
        /// <summary>
        /// 页面初始化默认遮罩层 ID
        /// </summary>
        internal const string ViewOverId = "AutoCSerViewOver";
        /// <summary>
        /// Google请求#!查询名称字节长度
        /// </summary>
        private const int googleFragmentNameSize = 18;
        /// <summary>
        /// Google请求#!查询名称
        /// </summary>
        private static Pointer googleFragmentName;
        /// <summary>
        /// 搜索引擎首字母查询表
        /// </summary>
        private static Pointer searchEngineLetterTable;
        /// <summary>
        /// HTTP 头部缓冲区池
        /// </summary>
        internal static readonly SubBuffer.Pool BufferPool;
        /// <summary>
        /// HTTP 头部最大未定义项数
        /// </summary>
        protected static readonly int maxHeaderCount;
        /// <summary>
        /// HTTP 头部未定义项保存位置
        /// </summary>
        internal static readonly int NameStartIndex;
        /// <summary>
        /// URI 最大查询参数项数
        /// </summary>
        protected static readonly int maxQueryCount;
        /// <summary>
        /// URI 查询参数保存位置
        /// </summary>
        internal static readonly int QueryStartIndex;
        /// <summary>
        /// HTTP 头部缓存去接收数据最大字节数
        /// </summary>
        internal static readonly int ReceiveBufferSize;
        /// <summary>
        /// KeepAlive 是否保持相同的域名服务
        /// </summary>
        protected static readonly bool isKeepAliveDomainServer;
        /// <summary>
        /// HTTP 头部接收超时
        /// </summary>
        internal static readonly SocketTimeoutLink.TimerLink ReceiveTimeout;
        /// <summary>
        /// 第二次 HTTP 头部接收超时
        /// </summary>
        internal static readonly SocketTimeoutLink.TimerLink KeepAliveReceiveTimeout;
        /// <summary>
        /// 查询模式类型集合
        /// </summary>
        private static Pointer methodTypes;
        /// <summary>
        /// 查询模式字节转枚举
        /// </summary>
        /// <param name="method">查询模式</param>
        /// <returns>查询模式枚举</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static unsafe MethodType GetMethod(byte* method)
        {
            uint code = *(uint*)method;
            return (MethodType)methodTypes.Byte[((code >> 12) ^ code) & ((1U << 4) - 1)];
        }
        
        static Header()
        {
            Config config = SocketBase.Config;
            BufferPool = SubBuffer.Pool.GetPool(config.HeadSize);
            int size = (int)config.HeadSize;
            NameStartIndex = size - (maxHeaderCount = Math.Max(config.MaxHeaderCount, 0)) * sizeof(BufferIndex) * 2;
            QueryStartIndex = NameStartIndex - (maxQueryCount = Math.Max(config.MaxQueryCount, 0)) * sizeof(BufferIndex) * 2;
            ReceiveBufferSize = Math.Min(QueryStartIndex - sizeof(int), size - 20 * 4);
            if (ReceiveBufferSize < 64)
            {
                AutoCSer.Log.Pub.Log.Add(Log.LogType.Warn, "HTTP 头部缓冲区不足");
                BufferPool = SubBuffer.Pool.GetPool(Config.DefaultHeadSize);
                NameStartIndex = (size = (int)Config.DefaultHeadSize) - (maxHeaderCount = Config.DefaultHeaderCount) * sizeof(BufferIndex) * 2;
                QueryStartIndex = NameStartIndex - (maxQueryCount = Config.DefaultQueryCount) * sizeof(BufferIndex) * 2;
                ReceiveBufferSize = Math.Min(QueryStartIndex - sizeof(int), size - 20 * 4);
            }
            isKeepAliveDomainServer = config.IsKeepAliveDomainServer;
            int receiveHeadSeconds = config.ReceiveHeadSeconds > 0 ? config.ReceiveHeadSeconds : Config.DefaultReceiveHeadSeconds;
            ReceiveTimeout = SocketTimeoutLink.TimerLink.Get(receiveHeadSeconds);
            int keepAliveReceiveHeadSeconds = config.KeepAliveReceiveHeadSeconds > 0 ? config.KeepAliveReceiveHeadSeconds : Config.DefaultKeepAliveReceiveHeadSeconds;
            KeepAliveReceiveTimeout = keepAliveReceiveHeadSeconds > receiveHeadSeconds ? SocketTimeoutLink.TimerLink.Get(keepAliveReceiveHeadSeconds) : ReceiveTimeout;

            searchEngineLetterTable = new Pointer { Data = Unmanaged.GetStatic64(256 + (1 << 4) + ((googleFragmentNameSize + 7) & (int.MaxValue - 7)), false) };
            Memory.ClearUnsafe((ulong*)searchEngineLetterTable.Data, (256 + (1 << 4)) >> 3);
            byte* letterTable = (byte*)searchEngineLetterTable.Data;
            letterTable['b'] = (byte)SearchEngineLetter.b;
            letterTable['D'] = (byte)SearchEngineLetter.D;
            letterTable['G'] = (byte)SearchEngineLetter.G;
            letterTable['i'] = (byte)SearchEngineLetter.i;
            letterTable['M'] = (byte)SearchEngineLetter.M;
            letterTable['m'] = (byte)SearchEngineLetter.m;
            letterTable['R'] = (byte)SearchEngineLetter.R;
            letterTable['r'] = (byte)SearchEngineLetter.r;
            letterTable['S'] = (byte)SearchEngineLetter.S;
            letterTable['s'] = (byte)SearchEngineLetter.s;
            letterTable['T'] = (byte)SearchEngineLetter.T;
            letterTable['Y'] = (byte)SearchEngineLetter.Y;
            letterTable['Z'] = (byte)SearchEngineLetter.Z;

            methodTypes = new Pointer { Data = searchEngineLetterTable.Byte + 256 };
            uint code;
            byte* methodBufferFixed = (byte*)&code;
            foreach (MethodType method in System.Enum.GetValues(typeof(MethodType)))
            {
                if (method != MethodType.Unknown)
                {
                    string methodString = method.ToString();
                    fixed (char* methodFixed = methodString)
                    {
                        byte* write = methodBufferFixed, end = methodBufferFixed;
                        if (methodString.Length >= sizeof(int)) end += sizeof(int);
                        else
                        {
                            code = 0x20202020U;
                            end += methodString.Length;
                        }
                        for (char* read = methodFixed; write != end; *write++ = (byte)*read++) ;
                        methodTypes.Byte[((code >> 12) ^ code) & ((1U << 4) - 1)] = (byte)method;
                    }
                }
            }

            googleFragmentName = new Pointer { Data = methodTypes.Byte + (1 << 4) };
            //escaped_fragment_=
            *googleFragmentName.Long = 'e' + ('s' << 8) + ('c' << 16) + ('a' << 24) + ((long)'p' << 32) + ((long)'e' << 40) + ((long)'d' << 48) + ((long)'_' << 56);
            *(googleFragmentName.Long + 1) = 'f' + ('r' << 8) + ('a' << 16) + ('g' << 24) + ((long)'m' << 32) + ((long)'e' << 40) + ((long)'n' << 48) + ((long)'t' << 56);
            *(short*)(googleFragmentName.Long + 2) = '_' + ('=' << 8);
        }
    }
}

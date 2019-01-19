using System;
using System.Threading;
using AutoCSer.Extension;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoCSer.Net.HttpDomainServer
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public class FileServer : Server
    {
        /// <summary>
        /// 输出编码
        /// </summary>
        internal EncodingCache ResponseEncoding;
        /// <summary>
        /// 默认内容类型头部
        /// </summary>
        internal readonly byte[] HtmlContentType;
        /// <summary>
        /// 默认内容类型头部
        /// </summary>
        internal readonly byte[] JsContentType;
        /// <summary>
        /// 路径标识
        /// </summary>
        private int pathIdentity;
        /// <summary>
        /// 缓存控制参数
        /// </summary>
        protected byte[] cacheControl;
        /// <summary>
        /// 文件路径
        /// </summary>
        protected virtual string path { get { return null; } }
        /// <summary>
        /// 客户端缓存时间，默认为0 (单位:秒)
        /// </summary>
        protected virtual int clientCacheSeconds { get { return 0; } }
        /// <summary>
        /// 文件服务
        /// </summary>
        protected FileServer()
        {
            ResponseEncoding = new EncodingCache(WebConfig.Encoding ?? AutoCSer.Config.Pub.Default.Encoding);
            if (ResponseEncoding.Encoding.CodePage == AutoCSer.Config.Pub.Default.Encoding.CodePage)
            {
                HtmlContentType = Http.ContentTypeAttribute.Html;
                JsContentType = Http.ContentTypeAttribute.Js;
            }
            else
            {
                HtmlContentType = EncodingCache.Ascii.GetBytesNotEmpty("text/html; charset=" + ResponseEncoding.Encoding.WebName);
                JsContentType = EncodingCache.Ascii.GetBytesNotEmpty("application/x-javascript; charset=" + ResponseEncoding.Encoding.WebName);
            }
        }
        /// <summary>
        /// 启动HTTP服务
        /// </summary>
        /// <param name="registerServer">HTTP 注册管理服务</param>
        /// <param name="domains">域名信息集合</param>
        /// <param name="onStop">停止服务处理</param>
        /// <returns>是否启动成功</returns>
        public override bool Start(HttpRegister.Server registerServer, HttpRegister.Domain[] domains, Action onStop)
        {
            string path = AutoCSer.IO.File.FileNameToLower((this.path.fileNameToLower() ?? LoadCheckPath).pathSuffix());
            if (Directory.Exists(path) && Interlocked.CompareExchange(ref isStart, 1, 0) == 0)
            {
                pathIdentity = PathCacheWatcher.Add(path);
                WorkPath = path;
                RegisterServer = registerServer;
                this.domains = domains;
                this.onStop += onStop;
                if (Http.SocketBase.Config.IsResponseCacheControl)
                {
                    int clientCacheSeconds = this.clientCacheSeconds;
                    if (clientCacheSeconds == Http.Response.StaticFileCacheControlSeconds) cacheControl = Http.Response.StaticFileCacheControl;
                    else if (clientCacheSeconds == 0) cacheControl = Http.Response.ZeroAgeBytes;
                    else cacheControl = ("public, max-age=" + clientCacheSeconds.toString()).getBytes();
                }
                createErrorResponse();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected new bool dispose()
        {
            if (base.dispose())
            {
                if (WorkPath != null) PathCacheWatcher.Free(WorkPath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            dispose();
        }
        /// <summary>
        /// HTTP请求处理[TRY]
        /// </summary>
        /// <param name="socket">HTTP套接字</param>
        public override unsafe void Request(Http.SocketBase socket)
        {
            Http.Response response = file(socket.HttpHeader);
            if (response != null) socket.ResponseIdentity(ref response);
            else socket.ResponseErrorIdentity(Http.ResponseState.NotFound404);
        }
        /// <summary>
        /// HTTP文件请求处理
        /// </summary>
        /// <param name="header">请求头部信息</param>
        /// <returns>HTTP响应</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected Http.Response file(Http.Header header)
        {
            Http.Response response = null;
            file(header, file(header, ref response), ref response);
            return response;
        }
        /// <summary>
        /// HTTP文件请求处理
        /// </summary>
        /// <param name="header">请求头部</param>
        /// <param name="response">HTTP响应输出</param>
        /// <returns>文件缓存</returns>
        protected unsafe FileCache file(Http.Header header, ref Http.Response response)
        {
            SubArray<byte> path = header.Path;
            string cacheFileName = null;
            try
            {
                if (path.Length != 0 && WorkPath.Length + path.Length <= AutoCSer.IO.File.MaxFullNameLength)
                {
                    byte[] contentType = null;
                    bool isCompress = true;
                    fixed (byte* pathFixed = path.Array)
                    {
                        byte* pathStart = pathFixed + path.Start, pathEnd = pathStart + path.Length;
                        if (isFile(pathEnd, ref contentType, ref isCompress) == 0)
                        {
                            if (*pathStart == '/') ++pathStart;
                            for (byte* formatStart = pathStart; formatStart != pathEnd; ++formatStart)
                            {
                                if (*formatStart == ':')
                                {
                                    response = Http.Response.Blank;
                                    return null;
                                }
#if !MONO
                                if ((uint)(*formatStart - 'A') < 26) *formatStart |= 0x20;
#endif
                            }
                            int cachePathLength = (int)(pathEnd - pathStart);
                            FileCacheKey cacheKey = new FileCacheKey(pathIdentity, path.Array, (int)(pathStart - pathFixed), cachePathLength);
                            FileCache fileCache = FileCacheQueue.Get(ref cacheKey);
                            if (fileCache == null)
                            {
                                cacheFileName = StringExtension.FastAllocateString(WorkPath.Length + cachePathLength);
                                fixed (char* nameFixed = cacheFileName)
                                {
                                    char* write = nameFixed + WorkPath.Length;
                                    char directorySeparatorChar = Path.DirectorySeparatorChar;
                                    StringExtension.CopyNotNull(WorkPath, nameFixed);
                                    for (byte* start = pathStart; start != pathEnd; ++start) *write++ = *start == '/' ? directorySeparatorChar : (char)*start;
                                }
                                FileInfo file = new FileInfo(cacheFileName);
                                if (file.Exists)
                                {
                                    string fileName = file.FullName;
                                    if (fileName.Length > WorkPath.Length && WorkPath.equalCaseNotNull(fileName, WorkPath.Length))
                                    {
                                        if (fileName.Length <= AutoCSer.IO.File.MaxFullNameLength && file.Length <= FileCacheQueue.MaxFileSize)
                                        {
                                            if (FileCacheQueue.Get(ref cacheKey, out fileCache, true) != 0)
                                            {
                                                try
                                                {
                                                    fileCache.LastModified = file.LastWriteTimeUtc.UniversalNewBytes();
                                                    int extensionNameLength = (int)(pathEnd - getExtensionNameStart(pathEnd));
                                                    SubArray<byte> fileData = readCacheFile(new SubString { String = fileName, Start = fileName.Length - extensionNameLength, Length = extensionNameLength });
                                                    FileCacheQueue.Set(ref cacheKey, fileCache, fileCache.Set(ref fileData, contentType, cacheControl, isCompress));
                                                    if ((header.Flag & Http.HeaderFlag.IsSetIfModifiedSince) != 0 && header.IfModifiedSinceIndex.Length == fileCache.LastModified.Length)
                                                    {
                                                        if (Memory.EqualNotNull(fileCache.LastModified, pathFixed + header.Buffer.StartIndex + header.IfModifiedSinceIndex.StartIndex, header.IfModifiedSinceIndex.Length))
                                                        {
                                                            response = Http.Response.NotChanged304;
                                                            return null;
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    if (fileCache.IsData == 0)
                                                    {
                                                        fileCache.PulseAll();
                                                        fileCache = null;
                                                        FileCacheQueue.RemoveOnly(ref cacheKey);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if ((header.Flag & Http.HeaderFlag.IsSetIfModifiedSince) != 0 && header.IfModifiedSinceIndex.Length == Date.ToByteLength && Date.UniversalByteEquals(file.LastWriteTimeUtc, header.IfModifiedSince) == 0)
                                            {
                                                response = Http.Response.NotChanged304;
                                                return null;
                                            }
                                            response = Http.Response.Get();
                                            //response.State = Http.ResponseState.Ok200;
                                            response.SetBodyFile(file);
                                            response.CacheControl = cacheControl;
                                            response.ContentType = contentType;
                                            response.SetLastModified(file.LastWriteTimeUtc.UniversalNewBytes());
                                            return null;
                                        }
                                    }
                                }
                            }
                            return fileCache;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error, cacheFileName);
            }
            return null;
        }
        /// <summary>
        /// HTTP文件请求处理
        /// </summary>
        /// <param name="header">请求头部信息</param>
        /// <param name="fileCache">文件输出信息</param>
        /// <param name="response">HTTP响应</param>
        protected unsafe void file(Http.Header header, FileCache fileCache, ref Http.Response response)
        {
            Http.HeaderFlag headerFlag = header.Flag;
            if (fileCache == null)
            {
                if (response != null)
                {
                    if (response.Type == Http.ResponseType.File)
                    {
                        if ((headerFlag & Http.HeaderFlag.IsRange) != 0 && !header.FormatRange(response.BodySize))
                        {
                            response = Http.Response.RangeNotSatisfiable416;
                            return;
                        }
                        if ((headerFlag & Http.HeaderFlag.IsVersion) != 0 || isStaticFileCacheControl(header.Path)) response.CacheControl = AutoCSer.Net.Http.Response.StaticFileCacheControl;
                    }
                    if ((response.Flag & Http.ResponseFlag.IsPool) != 0 && (headerFlag & Http.HeaderFlag.IsSetOrigin) != 0 && this.isOrigin(header.Origin, (headerFlag & Http.HeaderFlag.IsSsl) != 0))
                    {
                        response.SetAccessControlAllowOrigin(header.OriginIndex);
                    }
                }
                return;
            }
            if ((headerFlag & Http.HeaderFlag.IsRange) != 0 && !header.FormatRange(fileCache.Data.Length))
            {
                response = Http.Response.RangeNotSatisfiable416;
                return;
            }
            byte[] cacheControl = (headerFlag & Http.HeaderFlag.IsVersion) != 0 || isStaticFileCacheControl(header.Path) ? AutoCSer.Net.Http.Response.StaticFileCacheControl : this.cacheControl;
            bool isOrigin = (headerFlag & Http.HeaderFlag.IsSetOrigin) != 0 && this.isOrigin(header.Origin, (headerFlag & Http.HeaderFlag.IsSsl) != 0), isHeader = !isOrigin && (headerFlag & Http.HeaderFlag.IsRange) == 0 && FileCacheQueue.IsFileCacheHeader;
            if (isHeader && (response = (headerFlag & Http.HeaderFlag.IsGZip) == 0 ? fileCache.Response : fileCache.GZipResponse) != null && response.IsCacheControl(cacheControl))
            {
                return;
            }
            SubArray<byte> body = (headerFlag & Http.HeaderFlag.IsGZip) != 0 && (headerFlag & Http.HeaderFlag.IsRange) == 0 ? fileCache.GZipData : fileCache.Data;
            response = Http.Response.Get();
            //response.State = Http.ResponseState.Ok200;
            if (isHeader && body.Start == FileCache.HttpHeaderSize) response.SetCanHeaderSize(ref body);
            else response.SetBody(ref body);
            response.CacheControl = cacheControl;
            response.ContentType = fileCache.ContentType;
            if (body.Array != fileCache.Data.Array) response.SetContentEncoding(Http.Response.GZipEncoding);
            response.SetLastModified(fileCache.LastModified);
            if (isOrigin) response.SetAccessControlAllowOrigin(header.OriginIndex);
            return;
        }
        /// <summary>
        /// 是否支持访问控制权限
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="isSsl">是否 SSL 链接</param>
        /// <returns></returns>
        protected virtual bool isOrigin(SubArray<byte> origin, bool isSsl) { return false; }
        /// <summary>
        /// 是否采用静态文件缓存控制策略
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual bool isStaticFileCacheControl(SubArray<byte> path)
        {
            return false;
        }
        /// <summary>
        /// 创建错误输出数据
        /// </summary>
        protected unsafe virtual void createErrorResponse()
        {
            KeyValue<Http.Response, Http.Response>[] errorResponses = new KeyValue<Http.Response, Http.Response>[EnumAttribute<Http.ResponseState>.GetMaxValue(-1) + 1];
            int isResponse = 0;
            try
            {
                byte[] path = new byte[9];
                fixed (byte* pathFixed = path)
                {
                    *pathFixed = (byte)'/';
                    *(int*)(pathFixed + sizeof(int)) = '.' + ('h' << 8) + ('t' << 16) + ('m' << 24);
                    *(pathFixed + sizeof(int) * 2) = (byte)'l';
                    foreach (Http.ResponseState type in System.Enum.GetValues(typeof(Http.ResponseState)))
                    {
                        Http.ResponseStateAttribute state = EnumAttribute<Http.ResponseState, Http.ResponseStateAttribute>.Array((int)type);
                        if (state != null && state.IsError)
                        {
                            int stateValue = state.Number, value = stateValue / 100;
                            *(pathFixed + 1) = (byte)(value + '0');
                            stateValue -= value * 100;
                            *(pathFixed + 2) = (byte)((value = stateValue / 10) + '0');
                            *(pathFixed + 3) = (byte)((stateValue - value * 10) + '0');
                            Http.Response response = null;
                            FileCache fileCache = file(path, default(SubArray<byte>), ref response, true);
                            if (fileCache == null)
                            {
                                if (response != null)
                                {
                                    response.CancelPool();
                                    errorResponses[(int)type].Set(response, response);
                                    isResponse = 1;
                                }
                            }
                            else
                            {
                                Http.Response gzipResponse;
                                if ((response = fileCache.Response) == null)
                                {
                                    response = Http.Response.New();
                                    gzipResponse = Http.Response.New();
                                    SubArray<byte> data = fileCache.Data, gzipData = fileCache.GZipData;
                                    if (FileCacheQueue.IsFileCacheHeader && data.Start == FileCache.HttpHeaderSize)
                                    {
                                        response.SetCanHeaderSize(ref data);
                                        gzipResponse.SetCanHeaderSize(ref gzipData);
                                    }
                                    else
                                    {
                                        response.SetBody(ref data);
                                        gzipResponse.SetBody(ref gzipData);
                                    }
                                    gzipResponse.SetContentEncoding(Http.Response.GZipEncoding);
                                }
                                else gzipResponse = fileCache.GZipResponse ?? response;
                                response.SetState(type);
                                gzipResponse.SetState(type);
                                errorResponses[(int)type].Set(response, gzipResponse);
                                isResponse = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error);
            }
            if (isResponse != 0) this.errorResponses = errorResponses;
        }
        /// <summary>
        /// 是否允许文件扩展名
        /// </summary>
        /// <param name="pathEnd">文件路径</param>
        /// <param name="contentType">文件类型</param>
        /// <param name="isCompress">是否需要压缩</param>
        /// <returns>是否允许文件扩展名</returns>
        protected unsafe virtual int isFile(byte* pathEnd, ref byte[] contentType, ref bool isCompress)
        {
            int code = *(int*)(pathEnd - 4);
            if (code < ('A' << 24))
            {
                switch ((code >> 24) - '3')
                {
                    case '3' - '3':
                        if ((code | 0x202000) == '.' + ('m' << 8) + ('p' << 16) + ('3' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Mp3;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case '4' - '3':
                        if ((code | 0x202000) == '.' + ('m' << 8) + ('p' << 16) + ('4' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Mp4;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                }
            }
            else
            {
                switch (((code >> 24) | 0x20) - 'b')
                {
                    case 'b' - 'b':
                        if ((((code | 0x20202020) ^ ('r' + ('m' << 8) + ('v' << 16) + ('b' << 24))) | (*(pathEnd - 5) ^ '.')) == 0)
                        {
                            contentType = Http.ContentTypeAttribute.Rmvb;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case 'c' - 'b':
                        if ((code | 0x20202000) == '.' + ('d' << 8) + ('o' << 16) + ('c' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Doc;
                            return 0;
                        }
                        return 1;
                    case 'f' - 'b':
                        if (*(pathEnd - 5) == '.')
                        {
                            if ((code | 0x20202020) == 'w' + ('o' << 8) + ('f' << 16) + ('f' << 24))
                            {
                                contentType = Http.ContentTypeAttribute.Woff;
                                isCompress = false;
                                return 0;
                            }
                            return 1;
                        }
                        if ((code |= 0x20202000) == ('.' + ('g' << 8) + ('i' << 16) + ('f' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Gif;
                            isCompress = false;
                            return 0;
                        }
                        if (code == ('.' + ('s' << 8) + ('w' << 16) + ('f' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Swf;
                            return 0;
                        }
                        if (code == ('.' + ('p' << 8) + ('d' << 16) + ('f' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Pdf;
                            return 0;
                        }
                        if (code == '.' + ('o' << 8) + ('t' << 16) + ('f' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Otf;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case 'g' - 'b':
                        if (*(pathEnd - 5) == '.')
                        {
                            if ((code | 0x20202020) == 'j' + ('p' << 8) + ('e' << 16) + ('g' << 24))
                            {
                                contentType = Http.ContentTypeAttribute.Jpeg;
                                isCompress = false;
                                return 0;
                            }
                            return 1;
                        }
                        if ((code |= 0x20202000) == ('.' + ('j' << 8) + ('p' << 16) + ('g' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Jpg;
                            isCompress = false;
                            return 0;
                        }
                        if (code == ('.' + ('p' << 8) + ('n' << 16) + ('g' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Png;
                            isCompress = false;
                            return 0;
                        }
                        if (code == ('.' + ('m' << 8) + ('p' << 16) + ('g' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Mpg;
                            isCompress = false;
                            return 0;
                        }
                        if (code == '.' + ('s' << 8) + ('v' << 16) + ('g' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Svg;
                            return 0;
                        }
                        return 1;
                    case 'i' - 'b':
                        if ((code | 0x20202000) == '.' + ('a' << 8) + ('v' << 16) + ('i' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Avi;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case 'k' - 'b':
                        if ((code | 0x20202000) == '.' + ('a' << 8) + ('p' << 16) + ('k' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Apk;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case 'l' - 'b':
                        if ((code | 0x20202020) == ('h' + ('t' << 8) + ('m' << 16) + ('l' << 24)))
                        {
                            if (*(pathEnd - 5) == '.')
                            {
                                contentType = HtmlContentType;
                                return 0;
                            }
                            return 1;
                        }
                        if ((code | 0x20202000) == '.' + ('x' << 8) + ('m' << 16) + ('l' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Xml;
                            return 0;
                        }
                        return 1;
                    case 'm' - 'b':
                        if ((code | 0x20202000) == ('.' + ('h' << 8) + ('t' << 16) + ('m' << 24)))
                        {
                            contentType = HtmlContentType;
                            return 0;
                        }
                        if ((code | 0x202000ff) == 0xff + ('.' << 8) + ('r' << 16) + ('m' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Rm;
                            isCompress = false;
                        }
                        return 1;
                    case 'o' - 'b':
                        if ((code | 0x20202000) == '.' + ('i' << 8) + ('c' << 16) + ('o' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Ico;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case 'p' - 'b':
                        if ((code |= 0x20202000) == ('.' + ('z' << 8) + ('i' << 16) + ('p' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Zip;
                            isCompress = false;
                            return 0;
                        }
                        if (code == '.' + ('b' << 8) + ('m' << 16) + ('p' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Bmp;
                            return 0;
                        }
                        return 1;
                    case 'r' - 'b':
                        if ((code |= 0x20202000) == ('.' + ('r' << 8) + ('a' << 16) + ('r' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Rar;
                            isCompress = false;
                            return 0;
                        }
                        if (code == '.' + ('c' << 8) + ('u' << 16) + ('r' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Cur;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case 's' - 'b':
                        if ((code | 0x202000ff) == (0xff + ('.' << 8) + ('j' << 16) + ('s' << 24)))
                        {
                            contentType = JsContentType;
                            return 0;
                        }
                        if ((code |= 0x20202000) == ('.' + ('c' << 8) + ('s' << 16) + ('s' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Css;
                            return 0;
                        }
                        if (code == ('.' + ('x' << 8) + ('l' << 16) + ('s' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Xls;
                            return 0;
                        }
                        return 1;
                    case 't' - 'b':
                        if ((code |= 0x20202000) == ('.' + ('t' << 8) + ('x' << 16) + ('t' << 24)))
                        {
                            contentType = Http.ContentTypeAttribute.Txt;
                            return 0;
                        }
                        if (code == '.' + ('e' << 8) + ('o' << 16) + ('t' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Eot;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case 'v' - 'b':
                        if ((code | 0x20202000) == '.' + ('w' << 8) + ('a' << 16) + ('v' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Wav;
                            isCompress = false;
                            return 0;
                        }
                        else if ((code | 0x20202000) == '.' + ('w' << 8) + ('m' << 16) + ('v' << 24))
                        {
                            contentType = Http.ContentTypeAttribute.Wmv;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                    case 'x' - 'b':
                        if (*(pathEnd - 5) == '.')
                        {
                            if ((code |= 0x20202020) == ('d' + ('o' << 8) + ('c' << 16) + ('x' << 24)))
                            {
                                contentType = Http.ContentTypeAttribute.Docx;
                                return 0;
                            }
                            if (code == 'x' + ('l' << 8) + ('s' << 16) + ('x' << 24))
                            {
                                contentType = Http.ContentTypeAttribute.Xlsx;
                                return 0;
                            }
                        }
                        return 1;
                    case 'z' - 'b':
                        if ((code | 0x200000ff) == 0xff + ('.' << 8) + ('7' << 16) + ('z' << 24))
                        {
                            contentType = Http.ContentTypeAttribute._7z;
                            isCompress = false;
                            return 0;
                        }
                        return 1;
                }
            }
            return 1;
        }
        /// <summary>
        /// 读取缓存文件内容
        /// </summary>
        /// <param name="extensionName">文件扩展名</param>
        /// <returns>文件内容</returns>
        protected virtual SubArray<byte> readCacheFile(SubString extensionName)
        {
            return ReadCacheFile(extensionName.String);
        }
        /// <summary>
        /// HTTP文件请求处理
        /// </summary>
        /// <param name="path">请求路径</param>
        /// <param name="ifModifiedSince">文件修改时间</param>
        /// <param name="response">HTTP响应输出</param>
        /// <param name="isCopyPath">是否复制请求路径</param>
        /// <returns>文件缓存</returns>
        protected unsafe FileCache file(byte[] path, SubArray<byte> ifModifiedSince, ref Http.Response response, bool isCopyPath)
        {
            string cacheFileName = null;
            try
            {
                if (path.Length != 0 && WorkPath.Length + path.Length <= AutoCSer.IO.File.MaxFullNameLength)
                {
                    byte[] contentType = null;
                    bool isCompress = true;
                    fixed (byte* pathFixed = path)
                    {
                        byte* pathStart = pathFixed, pathEnd = pathStart + path.Length;
                        if (isFile(pathEnd, ref contentType, ref isCompress) == 0)
                        {
                            if (*pathStart == '/') ++pathStart;
                            for (byte* formatStart = pathStart; formatStart != pathEnd; ++formatStart)
                            {
                                if (*formatStart == ':')
                                {
                                    response = Http.Response.Blank;
                                    return null;
                                }
                            }
                            int cachePathLength = (int)(pathEnd - pathStart);
                            FileCacheKey cacheKey = new FileCacheKey(pathIdentity, path, (int)(pathStart - pathFixed), cachePathLength);
                            FileCache fileCache = FileCacheQueue.Get(ref cacheKey);
                            if (fileCache == null)
                            {
                                cacheFileName = StringExtension.FastAllocateString(WorkPath.Length + cachePathLength);
                                fixed (char* nameFixed = cacheFileName)
                                {
                                    char* write = nameFixed + WorkPath.Length;
                                    char directorySeparatorChar = Path.DirectorySeparatorChar;
                                    StringExtension.CopyNotNull(WorkPath, nameFixed);
                                    for (byte* start = pathStart; start != pathEnd; ++start) *write++ = *start == '/' ? directorySeparatorChar : (char)*start;
                                }
                                FileInfo file = new FileInfo(cacheFileName);
                                if (file.Exists)
                                {
                                    string fileName = file.FullName;
                                    if (fileName.Length > WorkPath.Length && WorkPath.equalCaseNotNull(fileName, WorkPath.Length))
                                    {
                                        if (fileName.Length <= AutoCSer.IO.File.MaxFullNameLength && file.Length <= FileCacheQueue.MaxFileSize)
                                        {
                                            if (FileCacheQueue.Get(ref cacheKey, out fileCache, isCopyPath) != 0)
                                            {
                                                try
                                                {
                                                    fileCache.LastModified = file.LastWriteTimeUtc.UniversalNewBytes();
                                                    int extensionNameLength = (int)(pathEnd - getExtensionNameStart(pathEnd));
                                                    SubArray<byte> fileData = readCacheFile(new SubString { String = fileName, Start = fileName.Length - extensionNameLength, Length = extensionNameLength });
                                                    FileCacheQueue.Set(ref cacheKey, fileCache, fileCache.Set(ref fileData, contentType, cacheControl, isCompress));
                                                    if (ifModifiedSince.Length == fileCache.LastModified.Length)
                                                    {
                                                        fixed (byte* ifModifiedSinceFixed = ifModifiedSince.Array)
                                                        {
                                                            if (Memory.EqualNotNull(fileCache.LastModified, ifModifiedSinceFixed + ifModifiedSince.Start, ifModifiedSince.Length))
                                                            {
                                                                response = Http.Response.NotChanged304;
                                                                return null;
                                                            }
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    if (fileCache.IsData == 0)
                                                    {
                                                        fileCache.PulseAll();
                                                        fileCache = null;
                                                        FileCacheQueue.RemoveOnly(ref cacheKey);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ifModifiedSince.Length == Date.ToByteLength && Date.UniversalByteEquals(file.LastWriteTimeUtc, ifModifiedSince) == 0)
                                            {
                                                response = Http.Response.NotChanged304;
                                                return null;
                                            }
                                            response = Http.Response.Get();
                                            //response.State = Http.ResponseState.Ok200;
                                            response.SetBodyFile(file);
                                            response.CacheControl = cacheControl;
                                            response.ContentType = contentType;
                                            response.SetLastModified(file.LastWriteTimeUtc.UniversalNewBytes());
                                            return null;
                                        }
                                    }
                                }
                            }
                            return fileCache;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                RegisterServer.TcpServer.Log.Add(AutoCSer.Log.LogType.Error, error, cacheFileName);
            }
            return null;
        }

        /// <summary>
        /// 获取扩展名起始位置
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private unsafe static byte* getExtensionNameStart(byte* end)
        {
            byte* start = end;
            while (*--start != '.') ;
            return start + 1;
        }
        /// <summary>
        /// 读取缓存文件内容
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static SubArray<byte> ReadCacheFile(string fileName)
        {
            if (FileCacheQueue.IsFileCacheHeader)
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    int length = (int)fileStream.Length;
                    byte[] data = new byte[FileCache.HttpHeaderSize + length];
                    fileStream.Read(data, FileCache.HttpHeaderSize, length);
                    return new SubArray<byte> { Array = data, Start = FileCache.HttpHeaderSize, Length = length };
                }
            }
            return new SubArray<byte>(File.ReadAllBytes(fileName));
        }
    }
}

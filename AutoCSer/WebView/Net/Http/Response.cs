using System;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Text;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 响应输出
    /// </summary>
    public sealed class Response : AutoCSer.Threading.Link<Response>//, IDisposable
    {
        ///// <summary>
        ///// 输出缓存流
        ///// </summary>
        //private UnmanagedStream bodyStream;
        /// <summary>
        /// 数据缓冲区
        /// </summary>
        internal SubBuffer.PoolBufferFull SubBuffer;
        /// <summary>
        /// 输出内容
        /// </summary>
        internal SubArray<byte> Body;
        /// <summary>
        /// 设置输出数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetBody()
        {
            freeBody();
            Body.Array = NullValue<byte>.Array;
            Type = ResponseType.ByteArray;
        }
        /// <summary>
        /// 释放数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void freeBody()
        {
            switch (Type)
            {
                case ResponseType.ByteArray: Body.Array = NullValue<byte>.Array; return;
                case ResponseType.SubByteArray:
                    Body.Array = NullValue<byte>.Array;
                    Flag &= ResponseFlag.All ^ ResponseFlag.CanHeaderSize;
                    return;
                case ResponseType.SubBuffer:
                    Body.Array = NullValue<byte>.Array;
                    SubBuffer.Free();
                    return;
                case ResponseType.File: BodyFile = null; return;
            }
        }
        /// <summary>
        /// 设置输出数据
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetCanHeaderSize(ref SubArray<byte> data)
        {
            freeBody();
            Flag |= ResponseFlag.CanHeaderSize;
            Body = data;
            Type = ResponseType.SubByteArray;
        }
        /// <summary>
        /// 设置输出数据
        /// </summary>
        /// <param name="data">输出数据</param>
        internal void SetBody(ref SubArray<byte> data)
        {
            if (data.Length == 0)
            {
                SetBody();
                Body.Array = NullValue<byte>.Array;
                Type = ResponseType.ByteArray;
            }
            else
            {
                freeBody();
                if (data.Length == data.Array.Length)
                {
                    Body.Array = data.Array;
                    Type = ResponseType.ByteArray;
                }
                else
                {
                    Body = data;
                    Type = ResponseType.SubByteArray;
                }
            }
        }
        /// <summary>
        /// 设置输出数据
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="encoding"></param>
        internal unsafe void SetBody(CharStream charStream, ref EncodingCache encoding)
        {
            if (charStream.ByteSize == 0) SetBody();
            else
            {
                freeBody();
                int size = encoding.GetByteCountNotNull(charStream);
                AutoCSer.SubBuffer.Pool.GetBuffer(ref SubBuffer, size);
                if (SubBuffer.PoolBuffer.Pool == null)
                {
                    encoding.WriteBytes(charStream, Body.Array = SubBuffer.Buffer);
                    SubBuffer.Buffer = null;
                    Type = ResponseType.ByteArray;
                }
                else
                {
                    Body.Set(SubBuffer.Buffer, SubBuffer.StartIndex, size);
                    encoding.WriteBytes(charStream, ref Body);
                    Type = ResponseType.SubBuffer;
                }
            }
        }
        /// <summary>
        /// 设置输出数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isAscii"></param>
        /// <param name="encoding"></param>
        internal unsafe void SetBody(string value, bool isAscii, ref EncodingCache encoding)
        {
            if (value.Length == 0) SetBody();
            else
            {
                freeBody();
                if (isAscii && encoding.IsAsciiOther != 0)
                {
                    int size = value.Length;
                    AutoCSer.SubBuffer.Pool.GetBuffer(ref SubBuffer, size);
                    fixed (char* textFixed = value)
                    fixed (byte* bufferFixed = SubBuffer.Buffer)
                    {
                        if (SubBuffer.PoolBuffer.Pool == null)
                        {
                            Body.Array = SubBuffer.Buffer;
                            AutoCSer.Extension.StringExtension.WriteBytes(textFixed, size, bufferFixed);
                            SubBuffer.Buffer = null;
                            Type = ResponseType.ByteArray;
                        }
                        else
                        {
                            Body.Set(SubBuffer.Buffer, SubBuffer.StartIndex, size);
                            AutoCSer.Extension.StringExtension.WriteBytes(textFixed, size, bufferFixed + Body.Start);
                            Type = ResponseType.SubBuffer;
                        }
                    }
                }
                else
                {
                    int size = encoding.GetByteCountNotNull(value);
                    AutoCSer.SubBuffer.Pool.GetBuffer(ref SubBuffer, size);
                    if (SubBuffer.PoolBuffer.Pool == null)
                    {
                        encoding.WriteBytesNotEmpty(value, Body.Array = SubBuffer.Buffer);
                        SubBuffer.Buffer = null;
                        Type = ResponseType.ByteArray;
                    }
                    else
                    {
                        Body.Set(SubBuffer.Buffer, SubBuffer.StartIndex, size);
                        encoding.WriteBytesNotEmpty(value, Body.Array, Body.Start);
                        Type = ResponseType.SubBuffer;
                    }
                }
            }
        }
        /// <summary>
        /// 设置输出数据
        /// </summary>
        /// <param name="bodyStream"></param>
        internal unsafe void SetBody(UnmanagedStream bodyStream)
        {
            if (bodyStream.ByteSize == 0) SetBody();
            else
            {
                freeBody();
                bodyStream.GetSubBuffer(ref SubBuffer, 0);
                if (SubBuffer.PoolBuffer.Pool == null)
                {
                    Body.Array = SubBuffer.Buffer;
                    Type = ResponseType.ByteArray;
                    SubBuffer.Buffer = null;
                }
                else
                {
                    Body.Set(SubBuffer.Buffer, SubBuffer.StartIndex, bodyStream.ByteSize);
                    Type = ResponseType.SubBuffer;
                }
            }
        }
        /// <summary>
        /// 输出内容重定向文件
        /// </summary>
        internal FileInfo BodyFile;
        /// <summary>
        /// 输出内容重定向文件
        /// </summary>
        /// <param name="file"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetBodyFile(FileInfo file)
        {
            if (file == null) SetBody();
            else
            {
                freeBody();
                BodyFile = file;
                Type = ResponseType.File;
            }
        }
        /// <summary>
        /// 文件是否有效
        /// </summary>
        internal bool IsFile
        {
            get
            {
                return Type != ResponseType.File || BodyFile.Exists;
            }
        }
        /// <summary>
        /// 输出内容长度
        /// </summary>
        internal long BodySize
        {
            get
            {
                switch (Type)
                {
                    case ResponseType.ByteArray:
                        return Body.Array.Length;
                    case ResponseType.SubByteArray:
                    case ResponseType.SubBuffer:
                        return Body.Length;
                    case ResponseType.File:
                        return BodyFile.Length;
                    default: return 0;
                }
            }
        }
        /// <summary>
        /// HTTP 输出标志位
        /// </summary>
        internal ResponseFlag Flag;
        /// <summary>
        /// HTTP 预留头部字节数量
        /// </summary>
        internal int HeaderSize;
        /// <summary>
        /// 设置 HTTP 预留头部字节数量
        /// </summary>
        /// <param name="headerSize"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetHeaderSize(int headerSize)
        {
            if (headerSize == 0) Flag &= ResponseFlag.All ^ ResponseFlag.HeaderSize;
            else
            {
                Flag |= ResponseFlag.HeaderSize;
                HeaderSize = headerSize;
            }
        }
        /// <summary>
        /// 跨域访问权限
        /// </summary>
        internal BufferIndex AccessControlAllowOrigin;
        /// <summary>
        /// 设置跨域访问权限
        /// </summary>
        /// <param name="index"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetAccessControlAllowOrigin(BufferIndex index)
        {
            if (index.Length == 0) Flag &= ResponseFlag.All ^ ResponseFlag.AccessControlAllowOrigin;
            else
            {
                AccessControlAllowOrigin = index;
                Flag |= ResponseFlag.AccessControlAllowOrigin;
            }
        }
        /// <summary>
        /// 重定向
        /// </summary>
        internal SubArray<byte> Location;
        /// <summary>
        /// 设置重定向
        /// </summary>
        /// <param name="data"></param>
        /// <param name="state"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetLocation(byte[] data, ResponseState state = ResponseState.Found302)
        {
            if (data == null) Location.Set(NullValue<byte>.Array, 0, 0);
            else Location.Set(data, 0, data.Length);
            State = state;
            Flag = (Flag | ResponseFlag.Location | ResponseFlag.State) & (ResponseFlag.All ^ ResponseFlag.AccessControlAllowOrigin);
        }
        /// <summary>
        /// 设置重定向
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="state"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetLocation(byte[] data, int index, int length, ResponseState state = ResponseState.Found302)
        {
            Location.Set(data, index, length);
            State = state;
            Flag = (Flag | ResponseFlag.Location | ResponseFlag.State) & (ResponseFlag.All ^ ResponseFlag.AccessControlAllowOrigin);
        }
        /// <summary>
        /// 设置重定向
        /// </summary>
        /// <param name="data"></param>
        /// <param name="state"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetLocation(ref SubArray<byte> data, ResponseState state = ResponseState.Found302)
        {
            Location = data;
            State = state;
            Flag = (Flag | ResponseFlag.Location | ResponseFlag.State) & (ResponseFlag.All ^ ResponseFlag.AccessControlAllowOrigin);
        }
        /// <summary>
        /// 设置重定向
        /// </summary>
        /// <param name="header"></param>
        /// <param name="path"></param>
        /// <param name="state"></param>
        internal unsafe void SetLocation(Header header, string path, ResponseState state = ResponseState.Found302)
        {
            if (path.Length == 0) Location.Set(NullValue<byte>.Array, 0, 0);
            else
            {
                header.SetResponseLocationRange(path, ref Location);
                fixed (char* pathFixed = path)
                fixed (byte* dataFixed = Location.Array)
                {
                    AutoCSer.Extension.StringExtension.WriteBytes(pathFixed, path.Length, dataFixed + Location.Start);
                }
            }
            State = state;
            Flag = (Flag | ResponseFlag.Location | ResponseFlag.State) & (ResponseFlag.All ^ ResponseFlag.AccessControlAllowOrigin);
        }
        /// <summary>
        /// Cookie 集合
        /// </summary>
        internal Cookie Cookie;
        /// <summary>
        /// 获取 Cookie 并清除数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal Cookie GetCookieClear()
        {
            Cookie cookie = Cookie;
            Cookie = null;
            return cookie;
        }
        /// <summary>
        /// 获取 Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Cookie GetCookie(byte[] name)
        {
            if ((Flag & ResponseFlag.Cookie) != 0)
            {
                Cookie cookie = Cookie;
                do
                {
                    if (AutoCSer.Memory.EqualNotNull(name, cookie.Name)) return cookie;
                }
                while ((cookie= cookie.LinkNext) != null);
            }
            return null;
        }
        /// <summary>
        /// 添加 Cookie
        /// </summary>
        /// <param name="cookie"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AppendCookie(Cookie cookie)
        {
            if ((Flag & ResponseFlag.Cookie) == 0) Flag |= ResponseFlag.Cookie;
            else cookie.LinkNext = Cookie;
            Cookie = cookie;
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        internal byte[] LastModifiedData;
        /// <summary>
        /// 设置最后修改时间
        /// </summary>
        public DateTime LastModified
        {
            set
            {
                LastModifiedData = (value.Kind == DateTimeKind.Local ? value.localToUniversalTime() : value).UniversalNewBytes();
                Flag |= lastModifiedFlag;
            }
        }
        /// <summary>
        /// 设置重定向
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetLastModified(byte[] data)
        {
            if (data == null || data.Length == 0) Flag &= ResponseFlag.All ^ ResponseFlag.LastModified;
            else
            {
                LastModifiedData = data;
                Flag |= lastModifiedFlag;
            }
        }
        /// <summary>
        /// 最后修改时间标志位
        /// </summary>
        private static readonly ResponseFlag lastModifiedFlag = SocketBase.Config.IsResponseLastModified ? ResponseFlag.LastModified : ResponseFlag.None;
        /// <summary>
        /// 缓存参数
        /// </summary>
        internal byte[] CacheControlData;
        /// <summary>
        /// 设置缓存参数
        /// </summary>
        public byte[] CacheControl
        {
            set
            {
                if (value == null || value.Length == 0) Flag &= ResponseFlag.All ^ ResponseFlag.CacheControl;
                else
                {
                    CacheControlData = value;
                    Flag |= cacheControlFlag;
                }
            }
        }
        /// <summary>
        /// 当缺少缓存控制参数是设置为默认静态文件缓存控制参数
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void TryStaticFileCacheControl()
        {
            if ((Flag & ResponseFlag.CacheControl) == 0)
            {
                CacheControlData = StaticFileCacheControl;
                Flag |= cacheControlFlag;
            }
        }
        /// <summary>
        /// 设置非缓存参数输出
        /// </summary>
        /// <param name="contentType"></param>
        internal void NoStore200(byte[] contentType)
        {
            if (ResponseState == Http.ResponseState.Ok200)
            {
                if (ContentTypeData == null)
                {
                    ContentTypeData = contentType;
                    Flag |= contentTypeFlag;
                }
                if (((Flag & ResponseFlag.LastModified) == 0 || LastModifiedData == null)
                    && ((Flag & ResponseFlag.CacheControl) == 0 || CacheControlData == null)
                    && ((Flag & ResponseFlag.ETag) == 0 || ETagData == null))
                {
                    CacheControlData = noStoreBytes;
                    Flag |= cacheControlFlag;
                }
            }
        }
        /// <summary>
        /// 判断缓存参数是否匹配
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsCacheControl(byte[] data)
        {
            return ((Flag & ResponseFlag.CacheControl) == 0 ? null : CacheControlData) == data;
        }
        /// <summary>
        /// 缓存参数 HTTP 输出标志位
        /// </summary>
        private static readonly ResponseFlag cacheControlFlag = SocketBase.Config.IsResponseCacheControl ? ResponseFlag.CacheControl : ResponseFlag.None;
        /// <summary>
        /// 输出内容类型
        /// </summary>
        internal byte[] ContentTypeData;
        /// <summary>
        /// 设置输出内容类型
        /// </summary>
        public byte[] ContentType
        {
            set
            {
                if (value == null || value.Length == 0) Flag &= ResponseFlag.All ^ ResponseFlag.ContentType;
                else
                {
                    ContentTypeData = value;
                    Flag |= contentTypeFlag;
                }
            }
        }
        /// <summary>
        /// 设置输出内容类型
        /// </summary>
        /// <param name="type"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetContentType(ResponseContentType type)
        {
            ContentType = AutoCSer.Net.Http.ContentTypeAttribute.Get(type);
        }
        /// <summary>
        /// 设置 JS 内容类型
        /// </summary>
        /// <param name="request">WEB 请求</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetJsContentType(AutoCSer.WebView.Request request)
        {
            SetJsContentType(request.DomainServer);
        }
        /// <summary>
        /// 设置 JS 内容类型
        /// </summary>
        /// <param name="domainServer">域名服务</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetJsContentType(HttpDomainServer.FileServer domainServer)
        {
            ContentTypeData = domainServer.JsContentType;
            Flag |= contentTypeFlag;
        }
        /// <summary>
        /// 输出内容类型 HTTP 输出标志位
        /// </summary>
        private static readonly ResponseFlag contentTypeFlag = SocketBase.Config.IsResponseContentType ? ResponseFlag.ContentType : ResponseFlag.None;
        /// <summary>
        /// 输出内容压缩编码
        /// </summary>
        internal byte[] ContentEncoding;
        /// <summary>
        /// 设置输出内容压缩编码
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetContentEncoding(byte[] data)
        {
            if (data == null || data.Length == 0) Flag &= ResponseFlag.All ^ ResponseFlag.ContentEncoding;
            else
            {
                ContentEncoding = data;
                Flag |= ResponseFlag.ContentEncoding;
            }
        }
        /// <summary>
        /// 缓存匹配标识
        /// </summary>
        internal byte[] ETagData;
        /// <summary>
        /// 设置缓存匹配标识
        /// </summary>
        public byte[] ETag
        {
            set
            {
                if (value == null || value.Length == 0) Flag &= ResponseFlag.All ^ ResponseFlag.ETag;
                else
                {
                    ETagData = value;
                    Flag |= ResponseFlag.ETag;
                }
            }
        }
        /// <summary>
        /// 内容描述
        /// </summary>
        internal byte[] ContentDispositionData;
        /// <summary>
        /// 设置内容描述
        /// </summary>
        public byte[] ContentDisposition
        {
            set
            {
                if (value == null || value.Length == 0) Flag &= ResponseFlag.All ^ ResponseFlag.ContentDisposition;
                else
                {
                    ContentDispositionData = value;
                    Flag |= ResponseFlag.ContentDisposition;
                }
            }
        }
        /// <summary>
        /// HTTP 响应状态
        /// </summary>
        internal ResponseState State;
        /// <summary>
        /// HTTP 响应状态
        /// </summary>
        internal ResponseState ResponseState
        {
            get
            {
                if ((Flag & ResponseFlag.State) == 0)
                {
                    Flag |= ResponseFlag.State;
                    return State = ResponseState.Ok200;
                }
                return State;
            }
        }
        /// <summary>
        /// 设置 HTTP 响应状态
        /// </summary>
        /// <param name="state"></param>
        internal void SetState(ResponseState state)
        {
            State = state;
            Flag |= ResponseFlag.State;
        }
        /// <summary>
        /// HTTP 响应输出类型
        /// </summary>
        internal ResponseType Type;
#if DOTNET2 || DOTNET4
        /// <summary>
        /// 压缩数据
        /// </summary>
        internal void Compress()
#else
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="isFastestCompressionLevel"></param>
        internal void Compress(bool isFastestCompressionLevel)
#endif
        {
            switch (Type)
            {
                case Net.Http.ResponseType.ByteArray:
                    Body.SetFull();
                    goto SUBBYTEARRAY;
                case Net.Http.ResponseType.SubByteArray:
            SUBBYTEARRAY:
                    if (Body.Length > GZipHeaderSize + 256)
                    {
                        SubArray<byte> compressData = default(SubArray<byte>);
                        try
                        {
#if DOTNET2 || DOTNET4
                            if (AutoCSer.IO.Compression.GzipCompressor.Get(Body.Array, Body.Start, Body.Length, ref SubBuffer, ref compressData, 0, GZipHeaderSize))
#else
                            if (AutoCSer.IO.Compression.GzipCompressor.Get(Body.Array, Body.Start, Body.Length, ref SubBuffer, ref compressData, 0, GZipHeaderSize, isFastestCompressionLevel))
#endif
                            {
                                Body = compressData;
                                Type = compressData.Array == SubBuffer.Buffer ? ResponseType.SubBuffer : ResponseType.SubByteArray;
                                ContentEncoding = GZipEncoding;
                                Flag |= ResponseFlag.ContentEncoding;
                            }
                        }
                        finally
                        {
                            if (Type != ResponseType.SubBuffer) SubBuffer.Free();
                        }
                    }
                    return;
                case Net.Http.ResponseType.SubBuffer:
                    if (Body.Length > GZipHeaderSize + 256)
                    {
                        SubBuffer.PoolBufferFull compressBuffer = default(SubBuffer.PoolBufferFull);
                        SubArray<byte> compressData = default(SubArray<byte>);
                        byte isCompress = 0;
                        try
                        {
#if DOTNET2 || DOTNET4
                            if (AutoCSer.IO.Compression.GzipCompressor.Get(Body.Array, Body.Start, Body.Length, ref compressBuffer, ref compressData, 0, GZipHeaderSize))
#else
                            if (AutoCSer.IO.Compression.GzipCompressor.Get(Body.Array, Body.Start, Body.Length, ref compressBuffer, ref compressData, 0, GZipHeaderSize, isFastestCompressionLevel))
#endif
                            {
                                isCompress = 1;
                                SubBuffer.Free();
                                Body = compressData;
                                if (compressData.Array == compressBuffer.Buffer) SubBuffer = compressBuffer;
                                else Type = ResponseType.SubByteArray;
                                ContentEncoding = GZipEncoding;
                                Flag |= ResponseFlag.ContentEncoding;
                            }
                        }
                        finally
                        {
                            if (isCompress == 0 || Type != ResponseType.SubBuffer) compressBuffer.Free();
                        }
                    }
                    return;
            }
        }
        /// <summary>
        ///  析构释放资源
        /// </summary>
        ~Response()
        {
            if (Type == ResponseType.SubBuffer) SubBuffer.Free();
        }
        ///// <summary>
        ///// 释放资源
        ///// </summary>
        //public void Dispose()
        //{
        //    if (Type == ResponseType.SubBuffer)
        //    {
        //        SubBuffer.Free();
        //        Body.Array = NullValue<byte>.Array;
        //        Type = ResponseType.ByteArray;
        //    }
        //}
        /// <summary>
        /// 取消使用HTTP响应池
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CancelPool()
        {
            Flag &= ResponseFlag.All ^ ResponseFlag.IsPool;
        }
        /// <summary>
        /// 添加到 HTTP 响应池
        /// </summary>
        internal void Push()
        {
            if ((Flag & ResponseFlag.IsPool) != 0)
            {
                freeBody();
                Type = ResponseType.ByteArray;
                Flag = ResponseFlag.IsPool;
                if (YieldPool.Default.IsPushNotNull(this) != 0) return;
            }
            if (Type == ResponseType.SubBuffer)
            {
                Body.Array = NullValue<byte>.Array;
                SubBuffer.Free();
                Type = ResponseType.ByteArray;
            }
        }

        /// <summary>
        /// 获取HTTP响应
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static Response Get()
        {
            Response response = YieldPool.Default.Pop();
            if (response == null)
            {
                response = new Response { Flag = ResponseFlag.IsPool };
                response.Body.Array = NullValue<byte>.Array;
            }
            return response;
        }
        /// <summary>
        /// 添加到 HTTP 响应池
        /// </summary>
        /// <param name="response">HTTP响应</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Push(ref Response response)
        {
            Response value = response;
            if (value != null)
            {
                response = null;
                value.Push();
            }
        }
        /// <summary>
        /// 获取HTTP响应
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Response New()
        {
            Response response = new Response { Flag = ResponseFlag.None };
            response.Body.Array = NullValue<byte>.Array;
            return response;
        }

        /// <summary>
        /// 非缓存参数输出
        /// </summary>
        private static readonly byte[] noStoreBytes = ("public, no-store").getBytes();
        /// <summary>
        /// 缓存过期
        /// </summary>
        internal static readonly byte[] ZeroAgeBytes = ("public, max-age=0").getBytes();
        /// <summary>
        /// 缓存控制参数
        /// </summary>
        internal const int StaticFileCacheControlSeconds = 10 * 365 * 24 * 60 * 60;
        /// <summary>
        /// 默认静态文件缓存控制参数
        /// </summary>
        internal static readonly byte[] StaticFileCacheControl = ("public, max-age=" + StaticFileCacheControlSeconds.toString()).getBytes();
        /// <summary>
        /// GZIP压缩响应头部
        /// </summary>
        internal static readonly byte[] GZipEncoding = new byte[] { (byte)'g', (byte)'z', (byte)'i', (byte)'p' };
        /// <summary>
        /// GZIP压缩响应头部字节尺寸
        /// </summary>
        internal static readonly int GZipHeaderSize = HeaderName.ContentEncoding.Length + GZipEncoding.Length + 2;
        /// <summary>
        /// 空页面输出
        /// </summary>
        internal static readonly Response Blank = new Response
        {
            Body = new SubArray<byte>(NullValue<byte>.Array),
            Flag = ResponseFlag.State,
            State = ResponseState.Ok200,
            CacheControlData = StaticFileCacheControl,
            LastModifiedData = Date.NowTime.Now.ToBytes()
        };
        /// <summary>
        /// 资源未修改
        /// </summary>
        internal static readonly Response NotChanged304 = new Response { Body = new SubArray<byte>(NullValue<byte>.Array), Flag = ResponseFlag.State, State = ResponseState.NotChanged304 };
        /// <summary>
        /// Range 请求无效
        /// </summary>
        internal static readonly Response RangeNotSatisfiable416 = new Response { Body = new SubArray<byte>(NullValue<byte>.Array), Flag = ResponseFlag.State, State = ResponseState.RangeNotSatisfiable416 };
#if DOTNET2 || DOTNET4
        /// <summary>
        /// 压缩处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="compressData"></param>
        /// <param name="seek"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool GetCompress(ref SubArray<byte> data, ref SubArray<byte> compressData, int seek)
        {
            return data.Length > GZipHeaderSize + 256 && AutoCSer.IO.Compression.GzipCompressor.Get(ref data, ref compressData, seek, GZipHeaderSize);
        }
#else
        /// <summary>
        /// 压缩处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="compressData"></param>
        /// <param name="seek"></param>
        /// <param name="isFastest"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool GetCompress(ref SubArray<byte> data, ref SubArray<byte> compressData, int seek, bool isFastest)
        {
            return data.Length > GZipHeaderSize + 256 && AutoCSer.IO.Compression.GzipCompressor.Get(ref data, ref compressData, seek, GZipHeaderSize, isFastest);
        }
#endif
    }
}

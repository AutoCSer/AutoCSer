using System;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.WebClient
{
    /// <summary>
    /// URI请求信息
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Request
    {
        /// <summary>
        /// 页面地址
        /// </summary>
        public Uri Uri;
        /// <summary>
        /// 页面地址
        /// </summary>
        public string UriString
        {
#if MONO
                set { Uri = new Uri(value); }
#else
            set { Uri = UriCreator.Create(value); }
#endif
        }
        /// <summary>
        /// POST内容
        /// </summary>
        public NameValueCollection Form;
        /// <summary>
        /// POST内容
        /// </summary>
        public byte[] UploadData;
        /// <summary>
        /// 来源页面地址
        /// </summary>
        public string RefererUrl;
        /// <summary>
        /// 出错时是否写日志
        /// </summary>
        public bool IsErrorOut;
        /// <summary>
        /// 出错时是否输出页面地址
        /// </summary>
        public bool IsErrorOutUri;
        /// <summary>
        /// 清除请求信息
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Clear()
        {
            Uri = null;
            Form = null;
            UploadData = null;
            RefererUrl = null;
            IsErrorOut = IsErrorOutUri = true;
        }
        /// <summary>
        /// URI隐式转换为请求信息
        /// </summary>
        /// <param name="uri">URI</param>
        /// <returns>请求信息</returns>
        public static implicit operator Request(Uri uri) { return new Request { Uri = uri, IsErrorOut = true, IsErrorOutUri = true }; }
        /// <summary>
        /// URI隐式转换为请求信息
        /// </summary>
        /// <param name="uri">URI</param>
        /// <returns>请求信息</returns>
        public static implicit operator Request(string uri)
        {
            return new Request { Uri = new Uri(uri), IsErrorOut = true, IsErrorOutUri = true };
        }
    }
}

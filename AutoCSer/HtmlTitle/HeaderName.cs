using System;
using AutoCSer.Extension;

namespace AutoCSer.Net.HtmlTitle
{
    /// <summary>
    /// HTTP 头名称唯一哈希
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct HeaderName : IEquatable<HeaderName>
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        public const string Connection = "Connection";
        /// <summary>
        /// 连接状态
        /// </summary>
        public static readonly byte[] ConnectionBytes = Connection.getBytes();
        /// <summary>
        /// 压缩编码名称
        /// </summary>
        public static readonly byte[] ContentEncodingBytes = AutoCSer.Net.Http.HeaderName.ContentEncoding.getBytes();
        /// <summary>
        /// 内容长度名称
        /// </summary>
        public const string ContentLength = "Content-Length";
        /// <summary>
        /// 内容长度名称
        /// </summary>
        public static readonly byte[] ContentLengthBytes = ContentLength.getBytes();
        /// <summary>
        /// 内容类型名称
        /// </summary>
        public static readonly byte[] ContentTypeBytes = AutoCSer.Net.Http.HeaderName.ContentType.getBytes();
        /// <summary>
        /// 传输编码
        /// </summary>
        public const string TransferEncoding = "Transfer-Encoding";
        /// <summary>
        /// 传输编码名称
        /// </summary>
        public static readonly byte[] TransferEncodingBytes = TransferEncoding.getBytes();

        //string[] keys = new string[] { "transfer-encoding", "content-length", "content-type","content-encoding", "connection" };
        /// <summary>
        /// HTTP头名称
        /// </summary>
        public SubArray<byte> Name;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">HTTP头名称</param>
        /// <returns>HTTP头名称唯一哈希</returns>
        public static implicit operator HeaderName(byte[] name) { return new HeaderName { Name = new SubArray<byte>(name) }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="name">HTTP头名称</param>
        /// <returns>HTTP头名称唯一哈希</returns>
        public static implicit operator HeaderName(SubArray<byte> name) { return new HeaderName { Name = name }; }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>哈希值</returns>
        public unsafe override int GetHashCode()
        {
            return Name.Length < 10 ? 0 : Name.Array[Name.StartIndex + Name.Length - 10] & 7;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">待匹配数据</param>
        /// <returns>是否相等</returns>
        public unsafe bool Equals(HeaderName other)
        {
            return Name.EqualCase(ref other.Name);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj">待匹配数据</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            return Equals((HeaderName)obj);
        }
    }
}

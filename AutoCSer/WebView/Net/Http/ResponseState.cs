using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 状态类型
    /// </summary>
    public enum ResponseState : byte
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        Unknown,
        /// <summary>
        /// 允许客户端继续发送数据
        /// </summary>
        [ResponseState(Number = 100, Text = @" 100 Continue
")]
        Continue100,
        /// <summary>
        /// WebSocket握手
        /// </summary>
        [ResponseState(Number = 101, Text = @" 101 Switching Protocols
")]
        WebSocket101,
        /// <summary>
        /// 客户端请求成功
        /// </summary>
        [ResponseState(Number = 200, Text = @" 200 OK
")]
        Ok200,
        /// <summary>
        /// 成功处理了Range头的GET请求
        /// </summary>
        [ResponseState(Number = 206, Text = @" 206 Partial Content
")]
        PartialContent206,
        /// <summary>
        /// 永久重定向
        /// </summary>
        [ResponseState(Number = 301, Text = @" 301 Moved Permanently
")]
        MovedPermanently301,
        /// <summary>
        /// 临时重定向
        /// </summary>
        [ResponseState(Number = 302, Text = @" 302 Found
")]
        Found302,
        /// <summary>
        /// 资源未修改
        /// </summary>
        [ResponseState(Number = 304, Text = @" 304 Not Changed
")]
        NotChanged304,
        /// <summary>
        /// 客户端请求有语法错误，不能被服务器所理解
        /// </summary>
        [ResponseState(IsError = true, Number = 400, Text = @" 400 Bad Request
")]
        BadRequest400,
        /// <summary>
        /// 请求未经授权，这个状态代码必须和WWW-Authenticate报头域一起使用
        /// </summary>
        [ResponseState(IsError = true, Number = 401, Text = @" 401 Unauthorized
")]
        Unauthorized401,
        /// <summary>
        /// 服务器收到请求，但是拒绝提供服务
        /// WWW-Authenticate响应报头域必须被包含在401（未授权的）响应消息中，客户端收到401响应消息时候，并发送Authorization报头域请求服务器对其进行验证时，服务端响应报头就包含该报头域。
        /// eg：WWW-Authenticate:Basic realm="Basic Auth Test!"  可以看出服务器对请求资源采用的是基本验证机制。
        /// </summary>
        [ResponseState(IsError = true, Number = 403, Text = @" 403 Forbidden
")]
        Forbidden403,
        /// <summary>
        /// 请求资源不存在
        /// </summary>
        [ResponseState(IsError = true, Number = 404, Text = @" 404 Not Found
")]
        NotFound404,
        /// <summary>
        /// 不允许使用的方法
        /// </summary>
        [ResponseState(IsError = true, Number = 405, Text = @" 405 Method Not Allowed
")]
        MethodNotAllowed405,
        /// <summary>
        /// Request Timeout
        /// </summary>
        [ResponseState(IsError = true, Number = 408, Text = @" 408 Request Timeout
")]
        RequestTimeout408,
        /// <summary>
        /// Range请求无效
        /// </summary>
        [ResponseState(IsError = true, Number = 416, Text = @" 416 Request Range Not Satisfiable
")]
        RangeNotSatisfiable416,
        /// <summary>
        /// 服务器发生不可预期的错误
        /// </summary>
        [ResponseState(IsError = true, Number = 500, Text = @" 500 Internal Server Error
")]
        ServerError500,
        /// <summary>
        /// 服务器当前不能处理客户端的请求，一段时间后可能恢复正常
        /// </summary>
        [ResponseState(IsError = true, Number = 503, Text = @" 503 Server Unavailable
")]
        ServerUnavailable503,
    }
    /// <summary>
    /// HTTP 响应状态
    /// </summary>
    internal sealed class ResponseStateAttribute : Attribute
    {
        /// <summary>
        /// 状态输出文本
        /// </summary>
        public string Text;
        /// <summary>
        /// 编号
        /// </summary>
        public int Number;
        /// <summary>
        /// 是否错误状态类型
        /// </summary>
        public bool IsError;
        /// <summary>
        /// 写入状态
        /// </summary>
        /// <param name="data"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal unsafe void Write(byte* data)
        {
            fixed (char* textFixed = Text)
            {
                char* read = textFixed;
                for (byte* end = data + Text.Length; data != end; *data++ = (byte)*read++) ;
            }
        }
    }
}

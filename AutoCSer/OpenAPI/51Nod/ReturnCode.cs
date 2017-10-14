using System;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// API返回码枚举
    /// </summary>
    public enum ReturnCode
    {
        /// <summary>
        /// 未知错误，可能是反序列化失败
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 服务器端异常
        /// </summary>
        ServerError = 2,
        /// <summary>
        /// 找不到Email用户
        /// </summary>
        NotFoundEmail = 3,
        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordError = 4,
        /// <summary>
        /// 令牌错误或者过期
        /// </summary>
        TokenError = 5,
        /// <summary>
        /// 用户没有相关权限
        /// </summary>
        PermissionError = 6,
        /// <summary>
        /// 没有找到题目
        /// </summary>
        NotFoundProblem = 7,
        /// <summary>
        /// 没有在上传网络流中找到文件内容
        /// </summary>
        NotFoundUploadFile = 8,
        /// <summary>
        /// 没有设置回调URL
        /// </summary>
        NotFoundCallbackUrl = 9,
        /// <summary>
        /// 不支持的程序语言
        /// </summary>
        ProgramLanguageError = 10,
    }
}

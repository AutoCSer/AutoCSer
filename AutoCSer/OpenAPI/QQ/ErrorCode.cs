using System;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 二级错误码
    /// </summary>
    public abstract class ErrorCode : Return
    {
#pragma warning disable
        /// <summary>
        /// 二级错误码
        /// </summary>
        public int errcode;
#pragma warning restore
        /// <summary>
        /// 提示信息
        /// </summary>
        public override string Message
        {
            get
            {
                return "[" + ret.toString() + ":" + errcode.toString() + "]" + msg;
            }
        }
    }
}

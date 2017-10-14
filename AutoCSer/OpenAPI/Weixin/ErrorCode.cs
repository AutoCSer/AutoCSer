using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// XML返回值错误代码
    /// </summary>
    public abstract class ErrorCode : ResultCode
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        internal ErrorCodeEnum? err_code;

        /// <summary>
        /// 提示信息
        /// </summary>
        public override string Message
        {
            get
            {
                return return_code == ReturnCodeEnum.SUCCESS ? (err_code == null ? null : ((ErrorCodeEnum)err_code).ToString()) + @"
" + err_code_des : return_msg;
            }
        }
    }
}

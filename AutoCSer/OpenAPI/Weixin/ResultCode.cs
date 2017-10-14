using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// XML返回值
    /// </summary>
    public abstract class ResultCode : ReturnCode, IReturn
    {
        /// <summary>
        /// 错误代码描述
        /// </summary>
        internal string err_code_des;
        /// <summary>
        /// 业务结果
        /// </summary>
        internal ReturnCodeEnum result_code;

        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsReturn
        {
            get { return result_code == ReturnCodeEnum.SUCCESS; }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public virtual string Message
        {
            get
            {
                return return_code == ReturnCodeEnum.SUCCESS ? err_code_des : return_msg;
            }
        }
    }
}

using System;
using AutoCSer.Extension;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 数据是否有效
    /// </summary>
    public class Return : IReturn
    {
#pragma warning disable
        /// <summary>
        /// 全局返回码
        /// </summary>
        private int errcode;
        /// <summary>
        /// 错误信息
        /// </summary>
        private string errmsg;
#pragma warning restore
        /// <summary>
        /// 系统繁忙，此时请开发者稍候再试
        /// </summary>
        internal bool IsBusy
        {
            get { return errcode == -1; }
        }
        /// <summary>
        /// 访问令牌是否错误
        /// </summary>
        internal bool IsTokenError
        {
            get { return errcode == 40014 || errcode == 42001; }
        }
        /// <summary>
        /// 访问令牌是否过期
        /// </summary>
        internal bool IsTokenExpired
        {
            get { return errcode == 42001; }
        }
        /// <summary>
        /// 数据是否有效
        /// </summary>
        public bool IsReturn
        {
            get { return errcode == 0; }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get
            {
                return errcode.toString() + @"
" + errmsg;
            }
        }
    }
}

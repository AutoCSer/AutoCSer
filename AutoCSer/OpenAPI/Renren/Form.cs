using System;

namespace AutoCSer.OpenAPI.Renren
{
    /// <summary>
    /// API调用表单
    /// </summary>
    public abstract class Form
    {
        /// <summary>
        /// 调用名称
        /// </summary>
        internal string method;
        /// <summary>
        /// API的版本号，固定值为1.0
        /// </summary>
        internal string v = "1.0";
        /// <summary>
        /// OAuth2.0验证授权后获得的token。同时兼容session_key方式调用。（必填项）
        /// </summary>
        internal string access_token;
        /// <summary>
        /// 返回值的格式。请指定为JSON或者XML，推荐使用JSON，缺省值为XML
        /// </summary>
        internal string format = "JSON";
    }
}

using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// XML返回值
    /// </summary>
    [AutoCSer.Xml.Serialize(Filter = Metadata.MemberFilters.InstanceField, IsBaseType = false)]//, IsAllMember = true
    public class ReturnCode
    {
        /// <summary>
        /// 返回信息 返回信息，如非空，为错误原因 签名失败 参数格式校验错误
        /// </summary>
        internal string return_msg;
        /// <summary>
        /// 返回状态码，此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断[必填]
        /// </summary>
        internal ReturnCodeEnum return_code;
        /// <summary>
        /// 获取成功返回值
        /// </summary>
        /// <returns></returns>
        public static ReturnCode GetSuccess()
        {
            return new ReturnCode { return_code = ReturnCodeEnum.SUCCESS };
        }
    }
}

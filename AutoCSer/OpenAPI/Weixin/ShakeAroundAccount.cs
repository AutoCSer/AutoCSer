using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 摇一摇周边注册
    /// </summary>
    public sealed class ShakeAroundAccount
    {
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string name;
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string phone_number;
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string email;
        /// <summary>
        /// 平台定义的行业代号，http://3gimg.qq.com/shake_nearby/Qualificationdocuments.html
        /// </summary>
        public string industry_id;
        /// <summary>
        /// 相关资质文件的图片url，图片需先上传至微信侧服务器，用“素材管理-上传图片素材”接口上传图片，返回的图片URL再配置在此处；当不需要资质文件时，数组内可以不填写url
        /// </summary>
        public string[] qualification_cert_urls;
        /// <summary>
        /// 申请理由
        /// </summary>
        public string apply_reason;
    }
}

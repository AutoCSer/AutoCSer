using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 查询申请开通摇一摇周边审核状态
    /// </summary>
    public sealed class ShakeAroundAccountStatus
    {
        /// <summary>
        /// 提交申请的时间戳
        /// </summary>
        public long apply_time;
        /// <summary>
        /// 确定审核结果的时间戳；若状态为审核中，则该时间值为0
        /// </summary>
        public long audit_time;
        /// <summary>
        /// 审核备注，包括审核不通过的原因
        /// </summary>
        public string audit_comment;
        /// <summary>
        /// 审核状态。0：审核未通过、1：审核中、2：审核已通过；审核会在三个工作日内完成
        /// </summary>
        public byte audit_status;
    }
}

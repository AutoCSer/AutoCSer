using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 用户增减数据
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct UserSummary
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public string ref_date;
        /// <summary>
        /// 用户的渠道，0代表其他（包括带参数二维码） 3代表扫二维码 17代表名片分享 35代表搜号码（即微信添加朋友页的搜索） 39代表查询微信公众帐号 43代表图文页右上角菜单
        /// </summary>
        public int user_source;
        /// <summary>
        /// 新增的用户数量
        /// </summary>
        public int new_user;
        /// <summary>
        /// 取消关注的用户数量，new_user减去cancel_user即为净增用户数量
        /// </summary>
        public int cancel_user;
    }
}

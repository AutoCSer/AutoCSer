using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 图文分享转发数据
    /// </summary>
    public class UserShare
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public string ref_date;
        /// <summary>
        /// 分享的次数
        /// </summary>
        public int share_count;
        /// <summary>
        /// 分享的人数
        /// </summary>
        public int share_user;
        /// <summary>
        /// 分享的场景 1代表好友转发 2代表朋友圈 3代表腾讯微博 255代表其他
        /// </summary>
        public byte share_scene;
    }
}

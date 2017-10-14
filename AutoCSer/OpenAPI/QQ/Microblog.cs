using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// 微博信息
    /// </summary>
    public partial class Microblog : Format
    {
        /// <summary>
        /// 微博编码类型
        /// </summary>
        public static readonly OpenAPI.Microblog Encoding = new OpenAPI.Microblog
        {
            Encoding = OpenAPI.MicroblogEncoding.Utf8,
            EncodingSize = 140 * 3,
            UrlSize = 11
        };
        /// <summary>
        /// 表示要发表的微博内容，必须项。必须为UTF-8编码，最长为140个汉字，也就是420字节。如果微博内容中有URL，后台会自动将该URL转换为短URL，每个URL折算成11个字节。若在此处@好友，需正确填写好友的微博账号，而非昵称。
        /// </summary>
        public string content;
        /// <summary>
        /// 用户ip，必须正确填写用户侧真实ip，不能为内网ip及以127或255开头的ip，以分析用户所在地
        /// </summary>
        public string clientip;
        /// <summary>
        /// 用户所在地理位置的经度，为实数，最多支持10位有效数字。有效范围：-180.0到+180.0，+表示东经，默认为0.0
        /// </summary>
        public string longitude;
        /// <summary>
        /// 用户所在地理位置的纬度，为实数，最多支持10位有效数字。有效范围：-90.0到+90.0，+表示北纬，默认为0.0。
        /// </summary>
        public string latitude;
        /// <summary>
        /// 容错标志，支持按位操作，默认为0。0x20：微博内容长度超过140字则报错；0：以上错误均做容错处理，即发表普通微博
        /// </summary>
        public string compatibleflag;
    }
}

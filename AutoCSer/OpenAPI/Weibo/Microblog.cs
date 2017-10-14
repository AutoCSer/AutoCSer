using System;

namespace AutoCSer.OpenAPI.Weibo
{
    /// <summary>
    /// 微博信息
    /// </summary>
    public partial class Microblog : Form
    {
        /// <summary>
        /// 微博编码类型
        /// </summary>
        public static readonly OpenAPI.Microblog Encoding = new OpenAPI.Microblog
        {
            Encoding = OpenAPI.MicroblogEncoding.WordByte,
            EncodingSize = 140 * 2,
            UrlSize = 0
        };
        /// <summary>
        /// 要发布的微博文本内容，必须做URLencode，内容不超过140个汉字，必选项
        /// </summary>
        public string status;
        /// <summary>
        /// 微博的可见性，0：所有人能看，1：仅自己可见，2：密友可见，3：指定分组可见，默认为0。
        /// </summary>
        public int visible;
        /// <summary>
        /// 微博的保护投递指定分组ID，只有当visible参数为3时生效且必选
        /// </summary>
        public string list_id;
        /// <summary>
        /// 纬度，有效范围：-90.0到+90.0，+表示北纬，默认为0.0
        /// </summary>
        public float lat;
        /// <summary>
        /// 经度，有效范围：-180.0到+180.0，+表示东经，默认为0.0
        /// </summary>
        public float Long;
        /// <summary>
        /// 元数据，主要是为了方便第三方应用记录一些适合于自己使用的信息，每条微博可以包含一个或者多个元数据，必须以json字串的形式提交，字串长度不超过512个字符，具体内容可以自定
        /// </summary>
        public string annotations;
    }
}

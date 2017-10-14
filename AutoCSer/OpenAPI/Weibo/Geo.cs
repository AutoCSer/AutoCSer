using System;

namespace AutoCSer.OpenAPI.Weibo
{
    /// <summary>
    /// 地理信息
    /// </summary>
    public sealed class Geo
    {
        /// <summary>
        /// 经度坐标
        /// </summary>
        public string longitude;
        /// <summary>
        /// 维度坐标
        /// </summary>
        public string latitude;
        /// <summary>
        /// 所在城市的城市代码
        /// </summary>
        public string city;
        /// <summary>
        /// 所在省份的省份代码
        /// </summary>
        public string province;
        /// <summary>
        /// 所在城市的城市名称
        /// </summary>
        public string city_name;
        /// <summary>
        /// 所在省份的省份名称
        /// </summary>
        public string province_name;
        /// <summary>
        /// 所在的实际地址，可以为空
        /// </summary>
        public string address;
        /// <summary>
        /// 地址的汉语拼音，不是所有情况都会返回该字段
        /// </summary>
        public string pinyin;
        /// <summary>
        /// 更多信息，不是所有情况都会返回该字段
        /// </summary>
        public string more;
    }
}

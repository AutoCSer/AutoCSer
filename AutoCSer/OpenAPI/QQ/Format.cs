using System;

namespace AutoCSer.OpenAPI.QQ
{
    /// <summary>
    /// API返回的数据格式
    /// </summary>
    public class Format : Form
    {
        /// <summary>
        /// 定义API返回的数据格式。取值说明：为xml时表示返回的格式是xml；为json时表示返回的格式是json。注意：json、xml为小写，否则将不识别。format不传或非xml，则返回json格式数据
        /// </summary>
        internal string format;
    }
}

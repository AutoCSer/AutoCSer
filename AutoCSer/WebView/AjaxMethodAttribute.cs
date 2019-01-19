using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// AJAX 调用函数配置
    /// </summary>
    public sealed partial class AjaxMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// 默认为 true 表示需要验证 Referer 的来源页主域名是否匹配当前主域名，用于防止跨域攻击。
        /// </summary>
        public bool IsReferer = true;
        /// <summary>
        /// 默认为 true 表示仅支持 POST 请求，否则支持 GET 请求。
        /// </summary>
        public bool IsOnlyPost = true;
        /// <summary>
        /// 是否相对于网站更新的静态资源，IsOnlyPost 设置为 true 才有效，默认为 false
        /// </summary>
        public bool IsVersion;
        /// <summary>
        /// 输入参数是否添加包装处理申明 AutoCSer.Emit.BoxSerialize，用于只有一个输入参数的类型忽略外壳类型的处理以减少序列化开销，默认为 false。
        /// </summary>
        public bool IsInputSerializeBox;
        /// <summary>
        /// 输出参数是否添加包装处理申明 AutoCSer.Emit.BoxSerialize，用于只有一个输出参数的类型忽略外壳类型的处理以减少序列化开销，默认为 false。
        /// </summary>
        public bool IsOutputSerializeBox;
        /// <summary>
        /// 默认为 true 表示在初始化的时候启动 JSON 序列化预编译任务
        /// </summary>
        public bool IsCompileJsonSerialize = true;
    }
}

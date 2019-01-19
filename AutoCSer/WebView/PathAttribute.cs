using System;

namespace AutoCSer.WebView
{
    /// <summary>
    /// WEB Path 配置
    /// </summary>
    public class PathAttribute : AutoCSer.Metadata.MemberFilterAttribute.PublicInstanceProperty
    {
        /// <summary>
        /// AutoCSer 默认公共 Path 名称
        /// </summary>
        public const string AutoCSerPubPathName = "AutoCSerPath.Pub";
        /// <summary>
        /// 导出二进制位标识
        /// </summary>
        public int Flag = 1;
        /// <summary>
        /// 查询名称，默认为 Type.Name + "Id"
        /// </summary>
        public string QueryName;
        /// <summary>
        /// 服务端生成属性名称
        /// </summary>
        public string MemberName = "Path";
        /// <summary>
        /// 默认绑定类型
        /// </summary>
        public Type Type;
    }
}

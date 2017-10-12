using System;

namespace AutoCSer.Web.Config
{
    /// <summary>
    /// AutoCSer.Web.SearchServer 项目配置
    /// </summary>
    public static class Search
    {
        /// <summary>
        /// HTML 目录
        /// </summary>
        public static readonly string HtmlPath = Pub.AutoCSerPath + @"www.AutoCSer.com\";
        /// <summary>
        /// 分词文件
        /// </summary>
        public static readonly string WordFileName = Pub.AutoCSerPath + @"SearchServer\bin\Release\AutoCSer.SearchWord.data";
    }
}

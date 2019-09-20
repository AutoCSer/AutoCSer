using System;
using System.Text.RegularExpressions;

namespace AutoCSer.Web.DeployClient.Nuget
{
    /// <summary>
    /// 元数据
    /// </summary>
    internal sealed class Metadata
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string id;
        /// <summary>
        /// 版本
        /// </summary>
        public string version;
        /// <summary>
        /// 作者
        /// </summary>
        public string authors;
        /// <summary>
        /// 所有者
        /// </summary>
        public string owners;
        /// <summary>
        /// 项目 URL
        /// </summary>
        public string projectUrl;
        /// <summary>
        /// License
        /// </summary>
        public string requireLicenseAcceptance;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 发布说明
        /// </summary>
        public string releaseNotes;
        /// <summary>
        /// 版权
        /// </summary>
        public string copyright;
        /// <summary>
        /// 标签
        /// </summary>
        public string tags;
        ///// <summary>
        ///// 依赖集合
        ///// </summary>
        //public dependencie[] dependencies;

        /// <summary>
        /// 版本
        /// </summary>
        internal static readonly Regex VersionRegex = new Regex(@"<version>[^<]+<\/version>", RegexOptions.Compiled);
    }
}

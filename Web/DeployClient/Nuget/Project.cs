using System;
using System.Text;

namespace AutoCSer.Web.DeployClient.Nuget
{
    /// <summary>
    /// 项目信息
    /// </summary>
    internal sealed class Project
    {
        /// <summary>
        /// 项目路径
        /// </summary>
        internal readonly string Path;
        /// <summary>
        /// 项目文件
        /// </summary>
        internal readonly string File;
        /// <summary>
        /// Nuget 包元数据信息文件
        /// </summary>
        internal readonly string MetadataFile;
        /// <summary>
        /// Nuget 包元数据信息
        /// </summary>
        internal Metadata Metadata
        {
            get
            {
                return AutoCSer.XmlDeSerializer.DeSerialize<Package>(System.IO.File.ReadAllText(MetadataFile, Encoding.UTF8), Package.XmlConfig).metadata;
            }
        }
        /// <summary>
        /// Nuget 包文件
        /// </summary>
        internal string PackageFile
        {
            get
            {
                Metadata metadata = Metadata;
                return AutoCSer.Web.Config.Deploy.NugetPath + metadata.id + "." + metadata.version + ".nupkg";
            }
        }
        /// <summary>
        /// 项目信息
        /// </summary>
        /// <param name="name">项目文件名称</param>
        /// <param name="path">项目路径</param>
        private Project(string name, string path = AutoCSer.Web.Config.Deploy.AutoCSerProjectPath)
        {
            this.Path = path;
            File = path + name + ".csproj";
            MetadataFile = path + name + ".nuspec";
        }

        /// <summary>
        /// 项目信息集合
        /// </summary>
        internal static readonly Project[] Projects = new Project[]
        {
            new Project("AutoCSer.DotNet4.5"),
            new Project("AutoCSer.Json.DotNet4.5"),

            new Project("AutoCSer.DotNet4"),
            new Project("AutoCSer.Json.DotNet4"),

            new Project("AutoCSer.DotNet2"),
            new Project("AutoCSer.Json.DotNet2"),
        };
    }
}

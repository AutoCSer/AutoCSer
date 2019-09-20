using System;
using System.Text;

namespace AutoCSer.Web.DeployClient.Project
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
        /// 包文件相对路径
        /// </summary>
        internal readonly string PackagePath;
        /// <summary>
        /// Nuget 包信息
        /// </summary>
        internal PropertyGroup PropertyGroup
        {
            get
            {
                return AutoCSer.Xml.Parser.Parse<Config>(System.IO.File.ReadAllText(File, Encoding.UTF8), Config.XmlConfig).PropertyGroup;
            }
        }
        /// <summary>
        /// 项目信息
        /// </summary>
        /// <param name="name">项目文件名称</param>
        /// <param name="packagePath">包文件相对路径</param>
        /// <param name="path">项目路径</param>
        private Project(string name, string packagePath, string path = AutoCSer.Web.Config.Deploy.AutoCSerProjectPath)
        {
            this.Path = path;
            File = path + name + ".csproj";
            PackagePath = path + packagePath;
        }

        /// <summary>
        /// 项目信息集合
        /// </summary>
        internal static readonly Project[] Projects = new Project[]
        {
            new Project("AutoCSer.NetStandard", @"bin\Release\NetStandard\"),
            new Project("AutoCSer.Json.NetStandard", @"bin\Release\NetStandard\Serialize\"),
        };
    }
}

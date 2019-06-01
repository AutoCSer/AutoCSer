using System;
using System.IO;
using AutoCSer.Extension;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// HTML模板
    /// </summary>
    [Generator(Name = "HTML模板", DependType = typeof(CSharper), IsAuto = true, IsTemplate = false)]
    internal sealed class Html : IGenerator
    {
        /// <summary>
        /// CSS模板文件名
        /// </summary>
        private const string cssPageExtension = ".page.css";
        /// <summary>
        /// css+background
        /// </summary>
        private static readonly Regex cssBackgroundRegex = new Regex(@"background(\-image)?\:url\((\.\.)?\/images\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// 安装参数
        /// </summary>
        internal ProjectParameter Parameter;
        /// <summary>
        /// css嵌入文件
        /// </summary>
        private Dictionary<HashString, string> includeCss = DictionaryCreator.CreateHashString<string>();
        /// <summary>
        /// HTML模板处理集合
        /// </summary>
        internal Dictionary<HashString, HtmlJs> Htmls;
        /// <summary>
        /// 网站生成配置
        /// </summary>
        internal AutoCSer.WebView.Config WebConfig;
        /// <summary>
        /// 代码生成入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否生成成功</returns>
        public bool Run(ProjectParameter parameter)
        {
            if ((WebConfig = parameter.WebConfig) != null && WebConfig.IsWebView)
            {
                this.Parameter = parameter;
                Type exportPathType = WebConfig.ExportPathType;
                if (exportPathType != null)
                {
                    DirectoryInfo viewDirectory = new DirectoryInfo(parameter.ProjectPath + WebConfig.ViewJsDirectory + Path.DirectorySeparatorChar);
                    if (viewDirectory.Exists)
                    {
                        string webPathFileName = viewDirectory.fullName() + "WebPath";
                        if (!(WebConfig.IsExportPathTypeScript ? TemplateGenerator.WebPath.TypeScript.Default.Run(exportPathType, webPathFileName) : TemplateGenerator.WebPath.JavaScript.Default.Run(exportPathType, webPathFileName)))
                        {
                            Messages.Message("WEB Path 生成失败");
                        }
                    }
                    else Messages.Message("没有找到 WEB视图扩展默认目录 " + viewDirectory.FullName);
                }
                if (WebConfig.IsCopyScript)
                {
                    if (AutoCSerScriptPath == null) Messages.Message("没有找到 js 文件路径");
                    else copyScript();
                }
                Htmls = DictionaryCreator.CreateHashString<HtmlJs>();
                if (!HtmlJs.Create(this)) return false;
                css();
                File.WriteAllText(parameter.ProjectPath + AutoCSer.Net.Http.Header.VersionFileName, HtmlJs.Version, System.Text.Encoding.ASCII);
            }
            return true;
        }
        /// <summary>
        /// 复制脚本文件
        /// </summary>
        private void copyScript()
        {
            string scriptPath = Parameter.ProjectPath + @"Js\";
            DirectoryInfo AutoCSerPath = new DirectoryInfo(AutoCSerScriptPath);
            copyJs(AutoCSerPath, scriptPath);
            foreach (DirectoryInfo directory in AutoCSerPath.GetDirectories())
            {
                switch (directory.Name)
                {
                    case "ace": copyJs(directory, scriptPath + directory.Name + @"\", new string[] { "load.js", "ace.js" }); break;
                    case "mathJax": copyJs(directory, scriptPath + directory.Name + @"\", new string[] { "load.js", "MathJax.js" }); break;
                    case "highcharts": copyJs(directory, scriptPath + directory.Name + @"\"); break;
                    default: Messages.Add("未知的js文件夹 " + directory.fullName()); break;
                }
            }
        }
        /// <summary>
        /// 复制js相关文件
        /// </summary>
        /// <param name="AutoCSerPath">AutoCSer源文件目录</param>
        /// <param name="projectPath">项目文件目录</param>
        /// <param name="fileNames">项目文件目录</param>
        private void copyJs(DirectoryInfo AutoCSerPath, string projectPath, string[] fileNames)
        {
            if (!Directory.Exists(projectPath)) Directory.CreateDirectory(projectPath);
            string path = AutoCSerPath.fullName();
            foreach (string fileName in fileNames)
            {
                FileInfo file = new FileInfo(path + fileName);
                if (file.Exists) copyJs(file, projectPath + file.Name);
            }
        }
        /// <summary>
        /// 复制js相关文件
        /// </summary>
        /// <param name="AutoCSerPath">AutoCSer源文件目录</param>
        /// <param name="projectPath">项目文件目录</param>
        private void copyJs(DirectoryInfo AutoCSerPath, string projectPath)
        {
            if (!Directory.Exists(projectPath)) Directory.CreateDirectory(projectPath);
            foreach (FileInfo file in AutoCSerPath.GetFiles())
            {
                if (!file.Extension.EndsWith(".ts", StringComparison.Ordinal)) copyJs(file, projectPath + file.Name);
            }
        }
        /// <summary>
        /// 复制js相关文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        private void copyJs(FileInfo file, string fileName)
        {
            FileInfo projectFile = new FileInfo(fileName);
            if (projectFile.Exists)
            {
                if (projectFile.LastWriteTimeUtc == file.LastWriteTimeUtc) return;
                projectFile.Attributes &= ~FileAttributes.ReadOnly;
                projectFile.Delete();
            }
            copyJs(file, projectFile);
        }
        /// <summary>
        /// 复制js相关文件
        /// </summary>
        /// <param name="file">AutoCSer源文件</param>
        /// <param name="projectFile">项目文件</param>
        private void copyJs(FileInfo file, FileInfo projectFile)
        {
            if (WebConfig.Encoding.CodePage != Encoding.UTF8.CodePage)
            {
                string extension = file.Extension.toLower();
                if (extension == ".ts" || extension == ".js" || extension == ".htm" || extension == ".css")
                {
                    File.WriteAllText(projectFile.FullName, File.ReadAllText(file.FullName, Encoding.UTF8), WebConfig.Encoding);
                    projectFile.LastWriteTimeUtc = file.LastWriteTimeUtc;
                    return;
                }
            }
            file.CopyTo(projectFile.FullName);
            new FileInfo(projectFile.FullName).Attributes &= ~FileAttributes.ReadOnly;
        }
        /// <summary>
        /// 创建目标css文件
        /// </summary>
        private void css()
        {
            foreach (string pageFileName in Directory.GetFiles(Parameter.ProjectPath, "*" + cssPageExtension, SearchOption.AllDirectories))
            {
                string fileName = pageFileName.Substring(0, pageFileName.Length - cssPageExtension.Length) + ".css";
                string content = getIncludeCss(pageFileName)
                    .Replace("__STATICDOMAIN__", WebConfig.StaticFileDomain)
                    .Replace("__IMAGEDOMAIN__", WebConfig.ImageDomain);
                //string code = File.ReadAllText(pageFileName, webConfig.Encoding)
                //    .Replace("__STATICDOMAIN__", webConfig.StaticFileDomain)
                //    .Replace("__IMAGEDOMAIN__", webConfig.ImageDomain);
                content = cssBackgroundRegex.Replace(content, cssBackgroundReplace);
                if (!File.Exists(fileName) || File.ReadAllText(fileName, WebConfig.Encoding) != content)
                {
                    File.WriteAllText(fileName, content, WebConfig.Encoding);
                }
            }
        }
        /// <summary>
        /// css背景图片域名替换
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private string cssBackgroundReplace(Match match)
        {
            return @"background" + (match.Groups.Count == 1 ? "-image" : "") + ":url(//" + WebConfig.StaticFileDomain + "/images/";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string getIncludeCss(string fileName)
        {
            string content;
            HashString fileKey = fileName.toLowerNotEmpty();
            if (!includeCss.TryGetValue(fileKey, out content))
            {
                content = File.ReadAllText(fileName, WebConfig.Encoding);
                string includeStart = @"/*Include:";
                if (content.StartsWith(includeStart, StringComparison.Ordinal))
                {
                    int index = content.IndexOf("*/", includeStart.Length);
                    if (index != -1)
                    {
                        content = System.String.Join(@"
", new SubString { String = content, Start = includeStart.Length, Length = index - includeStart.Length }.Split(',')
                            .getArray(value => getIncludeCss(Parameter.ProjectPath + value.ToString() + ".css"))
                            .getAdd(content.Substring(index + 2)));
                    }
                }
                includeCss.Add(fileKey, content);
            }
            return content;
        }

        /// <summary>
        /// AutoCSer Typescipt文件路径
        /// </summary>
        private static readonly string AutoCSerScriptPath;
        static Html()
        {
            string path = new DirectoryInfo(AutoCSer.PubPath.ApplicationPath).Parent.Parent.Parent
#if !DOTNET45
                .Parent
#endif
                .fullName();
            string jsPath = path + @"CodeGenerator\Js\";
            if (!Directory.Exists(jsPath))
            {
                jsPath = AutoCSer.PubPath.ApplicationPath + @"Js\";
                if (!Directory.Exists(jsPath)) jsPath = path + @"CodeGenerator\Js\";
            }
            if (Directory.Exists(jsPath)) AutoCSerScriptPath = new DirectoryInfo(jsPath).fullName().fileNameToLower();
        }
    }
}

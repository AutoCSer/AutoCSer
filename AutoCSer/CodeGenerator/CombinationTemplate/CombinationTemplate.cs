using System;
using System.Collections.Generic;
using System.IO;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 自定义简单组合模板
    /// </summary>
    [Generator(Name = "自定义简单组合模板", IsAuto = true, IsTemplate = false)]
    internal sealed class CombinationTemplate : IGenerator
    {
        /// <summary>
        /// 安装入口
        /// </summary>
        /// <param name="parameter">安装参数</param>
        /// <returns>是否安装成功</returns>
        public bool Run(ProjectParameter parameter)
        {
            if (parameter != null)
            {
                HashSet<string> codeFileNames = new HashSet<string>();
                foreach (Type type in parameter.Types ?? EmptyArray<Type>.Array)
                {
                    if (!type.IsAbstract && typeof(CombinationTemplateConfig).IsAssignableFrom(type) && type != typeof(CombinationTemplateConfig))
                    {
                        CombinationTemplateConfig config = (CombinationTemplateConfig)Activator.CreateInstance(type);
                        string codeFileName = config.GetCodeFileName(parameter.DefaultNamespace);
                        if (!codeFileNames.Add(codeFileName))
                        {
                            Messages.Error("自定义简单组合模板目标文件名称冲突 "+codeFileName);
                            return false;
                        }
                        LeftArray<string> codes = new LeftArray<string>(0);
                        foreach (string templatePath in config.TemplatePath)
                        {
                            DirectoryInfo directory = new DirectoryInfo(Path.Combine(parameter.ProjectPath, templatePath));
                            if (directory.Exists)
                            {
                                foreach (FileInfo file in directory.GetFiles("*.cs"))
                                {
                                    LeftArray<string> newCodes = getCode(file.FullName);
                                    if (newCodes.Length == 0) return false;
                                    codes.Add(ref newCodes);
                                }
                            }
                            else
                            {
                                Messages.Error("没有找到自定义模板相对项目路径" + config.TemplatePath);
                                return false;
                            }
                        }
                        string fileName = Path.Combine(parameter.ProjectPath, @"{" + codeFileName + "}.CombinationTemplate.cs");
                        if (Coder.WriteFile(fileName, Coder.WarningCode + string.Concat(codes.ToArray()) + Coder.FileEndCode)) Messages.Message(fileName + " 被修改");
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="fileName">模板文件名</param>
        /// <returns>代码</returns>
        private static LeftArray<string> getCode(string fileName)
        {
            string code = File.ReadAllText(fileName, System.Text.Encoding.UTF8);
            int startIndex = code.IndexOf("/*", StringComparison.Ordinal), endIndex = code.IndexOf(@"*/

", startIndex + 2, StringComparison.Ordinal);
            if (endIndex > 0 && startIndex > 0)
            {
                LeftArray<string[][]> headerArray = new LeftArray<string[][]>(0);
                foreach (SubString line in new SubString(startIndex += 2, endIndex - startIndex, code).Trim().Split('\n'))
                {
                    SubString headerLine = line.Trim();
                    if (headerLine.Length != 0) headerArray.Add(headerLine.Split(';').GetArray(p => p.Split(',').GetArray(v => (string)v)));
                }
                if (headerArray.Length != 0)
                {
                    CombinationTemplateLink header = new CombinationTemplateLink(code = code.Substring(endIndex + 4));
                    int row = 0;
                    foreach (string[][] line in headerArray)
                    {
                        int col = 0;
                        foreach (string value in line[0])
                        {
                            bool isSplit = false;
                            CombinationTemplateLink link = header;
                            do
                            {
                                link = link.Split(value, row, col, ref isSplit);
                            }
                            while (link != null);
                            if (!isSplit)
                            {
                                Messages.Error("自定义简单组合模板文件内容没有找到匹配替换标记 "+value+" : "+fileName);
                                return new LeftArray<string>(0);
                            }
                            ++col;
                        }
                        ++row;
                    }
                    LeftArray<CombinationTemplateLink> linkArray = new LeftArray<CombinationTemplateLink>(0);
                    LeftArray<string> codeArray = new LeftArray<string>(0);
                    do
                    {
                        if (header.Row >= 0)
                        {
                            linkArray.Add(header);
                            header.Index = codeArray.Length;
                            codeArray.Add(string.Empty);
                        }
                        else codeArray.Add(header.Code);
                    }
                    while ((header = header.Next) != null);
                    CombinationTemplateHeaderEnumerable headerEnumerable = new CombinationTemplateHeaderEnumerable(ref headerArray, codeArray.ToArray(), linkArray.ToArray());
                    codeArray.Length = 0;
                    foreach (string enumerableCode in headerEnumerable.GetCode()) codeArray.Add(enumerableCode);
                    return codeArray;
                }
                else Messages.Error("自定义简单组合模板文件缺少头部信息解析失败 : " + fileName);
            }
            else Messages.Error("自定义简单组合模板文件缺少头部注释信息 : " + fileName);
            return new LeftArray<string>(0);
        }
    }
}

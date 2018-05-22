using System;
using AutoCSer.Extension;
using System.IO;
using System.Collections.Generic;

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
                foreach (Type type in parameter.Types)
                {
                    if (typeof(CombinationTemplateConfig).IsAssignableFrom(type) && type != typeof(CombinationTemplateConfig))
                    {
                        CombinationTemplateConfig config = (CombinationTemplateConfig)Activator.CreateInstance(type);
                        DirectoryInfo directory = new DirectoryInfo(parameter.ProjectPath + config.TemplatePath);
                        if (directory.Exists)
                        {
                            LeftArray<string>[] codes = Directory.GetFiles(directory.FullName, "*.cs").getArray(name => code(name));
                            if (!codes.any(code => code.Length == 0))
                            {
                                string fileName = parameter.ProjectPath + @"{" + parameter.DefaultNamespace + "}.CombinationTemplate.cs";
                                if (Coder.WriteFile(fileName, Coder.WarningCode + string.Concat(codes.getArray(code => code.ToArray()).getArray()) + Coder.FileEndCode))
                                {
                                    Messages.Message(fileName + " 被修改");
                                }
                                return true;
                            }
                        }
                        else Messages.Message("没有找到自定义模板相对项目路径" + config.TemplatePath);
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 一个关键字的模板替换信息
        /// </summary>
        private sealed class Replace
        {
            /// <summary>
            /// 原始数组中的替换位置集合
            /// </summary>
            public LeftArray<int>[] indexs;
            /// <summary>
            /// 替换的目标组合集合
            /// </summary>
            public LeftArray<SubString>[] values;
            /// <summary>
            /// 当前组合索引
            /// </summary>
            public int index;
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="fileName">模板文件名</param>
        /// <returns>代码</returns>
        private static LeftArray<string> code(string fileName)
        {
            string code = File.ReadAllText(fileName);
            int index = code.IndexOf(@"*/

", StringComparison.Ordinal);
            if (index != -1)
            {
                int startIndex = index += 4, count = 0;
                Dictionary<SubString, Replace> keys = DictionaryCreator.CreateSubString<Replace>();
                Replace replace;
                foreach (string keyValue in code.Substring(0, index).Split(new string[] { "/*" }, StringSplitOptions.None))
                {
                    if (count == 0)
                    {
                        count = 1;
                        continue;
                    }
                    if ((index = keyValue.IndexOf(':')) != -1 && keyValue.EndsWith(@"*/
", StringComparison.Ordinal))
                    {
                        keys[new SubString { String = keyValue, Start = 0, Length = index }] = replace = new Replace
                        {
                            values = new SubString { String = keyValue, Start = ++index, Length = keyValue.Length - index - 4 }.Split(';').GetArray(value => value.Split(','))
                        };
                        replace.indexs = new LeftArray<int>[replace.values[0].Length];
                    }
                    else Messages.Add("自定义数据错误 : " + keyValue);
                }
                LeftArray<string> codeList = new LeftArray<string>();
                for (code = code.Substring(startIndex), startIndex = 0, index = code.IndexOf("/*", StringComparison.Ordinal); index != -1; index = code.IndexOf("/*", startIndex))
                {
                    codeList.Add(code.Substring(startIndex, index - startIndex));
                    if ((startIndex = code.IndexOf("*/", index, StringComparison.Ordinal) + 2) != 1 && startIndex != index + 4)
                    {
                        string name = code.Substring(index, startIndex - index);
                        if ((index = code.IndexOf(name, startIndex, StringComparison.Ordinal)) != -1)
                        {
                            startIndex = index + name.Length;
                            name = name.Substring(2, name.Length - 4);
                            if (name[name.Length - 1] == ']')
                            {
                                int arrayIndex = name.LastIndexOf('[') + 1;
                                if (arrayIndex == 0 || !int.TryParse(name.Substring(arrayIndex, name.Length - arrayIndex - 1), out index))
                                {
                                    Messages.Add("自定义名称索引错误 : " + name);
                                    index = 0;
                                }
                                else name = name.Substring(0, arrayIndex - 1);
                            }
                            else index = 0;
                            if (keys.TryGetValue(name, out replace))
                            {
                                replace.indexs[index].Add(codeList.Length);
                                codeList.Add(string.Empty);
                            }
                            else Messages.Add("自定义名称不匹配 : " + name);
                        }
                        else Messages.Add("自定义名称不匹配 : " + name);
                    }
                    else
                    {
                        Messages.Add("自定义名称错误 : " + code.Substring(index));
                        startIndex = code.Length;
                    }
                }
                codeList.Add(code.Substring(startIndex));
                string[] codes = codeList.ToArray();
                LeftArray<string> values = default(LeftArray<string>);
                Replace[] replaces = keys.Values.getArray();
                do
                {
                    index = replaces.Length;
                    while (--index >= 0 && ++replaces[index].index == replaces[index].values.Length) ;
                    if (index == -1) break;
                    while (++index != replaces.Length) replaces[index].index = 0;
                    for (index = replaces.Length; --index >= 0; )
                    {
                        replace = replaces[index];
                        LeftArray<SubString> replaceCode = replace.values[replace.index];
                        for (startIndex = replace.indexs.Length; --startIndex >= 0; )
                        {
                            foreach (int codeIndex in replace.indexs[startIndex]) codes[codeIndex] = replaceCode[startIndex];
                        }
                    }
                    values.Add(string.Concat(codes));
                }
                while (true);
                return values;
            }
            else Messages.Add("自定义文件错误 : " + fileName);
            return default(LeftArray<string>);
        }
    }
}

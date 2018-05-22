using System;
using AutoCSer.Extension;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using AutoCSer.CodeGenerator.Metadata;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 项目安装参数
    /// </summary>
    internal sealed class ProjectParameter
    {
        /// <summary>
        /// 当前程序集
        /// </summary>
        internal static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
        /// <summary>
        /// 项目名称
        /// </summary>
        internal readonly string ProjectName;
        /// <summary>
        /// 项目路径
        /// </summary>
        internal readonly string ProjectPath;
        /// <summary>
        /// 程序集文件名全称
        /// </summary>
        internal readonly string AssemblyPath;
        /// <summary>
        /// 项目默认命名空间
        /// </summary>
        public readonly string DefaultNamespace;
        /// <summary>
        /// 是否自动更新项目文件
        /// </summary>
        internal readonly bool IsProjectFile;
        /// <summary>
        /// 是否 AutoCSer 项目
        /// </summary>
        internal readonly bool IsAutoCSerCodeGenerator;
        /// <summary>
        /// 是否自定义代码生成项目
        /// </summary>
        internal readonly bool IsCustomCodeGenerator;
        /// <summary>
        /// 是否自定义简单组合模板
        /// </summary>
        internal readonly bool IsCombinationTemplate;
        /// <summary>
        /// 程序集
        /// </summary>
        internal Assembly Assembly;
        /// <summary>
        /// 类型集合
        /// </summary>
        private Type[] types;
        /// <summary>
        /// 类型集合
        /// </summary>
        internal Type[] Types
        {
            get
            {
                if (types == null && Assembly != null)
                {
                    try
                    {
                        types = Assembly.GetTypes().sort((left, right) => string.CompareOrdinal(left.FullName, right.FullName));
                    }
                    catch (Exception error)
                    {
                        types = NullValue<Type>.Array;
                        Messages.Add(error);
                    }
                }
                return types;
            }
        }
        /// <summary>
        /// 网站生成配置
        /// </summary>
        private AutoCSer.WebView.Config webConfig;
        /// <summary>
        /// 网站生成配置
        /// </summary>
        public AutoCSer.WebView.Config WebConfig
        {
            get
            {
                if (webConfig == null)
                {
                    foreach (Type type in Types)
                    {
                        if (!type.IsAbstract && typeof(AutoCSer.WebView.Config).IsAssignableFrom(type))
                        {
                            if (TypeAttribute.GetAttribute<AutoCSer.WebView.ConfigAttribute>(type) != null)
                            {
                                webConfig = Activator.CreateInstance(type) as AutoCSer.WebView.Config;
                                break;
                            }
                        }
                    }
                    if (webConfig == null) webConfig = AutoCSer.WebView.Config.Null.Default;
                }
                return webConfig == AutoCSer.WebView.Config.Null.Default ? null : webConfig;
            }
        }
        /// <summary>
        /// 网站生成配置类型
        /// </summary>
        public ExtensionType WebConfigType
        {
            get { return WebConfig.GetType(); }
        }
        /// <summary>
        /// 自动安装参数
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectPath">项目路径</param>
        /// <param name="assemblyPath">程序集文件名全称</param>
        /// <param name="defaultNamespace">项目默认命名空间</param>
        /// <param name="isProjectFile">是否自动更新项目文件</param>
        public ProjectParameter(string projectName, string projectPath, string assemblyPath, string defaultNamespace, bool isProjectFile)
        {
            ProjectName = projectName;
            ProjectPath = new System.IO.DirectoryInfo(projectPath).fullName().toLowerNotEmpty();
            AssemblyPath = assemblyPath;
            DefaultNamespace = defaultNamespace;
            IsProjectFile = isProjectFile;
            string assemblyFile = new FileInfo(assemblyPath).Name;
            if (assemblyFile == "AutoCSer.CodeGenerator.exe") IsAutoCSerCodeGenerator = true;
            else if (assemblyFile == CustomConfig.CustomAssemblyName + ".dll") IsCustomCodeGenerator = true;
        }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="isAssemblyPath"></param>
        /// <returns></returns>
        internal Exception LoadAssembly(ref bool isAssemblyPath)
        {
            try
            {
                string assemblyFile = AssemblyPath.Substring(AssemblyPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                foreach (Assembly value in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (value.ManifestModule.Name == assemblyFile)
                    {
                        Assembly = value;
                        isAssemblyPath = true;
                        return null;
                    }
                }
                if (isAssemblyPath = File.Exists(AssemblyPath))
                {
                    //System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = new FileInfo(AssemblyPath).DirectoryName;
                    Assembly = Assembly.LoadFrom(AssemblyPath);
                    //PortableExecutableKinds peKind;
                    //ImageFileMachine imageFileMachine;
                    //assembly.ManifestModule.GetPEKind(out peKind, out imageFileMachine);
                    //Messages.Message(assemblyFile + " PortableExecutableKinds[" + peKind.ToString() + "] ImageFileMachine[" + imageFileMachine.ToString() + "]");
                }
            }
            catch (Exception error)
            {
                return error;
            }
            return null;
        }
        /// <summary>
        /// 运行代码生成
        /// </summary>
        /// <param name="generator">代码生成接口</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void run(IGenerator generator)
        {
            if (!generator.Run(this)) Messages.Add(generator.GetType().fullName() + " 安装失败");
        }
        /// <summary>
        /// 启动代码生成
        /// </summary>
        public void Start()
        {
            if (string.IsNullOrEmpty(ProjectPath) || !Directory.Exists(ProjectPath)) Messages.Add("项目路径不存在 : " + ProjectPath);
            else
            {
                try
                {
                    if (IsAutoCSerCodeGenerator || IsCustomCodeGenerator)
                    {
                        run(new CSharper());
                    }
                    else
                    {
                        KeyValue<Type, GeneratorAttribute>[] generators = (CustomConfig.Default.IsAutoCSer ? CurrentAssembly.GetTypes() : NullValue<Type>.Array)
                            .concat(CustomConfig.Assembly == null ? null : CustomConfig.Assembly.GetTypes())
                            .getFind(type => !type.IsInterface && !type.IsAbstract && typeof(IGenerator).IsAssignableFrom(type))
                            .GetArray(type => new KeyValue<Type, GeneratorAttribute>(type, type.customAttribute<GeneratorAttribute>()))
                            .getFind(value => value.Value != null && value.Value.IsAuto).ToArray();
                        if (generators.Length != 0)
                        {
                            generators = generators.sort((left, right) => string.CompareOrdinal(left.Key.FullName, right.Key.FullName));
                            HashSet<Type> types = generators.getHash(value => value.Key);
                            KeyValue<Type, Type>[] depends = generators
                                .getFind(value => value.Value.DependType != null && types.Contains(value.Value.DependType))
                                .GetArray(value => new KeyValue<Type, Type>(value.Key, value.Value.DependType));
                            foreach (Type type in AutoCSer.Algorithm.TopologySort.Sort(depends, types, true)) run(type.Assembly.CreateInstance(type.FullName) as IGenerator);
                        }
                        if (CustomConfig.Default.IsAutoCSer)
                        {
                            foreach (Type type in Types)
                            {
                                if (!type.IsGenericType && !type.IsInterface && !type.IsEnum)
                                {
                                    foreach (Metadata.MethodIndex methodInfo in Metadata.MethodIndex.GetMethods<TestMethodAttribute>(type, MemberFilters.Static, false, true, false))
                                    {
                                        MethodInfo method = methodInfo.Method;
                                        if (method.IsGenericMethod)
                                        {
                                            //isTest = false;
                                            Messages.Message("测试用例不能是泛型函数 " + method.fullName());
                                        }
                                        else
                                        {
                                            Type returnType = method.ReturnType;
                                            if ((returnType == typeof(bool) || returnType == typeof(void)) && method.GetParameters().Length == 0)
                                            {
                                                try
                                                {
                                                    object returnValue = method.Invoke(null, null);
                                                    if (method.ReturnType == typeof(bool) && !(bool)returnValue)
                                                    {
                                                        //isTest = false;
                                                        Messages.Message("测试用例调用失败 " + method.fullName());
                                                    }
                                                }
                                                catch (Exception error)
                                                {
                                                    Messages.Message(error.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    Messages.Add(error);
                }
                finally { Coder.Output(this); }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using AutoCSer.Extension;

namespace AutoCSer
{
    /// <summary>
    /// 动态应用程序域
    /// </summary>
    [Serializable]
    public sealed class DynamicDomain : IDisposable
    {
        /// <summary>
        /// 程序集加载器
        /// </summary>
        private sealed class assemblyLoader : MarshalByRefObject
        {
            /// <summary>
            /// 已加载程序集
            /// </summary>
            [NonSerialized]
            private Dictionary<HashString, Assembly> assemblys = DictionaryCreator.CreateHashString<Assembly>();
            /// <summary>
            /// 日志处理
            /// </summary>
            [NonSerialized]
            internal AutoCSer.Log.ILog Log;
            /// <summary>
            /// 程序集加载器
            /// </summary>
            public assemblyLoader() { }
            /// <summary>
            /// 加载程序集
            /// </summary>
            /// <param name="assemblyFileName">程序集文件名</param>
            public Assembly Load(string assemblyFileName)
            {
                if (!string.IsNullOrEmpty(assemblyFileName))
                {
                    Assembly assembly;
                    HashString nameKey = assemblyFileName;
                    if (assemblys.TryGetValue(nameKey, out assembly)) return assembly;
                    try
                    {
                        if (assemblyFileName.Length > 4 && assemblyFileName.EndsWith(".dll"))
                        {
                            assembly = Assembly.LoadFrom(assemblyFileName.Substring(0, assemblyFileName.Length - 4));
                        }
                        else assembly = Assembly.LoadFrom(assemblyFileName);
                        assemblys[nameKey] = assembly;
                        return assembly;
                    }
                    catch (Exception error)
                    {
                        Log.Add(AutoCSer.Log.LogType.Error, error, "动态应用程序域加载程序集 " + assemblyFileName + " 失败");
                    }
                }
                return null;
            }
            /// <summary>
            /// 加载程序集获取类型
            /// </summary>
            /// <param name="assemblyFileName">程序集文件名</param>
            /// <param name="typeName">类型名称</param>
            /// <returns>类型</returns>
            public Type LoadType(string assemblyFileName, string typeName)
            {
                Assembly assembly = Load(assemblyFileName);
                return assembly != null ? assembly.GetType(typeName) : null;
            }
            /// <summary>
            /// 加载程序集并创建对象
            /// </summary>
            /// <param name="assemblyFileName">程序集文件名</param>
            /// <param name="typeName">对象类型名称</param>
            /// <returns>创建的对象,失败返回null</returns>
            public object CreateInstance(string assemblyFileName, string typeName)
            {
                Type type = LoadType(assemblyFileName, typeName);
                if (type != null)
                {
                    try
                    {
                        return Activator.CreateInstance(type);
                    }
                    catch (Exception error)
                    {
                        Log.Add(AutoCSer.Log.LogType.Error, error);
                    }
                }
                return null;
            }
            /// <summary>
            /// 加载程序集并创建包装对象
            /// </summary>
            /// <param name="assemblyFileName">程序集文件名</param>
            /// <param name="typeName">对象类型名称</param>
            /// <returns>创建的包装对象,失败返回null</returns>
            public ObjectHandle CreateHandle(string assemblyFileName, string typeName)
            {
                try
                {
                    return Activator.CreateInstance(assemblyFileName, typeName);
                }
                catch (Exception error)
                {
                    Log.Add(AutoCSer.Log.LogType.Error, error);
                }
                return null;
            }
        }
        /// <summary>
        /// 默认程序集名称
        /// </summary>
        [NonSerialized]
        private static readonly string assemblyName = typeof(DynamicDomain).Assembly.FullName;
        /// <summary>
        /// 默认程序集加载器类型名称
        /// </summary>
        [NonSerialized]
        private static readonly string assemblyLoaderName = typeof(DynamicDomain.assemblyLoader).FullName;
        /// <summary>
        /// 应用程序域
        /// </summary>
        private readonly AppDomainSetup setup = new AppDomainSetup();
        /// <summary>
        /// 应用程序域
        /// </summary>
        [NonSerialized]
        private AppDomain domain;
        /// <summary>
        /// 程序集加载器
        /// </summary>
        [NonSerialized]
        private assemblyLoader loader;
        /// <summary>
        /// 程序集私有目录
        /// </summary>
        [NonSerialized]
        private readonly string privatePath;
        /// <summary>
        /// 日志处理
        /// </summary>
        [NonSerialized]
        private readonly AutoCSer.Log.ILog log;
        /// <summary>
        /// 初始化动态应用程序域
        /// </summary>
        /// <param name="name">应用程序域名称</param>
        /// <param name="privatePath">程序集加载目录</param>
        /// <param name="configFile">配置文件</param>
        /// <param name="cacheDirectory">应用程序域缓存目录,null表示非缓存</param>
        /// <param name="log">日志处理</param>
        public DynamicDomain(string name, string privatePath, string configFile, string cacheDirectory, AutoCSer.Log.ILog log = null)
        {
            this.log = log ?? AutoCSer.Log.Pub.Log;
            if (string.IsNullOrEmpty(privatePath)) privatePath = AutoCSer.PubPath.ApplicationPath;
            else
            {
                privatePath = new DirectoryInfo(privatePath).fullName().fileNameToLower();
                if (privatePath != AutoCSer.PubPath.ApplicationPath) this.privatePath = privatePath;
            }
            setup = new AppDomainSetup();
            if (configFile != null && File.Exists(configFile)) setup.ConfigurationFile = configFile;
            setup.ApplicationName = name;
            setup.ApplicationBase = privatePath;
            setup.PrivateBinPath = privatePath;
            if (cacheDirectory != null && cacheDirectory.Length != 0)
            {
                setup.ShadowCopyFiles = "true";
                setup.ShadowCopyDirectories = cacheDirectory;
                setup.CachePath = cacheDirectory;
            }
            domain = AppDomain.CreateDomain(name, null, setup);
            loader = (assemblyLoader)domain.CreateInstanceAndUnwrap(assemblyName, assemblyLoaderName);
            loader.Log = this.log;
        }
        /// <summary>
        /// 卸载应用程序域
        /// </summary>
        public void Dispose()
        {
            if (domain != null)
            {
                try
                {
                    AppDomain.Unload(domain);
                }
                catch (Exception error)
                {
                    log.Add(Log.LogType.Error, error);
                }
                domain = null;
                loader = null;
            }
        }
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyFileName">程序集文件名</param>
        public Assembly LoadAssembly(string assemblyFileName)
        {
            if (privatePath == null) return loader.Load(assemblyFileName);
#pragma warning disable 618
            AppDomain.CurrentDomain.AppendPrivatePath(privatePath);
            try
            {
                return loader.Load(assemblyFileName);
            }
            finally { AppDomain.CurrentDomain.ClearPrivatePath(); }
#pragma warning restore 618
        }
        /// <summary>
        /// 加载程序集获取类型
        /// </summary>
        /// <param name="assemblyFileName">程序集文件名</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>类型</returns>
        public Type LoadType(string assemblyFileName, string typeName)
        {
            if (privatePath == null) return loader.LoadType(assemblyFileName, typeName);
#pragma warning disable 618
            AppDomain.CurrentDomain.AppendPrivatePath(privatePath);
            try
            {
                return loader.LoadType(assemblyFileName, typeName);
            }
            finally { AppDomain.CurrentDomain.ClearPrivatePath(); }
#pragma warning restore 618
        }
        /// <summary>
        /// 加载程序集并创建对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="assemblyFileName">对象类型名称</param>
        /// <param name="typeName">对象类型名称,必须表示为Serializable</param>
        /// <returns>创建的对象</returns>
        public valueType CreateInstance<valueType>(string assemblyFileName, string typeName)
        {
            if (privatePath == null) return (valueType)loader.CreateInstance(assemblyFileName, typeName);
#pragma warning disable 618
            AppDomain.CurrentDomain.AppendPrivatePath(privatePath);
            try
            {
                return (valueType)loader.CreateInstance(assemblyFileName, typeName);
            }
            finally { AppDomain.CurrentDomain.ClearPrivatePath(); }
#pragma warning restore 618
        }
        /// <summary>
        /// 加载程序集并创建包装对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="assemblyFileName">对象类型名称</param>
        /// <param name="typeName">对象类型名称,必须表示为Serializable</param>
        /// <returns>创建的对象</returns>
        public valueType CreateHandle<valueType>(string assemblyFileName, string typeName)
        {
            if (privatePath == null) return (valueType)loader.CreateHandle(assemblyFileName, typeName).Unwrap();
#pragma warning disable 618
            AppDomain.CurrentDomain.AppendPrivatePath(privatePath);
            try
            {
                return (valueType)loader.CreateHandle(assemblyFileName, typeName).Unwrap();
            }
            finally { AppDomain.CurrentDomain.ClearPrivatePath(); }
#pragma warning restore 618
        }
    }
}

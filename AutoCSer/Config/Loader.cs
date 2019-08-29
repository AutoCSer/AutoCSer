using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Collections.Specialized;

namespace AutoCSer.Config
{
    /// <summary>
    /// 配置加载
    /// </summary>
    public static partial class Loader
    {
        /// <summary>
        /// 配置加载程序集名称
        /// </summary>
        private const string configNamespace = "AutoCSer.Config";
        /// <summary>
        /// 配置关键字
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct Key : IEquatable<Key>
        {
            /// <summary>
            /// 配置类型
            /// </summary>
            private Type type;
            /// <summary>
            /// 配置名称
            /// </summary>
            private string name;
            /// <summary>
            /// 
            /// </summary>
            private int hashCode;
            /// <summary>
            /// 配置关键字
            /// </summary>
            /// <param name="type"></param>
            /// <param name="name"></param>
            public Key(Type type, string name)
            {
                this.type = type;
                this.name = name;
                hashCode = type.GetHashCode() ^ name.GetHashCode();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(Key other)
            {
                return type == other.type && hashCode == other.hashCode && name == other.name;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return Equals((Key)obj);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return hashCode;
            }
        }
        /// <summary>
        /// 配置集合
        /// </summary>
        private static readonly Dictionary<Key, Creator> configs = DictionaryCreator<Key>.Create<Creator>();
        /// <summary>
        /// 配置程序集名称集合（主要用于 WebForm 等动态应用程序域配置）
        /// </summary>
        private static readonly HashSet<string> assemblyNames = HashSetCreator.CreateOnly<string>();
        /// <summary>
        /// 配置访问锁
        /// </summary>
        private static readonly object configLock = new object();
        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="name">配置名称</param>
        /// <returns>配置项数据</returns>
        public static object GetObject(Type type, string name = "")
        {
            Key key = new Key(type, name ?? string.Empty);
            Creator creator;
            Monitor.Enter(configLock);
            if (configs.TryGetValue(key, out creator))
            {
                Monitor.Exit(configLock);
                return creator.Create();
            }
            Monitor.Exit(configLock);
            return null;
        }
        /// <summary>
        /// 获取配置项数据
        /// </summary>
        /// <typeparam name="valueType">配置数据类型</typeparam>
        /// <param name="name">配置名称</param>
        /// <returns>配置项数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Get<valueType>(string name = "")
        {
            object value = GetObject(typeof(valueType), name);
            return value == null ? default(valueType) : (valueType)value;
        }
        /// <summary>
        /// 检测程序集名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void check(object sender, AssemblyLoadEventArgs args)
        {
            Assembly assembly = args.LoadedAssembly;
            if (checkName(assembly))
            {
                foreach (KeyValue<Key, Creator> config in getConfigs(assembly, false))
                {
                    Monitor.Enter(configLock);
                    try
                    {
                        set(config);
                    }
                    finally { Monitor.Exit(configLock); }
                }
            }
        }
        /// <summary>
        /// 检测程序集名称
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool checkName(Assembly assembly)
        {
            string name = assembly.FullName;
            if (name[0] == configNamespace[0] && (name == configNamespace || name.StartsWith(configNamespace + ".", StringComparison.Ordinal))) return true;
            int endIndex = name.IndexOf(',');
            return assemblyNames.Contains(endIndex >= 0 ? name.Substring(0, endIndex) : name);
        }
        /// <summary>
        /// 获取配置数据
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="isMain"></param>
        /// <returns></returns>
        private static IEnumerable<KeyValue<Key, Creator>> getConfigs(Assembly assembly, bool isMain)
        {
            foreach (Type type in assembly.GetTypes())
            {
                foreach (TypeAttribute typeAttribute in type.GetCustomAttributes(typeof(TypeAttribute), false))
                {
                    object instance = null;
                    BindingFlags instanceBindingFlags;
                    if (typeAttribute.IsInstance)
                    {
                        instance = getInstance(type);
                        instanceBindingFlags = BindingFlags.Instance;
                    }
                    else instanceBindingFlags = BindingFlags.Static | BindingFlags.FlattenHierarchy;

                    if (!typeAttribute.IsInstance || instance != null)
                    {
                        foreach (FieldInfo field in type.GetFields(instanceBindingFlags | BindingFlags.NonPublic | BindingFlags.Public))
                        {
                            foreach (MemberAttribute attribute in field.GetCustomAttributes(typeof(MemberAttribute), false))
                            {
                                if (attribute != null)
                                {
                                    yield return new KeyValue<Key, Creator>(new Key(field.FieldType, attribute.Name ?? field.Name), new Creator.Field(instance, isMain, field));
                                    break;
                                }
                            }
                        }
                        foreach (PropertyInfo property in type.GetProperties(instanceBindingFlags | BindingFlags.NonPublic | BindingFlags.Public))
                        {
                            if (property.CanRead)
                            {
                                MethodInfo method = property.GetGetMethod(true);
                                if (method != null && method.GetParameters().Length == 0)
                                {
                                    foreach (MemberAttribute attribute in property.GetCustomAttributes(typeof(MemberAttribute), false))
                                    {
                                        if (attribute != null)
                                        {
                                            yield return new KeyValue<Key, Creator>(new Key(property.PropertyType, attribute.Name ?? property.Name), new Creator.Property(instance, isMain, method));
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 获取配置实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object getInstance(Type type)
        {
            try
            {
                foreach (PropertyInfo property in type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    if (property.CanRead && property.PropertyType == type)
                    {
                        MethodInfo method = property.GetGetMethod(true);
                        if (method != null && method.GetParameters().Length == 0)
                        {
                            foreach (MemberAttribute attribute in property.GetCustomAttributes(typeof(MemberAttribute), false))
                            {
                                if (attribute != null) return method.Invoke(null, null);
                            }
                        }
                    }
                }
                return Activator.CreateInstance(type);
            }
            catch { }
            return null;
        }
        /// <summary>
        /// 配置加载检测
        /// </summary>
        /// <param name="assembly"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void load(Assembly assembly)
        {
            foreach (KeyValue<Key, Creator> config in getConfigs(assembly, false)) set(config);
        }
        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="config"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void set(KeyValue<Key, Creator> config)
        {
            Creator oldCreator;
            if (!configs.TryGetValue(config.Key, out oldCreator) || !oldCreator.IsMain) configs[config.Key] = config.Value;
        }

        static Loader()
        {
            AppDomain.CurrentDomain.AssemblyLoad += check;
            Assembly mainAssembly = Assembly.GetEntryAssembly();
            Assembly[] currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in currentAssemblies)
            {
                if (assembly.FullName.StartsWith("System.Configuration,", StringComparison.Ordinal))
                {
                    Type configurationManagerType = assembly.GetType("System.Configuration.ConfigurationManager");
                    if (configurationManagerType != null)
                    {
                        PropertyInfo appSettingsPropertyInfo = configurationManagerType.GetProperty("AppSettings", BindingFlags.Static | BindingFlags.Public);
                        if (appSettingsPropertyInfo != null)
                        {
                            NameValueCollection appSettings = appSettingsPropertyInfo.GetValue(null, null) as NameValueCollection;
                            if (appSettings != null)
                            {
                                string assemblyNames = appSettings[configNamespace];
                                if (!string.IsNullOrEmpty(assemblyNames))
                                {
                                    foreach (string assemblyName in assemblyNames.Split('|')) Loader.assemblyNames.Add(assemblyName.Trim());
                                }
                                break;
                            }
                        }
                    }
                }
            }
            foreach (Assembly assembly in currentAssemblies)
            {
                if (assembly != mainAssembly && checkName(assembly)) load(assembly);
            }
            if (mainAssembly != null)
            {
                foreach (KeyValue<Key, Creator> config in getConfigs(mainAssembly, true)) configs[config.Key] = config.Value;
            }
        }
    }
}

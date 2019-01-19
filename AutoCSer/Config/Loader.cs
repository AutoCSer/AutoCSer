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
        private struct key : IEquatable<key>
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
            public key(Type type, string name)
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
            public bool Equals(key other)
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
                return Equals((key)obj);
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
        private static readonly Dictionary<key, KeyValue<object, bool>> configs = DictionaryCreator<key>.Create<KeyValue<object, bool>>();
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
            key key = new key(type, name ?? string.Empty);
            KeyValue<object, bool> value;
            Monitor.Enter(configLock);
            if (configs.TryGetValue(key, out value))
            {
                Monitor.Exit(configLock);
                MethodInfo method = value.Key as MethodInfo;
                return method == null ? (value.Key as FieldInfo).GetValue(null) : method.Invoke(null, null);
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
                foreach (KeyValue<key, object> config in getConfigs(assembly))
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
        /// <returns></returns>
        private static IEnumerable<KeyValue<key, object>> getConfigs(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsDefined(typeof(TypeAttribute), false))
                {
                    foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        //object fieldValue = null;
                        foreach (MemberAttribute attribute in field.GetCustomAttributes(typeof(MemberAttribute), false))
                        {
                            if (attribute != null)
                            {
                                //if (fieldValue == null)
                                //{
                                //    fieldValue = field.GetValue(null);
                                //    if (fieldValue == null) break;
                                //}
                                yield return new KeyValue<key, object>(new key(field.FieldType, attribute.Name ?? field.Name), field);
                                //yield return new KeyValue<key, object>(new key(attribute.Type ?? field.FieldType, attribute.Name ?? field.Name), field);
                                break;
                            }
                        }
                    }
                    foreach (PropertyInfo property in type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
                    {
                        if (property.CanRead)
                        {
                            MethodInfo method = property.GetGetMethod(true);
                            if (method != null && method.GetParameters().Length == 0)
                            {
                                //object propertyValue = null;
                                foreach (MemberAttribute attribute in property.GetCustomAttributes(typeof(MemberAttribute), false))
                                {
                                    if (attribute != null)
                                    {
                                        //if (propertyValue == null)
                                        //{
                                        //    propertyValue = method.Invoke(null, null);
                                        //    if (propertyValue == null) break;
                                        //}
                                        //yield return new KeyValue<key, object>(new key(attribute.Type ?? property.PropertyType, attribute.Name ?? property.Name), method);
                                        yield return new KeyValue<key, object>(new key(property.PropertyType, attribute.Name ?? property.Name), method);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 配置加载检测
        /// </summary>
        /// <param name="assembly"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void load(Assembly assembly)
        {
            foreach (KeyValue<key, object> config in getConfigs(assembly)) set(config);
        }
        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="config"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void set(KeyValue<key, object> config)
        {
            KeyValue<object, bool> oldConfig;
            if (!configs.TryGetValue(config.Key, out oldConfig) || !oldConfig.Value) configs[config.Key] = new KeyValue<object, bool>(config.Value, false);
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
                foreach (KeyValue<key, object> config in getConfigs(mainAssembly)) configs[config.Key] = new KeyValue<object, bool>(config.Value, true);
            }
        }
    }
}

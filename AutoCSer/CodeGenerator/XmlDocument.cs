using System;
using System.Reflection;
using System.IO;
using System.Text;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// XML 文档注释
    /// </summary>
    internal static class XmlDocument
    {
        /// <summary>
        /// 程序集信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockLastDictionary<Assembly, XmlDocumentAssembly> assemblyLock = new AutoCSer.Threading.LockLastDictionary<Assembly, XmlDocumentAssembly>();
        /// <summary>
        /// XML解析配置
        /// </summary>
        private static AutoCSer.Xml.ParseConfig xmlParserConfig = new AutoCSer.Xml.ParseConfig { BootNodeName = "doc", IsAttribute = true };
        /// <summary>
        /// 获取程序集信息
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private unsafe static XmlDocumentAssembly get(Assembly assembly)
        {
            if (assembly != null)
            {
                XmlDocumentAssembly value;
                if (assemblyLock.TryGetValue(assembly, out value)) return value;
                try
                {
                    string fileName = assembly.Location;
                    if (fileName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        if (File.Exists(fileName = fileName.Substring(0, fileName.Length - 3) + "xml"))
                        {
                            Xml.Node xmlNode = AutoCSer.Xml.Parser.Parse<Xml.Node>(File.ReadAllText(fileName, Encoding.UTF8), xmlParserConfig)["members"];
                            if (xmlNode.Type == Xml.NodeType.Node)
                            {
                                fixed (char* nameFixed = "name")
                                {
                                    value = new XmlDocumentAssembly();
                                    Range attribute = default(Range);
                                    foreach (KeyValue<SubString, Xml.Node> node in xmlNode.Nodes)
                                    {
                                        if (node.Value.Type == Xml.NodeType.Node && node.Key.Equals("member"))
                                        {
                                            if (node.Value.GetAttribute(nameFixed, 4, ref attribute) && attribute.Length > 2)
                                            {
                                                value.LoadMember(new SubString { String = node.Key.String, Start = attribute.StartIndex, Length = attribute.Length }, node.Value);
                                            }
                                        }
                                    }
                                }
                            }
                            else AutoCSer.Log.Pub.Log.Wait(Log.LogType.All, "XML文档解析失败 " + fileName);
                        }
                        else AutoCSer.Log.Pub.Log.Wait(Log.LogType.All, "没有找到XML文档注释 " + fileName);
                    }
                    assemblyLock.Set(assembly, value);
                }
                finally { assemblyLock.Exit(); }
                return value;
            }
            return null;
        }
        /// <summary>
        /// 获取类型描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string Get(Type type)
        {
            XmlDocumentAssembly assembly = get(type.Assembly);
            return assembly == null ? string.Empty : assembly.GetSummary(type);
        }
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string Get(FieldInfo field)
        {
            XmlDocumentAssembly assembly = get(field.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.GetSummary(field);
        }
        /// <summary>
        /// 获取属性描述
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string Get(PropertyInfo property)
        {
            XmlDocumentAssembly assembly = get(property.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.GetSummary(property);
        }
        /// <summary>
        /// 获取方法描述
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string Get(MethodInfo method)
        {
            XmlDocumentAssembly assembly = get(method.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.GetSummary(method);
        }
        /// <summary>
        /// 获取方法返回值描述
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string GetReturn(MethodInfo method)
        {
            XmlDocumentAssembly assembly = get(method.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.GetReturn(method);
        }
        /// <summary>
        /// 获取参数描述
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string Get(MethodInfo method, ParameterInfo parameter)
        {
            XmlDocumentAssembly assembly = get(method.DeclaringType.Assembly);
            return assembly == null ? string.Empty : assembly.Get(method, parameter);
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            assemblyLock.Clear();
        }
        static XmlDocument()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}

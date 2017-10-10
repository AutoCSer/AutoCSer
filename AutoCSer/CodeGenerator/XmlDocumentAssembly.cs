using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 程序集 XML 文档注释信息
    /// </summary>
    internal unsafe sealed class XmlDocumentAssembly
    {
        /// <summary>
        /// 类型：类、接口、结构、枚举、委托
        /// </summary>
        private readonly Dictionary<HashString, Xml.Node> types = DictionaryCreator.CreateHashString<Xml.Node>();
        /// <summary>
        /// 类型集合访问锁
        /// </summary>
        private AutoCSer.Threading.LockLastDictionary<Type, Xml.Node> typeLock = new AutoCSer.Threading.LockLastDictionary<Type, Xml.Node>();
        /// <summary>
        /// 类型名称流
        /// </summary>
        private readonly CharStream typeNameStream = new CharStream(null, 0);
        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Xml.Node get(Type type)
        {
            if (type != null)
            {
                Xml.Node node;
                if (typeLock.TryGetValue(type, out node)) return node;
                try
                {
                    HashString typeName;
                    byte* buffer = UnmanagedPool.Default.Get();
                    try
                    {
                        using (typeNameStream)
                        {
                            typeNameStream.Reset(buffer, UnmanagedPool.DefaultSize);
                            AutoCSer.Extension.TypeCodeGenerator.NameBuilder nameBuilder = new AutoCSer.Extension.TypeCodeGenerator.NameBuilder { NameStream = typeNameStream, IsXml = true };
                            nameBuilder.Xml(type);
                            typeName = typeNameStream.ToString();
                        }
                    }
                    finally { UnmanagedPool.Default.Push(buffer); }
                    if (types.TryGetValue(typeName, out node)) types.Remove(typeName);
                    typeLock.Set(type, node);
                }
                finally { typeLock.Exit(); }
                return node;
            }
            return default(Xml.Node);
        }
        /// <summary>
        /// 获取类型描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetSummary(Type type)
        {
            return get(get(type), "summary");
        }
        /// <summary>
        /// 字段
        /// </summary>
        private readonly Dictionary<HashString, Xml.Node> fields = DictionaryCreator.CreateHashString<Xml.Node>();
        /// <summary>
        /// 字段集合访问锁
        /// </summary>
        private AutoCSer.Threading.LockLastDictionary<FieldInfo, Xml.Node> fieldLock = new AutoCSer.Threading.LockLastDictionary<FieldInfo, Xml.Node>();
        /// <summary>
        /// 字段
        /// </summary>
        private readonly CharStream fieldNameStream = new CharStream(null, 0);
        /// <summary>
        /// 获取字段信息
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private Xml.Node get(FieldInfo field)
        {
            if (field != null)
            {
                Xml.Node node;
                if (fieldLock.TryGetValue(field, out node)) return node;
                try
                {
                    HashString fieldName;
                    byte* buffer = UnmanagedPool.Default.Get();
                    try
                    {
                        using (fieldNameStream)
                        {
                            fieldNameStream.Reset(buffer, UnmanagedPool.DefaultSize);
                            AutoCSer.Extension.TypeCodeGenerator.NameBuilder nameBuilder = new AutoCSer.Extension.TypeCodeGenerator.NameBuilder { NameStream = fieldNameStream, IsXml = true };
                            nameBuilder.Xml(field.DeclaringType);
                            fieldNameStream.Write('.');
                            fieldNameStream.SimpleWriteNotNull(field.Name);
                            fieldName = fieldNameStream.ToString();
                        }
                    }
                    finally { UnmanagedPool.Default.Push(buffer); }
                    if (fields.TryGetValue(fieldName, out node)) fields.Remove(fieldName);
                    fieldLock.Set(field, node);
                }
                finally { fieldLock.Exit(); }
                return node;
            }
            return default(Xml.Node);
        }
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetSummary(FieldInfo field)
        {
            return get(get(field), "summary");
        }
        /// <summary>
        /// 属性（包括索引程序或其他索引属性）
        /// </summary>
        private readonly Dictionary<HashString, Xml.Node> properties = DictionaryCreator.CreateHashString<Xml.Node>();
        /// <summary>
        /// 属性集合访问锁
        /// </summary>
        private AutoCSer.Threading.LockLastDictionary<PropertyInfo, Xml.Node> propertyLock = new AutoCSer.Threading.LockLastDictionary<PropertyInfo, Xml.Node>();
        /// <summary>
        /// 属性
        /// </summary>
        private readonly CharStream propertyNameStream = new CharStream(null, 0);
        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private Xml.Node get(PropertyInfo property)
        {
            if (property != null)
            {
                Xml.Node node;
                if (propertyLock.TryGetValue(property, out node)) return node;
                try
                {
                    HashString propertyName;
                    byte* buffer = UnmanagedPool.Default.Get();
                    try
                    {
                        using (propertyNameStream)
                        {
                            propertyNameStream.Reset(buffer, UnmanagedPool.DefaultSize);
                            AutoCSer.Extension.TypeCodeGenerator.NameBuilder nameBuilder = new AutoCSer.Extension.TypeCodeGenerator.NameBuilder { NameStream = propertyNameStream, IsXml = true };
                            nameBuilder.Xml(property.DeclaringType);
                            propertyNameStream.Write('.');
                            propertyNameStream.SimpleWriteNotNull(property.Name);
                            propertyName = propertyNameStream.ToString();
                        }
                    }
                    finally { UnmanagedPool.Default.Push(buffer); }
                    if (properties.TryGetValue(propertyName, out node)) properties.Remove(propertyName);
                    propertyLock.Set(property, node);
                }
                finally { propertyLock.Exit(); }
                return node;
            }
            return default(Xml.Node);
        }
        /// <summary>
        /// 获取属性描述
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetSummary(PropertyInfo property)
        {
            return get(get(property), "summary");
        }
        /// <summary>
        /// 方法（包括一些特殊方法，例如构造函数、运算符等）
        /// </summary>
        private readonly Dictionary<HashString, Xml.Node> methods = DictionaryCreator.CreateHashString<Xml.Node>();
        /// <summary>
        /// 方法集合访问锁
        /// </summary>
        private AutoCSer.Threading.LockLastDictionary<MethodInfo, Xml.Node> methodLock = new AutoCSer.Threading.LockLastDictionary<MethodInfo, Xml.Node>();
        /// <summary>
        /// 方法名称流
        /// </summary>
        private readonly CharStream methodNameStream = new CharStream(null, 0);
        /// <summary>
        /// 获取方法信息
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private Xml.Node get(MethodInfo method)
        {
            if (method != null)
            {
                Xml.Node node;
                if (methodLock.TryGetValue(method, out node)) return node;
                try
                {
                    HashString methodName;
                    byte* buffer = UnmanagedPool.Default.Get();
                    try
                    {
                        using (methodNameStream)
                        {
                            methodNameStream.Reset(buffer, UnmanagedPool.DefaultSize);
                            AutoCSer.Extension.TypeCodeGenerator.NameBuilder nameBuilder = new AutoCSer.Extension.TypeCodeGenerator.NameBuilder { NameStream = methodNameStream, IsXml = true };
                            nameBuilder.Xml(method.DeclaringType);
                            methodNameStream.Write('.');
                            string name = method.Name;
                            if (name[0] == '.')
                            {
                                methodNameStream.Write('#');
                                methodNameStream.Write(new SubString { String = name, Start = 1, Length = name.Length - 1 });
                            }
                            else methodNameStream.SimpleWriteNotNull(name);
                            ParameterInfo[] parameters = method.GetParameters();
                            if (parameters.Length != 0)
                            {
                                bool isFirst = true;
                                methodNameStream.Write('(');
                                foreach (ParameterInfo parameter in parameters)
                                {
                                    if (isFirst) isFirst = false;
                                    else methodNameStream.Write(',');
                                    nameBuilder.Xml(parameter.ParameterType);
                                }
                                methodNameStream.Write(')');
                            }
                            formatName(methodNameStream.Char, methodNameStream.CurrentChar);
                            methodName = methodNameStream.ToString();
                        }
                    }
                    finally { UnmanagedPool.Default.Push(buffer); }
                    if (methods.TryGetValue(methodName, out node)) methods.Remove(methodName);
                    methodLock.Set(method, node);
                }
                finally { methodLock.Exit(); }
                return node;
            }
            return default(Xml.Node);
        }
        /// <summary>
        /// 获取方法描述
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetSummary(MethodInfo method)
        {
            return get(get(method), "summary");
        }
        /// <summary>
        /// 获取方法返回值描述
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetReturn(MethodInfo method)
        {
            return get(get(method), "returns");
        }
        /// <summary>
        /// 获取参数描述
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public unsafe string Get(MethodInfo method, ParameterInfo parameter)
        {
            Xml.Node xmlNode = get(method);
            if (xmlNode.Type == Xml.NodeType.Node)
            {
                string parameterName = parameter.Name;
                Range attribute = default(Range);
                fixed (char* nameFixed = "name", parameterFixed = parameterName)
                {
                    foreach (KeyValue<SubString, Xml.Node> node in xmlNode.Nodes)
                    {
                        if (node.Value.Type != Xml.NodeType.Node && node.Key.Equals("param")
                            && node.Value.GetAttribute(nameFixed, 4, ref attribute)
                            && attribute.Length == parameterName.Length)
                        {
                            fixed (char* attributeFixed = node.Key.String)
                            {
                                if (AutoCSer.Memory.SimpleEqualNotNull((byte*)parameterFixed, (byte*)(attributeFixed + attribute.StartIndex), parameterName.Length << 1))
                                {
                                    return node.Value.String.Length == 0 ? string.Empty : node.Value.String.ToString();
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 名称格式化
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void formatName(char* start, char* end)
        {
            do
            {
                if (*start == '&') *start = '@';
            }
            while (++start != end);
        }
        /// <summary>
        /// 加载数据记录
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="node"></param>
        internal void LoadMember(SubString name, Xml.Node node)
        {//https://msdn.microsoft.com/zh-cn/library/fsbx0t7x(v=vs.80).aspx
            //对于泛型类型，类型名称后跟反勾号，再跟一个数字，指示泛型类型参数的个数。例如，
            //<member name="T:SampleClass`2"> 是定义为 public class SampleClass<T, U> 的类型的标记。
            if (name[1] == ':')
            {
                char code = name[0];
                switch (code & 7)
                {
                    case 'T' & 7://类型：类、接口、结构、枚举、委托
                        if (code == 'T') types[name.GetSub(2)] = node;
                        break;
                    case 'F' & 7://字段
                        if (code == 'F') fields[name.GetSub(2)] = node;
                        break;
                    case 'P' & 7://属性（包括索引程序或其他索引属性）
                        if (code == 'P') properties[name.GetSub(2)] = node;
                        break;
                    case 'M' & 7://方法（包括一些特殊方法，例如构造函数、运算符等）
                        if (code == 'M') methods[name.GetSub(2)] = node;
                        break;
                    //case 'E' & 7://事件
                    //    break;
                    //case 'N' & 7://命名空间
                    //case '!' & 7://错误字符串
                    //break;
                }
            }
        }
        /// <summary>
        /// 获取节点字符串
        /// </summary>
        /// <param name="node">成员节点</param>
        /// <param name="name">节点名称</param>
        /// <returns>字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static string get(Xml.Node node, string name)
        {
            return (node = node[name]).String.Length != 0 ? node.String.ToString() : string.Empty;
        }
    }
}
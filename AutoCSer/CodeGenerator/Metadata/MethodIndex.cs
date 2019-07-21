using System;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extension;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.CodeGenerator.Metadata
{
    /// <summary>
    /// 成员方法
    /// </summary>
    internal sealed class MethodIndex : MemberIndex
    {
        /// <summary>
        /// 成员方法信息
        /// </summary>
        public MethodInfo Method { get; private set; }
        /// <summary>
        /// 自定义方法相关成员信息
        /// </summary>
        private MemberInfo customMember;
        /// <summary>
        /// 自定义方法是否取值，否则为设置值
        /// </summary>
        public bool IsGetMember { get; private set; }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public ExtensionType ReturnType { get; private set; }
        /// <summary>
        /// 参数集合
        /// </summary>
        public MethodParameter[] Parameters { get; private set; }
        /// <summary>
        /// 泛型参数类型集合
        /// </summary>
        public ExtensionType[] GenericParameters { get; private set; }
        /// <summary>
        /// 泛型参数拼写
        /// </summary>
        private string genericParameterName;
        /// <summary>
        /// 泛型参数拼写
        /// </summary>
        public string GenericParameterName
        {
            get
            {
                if (genericParameterName == null)
                {
                    ExtensionType[] genericParameters = GenericParameters;
                    genericParameterName = genericParameters.Length == 0 ? string.Empty : ("<" + genericParameters.joinString(',', value => value.FullName) + ">");
                }
                return genericParameterName;
            }
        }
        /// <summary>
        /// 参数集合
        /// </summary>
        public MethodParameter[] OutputParameters { get; private set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName
        {
            get { return Method == null ? (IsGetMember ? "get_" : "set_") + customMember.Name : Method.Name; }
        }
        /// <summary>
        /// 方法泛型名称
        /// </summary>
        public string StaticMethodName
        {
            get { return MethodName; }
        }
        /// <summary>
        /// 方法泛型名称
        /// </summary>
        public string GenericMethodName
        {
            get
            {
                return MethodName + GenericParameterName;
            }
        }
        /// <summary>
        /// 方法泛型名称
        /// </summary>
        public string GenericStaticMethodName
        {
            get
            {
                return StaticMethodName + GenericParameterName;
            }
        }
        /// <summary>
        /// 异步函数名称
        /// </summary>
        public string TaskAsyncMethodName
        {
            get { return MethodName + "Async"; }
        }
        /// <summary>
        /// 异步函数名称
        /// </summary>
        public string AwaiterMethodName
        {
            get { return MethodName + "Awaiter"; }
        }
        /// <summary>
        /// 方法全称标识
        /// </summary>
        public string MethodKeyFullName
        {
            get
            {
                return (customMember ?? Method).DeclaringType.fullName() + MethodKeyName;
            }
        }
        /// <summary>
        /// 方法标识
        /// </summary>
        public string MethodKeyName
        {
            get
            {
                return "(" + Parameters.joinString(',', value => value.ParameterRef + value.ParameterType.FullName2) + ")" + GenericParameterName + MethodName;
            }
        }
        /// <summary>
        /// XML文档注释
        /// </summary>
        private new string xmlDocument;
        /// <summary>
        /// XML文档注释
        /// </summary>
        public override string XmlDocument
        {
            get
            {
                if (xmlDocument == null)
                {
                    if (customMember == null) xmlDocument = Method == null ? string.Empty : CodeGenerator.XmlDocument.Get(Method);
                    else
                    {
                        PropertyInfo property = customMember as PropertyInfo;
                        xmlDocument = property == null ? CodeGenerator.XmlDocument.Get((FieldInfo)customMember) : CodeGenerator.XmlDocument.Get(property);
                    }
                }
                return xmlDocument.Length == 0 ? null : xmlDocument;
            }
        }
        /// <summary>
        /// 返回值XML文档注释
        /// </summary>
        private string returnXmlDocument;
        /// <summary>
        /// 返回值XML文档注释
        /// </summary>
        public string ReturnXmlDocument
        {
            get
            {
                if (returnXmlDocument == null)
                {
                    if (customMember == null) returnXmlDocument = Method == null ? string.Empty : CodeGenerator.XmlDocument.GetReturn(Method);
                    else returnXmlDocument = XmlDocument ?? string.Empty;
                }
                return returnXmlDocument.Length == 0 ? null : returnXmlDocument;
            }
        }
        /// <summary>
        /// 自定义方法属性返回值参数
        /// </summary>
        public MethodParameter PropertyParameter { get; private set; }
        /// <summary>
        /// 成员方法
        /// </summary>
        /// <param name="method">成员方法信息</param>
        /// <param name="filter">选择类型</param>
        /// <param name="index">成员编号</param>
        internal MethodIndex(MethodInfo method, MemberFilters filter, int index)
            : base(method, filter, index)
        {
            Method = method;
            ReturnType = MemberSystemType = method.ReturnType;
            Type[] genericParameters = method.GetGenericArguments();
            Parameters = MethodParameter.Get(method, genericParameters);
            OutputParameters = Parameters.getFindArray(value => value.Parameter.ParameterType.IsByRef);
            GenericParameters = genericParameters.getArray(value => (ExtensionType)value);
        }
        /// <summary>
        /// 成员方法
        /// </summary>
        /// <param name="method">成员方法信息</param>
        /// <param name="awaiterReturnType">await 返回值类型</param>
        internal MethodIndex(MethodInfo method, Type awaiterReturnType)
            : this(method, MemberFilters.PublicInstance, 0)
        {
            this.AwaiterReturnType = awaiterReturnType;
        }
        /// <summary>
        /// 成员方法
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="isGet">是否取值</param>
        internal MethodIndex(FieldInfo field, bool isGet)
            : base(field.FieldType, field.Name)
        {
            customMember = field;
            if (IsGetMember = isGet)
            {
                ReturnType = field.FieldType;
                Parameters = NullValue<MethodParameter>.Array;
            }
            else
            {
                ReturnType = typeof(void);
                Parameters = new MethodParameter[] { new MethodParameter("value", field.FieldType) };
            }
            OutputParameters = NullValue<MethodParameter>.Array;
            GenericParameters = NullValue<ExtensionType>.Array;
        }
        /// <summary>
        /// 成员方法
        /// </summary>
        /// <param name="property">属性信息</param>
        /// <param name="isGet">是否取值</param>
        internal MethodIndex(PropertyInfo property, bool isGet)
            : this(isGet ? property.GetGetMethod(true) : property.GetSetMethod(true), MemberFilters.Instance, 0)
        {
            MemberName = property.Name;
            customMember = property;
            if (IsGetMember = isGet)
            {
                ReturnType = property.PropertyType;
                if (Parameters.Length != 0) PropertyParameter = Parameters[Parameters.Length - 1];
            }
            else
            {
                ReturnType = typeof(void);
                if (Parameters.Length != 1)
                {
                    (PropertyParameter = Parameters[Parameters.Length - 1]).IsPropertyValue = true;
                    Parameters[Parameters.Length - 2].ParameterJoin = null;
                }
            }
            OutputParameters = NullValue<MethodParameter>.Array;
            GenericParameters = NullValue<ExtensionType>.Array;
        }
        /// <summary>
        /// 类型成员方法缓存
        /// </summary>
        private static Dictionary<Type, MethodIndex[]> methodCache = DictionaryCreator.CreateOnly<Type, MethodIndex[]>();
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        private static readonly Func<MethodInfo, MethodInfo, int> methodCompare = compare;
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(MethodInfo left, MethodInfo right)
        {
            int value = string.CompareOrdinal(left.Name, right.Name);
            if (value == 0)
            {
                if (left.IsGenericMethod)
                {
                    if (!right.IsGenericMethod) return 1;
                }
                else if (right.IsGenericMethod) return -1;
                ParameterInfo[] leftParameters = left.GetParameters(), rightParameters = right.GetParameters();
                value = leftParameters.Length - rightParameters.Length;
                if (value == 0)
                {
                    for (int index = 0; index != leftParameters.Length; ++index)
                    {
                        value = compare(leftParameters[index], rightParameters[index]);
                        if (value != 0) return value;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 字符串比较大小
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int compare(ParameterInfo left, ParameterInfo right)
        {
            Type leftType = left.ParameterType, rightType = right.ParameterType;
            if (leftType != rightType)
            {
                int value = string.CompareOrdinal(leftType.FullName, rightType.FullName);
                return value == 0 ? string.CompareOrdinal(left.Name, right.Name) : value;
            }
            return 0;
        }
        /// <summary>
        /// 获取类型的成员方法集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>成员方法集合</returns>
        private static MethodIndex[] getMethods(Type type)
        {
            MethodIndex[] methods;
            if (!methodCache.TryGetValue(type, out methods))
            {
                int index = 0;
                methodCache[type] = methods = AutoCSer.Extension.ArrayExtension.concat(
                    type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).sort(methodCompare).getArray(value => new MethodIndex(value, MemberFilters.PublicStatic, index++)),
                    type.GetMethods(BindingFlags.Public | BindingFlags.Instance).sort(methodCompare).getArray(value => new MethodIndex(value, MemberFilters.PublicInstance, index++)),
                    type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).sort(methodCompare).getArray(value => new MethodIndex(value, MemberFilters.NonPublicStatic, index++)),
                    type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).sort(methodCompare).getArray(value => new MethodIndex(value, MemberFilters.NonPublicInstance, index++)));
            }
            return methods;
        }
        /// <summary>
        /// 获取匹配成员方法集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="isFilter">是否完全匹配选择类型</param>
        /// <returns>匹配的成员方法集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static LeftArray<MethodIndex> GetMethods(Type type, MemberFilters filter, bool isFilter)
        {
            return getMethods(type).getFind(value => isFilter ? (value.MemberFilters & filter) == filter : ((value.MemberFilters & filter) != 0));
        }
        /// <summary>
        /// 获取匹配成员方法集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="methods">成员方法集合</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">指定是否搜索该成员的继承链以查找这些特性，参考System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)。</param>
        /// <returns>匹配成员方法集合</returns>
        private static MethodIndex[] getMethods<attributeType>
            (Type type, LeftArray<MethodIndex> methods, bool isAttribute, bool isBaseType)
            where attributeType : IgnoreMemberAttribute
        {
            if (isAttribute)
            {
                return methods.GetFindArray(value => value.IsAttribute<attributeType>(isBaseType));
            }
            else
            {
                return methods.GetFindArray(value => value.Method.DeclaringType == type && !value.IsIgnoreAttribute<attributeType>(isBaseType));
            }
        }
        /// <summary>
        /// 获取匹配成员方法集合
        /// </summary>
        /// <typeparam name="attributeType">自定义属性类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="filter">选择类型</param>
        /// <param name="isFilter">是否完全匹配选择类型</param>
        /// <param name="isAttribute">是否匹配自定义属性类型</param>
        /// <param name="isBaseType">指定是否搜索该成员的继承链以查找这些特性，参考System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)。</param>
        /// <returns>匹配成员方法集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static MethodIndex[] GetMethods<attributeType>(Type type, MemberFilters filter, bool isFilter, bool isAttribute, bool isBaseType)
            where attributeType : IgnoreMemberAttribute
        {
            return getMethods<attributeType>(type, GetMethods(type, filter, isFilter), isAttribute, isBaseType);
        }

        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            if (methodCache.Count != 0) methodCache = DictionaryCreator.CreateOnly<Type, MethodIndex[]>();
        }

        static MethodIndex()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}

using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extensions;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    internal static partial class SerializeMethodCache
    {
        /// <summary>
        /// 未知类型序列化调用函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> memberMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 未知类型枚举序列化委托调用函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>未知类型序列化委托调用函数信息</returns>
        public static MethodInfo GetMember(Type type)
        {
            MethodInfo method;
            if (memberMethods.TryGetValue(type, out method)) return method;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                if (elementType.IsValueType)
                {
                    if (elementType.IsEnum)
                    {
                        method = EnumGenericType.Get(elementType).BinarySerializeEnumArrayMemberDelegate.Method;
                    }
                    else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        //method = nullableArrayMemberMethod.MakeGenericMethod(elementType.GetGenericArguments());
                        method = StructGenericType.Get(elementType.GetGenericArguments()[0]).BinarySerializeNullableArrayMemberMethod;
                    }
                    //else method = structArrayMemberMethod.MakeGenericMethod(elementType);
                    else method = GenericType.Get(elementType).BinarySerializeStructArrayMemberMethod;
                }
                //else method = arrayMemberMethod.MakeGenericMethod(elementType);
                else method = ClassGenericType.Get(elementType).BinarySerializeArrayMemberMethod;
            }
            else if (type.IsEnum) method = (EnumGenericType.Get(type).BinarySerializeEnumMemberDelegate).Method;
            else
            {
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    if (genericType == typeof(Dictionary<,>) || genericType == typeof(SortedDictionary<,>) || genericType == typeof(SortedList<,>))
                    {
                        Type[] parameterTypes = type.GetGenericArguments();
                        //method = dictionaryMemberMethod.MakeGenericMethod(type, parameterTypes[0], parameterTypes[1]);
                        method = DictionaryGenericType3.Get(type, parameterTypes[0], parameterTypes[1]).BinarySerializeDictionaryMemberMethod;
                    }
                    else if (genericType == typeof(Nullable<>))
                    {
                        //method = nullableMemberSerializeMethod.MakeGenericMethod(type.GetGenericArguments());
                        method = StructGenericType.Get(type.GetGenericArguments()[0]).BinarySerializeNullableMemberMethod;
                    }
                    else if (genericType == typeof(KeyValuePair<,>))
                    {
                        //method = KeyValuePairSerializeMethod.MakeGenericMethod(type.GetGenericArguments());
                        method = GenericType2.Get(type.GetGenericArguments()).BinarySerializeKeyValuePairMethod.Method;
                    }
                }
                if (method == null)
                {
                    //if (type.IsValueType) method = structSerializeMethod.MakeGenericMethod(type);
                    //else method = memberClassSerializeMethod.MakeGenericMethod(type);
                    if (type.IsValueType) method = StructGenericType.Get(type).BinarySerializeStructMethod;
                    else method = ClassGenericType.Get(type).BinarySerializeMemberClassMethod;
                }
            }
            memberMethods.Set(type, method);
            return method;
        }

        /// <summary>
        /// 获取字段成员集合
        /// </summary>
        /// <param name="fieldIndexs"></param>
        /// <param name="isJson"></param>
        /// <param name="memberCountVerify"></param>
        /// <returns>字段成员集合</returns>
        public static Fields<FieldSize> GetFields(FieldIndex[] fieldIndexs, bool isJson, out int memberCountVerify)
        {
            LeftArray<FieldSize> fixedFields = new LeftArray<FieldSize>(fieldIndexs.Length), fields = new LeftArray<FieldSize>(fieldIndexs.Length);
            LeftArray<FieldIndex> jsonFields = new LeftArray<FieldIndex>(0);
            int fixedSize = 0;
            foreach (FieldIndex field in fieldIndexs)
            {
                Type fieldType = field.Member.FieldType;
                if (!fieldType.IsPointer && (!fieldType.IsArray || fieldType.GetArrayRank() == 1) && !field.IsIgnore && !typeof(Delegate).IsAssignableFrom(fieldType))
                {
                    BinarySerializeMemberAttribute memberAttribute = field.GetAttribute<BinarySerializeMemberAttribute>(true);
                    if (memberAttribute == null || (memberAttribute.IsSetup && !memberAttribute.IsRemove))
                    {
                        if (memberAttribute != null && memberAttribute.GetIsJson) jsonFields.Add(field);
                        else
                        {
                            FieldSize value = new FieldSize(field);
                            if (value.FixedSize == 0) fields.Add(value);
                            else
                            {
                                fixedFields.Add(value);
                                fixedSize += value.FixedSize;
                            }
                        }
                    }
                }
            }
            return new Fields<FieldSize>(ref fixedFields, ref fields, ref jsonFields, fixedSize, isJson, out memberCountVerify);
        }
        /// <summary>
        /// 对象序列化函数信息
        /// </summary>
        internal static readonly MethodInfo BaseSerializeMethod = typeof(BinarySerializer).GetMethod("baseSerialize", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 获取自定义序列化函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isSerializer"></param>
        /// <returns></returns>
        public static MethodInfo GetCustom(Type type, bool isSerializer)
        {
            MethodInfo serializeMethod = null, deSerializeMethod = null;
            if (type.IsValueType)
            {
                foreach (AutoCSer.Metadata.AttributeMethod method in AutoCSer.Metadata.AttributeMethod.Get(type))
                {
                    if (method.Method.ReturnType == typeof(void) && method.GetAttribute<BinarySerializeCustomAttribute>() != null)
                    {
                        ParameterInfo[] parameters = method.Method.GetParameters();
                        if (parameters.Length == 1)
                        {
                            if (parameters[0].ParameterType == typeof(BinarySerializer))
                            {
                                if (deSerializeMethod != null) return isSerializer ? method.Method : deSerializeMethod;
                                serializeMethod = method.Method;
                            }
                            else if (parameters[0].ParameterType == typeof(BinaryDeSerializer))
                            {
                                if (serializeMethod != null) return isSerializer ? serializeMethod : method.Method;
                                deSerializeMethod = method.Method;
                            }
                        }
                    }
                }
            }
            else
            {
                Type refType = type.MakeByRefType();
                foreach (AutoCSer.Metadata.AttributeMethod method in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                {
                    if (method.Method.ReturnType == typeof(void) && method.GetAttribute<BinarySerializeCustomAttribute>() != null)
                    {
                        ParameterInfo[] parameters = method.Method.GetParameters();
                        if (parameters.Length == 2)
                        {
                            if (parameters[0].ParameterType == typeof(BinarySerializer))
                            {
                                if (parameters[1].ParameterType == type)
                                {
                                    if (deSerializeMethod != null) return isSerializer ? method.Method : deSerializeMethod;
                                    serializeMethod = method.Method;
                                }
                            }
                            else if (parameters[0].ParameterType == typeof(BinaryDeSerializer))
                            {
                                if (parameters[1].ParameterType == refType)
                                {
                                    if (serializeMethod != null) return isSerializer ? serializeMethod : method.Method;
                                    deSerializeMethod = method.Method;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 是否支持循环引用处理
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsReferenceMember(Type type)
        {
            return GenericType.Get(type).BinarySerializeIsReferenceMember;
            //bool isReferenceMember;
            //if (isReferenceMembers.TryGetValue(type, out isReferenceMember)) return isReferenceMember;
            //isReferenceMembers.Set(type, isReferenceMember = (bool)isReferenceMemberMethod.MakeGenericMethod(type).Invoke(null, null));
            //return isReferenceMember;
        }

        static SerializeMethodCache()
        {
            AutoCSer.Memory.Common.AddClearCache(memberMethods.Clear, typeof(SerializeMethodCache), 60 * 60);
        }
    }
}

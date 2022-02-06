using System;
using System.Collections.Generic;
using System.Reflection;
using AutoCSer.Extensions;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 反射模式类型扫描（非线程安全）
    /// </summary>
    public class ReflectionTypeScanner : TypeScanner
    {
        /// <summary>
        /// 类型统计数据集合
        /// </summary>
        internal readonly Dictionary<Type, ReflectionType> ObjectTypeCache = DictionaryCreator.CreateOnly<Type, ReflectionType>();
        /// <summary>
        /// 获取统计数组类型集合
        /// </summary>
        public IEnumerable<ReflectionArrayType> ArrayTypes
        {
            get
            {
                return ObjectTypeCache.Values.where(p => p.RootFields.Length != 0 && p.IsArray)
                    .cast<ReflectionArrayType>();
            }
        }
        /// <summary>
        /// 获取统计数组类型集合
        /// </summary>
        public IEnumerable<ReflectionObjectType> ObjectTypes
        {
            get
            {
                return ObjectTypeCache.Values.where(p => p.RootFields.Length != 0 && !p.IsArray)
                    .cast<ReflectionObjectType>();
            }
        }
        /// <summary>
        /// 反射模式对象扫描（非线程安全）
        /// </summary>
        /// <param name="maxObjectCount">每个静态根对象最大统计对象数量</param>
        /// <param name="maxRootFieldCount">静态根对象最大统计数量，超出部分淘汰数量最小的静态根对象</param>
        public ReflectionTypeScanner(int maxObjectCount = 65536, int maxRootFieldCount = 3) : base(maxObjectCount, maxRootFieldCount)
        {
            ObjectTypeCache.Add(typeof(object), ReflectionObjectType.NullType);
            ObjectTypeCache.Add(typeof(ValueType), ReflectionObjectType.NullType);
            ObjectTypeCache.Add(typeof(System.Reflection.Pointer), ReflectionObjectType.NullType);
            ObjectTypeCache.Add(typeof(string), new ReflectionArrayType());
        }
        /// <summary>
        /// 获取对象类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns>对象类型</returns>
        internal ReflectionType GetObjectType(Type type)
        {
            if (type.IsPrimitive || type.IsEnum || type.IsPointer) return ReflectionObjectType.NullType;
            if (type.IsInterface) return ReflectionObjectType.InterfaceType;
            if (type.IsClass || type.IsValueType)
            {
                ReflectionType objectType;
                if (!ObjectTypeCache.TryGetValue(type, out objectType))
                {
                    if (type.IsArray)
                    {
                        ReflectionArrayType reflectionType = new ReflectionArrayType(type);
                        ObjectTypeCache.Add(type, objectType = reflectionType);
                        reflectionType.GetElementType(this);
                    }
                    else
                    {
                        ReflectionObjectType reflectionType = new ReflectionObjectType(type);
                        ObjectTypeCache.Add(type, objectType = reflectionType);
                        reflectionType.CreateScanFiled(this);
                    }
                }
                return objectType;
            }
            return ReflectionObjectType.NullType;
        }
        /// <summary>
        /// 扫描根对象
        /// </summary>
        /// <param name="fieldInfo">根对象静态字段</param>
        /// <param name="value">根对象</param>
        protected override void scanRoot(FieldInfo fieldInfo, object value)
        {
            Type fieldType = value.GetType();
            ReflectionType objectType = GetObjectType(fieldType);
            if (objectType.IsScan != 0)
            {
                if (fieldType.IsClass)
                {
                    objectType.Add(fieldInfo, this);
                    if (fieldType == typeof(string)) ((ReflectionArrayType)objectType).ElementCount += ((string)value).Length;
                    else new ReflectionScanner(this, fieldInfo).ScanObject(value, objectType);
                }
                else new ReflectionScanner(this, fieldInfo).Scan(value, objectType);
            }
        }
    }
}

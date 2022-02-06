using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 反射模式对象类型
    /// </summary>
    public sealed class ReflectionObjectType : ReflectionType
    {
        /// <summary>
        /// 父类型
        /// </summary>
        internal ReflectionObjectType BaseType;
        /// <summary>
        /// 需要扫描的字段集合
        /// </summary>
        internal KeyValue<FieldInfo, ReflectionType>[] ScanFields;
        /// <summary>
        /// 空对象类型
        /// </summary>
        private ReflectionObjectType() : base(typeof(object))
        {
            BaseType = this;
            ScanFields = EmptyArray<KeyValue<FieldInfo, ReflectionType>>.Array;
        }
        /// <summary>
        /// 接口类型
        /// </summary>
        /// <param name="baseType"></param>
        private ReflectionObjectType(ReflectionObjectType baseType) : base(typeof(object))
        {
            BaseType = baseType;
            ScanFields = EmptyArray<KeyValue<FieldInfo, ReflectionType>>.Array;
            IsScan = 1;
        }
        /// <summary>
        /// 对象类型
        /// </summary>
        /// <param name="type">类型</param>
        internal ReflectionObjectType(Type type) : base(type)
        {
            IsScan = 1;
            ScanFields = EmptyArray<KeyValue<FieldInfo, ReflectionType>>.Array;
            BaseType = NullType;
        }
        /// <summary>
        /// 初始化需要扫描的字段集合
        /// </summary>
        /// <param name="scanner">对象扫描</param>
        internal void CreateScanFiled(ReflectionTypeScanner scanner)
        {
            BaseType = (ReflectionObjectType)scanner.GetObjectType(Type.BaseType);

            List<KeyValue<FieldInfo, ReflectionType>> fieldList = null;
            foreach (FieldInfo field in Type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                Type fieldType = field.FieldType;
                if (!fieldType.IsPointer && fieldType != typeof(System.Reflection.Pointer))
                {
                    ReflectionType objectType = scanner.GetObjectType(fieldType);
                    if (objectType.IsScan != 0 || IsScanDerived(fieldType))
                    {
                        if (fieldList == null) fieldList = new List<KeyValue<FieldInfo, ReflectionType>>();
                        fieldList.Add(new KeyValue<FieldInfo, ReflectionType>(field, objectType));
                    }
                }
            }
            if (fieldList != null) ScanFields = fieldList.ToArray();
            else if (BaseType.IsScan == 0) IsScan = 0;
        }
        /// <summary>
        /// 添加扫描对象
        /// </summary>
        /// <param name="scanner"></param>
        /// <param name="value"></param>
        /// <param name="isArray">对象是否数组元素，数组元素不统计根静态字段</param>
        /// <returns>添加对象类型</returns>
        internal override byte Append(ref ReflectionScanner scanner, object value, bool isArray)
        {
            if (ScanFields.Length != 0)
            {
                scanner.Append(new ReflectionObjectScanner(value, this, isArray));
                return 0;
            }
            return BaseType.IsScan == 0 ? (byte)2 : BaseType.Append(ref scanner, value, isArray);
        }

        /// <summary>
        /// 空对象类型
        /// </summary>
        internal static readonly ReflectionObjectType NullType;
        /// <summary>
        /// 接口对象类型
        /// </summary>
        internal static readonly ReflectionObjectType InterfaceType;
        static ReflectionObjectType()
        {
            InterfaceType = new ReflectionObjectType(NullType = new ReflectionObjectType());
        }
    }
}

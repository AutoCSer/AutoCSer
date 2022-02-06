using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.ObjectRoot
{
    /// <summary>
    /// 对象类型
    /// </summary>
    public abstract class ObjectType
    {
        /// <summary>
        /// 类型
        /// </summary>
        public readonly Type Type;
        /// <summary>
        /// 是否数组（包括 string）
        /// </summary>
        internal virtual bool IsArray { get { return false; } }
        /// <summary>
        /// 对象根静态字段集合
        /// </summary>
        internal LeftArray<KeyValue<FieldInfo, int>> RootFields = new LeftArray<KeyValue<FieldInfo, int>>(0);
        /// <summary>
        /// 是否需要扫描（仅统计 数组与可循环引用对象）
        /// </summary>
        internal int IsScan
        {
            get { return RootFields.Reserve; }
            set { RootFields.Reserve = value; }
        }
        /// <summary>
        /// 对象根静态字段集合
        /// </summary>
        public IList<KeyValue<FieldInfo, int>> RootFieldList
        {
            get { return RootFields; }
        }
        /// <summary>
        /// 对象类型
        /// </summary>
        /// <param name="type">类型</param>
        internal ObjectType(Type type)
        {
            Type = type;
        }
        ///// <summary>
        ///// 增加对象计数
        ///// </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //internal void Increment()
        //{
        //    if (++fields.Length <= 0) fields.Length = int.MaxValue;
        //}
        /// <summary>
        /// 添加根静态字段（仅统计 数组与可循环引用对象）
        /// </summary>
        /// <param name="fieldInfo">对象根静态字段</param>
        /// <param name="scanner">类型扫描</param>
        internal void Add(FieldInfo fieldInfo, TypeScanner scanner)
        {
            if (RootFields.Length == 0) RootFields.Add(new KeyValue<FieldInfo, int>(fieldInfo, 1));
            else
            {
                KeyValue<FieldInfo, int>[] rootFieldArray = RootFields.Array;
                if (!object.Equals(fieldInfo, rootFieldArray[0].Key))
                {
                    if (rootFieldArray.Length >= scanner.MaxRootFieldCount)
                    {
                        int minCount = rootFieldArray[0].Value, minIndex = 0, index = 0;
                        foreach (KeyValue<FieldInfo, int> field in RootFields)
                        {
                            if (field.Value < minCount)
                            {
                                minIndex = index;
                                minCount = field.Value;
                            }
                            ++index;
                        }
                        if (minIndex != 0) rootFieldArray[minIndex] = rootFieldArray[0];
                    }
                    else RootFields.Add(rootFieldArray[0]);
                    RootFields.Array[0].Set(fieldInfo, 1);
                }
                else ++rootFieldArray[0].Value;
            }
        }

        /// <summary>
        /// 是否需要扫描派生类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsScanDerived(Type type)
        {
            return type.IsInterface || (type.IsClass && !type.IsSealed);
        }

        ///// <summary>
        ///// 运行时类型
        ///// </summary>
        //private static readonly Type runtimeType;
        ///// <summary>
        ///// 判断类型是否初始化完毕的委托
        ///// </summary>
        //private static readonly Func<Type, bool> isDomainInitialized;
        ///// <summary>
        ///// 判断类型是否初始化完毕
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //internal static bool IsDomainInitialized(Type type)
        //{
        //    if (type.GetType() == runtimeType) return isDomainInitialized(type);
        //    AutoCSer.LogHelper.Add(LogLevel.Debug, $"类型不匹配 {type.GetType().FullName} <> {runtimeType.FullName}", true);
        //    return true;
        //}
        //static ObjectType()
        //{
        //    runtimeType = typeof(Type).GetType();
        //    PropertyInfo domainInitializedProperty = runtimeType.GetProperty("DomainInitialized", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //    DynamicMethod dynamicMethod = new DynamicMethod("RuntimeTypeGetDomainInitialized", typeof(bool), new Type[] { typeof(Type) }, typeof(Type), true);
        //    ILGenerator generator = dynamicMethod.GetILGenerator();
        //    if (domainInitializedProperty?.PropertyType == typeof(bool) && domainInitializedProperty.CanRead)
        //    {
        //        generator.ldarg(0);
        //        generator.call(domainInitializedProperty.GetGetMethod(true));
        //    }
        //    else
        //    {
        //        generator.int32(1);
        //        AutoCSer.LogHelper.Add(LogLevel.Debug, $"没有找到属性 {runtimeType.FullName}.DomainInitialized");
        //    }
        //    generator.Emit(OpCodes.Ret);
        //    isDomainInitialized = (Func<Type, bool>)dynamicMethod.CreateDelegate(typeof(Func<Type, bool>));
        //}
    }
}

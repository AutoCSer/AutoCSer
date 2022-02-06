﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.ColumnGroup
{
#if !NOJIT
    /// <summary>
    /// 数据列转换数组动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ToArray
    {
        /// <summary>
        /// 动态函数
        /// </summary>
        private readonly DynamicMethod dynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private readonly ILGenerator generator;
        /// <summary>
        /// 数据列配置
        /// </summary>
        private readonly ColumnAttribute attribute;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        public ToArray(Type type, ColumnAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlColumnToArray", null, new Type[] { type, typeof(object[]), Field.RefIntType }, type, true);
            generator = dynamicMethod.GetILGenerator();
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(Field field)
        {
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarga_S, 0);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Call, AutoCSer.Sql.Metadata.GenericType.Get(field.DataType).ToArrayMethod);
            }
            else
            {
                if (field.DataType == field.NullableDataType)
                {
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.Emit(OpCodes.Ldarga_S, 0);
                    generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                    if (field.ToSqlCastMethod != null) generator.Emit(OpCodes.Call, field.ToSqlCastMethod);
                    if (!field.IsUnknownJson && field.DataType.IsValueType) generator.Emit(OpCodes.Box, field.DataType);
                    else if (attribute.IsNullStringEmpty && field.DataType == typeof(string)) generator.nullStringEmpty();
                    generator.Emit(OpCodes.Stelem_Ref);
                }
                else
                {
                    AutoCSer.Metadata.StructGenericType StructGenericType = AutoCSer.Metadata.StructGenericType.Get(field.NullableDataType);
                    Label end = generator.DefineLabel();
                    generator.Emit(OpCodes.Ldarga_S, 0);
                    generator.Emit(OpCodes.Ldflda, field.FieldInfo);
                    generator.Emit(OpCodes.Call, StructGenericType.GetNullableHasValueMethod);
                    generator.Emit(OpCodes.Brtrue_S, end);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.Emit(OpCodes.Ldarga_S, 0);
                    generator.Emit(OpCodes.Ldflda, field.FieldInfo);
                    generator.Emit(OpCodes.Call, StructGenericType.GetNullableValueMethod);
                    generator.Emit(OpCodes.Box, field.DataType);
                    generator.Emit(OpCodes.Stelem_Ref);
                    generator.MarkLabel(end);
                }
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Dup);
                generator.Emit(OpCodes.Ldind_I4);
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.Emit(OpCodes.Add);
                generator.Emit(OpCodes.Stind_I4);
            }
        }
        /// <summary>
        /// 创建web表单委托
        /// </summary>
        /// <returns>web表单委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
#endif
    /// <summary>
    /// 数据列
    /// </summary>
    internal static partial class Column<valueType>
    {
        /// <summary>
        /// 数据列转换数组
        /// </summary>
        internal static class ToArray
        {
            /// <summary>
            /// 数据列转换数组
            /// </summary>
            /// <param name="values">目标数组</param>
            /// <param name="value">数据列</param>
            /// <param name="index">当前读取位置</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Write(valueType value, object[] values, ref int index)
            {
#if NOJIT
                if (fields != null)
                {
                    object[] sqlColumnParameters = null, castParameters = null;
                    object objectValue = value;
                    foreach (sqlModel.toArrayField field in fields)
                    {
                        AutoCSer.code.cSharp.sqlModel.fieldInfo fieldInfo = field.Field;
                        if (fieldInfo.IsSqlColumn)
                        {
                            if (sqlColumnParameters == null) sqlColumnParameters = new object[] { null, values, null };
                            sqlColumnParameters[0] = fieldInfo.Field.GetValue(objectValue);
                            sqlColumnParameters[2] = index;
                            field.SqlColumnMethod.Invoke(null, sqlColumnParameters);
                            index = (int)sqlColumnParameters[2];
                        }
                        else
                        {
                            object memberValue = fieldInfo.Field.GetValue(objectValue);
                            if (field.NullableHasValueMethod == null)
                            {
                                if (fieldInfo.ToSqlCastMethod != null)
                                {
                                    if (castParameters == null) castParameters = new object[1];
                                    castParameters[0] = memberValue;
                                    memberValue = fieldInfo.ToSqlCastMethod.Invoke(null, castParameters);
                                }
                            }
                            else
                            {
                                memberValue = (bool)field.NullableHasValueMethod.Invoke(memberValue, null) ? field.NullableValueMethod.Invoke(memberValue, null) : null;
                            }
                            values[index++] = memberValue;
                        }
                    }
                }
#else
                if (defaultWriter != null) defaultWriter(value, values, ref index);
#endif
                else if (custom != null) custom.ToArray(value, values, ref index);
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly sqlModel.toArrayField[] fields;
#else
            /// <summary>
            /// 数据列转换数组
            /// </summary>
            /// <param name="values">目标数组</param>
            /// <param name="value">数据列</param>
            /// <param name="index">当前读取位置</param>
            private delegate void Writer(valueType value, object[] values, ref int index);
            /// <summary>
            /// 数据列转换数组
            /// </summary>
            private static readonly Writer defaultWriter;
#endif

            static ToArray()
            {
                if (attribute != null && custom == null && Fields != null)
                {
#if NOJIT
                    fields = new sqlModel.toArrayField[Fields.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    ColumnGroup.ToArray dynamicMethod = new ColumnGroup.ToArray(typeof(valueType), attribute);
                    foreach (Field member in Fields) dynamicMethod.Push(member);
                    defaultWriter = (Writer)dynamicMethod.Create<Writer>();
#endif
                }
            }
        }
    }
}

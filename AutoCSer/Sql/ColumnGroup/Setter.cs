using System;
using System.Data.Common;
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
    /// 数据列设置动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Setter
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
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        public Setter(Type type)
        {
            dynamicMethod = new DynamicMethod("SqlColumnSetter", null, new Type[] { typeof(DbDataReader), type.MakeByRefType(), Field.RefIntType }, type, true);
            generator = dynamicMethod.GetILGenerator();
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(Field field)
        {
            if (field.DataReaderDelegate == null)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldflda, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Call, AutoCSer.Sql.Metadata.GenericType.Get(field.DataType).SetMethod);
            }
            else
            {
                if (field.DataType == field.NullableDataType && (field.DataType.IsValueType || !field.DataMember.IsNull))
                {
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.call(field.DataReaderDelegate.Method);
                    //if (field.IsUnknownJson)
                    generator.Emit(OpCodes.Call, field.ToModelCastMethod);
                    generator.Emit(OpCodes.Stfld, field.FieldInfo);
                }
                else
                {
                    Label notNull = generator.DefineLabel(), end = generator.DefineLabel();
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.Emit(OpCodes.Callvirt, DataReader.IsDBNullMethod);
                    generator.Emit(OpCodes.Brfalse_S, notNull);

                    generator.Emit(OpCodes.Ldarg_1);
                    if (field.DataType == field.NullableDataType)
                    {
                        generator.Emit(OpCodes.Ldnull);
                        generator.Emit(OpCodes.Stfld, field.FieldInfo);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldflda, field.FieldInfo);
                        generator.Emit(OpCodes.Initobj, field.FieldInfo.FieldType);
                    }
                    generator.Emit(OpCodes.Br_S, end);

                    generator.MarkLabel(notNull);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.call(field.DataReaderDelegate.Method);
                    if (field.DataType == field.NullableDataType)
                    {
                        if (field.ToModelCastMethod != null) generator.Emit(OpCodes.Call, field.ToModelCastMethod);
                    }
                    else generator.call(AutoCSer.Emit.NullableConstructor.Constructors[field.DataType]);
                    generator.Emit(OpCodes.Stfld, field.FieldInfo);
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
        /// 数据列设置
        /// </summary>
        internal static class Setter
        {
            /// <summary>
            /// 设置字段值
            /// </summary>
            /// <param name="reader">字段读取器物理存储</param>
            /// <param name="value">目标数据</param>
            /// <param name="index">当前读取位置</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Set(DbDataReader reader, ref valueType value, ref int index)
            {
#if NOJIT
                if (fields != null)
                {
                    object[] sqlColumnParameters = null, castParameters = null;
                    object objectValue = value;
                    foreach (sqlModel.setField field in fields)
                    {
                        AutoCSer.code.cSharp.sqlModel.fieldInfo fieldInfo = field.Field;
                        if (fieldInfo.DataReaderMethod == null)
                        {
                            if (sqlColumnParameters == null) sqlColumnParameters = new object[] { reader, null, null };
                            sqlColumnParameters[1] = null;
                            sqlColumnParameters[2] = index;
                            field.SqlColumnMethod.Invoke(null, sqlColumnParameters);
                            fieldInfo.Field.SetValue(objectValue, sqlColumnParameters[1]);
                            index = (int)sqlColumnParameters[2];
                        }
                        else
                        {
                            object memberValue;
                            if (fieldInfo.DataType == fieldInfo.NullableDataType && (fieldInfo.DataType.IsValueType || !fieldInfo.DataMember.IsNull))
                            {
                                memberValue = reader[index++];
                                if (fieldInfo.ToModelCastMethod != null)
                                {
                                    if (castParameters == null) castParameters = new object[1];
                                    castParameters[0] = memberValue;
                                    memberValue = fieldInfo.ToModelCastMethod.Invoke(null, castParameters);
                                }
                            }
                            else if (reader.IsDBNull(index))
                            {
                                memberValue = null;
                                ++index;
                            }
                            else
                            {
                                memberValue = reader[index++];
                                if (fieldInfo.ToModelCastMethod != null && fieldInfo.DataType == fieldInfo.NullableDataType)
                                {
                                    if (castParameters == null) castParameters = new object[1];
                                    castParameters[0] = memberValue;
                                    memberValue = fieldInfo.ToModelCastMethod.Invoke(null, castParameters);
                                }
                            }
                            fieldInfo.Field.SetValue(objectValue, memberValue);
                        }
                    }
                    value = (valueType)objectValue;
                }
                else if (custom != null)
                {
                    object objectValue = value;
                    custom.Set(reader, objectValue, ref index);
                    value = (valueType)objectValue;
                }
#else
                if (defaultSetter != null) defaultSetter(reader, ref value, ref index);
                else if (custom != null) custom.Set(reader, ref value, ref index);
#endif
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            [AutoCSer.IOS.Preserve(Conditional = true)]
            private static readonly sqlModel.setField[] fields;
#else
            /// <summary>
            /// 设置字段值
            /// </summary>
            /// <param name="reader">字段读取器物理存储</param>
            /// <param name="value">目标数据</param>
            /// <param name="index">当前读取位置</param>
            private delegate void SetValue(DbDataReader reader, ref valueType value, ref int index);
            /// <summary>
            /// 默认数据列设置
            /// </summary>
            [AutoCSer.IOS.Preserve(Conditional = true)]
            private static readonly SetValue defaultSetter;
#endif
            static Setter()
            {
                if (attribute != null && custom == null && Fields != null)
                {
#if NOJIT
                    fields = new sqlModel.setField[Fields.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    ColumnGroup.Setter dynamicMethod = new ColumnGroup.Setter(typeof(valueType));
                    foreach (Field member in Fields) dynamicMethod.Push(member);
                    defaultSetter = (SetValue)dynamicMethod.Create<SetValue>();
#endif
                }
            }
        }
    }
}

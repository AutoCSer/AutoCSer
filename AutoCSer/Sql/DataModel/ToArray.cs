using System;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.DataModel
{
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
        /// 数据库表格模型配置
        /// </summary>
        private readonly ModelAttribute attribute;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        public ToArray(Type type, ModelAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlModelToArray", null, new Type[] { type, typeof(object[]), Field.RefIntType, typeof(Table) }, type, true);
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
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Call, ColumnGroup.ToArray.GetTypeToArray(field.DataType));
            }
            else
            {
                if (field.DataType == field.NullableDataType)
                {
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    if (field.IsNowTime) generator.Emit(OpCodes.Ldarg_3);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                    if (field.IsNowTime)
                    {
                        generator.int32(field.MemberMapIndex);
                        generator.call(AutoCSer.Extension.EmitGenerator_Sql.TableGetNowTimeMethod);
                    }
                    if (field.ToSqlCastMethod != null) generator.Emit(OpCodes.Call, field.ToSqlCastMethod);
                    if (!field.IsUnknownJson && field.DataType.IsValueType) generator.Emit(OpCodes.Box, field.DataType);
                    else if (attribute.IsNullStringEmpty && field.DataType == typeof(string)) generator.nullStringEmpty();
                    generator.Emit(OpCodes.Stelem_Ref);
                }
                else
                {
                    Label end = generator.DefineLabel();
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldflda, field.FieldInfo);
                    generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableHasValue(field.NullableDataType));
                    generator.Emit(OpCodes.Brtrue_S, end);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_2);
                    generator.Emit(OpCodes.Ldind_I4);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldflda, field.FieldInfo);
                    generator.Emit(OpCodes.Call, AutoCSer.Emit.Pub.GetNullableValue(field.NullableDataType));
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
    /// <summary>
    /// 数据模型
    /// </summary>
    internal abstract partial class Model<modelType>
    {
        /// <summary>
        /// 数据列转换数组
        /// </summary>
        internal static class ToArray
        {
            /// <summary>
            /// 导入数据列集合
            /// </summary>
            private static KeyValue<string, Type>[] dataColumns;
            /// <summary>
            /// 导入数据列集合
            /// </summary>
            internal static KeyValue<string, Type>[] DataColumns
            {
                get
                {
                    if (dataColumns == null)
                    {
                        LeftArray<KeyValue<string, Type>> columns = new LeftArray<KeyValue<string, Type>>(Fields.Length);
                        foreach (Field field in Fields)
                        {
                            if (field.IsSqlColumn) columns.Add(ColumnGroup.ToArray.GetDataColumns(field.DataType)(field.FieldInfo.Name));
                            else columns.Add(new KeyValue<string, Type>(field.FieldInfo.Name, field.DataType));
                        }
                        dataColumns = columns.ToArray();
                    }
                    return dataColumns;
                }
            }
            /// <summary>
            /// 数据列转换数组
            /// </summary>
            /// <param name="values">目标数组</param>
            /// <param name="value">数据列</param>
            /// <param name="index">当前读取位置</param>
            /// <param name="table">数据表格</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Write(modelType value, object[] values, ref int index, Table table)
            {
#if NOJIT
                if (fields != null)
                {
                    object[] sqlColumnParameters = null, castParameters = null;
                    foreach (sqlModel.toArrayField field in fields)
                    {
                        AutoCSer.code.cSharp.sqlModel.fieldInfo fieldInfo = field.Field;
                        if (fieldInfo.IsSqlColumn)
                        {
                            if (sqlColumnParameters == null) sqlColumnParameters = new object[] { null, values, null };
                            sqlColumnParameters[0] = fieldInfo.Field.GetValue(value);
                            sqlColumnParameters[2] = index;
                            field.SqlColumnMethod.Invoke(null, sqlColumnParameters);
                            index = (int)sqlColumnParameters[2];
                        }
                        else
                        {
                //if (field.NowTimeField == null)
                            object memberValue = fieldInfo.Field.GetValue(value);
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
                if (defaultWriter != null) defaultWriter(value, values, ref index, table);
#endif
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
            /// <param name="table">数据表格</param>
            private delegate void Writer(modelType value, object[] values, ref int index, Table table);
            /// <summary>
            /// 数据列转换数组
            /// </summary>
            private static readonly Writer defaultWriter;
#endif

            static ToArray()
            {
                if (attribute != null)
                {
#if NOJIT
                    fields = new sqlModel.toArrayField[Fields.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    DataModel.ToArray dynamicMethod = new DataModel.ToArray(typeof(modelType), attribute);
                    foreach (Field member in Fields) dynamicMethod.Push(member);
                    defaultWriter = (Writer)dynamicMethod.Create<Writer>();
#endif
                }
            }
        }
    }
}

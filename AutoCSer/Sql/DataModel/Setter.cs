using System;
using System.Data.Common;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.DataModel
{
    /// <summary>
    /// 数据库模型设置动态函数
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
        /// 
        /// </summary>
        private readonly LocalBuilder indexMember;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        public Setter(Type type)
        {
            dynamicMethod = new DynamicMethod("SqlModelSetter", null, new Type[] { typeof(DbDataReader), type, typeof(MemberMap) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            indexMember = generator.DeclareLocal(typeof(int));
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Stloc_0);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(Field field)
        {
            Label notMember = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_2, field.MemberMapIndex);
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Brfalse_S, notMember);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldflda, field.FieldInfo);
                generator.Emit(OpCodes.Ldloca_S, indexMember);
                generator.Emit(OpCodes.Call, ColumnGroup.Setter.GetTypeSetter(field.DataType));
            }
            else
            {
                if (field.DataType == field.NullableDataType && (field.DataType.IsValueType || !field.DataMember.IsNull))
                {
                    generator.Emit(OpCodes.Brfalse_S, notMember);
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.Emit(OpCodes.Callvirt, field.DataReaderMethod);
                    if (field.ToModelCastMethod != null) generator.Emit(OpCodes.Call, field.ToModelCastMethod);
                    generator.Emit(OpCodes.Stfld, field.FieldInfo);
                }
                else
                {
                    generator.Emit(OpCodes.Brfalse, notMember);
                    Label notNull = generator.DefineLabel(), end = generator.DefineLabel();
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldloc_0);
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
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.Emit(OpCodes.Callvirt, field.DataReaderMethod);
                    if (field.DataType == field.NullableDataType)
                    {
                        if (field.ToModelCastMethod != null) generator.Emit(OpCodes.Call, field.ToModelCastMethod);
                    }
                    else generator.Emit(OpCodes.Newobj, AutoCSer.Emit.NullableConstructor.Constructors[field.DataType]);
                    generator.Emit(OpCodes.Stfld, field.FieldInfo);
                    generator.MarkLabel(end);
                }
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ldc_I4_1);
                generator.Emit(OpCodes.Add);
                generator.Emit(OpCodes.Stloc_0);
            }
            generator.MarkLabel(notMember);
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
        /// 数据库模型设置
        /// </summary>
        internal static class Setter
        {
            /// <summary>
            /// 设置字段值
            /// </summary>
            /// <param name="reader">字段读取器物理存储</param>
            /// <param name="value">目标数据</param>
            /// <param name="memberMap">成员位图</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Set(DbDataReader reader, modelType value, MemberMap memberMap)
            {
#if NOJIT
                if (fields != null)
                {
                    int index = 0;
                    object[] sqlColumnParameters = null, castParameters = null;
                    foreach (sqlModel.setField field in fields)
                    {
                        AutoCSer.code.cSharp.sqlModel.fieldInfo fieldInfo = field.Field;
                        if (memberMap.IsMember(fieldInfo.MemberMapIndex))
                        {
                            if (fieldInfo.DataReaderMethod == null)
                            {
                                if (sqlColumnParameters == null) sqlColumnParameters = new object[] { reader, null, null };
                                sqlColumnParameters[1] = null;
                                sqlColumnParameters[2] = index;
                                field.SqlColumnMethod.Invoke(null, sqlColumnParameters);
                                fieldInfo.Field.SetValue(value, sqlColumnParameters[1]);
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
                                fieldInfo.Field.SetValue(value, memberValue);
                            }
                        }
                    }
                }
#else
                if (setter != null) setter(reader, value, memberMap);
#endif
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly sqlModel.setField[] fields;
#else
            /// <summary>
            /// 默认数据列设置
            /// </summary>
            private static readonly Action<DbDataReader, modelType, MemberMap> setter;
#endif

            static Setter()
            {
                if (attribute != null)
                {
#if NOJIT
                    fields = new sqlModel.setField[Fields.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    DataModel.Setter dynamicMethod = new DataModel.Setter(typeof(modelType));
                    foreach (Field member in Fields) dynamicMethod.Push(member);
                    setter = (Action<DbDataReader, modelType, MemberMap>)dynamicMethod.Create<Action<DbDataReader, modelType, MemberMap>>();
#endif
                }
            }
        }
    }
}

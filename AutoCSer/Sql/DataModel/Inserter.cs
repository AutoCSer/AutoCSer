using System;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.DataModel
{
    /// <summary>
    /// 添加数据动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Inserter
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
        /// <param name="modelType"></param>
        /// <param name="attribute"></param>
        public Inserter(Type modelType, ModelAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlModelInserter", null, new Type[] { typeof(CharStream), typeof(MemberMap), modelType, typeof(ConstantConverter), typeof(Table) }, modelType, true);
            generator = dynamicMethod.GetILGenerator();
            generator.DeclareLocal(typeof(int));
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Stloc_0);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(Field field)
        {
            Label end = generator.DefineLabel(), isNext = generator.DefineLabel(), insert = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_1, field.MemberMapIndex);
            generator.Emit(OpCodes.Brfalse_S, end);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Brtrue_S, isNext);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Stloc_0);
            generator.Emit(OpCodes.Br_S, insert);
            generator.MarkLabel(isNext);
            generator.charStreamWriteChar(OpCodes.Ldarg_0, ',');
            generator.MarkLabel(insert);
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_3);
                generator.Emit(OpCodes.Call, ColumnGroup.Inserter.GetTypeInsert(field.DataType));
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_3);
                generator.Emit(OpCodes.Ldarg_0);
                if (field.IsNowTime) generator.Emit(OpCodes.Ldarg_S, 4);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                if (field.IsNowTime)
                {
                    generator.int32(field.MemberMapIndex);
                    generator.call(AutoCSer.Extension.EmitGenerator_Sql.TableGetNowTimeMethod);
                }
                if (field.ToSqlCastMethod != null) generator.Emit(OpCodes.Call, field.ToSqlCastMethod);
                if (attribute.IsNullStringEmpty && field.DataType == typeof(string)) generator.nullStringEmpty();
                generator.Emit(OpCodes.Callvirt, field.ToSqlMethod);
            }
            generator.MarkLabel(end);
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
        /// 添加数据
        /// </summary>
        internal static class Inserter
        {
            /// <summary>
            /// 获取逗号分割的列名集合
            /// </summary>
            /// <param name="sqlStream"></param>
            /// <param name="memberMap"></param>
            /// <param name="constantConverter"></param>
            public static void GetColumnNames(CharStream sqlStream, MemberMap memberMap, ConstantConverter constantConverter)
            {
                int isNext = 0;
                foreach (Field member in Fields)
                {
                    if (memberMap.IsMember(member.MemberMapIndex) || member == Identity || member.DataMember.PrimaryKeyIndex != 0)
                    {
                        if (isNext == 0) isNext = 1;
                        else sqlStream.Write(',');
                        if (member.IsSqlColumn) sqlStream.SimpleWriteNotNull(ColumnGroup.Inserter.GetColumnNames(member.FieldInfo.FieldType)(member.FieldInfo.Name));
                        else constantConverter.ConvertNameToSqlStream(sqlStream, member.FieldInfo.Name);
                    }
                }
            }
            /// <summary>
            /// 获取插入数据SQL表达式
            /// </summary>
            /// <param name="sqlStream">SQL表达式流</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">数据</param>
            /// <param name="converter">SQL常量转换</param>
            /// <param name="table">数据表格</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Insert(CharStream sqlStream, MemberMap memberMap, modelType value, ConstantConverter converter, Table table)
            {
#if NOJIT
                if (fields != null)
                {
                    object[] sqlColumnParameters = null, castParameters = null, parameters = null;
                    byte isNext = 0;
                    foreach (InsertField field in fields)
                    {
                        if (memberMap.IsMember(field.Field.MemberMapIndex))
                        {
                            if (isNext == 0) isNext = 1;
                            else sqlStream.Write(',');
                            Field fieldInfo = field.Field;
                            if (fieldInfo.IsSqlColumn)
                            {
                                if (sqlColumnParameters == null) sqlColumnParameters = new object[] { sqlStream, null, converter };
                                sqlColumnParameters[1] = fieldInfo.FieldInfo.GetValue(value);
                                field.SqlColumnMethod.Invoke(null, sqlColumnParameters);
                            }
                            else if (fieldInfo.NowTimeField == null)
                            {
                                object memberValue = fieldInfo.FieldInfo.GetValue(value);
                                if (fieldInfo.ToSqlCastMethod != null)
                                {
                                    if (castParameters == null) castParameters = new object[1];
                                    castParameters[0] = memberValue;
                                    memberValue = fieldInfo.ToSqlCastMethod.Invoke(null, castParameters);
                                }
                                if (parameters == null) parameters = new object[] { sqlStream, null };
                                parameters[1] = memberValue;
                                fieldInfo.ToSqlMethod.Invoke(converter, parameters);
                            }
                            else
                            {
                                if (parameters == null) parameters = new object[] { sqlStream, null };
                                parameters[1] = fieldInfo.NowTimeField.GetValue(null);
                                fieldInfo.ToSqlMethod.Invoke(converter, parameters);
                            }
                        }
                    }
                }
#else
                if (inserter != null) inserter(sqlStream, memberMap, value, converter, table);
#endif
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly InsertField[] fields;
#else
            /// <summary>
            /// 获取插入数据SQL表达式
            /// </summary>
            private static readonly Action<CharStream, MemberMap, modelType, ConstantConverter, Table> inserter;
#endif
            static Inserter()
            {
                if (attribute != null)
                {
#if NOJIT
                    fields = new sqlModel.insertField[Fields.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    DataModel.Inserter dynamicMethod = new DataModel.Inserter(typeof(modelType), attribute);
                    foreach (Field member in Fields) dynamicMethod.Push(member);
                    inserter = (Action<CharStream, MemberMap, modelType, ConstantConverter, Table>)dynamicMethod.Create<Action<CharStream, MemberMap, modelType, ConstantConverter, Table>>();
#endif
                }
            }
        }
    }
}

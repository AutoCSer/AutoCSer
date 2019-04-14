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
    /// 更新数据动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct Updater
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
        public Updater(Type type, ModelAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlModelUpdater", null, new Type[] { typeof(CharStream), typeof(MemberMap), type, typeof(ConstantConverter), typeof(Table) }, type, true);
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
            Label end = generator.DefineLabel(), isNext = generator.DefineLabel(), update = generator.DefineLabel();
            generator.memberMapIsMember(OpCodes.Ldarg_1, field.MemberMapIndex);
            generator.Emit(OpCodes.Brfalse_S, end);
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Brtrue_S, isNext);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Stloc_0);
            generator.Emit(OpCodes.Br_S, update);
            generator.MarkLabel(isNext);
            generator.charStreamWriteChar(OpCodes.Ldarg_0, ',');
            generator.MarkLabel(update);
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_3);
                generator.Emit(OpCodes.Ldstr, field.FieldInfo.Name);
                generator.Emit(OpCodes.Call, ColumnGroup.Updater.GetTypeUpdate(field.DataType));
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_3);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldstr, field.FieldInfo.Name);
                generator.call(AutoCSer.Extension.EmitGenerator_Sql.ConstantConverterConvertNameToSqlStreamMethod);
                generator.charStreamWriteChar(OpCodes.Ldarg_0, '=');
                //generator.charStreamSimpleWriteNotNull(OpCodes.Ldarg_0, AutoCSer.Emit.Pub.GetNameAssignmentPool(field.SqlFieldName), field.SqlFieldName.Length + 1);
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
        /// 数据列更新SQL流
        /// </summary>
        internal static class Updater
        {
            /// <summary>
            /// 获取更新数据SQL表达式
            /// </summary>
            /// <param name="sqlStream">SQL表达式流</param>
            /// <param name="memberMap">更新成员位图</param>
            /// <param name="value">数据</param>
            /// <param name="converter">SQL常量转换</param>
            /// <param name="table">数据表格</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Update(CharStream sqlStream, MemberMap memberMap, modelType value, ConstantConverter converter, Table table)
            {
#if NOJIT
                if (fields != null)
                {
                    byte isNext = 0;
                    object[] sqlColumnParameters = null, castParameters = null, parameters = null;
                    foreach (sqlModel.updateField field in fields)
                    {
                        if (memberMap.IsMember(field.Field.MemberMapIndex))
                        {
                            if (isNext == 0) isNext = 1;
                            else sqlStream.Write(',');
                //if (field.NowTimeField == null)
                            field.Set(sqlStream, value, converter, ref sqlColumnParameters, ref castParameters, ref parameters);
                        }
                    }
                }
#else
                if (updater != null) updater(sqlStream, memberMap, value, converter, table);
#endif
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly sqlModel.updateField[] fields;
#else
            /// <summary>
            /// 获取更新数据SQL表达式
            /// </summary>
            private static readonly Action<CharStream, MemberMap, modelType, ConstantConverter, Table> updater;
#endif

            static Updater()
            {
                if (attribute != null)
                {
#if NOJIT
                    fields = new sqlModel.updateField[Fields.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    DataModel.Updater dynamicMethod = new DataModel.Updater(typeof(modelType), attribute);
                    foreach (Field member in Fields) dynamicMethod.Push(member);
                    updater = (Action<CharStream, MemberMap, modelType, ConstantConverter, Table>)dynamicMethod.Create<Action<CharStream, MemberMap, modelType, ConstantConverter, Table>>();
#endif
                }
            }
        }
    }
}

using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.DataModel
{
    /// <summary>
    /// 关键字条件动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct PrimaryKeyWhere
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
        /// 
        /// </summary>
        private bool isNextMember;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        public PrimaryKeyWhere(Type type, ModelAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlModelPrimaryKeyWhere", null, new Type[] { typeof(CharStream), type, typeof(ConstantConverter) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isNextMember = false;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public unsafe void Push(Field field)
        {
            if (isNextMember) generator.charStreamSimpleWriteNotNull(OpCodes.Ldarg_0, AndString.Char, 5);
            else isNextMember = true;
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldstr, field.FieldInfo.Name);
                generator.Emit(OpCodes.Call, ColumnGroup.Updater.GetTypeUpdate(field.DataType));
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldstr, field.FieldInfo.Name);
                generator.call(AutoCSer.Extension.EmitGenerator_Sql.ConstantConverterConvertNameToSqlStreamMethod);
                generator.charStreamWriteChar(OpCodes.Ldarg_0, '=');
                //generator.charStreamSimpleWriteNotNull(OpCodes.Ldarg_0, AutoCSer.Emit.Pub.GetNameAssignmentPool(field.SqlFieldName), field.SqlFieldName.Length + 1);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                if (field.ToSqlCastMethod != null) generator.Emit(OpCodes.Call, field.ToSqlCastMethod);
                if (attribute.IsNullStringEmpty && field.DataType == typeof(string)) generator.nullStringEmpty();
                generator.Emit(OpCodes.Callvirt, field.ToSqlMethod);
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
        /// <summary>
        /// 添加连接字符串
        /// </summary>
        internal static Pointer AndString = new Pointer { Data = AutoCSer.Emit.NamePool.Get(" and ", 0, 0) };
    }
    /// <summary>
    /// 数据模型
    /// </summary>
    internal abstract partial class Model<modelType>
    {
        /// <summary>
        /// 关键字条件
        /// </summary>
        internal static class PrimaryKeyWhere
        {
            /// <summary>
            /// 关键字条件SQL流
            /// </summary>
            /// <param name="sqlStream">SQL表达式流</param>
            /// <param name="value">数据列</param>
            /// <param name="converter">SQL常量转换</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Write(CharStream sqlStream, modelType value, ConstantConverter converter)
            {
#if NOJIT
                if (fields != null)
                {
                    byte isAnd = 0;
                    object[] sqlColumnParameters = null, castParameters = null, parameters = null;
                    foreach (sqlModel.updateField field in fields)
                    {
                        if (isAnd == 0) isAnd = 1;
                        else sqlStream.WriteNotNull(" and ");
                        field.Set(sqlStream, value, converter, ref sqlColumnParameters, ref castParameters, ref parameters);
                    }
                }
#else
                if (writer != null) writer(sqlStream, value, converter);
#endif
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly sqlModel.updateField[] fields;
#else
            /// <summary>
            /// 关键字条件SQL流
            /// </summary>
            private static readonly Action<CharStream, modelType, ConstantConverter> writer;
#endif

            static unsafe PrimaryKeyWhere()
            {
                if (attribute != null && PrimaryKeys.Length != 0)
                {
#if NOJIT
                    fields = new sqlModel.updateField[PrimaryKeys.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in PrimaryKeys) fields[index++].Set(member);
#else
                    DataModel.PrimaryKeyWhere dynamicMethod = new DataModel.PrimaryKeyWhere(typeof(modelType), attribute);
                    foreach (Field member in PrimaryKeys) dynamicMethod.Push(member);
                    writer = (Action<CharStream, modelType, ConstantConverter>)dynamicMethod.Create<Action<CharStream, modelType, ConstantConverter>>();
#endif
                }
            }
        }
    }
}

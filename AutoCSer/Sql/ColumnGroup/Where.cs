using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.ColumnGroup
{
    /// <summary>
    /// 关键字条件动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct Where
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
        /// 更新数据动态函数
        /// </summary>
        private readonly Updater updateDynamicMethod;
        /// <summary>
        /// 
        /// </summary>
        private bool isNextMember;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        public Where(Type type, ColumnAttribute attribute)
        {
            dynamicMethod = new DynamicMethod("SqlColumnWhere", null, new Type[] { typeof(CharStream), type, typeof(ConstantConverter), typeof(string[]) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isNextMember = false;
            updateDynamicMethod = new Updater(dynamicMethod, generator, attribute);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="index">字段名称序号</param>
        public unsafe void Push(Field field, int index)
        {
            if (isNextMember) generator.charStreamSimpleWriteNotNull(OpCodes.Ldarg_0, DataModel.PrimaryKeyWhere.AndString.Char, 5);
            else isNextMember = true;
            updateDynamicMethod.PushOnly(field, index);
        }
        /// <summary>
        /// 创建 SQL 条件委托
        /// </summary>
        /// <returns>SQL 条件委托</returns>
        public Delegate Create<delegateType>()
        {
            generator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(delegateType));
        }
    }
    
    /// <summary>
    /// 数据列
    /// </summary>
    internal static partial class Column<valueType>
    {
        /// <summary>
        /// 条件
        /// </summary>
        internal static class Where
        {
            /// <summary>
            /// 条件SQL流
            /// </summary>
            /// <param name="sqlStream">SQL表达式流</param>
            /// <param name="value">数据列</param>
            /// <param name="converter">SQL常量转换</param>
            /// <param name="columnName">列名前缀</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Write(CharStream sqlStream, valueType value, ConstantConverter converter, string columnName)
            {
#if NOJIT
                if (fields != null)
                {
                    string[] columnNames = update.GetColumnNames(columnName);
                    object[] sqlColumnParameters = null, castParameters = null, parameters = null;
                    object objectValue = value;
                    int index = 0;
                    foreach (sqlModel.updateField field in fields)
                    {
                        if (index != 0) sqlStream.WriteNotNull(" and ");
                        field.Set(sqlStream, objectValue, converter, columnNames[index++], ref sqlColumnParameters, ref castParameters, ref parameters);
                    }
                }
#else
                if (writer != null) writer(sqlStream, value, converter, Updater.GetColumnNames(columnName));
#endif
                else if (custom != null) custom.Where(sqlStream, value, converter, columnName);
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly sqlModel.updateField[] fields;
#else
            /// <summary>
            /// 条件SQL流
            /// </summary>
            private static readonly Action<CharStream, valueType, ConstantConverter, string[]> writer;
#endif

            static Where()
            {
                if (attribute != null && custom == null && Fields != null)
                {
                    int index = 0;
#if NOJIT
                    fields = new sqlModel.updateField[Fields.Length];
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    ColumnGroup.Where dynamicMethod = new ColumnGroup.Where(typeof(valueType), attribute);
                    foreach (Field member in Fields) dynamicMethod.Push(member, index++);
                    writer = (Action<CharStream, valueType, ConstantConverter, string[]>)dynamicMethod.Create<Action<CharStream, valueType, ConstantConverter, string[]>>();
#endif
                }
            }
        }
    }
}

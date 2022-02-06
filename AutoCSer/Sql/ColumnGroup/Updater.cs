using System;
using AutoCSer.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.ColumnGroup
{
    /// <summary>
    /// 更新数据动态函数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct Updater
    {
#if !NOJIT
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
        /// 
        /// </summary>
        private bool isNextMember;
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        public Updater(Type type, ColumnAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlColumnUpdater", null, new Type[] { typeof(CharStream), type, typeof(ConstantConverter), typeof(string[]) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isNextMember = false;
        }
        /// <summary>
        /// 动态函数
        /// </summary>
        /// <param name="dynamicMethod"></param>
        /// <param name="generator"></param>
        /// <param name="attribute"></param>
        public Updater(DynamicMethod dynamicMethod, ILGenerator generator, ColumnAttribute attribute)
        {
            this.attribute = attribute;
            this.dynamicMethod = dynamicMethod;
            this.generator = generator;
            isNextMember = false;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="index">字段名称序号</param>
        public void Push(Field field, int index)
        {
            if (isNextMember) generator.charStreamWriteChar(OpCodes.Ldarg_0, ',');
            else isNextMember = true;
            PushOnly(field, index);
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        /// <param name="index">字段名称序号</param>
        public void PushOnly(Field field, int index)
        {
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarga_S, 1);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldarg_3);
                generator.int32(index);
                generator.Emit(OpCodes.Ldelem_Ref);
                generator.Emit(OpCodes.Call, AutoCSer.Sql.Metadata.GenericType.Get(field.DataType).UpdateMethod);
            }
            else
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_3);
                generator.int32(index);
                generator.Emit(OpCodes.Ldelem_Ref);
                generator.charStreamSimpleWrite();
                generator.charStreamWriteChar(OpCodes.Ldarg_0, '=');

                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarga_S, 1);
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
#endif
        /// <summary>
        /// 获取列名集合
        /// </summary>
        /// <param name="names">列名集合</param>
        /// <param name="name">列名前缀</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void GetColumnNames<valueType>(ref LeftArray<string> names, string name)
        {
            names.Add(Column<valueType>.Updater.GetColumnNames(name));
        }
    }
    /// <summary>
    /// 数据列
    /// </summary>
    internal static partial class Column<valueType>
    {
        /// <summary>
        /// 数据列更新SQL流
        /// </summary>
        internal static class Updater
        {
            /// <summary>
            /// 数据列名集合
            /// </summary>
            private static readonly AutoCSer.Threading.LockDictionary<HashString, string[]> columnNames;
            /// <summary>
            /// 获取列名集合
            /// </summary>
            /// <param name="name">列名前缀</param>
            /// <returns></returns>
            public unsafe static string[] GetColumnNames(string name)
            {
                string[] names;
                HashString nameKey = name;
                if (columnNames.TryGetValue(nameKey, out names)) return names;
                LeftArray<string> nameList = new LeftArray<string>(Fields.Length);
                foreach (Field field in Fields)
                {
                    if (field.IsSqlColumn) ((ColumnGroup.Verifyer.GetName)AutoCSer.Sql.Metadata.GenericType.Get(field.FieldInfo.FieldType).UpdaterGetColumnNamesMethod)(ref nameList, name + "_" + field.FieldInfo.Name);
                    else nameList.Add(name + "_" + field.FieldInfo.Name);
                }
                columnNames.Set(nameKey, names = nameList.ToArray());
                return names;
            }

            /// <summary>
            /// 获取更新数据SQL表达式
            /// </summary>
            /// <param name="sqlStream">SQL表达式流</param>
            /// <param name="value">数据列</param>
            /// <param name="converter">SQL常量转换</param>
            /// <param name="columnName">列名前缀</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Update(CharStream sqlStream, valueType value, ConstantConverter converter, string columnName)
            {
#if NOJIT
                if (fields != null)
                {
                    string[] columnNames = GetColumnNames(columnName);
                    object[] sqlColumnParameters = null, castParameters = null, parameters = null;
                    object objectValue = value;
                    int index = 0;
                    foreach (sqlModel.updateField field in fields)
                    {
                        if (index != 0) sqlStream.Write(',');
                        field.Set(sqlStream, objectValue, converter, columnNames[index++], ref sqlColumnParameters, ref castParameters, ref parameters);
                    }
                }
#else
                if (updater != null) updater(sqlStream, value, converter, GetColumnNames(columnName));
#endif
                else if (custom != null) custom.Update(sqlStream, value, converter, columnName);
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
            private static readonly Action<CharStream, valueType, ConstantConverter, string[]> updater;
#endif

            static Updater()
            {
                if (attribute != null && custom == null && Fields != null)
                {
                    columnNames = new AutoCSer.Threading.LockDictionary<HashString, string[]>();
                    int index = 0;
#if NOJIT
                    fields = new sqlModel.updateField[Fields.Length];
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    ColumnGroup.Updater dynamicMethod = new ColumnGroup.Updater(typeof(valueType), attribute);
                    foreach (Field member in Fields) dynamicMethod.Push(member, index++);
                    updater = (Action<CharStream, valueType, ConstantConverter, string[]>)dynamicMethod.Create<Action<CharStream, valueType, ConstantConverter, string[]>>();
#endif
                    AutoCSer.Memory.Common.AddClearCache(columnNames.Clear, typeof(Updater), 60 * 60);
                }
            }
        }
    }
}

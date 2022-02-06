using System;
using System.Reflection;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Memory;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Sql.ColumnGroup
{
#if !NOJIT
    /// <summary>
    /// 数据列添加SQL流动态函数
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
        public Inserter(Type type, ColumnAttribute attribute)
        {
            this.attribute = attribute;
            dynamicMethod = new DynamicMethod("SqlColumnInserter", null, new Type[] { typeof(CharStream), type, typeof(ConstantConverter) }, type, true);
            generator = dynamicMethod.GetILGenerator();
            isNextMember = false;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field">字段信息</param>
        public void Push(Field field)
        {
            if (isNextMember) generator.charStreamWriteChar(OpCodes.Ldarg_0, ',');
            else isNextMember = true;
            if (field.IsSqlColumn)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarga_S, 1);
                generator.Emit(OpCodes.Ldfld, field.FieldInfo);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Call, AutoCSer.Sql.Metadata.GenericType.Get(field.DataType).InsertMethod);
            }
            else
            {
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
    }
#endif
    /// <summary>
    /// 数据列
    /// </summary>
    internal static partial class Column<valueType>
    {
        /// <summary>
        /// 数据列添加SQL流
        /// </summary>
        internal static class Inserter
        {
            /// <summary>
            /// 数据列名集合
            /// </summary>
            private static readonly AutoCSer.Threading.LockDictionary<HashString, string> columnNames;
            /// <summary>
            /// 获取列名集合
            /// </summary>
            /// <param name="name">列名前缀</param>
            /// <returns></returns>
            public unsafe static string GetColumnNames(string name)
            {
                if (custom != null) return custom.GetColumnNames(name);
                if (columnNames != null)
                {
                    string names;
                    HashString nameKey = name;
                    if (columnNames.TryGetValue(nameKey, out names)) return names;
                    int isNext = 0;
                    AutoCSer.Memory.Pointer buffer = UnmanagedPool.Default.GetPointer();
                    try
                    {
                        using (CharStream sqlStream = new CharStream(ref buffer))
                        {
                            foreach (Field field in Fields)
                            {
                                if (field.IsSqlColumn)
                                {
                                    if ((names = ((Func<string, string>)AutoCSer.Sql.Metadata.GenericType.Get(field.FieldInfo.FieldType).InserterGetColumnNamesMethod)(name + "_" + field.FieldInfo.Name)) != null)
                                    {
                                        if (isNext == 0) isNext = 1;
                                        else sqlStream.Write(',');
                                        sqlStream.Write(names);
                                    }
                                }
                                else
                                {
                                    if (isNext == 0) isNext = 1;
                                    else sqlStream.Write(',');
                                    sqlStream.PrepCharSize(name.Length + field.FieldInfo.Name.Length + 1);
                                    sqlStream.SimpleWrite(name);
                                    sqlStream.Write('_');
                                    sqlStream.SimpleWrite(field.FieldInfo.Name);
                                }
                            }
                            names = sqlStream.Length == 0 ? null : sqlStream.ToString();
                            columnNames.Set(nameKey, names);
                        }
                    }
                    finally { UnmanagedPool.Default.PushOnly(ref buffer); }
                    return names;
                }
                return null;
            }

            /// <summary>
            /// 获取插入数据SQL表达式
            /// </summary>
            /// <param name="sqlStream">SQL表达式流</param>
            /// <param name="value">数据列</param>
            /// <param name="converter">SQL常量转换</param>
#if !NOJIT
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
#endif
            public static void Insert(CharStream sqlStream, valueType value, ConstantConverter converter)
            {
#if NOJIT
                if (fields != null)
                {
                    object[] sqlColumnParameters = null, castParameters = null, parameters = null;
                    object objectValue = value;
                    byte isNext = 0;
                    foreach (sqlModel.insertField field in fields)
                    {
                        if (isNext == 0) isNext = 1;
                        else sqlStream.Write(',');
                        AutoCSer.code.cSharp.sqlModel.fieldInfo fieldInfo = field.Field;
                        if (fieldInfo.IsSqlColumn)
                        {
                            if (sqlColumnParameters == null) sqlColumnParameters = new object[] { sqlStream, null, converter };
                            sqlColumnParameters[1] = fieldInfo.Field.GetValue(objectValue);
                            field.SqlColumnMethod.Invoke(null, sqlColumnParameters);
                        }
                        else
                        {
                            object memberValue = fieldInfo.Field.GetValue(objectValue);
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
                    }
                }
#else
                if (inserter != null) inserter(sqlStream, value, converter);
#endif
                else if (custom != null) custom.Insert(sqlStream, value, converter);
            }
#if NOJIT
            /// <summary>
            /// 字段集合
            /// </summary>
            private static readonly sqlModel.insertField[] fields;
#else
            /// <summary>
            /// 获取插入数据SQL表达式
            /// </summary>
            private static readonly Action<CharStream, valueType, ConstantConverter> inserter;
#endif

            static Inserter()
            {
                if (attribute != null && custom == null && Fields != null)
                {
                    columnNames = new AutoCSer.Threading.LockDictionary<HashString, string>();
#if NOJIT
                    fields = new sqlModel.insertField[Fields.Length];
                    int index = 0;
                    foreach (AutoCSer.code.cSharp.sqlModel.fieldInfo member in Fields) fields[index++].Set(member);
#else
                    ColumnGroup.Inserter dynamicMethod = new ColumnGroup.Inserter(typeof(valueType), attribute);
                    foreach (Field member in Fields) dynamicMethod.Push(member);
                    inserter = (Action<CharStream, valueType, ConstantConverter>)dynamicMethod.Create<Action<CharStream, valueType, ConstantConverter>>();
#endif
                    AutoCSer.Memory.Common.AddClearCache(columnNames.Clear, typeof(Inserter), 60 * 60);
                }
            }
        }
    }
}

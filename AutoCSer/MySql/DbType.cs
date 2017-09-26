using System;
using System.Data;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.MySql
{
    /// <summary>
    /// SQL数据类型相关操作
    /// </summary>
    internal static class DbType
    {
        /// <summary>
        /// 数据类型集合
        /// </summary>
        private static readonly string[] sqlTypeNames;
        /// <summary>
        /// 获取数据类型名称
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>数据类型名称</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string getSqlTypeName(this SqlDbType type)
        {
            return sqlTypeNames.get((int)type, null);
        }
        /// <summary>
        /// 默认值集合
        /// </summary>
        private static readonly string[] defaultValues;
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>默认值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string getDefaultValue(this SqlDbType type)
        {
            return defaultValues.get((int)type, null);
        }
        /// <summary>
        /// 数据类型集合唯一哈希
        /// </summary>
        private static readonly UniqueDictionary<TypeName, KeyValue<SqlDbType, int>> sqlTypes;
        /// <summary>
        /// 格式化数据类型
        /// </summary>
        /// <param name="typeString">数据类型字符串</param>
        /// <param name="size">列长</param>
        /// <returns>数据类型</returns>
        internal unsafe static SqlDbType FormatDbType(string typeString, out int size)
        {
            if (string.IsNullOrEmpty(typeString)) throw new ArgumentNullException();
            fixed (char* typeFixed = typeString)
            {
                char* end = typeFixed + typeString.Length, typeEnd = *(end - 1) == ')' ? AutoCSer.Extension.StringExtension.FindNotNull(typeFixed, end, '(') : end;
                TypeName typeName = new TypeName { Name = typeString, Length = (int)(typeEnd - typeFixed) };
                KeyValue<SqlDbType, int> value = new KeyValue<SqlDbType, int>((SqlDbType)(-1), int.MinValue);
                sqlTypes.Get(typeName, ref value);
                if (value.Value == int.MinValue)
                {
                    size = 0;
                    if (typeEnd != end)
                    {
                        for (--end; ++typeEnd != end; size += *typeEnd - '0') size *= 10;
                    }
                }
                else size = value.Value;
                return value.Key;
            }
        }

        unsafe static DbType()
        {
            #region 数据类型集合
            sqlTypeNames = new string[Sql.DbType.MaxEnumValue];
            sqlTypeNames[(int)SqlDbType.BigInt] = "BIGINT";
            //SqlTypeNames[(int)SqlDbType.Binary] = typeof(byte[]);
            sqlTypeNames[(int)SqlDbType.Bit] = "BIT";
            sqlTypeNames[(int)SqlDbType.Char] = "CHAR";
            sqlTypeNames[(int)SqlDbType.DateTime] = "DATETIME";
            sqlTypeNames[(int)SqlDbType.Decimal] = "DECIMAL";
            sqlTypeNames[(int)SqlDbType.Float] = "DOUBLE";
            //SqlTypeNames[(int)SqlDbType.Image] = typeof(byte[]);
            sqlTypeNames[(int)SqlDbType.Int] = "INT";
            sqlTypeNames[(int)SqlDbType.Money] = "DECIMAL";
            sqlTypeNames[(int)SqlDbType.NChar] = "CHAR";
            sqlTypeNames[(int)SqlDbType.NText] = "TEXT";
            sqlTypeNames[(int)SqlDbType.NVarChar] = "VARCHAR";
            sqlTypeNames[(int)SqlDbType.Real] = "FLOAT";
            //SqlTypeNames[(int)SqlDbType.UniqueIdentifier] = typeof(Guid);
            sqlTypeNames[(int)SqlDbType.SmallDateTime] = "DATETIME";
            sqlTypeNames[(int)SqlDbType.SmallInt] = "SMALLINT";
            sqlTypeNames[(int)SqlDbType.SmallMoney] = "DECIMAL";
            sqlTypeNames[(int)SqlDbType.Text] = "TEXT";
            //SqlTypeNames[(int)SqlDbType.Timestamp] = typeof(byte[]);
            sqlTypeNames[(int)SqlDbType.TinyInt] = "TINYINT UNSIGNED";
            //SqlTypeNames[(int)SqlDbType.VarBinary] = typeof(byte[]);
            sqlTypeNames[(int)SqlDbType.VarChar] = "VARCHAR";
            //SqlTypeNames[(int)SqlDbType.Variant] = typeof(object);
            #endregion

            #region 默认值集合
            defaultValues = new string[Sql.DbType.MaxEnumValue];
            defaultValues[(int)SqlDbType.BigInt] = "0";
            defaultValues[(int)SqlDbType.Bit] = "0";
            defaultValues[(int)SqlDbType.Char] = "''";
            defaultValues[(int)SqlDbType.DateTime] = "now()";
            defaultValues[(int)SqlDbType.Decimal] = "0";
            defaultValues[(int)SqlDbType.Float] = "0";
            defaultValues[(int)SqlDbType.Int] = "0";
            defaultValues[(int)SqlDbType.Money] = "0";
            defaultValues[(int)SqlDbType.NChar] = "''";
            defaultValues[(int)SqlDbType.NText] = "''";
            defaultValues[(int)SqlDbType.NVarChar] = "''";
            defaultValues[(int)SqlDbType.Real] = "0";
            defaultValues[(int)SqlDbType.SmallDateTime] = "now()";
            defaultValues[(int)SqlDbType.SmallInt] = "0";
            defaultValues[(int)SqlDbType.SmallMoney] = "0";
            defaultValues[(int)SqlDbType.Text] = "''";
            defaultValues[(int)SqlDbType.TinyInt] = "0";
            defaultValues[(int)SqlDbType.VarChar] = "''";
            #endregion

            #region 数据类型集合唯一哈希
            KeyValue<TypeName, KeyValue<SqlDbType, int>>[] names = new KeyValue<TypeName, KeyValue<SqlDbType, int>>[12];
            names[0].Set((TypeName)"bigint", new KeyValue<SqlDbType, int>(SqlDbType.BigInt, sizeof(long)));
            names[1].Set((TypeName)"bit", new KeyValue<SqlDbType, int>(SqlDbType.Bit, sizeof(bool)));
            names[2].Set((TypeName)"char", new KeyValue<SqlDbType, int>(SqlDbType.Char, sizeof(char)));
            names[3].Set((TypeName)"datetime", new KeyValue<SqlDbType, int>(SqlDbType.DateTime, sizeof(DateTime)));
            names[4].Set((TypeName)"decimal", new KeyValue<SqlDbType, int>(SqlDbType.Decimal, sizeof(decimal)));
            names[5].Set((TypeName)"double", new KeyValue<SqlDbType, int>(SqlDbType.Float, sizeof(double)));
            names[6].Set((TypeName)"int", new KeyValue<SqlDbType, int>(SqlDbType.Int, sizeof(int)));
            names[7].Set((TypeName)"text", new KeyValue<SqlDbType, int>(SqlDbType.Text, int.MinValue));
            names[8].Set((TypeName)"varchar", new KeyValue<SqlDbType, int>(SqlDbType.VarChar, int.MinValue));
            names[9].Set((TypeName)"float", new KeyValue<SqlDbType, int>(SqlDbType.Real, sizeof(float)));
            names[10].Set((TypeName)"smallint", new KeyValue<SqlDbType, int>(SqlDbType.SmallInt, sizeof(short)));
            names[11].Set((TypeName)"tinyint", new KeyValue<SqlDbType, int>(SqlDbType.TinyInt, sizeof(byte)));
            sqlTypes = new UniqueDictionary<TypeName, KeyValue<SqlDbType, int>>(names, 16);
            #endregion
        }
    }
}

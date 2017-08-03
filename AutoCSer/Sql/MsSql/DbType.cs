using System;
using System.Data;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.MsSql
{
    /// <summary>
    /// SQL数据类型相关操作
    /// </summary>
    internal static class DbType
    {
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
        internal static string getDefaultValue(this SqlDbType type)
        {
            return defaultValues.get((int)type, null);
        }
        /// <summary>
        /// SqlServer2000 syscolumns.xtype值与SqlDbType值映射关系集合
        /// </summary>
        private static readonly SqlDbType[] sqlTypeArray;
        /// <summary>
        /// 根据SqlServer syscolumns.xtype值与SqlDbType值
        /// </summary>
        /// <param name="dataType">SqlServer syscolumns.xtype值</param>
        /// <returns>SqlDbType值</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static SqlDbType GetType(short dataType)
        {
            return sqlTypeArray.get(dataType, (SqlDbType)(-1));
        }

        static DbType()
        {
            #region 默认值集合
            defaultValues = new string[Sql.DbType.MaxEnumValue];
            defaultValues[(int)SqlDbType.BigInt] = "0";
            defaultValues[(int)SqlDbType.Bit] = "0";
            defaultValues[(int)SqlDbType.Char] = "''";
            defaultValues[(int)SqlDbType.DateTime] = "getdate()";
            defaultValues[(int)SqlDbType.Decimal] = "0";
            defaultValues[(int)SqlDbType.Float] = "0";
            defaultValues[(int)SqlDbType.Int] = "0";
            defaultValues[(int)SqlDbType.Money] = "0";
            defaultValues[(int)SqlDbType.NChar] = "''";
            defaultValues[(int)SqlDbType.NText] = "''";
            defaultValues[(int)SqlDbType.NVarChar] = "''";
            defaultValues[(int)SqlDbType.Real] = "0";
            defaultValues[(int)SqlDbType.SmallDateTime] = "getdate()";
            defaultValues[(int)SqlDbType.SmallInt] = "0";
            defaultValues[(int)SqlDbType.SmallMoney] = "0";
            defaultValues[(int)SqlDbType.Text] = "''";
            defaultValues[(int)SqlDbType.TinyInt] = "0";
            defaultValues[(int)SqlDbType.VarChar] = "''";
            #endregion

            #region SqlServer syscolumns.xusertype值与sqlDbType值映射关系集合
            sqlTypeArray = new SqlDbType[256];
            sqlTypeArray[34] = SqlDbType.Image;
            sqlTypeArray[35] = SqlDbType.Text;
            sqlTypeArray[36] = SqlDbType.UniqueIdentifier;
            sqlTypeArray[40] = SqlDbType.Date;
            sqlTypeArray[41] = SqlDbType.Time;
            sqlTypeArray[42] = SqlDbType.DateTime2;
            sqlTypeArray[43] = SqlDbType.DateTimeOffset;
            sqlTypeArray[48] = SqlDbType.TinyInt;
            sqlTypeArray[52] = SqlDbType.SmallInt;
            sqlTypeArray[56] = SqlDbType.Int;
            sqlTypeArray[58] = SqlDbType.SmallDateTime;
            sqlTypeArray[59] = SqlDbType.Real;
            sqlTypeArray[60] = SqlDbType.Money;
            sqlTypeArray[61] = SqlDbType.DateTime;
            sqlTypeArray[62] = SqlDbType.Float;
            //SqlType[98] = SqlDbType.sql_variant;
            sqlTypeArray[99] = SqlDbType.NText;
            sqlTypeArray[104] = SqlDbType.Bit;
            sqlTypeArray[106] = SqlDbType.Decimal;
            //SqlType[108] = SqlDbType.numeric;
            sqlTypeArray[122] = SqlDbType.SmallMoney;
            sqlTypeArray[127] = SqlDbType.BigInt;
            //SqlType[128] = SqlDbType.hierarchyid;
            //SqlType[129] = SqlDbType.geometry;
            //SqlType[130] = SqlDbType.geography;
            sqlTypeArray[165] = SqlDbType.VarBinary;
            sqlTypeArray[167] = SqlDbType.VarChar;
            sqlTypeArray[173] = SqlDbType.Binary;
            sqlTypeArray[175] = SqlDbType.Char;
            sqlTypeArray[189] = SqlDbType.Timestamp;
            sqlTypeArray[231] = SqlDbType.NVarChar;
            sqlTypeArray[239] = SqlDbType.NChar;
            sqlTypeArray[241] = SqlDbType.Xml;
            //SqlType[256] = SqlDbType.sysname;
            #endregion
        }
    }
}

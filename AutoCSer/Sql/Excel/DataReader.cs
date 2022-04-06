using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql.Excel
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DataReader
    {
        /// <summary>
        /// decimal 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static decimal setDecimalSize(decimal value, int size)
        {
            return decimal.Round(value, size, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// decimal 四舍五入
        /// </summary>
        internal static readonly Func<decimal, int, decimal> SetDecimalSize = setDecimalSize;

        /// <summary>
        /// 基本类型设置函数
        /// </summary>
        private static readonly Dictionary<Type, Delegate> dataReaderGetDelegates;
        /// <summary>
        /// 获取基本类型设置函数
        /// </summary>
        /// <param name="type">基本类型</param>
        /// <returns>设置函数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static Delegate GetDelegate(Type type)
        {
            Delegate method;
            return dataReaderGetDelegates.TryGetValue(type, out method) ? method : null;
        }

        /// <summary>
        /// 获取字符串（兼容 Excel）
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string getString(DbDataReader reader, int index)
        {
            object value = reader[index];
            if (value == null) return null;
            return value.ToString();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool getBoolean(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(bool);
            object value = reader[index];
            if (value.GetType() == typeof(string)) return bool.Parse((string)value);
            return (bool)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static byte getByte(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(byte);
            object value = reader[index];
            Type type = value.GetType();
            if (type == typeof(string)) return byte.Parse((string)value);
            if (type == typeof(double)) return (byte)(double)value;
            return (byte)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static char getChar(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(char);
            object value = reader[index];
            if (value.GetType() == typeof(string)) return ((string)value)[0];
            return (char)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static DateTime getDateTime(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(DateTime);
            object value = reader[index];
            if (value.GetType() == typeof(string)) return DateTime.Parse((string)value);
            return (DateTime)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static decimal getDecimal(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(decimal);
            object value = reader[index];
            Type type = value.GetType();
            if (type == typeof(string)) return decimal.Parse((string)value);
            if (type == typeof(double)) return (decimal)(double)value;
            return (decimal)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static double getDouble(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(double);
            object value = reader[index];
            Type type = value.GetType();
            if (type == typeof(string)) return double.Parse((string)value);
            return (double)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static float getFloat(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(float);
            object value = reader[index];
            Type type = value.GetType();
            if (type == typeof(string)) return float.Parse((string)value);
            if (type == typeof(double)) return (float)(double)value;
            return (float)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Guid getGuid(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(Guid);
            object value = reader[index];
            if (value.GetType() == typeof(string)) return Guid.Parse((string)value);
            return (Guid)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static short getInt16(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(short);
            object value = reader[index];
            Type type = value.GetType();
            if (type == typeof(string)) return short.Parse((string)value);
            if (type == typeof(double)) return (short)(double)value;
            return (short)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static int getInt32(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(int);
            object value = reader[index];
            Type type = value.GetType();
            if (type == typeof(string)) return int.Parse((string)value);
            if (type == typeof(double)) return (int)(double)value;
            return (int)value;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static long getInt64(DbDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return default(long);
            object value = reader[index];
            Type type = value.GetType();
            if (type == typeof(string)) return long.Parse((string)value);
            if (type == typeof(double)) return (long)(double)value;
            return (long)value;
        }
        static DataReader()
        {
            dataReaderGetDelegates = DictionaryCreator.CreateOnly<Type, Delegate>();
            Type[] intType = new Type[] { typeof(int) };
            dataReaderGetDelegates.Add(typeof(string), (Func<DbDataReader, int, string>)getString);
            dataReaderGetDelegates.Add(typeof(int), (Func<DbDataReader, int, int>)getInt32);
            dataReaderGetDelegates.Add(typeof(byte), (Func<DbDataReader, int, byte>)getByte);
            dataReaderGetDelegates.Add(typeof(bool), (Func<DbDataReader, int, bool>)getBoolean);
            dataReaderGetDelegates.Add(typeof(DateTime), (Func<DbDataReader, int, DateTime>)getDateTime);
            dataReaderGetDelegates.Add(typeof(decimal), (Func<DbDataReader, int, decimal>)getDecimal);
            dataReaderGetDelegates.Add(typeof(long), (Func<DbDataReader, int, long>)getInt64);
            dataReaderGetDelegates.Add(typeof(double), (Func<DbDataReader, int, double>)getDouble);
            dataReaderGetDelegates.Add(typeof(float), (Func<DbDataReader, int, float>)getFloat);
            dataReaderGetDelegates.Add(typeof(short), (Func<DbDataReader, int, short>)getInt16);
            dataReaderGetDelegates.Add(typeof(Guid), (Func<DbDataReader, int, Guid>)getGuid);
            dataReaderGetDelegates.Add(typeof(char), (Func<DbDataReader, int, char>)getChar);
        }
    }
}

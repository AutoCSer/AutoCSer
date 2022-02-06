using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DataReader
    {
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
#if NOJIT
        /// <summary>
        /// 获取基本类型设置函数信息
        /// </summary>
        internal static readonly MethodInfo GetMethodInfo;
#else
        /// <summary>
        /// 判断数据是否为空
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static bool isDBNull(DbDataReader reader, int index)
        {
            return reader.IsDBNull(index);
        }
        /// <summary>
        /// 判断数据是否为空
        /// </summary>
        internal static readonly MethodInfo IsDBNullMethod = ((Func<DbDataReader, int, bool>)isDBNull).Method;
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
            return reader.GetBoolean(index);
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
            return reader.GetByte(index);
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
            return reader.GetChar(index);
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
            return reader.GetDateTime(index);
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
            return reader.GetDecimal(index);
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
            return reader.GetDouble(index);
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
            return reader.GetFloat(index);
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
            return reader.GetGuid(index);
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
            return reader.GetInt16(index);
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
            return reader.GetInt32(index);
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
            return reader.GetInt64(index);
        }
#endif
        static DataReader()
        {
            dataReaderGetDelegates = DictionaryCreator.CreateOnly<Type, Delegate>();
#if NOJIT
            GetMethodInfo = typeof(pub).GetMethod("GetDataReaderMethod", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(Type) }, null);
            dataReaderGetMethods.Add(typeof(bool), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(byte), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(char), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(DateTime), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(decimal), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(double), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(float), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(Guid), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(short), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(int), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(long), GetMethodInfo);
            dataReaderGetMethods.Add(typeof(string), GetMethodInfo);
#else
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
#endif
        }
    }
}

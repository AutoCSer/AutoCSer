using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 常量转换
    /// </summary>
    public class ConstantConverter
    {
        /// <summary>
        /// 常量转换处理集合
        /// </summary>
        protected readonly Dictionary<Type, Action<CharStream, object>> converters;
        /// <summary>
        /// 获取常量转换处理函数
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>失败返回null</returns>
        public Action<CharStream, object> this[Type type]
        {
            get
            {
                Action<CharStream, object> value;
                return converters.TryGetValue(type, out value) ? value : null;
            }
        }
        /// <summary>
        /// 常量转换
        /// </summary>
        protected ConstantConverter()
        {
            converters = DictionaryCreator.CreateOnly<Type, Action<CharStream, object>>();
            converters.Add(typeof(bool), convertConstantBoolTo01);
            converters.Add(typeof(bool?), convertConstantBoolNullable);
            converters.Add(typeof(byte), convertConstantByte);
            converters.Add(typeof(byte?), convertConstantByteNullable);
            converters.Add(typeof(sbyte), convertConstantSByte);
            converters.Add(typeof(sbyte?), convertConstantSByteNullable);
            converters.Add(typeof(short), convertConstantShort);
            converters.Add(typeof(short?), convertConstantShortNullable);
            converters.Add(typeof(ushort), convertConstantUShort);
            converters.Add(typeof(ushort?), convertConstantUShortNullable);
            converters.Add(typeof(int), convertConstantInt);
            converters.Add(typeof(int?), convertConstantIntNullable);
            converters.Add(typeof(uint), convertConstantUInt);
            converters.Add(typeof(uint?), convertConstantUIntNullable);
            converters.Add(typeof(long), convertConstantLong);
            converters.Add(typeof(long?), convertConstantLongNullable);
            converters.Add(typeof(ulong), convertConstantULong);
            converters.Add(typeof(ulong?), convertConstantULongNullable);
            converters.Add(typeof(float), convertConstantFloat);
            converters.Add(typeof(float?), convertConstantFloatNullable);
            converters.Add(typeof(double), convertConstantDouble);
            converters.Add(typeof(double?), convertConstantDoubleNullable);
            converters.Add(typeof(decimal), convertConstantDecimal);
            converters.Add(typeof(decimal?), convertConstantDecimalNullable);
            converters.Add(typeof(DateTime), convertConstantDateTimeMillisecond);
            converters.Add(typeof(DateTime?), convertConstantDateTimeMillisecondNullable);
            converters.Add(typeof(string), convertConstantString);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstantToString<valueType>(CharStream sqlStream, valueType value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else convertString(sqlStream, value.ToString());
        }
        /// <summary>
        /// 常量转换字符串函数信息
        /// </summary>
        private static readonly MethodInfo convertConstantToStringMethod = typeof(ConstantConverter).GetMethod("convertConstantToString", BindingFlags.Instance | BindingFlags.NonPublic);
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, bool value)
        {
            sqlStream.Write(value ? '1' : '0');
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantBoolTo01(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (bool)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, bool? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else sqlStream.Write((bool)value ? '1' : '0');
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantBoolNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (bool?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, byte value)
        {
            AutoCSer.Extension.Number.ToString(value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantByte(CharStream sqlStream, object value)
        {
            AutoCSer.Extension.Number.ToString((byte)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, byte? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else AutoCSer.Extension.Number.ToString((byte)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantByteNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (byte?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, sbyte value)
        {
            AutoCSer.Extension.Number.ToString(value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantSByte(CharStream sqlStream, object value)
        {
            AutoCSer.Extension.Number.ToString((sbyte)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, sbyte? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else AutoCSer.Extension.Number.ToString((sbyte)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantSByteNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (sbyte?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, short value)
        {
            AutoCSer.Extension.Number.ToString(value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantShort(CharStream sqlStream, object value)
        {
            AutoCSer.Extension.Number.ToString((short)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, short? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else AutoCSer.Extension.Number.ToString((short)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantShortNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (short?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, ushort value)
        {
            AutoCSer.Extension.Number.ToString(value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantUShort(CharStream sqlStream, object value)
        {
            AutoCSer.Extension.Number.ToString((ushort)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, ushort? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else AutoCSer.Extension.Number.ToString((ushort)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantUShortNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (ushort?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, int value)
        {
            AutoCSer.Extension.Number.ToString(value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantInt(CharStream sqlStream, object value)
        {
            AutoCSer.Extension.Number.ToString((int)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, int? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else AutoCSer.Extension.Number.ToString((int)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantIntNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (int?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, uint value)
        {
            AutoCSer.Extension.Number.ToString(value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantUInt(CharStream sqlStream, object value)
        {
            AutoCSer.Extension.Number.ToString((uint)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, uint? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else AutoCSer.Extension.Number.ToString((uint)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantUIntNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (uint?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, long value)
        {
            AutoCSer.Extension.Number.ToString(value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantLong(CharStream sqlStream, object value)
        {
            AutoCSer.Extension.Number.ToString((long)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, long? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else AutoCSer.Extension.Number.ToString((long)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantLongNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (long?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, ulong value)
        {
            AutoCSer.Extension.Number.ToString(value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantULong(CharStream sqlStream, object value)
        {
            AutoCSer.Extension.Number.ToString((ulong)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, ulong? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else AutoCSer.Extension.Number.ToString((ulong)value, sqlStream);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantULongNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (ulong?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, float value)
        {
            sqlStream.SimpleWriteNotNull(value.ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantFloat(CharStream sqlStream, object value)
        {
            sqlStream.SimpleWriteNotNull(((float)value).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, float? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else sqlStream.SimpleWriteNotNull(((float)value).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantFloatNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (float?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, double value)
        {
            sqlStream.SimpleWriteNotNull(value.ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantDouble(CharStream sqlStream, object value)
        {
            sqlStream.SimpleWriteNotNull(((double)value).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, double? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else sqlStream.SimpleWriteNotNull(((double)value).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantDoubleNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (double?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, decimal value)
        {
            sqlStream.SimpleWriteNotNull(value.ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantDecimal(CharStream sqlStream, object value)
        {
            sqlStream.SimpleWriteNotNull(((decimal)value).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, decimal? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else sqlStream.SimpleWriteNotNull(((decimal)value).ToString());
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantDecimalNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (decimal?)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        protected virtual void convertConstant(CharStream sqlStream, DateTime value)
        {
            sqlStream.PrepLength(AutoCSer.Date.MillisecondStringSize + 2);
            sqlStream.UnsafeWrite('\'');
            AutoCSer.Date.ToMillisecondString((DateTime)value, sqlStream);
            sqlStream.UnsafeWrite('\'');
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantDateTimeMillisecond(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (DateTime)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, DateTime? value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else convertConstant(sqlStream, (DateTime)value);
        }
        /// <summary>
        /// 常量转换字符串
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantDateTimeMillisecondNullable(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (DateTime?)value);
        }
        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void convertConstant(CharStream sqlStream, string value)
        {
            if (value == null) sqlStream.WriteJsonNull();
            else convertString(sqlStream, value);
        }
        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="sqlStream"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Convert(CharStream sqlStream, string value)
        {
            convertConstant(sqlStream, value);
        }
        /// <summary>
        /// 常量转换字符串(单引号变两个)
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        private void convertConstantString(CharStream sqlStream, object value)
        {
            convertConstant(sqlStream, (string)value);
        }
        ///// <summary>
        ///// SQL语句字符串格式化(单引号变两个)
        ///// </summary>
        ///// <param name="sqlStream">SQL字符流</param>
        ///// <param name="value">常量</param>
        //protected virtual unsafe void convertString(CharStream sqlStream, string value)
        //{
        //    fixed (char* valueFixed = value)
        //    {
        //        int length = 0;
        //        for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
        //        {
        //            if (*start == '\'') ++length;
        //            else if (*start == '\\')
        //            {
        //                if ((*(start + 1) == '\r' || *(start + 1) == '\n') && (int)(end - start) >= 2)
        //                {
        //                    length += 2;
        //                    ++start;
        //                }
        //            }
        //        }
        //        if (length == 0)
        //        {
        //            sqlStream.PrepLength(value.Length + 2);
        //            sqlStream.UnsafeWrite('\'');
        //            sqlStream.WriteNotNull(value);
        //            sqlStream.UnsafeWrite('\'');
        //            return;
        //        }
        //        sqlStream.PrepLength((length += value.Length) + 2);
        //        sqlStream.UnsafeWrite('\'');
        //        byte* write = (byte*)sqlStream.CurrentChar;
        //        for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
        //        {
        //            if (*start != '\'')
        //            {
        //                if (*start == '\\')
        //                {
        //                    if (*(start + 1) == '\n')
        //                    {
        //                        if ((int)(end - start) >= 2)
        //                        {
        //                            *(long*)write = '\\' + ('\\' << 16) + ((long)'\n' << 32) + ((long)'\n' << 48);
        //                            ++start;
        //                            write += sizeof(long);
        //                            continue;
        //                        }
        //                    }
        //                    else if (*(start + 1) == '\r' && (int)(end - start) >= 2)
        //                    {
        //                        *(long*)write = '\\' + ('\\' << 16) + ((long)'\n' << 32) + ((long)'\r' << 48);
        //                        ++start;
        //                        write += sizeof(long);
        //                        continue;
        //                    }
        //                }
        //                *(char*)write = *start;
        //                write += sizeof(char);
        //            }
        //            else
        //            {
        //                *(int*)write = ('\'' << 16) + '\'';
        //                write += sizeof(int);
        //            }
        //        }
        //        sqlStream.ByteSize += length * sizeof(char);
        //        sqlStream.UnsafeWrite('\'');
        //    }
        //}
        /// <summary>
        /// SQL语句字符串格式化(单引号变两个)
        /// </summary>
        /// <param name="sqlStream">SQL字符流</param>
        /// <param name="value">常量</param>
        protected virtual unsafe void convertString(CharStream sqlStream, string value)
        {
            fixed (char* valueFixed = value)
            {
                int length = 0;
                for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                {
                    if (*start == '\'') ++length;
                }
                if (length == 0)
                {
                    sqlStream.PrepLength(value.Length + 2);
                    sqlStream.UnsafeWrite('\'');
                    sqlStream.WriteNotNull(value);
                    sqlStream.UnsafeWrite('\'');
                    return;
                }
                sqlStream.PrepLength((length += value.Length) + 2);
                sqlStream.UnsafeWrite('\'');
                byte* write = (byte*)sqlStream.CurrentChar;
                for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                {
                    if (*start != '\'')
                    {
                        *(char*)write = *start;
                        write += sizeof(char);
                    }
                    else
                    {
                        *(int*)write = ('\'' << 16) + '\'';
                        write += sizeof(int);
                    }
                }
                sqlStream.ByteSize += length * sizeof(char);
                sqlStream.UnsafeWrite('\'');
            }
        }
        /// <summary>
        /// LIKE 字符串转义
        /// </summary>
        /// <param name="sqlStream"></param>
        /// <param name="value"></param>
        /// <param name="isStart"></param>
        /// <param name="isEnd"></param>
        internal void ConvertLike(CharStream sqlStream, object value, bool isStart, bool isEnd)
        {
            sqlStream.Write('\'');
            if (isStart) sqlStream.Write('%');
            if (value != null)
            {
                foreach (char code in value.ToString())
                {
                    switch (code)
                    {
                        case '[':
                        case '_':
                        case '%':
                            sqlStream.Write('[');
                            sqlStream.Write(code);
                            sqlStream.Write(']');
                            break;
                        default:
                            sqlStream.Write(code);
                            if (code == '\'') sqlStream.Write('\'');
                            break;
                    }
                }
            }
            if (isEnd) sqlStream.Write('%');
            sqlStream.Write('\'');
        }
        /// <summary>
        /// SQL 关键字搜索器
        /// </summary>
        private static AutoCSer.StateSearcher.AsciiSearcher keywordSearcher = new AutoCSer.StateSearcher.AsciiSearcher(AutoCSer.StateSearcher.AsciiBuilder.Create(new string[] { "add", "all", "alter", "and", "any", "as", "asc", "authorization", "backup", "begin", "between", "break", "browse", "bulk", "by", "cascade", "case", "check", "checkpoint", "close", "clustered", "coalesce", "collate", "column", "commit", "compute", "constraint", "contains", "containstable", "continue", "convert", "create", "cross", "current", "current_date", "current_time", "current_timestamp", "current_user", "cursor", "database", "dbcc", "deallocate", "declare", "default", "delete", "deny", "desc", "disk", "distinct", "distributed", "double", "drop", "dump", "else", "end", "errlvl", "escape", "except", "exec", "execute", "exists", "exit", "external", "fetch", "file", "fillfactor", "for", "foreign", "freetext", "freetexttable", "full", "function", "goto", "grant", "group", "having", "holdlock", "identity", "identity_insert", "identitycol", "if", "in", "index", "inner", "insert", "intersect", "into", "is", "join", "key", "kill", "left", "like", "lineno", "load", "merge", "national", "nocheck", "nonclustered", "not", "null", "nullif", "of", "off", "offsets", "on", "open", "opendatasource", "openquery", "openrowset", "openxml", "option", "or", "order", "outer", "over", "percent", "pivot", "plan", "precision", "primary", "print", "proc", "procedure", "public", "raiserror", "read", "readtext", "reconfigure", "references", "replication", "restore", "restrict", "return", "revert", "revoke", "right", "rollback", "rowcount", "rowguidcol", "rule", "save", "schema", "securityaudit", "select", "semantickeyphrasetable", "semanticsimilaritydetailstable", "semanticsimilaritytable", "session_user", "set", "setuser", "shutdown", "some", "statistics", "system_user", "table", "tablesample", "textsize", "then", "top", "tran", "transaction", "trigger", "truncate", "try_convert", "tsequal", "union", "unique", "unpivot", "update", "updatetext", "use", "user", "values", "varying", "view", "waitfor", "when", "where", "while", "with", "within", "writetext" }, true).Pointer);
        /// <summary>
        /// SQL名称关键字处理
        /// </summary>
        /// <param name="name"></param>
        /// <returns>SQL名称</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal virtual string ConvertName(string name)
        {
            return keywordSearcher.SearchLower(name) < 0 ? name : ("[" + name + "]");
        }
        /// <summary>
        /// SQL名称关键字处理
        /// </summary>
        /// <param name="sqlStream"></param>
        /// <param name="name"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal virtual void ConvertNameToSqlStream(CharStream sqlStream, string name)
        {
            if (keywordSearcher.SearchLower(name) < 0) sqlStream.SimpleWriteNotNull(name);
            else
            {
                sqlStream.Write('[');
                sqlStream.SimpleWriteNotNull(name);
                sqlStream.Write(']');
            }
        }
        /// <summary>
        /// 常量转换
        /// </summary>
        internal static readonly ConstantConverter Default = new ConstantConverter();

        /// <summary>
        /// SQL常量转换函数信息集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockDictionary<Type, MethodInfo> converterMethods = new AutoCSer.Threading.LockDictionary<Type, MethodInfo>();
        /// <summary>
        /// 获取SQL常量转换函数信息
        /// </summary>
        /// <param name="type">数值类型</param>
        /// <returns>SQL常量转换函数信息</returns>
        internal static MethodInfo GetMethod(Type type)
        {
            MethodInfo method;
            if (converterMethods.TryGetValue(type, out method)) return method;
            method = typeof(ConstantConverter).GetMethod("convertConstant", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(CharStream), type }, null)
                ?? convertConstantToStringMethod.MakeGenericMethod(type);
            converterMethods.Set(type, method);
            return method;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void clearCache(int count)
        {
            converterMethods.Clear();
        }
        static ConstantConverter()
        {
            AutoCSer.Pub.ClearCaches += clearCache;
        }
    }
}

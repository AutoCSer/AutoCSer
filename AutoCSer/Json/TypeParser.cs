using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型解析器
    /// </summary>
    /// <typeparam name="valueType">目标类型</typeparam>
    internal unsafe static partial class TypeParser<valueType>
    {
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal class EnumParser : Emit.EnumParser<valueType>
        {
            /// <summary>
            /// 枚举名称查找数据
            /// </summary>
            protected readonly static StateSearcher enumSearcher = new StateSearcher(enumSearchData);
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            protected static void parse(Parser parser, ref valueType value)
            {
                int index = enumSearcher.SearchString(parser);
                if (index != -1) value = enumValues[index];
                else if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                else if (parser.ParseState == ParseState.Success) parser.SearchStringEnd();
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            /// <param name="index">第一个枚举索引</param>
            /// <param name="nextIndex">第二个枚举索引</param>
            protected static void getIndex(Parser parser, ref valueType value, out int index, ref int nextIndex)
            {
                if ((index = enumSearcher.SearchFlagEnum(parser)) == -1)
                {
                    if (parser.Config.IsMatchEnum)
                    {
                        parser.ParseState = ParseState.NoFoundEnumValue;
                        return;
                    }
                    do
                    {
                        if (parser.ParseState != ParseState.Success || parser.Quote == 0) return;
                    }
                    while ((index = enumSearcher.NextFlagEnum(parser)) == -1);
                }
                do
                {
                    if (parser.Quote == 0)
                    {
                        value = enumValues[index];
                        return;
                    }
                    if ((nextIndex = enumSearcher.NextFlagEnum(parser)) != -1) break;
                    if (parser.ParseState != ParseState.Success) return;
                }
                while (true);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal sealed class EnumByte : EnumParser
        {
            /// <summary>
            /// 枚举值集合
            /// </summary>
            private static Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    byte intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, byte>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    byte intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, byte>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                        else parser.Ignore();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            byte intValue = enumInts.Byte[index];
                            intValue |= enumInts.Byte[nextIndex];
                            while (parser.Quote != 0)
                            {
                                index = enumSearcher.NextFlagEnum(parser);
                                if (parser.ParseState != ParseState.Success) return;
                                if (index != -1) intValue |= enumInts.Byte[index];
                            }
                            value = Emit.EnumCast<valueType, byte>.FromInt(intValue);
                        }
                    }
                }
            }
            static EnumByte()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic(enumValues.Length * sizeof(byte), false) };
                byte* data = enumInts.Byte;
                foreach (valueType value in enumValues) *(byte*)data++ = Emit.EnumCast<valueType, byte>.ToInt(value);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal sealed class EnumSByte : EnumParser
        {
            /// <summary>
            /// 枚举值集合
            /// </summary>
            private static Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    sbyte intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, sbyte>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    sbyte intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, sbyte>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                        else parser.Ignore();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            byte intValue = (byte)enumInts.SByte[index];
                            intValue |= (byte)enumInts.SByte[nextIndex];
                            while (parser.Quote != 0)
                            {
                                index = enumSearcher.NextFlagEnum(parser);
                                if (parser.ParseState != ParseState.Success) return;
                                if (index != -1) intValue |= (byte)enumInts.SByte[index];
                            }
                            value = Emit.EnumCast<valueType, sbyte>.FromInt((sbyte)intValue);
                        }
                    }
                }
            }
            static EnumSByte()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic(enumValues.Length * sizeof(sbyte), false) };
                sbyte* data = enumInts.SByte;
                foreach (valueType value in enumValues) *(sbyte*)data++ = Emit.EnumCast<valueType, sbyte>.ToInt(value);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal sealed class EnumShort : EnumParser
        {
            /// <summary>
            /// 枚举值集合
            /// </summary>
            private static Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    short intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, short>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    short intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, short>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                        else parser.Ignore();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            ushort intValue = (ushort)enumInts.Short[index];
                            intValue |= (ushort)enumInts.Short[nextIndex];
                            while (parser.Quote != 0)
                            {
                                index = enumSearcher.NextFlagEnum(parser);
                                if (parser.ParseState != ParseState.Success) return;
                                if (index != -1) intValue |= (ushort)enumInts.Short[index];
                            }
                            value = Emit.EnumCast<valueType, short>.FromInt((short)intValue);
                        }
                    }
                }
            }
            static EnumShort()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic(enumValues.Length * sizeof(short), false) };
                short* data = enumInts.Short;
                foreach (valueType value in enumValues) *(short*)data++ = Emit.EnumCast<valueType, short>.ToInt(value);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal sealed class EnumUShort : EnumParser
        {
            /// <summary>
            /// 枚举值集合
            /// </summary>
            private static Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    ushort intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, ushort>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    ushort intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, ushort>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                        else parser.Ignore();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            ushort intValue = enumInts.UShort[index];
                            intValue |= enumInts.UShort[nextIndex];
                            while (parser.Quote != 0)
                            {
                                index = enumSearcher.NextFlagEnum(parser);
                                if (parser.ParseState != ParseState.Success) return;
                                if (index != -1) intValue |= enumInts.UShort[index];
                            }
                            value = Emit.EnumCast<valueType, ushort>.FromInt(intValue);
                        }
                    }
                }
            }
            static EnumUShort()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic(enumValues.Length * sizeof(ushort), false) };
                ushort* data = enumInts.UShort;
                foreach (valueType value in enumValues) *(ushort*)data++ = Emit.EnumCast<valueType, ushort>.ToInt(value);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal sealed class EnumInt : EnumParser
        {
            /// <summary>
            /// 枚举值集合
            /// </summary>
            private static Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    int intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, int>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    int intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, int>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                        else parser.Ignore();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            int intValue = enumInts.Int[index];
                            intValue |= enumInts.Int[nextIndex];
                            while (parser.Quote != 0)
                            {
                                index = enumSearcher.NextFlagEnum(parser);
                                if (parser.ParseState != ParseState.Success) return;
                                if (index != -1) intValue |= enumInts.Int[index];
                            }
                            value = Emit.EnumCast<valueType, int>.FromInt(intValue);
                        }
                    }
                }
            }
            static EnumInt()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic(enumValues.Length * sizeof(int), false) };
                int* data = enumInts.Int;
                foreach (valueType value in enumValues) *(int*)data++ = Emit.EnumCast<valueType, int>.ToInt(value);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal sealed class EnumUInt : EnumParser
        {
            /// <summary>
            /// 枚举值集合
            /// </summary>
            private static Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    uint intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, uint>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    uint intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, uint>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                        else parser.Ignore();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            uint intValue = enumInts.UInt[index];
                            intValue |= enumInts.UInt[nextIndex];
                            while (parser.Quote != 0)
                            {
                                index = enumSearcher.NextFlagEnum(parser);
                                if (parser.ParseState != ParseState.Success) return;
                                if (index != -1) intValue |= enumInts.UInt[index];
                            }
                            value = Emit.EnumCast<valueType, uint>.FromInt(intValue);
                        }
                    }
                }
            }
            static EnumUInt()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic(enumValues.Length * sizeof(uint), false) };
                uint* data = enumInts.UInt;
                foreach (valueType value in enumValues) *(uint*)data++ = Emit.EnumCast<valueType, uint>.ToInt(value);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal sealed class EnumLong : EnumParser
        {
            /// <summary>
            /// 枚举值集合
            /// </summary>
            private static Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    long intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, long>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    long intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, long>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                        else parser.Ignore();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            long intValue = enumInts.Long[index];
                            intValue |= enumInts.Long[nextIndex];
                            while (parser.Quote != 0)
                            {
                                index = enumSearcher.NextFlagEnum(parser);
                                if (parser.ParseState != ParseState.Success) return;
                                if (index != -1) intValue |= enumInts.Long[index];
                            }
                            value = Emit.EnumCast<valueType, long>.FromInt(intValue);
                        }
                    }
                }
            }
            static EnumLong()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic64(enumValues.Length * sizeof(long), false) };
                long* data = enumInts.Long;
                foreach (valueType value in enumValues) *(long*)data++ = Emit.EnumCast<valueType, long>.ToInt(value);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        internal sealed class EnumULong : EnumParser
        {
            /// <summary>
            /// 枚举值集合
            /// </summary>
            private static Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    ulong intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, ulong>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    ulong intValue = 0;
                    parser.Parse(ref intValue);
                    value = Emit.EnumCast<valueType, ulong>.FromInt(intValue);
                }
                else if (parser.ParseState == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.ParseState = ParseState.NoFoundEnumValue;
                        else parser.Ignore();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            ulong intValue = enumInts.ULong[index];
                            intValue |= enumInts.ULong[nextIndex];
                            while (parser.Quote != 0)
                            {
                                index = enumSearcher.NextFlagEnum(parser);
                                if (parser.ParseState != ParseState.Success) return;
                                if (index != -1) intValue |= enumInts.ULong[index];
                            }
                            value = Emit.EnumCast<valueType, ulong>.FromInt(intValue);
                        }
                    }
                }
            }
            static EnumULong()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic64(enumValues.Length * sizeof(ulong), false) };
                ulong* data = enumInts.ULong;
                foreach (valueType value in enumValues) *(ulong*)data++ = Emit.EnumCast<valueType, ulong>.ToInt(value);
            }
        }
        /// <summary>
        /// 成员解析器过滤
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct TryParseFilter
        {
            /// <summary>
            /// 成员解析器
            /// </summary>
            public TryParse TryParse;
            /// <summary>
            /// 成员位图索引
            /// </summary>
            public int MemberMapIndex;
            ///// <summary>
            ///// 成员选择
            ///// </summary>
            //public MemberFilters MemberFilters;
            ///// <summary>
            ///// 成员解析器
            ///// </summary>
            ///// <param name="parser">Json解析器</param>
            ///// <param name="value">目标数据</param>
            //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            //public void Call(Parser parser, ref valueType value)
            //{
            //    if ((parser.Config.MemberFilters & MemberFilters) == MemberFilters) TryParse(parser, ref value);
            //    else parser.Ignore();
            //}
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="parser">Json解析器</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">目标数据</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public void Call(Parser parser, MemberMap memberMap, ref valueType value)
            {
                //if ((parser.Config.MemberFilters & MemberFilters) == MemberFilters)
                //{
                //    TryParse(parser, ref value);
                //    memberMap.SetMember(MemberMapIndex);
                //}
                //else parser.Ignore();
                TryParse(parser, ref value);
                memberMap.SetMember(MemberMapIndex);
            }
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        internal delegate void TryParse(Parser parser, ref valueType value);
        /// <summary>
        /// 未知名称处理委托
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        private delegate void UnknownParse(Parser parser, ref valueType value, ref Pointer.Size name);
#if !NOJIT
        /// <summary>
        /// 默认名称解析
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        private delegate int ParseMember(Parser parser, ref valueType value, byte* names);
        /// <summary>
        /// 默认名称解析
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        /// <param name="names"></param>
        /// <param name="memberMap"></param>
        /// <returns></returns>
        private delegate int ParseMemberMap(Parser parser, ref valueType value, byte* names, MemberMap memberMap);
        /// <summary>
        /// 默认名称解析
        /// </summary>
        private static readonly ParseMember parseMember;
        /// <summary>
        /// 默认名称解析
        /// </summary>
        private static readonly ParseMemberMap parseMemberMap;
#endif
        /// <summary>
        /// JSON 解析类型配置
        /// </summary>
        private static readonly ParseAttribute attribute;
        /// <summary>
        /// 解析委托
        /// </summary>
        internal static readonly TryParse DefaultParser;
        /// <summary>
        /// 未知名称处理委托
        /// </summary>
        private static readonly UnknownParse onUnknownName;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;
        /// <summary>
        /// 是否匿名类型
        /// </summary>
        private static readonly bool isAnonymousType;
        /// <summary>
        /// 成员解析器集合
        /// </summary>
        private static readonly TryParseFilter[] memberParsers;
        /// <summary>
        /// 包装处理
        /// </summary>
        private static readonly TryParse unboxParser;
        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        private static readonly StateSearcher memberSearcher;
        /// <summary>
        /// 默认顺序成员名称数据
        /// </summary>
        private static Pointer memberNames;
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        internal static void Parse(Parser parser, ref valueType value)
        {
            if (DefaultParser == null)
            {
                if (isValueType) ParseValue(parser, ref value);
                else parseClass(parser, ref value);
            }
            else DefaultParser(parser, ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ParseValue(Parser parser, ref valueType value)
        {
            if (parser.SearchObject()) ParseMembers(parser, ref value);
            else value = default(valueType);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        private static void parseClass(Parser parser, ref valueType value)
        {
            if (parser.SearchObject())
            {
                if (value == null)
                {
                    Func<valueType> constructor = AutoCSer.Emit.Constructor<valueType>.New;
                    if (constructor == null)
                    {
                        parser.CheckNoConstructor(ref value, isAnonymousType);
                        if (value == null) return;
                    }
                    else value = constructor();
                }
                else if (isAnonymousType) parser.SetAnonymousType(value);
                ParseMembers(parser, ref value);
            }
            else
            {
                if (value != null && isAnonymousType) parser.SetAnonymousType(value);
                value = default(valueType);
            }
        }
        /// <summary>
        /// 数据成员解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        internal static void ParseMembers(Parser parser, ref valueType value)
        {
#if NOJIT
            byte* names = memberNames.Byte;
#endif
            ParseConfig config = parser.Config;
            MemberMap memberMap = parser.MemberMap;
            if (memberMap == null)
            {
#if NOJIT
                int index = 0;
                while ((names = parser.IsName(names, ref index)) != null)
                {
                    if (index == -1) return;
                    memberParsers[index].TryParse(parser, ref value);
                    if (parser.ParseState != ParseState.Success) return;
                    ++index;
                }
#else
                int index = parseMember(parser, ref value, memberNames.Byte);
                if (index == -1) return;
#endif
                if (index == 0 ? parser.IsFirstObject() : parser.IsNextObject())
                {
                    bool isQuote;
                    if (onUnknownName == null)
                    {
                        do
                        {
                            if ((index = memberSearcher.SearchName(parser, out isQuote)) != -1)
                            {
                                if (parser.SearchColon() == 0) return;
                                memberParsers[index].TryParse(parser, ref value);
                            }
                            else
                            {
                                if (parser.ParseState != ParseState.Success) return;
                                if (isQuote) parser.SearchStringEnd();
                                else parser.SearchNameEnd();
                                if (parser.ParseState != ParseState.Success || parser.SearchColon() == 0) return;
                                parser.Ignore();
                            }
                        }
                        while (parser.ParseState == ParseState.Success && parser.IsNextObject());
                    }
                    else
                    {
                        Pointer.Size name;
                        do
                        {
                            name.Data = parser.Current;
                            if ((index = memberSearcher.SearchName(parser, out isQuote)) != -1)
                            {
                                if (parser.SearchColon() == 0) return;
                                memberParsers[index].TryParse(parser, ref value);
                            }
                            else
                            {
                                if (parser.ParseState != ParseState.Success) return;
                                if (isQuote) parser.SearchStringEnd();
                                else parser.SearchNameEnd();
                                if (parser.ParseState != ParseState.Success) return;
                                name.ByteSize = (int)((byte*)parser.Current - (byte*)name.Data);
                                if (parser.SearchColon() == 0) return;
                                if (isQuote)
                                {
                                    name.Data = name.Byte + 2;
                                    name.ByteSize -= 4;
                                }
                                onUnknownName(parser, ref value, ref name);
                                if (parser.ParseState != ParseState.Success) return;
                            }
                        }
                        while (parser.IsNextObject());
                    }
                }
            }
            else if (memberMap.Type == MemberMap<valueType>.TypeInfo)
            {
                try
                {
                    memberMap.Empty();
                    parser.MemberMap = null;
#if NOJIT
                    int index = 0;
                    while ((names = parser.IsName(names, ref index)) != null)
                    {
                        if (index == -1) return;
                        memberParsers[index].Call(parser, memberMap, ref value);
                        if (parser.ParseState != ParseState.Success) return;
                        ++index;
                    }
#else
                    int index = parseMemberMap(parser, ref value, memberNames.Byte, memberMap);
                    if (index == -1) return;
#endif
                    if (index == 0 ? parser.IsFirstObject() : parser.IsNextObject())
                    {
                        bool isQuote;
                        if (onUnknownName == null)
                        {
                            do
                            {
                                if ((index = memberSearcher.SearchName(parser, out isQuote)) != -1)
                                {
                                    if (parser.SearchColon() == 0) return;
                                    memberParsers[index].Call(parser, memberMap, ref value);
                                }
                                else
                                {
                                    if (parser.ParseState != ParseState.Success) return;
                                    if (isQuote) parser.SearchStringEnd();
                                    else parser.SearchNameEnd();
                                    if (parser.ParseState != ParseState.Success || parser.SearchColon() == 0) return;
                                    parser.Ignore();
                                }
                            }
                            while (parser.ParseState == ParseState.Success && parser.IsNextObject());
                        }
                        else
                        {
                            Pointer.Size name;
                            do
                            {
                                name.Data = parser.Current;
                                if ((index = memberSearcher.SearchName(parser, out isQuote)) != -1)
                                {
                                    if (parser.SearchColon() == 0) return;
                                    memberParsers[index].Call(parser, memberMap, ref value);
                                }
                                else
                                {
                                    if (parser.ParseState != ParseState.Success) return;
                                    if (isQuote) parser.SearchStringEnd();
                                    else parser.SearchNameEnd();
                                    if (parser.ParseState != ParseState.Success) return;
                                    name.ByteSize = (int)((byte*)parser.Current - (byte*)name.Data);
                                    if (parser.SearchColon() == 0) return;
                                    if (isQuote)
                                    {
                                        name.Data = name.Byte + 2;
                                        name.ByteSize -= 4;
                                    }
                                    onUnknownName(parser, ref value, ref name);
                                    if (parser.ParseState != ParseState.Success) return;
                                }
                            }
                            while (parser.IsNextObject());
                        }
                    }
                }
                finally { parser.MemberMap = memberMap; }
            }
            else parser.ParseState = ParseState.MemberMap;
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ParseClass(Parser parser, ref valueType value)
        {
            if (DefaultParser == null) parseClass(parser, ref value);
            else DefaultParser(parser, ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ParseStruct(Parser parser, ref valueType value)
        {
            if (DefaultParser == null) ParseValue(parser, ref value);
            else DefaultParser(parser, ref value);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="values">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Array(Parser parser, ref valueType[] values)
        {
            int count = ArrayIndex(parser, ref values);
            if (count != -1 && count != values.Length) System.Array.Resize(ref values, count);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="values">目标数据</param>
        /// <returns>数据数量,-1表示失败</returns>
        internal static int ArrayIndex(Parser parser, ref valueType[] values)
        {
            parser.SearchArray(ref values);
            if (parser.ParseState != ParseState.Success || values == null) return -1;
            int index = 0;
            if (parser.IsFirstArrayValue())
            {
                do
                {
                    if (index == values.Length)
                    {
                        valueType value = default(valueType);
                        Parse(parser, ref value);
                        if (parser.ParseState != ParseState.Success) return -1;
                        values = values.copyNew(index == 0 ? sizeof(int) : (index << 1));
                        values[index++] = value;
                    }
                    else
                    {
                        Parse(parser, ref values[index]);
                        if (parser.ParseState != ParseState.Success) return -1;
                        ++index;
                    }
                }
                while (parser.IsNextArrayValue());
            }
            return parser.ParseState == ParseState.Success ? index : -1;
        }

        /// <summary>
        /// 不支持多维数组
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void arrayManyRank(Parser parser, ref valueType value)
        {
            parser.ParseState = ParseState.ArrayManyRank;
        }
        /// <summary>
        /// 忽略数据
        /// </summary>
        /// <param name="parser">Json解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void ignore(Parser parser, ref valueType value)
        {
            parser.Ignore();
        }
        /// <summary>
        /// 包装处理
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void unbox(Parser parser, ref valueType value)
        {
            unboxParser(parser, ref value);
        }

        static TypeParser()
        {
            Type type = typeof(valueType);
            MethodInfo methodInfo = Parser.GetParseMethod(type);
            if (methodInfo != null)
            {
                DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), methodInfo);
                return;
            }
            if (type.IsArray)
            {
                //if (type.GetArrayRank() == 1) DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), ParseMethodCache.GetArray(type.GetElementType()));
                if (type.GetArrayRank() == 1) DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), GenericType.Get(type.GetElementType()).JsonParseArrayMethod);
                else DefaultParser = arrayManyRank;
                return;
            }
            if (type.IsEnum)
            {
                Type enumType = System.Enum.GetUnderlyingType(type);
                if (AutoCSer.Metadata.TypeAttribute.GetAttribute<FlagsAttribute>(type) == null)
                {
                    if (enumType == typeof(uint)) DefaultParser = EnumUInt.Parse;
                    else if (enumType == typeof(byte)) DefaultParser = EnumByte.Parse;
                    else if (enumType == typeof(ulong)) DefaultParser = EnumULong.Parse;
                    else if (enumType == typeof(ushort)) DefaultParser = EnumUShort.Parse;
                    else if (enumType == typeof(long)) DefaultParser = EnumLong.Parse;
                    else if (enumType == typeof(short)) DefaultParser = EnumShort.Parse;
                    else if (enumType == typeof(sbyte)) DefaultParser = EnumSByte.Parse;
                    else DefaultParser = EnumInt.Parse;
                }
                else
                {
                    if (enumType == typeof(uint)) DefaultParser = EnumUInt.ParseFlags;
                    else if (enumType == typeof(byte)) DefaultParser = EnumByte.ParseFlags;
                    else if (enumType == typeof(ulong)) DefaultParser = EnumULong.ParseFlags;
                    else if (enumType == typeof(ushort)) DefaultParser = EnumUShort.ParseFlags;
                    else if (enumType == typeof(long)) DefaultParser = EnumLong.ParseFlags;
                    else if (enumType == typeof(short)) DefaultParser = EnumShort.ParseFlags;
                    else if (enumType == typeof(sbyte)) DefaultParser = EnumSByte.ParseFlags;
                    else DefaultParser = EnumInt.ParseFlags;
                }
                return;
            }
            if (type.IsInterface || type.IsPointer || typeof(Delegate).IsAssignableFrom(type))
            {
                DefaultParser = ignore;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Dictionary<,>))
                {
                    //DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), ParseMethodCache.GetDictionary(type));
                    DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), GenericType2.Get(type.GetGenericArguments()).JsonParseDictionaryMethod);
                    return;
                }
                if (genericType == typeof(Nullable<>))
                {
                    DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), ParseMethodCache.GetNullable(type));
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    //DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), ParseMethodCache.GetKeyValuePair(type));
                    DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), GenericType2.Get(type.GetGenericArguments()).JsonParseKeyValuePairMethod);
                    isValueType = true;
                    return;
                }
            }
            if ((methodInfo = ParseMethodCache.GetCustom(type)) != null)
            {
                if (type.IsValueType)
                {
#if NOJIT
                    DefaultParser = new CustomParser(methodInfo).Parse;
#else
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomJsonParser", null, new Type[] { typeof(Parser), type.MakeByRefType() }, type, true);
                    ILGenerator generator = dynamicMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarg_1);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.call(methodInfo);
                    generator.Emit(OpCodes.Ret);
                    DefaultParser = (TryParse)dynamicMethod.CreateDelegate(typeof(TryParse));
#endif
                }
                else DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), methodInfo);
            }
            else
            {
                Type attributeType;
                attribute = type.customAttribute<ParseAttribute>(out attributeType) ?? Parser.AllMemberAttribute;
                if ((methodInfo = ParseMethodCache.GetIEnumerableConstructor(type)) != null)
                {
                    DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), methodInfo);
                }
                else
                {
                    if (type.IsValueType) isValueType = true;
                    else if (attribute != Parser.AllMemberAttribute && attributeType != type)
                    {
                        for (Type baseType = type.BaseType; baseType != typeof(object); baseType = baseType.BaseType)
                        {
                            ParseAttribute baseAttribute = baseType.customAttribute<ParseAttribute>();
                            if (baseAttribute != null)
                            {
                                if (baseAttribute.IsBaseType)
                                {
                                    methodInfo = ParseMethodCache.BaseParseMethod.MakeGenericMethod(baseType, type);
                                    DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), methodInfo);
                                    return;
                                }
                                break;
                            }
                        }
                    }
                    FieldIndex defaultMember = null;
                    LeftArray<FieldIndex> fields = ParseMethodCache.GetFields(MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), attribute, ref defaultMember);
                    LeftArray<KeyValue<PropertyIndex, MethodInfo>> properties = ParseMethodCache.GetProperties(MemberIndexGroup<valueType>.GetProperties(attribute.MemberFilters), attribute);
                    bool isBox = false;
                    if (type.IsValueType && fields.Length + properties.Length == 1)
                    {
                        BoxSerializeAttribute boxSerialize = AutoCSer.Metadata.TypeAttribute.GetAttribute<BoxSerializeAttribute>(type);
                        if (boxSerialize != null && boxSerialize.IsJson)
                        {
                            isBox = true;
                            defaultMember = null;
                        }
                    }
                    TryParseFilter[] parsers = new TryParseFilter[fields.Length + properties.Length + (defaultMember == null ? 0 : 1)];
                    //memberMap.type memberMapType = memberMap<valueType>.TypeInfo;
                    string[] names = isBox ? null : new string[parsers.Length];
#if !NOJIT
                    ParseDynamicMethod dynamicMethod = isBox ? default(ParseDynamicMethod) : new ParseDynamicMethod(type, false), memberMapDynamicMethod = isBox ? default(ParseDynamicMethod) : new ParseDynamicMethod(type, true);
#endif
                    int index = 0, nameLength = 0, maxNameLength = 0;
                    foreach (FieldIndex member in fields)
                    {
                        TryParseFilter tryParse = parsers[index] = new TryParseFilter
                        {
#if NOJIT
                            TryParse = new FieldParser(member.Member).Parser(),
#else
                            TryParse = (TryParse)ParseMethodCache.CreateDynamicMethod(type, member.Member).CreateDelegate(typeof(TryParse)),
#endif
                            MemberMapIndex = member.MemberIndex,
                            //MemberFilters = member.Member.IsPublic ? Metadata.MemberFilters.PublicInstanceField : Metadata.MemberFilters.NonPublicInstanceField
                        };
                        if (!isBox)
                        {
#if !NOJIT
                            dynamicMethod.Push(member);
                            memberMapDynamicMethod.Push(member);
#endif
                            string name = member.AnonymousName;
                            if (name.Length > maxNameLength) maxNameLength = name.Length;
                            nameLength += (names[index++] = name).Length;
                            if (member == defaultMember)
                            {
                                parsers[parsers.Length - 1] = tryParse;
                                names[parsers.Length - 1] = string.Empty;
                            }
                        }
                    }
                    foreach (KeyValue<PropertyIndex, MethodInfo> member in properties)
                    {
                        parsers[index] = new TryParseFilter
                        {
#if NOJIT
                            TryParse = new PropertyParser(member.Key.Member).Parser(),
#else
                            TryParse = (TryParse)ParseMethodCache.CreateDynamicMethod(type, member.Key.Member, member.Value).CreateDelegate(typeof(TryParse)),
#endif
                            MemberMapIndex = member.Key.MemberIndex,
                            //MemberFilters = member.Value.IsPublic ? Metadata.MemberFilters.PublicInstanceProperty : Metadata.MemberFilters.NonPublicInstanceProperty
                        };
                        if (!isBox)
                        {
#if !NOJIT
                            dynamicMethod.Push(member.Key, member.Value);
                            memberMapDynamicMethod.Push(member.Key, member.Value);
#endif
                            if (member.Key.Member.Name.Length > maxNameLength) maxNameLength = member.Key.Member.Name.Length;
                            nameLength += (names[index++] = member.Key.Member.Name).Length;
                        }
                    }
                    if (isBox)
                    {
                        unboxParser = parsers[0].TryParse;
                        DefaultParser = unbox;
                    }
                    else
                    {
#if !NOJIT
                        parseMember = (ParseMember)dynamicMethod.Create<ParseMember>();
                        parseMemberMap = (ParseMemberMap)memberMapDynamicMethod.Create<ParseMemberMap>();
#endif
                        if (type.Name[0] == '<') isAnonymousType = true;
                        if (maxNameLength > (short.MaxValue >> 1) - 4 || nameLength == 0) memberNames = Unmanaged.NullByte8;
                        else
                        {
                            memberNames = new Pointer { Data = Unmanaged.GetStatic((nameLength + (names.Length - (defaultMember == null ? 0 : 1)) * 5 + 1) << 1, false) };
                            byte* write = memberNames.Byte;
                            foreach (string name in names)
                            {
                                if (name.Length != 0)
                                {
                                    if (write == memberNames.Byte)
                                    {
                                        *(short*)write = (short)((name.Length + 3) * sizeof(char));
                                        *(char*)(write + sizeof(short)) = '"';
                                        write += sizeof(short) + sizeof(char);
                                    }
                                    else
                                    {
                                        *(short*)write = (short)((name.Length + 4) * sizeof(char));
                                        *(int*)(write + sizeof(short)) = ',' + ('"' << 16);
                                        write += sizeof(short) + sizeof(int);
                                    }
                                    fixed (char* nameFixed = name) AutoCSer.Extension.StringExtension.SimpleCopyNotNull(nameFixed, (char*)write, name.Length);
                                    *(int*)(write += name.Length << 1) = '"' + (':' << 16);
                                    write += sizeof(int);
                                }
                            }
                            *(short*)write = 0;
                        }
                        memberSearcher = new StateSearcher(StateSearcher.GetMemberSearcher(type, names));
                        memberParsers = parsers;

                        Type refType = type.MakeByRefType();
                        foreach (AutoCSer.Metadata.AttributeMethod attributeMethod in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                        {
                            if ((methodInfo = attributeMethod.Method).ReturnType == typeof(void))
                            {
                                ParameterInfo[] parameters = methodInfo.GetParameters();
                                if (parameters.Length == 3 && parameters[0].ParameterType == typeof(Parser) && parameters[1].ParameterType == refType && parameters[2].ParameterType == Emit.Pub.PointerSizeRefType)
                                {
                                    if (attributeMethod.GetAttribute<ParseUnknownNameAttriubte>() != null)
                                    {
                                        onUnknownName = (UnknownParse)Delegate.CreateDelegate(typeof(UnknownParse), methodInfo);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

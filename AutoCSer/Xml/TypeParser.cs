using System;
using AutoCSer.Metadata;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;
#if !NOJIT
using/**/System.Reflection.Emit;
#endif

namespace AutoCSer.Xml
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
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            protected static void parse(Parser parser, ref valueType value)
            {
                int index = enumSearcher.SearchEnum(parser);
                if (index != -1) value = enumValues[index];
                else if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value"></param>
            /// <param name="index">第一个枚举索引</param>
            /// <param name="nextIndex">第二个枚举索引</param>
            protected static void getIndex(Parser parser, ref valueType value, out int index, ref int nextIndex)
            {
                if ((index = enumSearcher.SearchFlagEnum(parser)) == -1)
                {
                    if (parser.Config.IsMatchEnum)
                    {
                        parser.State = ParseState.NoFoundEnumValue;
                        return;
                    }
                    else
                    {
                        do
                        {
                            if (parser.IsNextFlagEnum() == 0) return;
                        }
                        while ((index = enumSearcher.SearchFlagEnum(parser)) == -1);
                    }
                }
                do
                {
                    if (parser.IsNextFlagEnum() == 0)
                    {
                        value = enumValues[index];
                        return;
                    }
                    if ((nextIndex = enumSearcher.SearchFlagEnum(parser)) != -1) break;
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
            private static readonly Pointer enumInts;
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    byte intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, byte>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    byte intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, byte>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
                        else parser.IgnoreSearchValue();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            byte intValue = enumInts.Byte[index];
                            intValue |= enumInts.Byte[nextIndex];
                            while (parser.IsNextFlagEnum() != 0)
                            {
                                if ((index = enumSearcher.NextFlagEnum(parser)) != -1) intValue |= enumInts.Byte[index];
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
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    sbyte intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, sbyte>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    sbyte intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, sbyte>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
                        else parser.IgnoreSearchValue();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            byte intValue = (byte)enumInts.SByte[index];
                            intValue |= (byte)enumInts.SByte[nextIndex];
                            while (parser.IsNextFlagEnum() != 0)
                            {
                                if ((index = enumSearcher.NextFlagEnum(parser)) != -1) intValue |= (byte)enumInts.SByte[index];
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
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    short intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, short>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    short intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, short>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
                        else parser.IgnoreSearchValue();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            ushort intValue = (ushort)enumInts.Short[index];
                            intValue |= (ushort)enumInts.Short[nextIndex];
                            while (parser.IsNextFlagEnum() != 0)
                            {
                                if ((index = enumSearcher.NextFlagEnum(parser)) != -1) intValue |= (ushort)enumInts.Short[index];
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
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    ushort intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, ushort>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    ushort intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, ushort>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
                        else parser.IgnoreSearchValue();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            ushort intValue = enumInts.UShort[index];
                            intValue |= enumInts.UShort[nextIndex];
                            while (parser.IsNextFlagEnum() != 0)
                            {
                                if ((index = enumSearcher.NextFlagEnum(parser)) != -1) intValue |= enumInts.UShort[index];
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
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    int intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, int>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    int intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, int>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
                        else parser.IgnoreSearchValue();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            int intValue = enumInts.Int[index];
                            intValue |= enumInts.Int[nextIndex];
                            while (parser.IsNextFlagEnum() != 0)
                            {
                                if ((index = enumSearcher.NextFlagEnum(parser)) != -1) intValue |= enumInts.Int[index];
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
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    uint intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, uint>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    uint intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, uint>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
                        else parser.IgnoreSearchValue();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            uint intValue = enumInts.UInt[index];
                            intValue |= enumInts.UInt[nextIndex];
                            while (parser.IsNextFlagEnum() != 0)
                            {
                                if ((index = enumSearcher.NextFlagEnum(parser)) != -1) intValue |= enumInts.UInt[index];
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
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    long intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, long>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumberFlag())
                {
                    long intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, long>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
                        else parser.IgnoreSearchValue();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            long intValue = enumInts.Long[index];
                            intValue |= enumInts.Long[nextIndex];
                            while (parser.IsNextFlagEnum() != 0)
                            {
                                if ((index = enumSearcher.NextFlagEnum(parser)) != -1) intValue |= enumInts.Long[index];
                            }
                            value = Emit.EnumCast<valueType, long>.FromInt(intValue);
                        }
                    }
                }
            }
            static EnumLong()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic(enumValues.Length * sizeof(long), false) };
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
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void Parse(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    ulong intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, ulong>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success) parse(parser, ref value);
            }
            /// <summary>
            /// 枚举值解析
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            public static void ParseFlags(Parser parser, ref valueType value)
            {
                if (parser.IsEnumNumber())
                {
                    ulong intValue = 0;
                    parser.ParseNumber(ref intValue);
                    value = Emit.EnumCast<valueType, ulong>.FromInt(intValue);
                }
                else if (parser.State == ParseState.Success)
                {
                    if (enumSearcher.State == null)
                    {
                        if (parser.Config.IsMatchEnum) parser.State = ParseState.NoFoundEnumValue;
                        else parser.IgnoreSearchValue();
                    }
                    else
                    {
                        int index, nextIndex = -1;
                        getIndex(parser, ref value, out index, ref nextIndex);
                        if (nextIndex != -1)
                        {
                            ulong intValue = enumInts.ULong[index];
                            intValue |= enumInts.ULong[nextIndex];
                            while (parser.IsNextFlagEnum() != 0)
                            {
                                if ((index = enumSearcher.NextFlagEnum(parser)) != -1) intValue |= enumInts.ULong[index];
                            }
                            value = Emit.EnumCast<valueType, ulong>.FromInt(intValue);
                        }
                    }
                }
            }
            static EnumULong()
            {
                enumInts = new Pointer { Data = Unmanaged.GetStatic(enumValues.Length * sizeof(ulong), false) };
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
            /// 集合子节点名称
            /// </summary>
            public string ItemName;
            /// <summary>
            /// 成员位图索引
            /// </summary>
            public int MemberMapIndex;
            ///// <summary>
            ///// 成员选择
            ///// </summary>
            //public AutoCSer.Metadata.MemberFilters MemberFilter;
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="value">目标数据</param>
            /// <returns>是否存在下一个数据</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public int Call(Parser parser, ref valueType value)
            {
                //parser.ItemName = ItemName;
                //if ((parser.Config.MemberFilter & MemberFilter) == MemberFilter)
                //{
                //    TryParse(parser, ref value);
                //    return parser.State == ParseState.Success ? 1 : 0;
                //}
                //return parser.IgnoreValue();
                parser.ItemName = ItemName;
                TryParse(parser, ref value);
                return parser.State == ParseState.Success ? 1 : 0;
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="parser">XML解析器</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">目标数据</param>
            /// <returns>是否存在下一个数据</returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            public int Call(Parser parser, MemberMap memberMap, ref valueType value)
            {
                //parser.ItemName = ItemName;
                //if ((parser.Config.MemberFilter & MemberFilter) == MemberFilter)
                //{
                //    TryParse(parser, ref value);
                //    if (parser.State == ParseState.Success)
                //    {
                //        memberMap.SetMember(MemberMapIndex);
                //        return 1;
                //    }
                //    return 0;
                //}
                //return parser.IgnoreValue();
                parser.ItemName = ItemName;
                TryParse(parser, ref value);
                if (parser.State == ParseState.Success)
                {
                    memberMap.SetMember(MemberMapIndex);
                    return 1;
                }
                return 0;
            }
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        internal delegate void TryParse(Parser parser, ref valueType value);
        /// <summary>
        /// 未知名称解析委托
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        /// <param name="name">节点名称</param>
        internal delegate bool UnknownParse(Parser parser, ref valueType value, ref Pointer.Size name);
        /// <summary>
        /// 解析委托
        /// </summary>
        internal static readonly TryParse DefaultParser;
        /// <summary>
        /// 成员解析器集合
        /// </summary>
        private static readonly TryParseFilter[] memberParsers;
        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        private static readonly Pointer memberSearcher;
        /// <summary>
        /// 默认顺序成员名称数据
        /// </summary>
        private static readonly Pointer memberNames;
        /// <summary>
        /// 未知名称节点处理
        /// </summary>
        private static readonly UnknownParse onUnknownName;
        /// <summary>
        /// XML解析类型配置
        /// </summary>
        private static readonly SerializeAttribute attribute;
        /// <summary>
        /// 是否值类型
        /// </summary>
        private static readonly bool isValueType;
        /// <summary>
        /// 是否匿名类型
        /// </summary>
        private static readonly bool isAnonymousType;

        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        internal static void Parse(Parser parser, ref valueType value)
        {
            if (DefaultParser == null)
            {
                if (isValueType) ParseMembers(parser, ref value);
                else parseClass(parser, ref value);
            }
            else DefaultParser(parser, ref value);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void parseClass(Parser parser, ref valueType value)
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
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="parser">XML解析器</param>
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
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ParseStruct(Parser parser, ref valueType value)
        {
            if (DefaultParser == null) ParseMembers(parser, ref value);
            else DefaultParser(parser, ref value);
        }
        /// <summary>
        /// 数据成员解析
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        internal static void ParseMembers(Parser parser, ref valueType value)
        {
            byte* names = memberNames.Byte;
            ParseConfig config = parser.Config;
            MemberMap memberMap = parser.MemberMap;
            int index = 0;
            if (memberMap == null)
            {
                while (parser.IsName(names, ref index))
                {
                    if (index == -1) return;
                    memberParsers[index].Call(parser, ref value);
                    if (parser.State != ParseState.Success) return;
                    if (parser.IsNameEnd(names) == 0)
                    {
                        if (parser.CheckNameEnd((char*)(names + (sizeof(short) + sizeof(char))), (*(short*)names >> 1) - 2) == 0) return;
                        break;
                    }
                    ++index;
                    names += *(short*)names + sizeof(short);
                }
                AutoCSer.StateSearcher.CharSearcher searcher = new AutoCSer.StateSearcher.CharSearcher(memberSearcher);
                Pointer.Size name = new Pointer.Size();
                byte isTagEnd = 0;
                if (onUnknownName == null)
                {
                    do
                    {
                        if ((name.Data = parser.GetName(ref name.ByteSize, ref isTagEnd)) == null) return;
                        if (isTagEnd == 0)
                        {
                            if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                            {
                                if (parser.IgnoreValue() == 0) return;
                            }
                            else if (memberParsers[index].Call(parser, ref value) == 0) return;
                            if (parser.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                        }
                    }
                    while (true);
                }
                else
                {
                    do
                    {
                        if ((name.Data = parser.GetName(ref name.ByteSize, ref isTagEnd)) == null) return;
                        if (isTagEnd == 0)
                        {
                            if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                            {
                                name.ByteSize <<= 1;
                                if (onUnknownName(parser, ref value, ref name))
                                {
                                    if (parser.State != ParseState.Success) return;
                                }
                                else
                                {
                                    if (parser.State == ParseState.Success) parser.State = ParseState.UnknownNameError;
                                    return;
                                }
                            }
                            else if (memberParsers[index].Call(parser, ref value) == 0) return;
                            if (parser.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                        }
                    }
                    while (true);
                }
            }
            else if (memberMap.Type == MemberMap<valueType>.TypeInfo)
            {
                memberMap.Empty();
                parser.MemberMap = null;
                while (parser.IsName(names, ref index))
                {
                    if (index == -1) return;
                    memberParsers[index].Call(parser, memberMap, ref value);
                    if (parser.State != ParseState.Success) return;
                    if (parser.IsNameEnd(names) == 0)
                    {
                        if (parser.CheckNameEnd((char*)(names + (sizeof(short) + sizeof(char))), (*(short*)names >> 1) - 2) == 0) return;
                        break;
                    }
                    ++index;
                    names += *(short*)names + sizeof(short);
                }
                AutoCSer.StateSearcher.CharSearcher searcher = new AutoCSer.StateSearcher.CharSearcher(memberSearcher);
                Pointer.Size name = new Pointer.Size();
                byte isTagEnd = 0;
                try
                {
                    if (onUnknownName == null)
                    {
                        do
                        {
                            if ((name.Data = parser.GetName(ref name.ByteSize, ref isTagEnd)) == null) return;
                            if (isTagEnd == 0)
                            {
                                if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                                {
                                    if (parser.IgnoreValue() == 0) return;
                                }
                                else if (memberParsers[index].Call(parser, memberMap, ref value) == 0) return;
                                if (parser.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                            }
                        }
                        while (true);
                    }
                    else
                    {
                        do
                        {
                            if ((name.Data = parser.GetName(ref name.ByteSize, ref isTagEnd)) == null) return;
                            if (isTagEnd == 0)
                            {
                                if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                                {
                                    name.ByteSize <<= 1;
                                    if (onUnknownName(parser, ref value, ref name))
                                    {
                                        if (parser.State != ParseState.Success) return;
                                    }
                                    else
                                    {
                                        if (parser.State == ParseState.Success) parser.State = ParseState.UnknownNameError;
                                        return;
                                    }
                                }
                                else if (memberParsers[index].Call(parser, memberMap, ref value) == 0) return;
                                if (parser.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                            }
                        }
                        while (true);
                    }
                }
                finally { parser.MemberMap = memberMap; }
            }
            else parser.State = ParseState.MemberMap;
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="parser">XML解析器</param>
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
        /// <param name="parser">XML解析器</param>
        /// <param name="values">目标数据</param>
        /// <returns>数据数量,-1表示失败</returns>
        internal static int ArrayIndex(Parser parser, ref valueType[] values)
        {
            if (values == null) values = NullValue<valueType>.Array;
            string arrayItemName = parser.ArrayItemName;
            Pointer.Size name = new Pointer.Size();
            int index = 0;
            byte isTagEnd = 0;
            fixed (char* itemFixed = arrayItemName)
            {
                do
                {
                    if ((name.Data = parser.GetName(ref name.ByteSize, ref isTagEnd)) == null) break;
                    if (isTagEnd == 0)
                    {
                        if (arrayItemName.Length != name.ByteSize || !AutoCSer.Memory.SimpleEqualNotNull((byte*)itemFixed, name.Byte, name.ByteSize << 1))
                        {
                            parser.State = ParseState.NotArrayItem;
                            return -1;
                        }
                        if (index == values.Length)
                        {
                            valueType value = default(valueType);
                            if (parser.IsArrayItem(itemFixed, arrayItemName.Length) != 0)
                            {
                                Parse(parser, ref value);
                                if (parser.State != ParseState.Success) return -1;
                                if (parser.CheckNameEnd(itemFixed, name.ByteSize) == 0) break;
                            }
                            values = values.copyNew(index == 0 ? sizeof(int) : (index << 1));
                            values[index++] = value;
                        }
                        else
                        {
                            if (parser.IsArrayItem(itemFixed, arrayItemName.Length) != 0)
                            {
                                Parse(parser, ref values[index]);
                                if (parser.State != ParseState.Success) return -1;
                                if (parser.CheckNameEnd(itemFixed, name.ByteSize) == 0) break;
                            }
                            ++index;
                        }
                    }
                    else
                    {
                        if (index == values.Length) values = values.copyNew(index == 0 ? sizeof(int) : (index << 1));
                        values[index++] = default(valueType);
                    }
                }
                while (true);
            }
            return parser.State == ParseState.Success ? index : -1;
        }

        /// <summary>
        /// 不支持基元类型解析
        /// </summary>
        /// <param name="parser">XML解析器</param>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void notSupport(Parser parser, ref valueType value)
        {
            parser.State = ParseState.NotSupport;
        }
        /// <summary>
        /// 包装处理
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static void unbox(Parser parser, ref valueType value)
        {
            if (parser.IsValue() != 0) memberParsers[0].TryParse(parser, ref value);
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
                if (type.GetArrayRank() == 1) DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), GenericType.Get(type.GetElementType()).XmlParseArrayMethod);
                else DefaultParser = notSupport;
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
                DefaultParser = notSupport;
                return;
            }
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    Type[] parameterTypes = type.GetGenericArguments();
                    //DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), parameterTypes[0].IsEnum ? ParseMethodCache.GetNullableEnumParse(type, parameterTypes) : ParseMethodCache.GetNullableParse(type, parameterTypes));
                    DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), parameterTypes[0].IsEnum ? StructGenericType.Get(parameterTypes[0]).XmlParseNullableEnumMethod : StructGenericType.Get(parameterTypes[0]).XmlParseNullableMethod);
                    return;
                }
                if (genericType == typeof(KeyValuePair<,>))
                {
                    //DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), ParseMethodCache.GetKeyValuePair(type));
                    DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), GenericType2.Get(type.GetGenericArguments()).XmlParseKeyValuePairMethod);
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
                    DynamicMethod dynamicMethod = new DynamicMethod("CustomXmlParser", null, new Type[] { typeof(Parser), type.MakeByRefType() }, type, true);
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
                attribute = type.customAttribute<SerializeAttribute>(out attributeType) ?? Serializer.AllMemberAttribute;
                if ((methodInfo = ParseMethodCache.GetIEnumerableConstructor(type)) != null)
                {
                    DefaultParser = (TryParse)Delegate.CreateDelegate(typeof(TryParse), methodInfo);
                }
                else
                {
                    if (type.IsValueType) isValueType = true;
                    else if (attribute != Serializer.AllMemberAttribute && attributeType != type)
                    {
                        for (Type baseType = type.BaseType; baseType != typeof(object); baseType = baseType.BaseType)
                        {
                            SerializeAttribute baseAttribute = baseType.customAttribute<SerializeAttribute>();
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
                    if (type.IsValueType)
                    {
                        foreach (AutoCSer.Metadata.AttributeMethod attributeMethod in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                        {
                            if (attributeMethod.Method.ReturnType == typeof(bool))
                            {
                                ParameterInfo[] parameters = attributeMethod.Method.GetParameters();
                                if (parameters.Length == 2 && parameters[0].ParameterType == typeof(Parser) && parameters[1].ParameterType == Emit.Pub.PointerSizeRefType)
                                {
                                    if (attributeMethod.GetAttribute<UnknownNameAttribute>() != null)
                                    {
#if NOJIT
                                        onUnknownName = new UnknownParser(methodInfo).Parse;
#else
                                        DynamicMethod dynamicMethod = new DynamicMethod("XmlUnknownParse", null, new Type[] { typeof(Parser), type.MakeByRefType(), Emit.Pub.PointerSizeRefType }, type, true);
                                        ILGenerator generator = dynamicMethod.GetILGenerator();
                                        generator.Emit(OpCodes.Ldarg_1);
                                        generator.Emit(OpCodes.Ldarg_0);
                                        generator.Emit(OpCodes.Ldarg_2);
                                        generator.call(methodInfo);
                                        generator.Emit(OpCodes.Ret);
                                        onUnknownName = (UnknownParse)dynamicMethod.CreateDelegate(typeof(UnknownParse));
#endif
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Type refType = type.MakeByRefType();
                        foreach (AutoCSer.Metadata.AttributeMethod attributeMethod in AutoCSer.Metadata.AttributeMethod.GetStatic(type))
                        {
                            if (attributeMethod.Method.ReturnType == typeof(bool))
                            {
                                ParameterInfo[] parameters = attributeMethod.Method.GetParameters();
                                if (parameters.Length == 3 && parameters[0].ParameterType == typeof(Parser) && parameters[1].ParameterType == refType && parameters[2].ParameterType == Emit.Pub.PointerSizeRefType)
                                {
                                    if (attributeMethod.GetAttribute<UnknownNameAttribute>() != null)
                                    {
                                        onUnknownName = (UnknownParse)Delegate.CreateDelegate(typeof(UnknownParse), attributeMethod.Method);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    FieldIndex defaultMember = null;
                    LeftArray<KeyValue<FieldIndex, MemberAttribute>> fields = SerializeMethodCache.GetFields(MemberIndexGroup<valueType>.GetFields(attribute.MemberFilters), attribute);
                    LeftArray<PropertyMethod> properties = ParseMethodCache.GetProperties(MemberIndexGroup<valueType>.GetProperties(attribute.MemberFilters), attribute);
                    bool isBox = false;
                    if (type.IsValueType && fields.Length + properties.Length == 1)
                    {
                        BoxSerializeAttribute boxSerialize = AutoCSer.Metadata.TypeAttribute.GetAttribute<BoxSerializeAttribute>(type);
                        if (boxSerialize != null && boxSerialize.IsXml)
                        {
                            isBox = true;
                            defaultMember = null;
                        }
                    }
                    TryParseFilter[] parsers = new TryParseFilter[fields.Length + properties.Length + (defaultMember == null ? 0 : 1)];
                    //memberMap.type memberMapType = memberMap<valueType>.TypeInfo;
                    string[] names = isBox ? null : new string[parsers.Length];
                    int index = 0, nameLength = 0, maxNameLength = 0;
                    foreach (KeyValue<FieldIndex, MemberAttribute> member in fields)
                    {
                        TryParseFilter tryParse = parsers[index] = new TryParseFilter
                        {
#if NOJIT
                            TryParse = new FieldParser(member.Key.Member).Parser(),
#else
                            TryParse = (TryParse)ParseMethodCache.CreateDynamicMethod(type, member.Key.Member).CreateDelegate(typeof(TryParse)),
#endif
                            ItemName = member.Value == null ? null : member.Value.ItemName,
                            MemberMapIndex = member.Key.MemberIndex,
                            //MemberFilter = member.Key.Member.IsPublic ? MemberFilters.PublicInstanceField : MemberFilters.NonPublicInstanceField
                        };
                        if (!isBox)
                        {
                            string name = member.Key.AnonymousName;
                            if (name.Length > maxNameLength) maxNameLength = name.Length;
                            nameLength += (names[index++] = name).Length;
                            if (member.Key == defaultMember)
                            {
                                parsers[parsers.Length - 1] = tryParse;
                                names[parsers.Length - 1] = string.Empty;
                            }
                        }
                    }
                    foreach (PropertyMethod member in properties)
                    {
                        parsers[index] = new TryParseFilter
                        {
#if NOJIT
                            TryParse = new PropertyParser(member.Property.Member).Parser(),
#else
                            TryParse = (TryParse)ParseMethodCache.CreateDynamicMethod(type, member.Property.Member, member.Method).CreateDelegate(typeof(TryParse)),
#endif
                            ItemName = member.Attribute == null ? null : member.Attribute.ItemName,
                            MemberMapIndex = member.Property.MemberIndex,
                            //MemberFilter = member.Method.IsPublic ? MemberFilters.PublicInstanceProperty : MemberFilters.NonPublicInstanceProperty
                        };
                        if (!isBox)
                        {
                            if (member.Property.Member.Name.Length > maxNameLength) maxNameLength = member.Property.Member.Name.Length;
                            nameLength += (names[index++] = member.Property.Member.Name).Length;
                        }
                    }
                    memberParsers = parsers;
                    if (isBox) DefaultParser = unbox;
                    else
                    {
                        if (type.Name[0] == '<') isAnonymousType = true;
                        if (maxNameLength > (short.MaxValue >> 1) - 2 || nameLength == 0) memberNames = Unmanaged.NullByte8;
                        else
                        {
                            memberNames = new Pointer { Data = Unmanaged.GetStatic((nameLength + (names.Length - (defaultMember == null ? 0 : 1)) * 3 + 1) << 1, false) };
                            byte* write = memberNames.Byte;
                            foreach (string name in names)
                            {
                                if (name.Length != 0)
                                {
                                    *(short*)write = (short)((name.Length + 2) * sizeof(char));
                                    *(char*)(write + sizeof(short)) = '<';
                                    fixed (char* nameFixed = name) AutoCSer.Extension.StringExtension.SimpleCopyNotNull(nameFixed, (char*)(write + (sizeof(short) + sizeof(char))), name.Length);
                                    *(char*)(write += (sizeof(short) + sizeof(char)) + (name.Length << 1)) = '>';
                                    write += sizeof(char);
                                }
                            }
                            *(short*)write = 0;
                        }
                        if (type.IsGenericType) memberSearcher = ParseMethodCache.GetGenericDefinitionMemberSearcher(type, names);
                        else memberSearcher = AutoCSer.StateSearcher.CharBuilder.Create(names, true).Pointer;
                    }
                }
            }
        }
    }
}

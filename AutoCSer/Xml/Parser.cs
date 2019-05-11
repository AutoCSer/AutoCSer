using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Xml
{
    /// <summary>
    /// XML 解析器
    /// </summary>
    public unsafe sealed partial class Parser : AutoCSer.Threading.Link<Parser>
    {
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly ParseConfig DefaultConfig = ConfigLoader.GetUnion(typeof(ParseConfig)).ParseConfig ?? new ParseConfig();
        /// <summary>
        /// 字符状态位查询表格
        /// </summary>
        private readonly byte* bits = Bits.Byte;
        /// <summary>
        /// 属性
        /// </summary>
        private LeftArray<KeyValue<Range, Range>> attributes;
        /// <summary>
        /// 配置参数
        /// </summary>
        internal ParseConfig Config;
        /// <summary>
        /// 成员位图
        /// </summary>
        public AutoCSer.Metadata.MemberMap MemberMap { internal get; set; }
        /// <summary>
        /// 集合子节点名称
        /// </summary>
        internal string ItemName;
        /// <summary>
        /// 集合子节点名称
        /// </summary>
        internal string ArrayItemName
        {
            get
            {
                if (ItemName == null) return Config.ItemName ?? "item";
                string value = ItemName;
                ItemName = null;
                return value;
            }
        }
        /// <summary>
        /// 匿名类型数据
        /// </summary>
        private LeftArray<KeyValue<Type, object>> anonymousTypes;
        /// <summary>
        /// XML字符串
        /// </summary>
        private string xml;
        /// <summary>
        /// XML字符串起始位置
        /// </summary>
        private char* xmlFixed;
        /// <summary>
        /// 当前解析位置
        /// </summary>
        private char* current;
        /// <summary>
        /// 自定义序列化获取当前读取数据位置
        /// </summary>
        public char* CustomRead
        {
            get { return current; }
        }
        /// <summary>
        /// 解析结束位置
        /// </summary>
        private char* end;
        /// <summary>
        /// 当前数据起始位置
        /// </summary>
        private char* valueStart;
        /// <summary>
        /// 当前数据长度
        /// </summary>
        private int valueSize;
        /// <summary>
        /// 属性名称起始位置
        /// </summary>
        private int attributeNameStartIndex;
        /// <summary>
        /// 属性名称结束位置
        /// </summary>
        private int attributeNameEndIndex;
        /// <summary>
        /// 数字符号
        /// </summary>
        private char sign;
        /// <summary>
        /// 当前数据是否CDATA
        /// </summary>
        internal byte IsCData;
        /// <summary>
        /// 名称解析节点是否结束
        /// </summary>
        private byte isTagEnd;
        /// <summary>
        /// 解析状态
        /// </summary>
        internal ParseState State;
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="xml">XML字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>解析状态</returns>
        private ParseResult parse<valueType>(ref SubString xml, ref valueType value, ParseConfig config)
        {
            fixed (char* xmlFixed = (this.xml = xml.String))
            {
                current = (this.xmlFixed = xmlFixed) + xml.Start;
                end = current + xml.Length;
                this.Config = config ?? DefaultConfig;
                parse(ref value);
                if (State == ParseState.Success) return new ParseResult { State = ParseState.Success, MemberMap = MemberMap };
                return new ParseResult { State = State, MemberMap = MemberMap, Xml = xml, Index = (int)(current - xmlFixed) - xml.Start };
            }
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="xml">XML字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>解析状态</returns>
        private ParseResult parse<valueType>(string xml, ref valueType value, ParseConfig config)
        {
            fixed (char* xmlFixed = (this.xml = xml))
            {
                current = this.xmlFixed = xmlFixed;
                end = current + xml.Length;
                this.Config = config ?? DefaultConfig;
                parse(ref value);
                if (State == ParseState.Success) return new ParseResult { State = ParseState.Success, MemberMap = MemberMap };
                return new ParseResult { State = State, MemberMap = MemberMap, Xml = xml, Index = (int)(current - xmlFixed) };
            }
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标类型</typeparam>
        /// <param name="value">目标数据</param>
        private void parse<valueType>(ref valueType value)
        {
            string bootName = Config.BootNodeName;
            fixed (char* bootNameFixed = bootName)
            {
            NEXTEND:
                while (end != current)
                {
                    if (((bits[*(byte*)--end] & spaceBit) | *(((byte*)end) + 1)) != 0)
                    {
                        if (*end == '>')
                        {
                            if (*(end - 1) == '-')
                            {
                                if (*(end - 2) == '-' && (end -= 2 + 3) > current)
                                {
                                    do
                                    {
                                    NOTE:
                                        if (*--end == '<')
                                        {
                                            if (*(end + 1) == '!' && *(int*)(end + 2) == '-' + ('-' << 16)) goto NEXTEND;
                                            if ((end -= 3) <= current) break;
                                            goto NOTE;
                                        }
                                    }
                                    while (end != current);
                                }
                                State = ParseState.NoteError;
                            }
                            else if ((end -= (2 + bootName.Length)) > current && *(int*)end == ('<' + ('/' << 16))
                                && AutoCSer.Memory.SimpleEqualNotNull((byte*)bootNameFixed, (byte*)(end + 2), bootName.Length << 1))
                            {
                                goto START;
                            }
                        }
                        State = ParseState.NotFoundBootNodeEnd;
                        return;
                    }
                }
            START:
                State = ParseState.Success;
                space();
                if (State == ParseState.Success)
                {
                    if (*(int*)current == ('<' + ('?' << 16)))
                    {
                        current += 3;
                        do
                        {
                            if (*current == '>')
                            {
                                if (current <= end)
                                {
                                    if (*(current - 1) == '?')
                                    {
                                        ++current;
                                        break;
                                    }
                                    else State = ParseState.HeaderError;
                                }
                                else State = ParseState.CrashEnd;
                                return;
                            }
                            ++current;
                        }
                        while (true);
                        space();
                        if (State != ParseState.Success) return;
                    }
                    if (*current == '<' && AutoCSer.Memory.SimpleEqualNotNull((byte*)bootNameFixed, (byte*)(++current), bootName.Length << 1))
                    {
                        if (((bits[*(byte*)(current += bootName.Length)] & spaceBit) | *(((byte*)current) + 1)) != 0)
                        {
                            if (*current == '>')
                            {
                                attributes.Length = 0;
                                ++current;
                                goto PARSE;
                            }
                        }
                        else
                        {
                            ++current;
                            attribute();
                            if (State == ParseState.Success) goto PARSE;
                        }
                    }
                }
                else State = ParseState.NotFoundBootNodeStart;
                return;
            PARSE:
                TypeParser<valueType>.Parse(this, ref value);
                if (State == ParseState.Success)
                {
                    space();
                    if (State == ParseState.Success)
                    {
                        if (current == end) return;
                        State = ParseState.CrashEnd;
                    }
                }
            }
        }
        /// <summary>
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool TypeParse<valueType>(ref valueType value)
        {
            TypeParser<valueType>.Parse(this, ref value);
            return State == ParseState.Success;
        }
        /// <summary>
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType TypeParse<valueType>()
        {
            valueType value = default(valueType);
            TypeParser<valueType>.Parse(this, ref value);
            return State == ParseState.Success ? value : default(valueType);
        }
        /// <summary>
        /// 自定义序列化重置当前读取数据位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool VerifyRead(int size)
        {
            if ((current += size) <= end) return true;
            State = ParseState.Custom;
            return false;
        }
        /// <summary>
        /// 移动当前读取数据位置，负数表示自定义序列化失败
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool MoveRead(int size)
        {
            if (size >= 0)
            {
                if ((current += size) <= end) return true;
            }
            if (State == ParseState.Success) State = ParseState.Custom;
            return false;
        }
        /// <summary>
        /// 释放XML解析器
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void free()
        {
            xml = null;
            Config = null;
            ItemName = null;
            MemberMap = null;
            anonymousTypes.SetNull();
            YieldPool.Default.PushNotNull(this);
        }
        /// <summary>
        /// 空格过滤
        /// </summary>
        private void space()
        {
        START:
            while (((bits[*(byte*)current] & spaceBit) | *(((byte*)current) + 1)) == 0) ++current;
            if (*(long*)current == '<' + ('!' << 16) + ((long)'-' << 32) + ((long)'-' << 48))
            {
                current += 6;
                do
                {
                    if (*current == '>')
                    {
                        if (current > end)
                        {
                            State = ParseState.CrashEnd;
                            return;
                        }
                        if (*(int*)(current - 2) == '-' + ('-' << 16))
                        {
                            ++current;
                            goto START;
                        }
                        current += 3;
                    }
                    else ++current;
                }
                while (true);
            }
        }
        /// <summary>
        /// 空格过滤
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private char* endSpace()
        {
            char* end = current;
            while (((bits[*(byte*)--end] & spaceBit) | *(((byte*)end) + 1)) == 0) ;
            return end + 1;
        }
        /// <summary>
        /// 查找数据起始位置
        /// </summary>
        private void searchValue()
        {
            space();
            if (State == ParseState.Success)
            {
                if (*current == '<')
                {
                    switch (*(current + 1))
                    {
                        case '/':
                            IsCData = 0;
                            return;
                        case '!':
                            if (((*(int*)(current + 2) ^ ('[' + ('C' << 16))) | (*(int*)(current + 4) ^ ('D' + ('A' << 16))) | (*(int*)(current + 6) ^ ('T' + ('A' << 16))) | (*(short*)(current + 8) ^ '[')) == 0)
                            {
                                current += 9;
                                IsCData = 1;
                                return;
                            }
                            break;
                    }
                    State = ParseState.NotFoundValue;
                    IsCData = 2;
                }
                else IsCData = 0;
            }
            else IsCData = 2;
        }
        /// <summary>
        /// 数据结束处理
        /// </summary>
        internal void SearchValueEnd()
        {
            switch (IsCData)
            {
                case 0:
                SPACE:
                    space();
                    if (*current == '<' || State != ParseState.Success) return;
                    break;
                case 1:
                    if (((*(int*)current ^ (']' + (']' << 16))) | (*(short*)(current + 2) ^ '>')) == 0)
                    {
                        current += 3;
                        goto SPACE;
                    }
                    break;
            }
            State = ParseState.NotFoundValueEnd;
        }
        /// <summary>
        /// 获取文本数据
        /// </summary>
        private void getValue()
        {
            space();
            if (State != ParseState.Success) return;
            if (*current == '<')
            {
                switch (*(current + 1))
                {
                    case '/':
                        valueStart = current;
                        IsCData = 1;
                        valueSize = 0;
                        return;
                    case '!':
                        if (((*(int*)(current + 2) ^ ('[' + ('C' << 16))) | (*(int*)(current + 4) ^ ('D' + ('A' << 16))) | (*(int*)(current + 6) ^ ('T' + ('A' << 16))) | (*(short*)(current + 8) ^ '[')) == 0)
                        {
                            valueStart = current + 9;
                            current += 11;
                            do
                            {
                                if (*current == '>')
                                {
                                    char* cDataValueEnd = current - 2;
                                    if (*(int*)cDataValueEnd == (']' + (']' << 16)))
                                    {
                                        ++current;
                                        valueSize = (int)(cDataValueEnd - valueStart);
                                        IsCData = 1;
                                        return;
                                    }
                                    else if (current < end) current += 3;
                                    else
                                    {
                                        State = ParseState.CrashEnd;
                                        return;
                                    }
                                }
                                else ++current;
                            }
                            while (true);
                        }
                        break;
                }
                State = ParseState.NotFoundValue;
                return;
            }
            valueStart = current;
            while (*++current != '<') ;
            valueSize = (int)(endSpace() - valueStart);
            IsCData = 0;
            return;
        }
        /// <summary>
        /// 数据结束处理
        /// </summary>
        private void getValueEnd()
        {
            if (IsCData != 0)
            {
                space();
                if (*current != '<') State = ParseState.NotFoundValueEnd;
            }
        }
        /// <summary>
        /// 查找CDATA数据结束位置
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void searchCData2()
        {
            if (((*(int*)(current + 2) ^ ('[' + ('C' << 16))) | (*(int*)(current + 4) ^ ('D' + ('A' << 16))) | (*(int*)(current + 6) ^ ('T' + ('A' << 16))) | (*(short*)(current + 8) ^ '[')) == 0)
            {
                current += 9;
                SearchCDataValue();
            }
            else State = ParseState.NotFoundCDATAStart;
        }
        /// <summary>
        /// 查找CDATA数据结束位置
        /// </summary>
        internal void SearchCDataValue()
        {
            valueStart = current;
            current += 2;
            do
            {
                if (*current == '>')
                {
                    char* valueEnd = current - 2;
                    if (*(int*)valueEnd == (']' + (']' << 16)))
                    {
                        ++current;
                        valueSize = (int)(valueEnd - valueStart);
                        return;
                    }
                    else if (current < end) current += 3;
                    else
                    {
                        State = ParseState.CrashEnd;
                        return;
                    }
                }
                else ++current;
            }
            while (true);
        }
        /// <summary>
        /// 忽略数据
        /// </summary>
        /// <returns>是否成功</returns>
        internal int IgnoreValue()
        {
            LeftArray<Pointer.Size> PointerArray = default(LeftArray<Pointer.Size>);
        START:
            space();
            if (State != ParseState.Success) return 0;
            if (*current == '<')
            {
                char code = *(current + 1);
                if (((bits[code & 0xff] & targetStartCheckBit) | (code & 0xff00)) == 0)
                {
                    if (code == '/') goto CHECK;
                    if (code == '!')
                    {
                        if (((*(int*)(current + 2) ^ ('[' + ('C' << 16))) | (*(int*)(current + 4) ^ ('D' + ('A' << 16))) | (*(int*)(current + 6) ^ ('T' + ('A' << 16))) | (*(short*)(current + 8) ^ '[')) == 0)
                        {
                            current += 11;
                            do
                            {
                                if (*current == '>')
                                {
                                    if (*(int*)(current - 2) == (']' + (']' << 16)))
                                    {
                                        ++current;
                                        goto CHECK;
                                    }
                                    else if (current < end) current += 3;
                                    else
                                    {
                                        State = ParseState.CrashEnd;
                                        return 0;
                                    }
                                }
                                else ++current;
                            }
                            while (true);
                        }
                    }
                    State = ParseState.NotFoundTagStart;
                    return 0;
                }
                int nameSize = 0;
                char* nameStart = getNameOnly(ref nameSize);
                if (nameStart == null) return 0;
                if (isTagEnd == 0) PointerArray.Add(new Pointer.Size { Data = nameStart, ByteSize = nameSize });
                goto START;
            }
            while (*++current != '<') ;
            CHECK:
            if (PointerArray.Length != 0)
            {
                Pointer.Size name = PointerArray.UnsafePop();
                if (CheckNameEnd(name.Char, name.ByteSize) == 0) return 0;
                goto START;
            }
            return 1;
        }
        /// <summary>
        /// 是否非数字 NaN
        /// </summary>
        /// <returns></returns>

        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool isNaN()
        {
            return *valueStart == 'N' && *(int*)(valueStart + 1) == 'a' + ('N' << 16);
        }
        /// <summary>
        /// 是否 Infinity
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool isPositiveInfinity()
        {
            return *valueStart == 'I' && ((*(long*)valueStart ^ ('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48))) | (*(long*)(valueStart + 4) ^ ('n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48)))) == 0;
        }
        /// <summary>
        /// 是否 -Infinity
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool isNegativeInfinity()
        {
            return *valueStart == '-' && ((*(long*)(valueStart + 1) ^ ('I' + ('n' << 16) + ((long)'f' << 32) + ((long)'i' << 48))) | (*(long*)(valueStart + 5) ^ ('n' + ('i' << 16) + ((long)'t' << 32) + ((long)'y' << 48)))) == 0;
        }
        /// <summary>
        /// 获取节点名称
        /// </summary>
        /// <param name="nameSize">节点名称长度</param>
        /// <returns></returns>
        private char* getName(ref int nameSize)
        {
            space();
            if (State != ParseState.Success) return null;
            if (*current == '<')
            {
                char code = *(current + 1);
                if (((bits[code & 0xff] & targetStartCheckBit) | (code & 0xff00)) == 0)
                {
                    if (code == '/') return null;
                }
                else return getNameOnly(ref nameSize);
            }
            State = ParseState.NotFoundTagStart;
            return null;
        }
        /// <summary>
        /// 获取节点名称
        /// </summary>
        /// <param name="nameSize">节点名称长度</param>
        /// <returns></returns>
        private char* getNameOnly(ref int nameSize)
        {
            char* nameStart = ++current;
            do
            {
                if (((bits[*(byte*)++current] & spaceBit) | *(((byte*)current) + 1)) != 0)
                {
                    if (*current == '>')
                    {
                        if (current < end)
                        {
                            isTagEnd = 0;
                            attributes.Length = 0;
                            nameSize = (int)(current++ - nameStart);
                            return nameStart;
                        }
                        State = ParseState.CrashEnd;
                        return null;
                    }
                    if (*current == '/')
                    {
                        if (*(current + 1) == '>')
                        {
                            nameSize = (int)(current - nameStart);
                            isTagEnd = 1;
                            attributes.Length = 0;
                            current += 2;
                            return nameStart;
                        }
                        State = ParseState.NotFoundTagStart;
                        return null;
                    }
                }
                else
                {
                    nameSize = (int)(current++ - nameStart);
                    attribute();
                    return State == ParseState.Success ? nameStart : null;
                }
            }
            while (true);
        }
        /// <summary>
        /// 获取节点名称
        /// </summary>
        /// <param name="nameSize">节点名称长度</param>
        /// <param name="isTagEnd">名称解析节点是否结束</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal char* GetName(ref int nameSize, ref byte isTagEnd)
        {
            char* nameStart = getName(ref nameSize);
            isTagEnd = this.isTagEnd;
            return nameStart;
        }
        /// <summary>
        /// 节点名称结束检测
        /// </summary>
        /// <param name="nameStart"></param>
        /// <param name="nameSize"></param>
        /// <returns></returns>
        internal int CheckNameEnd(char* nameStart, int nameSize)
        {
            space();
            if (State == ParseState.Success)
            {
                if (*(int*)current == '<' + ('/' << 16) && *(current + (2 + nameSize)) == '>' && AutoCSer.Memory.SimpleEqualNotNull((byte*)(current + 2), (byte*)nameStart, nameSize << 1) && current != end)
                {
                    current += nameSize + 3;
                    return 1;
                }
                State = ParseState.NotFoundTagEnd;
            }
            return 0;
        }
        /// <summary>
        /// 属性解析
        /// </summary>
        private void attribute()
        {
            attributes.Length = 0;
            while (((bits[*(byte*)current] & spaceBit) | *(((byte*)current) + 1)) == 0) ++current;
            if (*current == '>')
            {
                if (current++ <= end) isTagEnd = 0;
                else State = ParseState.CrashEnd;
                return;
            }
            if (*current == '/')
            {
                if (*(current + 1) == '>')
                {
                    isTagEnd = 1;
                    current += 2;
                }
                else State = ParseState.NotFoundTagStart;
                return;
            }
            attributeName();
        }
        /// <summary>
        /// 属性名称解析
        /// </summary>
        private void attributeName()
        {
        NAME:
            attributeNameStartIndex = (int)(current - xmlFixed);
            do
            {
                if (((bits[*(byte*)++current] & attributeNameSearchBit) | *(((byte*)current) + 1)) == 0)
                {
                    switch (*current & 7)
                    {
                        case '\t' & 7:
                        case ' ' & 7:
                        case '\n' & 7:
                        SPACE:
                            attributeNameEndIndex = (int)(current - xmlFixed);
                            while (((bits[*(byte*)++current] & spaceBit) | *(((byte*)current) + 1)) == 0) ;
                            if (*current == '=')
                            {
                                if (attributeValue() == 0) return;
                                goto NAME;
                            }
                            break;
                        case '=' & 7:
                            if (*current == '=')
                            {
                                attributeNameEndIndex = (int)(current - xmlFixed);
                                if (attributeValue() == 0) return;
                                goto NAME;
                            }
                            goto SPACE;
                    }
                    State = ParseState.NotFoundAttributeName;
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// 属性值解析
        /// </summary>
        /// <returns></returns>
        private int attributeValue()
        {
            while (((bits[*(byte*)++current] & spaceBit) | *(((byte*)current) + 1)) == 0) ;
            if (*current == '"')
            {
                int valueStartIndex = (int)(++current - xmlFixed);
                do
                {
                    if (*current == '"')
                    {
                        if (Config.IsAttribute)
                        {
                            attributes.Add(new KeyValue<Range, Range>(new Range(attributeNameStartIndex, attributeNameEndIndex), new Range(valueStartIndex, (int)(current - xmlFixed))));
                        }
                        while (((bits[*(byte*)++current] & spaceBit) | *(((byte*)current) + 1)) == 0) ;
                        if (*current == '>')
                        {
                            if (current++ <= end) isTagEnd = 0;
                            else State = ParseState.CrashEnd;
                            return 0;
                        }
                        if (*current == '/')
                        {
                            if (*(current + 1) == '>')
                            {
                                isTagEnd = 1;
                                current += 2;
                            }
                            else State = ParseState.NotFoundTagStart;
                            return 0;
                        }
                        return 1;
                    }
                    if (*current == '<' && current >= end)
                    {
                        State = ParseState.NotFoundAttributeValue;
                        return 0;
                    }
                    ++current;
                }
                while (true);
            }
            State = ParseState.NotFoundAttributeValue;
            return 0;
        }
        /// <summary>
        /// 读取下一个枚举字符
        /// </summary>
        /// <returns>枚举字符,结束或者错误返回0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal char NextEnumChar()
        {
            if (((bits[*(byte*)current] & spaceBit) | *(((byte*)current) + 1)) != 0)
            {
                if (*current == '<') return (char)0;
                if (*current == '&')
                {
                    char value = (char)0;
                    decodeChar(ref value);
                    return value;
                }
                return *current++;
            }
            State = ParseState.NotEnumChar;
            return (char)0;
        }
        /// <summary>
        /// 读取下一个枚举字符
        /// </summary>
        /// <returns>枚举字符,结束或者错误返回0</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal char NextCDataEnumChar()
        {
            if (valueSize == 0) return (char)0;
            --valueSize;
            return *valueStart++;
        }
        /// <summary>
        /// 枚举值是否结束
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int IsNextFlagEnum()
        {
            if (State == ParseState.Success)
            {
                if (IsCData == 0)
                {
                START:
                    switch ((*current >> 3) & 3)
                    {
                        case (' ' >> 3) & 3:
                        case (',' >> 3) & 3:
                            if (*current == ',' || *current == ' ')
                            {
                                ++current;
                                goto START;
                            }
                            return 1;
                        case ('<' >> 3) & 3:
                            return *current - '<';
                        default:
                            return 1;
                    }
                }
                else
                {
                    while (valueSize != 0)
                    {
                        if (*valueStart == ',' || *valueStart == ' ')
                        {
                            --valueSize;
                            ++valueStart;
                            continue;
                        }
                        return 1;
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsEnumNumber()
        {
            searchValue();
            return (uint)(*current - '0') < 10;
        }
        /// <summary>
        /// 忽略数据
        /// </summary>
        internal void IgnoreSearchValue()
        {
            if (IsCData == 0)
            {
                while (*current != '<') ++current;
            }
            else
            {
                current += 2;
                do
                {
                    if (*current == '>')
                    {
                        if (*(int*)(current - 2) == (']' + (']' << 16)))
                        {
                            ++current;
                            space();
                            if (*current != '<') State = ParseState.NotFoundValueEnd;
                            return;
                        }
                        else if (current < end) current += 3;
                        else
                        {
                            State = ParseState.CrashEnd;
                            return;
                        }
                    }
                    else ++current;
                }
                while (true);
            }
        }
        /// <summary>
        /// 查找枚举数字
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool IsEnumNumberFlag()
        {
            searchValue();
            uint number = (uint)(*current - '0');
            return number < 10 || (int)number == '-' - '0';
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="names"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool IsName(byte* names, ref int index)
        {
            int length = *(short*)names;
            if (length == 0)
            {
                index = -1;
                return true;
            }
            else if (AutoCSer.Memory.SimpleEqualNotNull((byte*)current, names += sizeof(short), length))
            {
                current = (char*)((byte*)current + length);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否匹配默认顺序名称
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        internal byte IsNameEnd(byte* names)
        {
            if (*(int*)current == '<' + ('/' << 16))
            {
                int length = *(short*)names - sizeof(char);
                if (AutoCSer.Memory.SimpleEqualNotNull((byte*)current + sizeof(int), names + (sizeof(short) + sizeof(char)), length) && current != end)
                {
                    current = (char*)((byte*)current + (length + sizeof(int)));
                    return 1;
                }
            }
            return 0;
        }
        /// <summary>
        /// 是否存在数组数据
        /// </summary>
        /// <param name="nameStart"></param>
        /// <param name="nameSize"></param>
        /// <returns></returns>
        internal int IsArrayItem(char* nameStart, int nameSize)
        {
            if (*(int*)current == '<' + ('/' << 16) && *(current + (2 + nameSize)) == '>' && AutoCSer.Memory.SimpleEqualNotNull((byte*)(current + 2), (byte*)nameStart, nameSize << 1) && current != end)
            {
                current += nameSize + 3;
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 解析16进制数字
        /// </summary>
        /// <param name="value">数值</param>
        private void parseHex32(ref uint value)
        {
            uint isValue = 0;
            do
            {
                uint number = (uint)(*current - '0');
                if (number > 9)
                {
                    if ((number = (number - ('A' - '0')) & 0xffdfU) > 5)
                    {
                        if (isValue == 0) State = ParseState.NotHex;
                        return;
                    }
                    number += 10;
                }
                ++current;
                value <<= 4;
                isValue = 1;
                value += number;
            }
            while (true);
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private uint parseUInt32(uint value)
        {
            uint number;
            while ((number = (uint)(*current - '0')) < 10)
            {
                value *= 10;
                ++current;
                value += (byte)number;
            }
            return value;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void ParseNumber(ref byte value)
        {
            if (IsCData != 2)
            {
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = (byte)number;
                        }
                        else value = 0;
                    }
                    else value = (byte)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void ParseNumber(ref sbyte value)
        {
            if (IsCData != 2)
            {
                if ((sign = *current) == '-') ++current;
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = sign == '-' ? (sbyte)-(int)number : (sbyte)(byte)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? (sbyte)-(int)parseUInt32(number) : (sbyte)(byte)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void ParseNumber(ref ushort value)
        {
            if (IsCData != 2)
            {
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = (ushort)number;
                        }
                        else value = 0;
                    }
                    else value = (ushort)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void ParseNumber(ref short value)
        {
            if (IsCData != 2)
            {
                if ((sign = *current) == '-') ++current;
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = sign == '-' ? (short)-(int)number : (short)(ushort)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? (short)-(int)parseUInt32(number) : (short)(ushort)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void ParseNumber(ref uint value)
        {
            if (IsCData != 2)
            {
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = number;
                        }
                        else value = 0;
                    }
                    else value = parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void ParseNumber(ref int value)
        {
            if (IsCData != 2)
            {
                if ((sign = *current) == '-') ++current;
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            parseHex32(ref number);
                            if (State != ParseState.Success) return;
                            value = sign == '-' ? -(int)number : (int)number;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? -(int)parseUInt32(number) : (int)parseUInt32(number);
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 解析16进制数字
        /// </summary>
        private ulong parseHex64()
        {
            ulong number = 0;
            uint isValue = 0, value;
            do
            {
                if ((value = (uint)(*current - '0')) > 9)
                {
                    if ((value = (value - ('A' - '0')) & 0xffdfU) > 5)
                    {
                        if (isValue == 0) State = ParseState.NotHex;
                        return 0;
                    }
                    value += 10;
                }
                ++current;
                number <<= 4;
                isValue = 1;
                number += value;
            }
            while (true);
        }
        /// <summary>
        /// 解析10进制数字
        /// </summary>
        /// <param name="value">第一位数字</param>
        /// <returns>数字</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private ulong parseUInt64(uint value)
        {
            ulong number = value;
            while ((value = (uint)(*current - '0')) < 10)
            {
                number *= 10;
                ++current;
                number += value;
            }
            return number;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void ParseNumber(ref ulong value)
        {
            if (IsCData != 2)
            {
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            value = parseHex64();
                            if (State != ParseState.Success) return;
                        }
                        else value = 0;
                    }
                    else value = parseUInt64(number);
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        internal void ParseNumber(ref long value)
        {
            if (IsCData != 2)
            {
                if ((sign = *current) == '-') ++current;
                uint number = (uint)(*current - '0');
                if (number < 10)
                {
                    ++current;
                    if (number == 0)
                    {
                        if (*current == 'x')
                        {
                            ++current;
                            value = sign == '-' ? -(long)parseHex64() : (long)parseHex64();
                            if (State != ParseState.Success) return;
                        }
                        else value = 0;
                    }
                    else value = sign == '-' ? -(long)parseUInt64(number) : (long)parseUInt64(number);
                    SearchValueEnd();
                    return;
                }
                State = ParseState.NotNumber;
            }
        }
        /// <summary>
        /// 字符解码
        /// </summary>
        /// <param name="value"></param>
        private void decodeChar(ref char value)
        {
            if (*++valueStart == '#')
            {
                uint code = (uint)(*++valueStart - '0');
                if (code < 10)
                {
                    do
                    {
                        uint number = (uint)(*++valueStart - '0');
                        if (number < 10) code = code * 10 + number;
                        else
                        {
                            if (number == ';' - '0')
                            {
                                ++valueStart;
                                value = (char)code;
                            }
                            else State = ParseState.DecodeError;
                            return;
                        }
                    }
                    while (true);
                }
            }
            else
            {
                int code = decodeSearcher.UnsafeSearch(ref valueStart);
                if (code > 0)
                {
                    value = (char)code;
                    return;
                }
            }
            State = ParseState.DecodeError;
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private uint parseHex2()
        {
            uint code = (uint)(*valueStart++ - '0'), number = (uint)(*valueStart++ - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            return (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number) + (code << 4);
        }
        /// <summary>
        /// 解析16进制字符
        /// </summary>
        /// <returns>字符</returns>
        private uint parseHex4()
        {
            uint code = (uint)(*valueStart++ - '0'), number = (uint)(*valueStart++ - '0');
            if (code > 9) code = ((code - ('A' - '0')) & 0xffdfU) + 10;
            if (number > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code <<= 12;
            code += (number << 8);
            if ((number = (uint)(*valueStart++ - '0')) > 9) number = ((number - ('A' - '0')) & 0xffdfU) + 10;
            code += (number << 4);
            number = (uint)(*valueStart++ - '0');
            return code + (number > 9 ? (((number - ('A' - '0')) & 0xffdfU) + 10) : number);
        }
        /// <summary>
        /// Guid解析
        /// </summary>
        /// <param name="value">数据</param>
        private void parse(ref GuidCreator value)
        {
            value.Byte3 = (byte)parseHex2();
            value.Byte2 = (byte)parseHex2();
            value.Byte1 = (byte)parseHex2();
            value.Byte0 = (byte)parseHex2();
            if (*valueStart++ != '-')
            {
                State = ParseState.NotGuid;
                return;
            }
            value.Byte45 = (ushort)parseHex4();
            if (*valueStart++ != '-')
            {
                State = ParseState.NotGuid;
                return;
            }
            value.Byte67 = (ushort)parseHex4();
            if (*valueStart++ != '-')
            {
                State = ParseState.NotGuid;
                return;
            }
            value.Byte8 = (byte)parseHex2();
            value.Byte9 = (byte)parseHex2();
            if (*valueStart++ != '-')
            {
                State = ParseState.NotGuid;
                return;
            }
            value.Byte10 = (byte)parseHex2();
            value.Byte11 = (byte)parseHex2();
            value.Byte12 = (byte)parseHex2();
            value.Byte13 = (byte)parseHex2();
            value.Byte14 = (byte)parseHex2();
            value.Byte15 = (byte)parseHex2();
        }
        /// <summary>
        /// 字符串解码
        /// </summary>
        /// <param name="write"></param>
        /// <param name="writeEnd"></param>
        private void decodeString(char* write, char* writeEnd)
        {
            char decodeValue = (char)0;
            do
            {
                if (*valueStart == '&')
                {
                    decodeChar(ref decodeValue);
                    if (State != ParseState.Success) return;
                    *write = decodeValue;
                }
                else *write = *valueStart++;
            }
            while (++write != writeEnd);
        }
        /// <summary>
        /// 判断否存存在数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int IsValue()
        {
            space();
            return State == ParseState.Success ? *(int*)current ^ ('<' + ('/' << 16)) : 0;
        }

        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CustomParse(ref uint value)
        {
            Parse(ref value);
            return State == ParseState.Success;
        }
        /// <summary>
        /// 数字解析
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CustomParse(ref uint? value)
        {
            Parse(ref value);
            return State == ParseState.Success;
        }
        /// <summary>
        /// 字符串解析
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CustomParse(ref string value)
        {
            Parse(ref value);
            return State == ParseState.Success;
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CustomEnumByte<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumByte.Parse(this, ref value);
            return State == ParseState.Success;
        }
        /// <summary>
        /// 忽略数据
        /// </summary>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool CustomIgnoreValue()
        {
            return IgnoreValue() != 0;
        }

        /// <summary>
        /// 找不到构造函数
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="isAnonymousType"></param>
        internal void CheckNoConstructor<valueType>(ref valueType value, bool isAnonymousType)
        {
            Func<Type, object> constructor = Config.Constructor;
            if (constructor == null)
            {
                if (isAnonymousType)
                {
                    foreach (KeyValue<Type, object> type in anonymousTypes)
                    {
                        if (type.Key == typeof(valueType))
                        {
                            value = AutoCSer.MemberCopy.Copyer<valueType>.MemberwiseClone((valueType)type.Value);
                            return;
                        }
                    }
                }
                IgnoreValue();
                return;
            }
            object newValue = constructor(typeof(valueType));
            if (newValue == null)
            {
                IgnoreValue();
                return;
            }
            value = (valueType)newValue;
        }
        /// <summary>
        /// 设置匿名类型数据
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        internal void SetAnonymousType<valueType>(valueType value)
        {
            foreach (KeyValue<Type, object> type in anonymousTypes)
            {
                if (type.Key == typeof(valueType)) return;
            }
            anonymousTypes.Add(new KeyValue<Type, object>(typeof(valueType), AutoCSer.MemberCopy.Copyer<valueType>.MemberwiseClone(value)));
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void typeParse<valueType>(ref valueType value)
        {
            TypeParser<valueType>.ParseClass(this, ref value);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structParse<valueType>(ref valueType value) where valueType : struct
        {
            TypeParser<valueType>.ParseStruct(this, ref value);
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <param name="value">目标数据</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private void baseParse<valueType, childType>(ref childType value) where childType : valueType
        {
            if (value == null)
            {
                Func<childType> constructor = AutoCSer.Emit.Constructor<childType>.New;
                if (constructor == null)
                {
                    CheckNoConstructor(ref value, false);
                    if (value == null) return;
                }
                else value = constructor();
                valueType newValue = value;
                TypeParser<valueType>.ParseMembers(this, ref newValue);
            }
            else
            {
                valueType newValue = value;
                TypeParser<valueType>.ParseClass(this, ref newValue);
            }
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="values">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void array<valueType>(ref valueType[] values)
        {
            TypeParser<valueType>.Array(this, ref values);
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableEnumParse<valueType>(ref Nullable<valueType> value) where valueType : struct
        {
            space();
            if (State == ParseState.Success)
            {
                if (*current == '<')
                {
                    if (((*(int*)(current + 1) ^ ('!' + ('[' << 16))) | (*(int*)(current + 3) ^ ('C' + ('D' << 16))) | (*(int*)(current + 5) ^ ('A' + ('T' << 16))) | (*(int*)(current + 7) ^ ('A' + ('[' << 16)))) == 0)
                    {
                        if (((*(int*)(current + 9) ^ (']' + (']' << 16))) | (*(short*)(current + 11) ^ '>')) == 0)
                        {
                            current += 12;
                            value = null;
                            return;
                        }
                    }
                    else
                    {
                        value = null;
                        return;
                    }
                }
                valueType newValue = value.HasValue ? value.Value : default(valueType);
                TypeParser<valueType>.DefaultParser(this, ref newValue);
                value = newValue;
            }
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableParse<valueType>(ref Nullable<valueType> value) where valueType : struct
        {
            valueType newValue = value.HasValue ? value.Value : default(valueType);
            TypeParser<valueType>.Parse(this, ref newValue);
            value = newValue;
        }
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void keyValuePairParse<keyType, valueType>(ref KeyValuePair<keyType, valueType> value)
        {
            KeyValue<keyType, valueType> keyValue = new KeyValue<keyType, valueType>(value.Key, value.Value);
            TypeParser<KeyValue<keyType, valueType>>.ParseMembers(this, ref keyValue);
            value = new KeyValuePair<keyType, valueType>(keyValue.Key, keyValue.Value);
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void listConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeParser<argumentType>.ArrayIndex(this, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.ListConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void collectionConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeParser<argumentType>.ArrayIndex(this, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.CollectionConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumerableConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            int count = TypeParser<argumentType>.ArrayIndex(this, ref values);
            if (count == -1) value = default(valueType);
            else value = Emit.EnumerableConstructor<valueType, argumentType>.Constructor(new LeftArray<argumentType> { Array = values, Length = count });
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void arrayConstructor<valueType, argumentType>(ref valueType value)
        {
            argumentType[] values = null;
            TypeParser<argumentType>.Array(this, ref values);
            if (State == ParseState.Success)
            {
                if (values == null) value = default(valueType);
                else value = Emit.ArrayConstructor<valueType, argumentType>.Constructor(values);
            }
        }
        /// <summary>
        /// 集合构造函数解析
        /// </summary>
        /// <param name="value">目标数据</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void dictionaryConstructor<dictionaryType, keyType, valueType>(ref dictionaryType value)
        {
            KeyValuePair<keyType, valueType>[] values = null;
            int count = TypeParser<KeyValuePair<keyType, valueType>>.ArrayIndex(this, ref values);
            if (count == -1) value = default(dictionaryType);
            else
            {
                Dictionary<keyType, valueType> dictionary = AutoCSer.DictionaryCreator.CreateAny<keyType, valueType>(count);
                if (count != 0)
                {
                    foreach (KeyValuePair<keyType, valueType> keyValue in values)
                    {
                        dictionary.Add(keyValue.Key, keyValue.Value);
                        if (--count == 0) break;
                    }
                }
                value = Emit.DictionaryConstructor<dictionaryType, keyType, valueType>.Constructor(dictionary);
            }
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumByte<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumByte.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumSByte<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumSByte.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumShort<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumShort.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUShort<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumUShort.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumInt<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumInt.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUInt<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumUInt.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumLong<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumLong.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumULong<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumULong.Parse(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumByteFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumByte.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumSByteFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumSByte.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumShortFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumShort.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUShortFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumUShort.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumIntFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumInt.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumUIntFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumUInt.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumLongFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumLong.ParseFlags(this, ref value);
        }
        /// <summary>
        /// 枚举值解析
        /// </summary>
        /// <param name="value">目标数据</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumULongFlags<valueType>(ref valueType value)
        {
            TypeParser<valueType>.EnumULong.ParseFlags(this, ref value);
        }

        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult Parse<valueType>(SubString xml, ref valueType value, ParseConfig config = null)
        {
            return Parse(ref xml, ref value, config);
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Parse<valueType>(SubString xml, ParseConfig config = null)
        {
            valueType value = default(valueType);
            return Parse(ref xml, ref value, config).State == ParseState.Success ? value : default(valueType);
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult Parse<valueType>(ref SubString xml, ref valueType value, ParseConfig config = null)
        {
            if (xml.Length == 0) return new ParseResult { State = ParseState.NullXml };
            Parser parser = YieldPool.Default.Pop() ?? new Parser();
            try
            {
                return parser.parse<valueType>(ref xml, ref value, config);
            }
            finally { parser.free(); }
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Parse<valueType>(ref SubString xml, ParseConfig config = null)
        {
            valueType value = default(valueType);
            return Parse(ref xml, ref value, config).State == ParseState.Success ? value : default(valueType);
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult Parse<valueType>(string xml, ref valueType value, ParseConfig config = null)
        {
            if (string.IsNullOrEmpty(xml)) return new ParseResult { State = ParseState.NullXml };
            Parser parser = YieldPool.Default.Pop() ?? new Parser();
            try
            {
                return parser.parse<valueType>(xml, ref value, config);
            }
            finally { parser.free(); }
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static valueType Parse<valueType>(string xml, ParseConfig config = null)
        {
            valueType value = default(valueType);
            return Parse(xml, ref value, config).State == ParseState.Success ? value : default(valueType);
        }

        /// <summary>
        /// XML解析空格[ ,\t,\r,\n]
        /// </summary>
        private const byte spaceBit = 128;
        /// <summary>
        /// XML解析名称检测
        /// </summary>
        private const byte targetStartCheckBit = 64;
        /// <summary>
        /// XML解析属性名称查找
        /// </summary>
        private const byte attributeNameSearchBit = 32;
        /// <summary>
        /// XML序列化转换字符[ ,\t,\r,\n,&amp;,>,&lt;]
        /// </summary>
        internal const byte EncodeSpaceBit = 8;
        /// <summary>
        /// XML序列化转换字符[&amp;,>,&lt;]
        /// </summary>
        internal const byte EncodeBit = 4;
        /// <summary>
        /// 字符状态位
        /// </summary>
        internal static readonly Pointer Bits;
        /// <summary>
        /// 字符 Decode 转码
        /// </summary>
        private static readonly AutoCSer.StateSearcher.AsciiSearcher decodeSearcher;

        static Parser()
        {
            byte* bits = (byte*)Unmanaged.GetStatic64(256, false);
            Bits = new Pointer { Data = bits };
            AutoCSer.Memory.Fill((ulong*)bits, ulong.MaxValue, 256 >> 3);
            bits['\t'] &= (spaceBit | targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['\r'] &= (spaceBit | targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['\n'] &= (spaceBit | targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits[' '] &= (spaceBit | targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['/'] &= (targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['!'] &= targetStartCheckBit ^ 255;
            bits['<'] &= targetStartCheckBit ^ 255;
            bits['>'] &= (targetStartCheckBit | attributeNameSearchBit) ^ 255;
            bits['='] &= attributeNameSearchBit ^ 255;

            parseMethods = AutoCSer.DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(Parser).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(ParseMethod), false))
                {
                    parseMethods.Add(method.GetParameters()[0].ParameterType.GetElementType(), method);
                }
            }

            KeyValue<string, int>[] chars = new KeyValue<string, int>[]
            {
                new KeyValue<string, int>("AElig;", 198)
                , new KeyValue<string, int>("Aacute;", 193)
                , new KeyValue<string, int>("Acirc;", 194)
                , new KeyValue<string, int>("Agrave;", 192)
                , new KeyValue<string, int>("Alpha;", 913)
                , new KeyValue<string, int>("Aring;", 197)
                , new KeyValue<string, int>("Atilde;", 195)
                , new KeyValue<string, int>("Auml;", 196)
                , new KeyValue<string, int>("Beta;", 914)
                , new KeyValue<string, int>("Ccedil;", 199)
                , new KeyValue<string, int>("Chi;", 935)
                , new KeyValue<string, int>("Dagger;", 8225)
                , new KeyValue<string, int>("Delta;", 916)
                , new KeyValue<string, int>("ETH;", 208)
                , new KeyValue<string, int>("Eacute;", 201)
                , new KeyValue<string, int>("Ecirc;", 202)
                , new KeyValue<string, int>("Egrave;", 200)
                , new KeyValue<string, int>("Epsilon;", 917)
                , new KeyValue<string, int>("Eta;", 919)
                , new KeyValue<string, int>("Euml;", 203)
                , new KeyValue<string, int>("Gamma;", 915)
                , new KeyValue<string, int>("Iacute;", 205)
                , new KeyValue<string, int>("Icirc;", 206)
                , new KeyValue<string, int>("Igrave;", 204)
                , new KeyValue<string, int>("Iota;", 921)
                , new KeyValue<string, int>("Iuml;", 207)
                , new KeyValue<string, int>("Kappa;", 922)
                , new KeyValue<string, int>("Lambda;", 923)
                , new KeyValue<string, int>("Mu;", 924)
                , new KeyValue<string, int>("Ntilde;", 209)
                , new KeyValue<string, int>("Nu;", 925)
                , new KeyValue<string, int>("OElig;", 338)
                , new KeyValue<string, int>("Oacute;", 211)
                , new KeyValue<string, int>("Ocirc;", 212)
                , new KeyValue<string, int>("Ograve;", 210)
                , new KeyValue<string, int>("Omega;", 937)
                , new KeyValue<string, int>("Omicron;", 927)
                , new KeyValue<string, int>("Oslash;", 216)
                , new KeyValue<string, int>("Otilde;", 213)
                , new KeyValue<string, int>("Ouml;", 214)
                , new KeyValue<string, int>("Phi;", 934)
                , new KeyValue<string, int>("Pi;", 928)
                , new KeyValue<string, int>("Prime;", 8243)
                , new KeyValue<string, int>("Psi;", 936)
                , new KeyValue<string, int>("Rho;", 929)
                , new KeyValue<string, int>("Scaron;", 352)
                , new KeyValue<string, int>("Sigma;", 931)
                , new KeyValue<string, int>("THORN;", 222)
                , new KeyValue<string, int>("Tau;", 932)
                , new KeyValue<string, int>("Theta;", 920)
                , new KeyValue<string, int>("Uacute;", 218)
                , new KeyValue<string, int>("Ucirc;", 219)
                , new KeyValue<string, int>("Ugrave;", 217)
                , new KeyValue<string, int>("Upsilon;", 933)
                , new KeyValue<string, int>("Uuml;", 220)
                , new KeyValue<string, int>("Xi;", 926)
                , new KeyValue<string, int>("Yacute;", 221)
                , new KeyValue<string, int>("Yuml;", 376)
                , new KeyValue<string, int>("Zeta;", 918)
                , new KeyValue<string, int>("aacute;", 225)
                , new KeyValue<string, int>("acirc;", 226)
                , new KeyValue<string, int>("acute;", 180)
                , new KeyValue<string, int>("aelig;", 230)
                , new KeyValue<string, int>("agrave;", 224)
                , new KeyValue<string, int>("alefsym;", 8501)
                , new KeyValue<string, int>("alpha;", 945)
                , new KeyValue<string, int>("amp;", 38)
                , new KeyValue<string, int>("and;", 8743)
                , new KeyValue<string, int>("ang;", 8736)
                , new KeyValue<string, int>("aring;", 229)
                , new KeyValue<string, int>("asymp;", 8776)
                , new KeyValue<string, int>("atilde;", 227)
                , new KeyValue<string, int>("auml;", 228)
                , new KeyValue<string, int>("bdquo;", 8222)
                , new KeyValue<string, int>("beta;", 946)
                , new KeyValue<string, int>("brvbar;", 166)
                , new KeyValue<string, int>("bull;", 8226)
                , new KeyValue<string, int>("cap;", 8745)
                , new KeyValue<string, int>("ccedil;", 231)
                , new KeyValue<string, int>("cedil;", 184)
                , new KeyValue<string, int>("cent;", 162)
                , new KeyValue<string, int>("chi;", 967)
                , new KeyValue<string, int>("circ;", 710)
                , new KeyValue<string, int>("clubs;", 9827)
                , new KeyValue<string, int>("cong;", 8773)
                , new KeyValue<string, int>("copy;", 169)
                , new KeyValue<string, int>("crarr;", 8629)
                , new KeyValue<string, int>("cup;", 8746)
                , new KeyValue<string, int>("curren;", 164)
                , new KeyValue<string, int>("dArr;", 8659)
                , new KeyValue<string, int>("dagger;", 8224)
                , new KeyValue<string, int>("darr;", 8595)
                , new KeyValue<string, int>("deg;", 176)
                , new KeyValue<string, int>("delta;", 948)
                , new KeyValue<string, int>("diams;", 9830)
                , new KeyValue<string, int>("divide;", 247)
                , new KeyValue<string, int>("eacute;", 233)
                , new KeyValue<string, int>("ecirc;", 234)
                , new KeyValue<string, int>("egrave;", 232)
                , new KeyValue<string, int>("empty;", 8709)
                , new KeyValue<string, int>("emsp;", 8195)
                , new KeyValue<string, int>("ensp;", 8194)
                , new KeyValue<string, int>("epsilon;", 949)
                , new KeyValue<string, int>("equiv;", 8801)
                , new KeyValue<string, int>("eta;", 951)
                , new KeyValue<string, int>("eth;", 240)
                , new KeyValue<string, int>("euml;", 235)
                , new KeyValue<string, int>("euro;", 8364)
                , new KeyValue<string, int>("exist;", 8707)
                , new KeyValue<string, int>("fnof;", 402)
                , new KeyValue<string, int>("forall;", 8704)
                , new KeyValue<string, int>("frac12;", 189)
                , new KeyValue<string, int>("frac14;", 188)
                , new KeyValue<string, int>("frac34;", 190)
                , new KeyValue<string, int>("frasl;", 8260)
                , new KeyValue<string, int>("gamma;", 947)
                , new KeyValue<string, int>("ge;", 8805)
                , new KeyValue<string, int>("gt;", 62)
                , new KeyValue<string, int>("hArr;", 8660)
                , new KeyValue<string, int>("harr;", 8596)
                , new KeyValue<string, int>("hearts;", 9829)
                , new KeyValue<string, int>("hellip;", 8230)
                , new KeyValue<string, int>("iacute;", 237)
                , new KeyValue<string, int>("icirc;", 238)
                , new KeyValue<string, int>("iexcl;", 161)
                , new KeyValue<string, int>("igrave;", 236)
                , new KeyValue<string, int>("image;", 8465)
                , new KeyValue<string, int>("infin;", 8734)
                , new KeyValue<string, int>("int;", 8747)
                , new KeyValue<string, int>("iota;", 953)
                , new KeyValue<string, int>("iquest;", 191)
                , new KeyValue<string, int>("isin;", 8712)
                , new KeyValue<string, int>("iuml;", 239)
                , new KeyValue<string, int>("kappa;", 954)
                , new KeyValue<string, int>("lArr;", 8656)
                , new KeyValue<string, int>("lambda;", 955)
                , new KeyValue<string, int>("lang;", 9001)
                , new KeyValue<string, int>("laquo;", 171)
                , new KeyValue<string, int>("larr;", 8592)
                , new KeyValue<string, int>("lceil;", 8968)
                , new KeyValue<string, int>("ldquo;", 8220)
                , new KeyValue<string, int>("le;", 8804)
                , new KeyValue<string, int>("lfloor;", 8970)
                , new KeyValue<string, int>("lowast;", 8727)
                , new KeyValue<string, int>("loz;", 9674)
                , new KeyValue<string, int>("lrm;", 8206)
                , new KeyValue<string, int>("lsaquo;", 8249)
                , new KeyValue<string, int>("lsquo;", 8216)
                , new KeyValue<string, int>("lt;", 60)
                , new KeyValue<string, int>("macr;", 175)
                , new KeyValue<string, int>("mdash;", 8212)
                , new KeyValue<string, int>("micro;", 181)
                , new KeyValue<string, int>("middot;", 183)
                , new KeyValue<string, int>("minus;", 8722)
                , new KeyValue<string, int>("mu;", 956)
                , new KeyValue<string, int>("nabla;", 8711)
                , new KeyValue<string, int>("nbsp;", 160)
                , new KeyValue<string, int>("ndash;", 8211)
                , new KeyValue<string, int>("ne;", 8800)
                , new KeyValue<string, int>("ni;", 8715)
                , new KeyValue<string, int>("not;", 172)
                , new KeyValue<string, int>("notin;", 8713)
                , new KeyValue<string, int>("nsub;", 8836)
                , new KeyValue<string, int>("ntilde;", 241)
                , new KeyValue<string, int>("nu;", 957)
                , new KeyValue<string, int>("oacute;", 243)
                , new KeyValue<string, int>("ocirc;", 244)
                , new KeyValue<string, int>("oelig;", 339)
                , new KeyValue<string, int>("ograve;", 242)
                , new KeyValue<string, int>("oline;", 8254)
                , new KeyValue<string, int>("omega;", 969)
                , new KeyValue<string, int>("omicron;", 959)
                , new KeyValue<string, int>("oplus;", 8853)
                , new KeyValue<string, int>("or;", 8744)
                , new KeyValue<string, int>("ordf;", 170)
                , new KeyValue<string, int>("ordm;", 186)
                , new KeyValue<string, int>("oslash;", 248)
                , new KeyValue<string, int>("otilde;", 245)
                , new KeyValue<string, int>("otimes;", 8855)
                , new KeyValue<string, int>("ouml;", 246)
                , new KeyValue<string, int>("para;", 182)
                , new KeyValue<string, int>("part;", 8706)
                , new KeyValue<string, int>("permil;", 8240)
                , new KeyValue<string, int>("perp;", 8869)
                , new KeyValue<string, int>("phi;", 966)
                , new KeyValue<string, int>("pi;", 960)
                , new KeyValue<string, int>("piv;", 982)
                , new KeyValue<string, int>("plusmn;", 177)
                , new KeyValue<string, int>("pound;", 163)
                , new KeyValue<string, int>("prime;", 8242)
                , new KeyValue<string, int>("prod;", 8719)
                , new KeyValue<string, int>("prop;", 8733)
                , new KeyValue<string, int>("psi;", 968)
                , new KeyValue<string, int>("quot;", 34)
                , new KeyValue<string, int>("rArr;", 8658)
                , new KeyValue<string, int>("radic;", 8730)
                , new KeyValue<string, int>("rang;", 9002)
                , new KeyValue<string, int>("raquo;", 187)
                , new KeyValue<string, int>("rarr;", 8594)
                , new KeyValue<string, int>("rceil;", 8969)
                , new KeyValue<string, int>("rdquo;", 8221)
                , new KeyValue<string, int>("real;", 8476)
                , new KeyValue<string, int>("reg;", 174)
                , new KeyValue<string, int>("rfloor;", 8971)
                , new KeyValue<string, int>("rho;", 961)
                , new KeyValue<string, int>("rlm;", 8207)
                , new KeyValue<string, int>("rsaquo;", 8250)
                , new KeyValue<string, int>("rsquo;", 8217)
                , new KeyValue<string, int>("sbquo;", 8218)
                , new KeyValue<string, int>("scaron;", 353)
                , new KeyValue<string, int>("sdot;", 8901)
                , new KeyValue<string, int>("sect;", 167)
                , new KeyValue<string, int>("shy;", 173)
                , new KeyValue<string, int>("sigma;", 963)
                , new KeyValue<string, int>("sigmaf;", 962)
                , new KeyValue<string, int>("sim;", 8764)
                , new KeyValue<string, int>("spades;", 9824)
                , new KeyValue<string, int>("sub;", 8834)
                , new KeyValue<string, int>("sube;", 8838)
                , new KeyValue<string, int>("sum;", 8721)
                , new KeyValue<string, int>("sup1;", 185)
                , new KeyValue<string, int>("sup2;", 178)
                , new KeyValue<string, int>("sup3;", 179)
                , new KeyValue<string, int>("sup;", 8835)
                , new KeyValue<string, int>("supe;", 8839)
                , new KeyValue<string, int>("szlig;", 223)
                , new KeyValue<string, int>("tau;", 964)
                , new KeyValue<string, int>("there4;", 8756)
                , new KeyValue<string, int>("theta;", 952)
                , new KeyValue<string, int>("thetasym;", 977)
                , new KeyValue<string, int>("thinsp;", 8201)
                , new KeyValue<string, int>("thorn;", 254)
                , new KeyValue<string, int>("tilde;", 732)
                , new KeyValue<string, int>("times;", 215)
                , new KeyValue<string, int>("trade;", 8482)
                , new KeyValue<string, int>("uArr;", 8657)
                , new KeyValue<string, int>("uacute;", 250)
                , new KeyValue<string, int>("uarr;", 8593)
                , new KeyValue<string, int>("ucirc;", 251)
                , new KeyValue<string, int>("ugrave;", 249)
                , new KeyValue<string, int>("uml;", 168)
                , new KeyValue<string, int>("upsih;", 978)
                , new KeyValue<string, int>("upsilon;", 965)
                , new KeyValue<string, int>("uuml;", 252)
                , new KeyValue<string, int>("weierp;", 8472)
                , new KeyValue<string, int>("xi;", 958)
                , new KeyValue<string, int>("yacute;", 253)
                , new KeyValue<string, int>("yen;", 165)
                , new KeyValue<string, int>("yuml;", 255)
                , new KeyValue<string, int>("zeta;", 950)
                , new KeyValue<string, int>("zwj;", 8205)
                , new KeyValue<string, int>("zwnj;", 8204)
            };
            decodeSearcher = new AutoCSer.StateSearcher.AsciiSearcher(new AutoCSer.StateSearcher.AsciiBuilder(chars, true).Data.Pointer);
        }
    }
}

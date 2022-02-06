using System;
using System.Reflection;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Memory;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Xml;

namespace AutoCSer
{
    /// <summary>
    /// XML序列化
    /// </summary>
    public unsafe sealed partial class XmlSerializer : AutoCSer.Threading.Link<XmlSerializer>, IDisposable
    {
        /// <summary>
        /// 反序列化配置名称
        /// </summary>
        public const string DeSerializeAttributeName = "DeSerialize";
        /// <summary>
        /// 默认反序列化类型配置
        /// </summary>
        internal static readonly XmlSerializeAttribute AllMemberAttribute = (XmlSerializeAttribute)AutoCSer.Configuration.Common.Get(typeof(XmlSerializeAttribute), DeSerializeAttributeName) ?? new XmlSerializeAttribute { Filter = MemberFilters.Instance, IsBaseType = false };
        /// <summary>
        /// 默认序列化类型配置
        /// </summary>
        internal static readonly XmlSerializeAttribute DefaultAttribute = (XmlSerializeAttribute)AutoCSer.Configuration.Common.Get(typeof(XmlSerializeAttribute)) ?? new XmlSerializeAttribute { IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly SerializeConfig DefaultConfig = (SerializeConfig)AutoCSer.Configuration.Common.Get(typeof(SerializeConfig)) ?? new SerializeConfig { CheckLoopDepth = SerializeConfig.DefaultCheckLoopDepth };
        /// <summary>
        /// 字符状态位查询表格
        /// </summary>
        private readonly byte* bits = XmlDeSerializer.Bits.Byte;
        /// <summary>
        /// 字符串输出缓冲区
        /// </summary>
        public readonly CharStream CharStream = new CharStream(default(AutoCSer.Memory.Pointer));
        /// <summary>
        /// 获取字符串输出缓冲区
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static CharStream GetCharStream(XmlSerializer xmlSerializer)
        {
            return xmlSerializer.CharStream;
        }
        /// <summary>
        /// 配置参数
        /// </summary>
        internal SerializeConfig Config;
        /// <summary>
        /// 警告提示状态
        /// </summary>
        public SerializeWarning Warning { get; internal set; }
        /// <summary>
        /// 集合子节点名称
        /// </summary>
        private string itemName;
        /// <summary>
        /// 集合子节点名称
        /// </summary>
        internal string GetItemName()
        {
            if (itemName == null) return Config.ItemName ?? "item";
            string value = itemName;
            itemName = null;
            return value;
        }
        /// <summary>
        /// 设置集合子节点名称
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="itemName"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SetItemName(XmlSerializer xmlSerializer, string itemName)
        {
            xmlSerializer.itemName = itemName;
        }
        /// <summary>
        /// 祖先节点集合
        /// </summary>
        private object[] forefather;
        /// <summary>
        /// 祖先节点数量
        /// </summary>
        private int forefatherCount;
        /// <summary>
        /// 循环检测深度
        /// </summary>
        private int checkLoopDepth = int.MinValue;

        /// <summary>
        /// 释放资源
        /// </summary>
        void IDisposable.Dispose()
        {
            CharStream.Dispose();
        }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>Xml 序列化结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private SerializeResult serializeResult<valueType>(valueType value, SerializeConfig config)
        {
            return new SerializeResult { Xml = serialize(value, config), Warning = Warning };
        }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>Xml 字符串</returns>
        private string serialize<valueType>(valueType value, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            AutoCSer.Memory.Pointer buffer = UnmanagedPool.Default.GetPointer();
            try
            {
                CharStream.Reset(ref buffer);
                using (CharStream)
                {
                    serialize(value);
                    return CharStream.ToString();
                }
            }
            finally { UnmanagedPool.Default.PushOnly(ref buffer); }
        }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="xmlStream">Xml输出缓冲区</param>
        /// <param name="config">配置参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private SerializeWarning serialize<valueType>(valueType value, CharStream xmlStream, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            CharStream.From(xmlStream);
            try
            {
                serialize(value);
                return Warning;
            }
            finally { xmlStream.From(CharStream); }
        }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        private void serialize<valueType>(valueType value)
        {
            Warning = SerializeWarning.None;
            if (checkLoopDepth != Config.CheckLoopDepth)
            {
                if (Config.CheckLoopDepth <= 0)
                {
                    checkLoopDepth = 0;
                    if (forefather == null) forefather = new object[sizeof(int)];
                }
                else checkLoopDepth = Config.CheckLoopDepth;
            }
            CharStream.Write(Config.Header);
            fixed (char* nameFixed = Config.BootNodeName)
            {
                nameStart(nameFixed, Config.BootNodeName.Length);
                TypeSerializer<valueType>.Serialize(this, value);
                nameEnd(nameFixed, Config.BootNodeName.Length);
            }
        }
        /// <summary>
        /// 自定义序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void TypeSerialize<valueType>(valueType value)
        {
            if (value != null) TypeSerializer<valueType>.Serialize(this, value);
        }
        /// <summary>
        /// 获取并设置自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">序列化成员位图</param>
        /// <returns>序列化成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public AutoCSer.Metadata.MemberMap SetCustomMemberMap(AutoCSer.Metadata.MemberMap memberMap)
        {
            return Config.SetCustomMemberMap(memberMap);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void free()
        {
            Config = null;
            itemName = null;
            if (forefatherCount != 0)
            {
                System.Array.Clear(forefather, 0, forefatherCount);
                forefatherCount = 0;
            }
            YieldPool.Default.Push(this);
        }
        /// <summary>
        /// 进入对象节点
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <returns>是否继续处理对象</returns>
        internal bool Push<valueType>(valueType value)
        {
            if (checkLoopDepth == 0)
            {
                if (forefatherCount != 0)
                {
                    int count = forefatherCount;
                    object objectValue = value;
                    foreach (object arrayValue in forefather)
                    {
                        if (arrayValue == objectValue) return false;
                        if (--count == 0) break;
                    }
                }
                if (forefatherCount == forefather.Length) forefather = forefather.copyNew(forefatherCount << 1);
                forefather[forefatherCount++] = value;
            }
            else if (--checkLoopDepth == 0) throw new OverflowException();
            return true;
        }
        /// <summary>
        /// 退出对象节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Pop()
        {
            if (checkLoopDepth == 0) forefather[--forefatherCount] = null;
            else ++checkLoopDepth;
        }
        /// <summary>
        /// 标签开始
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void nameStart(char* start, int length)
        {
            char* data = CharStream.GetPrepCharSizeCurrent(length + (2 + 2));
            *data = '<';
            AutoCSer.Extensions.StringExtension.SimpleCopyNotNull64(start, ++data, length);
            *(data + length) = '>';
            CharStream.Data.CurrentIndex += (length + 2) * sizeof(char);
        }
        /// <summary>
        /// 标签结束
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void nameEnd(char* start, int length)
        {
            char* data = CharStream.GetPrepCharSizeCurrent(length + (3 + 2));
            *(int*)data = '<' + ('/' << 16);
            AutoCSer.Extensions.StringExtension.SimpleCopyNotNull64(start, data + 2, length);
            *(data + (length + 2)) = '>';
            CharStream.Data.CurrentIndex += (length + 3) * sizeof(char);
        }

        /// <summary>
        /// 计算编码增加长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private int encodeSpaceSize(char value)
        {
            if (((bits[(byte)value] & XmlDeSerializer.EncodeSpaceBit) | (value & 0xff00)) == 0)
            {
                switch (value & 7)
                {
                    case '&' & 7:  //26 00100110
                        //case '>' & 7://3e 00111110
                        return 4 - ((value >> 4) & 1);
                    case '\n' & 7:
                    case '\r' & 7:
                    case ' ' & 7:
                        return 4;
                    case '\t' & 7:
                    case '<' & 7:
                        return 3;
                }
            }
            return 0;
        }
        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value">字符</param>
        private void encodeSpace(ref byte* data, char value)
        {
            if (((bits[(byte)value] & XmlDeSerializer.EncodeSpaceBit) | (value & 0xff00)) == 0)
            {
                switch ((byte)value)
                {
                    case (byte)'\t':
                        *(long*)data = '&' + ('#' << 16) + ((long)'9' << 32) + ((long)';' << 48);
                        data += sizeof(long);
                        return;
                    case (byte)'\n':
                        *(long*)data = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'0' << 48);
                        *(char*)(data + sizeof(long)) = ';';
                        data += sizeof(long) + sizeof(char);
                        return;
                    case (byte)'\r':
                        *(long*)data = '&' + ('#' << 16) + ((long)'1' << 32) + ((long)'3' << 48);
                        *(char*)(data + sizeof(long)) = ';';
                        data += sizeof(long) + sizeof(char);
                        return;
                    case (byte)' ':
                        *(long*)data = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'2' << 48);
                        *(char*)(data + sizeof(long)) = ';';
                        data += sizeof(long) + sizeof(char);
                        return;
                    case (byte)'&':
                        *(long*)data = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'8' << 48);
                        *(char*)(data + sizeof(long)) = ';';
                        data += sizeof(long) + sizeof(char);
                        return;
                    case (byte)'<':
                        *(long*)data = '&' + ('l' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        data += sizeof(long);
                        return;
                    case (byte)'>':
                        *(long*)data = '&' + ('g' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                        data += sizeof(long);
                        return;
                }
            }
            *(char*)data = value;
            data += sizeof(char);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void serialize(char* start, int length)
        {
            switch (length)
            {
                case 1:
                    CallSerialize(*start);
                    return;
                case 2:
                    CallSerialize(*start);
                    CallSerialize(*(start + 1));
                    return;
            }
            char* end = start + (length - 1);
            int addLength = 0;
            if (length > 8)
            {
                int isCData = *end == '>' && *(int*)(end - 2) == ']' + (']' << 16) ? 0 : 1;
                for (char* code = start + 2; code != end; ++code)
                {
                    if (((bits[*(byte*)code] & XmlDeSerializer.EncodeBit) | *(((byte*)code) + 1)) == 0)
                    {
                        switch ((*(byte*)code >> 1) & 7)
                        {
                            case ('&' >> 1) & 7:
                                addLength += 4;
                                break;
                            case ('<' >> 1) & 7:
                                addLength += 3;
                                break;
                            case ('>' >> 1) & 7:
                                addLength += 3;
                                if (*(int*)(code - 2) == ']' + (']' << 16)) isCData = 0;
                                break;
                        }
                    }
                }
                if (isCData == 0)
                {
                    byte* code = (byte*)(start + 1);
                    if (((bits[*(byte*)code] & XmlDeSerializer.EncodeBit) | *(((byte*)code) + 1)) == 0)
                    {
                        //& 26 00100110
                        //< 3c 00111100
                        //> 3e 00111110
                        addLength += 4 - ((*(byte*)code >> 4) & 1);
                    }
                }
                else
                {
                    if (addLength == 0)
                    {
                        addLength += encodeSpaceSize(*start) + encodeSpaceSize(*(start + 1)) + encodeSpaceSize(*end);
                    }
                    if (addLength == 0) CharStream.Write(start, length);
                    else
                    {
                        byte* write = (byte*)CharStream.GetPrepCharSizeCurrent(length + 13);
                        *(long*)write = '<' + ('!' << 16) + ((long)'[' << 32) + ((long)'C' << 48);
                        *(long*)(write + sizeof(long)) = 'D' + ('A' << 16) + ((long)'T' << 32) + ((long)'A' << 48);
                        *(char*)(write + sizeof(long) * 2) = '[';
                        new Span<char>(start, length).CopyTo(new Span<char>(write + (sizeof(long) * 2 + sizeof(char)), length));
                        *(long*)(write + (sizeof(long) * 2 + sizeof(char)) + (length << 1)) = ']' + (']' << 16) + ((long)'>' << 32);
                        CharStream.Data.CurrentIndex += (length + 12) * sizeof(char);
                    }
                    return;
                }
            }
            else
            {
                for (char* code = start + 1; code != end; ++code)
                {
                    if (((bits[*(byte*)code] & XmlDeSerializer.EncodeBit) | *(((byte*)code) + 1)) == 0)
                    {
                        //& 26 00100110
                        //< 3c 00111100
                        //> 3e 00111110
                        addLength += 4 - ((*(byte*)code >> 4) & 1);
                    }
                }
            }
            if ((addLength += encodeSpaceSize(*start) + encodeSpaceSize(*end)) == 0) CharStream.Write(start, length);
            else
            {
                byte* write = (byte*)CharStream.GetPrepCharSizeCurrent(length + addLength);
                encodeSpace(ref write, *start++);
                do
                {
                    if (((bits[*(byte*)start] & XmlDeSerializer.EncodeBit) | *(((byte*)start) + 1)) == 0)
                    {
                        switch ((*(byte*)start >> 1) & 7)
                        {
                            case ('&' >> 1) & 7:
                                *(long*)write = '&' + ('#' << 16) + ((long)'3' << 32) + ((long)'8' << 48);
                                *(char*)(write + sizeof(long)) = ';';
                                write += sizeof(long) + sizeof(char);
                                break;
                            case ('<' >> 1) & 7:
                                *(long*)write = '&' + ('l' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                                write += sizeof(long);
                                break;
                            case ('>' >> 1) & 7:
                                *(long*)write = '&' + ('g' << 16) + ((long)'t' << 32) + ((long)';' << 48);
                                write += sizeof(long);
                                break;
                        }
                    }
                    else
                    {
                        *(char*)write = *start;
                        write += sizeof(char);
                    }
                }
                while (++start != end);
                encodeSpace(ref write, *start);
                CharStream.Data.CurrentIndex += (length + addLength) * sizeof(char);
            }
        }
        /// <summary>
        /// 输出空字符串
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void emptyString()
        {
            //<![CDATA[]]>
            byte* write = CharStream.GetBeforeMove(24);
            *(long*)write = '<' + ('!' << 16) + ((long)'[' << 32) + ((long)'C' << 48);
            *(long*)(write + sizeof(long)) = 'D' + ('A' << 16) + ((long)'T' << 32) + ((long)'A' << 48);
            *(long*)(write + sizeof(long) * 2) = '[' + (']' << 16) + ((long)']' << 32) + ((long)'>' << 48);
        }

        /// <summary>
        /// 是否输出字符串
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsOutputSubString(XmlSerializer xmlSerializer, SubString value)
        {
            return value.Length != 0 || xmlSerializer.Config.IsOutputEmptyString;
        }
        /// <summary>
        /// 是否输出字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private bool isOutputString(string value)
        {
            if (value == null) return Config.IsOutputNull && Config.IsOutputEmptyString;
            return value.Length != 0 || Config.IsOutputEmptyString;
        }
        /// <summary>
        /// 是否输出字符串
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsOutputString(XmlSerializer xmlSerializer, string value)
        {
            return xmlSerializer.isOutputString(value);
        }
        /// <summary>
        /// 是否输出对象
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsOutput(XmlSerializer xmlSerializer, object value)
        {
            return value != null || xmlSerializer.Config.IsOutputNull;
        }
        /// <summary>
        /// 是否输出可空对象
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool IsOutputNullable<valueType>(XmlSerializer xmlSerializer, Nullable<valueType> value) where valueType : struct
        {
            return value.HasValue || xmlSerializer.Config.IsOutputNull;
        }
        /// <summary>
        /// 引用类型对象转换XML字符串
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassSerialize<valueType>(XmlSerializer xmlSerializer, valueType value)
        {
            if (value != null) TypeSerializer<valueType>.ClassSerialize(xmlSerializer, value);
        }
        /// <summary>
        /// 值类型对象转换XML字符串
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize<valueType>(XmlSerializer xmlSerializer, valueType value)
        {
            TypeSerializer<valueType>.StructSerialize(xmlSerializer, value);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void EnumToString<valueType>(XmlSerializer xmlSerializer, valueType value)
        {
            xmlSerializer.CallSerialize(value.ToString());
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <param name="xmlSerializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void baseSerialize<valueType, childType>(XmlSerializer xmlSerializer, childType value) where childType : valueType
        {
            TypeSerializer<valueType>.ClassSerialize(xmlSerializer, value);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        private void structArray<valueType>(valueType[] array)
        {
            if (array != null && Push(array))
            {
                //charStream xmlStream = CharStream;
                string itemName = GetItemName();
                fixed (char* itemNameFixed = itemName)
                {
                    int itemNameLength = itemName.Length;
                    foreach (valueType value in array)
                    {
                        nameStart(itemNameFixed, itemNameLength);
                        TypeSerializer<valueType>.StructSerialize(this, value);
                        nameEnd(itemNameFixed, itemNameLength);
                    }
                }
                Pop();
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructArray<valueType>(XmlSerializer xmlSerializer, valueType[] array)
        {
            xmlSerializer.structArray(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        private void array<valueType>(valueType[] array)
        {
            if (array != null && Push(array))
            {
                //charStream xmlStream = CharStream;
                string itemName = GetItemName();
                fixed (char* itemNameFixed = itemName)
                {
                    int itemNameLength = itemName.Length;
                    foreach (valueType value in array)
                    {
                        nameStart(itemNameFixed, itemNameLength);
                        if (value != null)
                        {
                            int length = CharStream.Length;
                            TypeSerializer<valueType>.ClassSerialize(this, value);
                            if (length == CharStream.Length) CharStream.Write(' ');
                        }
                        nameEnd(itemNameFixed, itemNameLength);
                    }
                }
                Pop();
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Array<valueType>(XmlSerializer xmlSerializer, valueType[] array)
        {
            xmlSerializer.array(array);
        }
        /// <summary>
        /// 值类型对象转换XML字符串
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullableSerialize<valueType>(XmlSerializer xmlSerializer, Nullable<valueType> value) where valueType : struct
        {
            if (value.HasValue) TypeSerializer<valueType>.StructSerialize(xmlSerializer, value.Value);
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="values">枚举集合</param>
        private void structStructEnumerable<valueType, elementType>(valueType values) where valueType : IEnumerable<elementType>
        {
            //charStream xmlStream = CharStream;
            string itemName = GetItemName();
            fixed (char* itemNameFixed = itemName)
            {
                int itemNameLength = itemName.Length;
                foreach (elementType value in values)
                {
                    nameStart(itemNameFixed, itemNameLength);
                    TypeSerializer<elementType>.StructSerialize(this, value);
                    nameEnd(itemNameFixed, itemNameLength);
                }
            }
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="values">枚举集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructStructEnumerable<valueType, elementType>(XmlSerializer xmlSerializer, valueType values) where valueType : IEnumerable<elementType>
        {
            xmlSerializer.structStructEnumerable<valueType, elementType>(values);
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="values">枚举集合</param>
        private void structClassEnumerable<valueType, elementType>(valueType values) where valueType : IEnumerable<elementType>
        {
            //charStream xmlStream = CharStream;
            string itemName = GetItemName();
            fixed (char* itemNameFixed = itemName)
            {
                int itemNameLength = itemName.Length;
                foreach (elementType value in values)
                {
                    nameStart(itemNameFixed, itemNameLength);
                    if (value != null)
                    {
                        int length = CharStream.Length;
                        TypeSerializer<elementType>.ClassSerialize(this, value);
                        if (length == CharStream.Length) CharStream.Write(' ');
                    }
                    nameEnd(itemNameFixed, itemNameLength);
                }
            }
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="values">枚举集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructClassEnumerable<valueType, elementType>(XmlSerializer xmlSerializer, valueType values) where valueType : IEnumerable<elementType>
        {
            xmlSerializer.structClassEnumerable<valueType, elementType>(values);
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="value">枚举集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void classStructEnumerable<valueType, elementType>(valueType value) where valueType : IEnumerable<elementType>
        {
            if (Push(value))
            {
                structStructEnumerable<valueType, elementType>(value);
                Pop();
            }
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value">枚举集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassStructEnumerable<valueType, elementType>(XmlSerializer xmlSerializer, valueType value) where valueType : IEnumerable<elementType>
        {
            if (value != null) xmlSerializer.classStructEnumerable<valueType, elementType>(value);
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="value">枚举集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void classClassEnumerable<valueType, elementType>(valueType value) where valueType : IEnumerable<elementType>
        {
            if (Push(value))
            {
                structClassEnumerable<valueType, elementType>(value);
                Pop();
            }
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="value">枚举集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassClassEnumerable<valueType, elementType>(XmlSerializer xmlSerializer, valueType value) where valueType : IEnumerable<elementType>
        {
            if (value != null) xmlSerializer.classClassEnumerable<valueType, elementType>(value);
        }

        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化结果</returns>
        public static SerializeResult SerializeResult<valueType>(valueType value, SerializeConfig config = null)
        {
            XmlSerializer xmlSerializer = YieldPool.Default.Pop() ?? new XmlSerializer();
            try
            {
                return xmlSerializer.serializeResult<valueType>(value, config);
            }
            finally { xmlSerializer.free(); }
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>Xml 字符串</returns>
        public static string Serialize<valueType>(valueType value, SerializeConfig config = null)
        {
            XmlSerializer xmlSerializer = YieldPool.Default.Pop() ?? new XmlSerializer();
            try
            {
                return xmlSerializer.serialize<valueType>(value, config);
            }
            finally { xmlSerializer.free(); }
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="xmlStream">Xml输出缓冲区</param>
        /// <param name="config">配置参数</param>
        /// <returns>警告提示状态</returns>
        public static SerializeWarning Serialize<valueType>(valueType value, CharStream xmlStream, SerializeConfig config = null)
        {
            XmlSerializer xmlSerializer = YieldPool.Default.Pop() ?? new XmlSerializer();
            try
            {
                return xmlSerializer.serialize<valueType>(value, xmlStream, config);
            }
            finally { xmlSerializer.free(); }
        }

        static XmlSerializer()
        {
            byte* bits = XmlDeSerializer.Bits.Byte;
            bits['\t'] &= XmlDeSerializer.EncodeSpaceBit ^ 255;
            bits['\r'] &= XmlDeSerializer.EncodeSpaceBit ^ 255;
            bits['\n'] &= XmlDeSerializer.EncodeSpaceBit ^ 255;
            bits[' '] &= XmlDeSerializer.EncodeSpaceBit ^ 255;
            bits['&'] &= (XmlDeSerializer.EncodeSpaceBit | XmlDeSerializer.EncodeBit) ^ 255;
            bits['<'] &= (XmlDeSerializer.EncodeSpaceBit | XmlDeSerializer.EncodeBit) ^ 255;
            bits['>'] &= (XmlDeSerializer.EncodeSpaceBit | XmlDeSerializer.EncodeBit) ^ 255;

            serializeMethods = AutoCSer.DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(XmlSerializer).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(SerializeMethod), false))
                {
                    serializeMethods.Add(method.GetParameters()[0].ParameterType, method);
                }
            }
        }
    }
}

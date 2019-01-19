using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using AutoCSer.Extension;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 节点
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public unsafe struct Node
    {
        /// <summary>
        /// 64位整数值
        /// </summary>
        internal long Int64;
        /// <summary>
        /// 字符串
        /// </summary>
        public string String
        {
            get
            {
                switch (Type)
                {
                    case NodeType.String:
                    case NodeType.NumberString:
                    case NodeType.ErrorQuoteString:
                        return SubString;
                    case NodeType.QuoteString:
                        checkQuoteString();
                        return SubString;
                    case NodeType.NaN: 
                        return "NaN";
                    case NodeType.PositiveInfinity:
                        return "Infinity";
                    case NodeType.NegativeInfinity:
                        return "-Infinity";
                    case NodeType.DateTimeTick:
                        return new DateTime(Int64, DateTimeKind.Local).toString();
                    case NodeType.Bool:
                        return (int)Int64 == 0 ? "false" : "true";
                    case NodeType.Array:
                        int count = (int)Int64;
                        if (count == 0) return "[]";
                        int isNext = 0;
                        Parser parser = null;
                        byte* buffer = UnmanagedPool.Default.Get();
                        try
                        {
                            CharStream charStream = new CharStream((char*)buffer, UnmanagedPool.DefaultSize >> 1);
                            charStream.UnsafeWrite('[');
                            foreach (Node node in ListArray)
                            {
                                if (isNext == 0) isNext = 1;
                                else charStream.Write(',');
                                node.toString(charStream, ref parser);
                                if (--count == 0) break;
                            }
                            charStream.Write(']');
                            return charStream.ToString();
                        }
                        finally
                        {
                            UnmanagedPool.Default.Push(buffer);
                            if (parser != null) Parser.YieldPool.Default.PushNotNull(parser);
                        }
                    case NodeType.Dictionary:
                        return "[object Object]";
                }
                return null;
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为字符串
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>字符串</returns>
        public static implicit operator string(Node value) { return value.String; }
        /// <summary>
        /// 检测未解析字符串
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void checkQuoteString()
        {
            Type = Parser.ParseQuoteString(ref SubString, (int)(Int64 >> 32), (char)Int64, (int)Int64 >> 16) ? NodeType.String : NodeType.ErrorQuoteString;
        }
        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="parser"></param>
        private void toString(CharStream charStream, ref Parser parser)
        {
            switch (Type)
            {
                case NodeType.String:
                case NodeType.NumberString:
                case NodeType.ErrorQuoteString:
                    charStream.Write(ref SubString);
                    return;
                case NodeType.QuoteString:
                    if (parser == null) parser = Parser.YieldPool.Default.Pop() ?? new Parser();
                    if (!parser.ParseQuoteString(ref SubString, charStream, (int)(Int64 >> 32), (char)Int64)) Type = NodeType.ErrorQuoteString;
                    return;
                case NodeType.NaN:
                    charStream.WriteJsonNaN();
                    return;
                case NodeType.PositiveInfinity:
                    charStream.WritePositiveInfinity();
                    return;
                case NodeType.NegativeInfinity:
                    charStream.WriteNegativeInfinity();
                    return;
                case NodeType.DateTimeTick:
                    new DateTime(Int64, DateTimeKind.Local).ToString(charStream);
                    return;
                case NodeType.Bool:
                    charStream.WriteJsonBool((int)Int64 != 0);
                    return;
                case NodeType.Array:
                    int count = (int)Int64;
                    if (count == 0) charStream.WriteJsonArray();
                    int isNext = 0;
                    charStream.Write('[');
                    foreach (Node node in ListArray)
                    {
                        if (isNext == 0) isNext = 1;
                        else charStream.Write(',');
                        node.toString(charStream, ref parser);
                        if (--count == 0)
                        {
                            charStream.Write(']');
                            return;
                        }
                    }
                    return;
                case NodeType.Dictionary:
                    charStream.WriteJsonObjectString();
                    return;
            }
        }
        /// <summary>
        /// 逻辑值
        /// </summary>
        public bool Bool
        {
            get
            {
                switch (Type)
                {
                    case NodeType.String:
                        return SubString.Length != 0;
                    case NodeType.QuoteString:
                    case NodeType.ErrorQuoteString:
                        return true;
                    case NodeType.Array:
                    case NodeType.Dictionary:
                        return true;
                    case NodeType.NumberString:
                        if ((int)Int64 == 0)
                        {
                            if (SubString.Length == 1) return SubString[0] != '0';
                            double value = double.Parse(SubString, System.Globalization.CultureInfo.InvariantCulture);
                            return value != 0 && !double.IsNaN(value);
                        }
                        return true;
                    case NodeType.DateTimeTick:
                        return Int64 != 0;
                    case NodeType.Bool:
                        return (int)Int64 != 0;
                }
                return false;
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为逻辑值
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>逻辑值</returns>
        public static implicit operator bool(Node value) { return value.Bool; }
        /// <summary>
        /// 数值
        /// </summary>
        public double Number
        {
            get
            {
                switch (Type)
                {
                    case NodeType.NumberString:
                        if (SubString.Length == 1)
                        {
                            int value = SubString[0] - '0';
                            if ((uint)value < 10) return value;
                        }
                        return double.Parse(SubString, System.Globalization.CultureInfo.InvariantCulture);
                    case NodeType.NaN:
                        return double.NaN;
                    case NodeType.PositiveInfinity:
                        return double.PositiveInfinity;
                    case NodeType.NegativeInfinity:
                        return double.NegativeInfinity;
                }
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为数值
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>数值</returns>
        public static implicit operator double(Node value) { return value.Number; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                switch (Type)
                {
                    case NodeType.String: return DateTime.Parse(SubString);
                    case NodeType.DateTimeTick: return new DateTime(Int64, DateTimeKind.Local);
                }
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// JSON 节点隐式转换为时间
        /// </summary>
        /// <param name="value">JSON 节点</param>
        /// <returns>字符串</returns>
        public static implicit operator DateTime(Node value) { return value.DateTime; }
        /// <summary>
        /// 字典
        /// </summary>
        internal KeyValue<Node, Node>[] DictionaryArray;
        /// <summary>
        /// 字典
        /// </summary>
        internal LeftArray<KeyValue<Node, Node>> Dictionary
        {
            get
            {
                return new LeftArray<KeyValue<Node, Node>> { Array = DictionaryArray, Length = (int)Int64 };
            }
        }
        /// <summary>
        /// 字典数据集合
        /// </summary>
        public IEnumerable<KeyValue<Node, Node>> Values
        {
            get
            {
                if (Type == NodeType.Array) return Dictionary;
                if (Type == NodeType.Null) return NullValue<KeyValue<Node, Node>>.Array;
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        internal Node[] ListArray;
        /// <summary>
        /// 列表
        /// </summary>
        internal LeftArray<Node> LeftArray
        {
            get
            {
                return new LeftArray<Node> { Array = ListArray, Length = (int)Int64 };
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        public LeftArray<Node> Array
        {
            get
            {
                if (Type == NodeType.Array) return LeftArray;
                if (Type == NodeType.Null) return default(LeftArray<Node>);
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 字符串
        /// </summary>
        internal SubString SubString;
        /// <summary>
        /// 类型
        /// </summary>
        public NodeType Type { get; internal set; }
        /// <summary>
        /// 是否空节点
        /// </summary>
        public bool IsNull
        {
            get { return Type == NodeType.Null; }
        }
        /// <summary>
        /// 字典 / 列表节点数量
        /// </summary>
        public int Count
        {
            get
            {
                switch (Type)
                {
                    case NodeType.Dictionary:
                    case NodeType.Array: return (int)Int64;
                    case NodeType.Null: return 0;
                }
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 获取列表节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Node this[int index]
        {
            get
            {
                switch (Type)
                {
                    case NodeType.String:
                    case NodeType.NumberString:
                    case NodeType.ErrorQuoteString:
                        goto CHARNODE;
                    case NodeType.QuoteString:
                        checkQuoteString();
                    CHARNODE:
                        if ((uint)index < SubString.Length) return new Node { Type = NodeType.String, SubString = SubString.GetSub(index, 1) };
                        break;
                    case NodeType.Array:
                        if ((uint)index < (uint)Int64) return ListArray[index];
                        break;
                    case NodeType.Dictionary:
                        int count = (int)Int64;
                        if (count != 0)
                        {
                            string key = index.toString();
                            foreach (KeyValue<Node, Node> value in DictionaryArray)
                            {
                                if (value.Key.String == key) return value.Value;
                            }
                        }
                        break;
                }
                return default(Node);
            }
        }
        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public Node this[string key]
        {
            get
            {
                switch (Type)
                {
                    case NodeType.Dictionary:
                        int count = (int)Int64;
                        if (count != 0)
                        {
                            foreach (KeyValue<Node, Node> value in DictionaryArray)
                            {
                                if (value.Key.String == key) return value.Value;
                            }
                        }
                        break;
                }
                return default(Node);
            }
        }

        /// <summary>
        /// 设置数字字符串
        /// </summary>
        /// <param name="quote"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetNumberString(char quote)
        {
            Int64 = quote;
            Type = NodeType.NumberString;
        }
        /// <summary>
        /// 未解析字符串
        /// </summary>
        /// <param name="escapeIndex">未解析字符串起始位置</param>
        /// <param name="quote">字符串引号</param>
        /// <param name="isTempString"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetQuoteString(int escapeIndex, char quote, bool isTempString)
        {
            Type = NodeType.QuoteString;
            Int64 = ((long)escapeIndex << 32) + quote;
            if (isTempString) Int64 += 0x10000;
        }
        /// <summary>
        /// 设置列表
        /// </summary>
        /// <param name="list"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetList(ref LeftArray<Node> list)
        {
            this.ListArray = list.Array;
            Int64 = list.Length;
            Type = NodeType.Array;
        }
        /// <summary>
        /// 设置列表
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetList()
        {
            ListArray = null;
            Int64 = 0;
            Type = NodeType.Array;
        }
        /// <summary>
        /// 设置字典
        /// </summary>
        /// <param name="dictionary"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetDictionary(ref LeftArray<KeyValue<Node, Node>> dictionary)
        {
            this.DictionaryArray = dictionary.Array;
            Int64 = dictionary.Length;
            Type = NodeType.Dictionary;
        }
        /// <summary>
        /// 设置字典
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetDictionary()
        {
            DictionaryArray = null;
            Int64 = 0;
            Type = NodeType.Dictionary;
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String;
        }
    }
}

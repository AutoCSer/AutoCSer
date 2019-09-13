using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class Serializer : AutoCSer.Threading.Link<Serializer>, IDisposable
    {
        /// <summary>
        /// 最大整数值
        /// </summary>
        internal const long MaxInt = (1L << 52) - 1;
        /// <summary>
        /// 默认序列化类型配置
        /// </summary>
        internal static readonly SerializeAttribute AllMemberAttribute = ConfigLoader.GetUnion(typeof(SerializeAttribute)).SerializeAttribute ?? new SerializeAttribute { IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly SerializeConfig DefaultConfig = ConfigLoader.GetUnion(typeof(SerializeConfig)).SerializeConfig ?? new SerializeConfig();
        /// <summary>
        /// 字符串输出缓冲区
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public readonly CharStream CharStream;
        /// <summary>
        /// 配置参数
        /// </summary>
        internal SerializeConfig Config;
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
        /// 警告提示状态
        /// </summary>
        public SerializeWarning Warning { get; internal set; }
#if AutoCSer
        /// <summary>
        /// 对象编号
        /// </summary>
        private ReusableDictionary<ObjectReference, int> objectIndexs;
        /// <summary>
        /// 是否调用循环引用处理函数
        /// </summary>
        private bool isLoopObject;
#endif
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <param name="isThread">是否单线程模式</param>
        internal Serializer(bool isThread = false)
        {
            CharStream = isThread ? new CharStream() : new CharStream(null, 0);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        void IDisposable.Dispose()
        {
            CharStream.Dispose();
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>Json字符串</returns>
        private string serialize<valueType>(ref valueType value, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            byte* buffer = AutoCSer.UnmanagedPool.Default.Get();
            try
            {
                CharStream.Reset((byte*)buffer, AutoCSer.UnmanagedPool.DefaultSize);
                using (CharStream)
                {
                    serialize(ref value);
                    return CharStream.ToString();
                }
            }
            finally { AutoCSer.UnmanagedPool.Default.Push(buffer); }
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="jsonStream">Json输出缓冲区</param>
        /// <param name="config">配置参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void serialize<valueType>(ref valueType value, CharStream jsonStream, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            CharStream.From(jsonStream);
            try
            {
                serialize(ref value);
            }
            finally { jsonStream.From(CharStream); }
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="stream">二进制缓冲区</param>
        /// <param name="config">配置参数</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void serialize<valueType>(ref valueType value, UnmanagedStream stream, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            CharStream.From(stream);
            try
            {
                serialize(ref value);
            }
            finally { stream.From(CharStream); }
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        private void serialize<valueType>(ref valueType value)
        {
#if AutoCSer
            if (Config.GetLoopObject == null || Config.SetLoopObject == null)
            {
                if (Config.GetLoopObject != null) Warning = SerializeWarning.LessSetLoop;
                else if (Config.SetLoopObject != null) Warning = SerializeWarning.LessGetLoop;
                else Warning = SerializeWarning.None;
                if (checkLoopDepth != Config.CheckLoopDepth || isLoopObject)
                {
                    if (Config.CheckLoopDepth <= 0)
                    {
                        checkLoopDepth = 0;
                        if (forefather == null) forefather = new object[sizeof(int)];
                    }
                    else checkLoopDepth = Config.CheckLoopDepth;
                }
                isLoopObject = false;
            }
            else
            {
                if (!isLoopObject)
                {
                    isLoopObject = true;
                    if (objectIndexs == null) objectIndexs = ReusableDictionary<ObjectReference>.Create<int>();
                }
                Warning = SerializeWarning.None;
                checkLoopDepth = Config.CheckLoopDepth <= 0 ? SerializeConfig.DefaultCheckLoopDepth : Config.CheckLoopDepth;
            }
#else
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
#endif
            TypeSerializer<valueType>.Serialize(this, ref value);
        }
        /// <summary>
        /// 对象转换JSON字符串（单线程模式）
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>Json字符串</returns>
        private string serializeThread<valueType>(ref valueType value, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            CharStream.Clear();
            serialize(ref value);
            return CharStream.ToString();
        }
        /// <summary>
        /// 自定义序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void TypeSerialize<valueType>(valueType value)
        {
            if (value == null) CharStream.WriteJsonNull();
            else TypeSerializer<valueType>.Serialize(this, ref value);
        }
        /// <summary>
        /// 自定义序列化调用
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void TypeSerialize<valueType>(ref valueType value)
        {
            if (value == null) CharStream.WriteJsonNull();
            else TypeSerializer<valueType>.Serialize(this, ref value);
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
        /// 释放资源（单线程模式）
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void freeThread()
        {
            Config = null;
#if AutoCSer
            if (isLoopObject) objectIndexs.Clear();
            else
#endif
            if (forefatherCount != 0)
            {
                System.Array.Clear(forefather, 0, forefatherCount);
                forefatherCount = 0;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Free()
        {
            freeThread();
            YieldPool.Default.PushNotNull(this);
        }
        /// <summary>
        /// 进入对象节点
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <returns>是否继续处理对象</returns>
        private bool pushArray<valueType>(valueType value)
        {
            if (checkLoopDepth == 0)
            {
                if (forefatherCount != 0)
                {
                    int count = forefatherCount;
                    object objectValue = value;
                    foreach (object arrayValue in forefather)
                    {
                        if (arrayValue == objectValue)
                        {
                            CharStream.WriteJsonObject();
                            return false;
                        }
                        if (--count == 0) break;
                    }
                }
                if (forefatherCount == forefather.Length) forefather = forefather.copyNew(forefatherCount << 1);
                forefather[forefatherCount++] = value;
            }
            else
            {
                if (--checkLoopDepth == 0) throw new OverflowException();
#if AutoCSer
                if (isLoopObject)
                {
                    int index;
                    if (objectIndexs.TryGetValue(new ObjectReference { Value = value }, out index))
                    {
                        CharStream.PrepLength(Config.GetLoopObject.Length + (10 + 5 + 3));
                        CharStream.UnsafeSimpleWrite(Config.GetLoopObject);
                        CharStream.UnsafeWrite('(');
                        CharStream.WriteJson((uint)index, true);
                        CharStream.UnsafeSimpleWrite(",[])");
                        return false;
                    }
                    objectIndexs.Set(new ObjectReference { Value = value }, index = objectIndexs.Count);
                    CharStream.PrepLength(Config.SetLoopObject.Length + (10 + 2 + 2));
                    CharStream.UnsafeSimpleWrite(Config.SetLoopObject);
                    CharStream.UnsafeWrite('(');
                    CharStream.WriteJson((uint)index, true);
                    CharStream.UnsafeWrite(',');
                }
#endif
            }
            return true;
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
                        if (arrayValue == objectValue)
                        {
                            CharStream.WriteJsonObject();
                            return false;
                        }
                        if (--count == 0) break;
                    }
                }
                if (forefatherCount == forefather.Length) forefather = forefather.copyNew(forefatherCount << 1);
                forefather[forefatherCount++] = value;
            }
            else
            {
                if (--checkLoopDepth == 0) throw new OverflowException();
#if AutoCSer
                if (isLoopObject)
                {
                    int index;
                    if (objectIndexs.TryGetValue(new ObjectReference { Value = value }, out index))
                    {
                        CharStream.PrepLength(Config.GetLoopObject.Length + (10 + 2 + 2));
                        CharStream.UnsafeSimpleWrite(Config.GetLoopObject);
                        CharStream.UnsafeWrite('(');
                        CharStream.WriteJson((uint)index, true);
                        CharStream.UnsafeWrite(')');
                        return false;
                    }
                    objectIndexs.Set(new ObjectReference { Value = value }, index = objectIndexs.Count);
                    CharStream.PrepLength(Config.SetLoopObject.Length + (10 + 2 + 2));
                    CharStream.UnsafeSimpleWrite(Config.SetLoopObject);
                    CharStream.UnsafeWrite('(');
                    CharStream.WriteJson((uint)index, true);
                    CharStream.UnsafeWrite(',');
                }
#endif
            }
            return true;
        }
        /// <summary>
        /// 退出对象节点
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Pop()
        {
            if (checkLoopDepth == 0) forefather[--forefatherCount] = null;
            else
            {
                ++checkLoopDepth;
#if AutoCSer
                if (isLoopObject) CharStream.Write(')');
#endif
            }
        }

        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CustomSerialize<valueType>(valueType value)
        {
            if (value == null) CharStream.WriteJsonNull();
            else TypeSerializer<valueType>.Serialize(this, ref value);
        }
        /// <summary>
        /// 写入对象名称
        /// </summary>
        /// <param name="name"></param>
        public void CustomWriteFirstName(string name)
        {
            CharStream.PrepLength(name.Length + (5 + 1));
            CharStream.UnsafeWrite('{');
            CharStream.UnsafeWrite('"');
            CharStream.UnsafeSimpleWrite(name);
            CharStream.UnsafeWrite('"');
            CharStream.UnsafeWrite(':');
        }
        /// <summary>
        /// 写入对象名称
        /// </summary>
        /// <param name="name"></param>
        public void CustomWriteNextName(string name)
        {
            CharStream.PrepLength(name.Length + (5 + 1));
            CharStream.UnsafeWrite(',');
            CharStream.UnsafeWrite('"');
            CharStream.UnsafeSimpleWrite(name);
            CharStream.UnsafeWrite('"');
            CharStream.UnsafeWrite(':');
        }
        /// <summary>
        /// 写入对象结束括号
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CustomObjectEnd()
        {
            CharStream.Write('}');
        }
        /// <summary>
        /// 写入数组开始括号
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CustomArrayStart()
        {
            CharStream.Write('[');
        }
        /// <summary>
        /// 写入数组结束括号
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CustomArrayEnd()
        {
            CharStream.Write(']');
        }

        /// <summary>
        /// 引用类型对象转换JSON字符串
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void classSerialize<valueType>(valueType value)
        {
            if (value == null) CharStream.WriteJsonNull();
            else TypeSerializer<valueType>.ClassSerialize(this, value);
        }
        /// <summary>
        /// 值类型对象转换JSON字符串
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structSerialize<valueType>(valueType value)
        {
            TypeSerializer<valueType>.StructSerialize(this, value);
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void baseSerialize<valueType, childType>(Serializer jsonSerializer, childType value) where childType : valueType
        {
            TypeSerializer<valueType>.ClassSerialize(jsonSerializer, value);
        }
        /// <summary>
        /// object转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal static void serializeObject<valueType>(Serializer jsonSerializer, object value)
        {
            TypeSerializer<valueType>.Serialize(jsonSerializer, (valueType)value);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void array<valueType>(valueType[] array)
        {
            if (array == null) CharStream.WriteJsonNull();
            else if (Push(array))
            {
                TypeSerializer<valueType>.Array(this, array);
                Pop();
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">数据对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void EnumToString<valueType>(valueType value)
        {
            string stringValue = value.ToString();
            char charValue = stringValue[0];
            if ((uint)(charValue - '1') < 9 || charValue == '-') CharStream.SimpleWriteNotNull(stringValue);
            else
            {
                CharStream.PrepLength(stringValue.Length + 2);
                CharStream.UnsafeWrite('"');
                CharStream.UnsafeWrite(stringValue);
                CharStream.UnsafeWrite('"');
            }
        }
        /// <summary>
        /// 字典转换
        /// </summary>
        /// <param name="dictionary">数据对象</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void dictionary<valueType, dictionaryValueType>(Dictionary<valueType, dictionaryValueType> dictionary)
        {
            if (dictionary == null) CharStream.WriteJsonNull();
            else if (Push(dictionary))
            {
                TypeSerializer<valueType>.Dictionary(this, dictionary);
                Pop();
            }
        }
        /// <summary>
        /// 字典转换
        /// </summary>
        /// <param name="dictionary">字典</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void stringDictionary<valueType>(Dictionary<string, valueType> dictionary)
        {
            if (dictionary == null) CharStream.WriteJsonNull();
            else if (Push(dictionary))
            {
                if (Config.IsStringDictionaryToObject) TypeSerializer<valueType>.StringDictionary(this, dictionary);
                else TypeSerializer<string>.Dictionary<valueType>(this, dictionary);
                Pop();
            }
        }
        /// <summary>
        /// 值类型对象转换JSON字符串
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void nullableSerialize<valueType>(Nullable<valueType> value) where valueType : struct
        {
            if (value.HasValue) TypeSerializer<valueType>.StructSerialize(this, value.Value);
            else CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 字典转换JSON字符串
        /// </summary>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void keyValuePairSerialize<keyValue, valueType>(KeyValuePair<keyValue, valueType> value)
        {
            TypeSerializer<keyValue>.KeyValuePair(this, value);
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void structEnumerable<valueType, elementType>(valueType value) where valueType : IEnumerable<elementType>
        {
            TypeSerializer<elementType>.Enumerable(this, value);
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="value">枚举集合</param>
        //[AutoCSer.IOS.Preserve(Conditional = true)]
        internal void enumerable<valueType, elementType>(valueType value) where valueType : IEnumerable<elementType>
        {
            if (value == null) CharStream.WriteJsonNull();
            else if (pushArray(value))
            {
                TypeSerializer<elementType>.Enumerable(this, value);
                Pop();
            }
        }

        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        public static SerializeResult Serialize<valueType>(valueType value, SerializeConfig config = null)
        {
            Serializer jsonSerializer = YieldPool.Default.Pop() ?? new Serializer();
            try
            {
                string json = jsonSerializer.serialize<valueType>(ref value, config);
                return new SerializeResult { Json = json, Warning = jsonSerializer.Warning };
            }
            finally { jsonSerializer.Free(); }
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="jsonStream">JSON 输出缓冲区</param>
        /// <param name="config">配置参数</param>
        public static SerializeWarning Serialize<valueType>(valueType value, CharStream jsonStream, SerializeConfig config = null)
        {
            Serializer jsonSerializer = YieldPool.Default.Pop() ?? new Serializer();
            try
            {
                jsonSerializer.serialize<valueType>(ref value, jsonStream, config);
                return jsonSerializer.Warning;
            }
            finally { jsonSerializer.Free(); }
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="jsonStream">JSON 输出缓冲区</param>
        /// <param name="config">配置参数</param>
        public static SerializeWarning Serialize<valueType>(ref valueType value, CharStream jsonStream, SerializeConfig config = null)
        {
            Serializer jsonSerializer = YieldPool.Default.Pop() ?? new Serializer();
            try
            {
                jsonSerializer.serialize<valueType>(ref value, jsonStream, config);
                return jsonSerializer.Warning;
            }
            finally { jsonSerializer.Free(); }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <param name="config"></param>
        internal static void Serialize<valueType>(valueType value, UnmanagedStream stream, SerializeConfig config)
        {
            int index = stream.AddSize(sizeof(int));
            Serializer jsonSerializer = YieldPool.Default.Pop() ?? new Serializer();
            try
            {
                jsonSerializer.serialize<valueType>(ref value, stream, config);
            }
            finally { jsonSerializer.Free(); }
            int length = stream.ByteSize - index;
            *(int*)(stream.Data.Byte + index - sizeof(int)) = length;
            if ((length & 2) != 0) stream.Write(' ');
        }
        /// <summary>
        /// 对象转换 JSON 字符串（单线程模式）
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        public static SerializeResult SerializeThread<valueType>(valueType value, SerializeConfig config = null)
        {
            Serializer jsonSerializer = ThreadStatic.Get().Serializer;
            try
            {
                string json = jsonSerializer.serializeThread<valueType>(ref value, config);
                return new SerializeResult { Json = json, Warning = jsonSerializer.Warning };
            }
            finally { jsonSerializer.freeThread(); }
        }

        static Serializer()
        {
            serializeMethods = AutoCSer.DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(Serializer).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(SerializeMethod), false))
                {
                    serializeMethods.Add(method.GetParameters()[0].ParameterType, method);
                }
            }
        }
    }
}

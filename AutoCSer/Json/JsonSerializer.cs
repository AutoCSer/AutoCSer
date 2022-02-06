using System;
using System.Reflection;
using AutoCSer.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Json;
using AutoCSer.Memory;

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer : AutoCSer.Threading.Link<JsonSerializer>, IDisposable
    {
        /// <summary>
        /// 最大整数值
        /// </summary>
        internal const long MaxInteger = (1L << 52) - 1;
        /// <summary>
        /// JSON 自定义全局配置
        /// </summary>
        public static readonly CustomConfig CustomConfig = (CustomConfig)AutoCSer.Configuration.Common.Get(typeof(CustomConfig)) ?? new CustomConfig();
        /// <summary>
        /// 默认序列化类型配置
        /// </summary>
        internal static readonly JsonSerializeAttribute AllMemberAttribute = (JsonSerializeAttribute)AutoCSer.Configuration.Common.Get(typeof(JsonSerializeAttribute)) ?? new JsonSerializeAttribute { IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly SerializeConfig DefaultConfig = (SerializeConfig)AutoCSer.Configuration.Common.Get(typeof(SerializeConfig)) ?? new SerializeConfig();
        /// <summary>
        /// 字符串输出缓冲区
        /// </summary>
        public readonly CharStream CharStream;
        /// <summary>
        /// 获取字符串输出缓冲区
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static CharStream GetCharStream(JsonSerializer jsonSerializer)
        {
            return jsonSerializer.CharStream;
        }
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
        internal JsonSerializer(bool isThread = false)
        {
            CharStream = isThread ? new CharStream() : new CharStream(default(AutoCSer.Memory.Pointer));
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
        /// <returns>Json序列化结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private SerializeResult serializeResult<valueType>(ref valueType value, SerializeConfig config)
        {
            return new SerializeResult { Json = serialize(ref value, config), Warning = Warning };
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
            AutoCSer.Memory.Pointer buffer = UnmanagedPool.Default.GetPointer();
            try
            {
                CharStream.Reset(ref buffer);
                using (CharStream)
                {
                    serialize(ref value);
                    return CharStream.ToString();
                }
            }
            finally { UnmanagedPool.Default.PushOnly(ref buffer); }
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
        private string serializeThreadStatic<valueType>(ref valueType value, SerializeConfig config)
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
        public void TypeSerialize<valueType>(ref valueType value) where valueType : struct
        {
            TypeSerializer<valueType>.StructSerialize(this, ref value);
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
        private void freeThreadStatic()
        {
            CharStream.Clear();
            Config = DefaultConfig;
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
            freeThreadStatic();
            YieldPool.Default.Push(this);
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
                        CharStream.PrepCharSize(Config.GetLoopObject.Length + (10 + 5 + 3));
                        CharStream.Data.SimpleWrite(Config.GetLoopObject);
                        CharStream.Data.Write('(');
                        CharStream.WriteJsonHex((uint)index);
                        CharStream.Data.Write(',' + ('[' << 16) + ((long)']' << 32) + ((long)')' << 48));
                        return false;
                    }
                    objectIndexs.Set(new ObjectReference { Value = value }, index = objectIndexs.Count);
                    CharStream.PrepCharSize(Config.SetLoopObject.Length + (10 + 2 + 2));
                    CharStream.Data.SimpleWrite(Config.SetLoopObject);
                    CharStream.Data.Write('(');
                    CharStream.WriteJsonHex((uint)index);
                    CharStream.Data.Write(',');
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
                        CharStream.PrepCharSize(Config.GetLoopObject.Length + (10 + 2 + 2));
                        CharStream.Data.SimpleWrite(Config.GetLoopObject);
                        CharStream.Data.Write('(');
                        CharStream.WriteJsonHex((uint)index);
                        CharStream.Data.Write(')');
                        return false;
                    }
                    objectIndexs.Set(new ObjectReference { Value = value }, index = objectIndexs.Count);
                    CharStream.PrepCharSize(Config.SetLoopObject.Length + (10 + 2 + 2));
                    CharStream.Data.SimpleWrite(Config.SetLoopObject);
                    CharStream.Data.Write('(');
                    CharStream.WriteJsonHex((uint)index);
                    CharStream.Data.Write(',');
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
        /// 自定义序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        internal static void NotSupport<T>(JsonSerializer jsonSerializer, T value)
        {
            int size = CustomConfig.NotSupport(jsonSerializer, value);
            if (size > 0) jsonSerializer.CharStream.Data.CurrentIndex += size << 1;
        }
        /// <summary>
        /// 写入对象名称
        /// </summary>
        /// <param name="name"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CustomWriteFirstName(string name)
        {
            CharStream.WriteJsonCustomNameFirst(name);
        }
        /// <summary>
        /// 写入对象名称
        /// </summary>
        /// <param name="name"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CustomWriteNextName(string name)
        {
            CharStream.WriteJsonCustomNameNext(name);
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
        /// <param name="jsonSerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void ClassSerialize<valueType>(JsonSerializer jsonSerializer, valueType value)
        {
            if (value == null) jsonSerializer.CharStream.WriteJsonNull();
            else TypeSerializer<valueType>.ClassSerialize(jsonSerializer, value);
        }
        /// <summary>
        /// 值类型对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructSerialize<valueType>(JsonSerializer jsonSerializer, valueType value)
        {
            TypeSerializer<valueType>.StructSerialize(jsonSerializer, ref value);
        }
        /// <summary>
        /// 基类转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static void baseSerialize<valueType, childType>(JsonSerializer jsonSerializer, childType value) where childType : valueType
        {
            TypeSerializer<valueType>.ClassSerialize(jsonSerializer, value);
        }
        /// <summary>
        /// object转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void SerializeObject<valueType>(JsonSerializer jsonSerializer, object value)
        {
            TypeSerializer<valueType>.Serialize(jsonSerializer, (valueType)value);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        private void array<valueType>(valueType[] array)
        {
            if (array == null) CharStream.WriteJsonNull();
            else if (Push(array))
            {
                TypeSerializer<valueType>.Array(this, array);
                Pop();
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Array<valueType>(JsonSerializer jsonSerializer, valueType[] array)
        {
            jsonSerializer.array(array);
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="array">数组对象</param>
        private void structArray<valueType>(valueType[] array) where valueType : struct
        {
            if (array == null) CharStream.WriteJsonNull();
            else if (Push(array))
            {
                TypeSerializer<valueType>.StructArray(this, array);
                Pop();
            }
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="array">数组对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructArray<valueType>(JsonSerializer jsonSerializer, valueType[] array) where valueType : struct
        {
            jsonSerializer.structArray(array);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">数据对象</param>
        internal static void EnumToString<valueType>(JsonSerializer jsonSerializer, valueType value)
        {
            TypeSerializer<valueType>.EnumToString(jsonSerializer, value);
        }
        /// <summary>
        /// 字典转换
        /// </summary>
        /// <param name="dictionary">数据对象</param>
        private void dictionary<valueType, dictionaryValueType>(Dictionary<valueType, dictionaryValueType> dictionary)
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
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Dictionary<valueType, dictionaryValueType>(JsonSerializer jsonSerializer, Dictionary<valueType, dictionaryValueType> dictionary)
        {
            jsonSerializer.dictionary<valueType, dictionaryValueType>(dictionary);
        }
        /// <summary>
        /// 字典转换
        /// </summary>
        /// <param name="dictionary">字典</param>
        private void stringDictionary<valueType>(Dictionary<string, valueType> dictionary)
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
        /// 字典转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary">字典</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StringDictionary<valueType>(JsonSerializer jsonSerializer, Dictionary<string, valueType> dictionary)
        {
            jsonSerializer.stringDictionary(dictionary);
        }
        /// <summary>
        /// 值类型对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullableSerialize<valueType>(JsonSerializer jsonSerializer, Nullable<valueType> value) where valueType : struct
        {
            if (value.HasValue) TypeSerializer<valueType>.StructSerialize(jsonSerializer, value.Value);
            else jsonSerializer.CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 字典转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void KeyValuePairSerialize<keyValue, valueType>(JsonSerializer jsonSerializer, KeyValuePair<keyValue, valueType> value)
        {
            TypeSerializer<keyValue>.KeyValuePair(jsonSerializer, value);
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void StructEnumerable<valueType, elementType>(JsonSerializer jsonSerializer, valueType value) where valueType : IEnumerable<elementType>
        {
            TypeSerializer<elementType>.Enumerable(jsonSerializer, value);
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="value">枚举集合</param>
        private void enumerable<valueType, elementType>(valueType value) where valueType : IEnumerable<elementType>
        {
            if (value == null) CharStream.WriteJsonNull();
            else if (pushArray(value))
            {
                TypeSerializer<elementType>.Enumerable(this, value);
                Pop();
            }
        }
        /// <summary>
        /// 枚举集合转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">枚举集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Enumerable<valueType, elementType>(JsonSerializer jsonSerializer, valueType value) where valueType : IEnumerable<elementType>
        {
            jsonSerializer.enumerable<valueType, elementType>(value);
        }

        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 序列化结果</returns>
        public static SerializeResult SerializeResult<valueType>(valueType value, SerializeConfig config = null)
        {
            JsonSerializer jsonSerializer = YieldPool.Default.Pop() ?? new JsonSerializer();
            try
            {
                return jsonSerializer.serializeResult<valueType>(ref value, config);
            }
            finally { jsonSerializer.Free(); }
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        public static string Serialize<valueType>(valueType value, SerializeConfig config = null)
        {
            JsonSerializer jsonSerializer = YieldPool.Default.Pop() ?? new JsonSerializer();
            try
            {
                return jsonSerializer.serialize<valueType>(ref value, config);
            }
            finally { jsonSerializer.Free(); }
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 序列化结果</returns>
        public static SerializeResult SerializeResult<valueType>(ref valueType value, SerializeConfig config = null)
        {
            JsonSerializer jsonSerializer = YieldPool.Default.Pop() ?? new JsonSerializer();
            try
            {
                return jsonSerializer.serializeResult<valueType>(ref value, config);
            }
            finally { jsonSerializer.Free(); }
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        public static string Serialize<valueType>(ref valueType value, SerializeConfig config = null)
        {
            JsonSerializer jsonSerializer = YieldPool.Default.Pop() ?? new JsonSerializer();
            try
            {
                return jsonSerializer.serialize<valueType>(ref value, config);
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
            JsonSerializer jsonSerializer = YieldPool.Default.Pop() ?? new JsonSerializer();
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
            JsonSerializer jsonSerializer = YieldPool.Default.Pop() ?? new JsonSerializer();
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
        internal static void Serialize<valueType>(ref valueType value, UnmanagedStream stream, SerializeConfig config)
        {
            int index = stream.AddSize(sizeof(int));
            JsonSerializer jsonSerializer = YieldPool.Default.Pop() ?? new JsonSerializer();
            try
            {
                jsonSerializer.serialize<valueType>(ref value, stream, config);
            }
            finally { jsonSerializer.Free(); }
            int length = stream.Data.CurrentIndex - index;
            *(int*)(stream.Data.Byte + (index - sizeof(int))) = length;
            if ((length & 2) != 0) stream.Write(' ');
        }


        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string ThreadStaticSerialize<T>(T value, SerializeConfig config = null)
        {
            SerializeWarning warning;
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string ThreadStaticSerialize<T>(ref T value, SerializeConfig config = null)
        {
            SerializeWarning warning;
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static string ThreadStaticSerialize<T>(T value, out SerializeWarning warning, SerializeConfig config = null)
        {
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        public static string ThreadStaticSerialize<T>(ref T value, out SerializeWarning warning, SerializeConfig config = null)
        {
            JsonSerializer jsonSerializer = ThreadStaticSerializer.Get().Serializer;
            try
            {
                string json = jsonSerializer.serializeThreadStatic(ref value, config);
                warning = jsonSerializer.Warning;
                return json;
            }
            finally { jsonSerializer.freeThreadStatic(); }
        }

        static JsonSerializer()
        {
            serializeMethods = AutoCSer.DictionaryCreator.CreateOnly<Type, MethodInfo>();
            foreach (MethodInfo method in typeof(JsonSerializer).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.IsDefined(typeof(SerializeMethod), false))
                {
                    serializeMethods.Add(method.GetParameters()[0].ParameterType, method);
                }
            }
        }
    }
}

using System;
using System.Reflection;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class Serializer
    {
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="onSerializeStream">序列化以后的数据流处理事件</param>
        /// <param name="config">配置参数</param>
        private void serialize<valueType>(ref valueType value, Action<CharStream> onSerializeStream, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            byte* buffer = AutoCSer.UnmanagedPool.Default.Get();
            try
            {
                CharStream.Reset((byte*)buffer, AutoCSer.UnmanagedPool.DefaultSize);
                using (CharStream)
                {
                    serialize(ref value);
                    onSerializeStream(CharStream);
                }
            }
            finally { AutoCSer.UnmanagedPool.Default.Push(buffer); }
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="onSerializeStream">序列化以后的数据流处理事件</param>
        /// <param name="config">配置参数</param>
        public static SerializeWarning Serialize<valueType>(valueType value, Action<CharStream> onSerializeStream, SerializeConfig config = null)
        {
            if (onSerializeStream == null) throw new ArgumentNullException();
            Serializer jsonSerializer = YieldPool.Default.Pop() ?? new Serializer();
            try
            {
                jsonSerializer.serialize<valueType>(ref value, onSerializeStream, config);
                return jsonSerializer.Warning;
            }
            finally { jsonSerializer.Free(); }
        }
        ///// <summary>
        ///// 创建默认配置 JSON 序列化
        ///// </summary>
        ///// <param name="jsonStream"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public static Serializer CreateDefault(out CharStream jsonStream)
        //{
        //    Serializer serializer = new Serializer();
        //    serializer.SetTcpServer();
        //    (jsonStream = serializer.CharStream).Reset();
        //    return serializer;
        //}
        /// <summary>
        /// 初始化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetTcpServer()
        {
            Config = AutoCSer.Net.TcpServer.Server.JsonConfig;
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
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        internal void SerializeTcpServer<valueType>(ref valueType value, UnmanagedStream stream)
            where valueType : struct
        {
            int index = stream.ByteSize;
            if (forefatherCount != 0)
            {
                System.Array.Clear(forefather, 0, forefatherCount);
                forefatherCount = 0;
            }
            CharStream.From(stream);
            try
            {
                //Warning = SerializeWarning.None;
                TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
            }
            finally { stream.From(CharStream); }
            if (((stream.ByteSize - index) & 2) != 0) stream.Write(' ');
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        internal void SerializeTcpServer<valueType>(valueType value, UnmanagedStream stream)
        {
            if (value == null)
            {
                *(long*)stream.GetPrepSizeCurrent(8) = 'n' + ('u' << 16) + ((long)'l' << 32) + ((long)'l' << 48);
                stream.ByteSize += 4 * sizeof(char);
            }
            else
            {
                int index = stream.ByteSize;
                if (forefatherCount != 0)
                {
                    System.Array.Clear(forefather, 0, forefatherCount);
                    forefatherCount = 0;
                }
                CharStream.From(stream);
                try
                {
                    //Warning = SerializeWarning.None;
                    TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
                }
                finally { stream.From(CharStream); }
                if (((stream.ByteSize - index) & 2) != 0) stream.Write(' ');
            }
        }

        /// <summary>
        /// 预编译类型
        /// </summary>
        /// <param name="types"></param>
        internal static void Compile(Type[] types)
        {
            foreach (Type type in types)
            {
                if (type != null) GenericType.Get(type).JsonDeSerializeCompile(); 
            }
        }
    }
}

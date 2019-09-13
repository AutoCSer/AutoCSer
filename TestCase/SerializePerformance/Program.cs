using System;
using System.Diagnostics;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SerializePerformance
{
    class Program
    {
        static void Main(string[] args)
        {
#if NOJIT
            int count = 10 * 10000;
#else
            int count = 100 * 10000;
#endif
            bool isJson = true, isJsonThread = true, isXml = true, isBinary = true;
            AutoCSer.RandomObject.Config randomConfig = new AutoCSer.RandomObject.Config { IsSecondDateTime = true, IsParseFloat = true };
            do
            {
                //AOT（NoJIT）模式应该尽量使用属性而非字段
                PropertyData propertyData = AutoCSer.RandomObject.Creator<PropertyData>.Create(randomConfig);
                if (isJson) json(propertyData, "Property Json", count);
                if (isJsonThread) jsonThread(propertyData, "Property Json", count);
                if (isXml) xml(propertyData, "Property Json", count);

                //浮点数不仅存在精度问题，而且序列化性能非常低，应该尽量避免使用。特别是.NET Core 的实现，指数越高性能越低。
                FloatPropertyData floatPropertyData = AutoCSer.RandomObject.Creator<FloatPropertyData>.Create(randomConfig);
                if (isJson) json(floatPropertyData, "float Property Json", count);
                if (isJsonThread) jsonThread(floatPropertyData, "float Property Json", count);
                if (isXml) xml(floatPropertyData, "float Property Json", count);

                FieldData filedData = AutoCSer.RandomObject.Creator<FieldData>.Create(randomConfig);
                if (isJson) json(filedData, "Field Json", count);
                if (isJsonThread) jsonThread(filedData, "Field Json", count);
                if (isXml) xml(filedData, "Field Json", count);

                //浮点数不仅存在精度问题，而且序列化性能非常低，应该尽量避免使用。特别是.NET Core 的实现，指数越高性能越低。
                FloatFieldData floatFiledData = AutoCSer.RandomObject.Creator<FloatFieldData>.Create(randomConfig);
                if (isJson) json(floatFiledData, "float Field Json", count);
                if (isJsonThread) jsonThread(floatFiledData, "float Field Json", count);
                if (isXml) xml(floatFiledData, "float Field Json", count);

                //AOT（NoJIT）模式尽量不要使用二进制序列化
                //浮点数对二进制序列化无影响
                if (isBinary) fieldSerialize(floatFiledData, count);

                Console.WriteLine(@"Sleep 3000ms
");
                System.Threading.Thread.Sleep(3000);
            }
            while (true);
        }

        /// <summary>
        /// JSON 序列化测试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="title"></param>
        /// <param name="isOutput"></param>
        private static void json<T>(T value, string type, int count, string title = null, bool isOutput = false)
        {
            Console.WriteLine(title ?? @"
http://www.AutoCSer.com/Serialize/Json.html");
            string json = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) json = AutoCSer.Json.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((json.Length * count * 2) >> 20).toString() + "MB "+ type + " Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            if (isOutput) Console.WriteLine(json);

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Json.Parser.Parse<T>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((json.Length * count * 2) >> 20).toString() + "MB " + type + " DeSerialize " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// JSON 序列化测试（单线程模式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="title"></param>
        /// <param name="isOutput"></param>
        private static void jsonThread<T>(T value, string type, int count, string title = null, bool isOutput = false)
        {
            Console.WriteLine(title ?? @"
http://www.AutoCSer.com/Serialize/Json.html");
            string json = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) json = AutoCSer.Json.Serializer.SerializeThread(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((json.Length * count * 2) >> 20).toString() + "MB " + type + " Thread Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            if (isOutput) Console.WriteLine(json);

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Json.Parser.ParseThread<T>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((json.Length * count * 2) >> 20).toString() + "MB " + type + " Thread DeSerialize " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// XML 序列化测试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="title"></param>
        /// <param name="isOutput"></param>
        private static void xml<T>(T value, string type, int count, string title = null, bool isOutput = false)
        {
            Console.WriteLine(title ?? @"
http://www.AutoCSer.com/Serialize/Xml.html");
            string xml = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) xml = AutoCSer.Xml.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((xml.Length * count * 2) >> 20).toString() + "MB " + type + " Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            if (isOutput) Console.WriteLine(xml);

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Xml.Parser.Parse<T>(xml);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((xml.Length * count * 2) >> 20).toString() + "MB " + type + " DeSerialize " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// 二进制序列化测试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="count"></param>
        private static void fieldSerialize<T>(T value, int count)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Binary.html");
            byte[] bytes = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) bytes = AutoCSer.BinarySerialize.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((bytes.Length * count) >> 20).toString() + "MB Binary Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<T>(bytes);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((bytes.Length * count) >> 20).toString() + "MB Binary DeSerialize " + time.ElapsedMilliseconds.toString() + "ms");
        }
    }
}

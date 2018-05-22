using System;
using System.Diagnostics;
using AutoCSer.Extension;

namespace AutoCSer.TestCase.SerializePerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                //AOT（NoJIT）模式应该尽量使用属性而非字段
                PropertyData propertyData = AutoCSer.RandomObject.Creator<PropertyData>.Create(randomConfig);
                json(propertyData);
                xml(propertyData);

                //浮点数不仅存在精度问题，而且序列化性能非常低，应该尽量避免使用。特别是.NET Core 的实现，指数越高性能越低。
                FloatPropertyData floatPropertyData = AutoCSer.RandomObject.Creator<FloatPropertyData>.Create(randomConfig);
                json(floatPropertyData);
                xml(floatPropertyData);

                FieldData filedData = AutoCSer.RandomObject.Creator<FieldData>.Create(randomConfig);
                json(filedData);
                xml(filedData);

                //浮点数不仅存在精度问题，而且序列化性能非常低，应该尽量避免使用。特别是.NET Core 的实现，指数越高性能越低。
                FloatFieldData floatFiledData = AutoCSer.RandomObject.Creator<FloatFieldData>.Create(randomConfig);
                json(floatFiledData);
                xml(floatFiledData);

                //AOT（NoJIT）模式尽量不要使用二进制序列化
                //浮点数对二进制序列化无影响
                fieldSerialize(floatFiledData);

                Console.WriteLine(@"Sleep 3000ms
");
                System.Threading.Thread.Sleep(3000);
            }
            while (true);
        }

        /// <summary>
        /// 测试对象数量
        /// </summary>
#if NOJIT
        private const int count = 10 * 10000;
#else
        private const int count = 100 * 10000;
#endif
        /// <summary>
        /// 随机生成对象参数
        /// </summary>
        private static readonly AutoCSer.RandomObject.Config randomConfig = new AutoCSer.RandomObject.Config { IsSecondDateTime = true, IsParseFloat = true };

        /// <summary>
        /// JSON 序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void json(PropertyData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Json.html");
            string json = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) json = AutoCSer.Json.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object "+ ((json.Length * count * 2) >> 20).toString() + "MB Property Json Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Json.Parser.Parse<PropertyData>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((json.Length * count * 2) >> 20).toString() + "MB Property Json Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// JSON 序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void json(FloatPropertyData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Json.html");
            string json = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) json = AutoCSer.Json.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object + float " + ((json.Length * count * 2) >> 20).toString() + "MB Property Json Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Json.Parser.Parse<FloatPropertyData>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object + float " + ((json.Length * count * 2) >> 20).toString() + "MB Property Json Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// JSON序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void json(FieldData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Json.html");
            string json = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) json = AutoCSer.Json.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((json.Length * count * 2) >> 20).toString() + "MB Field Json Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Json.Parser.Parse<FieldData>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((json.Length * count * 2) >> 20).toString() + "MB Field Json Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// JSON序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void json(FloatFieldData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Json.html");
            string json = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) json = AutoCSer.Json.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object + float " + ((json.Length * count * 2) >> 20).toString() + "MB Field Json Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Json.Parser.Parse<FloatFieldData>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object + float " + ((json.Length * count * 2) >> 20).toString() + "MB Field Json Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// XML 序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void xml(PropertyData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Xml.html");
            string xml = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) xml = AutoCSer.Xml.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((xml.Length * count * 2) >> 20).toString() + "MB Property XML Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Xml.Parser.Parse<PropertyData>(xml);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((xml.Length * count * 2) >> 20).toString() + "MB Property XML Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// XML 序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void xml(FloatPropertyData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Xml.html");
            string xml = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) xml = AutoCSer.Xml.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object + float " + ((xml.Length * count * 2) >> 20).toString() + "MB Property XML Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Xml.Parser.Parse<FloatPropertyData>(xml);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object + float " + ((xml.Length * count * 2) >> 20).toString() + "MB Property XML Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// XML序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void xml(FieldData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Xml.html");
            string xml = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) xml = AutoCSer.Xml.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((xml.Length * count * 2) >> 20).toString() + "MB XML Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Xml.Parser.Parse<FieldData>(xml);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((xml.Length * count * 2) >> 20).toString() + "MB XML Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// XML序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void xml(FloatFieldData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Xml.html");
            string xml = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) xml = AutoCSer.Xml.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object + float " + ((xml.Length * count * 2) >> 20).toString() + "MB XML Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Xml.Parser.Parse<FloatFieldData>(xml);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object + float " + ((xml.Length * count * 2) >> 20).toString() + "MB XML Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// 二进制序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void fieldSerialize(FloatFieldData value)
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
            for (int index = count; index != 0; --index) value = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<FloatFieldData>(bytes);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object " + ((bytes.Length * count) >> 20).toString() + "MB Binary DeSerialize " + time.ElapsedMilliseconds.toString() + "ms");
        }
    }
}

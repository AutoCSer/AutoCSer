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
                PropertyData propertyData = AutoCSer.RandomObject.Creator<PropertyData>.Create(randomConfig);
                json(propertyData);

                FieldData filedData = AutoCSer.RandomObject.Creator<FieldData>.Create(randomConfig);
                json(filedData);

                xml(filedData);

                fieldSerialize(filedData);

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
        /// JSON序列化测试
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
            Console.WriteLine((count / 10000).toString() + "W object Property Json Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Json.Parser.Parse<PropertyData>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object Property Json Parse " + time.ElapsedMilliseconds.toString() + "ms");
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
            Console.WriteLine((count / 10000).toString() + "W object Field Json Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Json.Parser.Parse<FieldData>(json);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object Field Json Parse " + time.ElapsedMilliseconds.toString() + "ms");
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
            Console.WriteLine((count / 10000).toString() + "W object XML Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.Xml.Parser.Parse<FieldData>(xml);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object XML Parse " + time.ElapsedMilliseconds.toString() + "ms");
        }
        /// <summary>
        /// 二进制序列化测试
        /// </summary>
        /// <param name="value"></param>
        private static void fieldSerialize(FieldData value)
        {
            Console.WriteLine(@"
http://www.AutoCSer.com/Serialize/Binary.html");
            byte[] bytes = null;
            Stopwatch time = new Stopwatch();
            time.Start();
            for (int index = count; index != 0; --index) bytes = AutoCSer.BinarySerialize.Serializer.Serialize(value);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object Binary Serialize " + time.ElapsedMilliseconds.toString() + "ms");

            time.Reset();
            time.Start();
            for (int index = count; index != 0; --index) value = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<FieldData>(bytes);
            time.Stop();
            Console.WriteLine((count / 10000).toString() + "W object Binary DeSerialize " + time.ElapsedMilliseconds.toString() + "ms");
        }
    }
}

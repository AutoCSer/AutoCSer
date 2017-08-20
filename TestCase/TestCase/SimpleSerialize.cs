using System;

namespace AutoCSer.TestCase
{
    class SimpleSerialize
    {
        /// <summary>
        /// 字段数据定义(值类型外壳)
        /// </summary>
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal struct Data
        {
            public bool Bool;
            public byte Byte;
            public sbyte SByte;
            public short Short;
            public ushort UShort;
            public int Int;
            public uint UInt;
            public long Long;
            public ulong ULong;
            public DateTime DateTime;
            public float Float;
            public double Double;
            public decimal Decimal;
            public Guid Guid;
            public char Char;
            public string String;
            public bool? BoolNull;
            public byte? ByteNull;
            public sbyte? SByteNull;
            public short? ShortNull;
            public ushort? UShortNull;
            public int? IntNull;
            public uint? UIntNull;
            public long? LongNull;
            public ulong? ULongNull;
            public DateTime? DateTimeNull;
            public float? FloatNull;
            public double? DoubleNull;
            public decimal? DecimalNull;
            public Guid? GuidNull;
            public char? CharNull;
            public TestCase.Data.ByteEnum ByteEnum;
            public TestCase.Data.ByteFlagEnum ByteFlagEnum;
            public TestCase.Data.SByteEnum SByteEnum;
            public TestCase.Data.SByteFlagEnum SByteFlagEnum;
            public TestCase.Data.ShortEnum ShortEnum;
            public TestCase.Data.ShortFlagEnum ShortFlagEnum;
            public TestCase.Data.UShortEnum UShortEnum;
            public TestCase.Data.UShortFlagEnum UShortFlagEnum;
            public TestCase.Data.IntEnum IntEnum;
            public TestCase.Data.IntFlagEnum IntFlagEnum;
            public TestCase.Data.UIntEnum UIntEnum;
            public TestCase.Data.UIntFlagEnum UIntFlagEnum;
            public TestCase.Data.LongEnum LongEnum;
            public TestCase.Data.LongFlagEnum LongFlagEnum;
            public TestCase.Data.ULongEnum ULongEnum;
            public TestCase.Data.ULongFlagEnum ULongFlagEnum;
        }
        /// <summary>
        /// 简单序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal unsafe static bool TestCase()
        {
            try
            {
                Data data = AutoCSer.RandomObject.Creator<Data>.Create();
                using (UnmanagedStream stream = new UnmanagedStream())
                {
                    AutoCSer.Net.SimpleSerialize.TypeSerializer<Data>.Serializer(stream, ref data);
                    Data newData = default(Data);
                    if (AutoCSer.Net.SimpleSerialize.TypeDeSerializer<Data>.DeSerialize(stream.Data.Byte, ref newData, stream.CurrentData) != stream.CurrentData)
                    {
                        return false;
                    }
                    if (!AutoCSer.FieldEquals.Comparor<Data>.Equals(data, newData))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                return false;
            }
        }
    }
}

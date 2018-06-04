using System;

namespace AutoCSer.TestCase.CacheServer
{
    /// <summary>
    /// 测试数据
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    class Data
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
        public Guid? GuidNull;
        public char? CharNull;
    }
}

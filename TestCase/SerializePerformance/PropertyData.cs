using System;

#pragma warning disable
namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 属性测试数据
    /// </summary>
    class PropertyData
    {
        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public int Int { get; set; }
        public uint UInt { get; set; }
        public long Long { get; set; }
        public ulong ULong { get; set; }
        public DateTime DateTime { get; set; }
        public Guid Guid { get; set; }
        public char Char { get; set; }
        public string String { get; set; }
        public bool? BoolNull { get; set; }
        public byte? ByteNull { get; set; }
        public sbyte? SByteNull { get; set; }
        public short? ShortNull { get; set; }
        public ushort? UShortNull { get; set; }
        public int? IntNull { get; set; }
        public uint? UIntNull { get; set; }
        public long? LongNull { get; set; }
        public ulong? ULongNull { get; set; }
        public DateTime? DateTimeNull { get; set; }
        public Guid? GuidNull { get; set; }
        public char? CharNull { get; set; }
    }
}
#pragma warning restore

using System;
using System.Collections.Generic;

#pragma warning disable
namespace AutoCSer.TestCase
{
    /// <summary>
    /// 测试数据
    /// </summary>
    class Data
    {
        #region 测试数据枚举定义
        internal enum ByteEnum : byte
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80
        }
        [Flags]
        internal enum ByteFlagEnum : byte
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80
        }
        internal enum SByteEnum : sbyte
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40
        }
        [Flags]
        internal enum SByteFlagEnum : sbyte
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40
        }
        internal enum ShortEnum : short
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000
        }
        [Flags]
        internal enum ShortFlagEnum : short
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000
        }
        internal enum UShortEnum : ushort
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000
        }
        [Flags]
        internal enum UShortFlagEnum : ushort
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000
        }
        internal enum IntEnum : int
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000,
            Q = 0x10000,
            R = 0x20000,
            S = 0x40000,
            T = 0x80000,
            U = 0x100000,
            V = 0x200000,
            W = 0x400000,
            X = 0x800000,
            Y = 0x1000000,
            Z = 0x2000000,
            AA = 0x4000000,
            AB = 0x8000000,
            BA = 0x10000000,
            BB = 0x20000000
        }
        [Flags]
        internal enum IntFlagEnum : int
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000,
            Q = 0x10000,
            R = 0x20000,
            S = 0x40000,
            T = 0x80000,
            U = 0x100000,
            V = 0x200000,
            W = 0x400000,
            X = 0x800000,
            Y = 0x1000000,
            Z = 0x2000000,
            AA = 0x4000000,
            AB = 0x8000000,
            BA = 0x10000000,
            BB = 0x20000000
        }
        internal enum UIntEnum : uint
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000,
            Q = 0x10000,
            R = 0x20000,
            S = 0x40000,
            T = 0x80000,
            U = 0x100000,
            V = 0x200000,
            W = 0x400000,
            X = 0x800000,
            Y = 0x1000000,
            Z = 0x2000000,
            AA = 0x4000000,
            AB = 0x8000000,
            BA = 0x10000000,
            BB = 0x20000000
        }
        [Flags]
        internal enum UIntFlagEnum : uint
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000,
            Q = 0x10000,
            R = 0x20000,
            S = 0x40000,
            T = 0x80000,
            U = 0x100000,
            V = 0x200000,
            W = 0x400000,
            X = 0x800000,
            Y = 0x1000000,
            Z = 0x2000000,
            AA = 0x4000000,
            AB = 0x8000000,
            BA = 0x10000000,
            BB = 0x20000000,
        }
        internal enum LongEnum : long
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000,
            Q = 0x10000,
            R = 0x20000,
            S = 0x40000,
            T = 0x80000,
            U = 0x100000,
            V = 0x200000,
            W = 0x400000,
            X = 0x800000,
            Y = 0x1000000,
            Z = 0x2000000,
            AA = 0x400000000000000,
            AB = 0x800000000000000,
            BA = 0x1000000000000000,
            BB = 0x2000000000000000
        }
        [Flags]
        internal enum LongFlagEnum : long
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000,
            Q = 0x10000,
            R = 0x20000,
            S = 0x40000,
            T = 0x80000,
            U = 0x100000,
            V = 0x200000,
            W = 0x400000,
            X = 0x800000,
            Y = 0x1000000,
            Z = 0x2000000,
            AA = 0x400000000000000,
            AB = 0x800000000000000,
            BA = 0x1000000000000000,
            BB = 0x2000000000000000
        }
        internal enum ULongEnum : ulong
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000,
            Q = 0x10000,
            R = 0x20000,
            S = 0x40000,
            T = 0x80000,
            U = 0x100000,
            V = 0x200000,
            W = 0x400000,
            X = 0x800000,
            Y = 0x1000000,
            Z = 0x2000000,
            AA = 0x400000000000000,
            AB = 0x800000000000000,
            BA = 0x1000000000000000,
            BB = 0x2000000000000000
        }
        [Flags]
        internal enum ULongFlagEnum : ulong
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 0x10,
            F = 0x20,
            G = 0x40,
            H = 0x80,
            I = 0x100,
            J = 0x200,
            K = 0x400,
            L = 0x800,
            M = 0x1000,
            N = 0x2000,
            O = 0x4000,
            P = 0x8000,
            Q = 0x10000,
            R = 0x20000,
            S = 0x40000,
            T = 0x80000,
            U = 0x100000,
            V = 0x200000,
            W = 0x400000,
            X = 0x800000,
            Y = 0x1000000,
            Z = 0x2000000,
            AA = 0x400000000000000,
            AB = 0x800000000000000,
            BA = 0x1000000000000000,
            BB = 0x2000000000000000
        }
        #endregion
        /// <summary>
        /// 空壳类型定义
        /// </summary>
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal class NoMemberClass
        {
        }
        /// <summary>
        /// 引用类型定义
        /// </summary>
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal class MemberClass
        {
            public string String;
            public int Int;
        }
        /// <summary>
        /// 值类型成员包装处理测试数据定义
        /// </summary>
        [AutoCSer.Metadata.BoxSerialize]
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal struct BoxStruct
        {
            public int Int;
        }
        /// <summary>
        /// 引用类型成员包装处理测试数据定义
        /// </summary>
        [AutoCSer.Metadata.BoxSerialize]
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal struct BoxClass
        {
            public MemberClass Value;
        }
        /// <summary>
        /// 字段数据定义(引用类型外壳)
        /// </summary>
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal class FieldData
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
            public ByteEnum ByteEnum;
            public ByteFlagEnum ByteFlagEnum;
            public SByteEnum SByteEnum;
            public SByteFlagEnum SByteFlagEnum;
            public ShortEnum ShortEnum;
            public ShortFlagEnum ShortFlagEnum;
            public UShortEnum UShortEnum;
            public UShortFlagEnum UShortFlagEnum;
            public IntEnum IntEnum;
            public IntFlagEnum IntFlagEnum;
            public UIntEnum UIntEnum;
            public UIntFlagEnum UIntFlagEnum;
            public LongEnum LongEnum;
            public LongFlagEnum LongFlagEnum;
            public ULongEnum ULongEnum;
            public ULongFlagEnum ULongFlagEnum;
            public KeyValuePair<string, int> KeyValuePair;
            public KeyValue<int, string> KeyValue;

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
            public ByteEnum? ByteEnumNull;
            public ByteFlagEnum? ByteFlagEnumNull;
            public SByteEnum? SByteEnumNull;
            public SByteFlagEnum? SByteFlagEnumNull;
            public ShortEnum? ShortEnumNull;
            public ShortFlagEnum? ShortFlagEnumNull;
            public UShortEnum? UShortEnumNull;
            public UShortFlagEnum? UShortFlagEnumNull;
            public IntEnum? IntEnumNull;
            public IntFlagEnum? IntFlagEnumNull;
            public UIntEnum? UIntEnumNull;
            public UIntFlagEnum? UIntFlagEnumNull;
            public LongEnum? LongEnumNull;
            public LongFlagEnum? LongFlagEnumNull;
            public ULongEnum? ULongEnumNull;
            public ULongFlagEnum? ULongFlagEnumNull;
            public KeyValuePair<string, int?>? KeyValuePairNull;
            public KeyValue<int?, string>? KeyValueNull;

            public string String;
            public string String2;
            public SubString SubString;
            public SubString SubString2;
            public MemberClass MemberClass;
            public MemberClass MemberClass2;
            public NoMemberClass NoMemberClass;
            public NoMemberClass NoMemberClass2;
            public NoMemberClass NoMemberClass3;
            public BoxStruct BoxStruct;
            public BoxStruct BoxStruct2;
            public BoxClass BoxClass;
            public BoxClass BoxClass2;

            public bool[] BoolArray;
            public byte[] ByteArray;
            public sbyte[] SByteArray;
            public short[] ShortArray;
            public ushort[] UShortArray;
            public int[] IntArray;
            public uint[] UIntArray;
            public long[] LongArray;
            public ulong[] ULongArray;
            public DateTime[] DateTimeArray;
            public float[] FloatArray;
            public double[] DoubleArray;
            public decimal[] DecimalArray;
            public Guid[] GuidArray;
            public char[] CharArray;
            public ByteEnum[] ByteEnumArray;
            public ByteFlagEnum[] ByteFlagEnumArray;
            public SByteEnum[] SByteEnumArray;
            public SByteFlagEnum[] SByteFlagEnumArray;
            public ShortEnum[] ShortEnumArray;
            public ShortFlagEnum[] ShortFlagEnumArray;
            public UShortEnum[] UShortEnumArray;
            public UShortFlagEnum[] UShortFlagEnumArray;
            public IntEnum[] IntEnumArray;
            public IntFlagEnum[] IntFlagEnumArray;
            public UIntEnum[] UIntEnumArray;
            public UIntFlagEnum[] UIntFlagEnumArray;
            public LongEnum[] LongEnumArray;
            public LongFlagEnum[] LongFlagEnumArray;
            public ULongEnum[] ULongEnumArray;
            public ULongFlagEnum[] ULongFlagEnumArray;
            public KeyValuePair<string, int>[] KeyValuePairArray;
            public KeyValue<int, string>[] KeyValueArray;

            public bool?[] BoolNullArray;
            public byte?[] ByteNullArray;
            public sbyte?[] SByteNullArray;
            public short?[] ShortNullArray;
            public ushort?[] UShortNullArray;
            public int?[] IntNullArray;
            public uint?[] UIntNullArray;
            public long?[] LongNullArray;
            public ulong?[] ULongNullArray;
            public DateTime?[] DateTimeNullArray;
            public float?[] FloatNullArray;
            public double?[] DoubleNullArray;
            public decimal?[] DecimalNullArray;
            public Guid?[] GuidNullArray;
            public char?[] CharNullArray;
            public ByteEnum?[] ByteEnumNullArray;
            public ByteFlagEnum?[] ByteFlagEnumNullArray;
            public SByteEnum?[] SByteEnumNullArray;
            public SByteFlagEnum?[] SByteFlagEnumNullArray;
            public ShortEnum?[] ShortEnumNullArray;
            public ShortFlagEnum?[] ShortFlagEnumNullArray;
            public UShortEnum?[] UShortEnumNullArray;
            public UShortFlagEnum?[] UShortFlagEnumNullArray;
            public IntEnum?[] IntEnumNullArray;
            public IntFlagEnum?[] IntFlagEnumNullArray;
            public UIntEnum?[] UIntEnumNullArray;
            public UIntFlagEnum?[] UIntFlagEnumNullArray;
            public LongEnum?[] LongEnumNullArray;
            public LongFlagEnum?[] LongFlagEnumNullArray;
            public ULongEnum?[] ULongEnumNullArray;
            public ULongFlagEnum?[] ULongFlagEnumNullArray;
            public KeyValuePair<string, int?>?[] KeyValuePairNullArray;
            public KeyValue<int?, string>?[] KeyValueNullArray;

            public string[] StringArray;
            public SubString[] SubStringArray;
            public MemberClass[] MemberClassArray;
            public NoMemberClass[] NoMemberClassArray;

            public bool[] BoolArray2;
            public byte[] ByteArray2;
            public sbyte[] SByteArray2;
            public short[] ShortArray2;
            public ushort[] UShortArray2;
            public int[] IntArray2;
            public uint[] UIntArray2;
            public long[] LongArray2;
            public ulong[] ULongArray2;
            public DateTime[] DateTimeArray2;
            public float[] FloatArray2;
            public double[] DoubleArray2;
            public decimal[] DecimalArray2;
            public Guid[] GuidArray2;
            public char[] CharArray2;
            public ByteEnum[] ByteEnumArray2;
            public ByteFlagEnum[] ByteFlagEnumArray2;
            public SByteEnum[] SByteEnumArray2;
            public SByteFlagEnum[] SByteFlagEnumArray2;
            public ShortEnum[] ShortEnumArray2;
            public ShortFlagEnum[] ShortFlagEnumArray2;
            public UShortEnum[] UShortEnumArray2;
            public UShortFlagEnum[] UShortFlagEnumArray2;
            public IntEnum[] IntEnumArray2;
            public IntFlagEnum[] IntFlagEnumArray2;
            public UIntEnum[] UIntEnumArray2;
            public UIntFlagEnum[] UIntFlagEnumArray2;
            public LongEnum[] LongEnumArray2;
            public LongFlagEnum[] LongFlagEnumArray2;
            public ULongEnum[] ULongEnumArray2;
            public ULongFlagEnum[] ULongFlagEnumArray2;
            public KeyValuePair<string, int>[] KeyValuePairArray2;
            public KeyValue<int, string>[] KeyValueArray2;

            public bool?[] BoolNullArray2;
            public byte?[] ByteNullArray2;
            public sbyte?[] SByteNullArray2;
            public short?[] ShortNullArray2;
            public ushort?[] UShortNullArray2;
            public int?[] IntNullArray2;
            public uint?[] UIntNullArray2;
            public long?[] LongNullArray2;
            public ulong?[] ULongNullArray2;
            public DateTime?[] DateTimeNullArray2;
            public float?[] FloatNullArray2;
            public double?[] DoubleNullArray2;
            public decimal?[] DecimalNullArray2;
            public Guid?[] GuidNullArray2;
            public char?[] CharNullArray2;
            public ByteEnum?[] ByteEnumNullArray2;
            public ByteFlagEnum?[] ByteFlagEnumNullArray2;
            public SByteEnum?[] SByteEnumNullArray2;
            public SByteFlagEnum?[] SByteFlagEnumNullArray2;
            public ShortEnum?[] ShortEnumNullArray2;
            public ShortFlagEnum?[] ShortFlagEnumNullArray2;
            public UShortEnum?[] UShortEnumNullArray2;
            public UShortFlagEnum?[] UShortFlagEnumNullArray2;
            public IntEnum?[] IntEnumNullArray2;
            public IntFlagEnum?[] IntFlagEnumNullArray2;
            public UIntEnum?[] UIntEnumNullArray2;
            public UIntFlagEnum?[] UIntFlagEnumNullArray2;
            public LongEnum?[] LongEnumNullArray2;
            public LongFlagEnum?[] LongFlagEnumNullArray2;
            public ULongEnum?[] ULongEnumNullArray2;
            public ULongFlagEnum?[] ULongFlagEnumNullArray2;
            public KeyValuePair<string, int?>?[] KeyValuePairNullArray2;
            public KeyValue<int?, string>?[] KeyValueNullArray2;

            public string[] StringArray2;
            public SubString[] SubStringArray2;
            public MemberClass[] MemberClassArray2;
            public NoMemberClass[] NoMemberClassArray2;
            public NoMemberClass[] NoMemberClassArray3;
            public BoxStruct[] BoxStructArray;
            public BoxStruct[] BoxStructArray2;
            public BoxClass[] BoxClassArray;
            public BoxClass[] BoxClassArray2;

            public List<bool> BoolList;
            public List<byte> ByteList;
            public List<sbyte> SByteList;
            public List<short> ShortList;
            public List<ushort> UShortList;
            public List<int> IntList;
            public List<uint> UIntList;
            public List<long> LongList;
            public List<ulong> ULongList;
            public List<DateTime> DateTimeList;
            public List<float> FloatList;
            public List<double> DoubleList;
            public List<decimal> DecimalList;
            public List<Guid> GuidList;
            public List<char> CharList;
            public List<ByteEnum> ByteEnumList;
            public List<ByteFlagEnum> ByteFlagEnumList;
            public List<SByteEnum> SByteEnumList;
            public List<SByteFlagEnum> SByteFlagEnumList;
            public List<ShortEnum> ShortEnumList;
            public List<ShortFlagEnum> ShortFlagEnumList;
            public List<UShortEnum> UShortEnumList;
            public List<UShortFlagEnum> UShortFlagEnumList;
            public List<IntEnum> IntEnumList;
            public List<IntFlagEnum> IntFlagEnumList;
            public List<UIntEnum> UIntEnumList;
            public List<UIntFlagEnum> UIntFlagEnumList;
            public List<LongEnum> LongEnumList;
            public List<LongFlagEnum> LongFlagEnumList;
            public List<ULongEnum> ULongEnumList;
            public List<ULongFlagEnum> ULongFlagEnumList;
            public List<KeyValuePair<string, int>> KeyValuePairList;
            public List<KeyValue<int, string>> KeyValueList;

            public List<bool?> BoolNullList;
            public List<byte?> ByteNullList;
            public List<sbyte?> SByteNullList;
            public List<short?> ShortNullList;
            public List<ushort?> UShortNullList;
            public List<int?> IntNullList;
            public List<uint?> UIntNullList;
            public List<long?> LongNullList;
            public List<ulong?> ULongNullList;
            public List<DateTime?> DateTimeNullList;
            public List<float?> FloatNullList;
            public List<double?> DoubleNullList;
            public List<decimal?> DecimalNullList;
            public List<Guid?> GuidNullList;
            public List<char?> CharNullList;
            public List<ByteEnum?> ByteEnumNullList;
            public List<ByteFlagEnum?> ByteFlagEnumNullList;
            public List<SByteEnum?> SByteEnumNullList;
            public List<SByteFlagEnum?> SByteFlagEnumNullList;
            public List<ShortEnum?> ShortEnumNullList;
            public List<ShortFlagEnum?> ShortFlagEnumNullList;
            public List<UShortEnum?> UShortEnumNullList;
            public List<UShortFlagEnum?> UShortFlagEnumNullList;
            public List<IntEnum?> IntEnumNullList;
            public List<IntFlagEnum?> IntFlagEnumNullList;
            public List<UIntEnum?> UIntEnumNullList;
            public List<UIntFlagEnum?> UIntFlagEnumNullList;
            public List<LongEnum?> LongEnumNullList;
            public List<LongFlagEnum?> LongFlagEnumNullList;
            public List<ULongEnum?> ULongEnumNullList;
            public List<ULongFlagEnum?> ULongFlagEnumNullList;
            public List<KeyValuePair<string, int?>?> KeyValuePairNullList;
            public List<KeyValue<int?, string>?> KeyValueNullList;

            public List<string> StringList;
            public List<SubString> SubStringList;
            public List<string> StringList2;
            public List<SubString> SubStringList2;
            public List<MemberClass> MemberClassList;
            public List<MemberClass> MemberClassList2;
            public List<NoMemberClass> NoMemberClassList;
            public List<NoMemberClass> NoMemberClassList2;
            public List<NoMemberClass> NoMemberClassList3;
            public List<BoxStruct> BoxStructList;
            public List<BoxStruct> BoxStructList2;
            public List<BoxClass> BoxClassList;
            public List<BoxClass> BoxClassList2;

            public LeftArray<bool> BoolSubArray;
            public LeftArray<byte> ByteSubArray;
            public LeftArray<sbyte> SByteSubArray;
            public LeftArray<short> ShortSubArray;
            public LeftArray<ushort> UShortSubArray;
            public LeftArray<int> IntSubArray;
            public LeftArray<uint> UIntSubArray;
            public LeftArray<long> LongSubArray;
            public LeftArray<ulong> ULongSubArray;
            public LeftArray<DateTime> DateTimeSubArray;
            public LeftArray<float> FloatSubArray;
            public LeftArray<double> DoubleSubArray;
            public LeftArray<decimal> DecimalSubArray;
            public LeftArray<Guid> GuidSubArray;
            public LeftArray<char> CharSubArray;
            public LeftArray<ByteEnum> ByteEnumSubArray;
            public LeftArray<ByteFlagEnum> ByteFlagEnumSubArray;
            public LeftArray<SByteEnum> SByteEnumSubArray;
            public LeftArray<SByteFlagEnum> SByteFlagEnumSubArray;
            public LeftArray<ShortEnum> ShortEnumSubArray;
            public LeftArray<ShortFlagEnum> ShortFlagEnumSubArray;
            public LeftArray<UShortEnum> UShortEnumSubArray;
            public LeftArray<UShortFlagEnum> UShortFlagEnumSubArray;
            public LeftArray<IntEnum> IntEnumSubArray;
            public LeftArray<IntFlagEnum> IntFlagEnumSubArray;
            public LeftArray<UIntEnum> UIntEnumSubArray;
            public LeftArray<UIntFlagEnum> UIntFlagEnumSubArray;
            public LeftArray<LongEnum> LongEnumSubArray;
            public LeftArray<LongFlagEnum> LongFlagEnumSubArray;
            public LeftArray<ULongEnum> ULongEnumSubArray;
            public LeftArray<ULongFlagEnum> ULongFlagEnumSubArray;
            public LeftArray<KeyValuePair<string, int>> KeyValuePairSubArray;
            public LeftArray<KeyValue<int, string>> KeyValueSubArray;

            public LeftArray<bool?> BoolNullSubArray;
            public LeftArray<byte?> ByteNullSubArray;
            public LeftArray<sbyte?> SByteNullSubArray;
            public LeftArray<short?> ShortNullSubArray;
            public LeftArray<ushort?> UShortNullSubArray;
            public LeftArray<int?> IntNullSubArray;
            public LeftArray<uint?> UIntNullSubArray;
            public LeftArray<long?> LongNullSubArray;
            public LeftArray<ulong?> ULongNullSubArray;
            public LeftArray<DateTime?> DateTimeNullSubArray;
            public LeftArray<float?> FloatNullSubArray;
            public LeftArray<double?> DoubleNullSubArray;
            public LeftArray<decimal?> DecimalNullSubArray;
            public LeftArray<Guid?> GuidNullSubArray;
            public LeftArray<char?> CharNullSubArray;
            public LeftArray<ByteEnum?> ByteEnumNullSubArray;
            public LeftArray<ByteFlagEnum?> ByteFlagEnumNullSubArray;
            public LeftArray<SByteEnum?> SByteEnumNullSubArray;
            public LeftArray<SByteFlagEnum?> SByteFlagEnumNullSubArray;
            public LeftArray<ShortEnum?> ShortEnumNullSubArray;
            public LeftArray<ShortFlagEnum?> ShortFlagEnumNullSubArray;
            public LeftArray<UShortEnum?> UShortEnumNullSubArray;
            public LeftArray<UShortFlagEnum?> UShortFlagEnumNullSubArray;
            public LeftArray<IntEnum?> IntEnumNullSubArray;
            public LeftArray<IntFlagEnum?> IntFlagEnumNullSubArray;
            public LeftArray<UIntEnum?> UIntEnumNullSubArray;
            public LeftArray<UIntFlagEnum?> UIntFlagEnumNullSubArray;
            public LeftArray<LongEnum?> LongEnumNullSubArray;
            public LeftArray<LongFlagEnum?> LongFlagEnumNullSubArray;
            public LeftArray<ULongEnum?> ULongEnumNullSubArray;
            public LeftArray<ULongFlagEnum?> ULongFlagEnumNullSubArray;
            public LeftArray<KeyValuePair<string, int?>?> KeyValuePairNullSubArray;
            public LeftArray<KeyValue<int?, string>?> KeyValueNullSubArray;

            public LeftArray<string> StringSubArray;
            public LeftArray<SubString> SubStringSubArray;
            public LeftArray<string> StringSubArray2;
            public LeftArray<SubString> SubStringSubArray2;
            public LeftArray<MemberClass> MemberClassSubArray;
            public LeftArray<MemberClass> MemberClassSubArray2;
            public LeftArray<BoxStruct> BoxStructSubArray;
            public LeftArray<BoxStruct> BoxStructSubArray2;
            public LeftArray<BoxClass> BoxClassSubArray;
            public LeftArray<BoxClass> BoxClassSubArray2;

            public Dictionary<string, int> StringDictionary;
            public Dictionary<int, string> IntDictionary;
        }
        /// <summary>
        /// 属性数据定义(引用类型外壳)
        /// </summary>
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal class PropertyData
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
            public float Float { get; set; }
            public double Double { get; set; }
            public decimal Decimal { get; set; }
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
            public float? FloatNull { get; set; }
            public double? DoubleNull { get; set; }
            public decimal? DecimalNull { get; set; }
            public Guid? GuidNull { get; set; }
            public char? CharNull { get; set; }
            public int[] Array { get; set; }
            public List<int> List { get; set; }
            public ByteEnum Enum { get; set; }
            public ByteFlagEnum FlagEnum { get; set; }
            public MemberClass Class { get; set; }
            public BoxStruct BoxStruct { get; set; }
            public BoxClass BoxClass { get; set; }
            public Dictionary<string, int> StringDictionary { get; set; }
            public Dictionary<int, string> IntDictionary { get; set; }
        }
        /// <summary>
        /// 字段数据定义(值类型外壳)
        /// </summary>
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal struct StructFieldData
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
            public int[] Array;
            public List<int> List;
            public ByteEnum Enum;
            public ByteFlagEnum FlagEnum;
            public MemberClass Class;
            public BoxStruct BoxStruct;
            public BoxClass BoxClass;
            public Dictionary<string, int> StringDictionary;
            public Dictionary<int, string> IntDictionary;
        }
        /// <summary>
        /// 属性数据定义(值类型外壳)
        /// </summary>
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        internal struct StructPropertyData
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
            public float Float { get; set; }
            public double Double { get; set; }
            public decimal Decimal { get; set; }
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
            public float? FloatNull { get; set; }
            public double? DoubleNull { get; set; }
            public decimal? DecimalNull { get; set; }
            public Guid? GuidNull { get; set; }
            public char? CharNull { get; set; }
            public int[] Array { get; set; }
            public List<int> List { get; set; }
            public ByteEnum Enum { get; set; }
            public ByteFlagEnum FlagEnum { get; set; }
            public MemberClass Class { get; set; }
            public BoxStruct BoxStruct { get; set; }
            public BoxClass BoxClass { get; set; }
            public Dictionary<string, int> StringDictionary { get; set; }
            public Dictionary<int, string> IntDictionary { get; set; }
        }
        /// <summary>
        /// 浮点数测试
        /// </summary>
        public class Float
        {
            public float FloatMin = float.MinValue;
            public float FloatMax = float.MaxValue;
            public float FloatNaN = float.NaN;
            public float FloatPositiveInfinity = float.PositiveInfinity;
            public float FloatNegativeInfinity = float.NegativeInfinity;

            public double DoubleMin = double.MinValue;
            public double DoubleMax = double.MaxValue;
            public double DoubleNaN = double.NaN;
            public double DoublePositiveInfinity = double.PositiveInfinity;
            public double DoubleNegativeInfinity = double.NegativeInfinity;

            public double Double0 = 1.7976931348623150E+308;
            public double Double1 = 1.7976931348623151E+308;
            public double Double2 = 1.7976931348623152E+308;
            public double Double3 = 1.7976931348623153E+308;
            public double Double4 = 1.7976931348623154E+308;
            public double Double5 = 1.7976931348623155E+308;
            public double Double6 = 1.7976931348623156E+308;
            public double Double7 = 1.7976931348623157E+308;

            public double DoubleNegative0 = -1.7976931348623150E+308;
            public double DoubleNegative1 = -1.7976931348623151E+308;
            public double DoubleNegative2 = -1.7976931348623152E+308;
            public double DoubleNegative3 = -1.7976931348623153E+308;
            public double DoubleNegative4 = -1.7976931348623154E+308;
            public double DoubleNegative5 = -1.7976931348623155E+308;
            public double DoubleNegative6 = -1.7976931348623156E+308;
            public double DoubleNegative7 = -1.7976931348623157E+308;
        }
    }
}
#pragma warning restore
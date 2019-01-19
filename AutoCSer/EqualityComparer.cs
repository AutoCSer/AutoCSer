using System;
using System.Collections.Generic;

namespace AutoCSer
{
    /// <summary>
    /// 默认等于比较
    /// </summary>
    public static class EqualityComparer
    {
        /// <summary>
        /// 
        /// </summary>
        private sealed class byteComparer : IEqualityComparer<byte>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(byte x, byte y) { return x == y; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(byte obj) { return obj; }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<byte> Byte;
        /// <summary>
        /// 
        /// </summary>
        private sealed class shortComparer : IEqualityComparer<short>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(short x, short y) { return x == y; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(short obj) { return obj; }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<short> Short;
        /// <summary>
        /// 
        /// </summary>
        private sealed class intComparer : IEqualityComparer<int>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(int x, int y) { return x == y; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(int obj) { return obj; }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<int> Int;
        /// <summary>
        /// 
        /// </summary>
        private sealed class ulongComparer : IEqualityComparer<ulong>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(ulong x, ulong y) { return x == y; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(ulong obj) { return (int)((uint)obj ^ (uint)(obj >> 32)); }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<ulong> ULong;
        /// <summary>
        /// 
        /// </summary>
        private sealed class charComparer : IEqualityComparer<char>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(char x, char y) { return x == y; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(char obj) { return obj; }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<char> Char;
        /// <summary>
        /// 
        /// </summary>
        private unsafe sealed class pointerComparer : IEqualityComparer<Pointer>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(Pointer x, Pointer y) { return x.Data == y.Data; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(Pointer obj) { return obj.GetHashCode(); }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<Pointer> Pointer;
        /// <summary>
        /// 
        /// </summary>
        private unsafe sealed class subStringComparer : IEqualityComparer<SubString>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(SubString x, SubString y) { return x.Equals(ref y); }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(SubString obj) { return obj.GetHashCode(); }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<SubString> SubString;
        /// <summary>
        /// 
        /// </summary>
        private unsafe sealed class hashBytesComparer : IEqualityComparer<HashBytes>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(HashBytes x, HashBytes y) { return x.Equals(ref y); }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(HashBytes obj) { return obj.GetHashCode(); }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<HashBytes> HashBytes;
        /// <summary>
        /// 
        /// </summary>
        private unsafe sealed class hashStringComparer : IEqualityComparer<HashString>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(HashString x, HashString y) { return x.Equals(ref y); }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(HashString obj) { return obj.GetHashCode(); }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<HashString> HashString;
        /// <summary>
        /// 
        /// </summary>
        private unsafe sealed class sessionIdComparer : IEqualityComparer<sessionId>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(sessionId x, sessionId y) { return x.Equals(ref y) == 0; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(sessionId obj) { return obj.GetHashCode(); }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        public static readonly IEqualityComparer<sessionId> SessionId;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        [Foundation.Preserve(AllMembers = true)]
        private sealed class equatable<valueType> : IEqualityComparer<valueType> where valueType : IEquatable<valueType>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(valueType x, valueType y)
            {
                return x == null ? y == null : x.Equals(y);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(valueType obj) { return obj != null ? obj.GetHashCode() : 0; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        [AutoCSer.IOS.Preserve(AllMembers = true)]
        private sealed class comparable<valueType> : IEqualityComparer<valueType> where valueType : IComparable<valueType>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(valueType x, valueType y)
            {
                return x == null ? y == null : x.CompareTo(y) == 0;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(valueType obj) { return obj != null ? obj.GetHashCode() : 0; }
        }
        /// <summary>
        /// 默认等于比较
        /// </summary>
        /// <typeparam name="valueType">比较值类型</typeparam>
        public static class comparer<valueType>
        {
            /// <summary>
            /// 默认等于比较
            /// </summary>
            public static readonly IEqualityComparer<valueType> Default;
            /// <summary>
            /// 未知类型等于比较
            /// </summary>
            private sealed class unknownComparer : IEqualityComparer<valueType>
            {
                /// <summary>
                /// 
                /// </summary>
                /// <param name="x"></param>
                /// <param name="y"></param>
                /// <returns></returns>
                public bool Equals(valueType x, valueType y)
                {
                    return x == null ? y == null : x.Equals(y);
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public int GetHashCode(valueType obj) { return obj != null ? obj.GetHashCode() : 0; }
            }
            /// <summary>
            /// 未知类型等于比较
            /// </summary>
            private sealed class unknownNotNullComparer : IEqualityComparer<valueType>
            {
                /// <summary>
                /// 
                /// </summary>
                /// <param name="x"></param>
                /// <param name="y"></param>
                /// <returns></returns>
                public bool Equals(valueType x, valueType y) { return x.Equals(y); }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public int GetHashCode(valueType obj) { return obj.GetHashCode(); }
            }
            static comparer()
            {
                Type type = typeof(valueType);
                object comparer;
                if (!comparers.TryGetValue(type, out comparer))
                {
                    if (typeof(IEquatable<valueType>).IsAssignableFrom(type))
                    {
                        Default = (IEqualityComparer<valueType>)Activator.CreateInstance(typeof(equatable<>).MakeGenericType(type));
                    }
                    else if (typeof(IComparable<valueType>).IsAssignableFrom(type))
                    {
                        Default = (IEqualityComparer<valueType>)Activator.CreateInstance(typeof(comparable<>).MakeGenericType(type));
                    }
                    else Default = type.isStruct() ? (IEqualityComparer<valueType>)new unknownNotNullComparer() : new unknownComparer();
                }
                else Default = (IEqualityComparer<valueType>)comparer;
            }
        }
        /// <summary>
        /// 等于比较集合
        /// </summary>
        private static readonly Dictionary<Type, object> comparers;
        static EqualityComparer()
        {
            comparers = DictionaryCreator.CreateOnly<Type, object>();
            comparers.Add(typeof(byte), Byte = new byteComparer());
            comparers.Add(typeof(short), Short = new shortComparer());
            comparers.Add(typeof(int), Int = new intComparer());
            comparers.Add(typeof(char), Char = new charComparer());
            comparers.Add(typeof(ulong), ULong = new ulongComparer());
            comparers.Add(typeof(Pointer), Pointer = new pointerComparer());
            comparers.Add(typeof(SubString), SubString = new subStringComparer());
            comparers.Add(typeof(HashBytes), HashBytes = new hashBytesComparer());
            comparers.Add(typeof(HashString), HashString = new hashStringComparer());
            comparers.Add(typeof(sessionId), SessionId = new sessionIdComparer());
        }
    }
}
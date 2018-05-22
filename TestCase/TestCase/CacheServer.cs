using System;
using System.Threading;
using AutoCSer.CacheServer;
using AutoCSer.CacheServer.DataStructure;
using AutoCSer.CacheServer.DataStructure.Value;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// 缓存测试
    /// </summary>
    internal sealed class CacheServer
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

        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCaseServer()
        {
            using (MasterServer.TcpInternalServer cacheMasterServer = new MasterServer.TcpInternalServer())
            using (SlaveServer.TcpInternalServer cacheSlaveServer = new SlaveServer.TcpInternalServer())
            {
                if (cacheMasterServer.IsListen && cacheSlaveServer.IsListen)
                {
                    return TestCase();
                }
            }
            return false;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            using (MasterClient masterClient = new MasterClient())
            using (SlaveClient slaveClient = new SlaveClient())
            {
                #region 获取环境参数
                HashSet<int> hashSet = masterClient.GetOrCreateDataStructure<HashSet<int>>("hashSet").Value;
                if (hashSet == null)
                {
                    return false;
                }
                int tryCount = 100;
                HashSet<int> slaveHashSet = null;
                do
                {
                    slaveHashSet = slaveClient.GetOrCreateDataStructure<HashSet<int>>("hashSet").Value;
                    if (slaveHashSet != null) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ReturnValue<bool> isHashSet = hashSet.Contains(1);
                if (isHashSet.Type != ReturnType.Success)
                {
                    return false;
                }
                if (isHashSet.Value)
                {
                    if (!slaveHashSet.Contains(1).Value)
                    {
                        return false;
                    }
                }
                else
                {
                    isHashSet = slaveHashSet.Contains(1);
                    if (isHashSet.Type != ReturnType.Success || isHashSet.Value)
                    {
                        return false;
                    }
                }
                #endregion

                if (!valueDictionary(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!valueFragmentDictionary(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!valueSearchTreeDictionary(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!valueArray(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!valueFragmentArray(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!fragmentHashSet(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!link(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!array(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!fragmentArray(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!dictionary(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!fragmentDictionary(masterClient, slaveClient, !isHashSet.Value)) return false;
                if (!searchTreeDictionary(masterClient, slaveClient, !isHashSet.Value)) return false;

                #region 修改环境参数
                if (isHashSet.Value)
                {
                    isHashSet = hashSet.Remove(1);
                    if (!isHashSet.Value)
                    {
                        return false;
                    }
                    isHashSet = hashSet.Contains(1);
                    if (isHashSet.Type != ReturnType.Success || isHashSet.Value)
                    {
                        return false;
                    }

                    tryCount = 10;
                    do
                    {
                        isHashSet = slaveHashSet.Contains(1);
                        if (isHashSet.Type == ReturnType.Success && !isHashSet.Value) break;
                        if (--tryCount == 0)
                        {
                            return false;
                        }
                        Thread.Sleep(1);
                    }
                    while (true);
                }
                else
                {
                    isHashSet = hashSet.Add(1);
                    if (!isHashSet.Value)
                    {
                        return false;
                    }
                    isHashSet = hashSet.Contains(1);
                    if (!isHashSet.Value)
                    {
                        return false;
                    }

                    tryCount = 10;
                    do
                    {
                        isHashSet = slaveHashSet.Contains(1);
                        if (isHashSet.Value) break;
                        if (--tryCount == 0)
                        {
                            return false;
                        }
                        Thread.Sleep(1);
                    }
                    while (true);
                }
                #endregion
                masterClient.WriteFile();
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool valueDictionary(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "valueDictionary";
            ValueDictionary<string, Binary<Data>> data = masterClient.GetOrCreateDataStructure<ValueDictionary<string, Binary<Data>>>(name).Value;
            if (data == null)
            {
                return false;
            }
            int tryCount = 10;
            ValueDictionary<string, Binary<Data>> slaveData;
            do
            {
                slaveData = slaveClient.GetOrCreateDataStructure<ValueDictionary<string, Binary<Data>>>(name).Value;
                if (slaveData != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            string key = "data";
            if (isStart)
            {
                ReturnValue<bool> isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                isData = slaveData.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }

                Data filedData = AutoCSer.RandomObject.Creator<Data>.Create(Json.RandomConfig);
                isData = data.Set(key, filedData);
                if (!isData.Value)
                {
                    return false;
                }
                ReturnValue<Data> getData = data.Get(key).Get();
                if (getData.Type != AutoCSer.CacheServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getData.Value))
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<Data> setData = data.Get(key).Get();
                if (setData.Type != AutoCSer.CacheServer.ReturnType.Success || setData.Value == null)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<Data> getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(setData.Value, getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ReturnValue<bool> isData = data.Remove(key);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveData.ContainsKey(key);
                    if (isData.Type == AutoCSer.CacheServer.ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            ReturnValue<int> count = data.Count;
            if (count.Type != ReturnType.Success || count.Value != (isStart ? 1: 0))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool valueFragmentDictionary(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "valueFragmentDictionary";
            ValueDictionary<decimal, Json<Data>> data = masterClient.GetOrCreateDataStructure<ValueDictionary<decimal, Json<Data>>>(name).Value;
            if (data == null)
            {
                return false;
            }
            int tryCount = 10;
            ValueDictionary<decimal, Json<Data>> slaveData;
            do
            {
                slaveData = slaveClient.GetOrCreateDataStructure<ValueDictionary<decimal, Json<Data>>>(name).Value;
                if (slaveData != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            decimal key = 3;
            if (isStart)
            {
                ReturnValue<bool> isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                isData = slaveData.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }

                Data filedData = AutoCSer.RandomObject.Creator<Data>.Create(Json.RandomConfig);
                isData = data.Set(key, filedData);
                if (!isData.Value)
                {
                    return false;
                }
                ReturnValue<Data> getData = data.Get(key).Get();
                if (getData.Type != AutoCSer.CacheServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getData.Value))
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<Data> setData = data.Get(key).Get();
                if (setData.Type != AutoCSer.CacheServer.ReturnType.Success || setData.Value == null)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<Data> getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(setData.Value, getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ReturnValue<bool> isData = data.Remove(key);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveData.ContainsKey(key);
                    if (isData.Type == AutoCSer.CacheServer.ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            ReturnValue<int> count = data.Count;
            if (count.Type != ReturnType.Success || count.Value != (isStart ? 1 : 0))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool valueSearchTreeDictionary(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "valueSearchTreeDictionary";
            ValueSearchTreeDictionary<DateTime, Value<byte[]>> data = masterClient.GetOrCreateDataStructure<ValueSearchTreeDictionary<DateTime, Value<byte[]>>>(name).Value;
            if (data == null)
            {
                return false;
            }
            int tryCount = 10;
            ValueSearchTreeDictionary<DateTime, Value<byte[]>> slaveData;
            do
            {
                slaveData = slaveClient.GetOrCreateDataStructure<ValueSearchTreeDictionary<DateTime, Value<byte[]>>>(name).Value;
                if (slaveData != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            DateTime key = new DateTime(1981, 12, 6);
            byte[] value = new byte[] { 5, 4, 3, 2, 1 };
            if (isStart)
            {
                ReturnValue<bool> isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                isData = slaveData.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }

                isData = data.Set(key, value);
                if (!isData.Value)
                {
                    return false;
                }
                ReturnValue<byte[]> getData = data.Get(key).Get();
                if (getData.Type != AutoCSer.CacheServer.ReturnType.Success || !value.equal(getData.Value))
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && value.equal(getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<byte[]> setData = data.Get(key).Get();
                if (setData.Type != AutoCSer.CacheServer.ReturnType.Success || !value.equal(setData.Value))
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<byte[]> getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && value.equal(getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ReturnValue<bool> isData = data.Remove(key);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveData.ContainsKey(key);
                    if (isData.Type == AutoCSer.CacheServer.ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            ReturnValue<int> count = data.Count;
            if (count.Type != ReturnType.Success || count.Value != (isStart ? 1 : 0))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool valueArray(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "valueArray";
            ValueArray<Value<long>> data = masterClient.GetOrCreateDataStructure<ValueArray<Value<long>>>(name).Value;
            if (data == null)
            {
                return false;
            }
            int tryCount = 10;
            ValueArray<Value<long>> slaveData;
            do
            {
                slaveData = slaveClient.GetOrCreateDataStructure<ValueArray<Value<long>>>(name).Value;
                if (slaveData != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            int index = 5;
            long value = long.MaxValue - index;
            if (isStart)
            {
                ReturnValue<long> getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }

                ReturnValue<bool> isData = data.Set(index, value);
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<long> getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ReturnValue<bool> isData = data.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if ((getData.Type == AutoCSer.CacheServer.ReturnType.Success || getData.Type == ReturnType.ArrayIndexOutOfRange) && getData.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            ReturnValue<int> count = data.Count;
            if (count.Type != ReturnType.Success || count.Value != (isStart ? index + 1 : 0))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool valueFragmentArray(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "valueFragmentArray";
            ValueFragmentArray<Value<uint>> data = masterClient.GetOrCreateDataStructure<ValueFragmentArray<Value<uint>>>(name).Value;
            if (data == null)
            {
                return false;
            }
            int tryCount = 10;
            ValueFragmentArray<Value<uint>> slaveData;
            do
            {
                slaveData = slaveClient.GetOrCreateDataStructure<ValueFragmentArray<Value<uint>>>(name).Value;
                if (slaveData != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            int index = 5;
            uint value = uint.MaxValue - (uint)index;
            if (isStart)
            {
                ReturnValue<uint> getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }

                ReturnValue<bool> isData = data.Set(index, value);
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<uint> getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ReturnValue<bool> isData = data.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if ((getData.Type == AutoCSer.CacheServer.ReturnType.Success || getData.Type == ReturnType.ArrayIndexOutOfRange) && getData.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            ReturnValue<int> count = data.Count;
            if (count.Type != ReturnType.Success || count.Value != (isStart ? index + 1 : 0))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool fragmentHashSet(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "fragmentHashSet";
            FragmentHashSet<string> data = masterClient.GetOrCreateDataStructure<FragmentHashSet<string>>(name).Value;
            if (data == null)
            {
                return false;
            }
            int tryCount = 10;
            FragmentHashSet<string> slaveData;
            do
            {
                slaveData = slaveClient.GetOrCreateDataStructure<FragmentHashSet<string>>(name).Value;
                if (slaveData != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            string key = "key";
            if (isStart)
            {
                ReturnValue<bool> isData = data.Contains(key);
                if (isData.Type != ReturnType.Success || isData.Value)
                {
                    return false;
                }
                isData = slaveData.Contains(key);
                if (isData.Type != ReturnType.Success || isData.Value)
                {
                    return false;
                }

                isData = data.Add(key);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.Contains(key);
                if (!isData.Value)
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    isData = slaveData.Contains(key);
                    if (isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<bool> isData = data.Contains(key);
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveData.Contains(key);
                    if (isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = data.Remove(key);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.Contains(key);
                if (isData.Type != ReturnType.Success || isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveData.Contains(key);
                    if (isData.Type == AutoCSer.CacheServer.ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            ReturnValue<int> count = data.Count;
            if (count.Type != ReturnType.Success || count.Value != (isStart ? 1 : 0))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool link(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "link";
            Link<Value<byte>> data = masterClient.GetOrCreateDataStructure<Link<Value<byte>>>(name).Value;
            if (data == null)
            {
                return false;
            }
            int tryCount = 10;
            Link<Value<byte>> slaveData;
            do
            {
                slaveData = slaveClient.GetOrCreateDataStructure<Link<Value<byte>>>(name).Value;
                if (slaveData != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            ReturnValue<int> count = data.Count;
            if (count.Type != ReturnType.Success || count.Value != (isStart ? 0 : 6))
            {
                return false;
            }
            if (isStart)
            {
                ReturnValue<bool> isData = data.Append(3);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.Append(5);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.InsertBefore(0, 1);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.InsertBefore(1, 2);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.InsertAfter(-2, 4);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.InsertAfter(-1, 6);
                if (!isData.Value)
                {
                    return false;
                }

                ReturnValue<byte> value;
                for (int index = 0; index != 6; ++index)
                {
                    value = data.Get(index).Get();
                    if (value.Value != index + 1)
                    {
                        return false;
                    }
                }

                tryCount = 10;
                do
                {
                    value = slaveData.Get(-1).Get();
                    if (value.Value == 6) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
                for (int index = -2; index >= -6; --index)
                {
                    value = slaveData.Get(index).Get();
                    if (value.Value != index + 7)
                    {
                        return false;
                    }
                }
            }
            else
            {
                ReturnValue<byte> value;
                for (int index = 0; index != 6; ++index)
                {
                    value = data.Get(index).Get();
                    if (value.Value != index + 1)
                    {
                        return false;
                    }
                }

                tryCount = 10;
                do
                {
                    value = slaveData.Get(-1).Get();
                    if (value.Value == 6) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
                for (int index = -2; index >= -6; --index)
                {
                    value = slaveData.Get(index).Get();
                    if (value.Value != index + 7)
                    {
                        return false;
                    }
                }

                value = data.Dequeue().Get();
                if (value.Value != 1)
                {
                    return false;
                }
                value = data.StackPop().Get();
                if (value.Value != 6)
                {
                    return false;
                }
                value = data.GetRemove(1).Get();
                if (value.Value != 3)
                {
                    return false;
                }
                ReturnValue<bool> isData = data.Remove(-2);
                if (!isData.Value)
                {
                    return false;
                }

                count = data.Count;
                if (count.Type != ReturnType.Success || count.Value != 2)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    count = slaveData.Count;
                    if (count.Value == 2) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                value = data.Get(0).Get();
                if (value.Value != 2)
                {
                    return false;
                }
                value = data.Get(1).Get();
                if (value.Value != 5)
                {
                    return false;
                }
                value = slaveData.Get(-2).Get();
                if (value.Value != 2)
                {
                    return false;
                }
                value = slaveData.Get(-1).Get();
                if (value.Value != 5)
                {
                    return false;
                }

                isData = data.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    count = slaveData.Count;
                    if (count.Type == ReturnType.Success && count.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            count = data.Count;
            if (count.Type != ReturnType.Success || count.Value != (isStart ? 6 : 0))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool array(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "array";
            Array<ValueDictionary<string, Binary<Data>>> array = masterClient.GetOrCreateDataStructure<Array<ValueDictionary<string, Binary<Data>>>>(name).Value;
            if (array == null)
            {
                return false;
            }
            int tryCount = 10;
            Array<ValueDictionary<string, Binary<Data>>> slaveArray;
            do
            {
                slaveArray = slaveClient.GetOrCreateDataStructure<Array<ValueDictionary<string, Binary<Data>>>>(name).Value;
                if (slaveArray != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            int index = 5;
            string key = "data";
            if (isStart)
            {
                ReturnValue<bool> isData = array.IsNode(index);
                if (isData.Type != ReturnType.Success || isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveArray.IsNode(index);
                    if (isData.Type == ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueDictionary<string, Binary<Data>> data = array.GetOrCreate(index).Value;
                if (data == null)
                {
                    return false;
                }
                ValueDictionary<string, Binary<Data>> slaveData = slaveArray[index];

                isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                isData = slaveData.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }

                Data filedData = AutoCSer.RandomObject.Creator<Data>.Create(Json.RandomConfig);
                isData = data.Set(key, filedData);
                if (!isData.Value)
                {
                    return false;
                }
                ReturnValue<Data> getData = data.Get(key).Get();
                if (getData.Type != AutoCSer.CacheServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getData.Value))
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<bool> isData = array.IsNode(index);
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveArray.IsNode(index);
                    if (isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueDictionary<string, Binary<Data>> data = array[index];
                ValueDictionary<string, Binary<Data>> slaveData = slaveArray[index];

                ReturnValue<Data> setData = data.Get(key).Get();
                if (setData.Type != AutoCSer.CacheServer.ReturnType.Success || setData.Value == null)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<Data> getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(setData.Value, getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = data.Remove(key);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveData.ContainsKey(key);
                    if (isData.Type == AutoCSer.CacheServer.ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = array.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<int> count = slaveArray.Count;
                    if (count.Type == AutoCSer.CacheServer.ReturnType.Success && count.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool fragmentArray(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "fragmentArray";
            FragmentArray<ValueDictionary<string, Binary<Data>>> array = masterClient.GetOrCreateDataStructure<FragmentArray<ValueDictionary<string, Binary<Data>>>>(name).Value;
            if (array == null)
            {
                return false;
            }
            int tryCount = 10;
            FragmentArray<ValueDictionary<string, Binary<Data>>> slaveArray;
            do
            {
                slaveArray = slaveClient.GetOrCreateDataStructure<FragmentArray<ValueDictionary<string, Binary<Data>>>>(name).Value;
                if (slaveArray != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            int index = 5;
            string key = "data";
            if (isStart)
            {
                ReturnValue<bool> isData = array.IsNode(index);
                if (isData.Type != ReturnType.Success || isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveArray.IsNode(index);
                    if (isData.Type == ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueDictionary<string, Binary<Data>> data = array.GetOrCreate(index).Value;
                if (data == null)
                {
                    return false;
                }
                ValueDictionary<string, Binary<Data>> slaveData = slaveArray[index];

                isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                isData = slaveData.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }

                Data filedData = AutoCSer.RandomObject.Creator<Data>.Create(Json.RandomConfig);
                isData = data.Set(key, filedData);
                if (!isData.Value)
                {
                    return false;
                }
                ReturnValue<Data> getData = data.Get(key).Get();
                if (getData.Type != AutoCSer.CacheServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getData.Value))
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<bool> isData = array.IsNode(index);
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveArray.IsNode(index);
                    if (isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueDictionary<string, Binary<Data>> data = array[index], slaveData = slaveArray[index];
                ReturnValue<Data> setData = data.Get(key).Get();
                if (setData.Type != AutoCSer.CacheServer.ReturnType.Success || setData.Value == null)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<Data> getData = slaveData.Get(key).Get();
                    if (getData.Type == AutoCSer.CacheServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(setData.Value, getData.Value)) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = data.Remove(key);
                if (!isData.Value)
                {
                    return false;
                }
                isData = data.ContainsKey(key);
                if (isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveData.ContainsKey(key);
                    if (isData.Type == AutoCSer.CacheServer.ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = array.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<int> count = slaveArray.Count;
                    if (count.Type == AutoCSer.CacheServer.ReturnType.Success && count.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool dictionary(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "dictionary";
            Dictionary<int, ValueArray<Value<long>>> dictionary = masterClient.GetOrCreateDataStructure<Dictionary<int, ValueArray<Value<long>>>>(name).Value;
            if (dictionary == null)
            {
                return false;
            }
            int tryCount = 10;
            Dictionary<int, ValueArray<Value<long>>> slaveDictionary;
            do
            {
                slaveDictionary = slaveClient.GetOrCreateDataStructure<Dictionary<int, ValueArray<Value<long>>>>(name).Value;
                if (slaveDictionary != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            int index = 5, key = 10;
            long value = long.MaxValue - index;
            if (isStart)
            {
                ReturnValue<bool> isData = dictionary.ContainsKey(key);
                if (isData.Type != ReturnType.Success || isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveDictionary.ContainsKey(key);
                    if (isData.Type == ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueArray<Value<long>> data = dictionary.GetOrCreate(key).Value;
                if (data == null)
                {
                    return false;
                }
                ValueArray<Value<long>> slaveData = slaveDictionary[key];

                ReturnValue<long> getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }

                isData = data.Set(index, value);
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<bool> isData = dictionary.ContainsKey(key);
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveDictionary.ContainsKey(key);
                    if (isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueArray<Value<long>> data = dictionary[key], slaveData = slaveDictionary[key];

                ReturnValue<long> getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = data.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if ((getData.Type == AutoCSer.CacheServer.ReturnType.Success || getData.Type == ReturnType.ArrayIndexOutOfRange) && getData.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = dictionary.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<int> count = slaveDictionary.Count;
                    if (count.Type == AutoCSer.CacheServer.ReturnType.Success && count.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool fragmentDictionary(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "fragmentDictionary";
            FragmentDictionary<int, ValueArray<Value<long>>> dictionary = masterClient.GetOrCreateDataStructure<FragmentDictionary<int, ValueArray<Value<long>>>>(name).Value;
            if (dictionary == null)
            {
                return false;
            }
            int tryCount = 10;
            FragmentDictionary<int, ValueArray<Value<long>>> slaveDictionary;
            do
            {
                slaveDictionary = slaveClient.GetOrCreateDataStructure<FragmentDictionary<int, ValueArray<Value<long>>>>(name).Value;
                if (slaveDictionary != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            int index = 5, key = 10;
            long value = long.MaxValue - index;
            if (isStart)
            {
                ReturnValue<bool> isData = dictionary.ContainsKey(key);
                if (isData.Type != ReturnType.Success || isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveDictionary.ContainsKey(key);
                    if (isData.Type == ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueArray<Value<long>> data = dictionary.GetOrCreate(key).Value;
                if (data == null)
                {
                    return false;
                }
                ValueArray<Value<long>> slaveData = slaveDictionary[key];

                ReturnValue<long> getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }

                isData = data.Set(index, value);
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<bool> isData = dictionary.ContainsKey(key);
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveDictionary.ContainsKey(key);
                    if (isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueArray<Value<long>> data = dictionary[key], slaveData = slaveDictionary[key];

                ReturnValue<long> getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = data.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if ((getData.Type == AutoCSer.CacheServer.ReturnType.Success || getData.Type == ReturnType.ArrayIndexOutOfRange) && getData.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = dictionary.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<int> count = slaveDictionary.Count;
                    if (count.Type == AutoCSer.CacheServer.ReturnType.Success && count.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            return true;
        }
        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static bool searchTreeDictionary(MasterClient masterClient, SlaveClient slaveClient, bool isStart)
        {
            string name = "searchTreeDictionary";
            SearchTreeDictionary<int, ValueArray<Value<long>>> dictionary = masterClient.GetOrCreateDataStructure<SearchTreeDictionary<int, ValueArray<Value<long>>>>(name).Value;
            if (dictionary == null)
            {
                return false;
            }
            int tryCount = 10;
            SearchTreeDictionary<int, ValueArray<Value<long>>> slaveDictionary;
            do
            {
                slaveDictionary = slaveClient.GetOrCreateDataStructure<SearchTreeDictionary<int, ValueArray<Value<long>>>>(name).Value;
                if (slaveDictionary != null) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            int index = 5, key = 10;
            long value = long.MaxValue - index;
            if (isStart)
            {
                ReturnValue<bool> isData = dictionary.ContainsKey(key);
                if (isData.Type != ReturnType.Success || isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveDictionary.ContainsKey(key);
                    if (isData.Type == ReturnType.Success && !isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueArray<Value<long>> data = dictionary.GetOrCreate(key).Value;
                if (data == null)
                {
                    return false;
                }
                ValueArray<Value<long>> slaveData = slaveDictionary[key];

                ReturnValue<long> getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }

                isData = data.Set(index, value);
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }

                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            else
            {
                ReturnValue<bool> isData = dictionary.ContainsKey(key);
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    isData = slaveDictionary.ContainsKey(key);
                    if (isData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                ValueArray<Value<long>> data = dictionary[key], slaveData = slaveDictionary[key];

                ReturnValue<long> getData = data.Get(index).Get();
                if (value != getData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if (value == getData.Value) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = data.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                getData = data.Get(index).Get();
                if ((getData.Type != ReturnType.Success && getData.Type != ReturnType.ArrayIndexOutOfRange) || getData.Value != 0)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    getData = slaveData.Get(index).Get();
                    if ((getData.Type == AutoCSer.CacheServer.ReturnType.Success || getData.Type == ReturnType.ArrayIndexOutOfRange) && getData.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);

                isData = dictionary.Clear();
                if (!isData.Value)
                {
                    return false;
                }
                tryCount = 10;
                do
                {
                    ReturnValue<int> count = slaveDictionary.Count;
                    if (count.Type == AutoCSer.CacheServer.ReturnType.Success && count.Value == 0) break;
                    if (--tryCount == 0)
                    {
                        return false;
                    }
                    Thread.Sleep(1);
                }
                while (true);
            }
            return true;
        }
    }
}

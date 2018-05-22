using System;
using System.Threading;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// K-V 缓存测试
    /// </summary>
    internal sealed class KeyValueStream
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
#if !NoAutoCSer
        /// <summary>
        /// K-V 缓存测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCaseServer()
        {
            using (AutoCSer.RemoteDictionaryStreamServer.MasterServer.TcpInternalServer keyValueStreamMasterServer = new RemoteDictionaryStreamServer.MasterServer.TcpInternalServer())
            using (AutoCSer.RemoteDictionaryStreamServer.SlaveServer.TcpInternalServer keyValueStreamSlaveServer = new RemoteDictionaryStreamServer.SlaveServer.TcpInternalServer())
            {
                if (keyValueStreamMasterServer.IsListen && keyValueStreamSlaveServer.IsListen)
                {
                    return TestCase();
                }
            }
            return false;
        }
        /// <summary>
        /// K-V 缓存测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            using (AutoCSer.RemoteDictionaryStreamServer.MasterClient masterClient = new AutoCSer.RemoteDictionaryStreamServer.MasterClient())
            using (AutoCSer.RemoteDictionaryStreamServer.SlaveClient slaveClient = new AutoCSer.RemoteDictionaryStreamServer.SlaveClient())
            {
                if (!test(masterClient, slaveClient)) return false;
                if (!dictionary(masterClient.Dictionary, slaveClient.Dictionary)) return false;
                if (!link(masterClient.Link, slaveClient.Link)) return false;
            }
            return true;
        }
        /// <summary>
        /// K-V 缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <returns></returns>
        private static bool test(AutoCSer.RemoteDictionaryStreamServer.MasterClient masterClient, AutoCSer.RemoteDictionaryStreamServer.SlaveClient slaveClient)
        {
            int tryCount;
            Data filedData = AutoCSer.RandomObject.Creator<Data>.Create(Json.RandomConfig);
            #region 二进制序列化
            masterClient.Set("filedData", filedData);
            AutoCSer.Net.TcpServer.ReturnValue<Data> getFieldData = masterClient.Get<Data>("filedData");
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }

            tryCount = 10;
            do
            {
                getFieldData = slaveClient.Get<Data>("filedData");
                if (getFieldData.Type == Net.TcpServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData)) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);
            #endregion

            #region JSON 序列化
            masterClient.SetJson("filedData", filedData);
            getFieldData = masterClient.Get<Data>("filedData");
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }

            tryCount = 10;
            do
            {
                getFieldData = slaveClient.Get<Data>("filedData");
                if (getFieldData.Type == Net.TcpServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData)) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);
            #endregion

            #region 字节数据
            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(filedData), rangeData = new SubArray<byte>(data, 1, data.Length - 2).GetArray();
            masterClient.Set("filedData", data);
            if (!masterClient.Contains("filedData"))
            {
                return false;
            }
            if (masterClient.GetSize("filedData") != data.Length)
            {
                return false;
            }
            byte[] getData = masterClient.Get("filedData");
            if (getData == null || !AutoCSer.Memory.EqualNotNull(data, getData))
            {
                return false;
            }
            getData = masterClient.GetRange("filedData", 1, -1);
            if (getData == null || !AutoCSer.Memory.EqualNotNull(rangeData, getData))
            {
                return false;
            }
            int index = AutoCSer.Random.Default.Next(data.Length << 3);
            AutoCSer.Net.TcpServer.ReturnValue<bool?> bit = masterClient.GetBit("filedData", index);
            if (bit.Type != Net.TcpServer.ReturnType.Success || bit.Value == null || (bool)bit.Value ^ ((data[index >> 3] & (1 << (index & 7))) != 0))
            {
                return false;
            }
            index = AutoCSer.Random.Default.Next(data.Length - (sizeof(ulong) - 1));
            AutoCSer.Net.TcpServer.ReturnValue<ulong?> int64 = masterClient.BitConverter("filedData", index);
            if (int64.Type != Net.TcpServer.ReturnType.Success || int64.Value == null || (ulong)int64.Value != BitConverter.ToUInt64(data, index))
            {
                return false;
            }

            tryCount = 10;
            do
            {
                getData = slaveClient.Get("filedData");
                if (getData != null && AutoCSer.Memory.EqualNotNull(data, getData)) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);
            getData = slaveClient.GetRange("filedData", 1, -1);
            if (getData == null || !AutoCSer.Memory.EqualNotNull(rangeData, getData))
            {
                return false;
            }
            index = AutoCSer.Random.Default.Next(data.Length << 3);
            bit = slaveClient.GetBit("filedData", index);
            if (bit.Type != Net.TcpServer.ReturnType.Success || bit.Value == null || (bool)bit.Value ^ ((data[index >> 3] & (1 << (index & 7))) != 0))
            {
                return false;
            }
            index = AutoCSer.Random.Default.Next(data.Length - (sizeof(ulong) - 1));
            int64 = slaveClient.BitConverter("filedData", index);
            if (int64.Type != Net.TcpServer.ReturnType.Success || int64.Value == null || (ulong)int64.Value != BitConverter.ToUInt64(data, index))
            {
                return false;
            }

            if (masterClient.Append("filedData", rangeData).Value != RemoteDictionaryStreamServer.ReturnType.Success)
            {
                return false;
            }
            if (masterClient.GetSize("filedData") != data.Length + rangeData.Length)
            {
                return false;
            }

            tryCount = 10;
            do
            {
                if (slaveClient.GetSize("filedData") == data.Length + rangeData.Length) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.RemoteDictionaryStreamServer.ReturnType>  returnType = masterClient.SetSize("filedData", 1);
            if (returnType.Type != Net.TcpServer.ReturnType.Success || returnType.Value != RemoteDictionaryStreamServer.ReturnType.Success)
            {
                return false;
            }
            if (masterClient.GetSize("filedData") != 1)
            {
                return false;
            }

            tryCount = 10;
            do
            {
                if (slaveClient.GetSize("filedData") == 1) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            if (masterClient.SetRange("filedData", 2, new byte[] { 1, 2 }).Value != RemoteDictionaryStreamServer.ReturnType.Success)
            {//x,0,1,2
                return false;
            }
            if (masterClient.GetSize("filedData") != 4)
            {
                Console.WriteLine(masterClient.GetSize("filedData").Value);
                return false;
            }
            AutoCSer.Net.TcpServer.ReturnValue<bool?> isSet = masterClient.SetBit("filedData", 5 * 8);
            if (isSet.Type != Net.TcpServer.ReturnType.Success || isSet.Value == null || (bool)isSet.Value)
            {//x,0,1,2,0,1
                return false;
            }
            if (masterClient.GetSize("filedData") != 6)
            {
                return false;
            }
            isSet = masterClient.ClearBit("filedData", 2 * 8);
            if (isSet.Type != Net.TcpServer.ReturnType.Success || isSet.Value == null || !(bool)isSet.Value)
            {//x,0,0,2,0,1
                return false;
            }
            isSet = masterClient.SetBitNot("filedData", 3 * 8);
            if (isSet.Type != Net.TcpServer.ReturnType.Success || isSet.Value == null || !(bool)isSet.Value)
            {//x,0,0,3,0,1
                return false;
            }
            int64 = masterClient.BitConverter("filedData", 0);
            if (int64.Type != Net.TcpServer.ReturnType.Success || int64.Value == null || (ulong)int64.Value != data[0] + 0x10003000000UL)
            {
                return false;
            }

            tryCount = 10;
            do
            {
                int64 = slaveClient.BitConverter("filedData", 0);
                if (int64.Type == Net.TcpServer.ReturnType.Success && int64.Value != null && (ulong)int64.Value == data[0] + 0x10003000000UL) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            int64 = masterClient.SetByte("filedData", 0, 0x0102030405060708UL);
            if (int64.Type != Net.TcpServer.ReturnType.Success || int64.Value == null || (ulong)int64.Value != 0x10003000000UL)
            {//0102030405060708
                return false;
            }
            AutoCSer.Net.TcpServer.ReturnValue<uint?> int32 = masterClient.SetByte("filedData", 2, 0x090A0B0CU);
            if (int32.Type != Net.TcpServer.ReturnType.Success || int32.Value == null || (uint)int32.Value != 0x03040506U)
            {//0102090A0B0C0708
                return false;
            }
            int64 = masterClient.Or("filedData", 0, 0x1020304050607080UL);
            if (int64.Type != Net.TcpServer.ReturnType.Success || int64.Value == null || (ulong)int64.Value != 0x1122394A5B6C7788UL)
            {//1122394A5B6C7788
                return false;
            }
            int32 = masterClient.Xor("filedData", 2, 0x090A0B0CU);
            if (int32.Type != Net.TcpServer.ReturnType.Success || int32.Value == null || (uint)int32.Value != 0x30405060U)
            {//1122304050607788
                return false;
            }
            int64 = masterClient.And("filedData", 0, 0xF0F0F0F0F0F0F0F0UL);
            if (int64.Type != Net.TcpServer.ReturnType.Success || int64.Value == null || (ulong)int64.Value != 0x1020304050607080UL)
            {//1020304050607080
                return false;
            }
            int64 = masterClient.Increment("filedData", 0);
            if (int64.Type != Net.TcpServer.ReturnType.Success || int64.Value == null || (ulong)int64.Value != 0x1020304050607081UL)
            {//1020304050607081
                return false;
            }
            int32 = masterClient.Increment("filedData", 2, 0x03040506U);
            if (int32.Type != Net.TcpServer.ReturnType.Success || int32.Value == null || (uint)int32.Value != 0x33445566U)
            {//1020334455667081
                return false;
            }
            int64 = masterClient.Decrement("filedData", 0);
            if (int64.Type != Net.TcpServer.ReturnType.Success || int64.Value == null || (ulong)int64.Value != 0x1020334455667080UL)
            {//1020334455667080
                return false;
            }
            int32 = masterClient.Decrement("filedData", 2, 0x03040506U);
            if (int32.Type != Net.TcpServer.ReturnType.Success || int32.Value == null || (uint)int32.Value != 0x030405060U)
            {//1020304050607080
                return false;
            }

            tryCount = 10;
            do
            {
                int64 = slaveClient.BitConverter("filedData", 0);
                if (int64.Type == Net.TcpServer.ReturnType.Success && int64.Value != null && (ulong)int64.Value == 0x1020304050607080UL) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            returnType = masterClient.Remove("filedData");
            if (returnType.Type != Net.TcpServer.ReturnType.Success || returnType.Value != RemoteDictionaryStreamServer.ReturnType.Success)
            {
                return false;
            }
            if (masterClient.Contains("filedData"))
            {
                return false;
            }

            tryCount = 10;
            do
            {
                AutoCSer.Net.TcpServer.ReturnValue<bool> boolValue = slaveClient.Contains("filedData");
                if (boolValue.Type == Net.TcpServer.ReturnType.Success && !boolValue.Value) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);
            #endregion
            return true;
        }
        /// <summary>
        /// 字典缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <returns></returns>
        private static bool dictionary(AutoCSer.RemoteDictionaryStreamServer.Dictionary.MasterClient masterClient, AutoCSer.RemoteDictionaryStreamServer.Dictionary.SlaveClient slaveClient)
        {
            int tryCount;
            Data filedData = AutoCSer.RandomObject.Creator<Data>.Create(Json.RandomConfig);
            #region 二进制序列化
            masterClient.Set("hash", "filedData", filedData);
            AutoCSer.Net.TcpServer.ReturnValue<Data> getFieldData = masterClient.Get<Data>("hash", "filedData");
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }

            tryCount = 10;
            do
            {
                getFieldData = slaveClient.Get<Data>("hash", "filedData");
                if (getFieldData.Type == Net.TcpServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData)) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);
            #endregion

            #region JSON 序列化
            masterClient.SetJson("hash", "filedData", filedData);
            getFieldData = masterClient.Get<Data>("hash", "filedData");
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }

            tryCount = 10;
            do
            {
                getFieldData = slaveClient.Get<Data>("hash", "filedData");
                if (getFieldData.Type == Net.TcpServer.ReturnType.Success && AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData)) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);
            #endregion

            #region 字节数据
            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(filedData), rangeData = new SubArray<byte>(data, 1, data.Length - 2).GetArray();
            masterClient.Set("hash", "filedData", data);
            if (!masterClient.Contains("hash", "filedData"))
            {
                return false;
            }
            if (masterClient.GetSize("hash", "filedData") != data.Length)
            {
                return false;
            }
            byte[] getData = masterClient.Get("hash", "filedData");
            if (getData == null || !AutoCSer.Memory.EqualNotNull(data, getData))
            {
                return false;
            }

            tryCount = 10;
            do
            {
                getData = slaveClient.Get("hash", "filedData");
                if (getData != null && AutoCSer.Memory.EqualNotNull(data, getData)) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            AutoCSer.Net.TcpServer.ReturnValue<RemoteDictionaryStreamServer.ReturnType> isSet = masterClient.Remove("hash", "filedData");
            if (isSet.Type != Net.TcpServer.ReturnType.Success || isSet.Value != RemoteDictionaryStreamServer.ReturnType.Success)
            {
                return false;
            }
            if (masterClient.Contains("hash", "filedData"))
            {
                return false;
            }

            tryCount = 10;
            do
            {
                AutoCSer.Net.TcpServer.ReturnValue<bool> boolValue = slaveClient.Contains("hash", "filedData");
                if (boolValue.Type == Net.TcpServer.ReturnType.Success && !boolValue.Value) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);
            #endregion
            return true;
        }
        /// <summary>
        /// 链表缓存测试
        /// </summary>
        /// <param name="masterClient"></param>
        /// <param name="slaveClient"></param>
        /// <returns></returns>
        private static bool link(AutoCSer.RemoteDictionaryStreamServer.Link.MasterClient masterClient, AutoCSer.RemoteDictionaryStreamServer.Link.SlaveClient slaveClient)
        {
            int tryCount;
            Data filedData = AutoCSer.RandomObject.Creator<Data>.Create(Json.RandomConfig);

            if (masterClient.Contains("link").Value) masterClient.Remove("link");
            masterClient.Push("link", BitConverter.GetBytes(0x12345678));
            masterClient.Push("link", filedData);
            masterClient.PushJson("link", filedData);
            masterClient.PushHeadJson("link", filedData);
            masterClient.PushHead("link", filedData);

            if (masterClient.GetSize("link").Value != 5)
            {
                return false;
            }
            tryCount = 10;
            do
            {
                if (slaveClient.GetSize("link").Value == 5) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);
            AutoCSer.Net.TcpServer.ReturnValue<Data> getFieldData = slaveClient.Get<Data>("link", 0);
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }
            getFieldData = slaveClient.Get<Data>("link", 1);
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }

            getFieldData = masterClient.Pop<Data>("link");
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }
            getFieldData = masterClient.PopHead<Data>("link");
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }
            AutoCSer.Net.TcpServer.ReturnValue<RemoteDictionaryStreamServer.ReturnType> isRemove = masterClient.Remove("link", 1);
            if (isRemove.Type != Net.TcpServer.ReturnType.Success || isRemove.Value != RemoteDictionaryStreamServer.ReturnType.Success)
            {
                return false;
            }

            if (masterClient.GetSize("link").Value != 2)
            {
                return false;
            }
            tryCount = 10;
            do
            {
                if (slaveClient.GetSize("link").Value == 2) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            getFieldData = masterClient.Pop<Data>("link");
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }
            getFieldData = masterClient.PopHead<Data>("link");
            if (getFieldData.Type != Net.TcpServer.ReturnType.Success || !AutoCSer.FieldEquals.Comparor<Data>.Equals(filedData, getFieldData))
            {
                return false;
            }

            isRemove = masterClient.Remove("link");
            if (isRemove.Type != Net.TcpServer.ReturnType.Success || isRemove.Value != RemoteDictionaryStreamServer.ReturnType.Success)
            {
                return false;
            }
            AutoCSer.Net.TcpServer.ReturnValue<bool> isContains = masterClient.Contains("link");
            if (isContains.Type != Net.TcpServer.ReturnType.Success || isContains.Value)
            {
                return false;
            }
            tryCount = 10;
            do
            {
                isContains = slaveClient.Contains("link");
                if (isContains.Type == Net.TcpServer.ReturnType.Success && !isContains.Value) break;
                if (--tryCount == 0)
                {
                    return false;
                }
                Thread.Sleep(1);
            }
            while (true);

            return true;
        }
#endif
    }
}

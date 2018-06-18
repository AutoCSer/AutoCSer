using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// 全局版本更新 示例
    /// </summary>
    class GlobalVersion
    {
        /// <summary>
        /// 版本 0 的数据
        /// </summary>
        public int Value9;

        /// <summary>
        /// 版本 1 增加了一个数据，仅用于模拟历史数据版本
        /// </summary>
        class Version1 : GlobalVersion
        {
            /// <summary>
            /// 版本 1 增加了一个数据
            /// </summary>
            [AutoCSer.BinarySerialize.SerializeMember(GlobalVersion = 1)]
            public int Value8;
        }
        /// <summary>
        /// 版本 2 删除了一个数据，仅用于模拟历史数据版本
        /// </summary>
        class Version2 : GlobalVersion
        {
            /// <summary>
            /// 版本 2 删除了一个数据
            /// </summary>
            [AutoCSer.BinarySerialize.SerializeMember(GlobalVersion = 1, RemoveGlobalVersion = 2)]
            public int Value8;
            /// <summary>
            /// 版本 2 增加了一个数据
            /// </summary>
            [AutoCSer.BinarySerialize.SerializeMember(GlobalVersion = 2)]
            public int Value7;
        }
        /// <summary>
        /// 版本 3 使用静态字段删除了一个数据，仅用于模拟历史数据版本
        /// </summary>
        class Version3 : GlobalVersion
        {
            /// <summary>
            /// 版本 3 删除了一个数据
            /// </summary>
            [AutoCSer.BinarySerialize.SerializeMember(GlobalVersion = 1, RemoveGlobalVersion = 2)]
            public int Value8;
            /// <summary>
            /// 版本 2 增加了一个数据
            /// </summary>
            [AutoCSer.BinarySerialize.SerializeMember(GlobalVersion = 2, RemoveGlobalVersion = 3, IsRemovePublic = true)]
            static int Value7;
            /// <summary>
            /// 版本 3 增加了一个数据
            /// </summary>
            [AutoCSer.BinarySerialize.SerializeMember(GlobalVersion = 3)]
            public int Value6;
        }

        /// <summary>
        /// 全局版本更新 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            #region 序列化版本 0 的数据，手动传 SerializeConfig 参数仅用于测试模拟历史数据版本
            byte[] data0 = AutoCSer.BinarySerialize.Serializer.Serialize(new GlobalVersion { Value9 = 1 }, new AutoCSer.BinarySerialize.SerializeConfig { GlobalVersion = 0 });
            Version1 value1 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version1>(data0);
            if (value1 == null || value1.Value9 != 1)
            {
                return false;
            }
            Version2 value2 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version2>(data0);
            if (value2 == null || value2.Value9 != 1)
            {
                return false;
            }
            Version3 value3 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version3>(data0);
            if (value3 == null || value3.Value9 != 1)
            {
                return false;
            }
            #endregion

            #region 序列化版本 1 的数据，手动传 SerializeConfig 参数仅用于测试模拟历史数据版本
            byte[] data1 = AutoCSer.BinarySerialize.Serializer.Serialize(new Version1 { Value9 = 1, Value8 = 2 }, new AutoCSer.BinarySerialize.SerializeConfig { GlobalVersion = 1 });
            value1 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version1>(data1);
            if (value1 == null || value1.Value9 != 1 || value1.Value8 != 2)
            {
                return false;
            }
            value2 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version2>(data1);
            if (value2 == null || value2.Value9 != 1 || value2.Value8 != 2)
            {
                return false;
            }
            value3 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version3>(data1);
            if (value3 == null || value3.Value9 != 1 || value3.Value8 != 2)
            {
                return false;
            }
            #endregion

            #region 序列化版本 2 的数据，手动传 SerializeConfig 参数仅用于测试模拟历史数据版本
            byte[] data2 = AutoCSer.BinarySerialize.Serializer.Serialize(new Version2 { Value9 = 1, Value8 = 2, Value7 = 3 }, new AutoCSer.BinarySerialize.SerializeConfig { GlobalVersion = 2 });
            value2 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version2>(data2);
            if (value2 == null || value2.Value9 != 1 || value2.Value8 != 0 || value2.Value7 != 3)
            {
                return false;
            }
            value3 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version3>(data2);
            if (value3 == null || value3.Value9 != 1 || value3.Value8 != 0)
            {
                return false;
            }
            #endregion

            #region 序列化版本 3 的数据，手动传 SerializeConfig 参数仅用于测试模拟历史数据版本
            byte[] data3 = AutoCSer.BinarySerialize.Serializer.Serialize(new Version3 { Value9 = 1, Value8 = 2, Value6 = 4 }, new AutoCSer.BinarySerialize.SerializeConfig { GlobalVersion = 3 });
            value3 = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<Version3>(data3);
            if (value3 == null || value3.Value9 != 1 || value3.Value8 != 0 || value3.Value6 != 4)
            {
                return false;
            }
            #endregion

            return true;
        }
    }
}

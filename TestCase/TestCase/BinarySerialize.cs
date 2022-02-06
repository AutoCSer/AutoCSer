﻿using System;

namespace AutoCSer.TestCase
{
    class BinarySerialize
    {
        /// <summary>
        /// 带成员位图的二进制序列化参数配置
        /// </summary>
        private static readonly AutoCSer.BinarySerialize.SerializeConfig serializeConfig = new AutoCSer.BinarySerialize.SerializeConfig();
        /// <summary>
        /// 二进制序列化测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            #region 引用类型二进制序列化测试
            Data.FieldData fieldData = AutoCSer.RandomObject.Creator<Data.FieldData>.Create();
            byte[] data = AutoCSer.BinarySerializer.Serialize(fieldData);
            Data.FieldData newFieldData = AutoCSer.BinaryDeSerializer.DeSerialize<Data.FieldData>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.Equals(fieldData, newFieldData))
            {
                return false;
            }
            #endregion

            #region 带成员位图的引用类型二进制序列化测试
            serializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.FieldData>.NewFull();
            data = AutoCSer.BinarySerializer.Serialize(fieldData, serializeConfig);
            newFieldData = AutoCSer.BinaryDeSerializer.DeSerialize<Data.FieldData>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.FieldData>.MemberMapEquals(fieldData, newFieldData, serializeConfig.MemberMap))
            {
                return false;
            }
            #endregion

            #region 值类型二进制序列化测试
            Data.StructFieldData structFieldData = AutoCSer.RandomObject.Creator<Data.StructFieldData>.Create();
            data = AutoCSer.BinarySerializer.Serialize(structFieldData);
            Data.StructFieldData newStructFieldData = AutoCSer.BinaryDeSerializer.DeSerialize<Data.StructFieldData>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructFieldData>.Equals(structFieldData, newStructFieldData))
            {
                return false;
            }
            #endregion

            #region 带成员位图的值类型二进制序列化测试
            serializeConfig.MemberMap = AutoCSer.Metadata.MemberMap<Data.StructFieldData>.NewFull();
            data = AutoCSer.BinarySerializer.Serialize(structFieldData, serializeConfig);
            newStructFieldData = AutoCSer.BinaryDeSerializer.DeSerialize<Data.StructFieldData>(data);
            if (!AutoCSer.FieldEquals.Comparor<Data.StructFieldData>.MemberMapEquals(structFieldData, newStructFieldData, serializeConfig.MemberMap))
            {
                return false;
            }
            #endregion

            if (AutoCSer.BinaryDeSerializer.DeSerialize<int>(data = AutoCSer.BinarySerializer.Serialize<int>(1)) != 1)
            {
                return false;
            }
            if (AutoCSer.BinaryDeSerializer.DeSerialize<string>(data = AutoCSer.BinarySerializer.Serialize<string>("1")) != "1")
            {
                return false;
            }

            return true;
        }
    }
}

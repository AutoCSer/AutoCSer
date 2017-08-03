using System;

namespace AutoCSer.TestCase.DiskBlock
{
    class File
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        internal const string FileName = "blockFile" + AutoCSer.DiskBlock.File.ExtensionName;
        /// <summary>
        /// 文件块服务测试
        /// </summary>
        /// <returns></returns>
#if TEST
        [AutoCSer.Metadata.TestMethod]
#endif
        internal static bool TestCase()
        {
            AutoCSer.DiskBlock.Member<Data.FieldData> fieldDataMember = default(AutoCSer.DiskBlock.Member<Data.FieldData>);
            Data.FieldData fieldData = AutoCSer.RandomObject.Creator<Data.FieldData>.Create();
            if (!fieldDataMember.Set(fieldData, 0))
            {
                return false;
            }

            AutoCSer.DiskBlock.MemberIndex memberIndex = fieldDataMember;
            fieldDataMember = memberIndex;
            Data.FieldData newFieldData = fieldDataMember.Value.Value;
            if (newFieldData == null || !AutoCSer.FieldEquals.Comparor<Data.FieldData>.Equals(fieldData, newFieldData))
            {
                return false;
            }

            fieldDataMember = memberIndex;
            if (!fieldDataMember.Set(fieldData, 0))
            {
                return false;
            }
            if (!memberIndex.Equals(fieldDataMember))
            {
                return false;
            }
            return true;
        }
    }
}

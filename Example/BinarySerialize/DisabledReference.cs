using System;

namespace AutoCSer.Example.BinarySerialize
{
    /// <summary>
    /// 禁用循环引用 示例
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false)]
    class DisabledReference
    {
        /// <summary>
        /// 禁用循环引用 测试
        /// </summary>
        /// <returns></returns>
        [AutoCSer.Metadata.TestMethod]
        internal static bool TestCase()
        {
            DisabledReference value = new DisabledReference();
            DisabledReference[] array = new DisabledReference[] { value, value };//在数组中引用两次

            byte[] data = AutoCSer.BinarySerialize.Serializer.Serialize(array);
            DisabledReference[] newArray = AutoCSer.BinarySerialize.DeSerializer.DeSerialize<DisabledReference[]>(data);

            return newArray != null && newArray.Length == 2 && newArray[0] != null && newArray[1] != null && newArray[0] != newArray[1];
        }
    }
}

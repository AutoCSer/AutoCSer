using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;DateTime*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [AutoCSer.Json.DeSerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ref ulong[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallSerialize(ref array[index]);
                    if (DeSerializeState == AutoCSer.Json.DeSerializeState.Success) ++index;
                    else return;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
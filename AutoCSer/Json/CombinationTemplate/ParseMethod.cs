using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;bool;DateTime*/

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class Parser
    {
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="array"></param>
        [ParseMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallParse(ref /*Type[0]*/ulong/*Type[0]*/[] array)
        {
            if (searchArraySize(ref array))
            {
                int index = 0;
                do
                {
                    CallParse(ref array[index]);
                    if (ParseState != ParseState.Success) return;
                    ++index;
                }
                while (IsNextArrayValue());
            }
        }
    }
}
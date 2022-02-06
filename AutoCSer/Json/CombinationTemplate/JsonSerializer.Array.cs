using System;
/*ulong;long;uint;int;ushort;short;byte;sbyte;bool;DateTime*/

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [AutoCSer.Json.SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(ulong[] array)
        {
            if (array != null)
            {
                switch (array.Length)
                {
                    case 0: CharStream.WriteJsonArray(); return;
                    case 1:
                        CharStream.Write('[');
                        CallSerialize(array[0]);
                        CharStream.Write(']');
                        return;
                    default:
                        bool isNext = false;
                        CharStream.Write('[');
                        foreach (ulong value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CallSerialize(value);
                        }
                        CharStream.Write(']');
                        return;
                }
            }
            CharStream.WriteJsonNull();
        }
    }
}

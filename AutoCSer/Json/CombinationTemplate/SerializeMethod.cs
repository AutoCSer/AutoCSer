using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;bool;DateTime*/

namespace AutoCSer.Json
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class Serializer
    {
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">数组</param>
        [SerializeMethod]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public void CallSerialize(/*Type[0]*/ulong/*Type[0]*/[] array)
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
                        foreach (/*Type[0]*/ulong/*Type[0]*/ value in array)
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

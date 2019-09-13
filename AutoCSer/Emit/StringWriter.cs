using System;
using AutoCSer.Extension;
using System.Reflection.Emit;

namespace AutoCSer.Emit
{
    /// <summary>
    /// 字符串写入器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct StringWriter
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILGenerator generator;
        /// <summary>
        /// 
        /// </summary>
        private readonly OpCode target;
        /// <summary>
        /// 写入阶段
        /// </summary>
        private int step;
        /// <summary>
        /// 未写入字节数
        /// </summary>
        private int size;
        /// <summary>
        /// 写入数据缓冲区
        /// </summary>
        private ulong value0;
        /// <summary>
        /// 写入数据缓冲区
        /// </summary>
        private ulong value1;
        /// <summary>
        /// 写入数据缓冲区
        /// </summary>
        private ulong value2;
        /// <summary>
        /// 写入数据缓冲区
        /// </summary>
        private ulong value3;
        /// <summary>
        /// 字符串写入器
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="size"></param>
        internal StringWriter(ILGenerator generator, OpCode target, int size)
        {
            this.generator = generator;
            this.target = target;
            this.size = size;
            value0 = value1 = value2 = value3 = 0;
            step = 0;

            generator.unmanagedStreamBasePrepSize(target, size + 6);
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        internal void Write(string value)
        {
            foreach (char code in value) Write(code);
        }
        /// <summary>
        /// 添加字符
        /// </summary>
        /// <param name="value"></param>
        internal void Write(char value)
        {
            switch (step)
            {
                case 0: value0 = value; break;
                case 1: value0 |= ((ulong)value << 16); break;
                case 2: value0 |= ((ulong)value << 32); break;
                case 3: value0 |= ((ulong)value << 48); break;
                case 4: value1 = value; break;
                case 5: value1 |= ((ulong)value << 16); break;
                case 6: value1 |= ((ulong)value << 32); break;
                case 7: value1 |= ((ulong)value << 48); break;
                case 8: value2 = value; break;
                case 9: value2 |= ((ulong)value << 16); break;
                case 10: value2 |= ((ulong)value << 32); break;
                case 11: value2 |= ((ulong)value << 48); break;
                case 12: value3 = value; break;
                case 13: value3 |= ((ulong)value << 16); break;
                case 14: value3 |= ((ulong)value << 32); break;
                case 15:
                    step -= 16;
                    size -= sizeof(ulong) * 4;
                    generator.unmanagedStreamBaseUnsafeWrite(target, value0, value1, value2, value3 | ((ulong)value << 48));
                    break;
            }
            ++step;
        }
        /// <summary>
        /// 写入结束
        /// </summary>
        internal void WriteEnd()
        {
            switch ((step + 3) >> 2)
            {
                case 1: generator.unmanagedStreamBaseUnsafeWrite(target, value0, size); break;
                case 2: generator.unmanagedStreamBaseUnsafeWrite(target, value0, value1, size); break;
                case 3: generator.unmanagedStreamBaseUnsafeWrite(target, value0, value1, value2, size); break;
                case 4: generator.unmanagedStreamBaseUnsafeWrite(target, value0, value1, value2, value3, size); break;
            }
        }

        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        internal static void Write(ILGenerator generator, OpCode target, string value)
        {
            StringWriter stringWriter = new StringWriter(generator, target, value.Length << 1);
            stringWriter.Write(value);
            stringWriter.WriteEnd();
        }
    }
}

using System;

namespace AutoCSer.TestCase.RadixSortPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/
");
            int count = (1 << 10) * 10000;
            do
            {
                Int32.Test(count);
                GC.Collect();

                UInt32.Test(count);
                GC.Collect();

                Int64.Test(count);
                GC.Collect();

                UInt64.Test(count);
                GC.Collect();

                KeyInt32.Test(count);
                GC.Collect();

                KeyUInt32.Test(count);
                GC.Collect();

                KeyInt64.Test(count);
                GC.Collect();

                KeyUInt64.Test(count);
                GC.Collect();

                Console.WriteLine("press quit to exit.");
            }
            while (Console.ReadLine() != "quit");
        }
    }
}

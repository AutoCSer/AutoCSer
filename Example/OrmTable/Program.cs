using System;
using AutoCSer.Extension;

namespace AutoCSer.Example.OrmTable
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/OrmCache/Index.html
");
            OrmOnly.Test();
            NowTime.Test();

            Console.WriteLine("Over");
            Console.ReadKey();
        }
    }
}

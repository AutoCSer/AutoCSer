using System;

namespace AutoCSer.Example.Xml
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/Serialize/Xml.html
");
            Console.WriteLine(PublicInstanceField.TestCase());
            Console.WriteLine(AnonymousType.TestCase());
            Console.WriteLine(MemberMap.TestCase());
            Console.WriteLine(MemberMapValue.TestCase());
            Console.WriteLine(IgnoreMember.TestCase());
            Console.WriteLine(BaseType.TestCase());
            Console.WriteLine(CustomClass.TestCase());
            Console.WriteLine(CustomStruct.TestCase());
            Console.WriteLine(NoConstructor.TestCase());
            Console.WriteLine("Over");
            Console.ReadKey();
        }
    }
}

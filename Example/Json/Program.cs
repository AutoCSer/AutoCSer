using System;

namespace AutoCSer.Example.Json
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/Serialize/Json.html
");
            Console.WriteLine(PublicInstanceField.TestCase());
            Console.WriteLine(AnonymousType.TestCase());
            Console.WriteLine(MemberMap.TestCase());
            Console.WriteLine(MemberMapValue.TestCase());
            Console.WriteLine(IgnoreMember.TestCase());
            Console.WriteLine(SerializeIgnoreMember.TestCase());
            Console.WriteLine(ParseIgnoreMember.TestCase());
            Console.WriteLine(CustomClass.TestCase());
            Console.WriteLine(CustomStruct.TestCase());
            Console.WriteLine(BaseType.TestCase());
            Console.WriteLine(NoConstructor.TestCase());
            Console.WriteLine(SerializeNode.TestCase());
            Console.WriteLine("Over");
            Console.ReadKey();
        }
    }
}

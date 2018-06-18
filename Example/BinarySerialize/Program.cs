using System;

namespace AutoCSer.Example.BinarySerialize
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/Serialize/Binary.html
");

            Console.WriteLine(Reference.TestCase());
            Console.WriteLine(DisabledReference.TestCase());
            Console.WriteLine(PublicInstanceField.TestCase());
            Console.WriteLine(AnonymousType.TestCase());
            Console.WriteLine(MemberMap.TestCase());
            Console.WriteLine(DisabledMemberMap.TestCase());
            Console.WriteLine(MemberMapValue.TestCase());
            Console.WriteLine(Property.TestCase());
            Console.WriteLine(IgnoreMember.TestCase());
            Console.WriteLine(CustomClass.TestCase());
            Console.WriteLine(CustomStruct.TestCase());
            Console.WriteLine(GlobalVersion.TestCase());
            Console.WriteLine(Json.TestCase());
            Console.WriteLine(BaseType.TestCase());
            Console.WriteLine(RealType.TestCase());
            Console.WriteLine("Over");
            Console.ReadKey();
        }
    }
}

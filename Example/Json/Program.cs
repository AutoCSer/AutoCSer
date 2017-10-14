using System;

namespace AutoCSer.Example.Json
{
    class Program
    {
        static int[] mod5s = new int[] { 1, 1, 2, 1, 4 };
        static int[] mod2s = new int[] { 1, 2, 4, 3 };
        static int mod5(int value)
        {
            if (value >= 5)
            {
                int div = value / 5;
                return (mod5(div) * mod5s[value % 5] * mod2s[div % 4]) % 5;
            }
            return mod5s[value];
        }
        static int mod10(int value)
        {
            if (value <= 1) return 1;
            value = mod5(value);
            return (value % 2) == 0 ? value : (value + 5);
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine(mod10(100));

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

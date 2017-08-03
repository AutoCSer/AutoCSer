using System;
using System.Diagnostics;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator.X86
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    args = AutoCSer.Json.Parser.Parse<string[]>(File.ReadAllText(args[0]));
                    if (args.Length >= 4) AutoCSer.CodeGenerator.Program.X86(args);
                }
            }
            catch (Exception error)
            {
                Messages.Add(error);
            }
            finally { Messages.Open(); }
        }
    }
}

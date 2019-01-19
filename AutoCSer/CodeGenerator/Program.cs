using System;
using System.Diagnostics;
using System.IO;
using AutoCSer.Extension;

namespace AutoCSer.CodeGenerator
{
    public class Program
    {
        /// <summary>
        /// 尝试兼容平台
        /// </summary>
#if X64
       private const string platform = "X64";
#else
        private const string platform = "X86";
#endif

        static void Main(string[] args)
        {
            try
            {
                AutoCSer.Log.Pub.Log.Wait(Log.LogType.Info, string.Join(@""", @""", args));
                //args = new string[] { @"AutoCSer.TestCase.SqlTableCacheServer", @"C:\AutoCSerNew\TestCase\SqlTableCacheServer\ ", @"C:\AutoCSerNew\TestCase\SqlTableCacheServer\bin\Release\AutoCSer.TestCase.SqlTableCacheServer.dll ", @"AutoCSer.TestCase.SqlTableCacheServer" };
                //                AutoCSer.Log.Pub.Log.Wait(AutoCSer.Log.LogType.All, "args.Length[" + args.Length.ToString() + @"]
                //args = new string[] { @"AutoCSer.CodeGenerator.DotNet4.5", @"C:\AutoCSer\AutoCSer\CodeGenerator\ ", @"C:\AutoCSer\Packet\DotNet4.5\AutoCSer.CodeGenerator.exe ", @"AutoCSer.CodeGenerator" };
                if (args.Length >= 4)
                {
                    AutoCSer.CodeGenerator.ProjectParameter parameter = new AutoCSer.CodeGenerator.ProjectParameter(args[0].TrimEnd(' '), args[1].TrimEnd(' '), args[2].TrimEnd(' '), args[3].TrimEnd(' '), args.Length > 4);
                    bool isAssemblyPath = false;
                    Exception exception = parameter.LoadAssembly(ref isAssemblyPath);
                    if (isAssemblyPath)
                    {
                        if (exception == null) parameter.Start();
                        else
                        {
#if !DotNetStandard
                            FileInfo file = new FileInfo(AutoCSer.PubPath.ApplicationPath + "AutoCSer.CodeGenerator." + platform + ".exe");
                            if (file.Exists)
                            {
                                string fileName = AutoCSer.IO.File.BakPrefix + ((ulong)DateTime.Now.Ticks).toHex() + ((uint)AutoCSer.Random.Default.Next()).toHex() + "." + platform;
                                File.WriteAllText(fileName, AutoCSer.Json.Serializer.Serialize(args));
                                Process process = Process.Start(file.FullName, fileName);
                                process.WaitForExit();
                                File.Delete(fileName);
                            }
                            else
#endif
                            {
                                Messages.Add(exception);
                            }
                        }
                    }
                    else Messages.Add("未找到程序集文件 : " + parameter.AssemblyPath);
                }
            }
            catch (Exception error)
            {
                Messages.Add(error);
            }
            finally { Messages.Open(); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
#if X64
        public static void X64(string[] args)
#else
        public static void X86(string[] args)
#endif
        {
            try
            {
                Console.WriteLine(args.joinString(" "));
                AutoCSer.CodeGenerator.ProjectParameter parameter = new AutoCSer.CodeGenerator.ProjectParameter(args[0].TrimEnd(' '), args[1].TrimEnd(' '), args[2].TrimEnd(' '), args[3].TrimEnd(' '), args.Length > 4); bool isAssemblyPath = false;
                Exception exception = parameter.LoadAssembly(ref isAssemblyPath);
                if (exception == null) parameter.Start();
                else Messages.Add(exception);
            }
            catch (Exception error)
            {
                Messages.Add(error);
            }
            finally { Messages.Open(); }
        }
    }
}

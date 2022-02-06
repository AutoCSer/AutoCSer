using System;
using System.Diagnostics;
using System.IO;
using AutoCSer.Extensions;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 
    /// </summary>
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
                AutoCSer.LogHelper.Info(string.Join(@""", @""", args), LogLevel.Info | LogLevel.AutoCSer);
                AutoCSer.LogHelper.Flush();
                //args = new string[] { @"AutoCSer.TestCase.SqlTableCacheServer", @"C:\AutoCSerNew\TestCase\SqlTableCacheServer\ ", @"C:\AutoCSerNew\TestCase\SqlTableCacheServer\bin\Release\AutoCSer.TestCase.SqlTableCacheServer.dll ", @"AutoCSer.TestCase.SqlTableCacheServer" };
                //                AutoCSer.LogHelper.Wait(AutoCSer.LogLevel.All, "args.Length[" + args.Length.ToString() + @"]
                 //args = new string[] { @"AutoCSer.Web.DotNet2", @"C:\AutoCSer\Web\www.AutoCSer.com\ ", @"C:\AutoCSer\Web\www.AutoCSer.com\bin\Release\AutoCSer.Web.exe ", @"AutoCSer.Web" };
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
                            FileInfo file = new FileInfo(AutoCSer.Config.ApplicationPath + "AutoCSer.CodeGenerator." + platform + ".exe");
                            if (file.Exists)
                            {
                                string fileName = AutoCSer.IO.File.BakPrefix + ((ulong)DateTime.Now.Ticks).toHex() + ((uint)AutoCSer.Random.Default.Next()).toHex() + "." + platform;
                                File.WriteAllText(fileName, AutoCSer.JsonSerializer.Serialize(args));
                                Process process = Process.Start(file.FullName, fileName);
                                process.WaitForExit();
                                File.Delete(fileName);
                            }
                            else
#endif
                            {
                                Messages.Exception(exception);
                            }
                        }
                    }
                    else Messages.Error("未找到程序集文件 : " + parameter.AssemblyPath);
                }
            }
            catch (Exception error)
            {
                Messages.Exception(error);
            }
            finally
            {
                Messages.Open();
            }
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
                else Messages.Exception(exception);
            }
            catch (Exception error)
            {
                Messages.Exception(error);
            }
            finally { Messages.Open(); }
        }
    }
}

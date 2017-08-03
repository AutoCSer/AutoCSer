using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace AutoCSer.Tool.OpenPack
{
    class Program
    {
        /// <summary>
        /// ZIP 文件名称
        /// </summary>
        private const string zipFileName = "AutoCSer.Example.zip";
        /// <summary>
        /// 第三方 dll 文件名称集合
        /// </summary>
        private static readonly HashSet<string> thirdPartyFileNames = new HashSet<string>();
        /// <summary>
        /// ZIP 压缩包
        /// </summary>
        private static ZipArchive zipArchive;
        static void Main(string[] args)
        {
            string zipPath = new DirectoryInfo(@"..\..\..\..\Web\www.AutoCSer.com\Download\").FullName;
            //if (File.Exists(zipPath + zipFileName)) File.Delete(zipPath + zipFileName);
            foreach (FileInfo file in new DirectoryInfo(@"..\..\..\..\ThirdParty\").GetFiles()) thirdPartyFileNames.Add(file.Name.ToLower());
            using (MemoryStream stream = new MemoryStream())
            {
                using (zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true)) boot(new DirectoryInfo(@"..\..\..\..\"));
                using (FileStream packFile = new FileStream(zipPath + zipFileName, FileMode.Create)) packFile.Write(stream.GetBuffer(), 0, (int)stream.Position);
            }
            Console.WriteLine(zipPath + zipFileName);
            Console.ReadKey();
        }
        /// <summary>
        /// 根目录处理
        /// </summary>
        /// <param name="directory"></param>
        private static void boot(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                string fileName = file.Name;
                if (fileName.IndexOf(".example", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    using (Stream entryStream = zipArchive.CreateEntry(fileName).Open())
                    {
                        byte[] data = File.ReadAllBytes(file.FullName);
                        entryStream.Write(data, 0, data.Length);
                    }
                }
            }
            foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
            {
                switch (nextDircectory.Name.ToLower())
                {
                    case ".vs": vs(nextDircectory); break;
                    case "autocser": AutoCSer(nextDircectory); break;
                    case "packet": case "thirdparty": case "example": case "testcase": case "web": copy(nextDircectory, nextDircectory.Name + @"\"); break;
                }
            }
        }
        /// <summary>
        /// VS 目录处理
        /// </summary>
        /// <param name="directory"></param>
        private static void vs(DirectoryInfo directory)
        {
            string path = directory.Name + @"\";
            foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
            {
                if (nextDircectory.Name.IndexOf(".example", StringComparison.OrdinalIgnoreCase) > 0) copy(nextDircectory, path + nextDircectory.Name + @"\");
            }
        }
        /// <summary>
        /// AutoCSer 项目目录处理
        /// </summary>
        /// <param name="directory"></param>
        private static void AutoCSer(DirectoryInfo directory)
        {
            string path = directory.Name + @"\";
            foreach (FileInfo file in directory.GetFiles())
            {
                string fileName = file.Name;
                bool isFile = false;
                if (file.Extension == ".cs")
                {
                    isFile = fileName == "{AutoCSer}.CSharper.cs"
                        || fileName.StartsWith("Program.", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("{AutoCSer.DiskBlock", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("{AutoCSer.Deploy", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    isFile = fileName.StartsWith("AutoCSer.CodeGenerator", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("AutoCSer.DiskBlock", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("AutoCSer.Sql", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("AutoCSer.MySql", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("AutoCSer.Deploy", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("AutoCSer.Drawing.Gif", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("AutoCSer.FieldEquals", StringComparison.OrdinalIgnoreCase)
                        || fileName.StartsWith("AutoCSer.RandomObject", StringComparison.OrdinalIgnoreCase);
                }
                if (isFile)
                {
                    using (Stream entryStream = zipArchive.CreateEntry(path + fileName).Open())
                    {
                        byte[] data = File.ReadAllBytes(file.FullName);
                        entryStream.Write(data, 0, data.Length);
                    }
                }
            }
            foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
            {
                int isDircectory;
                switch (nextDircectory.Name.Length)
                {
                    case 2: isDircectory = string.Compare(nextDircectory.Name, "js", true); break;
                    case 3: isDircectory = string.Compare(nextDircectory.Name, "Sql", true); break;
                    case 4:
                        if (string.Compare(nextDircectory.Name, "Emit", true) == 0) sql(nextDircectory, path + nextDircectory.Name + @"\", sqlEmitFileNames);
                        isDircectory = 1;
                        break;
                    case 6: isDircectory = string.Compare(nextDircectory.Name, "Deploy", true); break;
                    case 7: isDircectory = string.Compare(nextDircectory.Name, "Drawing", true); break;
                    case 8:
                        if (string.Compare(nextDircectory.Name, "Metadata", true) == 0) sql(nextDircectory, path + nextDircectory.Name + @"\", sqlMetadataFileNames);
                        isDircectory = 1;
                        break;
                    case 9:
                        isDircectory = string.Compare(nextDircectory.Name, "DiskBlock", true);
                        if (isDircectory == 1 && string.Compare(nextDircectory.Name, "Extension", true) == 0) sql(nextDircectory, path + nextDircectory.Name + @"\", extensionFileNames);
                        break;
                    case 10: isDircectory = string.Compare(nextDircectory.Name, "Properties", true); break;
                    case 11: isDircectory = string.Compare(nextDircectory.Name, "FieldEquals", true); break;
                    case 12: isDircectory = string.Compare(nextDircectory.Name, "RandomObject", true); break;
                    case 13: isDircectory = string.Compare(nextDircectory.Name, "CodeGenerator", true); break;
                    default: isDircectory = 1; break;
                }
                if (isDircectory == 0) copy(nextDircectory, path + nextDircectory.Name + @"\");
            }
        }
        /// <summary>
        /// Sql Emit 程序文件
        /// </summary>
        private static readonly string[] sqlEmitFileNames = new string[] { "CastType.cs", "NullableConstructor.cs" };
        /// <summary>
        /// Sql Extension 程序文件
        /// </summary>
        private static readonly string[] extensionFileNames = new string[] { "EmitGenerator.Sql.cs", "Expression.cs", "SqlTable.cs", "Type.Sql.cs"/**/, "MethodInfo.cs", "Type.CodeGenerator.cs" };
        /// <summary>
        /// Sql Metadata 程序文件
        /// </summary>
        private static readonly string[] sqlMetadataFileNames = new string[] { "MemberMapValue.cs" , "MemberMapValueJsonSerializeConfig.cs" };
        /// <summary>
        /// Sql 程序文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="path"></param>
        /// <param name="fileNames"></param>
        private static void sql(DirectoryInfo directory, string path, string[] fileNames)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                string fileName = file.Name;
                foreach (string name in fileNames)
                {
                    if (name == fileName)
                    {
                        using (Stream entryStream = zipArchive.CreateEntry(path + fileName).Open())
                        {
                            byte[] data = File.ReadAllBytes(file.FullName);
                            entryStream.Write(data, 0, data.Length);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="path"></param>
        private static void copy(DirectoryInfo directory, string path)
        {
            if (string.Compare(path, @"Web\www.AutoCSer.com\Download\", true) != 0)
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    string fileName = file.Name;
                    bool isDelete = false;
                    if (fileName[0] == '%') isDelete = true;
                    else
                    {
                        switch (file.Extension)
                        {
                            case ".png": fileName = null; break;
                            case ".cs":
                                if (string.Compare(fileName, "Pub.cs", true) == 0 && string.Compare(path, @"Web\Config\", true) == 0)
                                {
                                    using (Stream entryStream = zipArchive.CreateEntry(path + fileName).Open())
                                    {
                                        string code = File.ReadAllText(file.FullName).Replace(@" public static readonly bool IsLocal = false;", @" public static readonly bool IsLocal = true;");
                                        code = "000" + new Regex(@" public const string TcpVerifyString = ""([^""]+)"";").Replace(code, match => @" public const string TcpVerifyString = ""XXX"";");
                                        byte[] data = System.Text.Encoding.UTF8.GetBytes(code);
                                        data[0] = 0xef;
                                        data[1] = 0xbb;
                                        data[2] = 0xbf;
                                        entryStream.Write(data, 0, data.Length);
                                    }
                                    fileName = null;
                                }
                                break;
                            case ".dll":
                                if (string.Compare(path, @"ThirdParty\", true) != 0 && thirdPartyFileNames.Contains(fileName.ToLower())) fileName = null;
                                break;
                            case ".txt":
                                if (string.Compare(fileName, "ip.txt", true) == 0) fileName = null;
                                else if (fileName.StartsWith("log_default", StringComparison.OrdinalIgnoreCase)) isDelete = true;
                                break;
                        }
                    }
                    if (isDelete)
                    {
                        file.Attributes = 0;
                        file.Delete();
                    }
                    else if (fileName != null)
                    {
                        using (Stream entryStream = zipArchive.CreateEntry(path + fileName).Open())
                        {
                            byte[] data = File.ReadAllBytes(file.FullName);
                            entryStream.Write(data, 0, data.Length);
                        }
                    }
                }
                foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
                {
                    switch (nextDircectory.Name.ToLower())
                    {
                        case "bin": case "obj": break;
                        default: copy(nextDircectory, path + nextDircectory.Name + @"\"); break;
                    }
                }
            }
        }
    }
}

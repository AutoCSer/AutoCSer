using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const string zipFileName = "AutoCSer.zip";
        /// <summary>
        /// ZIP 文件名称
        /// </summary>
        private const string zip2FileName = "AutoCSer2.zip";
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
            if (new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Name == "AutoCSer.Web.Pack.exe")
            {
                string zipPath = new DirectoryInfo(@"..\..\..\..\Web\www.AutoCSer.com\Download\").FullName;
                DirectoryInfo githubDirectory = new DirectoryInfo(@"..\..\..\..\Github\");
                if(!githubDirectory.Exists) githubDirectory.Create();
                string[] githubPath = new string[] { githubDirectory.FullName };
                foreach (FileInfo file in new DirectoryInfo(@"..\..\..\..\ThirdParty\").GetFiles()) thirdPartyFileNames.Add(file.Name.ToLower());
                using (FileStream stream = new FileStream(zipPath + zipFileName, FileMode.Create))
                using (zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    boot(new DirectoryInfo(@"..\..\..\..\"), githubPath);
                }
                Console.WriteLine(zipPath + zipFileName);

                githubDirectory = new DirectoryInfo(@"..\..\..\..\..\AutoCSer2\Github\");
                if (!githubDirectory.Exists) githubDirectory.Create();
                githubPath = new string[] { githubDirectory.FullName, null };
                githubDirectory = new DirectoryInfo(@"..\..\..\..\..\AutoCSer2\AtomGit\");
                if (!githubDirectory.Exists) githubDirectory.Create();
                githubPath[1] = githubDirectory.FullName;
                using (FileStream stream = new FileStream(zipPath + zip2FileName, FileMode.Create))
                using (zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    boot2(new DirectoryInfo(@"..\..\..\..\..\AutoCSer2\"), githubPath);
                }
                Console.WriteLine(zipPath + zip2FileName);
            }
            else
            {
                Console.WriteLine("Please enter the path ...");
                string path = Console.ReadLine();
                if (Directory.Exists(path))
                {
                    using (FileStream stream = new FileStream(newPackZipFileName, FileMode.Create))
                    using (zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                    {
                        newPack(new DirectoryInfo(path), string.Empty);
                        //foreach (DirectoryInfo directory in new DirectoryInfo(path).GetDirectories()) newPack(directory, directory.Name + @"\");
                    }
                    Console.WriteLine(newPackZipFileName);
                }
                else Console.WriteLine("Not found path " + path);
            }
            Console.ReadKey();
        }

        private const string newPackZipFileName = @"C:\AutoCSer2\temp.zip";
        private readonly static DateTime newOutputTime = new DateTime(2020, 1, 28);
        private static bool newOutput;
        private static void newPack(DirectoryInfo directory, string path)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                string fileName = file.Name;
                using (Stream entryStream = zipArchive.CreateEntry(path + fileName).Open())
                {
                    try
                    {
                        if (!newOutput && file.Extension == ".cs" && file.LastWriteTimeUtc > newOutputTime)
                        {
                            newOutput = true;
                            Console.WriteLine(File.ReadAllText(file.FullName));
                        }
                        githubFile(file.Name, File.ReadAllBytes(file.FullName), entryStream, null, null);
                    }
                    catch { }
                }
            }
            foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
            {
                switch (nextDircectory.Name)
                {
                    case "bin":
                    case "obj":
                    case ".vs":
                    case ".git": break;
                    default: newPack(nextDircectory, path + nextDircectory.Name + @"\"); break;
                }
            }
        }
        /// <summary>
        /// 根目录处理
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="githubPath"></param>
        private static void boot(DirectoryInfo directory, string[] githubPath)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                string fileName = file.Name;
                //if (fileName.IndexOf(".example", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    using (Stream entryStream = zipArchive.CreateEntry(fileName).Open()) githubFile(file, entryStream, githubPath, false);
                }
            }
            foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
            {
                switch (nextDircectory.Name.ToLower())
                {
                    case ".vs":
                        //vs(nextDircectory, githubPath); break;
                    case "autocser":
                    //AutoCSer(nextDircectory, githubPath); break;
                    case "example":
                    //case "packet":
                    case "testcase":
                    case "thirdparty":
                    case "web": copy(nextDircectory, nextDircectory.Name + @"\", githubPath, false); break;
                }
            }
        }
        /// <summary>
        /// github 文件处理
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entryStream"></param>
        /// <param name="githubPath"></param>
        /// <param name="checkUtf8"></param>
        private unsafe static void githubFile(FileInfo file, Stream entryStream, string[] githubPath, bool checkUtf8)
        {
            githubFile(file.Name, File.ReadAllBytes(file.FullName), entryStream, githubPath, checkUtf8 ? file : null);
        }
        /// <summary>
        /// github 文件处理
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <param name="entryStream"></param>
        /// <param name="githubPaths"></param>
        /// <param name="file"></param>
        private unsafe static void githubFile(string fileName, byte[] data, Stream entryStream, string[] githubPaths, FileInfo file)
        {
            if (file != null)
            {
                switch (file.Extension)
                {
                    case ".cs":
                    case ".ts":
                    //case ".js":
                    case ".html":
                        if (data.Length < 3 || data[0] != 0xef || data[1] != 0xbb || data[2] != 0xbf)
                        {
                            Console.WriteLine("文件 " + file.FullName + " 缺少 UTF-8 BOM");
                            //if (file.Extension == ".cs")
                            {
                                string notepad = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "notepad.exe");
                                using (Process process = File.Exists(notepad) ? Process.Start(notepad, file.FullName) : Process.Start(file.FullName)) process.WaitForExit();
                            }
                        }
                        break;
                }
            }
            githubFile(fileName, data, entryStream, githubPaths);
        }
        /// <summary>
        /// github 文件处理
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <param name="entryStream"></param>
        /// <param name="githubPaths"></param>
        private unsafe static void githubFile(string fileName, byte[] data, Stream entryStream, string[] githubPaths)
        {
            entryStream.Write(data, 0, data.Length);
            foreach (string githubPath in githubPaths)
            {
                FileInfo githubFile = new FileInfo(githubPath + fileName);
                bool isFile = true;
                if (githubFile.Exists && githubFile.Length == data.Length)
                {
                    byte[] githubData = File.ReadAllBytes(githubFile.FullName);
                    isFile = false;
                    fixed (byte* dataFixed = data, githubDataFixed = githubData)
                    {
                        byte* start = dataFixed, end = dataFixed + (data.Length & (int.MaxValue - 7)), githubStart = githubDataFixed;
                        while (start != end)
                        {
                            if (*(ulong*)start != *(ulong*)githubStart)
                            {
                                isFile = true;
                                break;
                            }
                            start += sizeof(ulong);
                            githubStart += sizeof(ulong);
                        }
                        end += data.Length & 7;
                        while (start != end)
                        {
                            if (*start++ != *githubStart++) isFile = true;
                        }
                    }
                }
                if (isFile) File.WriteAllBytes(githubFile.FullName, data);
            }
        }
        /// <summary>
        /// github 目录处理
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="githubPaths"></param>
        private static string[] checkGithubPath(DirectoryInfo directory, string[] githubPaths)
        {
            int index = 0;
            string[] nextGithubPath = new string[githubPaths.Length];
            foreach (string githubPath in githubPaths)
            {
                string path = githubPath + directory.Name + @"\";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                nextGithubPath[index++] = path;
            }
            return nextGithubPath;
        }
        ///// <summary>
        ///// VS 目录处理
        ///// </summary>
        ///// <param name="directory"></param>
        ///// <param name="githubPath"></param>
        //private static void vs(DirectoryInfo directory, string githubPath)
        //{
        //    githubPath = checkGithubPath(directory, githubPath);
        //    string path = directory.Name + @"\";
        //    foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
        //    {
        //        if (nextDircectory.Name.IndexOf(".example", StringComparison.OrdinalIgnoreCase) > 0) copy(nextDircectory, path + nextDircectory.Name + @"\", githubPath);
        //    }
        //}
        ///// <summary>
        ///// AutoCSer 项目目录处理
        ///// </summary>
        ///// <param name="directory"></param>
        ///// <param name="githubPath"></param>
        //private static void AutoCSer(DirectoryInfo directory, string githubPath)
        //{
        //    githubPath = checkGithubPath(directory, githubPath);
        //    string path = directory.Name + @"\";
        //    foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
        //    {
        //        int isDircectory;
        //        switch (nextDircectory.Name.Length)
        //        {
        //            case 2: isDircectory = string.Compare(nextDircectory.Name, "js", true); break;
        //            case 3: isDircectory = string.Compare(nextDircectory.Name, "Sql", true); break;
        //            case 5: isDircectory = string.Compare(nextDircectory.Name, "MySql", true); break;
        //            case 6:
        //                isDircectory = string.Compare(nextDircectory.Name, "Deploy", true);
        //                if (isDircectory != 0) isDircectory = string.Compare(nextDircectory.Name, "Search", true);
        //                if (isDircectory != 0) isDircectory = string.Compare(nextDircectory.Name, "Expand", true);
        //                break;
        //            case 7:
        //                isDircectory = string.Compare(nextDircectory.Name, "Drawing", true);
        //                if (isDircectory != 0) isDircectory = string.Compare(nextDircectory.Name, "OpenAPI", true);
        //                break;
        //            case 8: isDircectory = string.Compare(nextDircectory.Name, "HtmlNode", true); break;
        //            case 9:
        //                isDircectory = string.Compare(nextDircectory.Name, "DiskBlock", true);
        //                if (isDircectory != 0) isDircectory = string.Compare(nextDircectory.Name, "WebClient", true);
        //                if (isDircectory != 0) isDircectory = string.Compare(nextDircectory.Name, "HtmlTitle", true);
        //                break;
        //            case 10: isDircectory = string.Compare(nextDircectory.Name, "Properties", true); break;
        //            case 11:
        //                isDircectory = string.Compare(nextDircectory.Name, "FieldEquals", true);
        //                if (isDircectory != 0) isDircectory = string.Compare(nextDircectory.Name, "CacheServer", true);
        //                break;
        //            case 12: isDircectory = string.Compare(nextDircectory.Name, "RandomObject", true); break;
        //            case 13: isDircectory = string.Compare(nextDircectory.Name, "CodeGenerator", true); break;
        //            case 15:
        //                isDircectory = string.Compare(nextDircectory.Name, "TcpStreamServer", true);
        //                if (isDircectory != 0) isDircectory = string.Compare(nextDircectory.Name, "TcpSimpleServer", true);
        //                break;
        //            case 16: isDircectory = string.Compare(nextDircectory.Name, "DataSetSerialize", true); break;
        //            case 17: isDircectory = string.Compare(nextDircectory.Name, "RawSocketListener", true); break;
        //            default: isDircectory = 1; break;
        //        }
        //        if (isDircectory == 0) copy(nextDircectory, path + nextDircectory.Name + @"\", githubPath);
        //    }
        //}
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="path"></param>
        /// <param name="githubPath"></param>
        /// <param name="checkUtf8"></param>
        private static void copy(DirectoryInfo directory, string path, string[] githubPath, bool checkUtf8)
        {
            githubPath = checkGithubPath(directory, githubPath);
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
                                        code = new Regex(@" ""[^""]+""; *//SECRET").Replace(code, match => @" ""XXX"";");
                                        code = new Regex(@" 0x[0-9A-Za-z]+UL; *//SECRET").Replace(code, match => " 1UL;");
                                        byte[] data = System.Text.Encoding.UTF8.GetBytes("000" + code);
                                        data[0] = 0xef;
                                        data[1] = 0xbb;
                                        data[2] = 0xbf;
                                        githubFile(fileName, data, entryStream, githubPath);
                                    }
                                    fileName = null;
                                }
                                break;
                            case ".dll":
                                if (string.Compare(path, @"ThirdParty\", true) != 0 && thirdPartyFileNames.Contains(fileName.ToLower())) fileName = null;
                                break;
                            case ".log":
                                if (fileName.StartsWith("AutoCSer", StringComparison.OrdinalIgnoreCase)) isDelete = true;
                                break;
                            case ".txt":
                                if (string.Compare(fileName, "ip.txt", true) == 0) fileName = null;
                                else if (fileName.StartsWith("log_default", StringComparison.OrdinalIgnoreCase)) isDelete = true;
                                break;
                            case ".json":
                                if (fileName == "launchSettings.json")
                                {
                                    if (File.ReadAllText(file.FullName, System.Text.Encoding.UTF8) == @"{
  ""profiles"": {
    ""WSL"": {
      ""commandName"": ""WSL2"",
      ""distributionName"": """"
    }
  }
}")
                                    {
                                        isDelete = true;
                                    }
                                }
                                else if (fileName.EndsWith(".runtimeconfig.dev.json", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".runtimeconfig.json", StringComparison.OrdinalIgnoreCase)) fileName = null;
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
                        using (Stream entryStream = zipArchive.CreateEntry(path + fileName).Open()) githubFile(file, entryStream, githubPath, checkUtf8);
                    }
                }
                foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
                {
                    switch (nextDircectory.Name.ToLower())
                    {
                        case "bin":
                        case "obj":
                        case "remotecontrol":
                        case "remotecontrolclient":
                        case "remotecontrolserver":

                        case "memorycache":
                        case "highavailabilityconsistency":
                            break;
                        default: copy(nextDircectory, path + nextDircectory.Name + @"\", githubPath, checkUtf8); break;
                    }
                }
            }
        }
        /// <summary>
        /// 根目录处理
        /// </summary>
        /// <param name="directory"></param>
        private static void boot2(DirectoryInfo directory, string[] githubPath)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                string fileName = file.Name;
                //if (fileName.IndexOf(".example", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    using (Stream entryStream = zipArchive.CreateEntry(fileName).Open()) githubFile(file, entryStream, githubPath, true);
                }
            }
            foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
            {
                switch (nextDircectory.Name.ToLower())
                {
                    case "autocser":// AutoCSer2(nextDircectory, githubPath); break;
                    case ".vs":
                    case "application":
                    case "example":
                    case "testcase":
                    case "document":
                        copy(nextDircectory, nextDircectory.Name + @"\", githubPath, true);
                        break;
                }
            }
        }
        ///// <summary>
        ///// AutoCSer2 项目目录处理
        ///// </summary>
        ///// <param name="directory"></param>
        //private static void AutoCSer2(DirectoryInfo directory, string[] githubPath)
        //{
        //    string path = directory.Name + @"\";
        //    githubPath = checkGithubPath(directory, githubPath);
        //    foreach (FileInfo file in directory.GetFiles())
        //    {
        //        if (file.Name[0] == '%')
        //        {
        //            file.Attributes = 0;
        //            file.Delete();
        //        }
        //    }
        //    foreach (DirectoryInfo nextDircectory in directory.GetDirectories())
        //    {
        //        if (string.Compare(nextDircectory.Name, "bin", true) == 0 || nextDircectory.GetFiles("*.csproj").Length != 0)
        //        {
        //            copy(nextDircectory, path + nextDircectory.Name + @"\", githubPath, true);
        //        }
        //    }
        //}
    }
}

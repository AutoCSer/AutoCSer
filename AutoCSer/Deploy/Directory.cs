using System;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 目录信息
    /// </summary>
    [AutoCSer.BinarySerialize.Serialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct Directory
    {
        /// <summary>
        /// 目录名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 目录信息集合
        /// </summary>
        public Directory[] Directorys;
        /// <summary>
        /// 文件最后修改时间集合
        /// </summary>
        public FileTime[] Files;
        /// <summary>
        /// 服务器端比较文件最后修改时间
        /// </summary>
        /// <param name="directoryInfo"></param>
        internal void Different(DirectoryInfo directoryInfo)
        {
            string directoryName = directoryInfo.fullName();
            if (Directorys != null)
            {
                int directoryCount = 0;
                foreach (Directory directory in Directorys)
                {
                    DirectoryInfo nextDirectoryInfo = new DirectoryInfo(directoryName + directory.Name);
                    if (nextDirectoryInfo.Exists)
                    {
                        directory.Different(nextDirectoryInfo);
                        if (directory.Name != null) Directorys[directoryCount++] = directory;
                    }
                    else Directorys[directoryCount++] = directory;
                }
                if (directoryCount == 0) Directorys = null;
                else if (directoryCount != Directorys.Length) Array.Resize(ref Directorys, directoryCount);
            }
            if (Files != null)
            {
                int fileCount = 0;
                foreach (FileTime fileTime in Files)
                {
                    FileInfo file = new FileInfo(directoryName + fileTime.FileName);
                    if (file.Exists)
                    {
                        if (file.LastWriteTimeUtc != fileTime.LastWriteTimeUtc) Files[fileCount++].Set(file.LastWriteTimeUtc, fileTime.FileName);
                    }
                    else Files[fileCount++].Set(fileTime.FileName);
                }
                if (fileCount == 0) Files = null;
                else if (fileCount != Files.Length) Array.Resize(ref Files, fileCount);
            }
            if (Files == null && Directorys == null) Name = null;
        }
        /// <summary>
        /// 加载文件数据
        /// </summary>
        /// <param name="directoryInfo"></param>
        internal void Load(DirectoryInfo directoryInfo)
        {
            string directoryName = directoryInfo.fullName();
            if (Directorys != null)
            {
                for (int index = Directorys.Length; index != 0; Directorys[--index].load(directoryName)) ;
            }
            if (Files != null)
            {
                for (int index = Files.Length; index != 0; Files[--index].Load(directoryName)) ;
            }
        }
        /// <summary>
        /// 加载文件数据
        /// </summary>
        /// <param name="path"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void load(string path)
        {
            Load(new DirectoryInfo(path + Name));
        }
        /// <summary>
        /// 服务器端部署
        /// </summary>
        /// <param name="serverDirectory"></param>
        /// <param name="bakDirectory"></param>
        internal void Deploy(DirectoryInfo serverDirectory, DirectoryInfo bakDirectory)
        {
            string serverDirectoryName = serverDirectory.fullName(), bakDirectoryName = bakDirectory.fullName();
            if (Directorys != null)
            {
                foreach (Directory directory in Directorys)
                {
                    DirectoryInfo nextServerDirectory = new DirectoryInfo(serverDirectoryName + directory.Name), nextBakDirectory = new DirectoryInfo(bakDirectoryName + directory.Name);
                    if (!nextServerDirectory.Exists) nextServerDirectory.Create();
                    if (!nextBakDirectory.Exists) nextBakDirectory.Create();
                    directory.Deploy(nextServerDirectory, nextBakDirectory);
                }
            }
            if (Files != null)
            {
                foreach (FileTime fileTime in Files)
                {
                    if (fileTime.Data != null)
                    {
                        string fileName = serverDirectoryName + fileTime.FileName;
                        FileInfo file = new FileInfo(fileName);
                        if (file.Exists)
                        {
                            string newFileName = serverDirectoryName + "%" + fileTime.FileName;
                            using (FileStream fileStream = new FileStream(newFileName, FileMode.Create)) fileStream.Write(fileTime.Data, 0, fileTime.Data.Length);
                            new FileInfo(newFileName).LastWriteTimeUtc = fileTime.LastWriteTimeUtc;
                            string bakFilaName = bakDirectoryName + fileTime.FileName;
                            if (File.Exists(bakFilaName))
                            {
                                Console.WriteLine("文件已存在 " + bakFilaName);
                                File.Delete(bakFilaName);
                            }
                            File.Move(fileName, bakFilaName);
                            File.Move(newFileName, fileName);
                        }
                        else
                        {
                            using (FileStream fileStream = file.Create()) fileStream.Write(fileTime.Data, 0, fileTime.Data.Length);
                            new FileInfo(fileName).LastWriteTimeUtc = fileTime.LastWriteTimeUtc;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 客户端创建目录信息
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="lastWriteTime"></param>
        /// <param name="searchPatterns"></param>
        /// <returns></returns>
        internal static Directory Create(DirectoryInfo directoryInfo, DateTime lastWriteTime, string[] searchPatterns)
        {
            Directory directory = create(directoryInfo, lastWriteTime, searchPatterns);
            if (directory.Files != null && directory.Files.Length == 0) directory.Files = null;
            if (directory.Directorys != null && directory.Directorys.Length == 0) directory.Directorys = null;
            directory.Name = string.Empty;
            return directory;
        }
        /// <summary>
        /// 客户端创建目录信息
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="lastWriteTime"></param>
        /// <param name="searchPatterns"></param>
        /// <returns></returns>
        private static Directory create(DirectoryInfo directoryInfo, DateTime lastWriteTime, string[] searchPatterns)
        {
            Directory directory = new Directory();
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            if (directoryInfos.Length != 0)
            {
                LeftArray<Directory> directorys = new LeftArray<Directory>(directoryInfos.Length);
                foreach (DirectoryInfo nextDirectoryInfo in directoryInfos)
                {
                    Directory nextDirectory = create(nextDirectoryInfo, lastWriteTime, searchPatterns);
                    if (nextDirectory.Name != null) directorys.Add(nextDirectory);
                }
                if (directorys.Length != 0) directory.Directorys = directorys.ToArray();
            }
            if (searchPatterns == null)
            {
                FileInfo[] files = directoryInfo.GetFiles();
                if (files.Length != 0)
                {
                    LeftArray<FileTime> fileTimes = new LeftArray<FileTime>(files.Length);
                    foreach (FileInfo file in files)
                    {
                        if (file.LastWriteTimeUtc > lastWriteTime) fileTimes.Add(new FileTime(file));
                    }
                    if (fileTimes.Length != 0) directory.Files = fileTimes.ToArray();
                }
            }
            else
            {
                LeftArray<FileTime> fileTimes = new LeftArray<FileTime>();
                foreach (string searchPattern in searchPatterns)
                {
                    foreach (FileInfo file in directoryInfo.GetFiles(searchPattern))
                    {
                        if (file.LastWriteTimeUtc > lastWriteTime) fileTimes.Add(new FileTime(file));
                    }
                }
                if (fileTimes.Length != 0) directory.Files = fileTimes.ToArray();
            }
            if (directory.Files != null || directory.Directorys != null) directory.Name = directoryInfo.Name;
            return directory;
        }
        /// <summary>
        /// 客户端创建目录信息
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="lastWriteTime"></param>
        /// <returns></returns>
        internal static Directory CreateWeb(DirectoryInfo directory, DateTime lastWriteTime)
        {
            Directory cssDirectory = page(directory, true, "css", null, lastWriteTime), jsDirectory = js(directory, true, lastWriteTime), loadJsDirectory = loadJs(directory, lastWriteTime), htmlDirectory = page(directory, true, "html", "page.html", lastWriteTime), imageDirectory = image(directory, lastWriteTime);
            cssDirectory.Files = AutoCSer.Extension.ArrayExtension.concat(cssDirectory.Files, jsDirectory.Files, htmlDirectory.Files, imageDirectory.Files);
            if (cssDirectory.Files.Length == 0) cssDirectory.Files = null;
            cssDirectory.Directorys = AutoCSer.Extension.ArrayExtension.concat(cssDirectory.Directorys, jsDirectory.Directorys, loadJsDirectory.Directorys, htmlDirectory.Directorys, imageDirectory.Directorys);
            if (cssDirectory.Directorys.Length == 0) cssDirectory.Directorys = null;
            cssDirectory.Name = string.Empty;
            return cssDirectory;
        }
        /// <summary>
        /// 创建文件目录信息
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="isBoot"></param>
        /// <param name="extension"></param>
        /// <param name="pageExtension"></param>
        /// <param name="lastWriteTime"></param>
        /// <returns></returns>
        private static Directory page(DirectoryInfo directoryInfo, bool isBoot, string extension, string pageExtension, DateTime lastWriteTime)
        {
            Directory directory = new Directory();
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            if (directoryInfos.Length != 0)
            {
                LeftArray<Directory> directorys = new LeftArray<Directory>(directoryInfos.Length);
                foreach (DirectoryInfo nextDirectoryInfo in directoryInfos)
                {
                    if (!isBoot || string.Compare(nextDirectoryInfo.Name, "js", true) != 0)
                    {
                        Directory nextDirectory = page(nextDirectoryInfo, false, extension, pageExtension, lastWriteTime);
                        if (nextDirectory.Name != null) directorys.Add(nextDirectory);
                    }
                }
                if (directorys.Length != 0) directory.Directorys = directorys.ToArray();
            }
            FileInfo[] files = directoryInfo.GetFiles("*." + extension);
            if (files.Length != 0)
            {
                LeftArray<FileTime> fileTimes = new LeftArray<FileTime>(files.Length);
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTimeUtc > lastWriteTime && (pageExtension == null || !file.Name.EndsWith(pageExtension, StringComparison.Ordinal))) fileTimes.Add(new FileTime(file));
                }
                if (fileTimes.Length != 0) directory.Files = fileTimes.ToArray();
            }
            if (directory.Files != null || directory.Directorys != null) directory.Name = directoryInfo.Name;
            return directory;
        }
        /// <summary>
        /// 创建JS脚本文件目录信息
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="isBoot"></param>
        /// <param name="lastWriteTime"></param>
        /// <returns></returns>
        private static Directory js(DirectoryInfo directoryInfo, bool isBoot, DateTime lastWriteTime)
        {
            Directory directory = new Directory();
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            if (directoryInfos.Length != 0)
            {
                LeftArray<Directory> directorys = new LeftArray<Directory>(directoryInfos.Length);
                foreach (DirectoryInfo nextDirectoryInfo in directoryInfos)
                {
                    if (!isBoot || string.Compare(nextDirectoryInfo.Name, "js", true) != 0)
                    {
                        Directory nextDirectory = js(nextDirectoryInfo, false, lastWriteTime);
                        if (nextDirectory.Name != null) directorys.Add(nextDirectory);
                    }
                }
                if (directorys.Length != 0) directory.Directorys = directorys.ToArray();
            }
            FileInfo[] files = directoryInfo.GetFiles("*.js");
            if (files.Length != 0)
            {
                LeftArray<FileTime> fileTimes = new LeftArray<FileTime>(files.Length);
                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTimeUtc > lastWriteTime && !file.Name.EndsWith(".page.js", StringComparison.Ordinal)) fileTimes.Add(new FileTime(file));
                }
                if (fileTimes.Length != 0) directory.Files = fileTimes.ToArray();
            }
            if (directory.Files != null || directory.Directorys != null) directory.Name = directoryInfo.Name;
            return directory;
        }
        /// <summary>
        /// 创建JS脚本文件类库目录信息
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="lastWriteTime"></param>
        /// <returns></returns>
        private static Directory loadJs(DirectoryInfo directoryInfo, DateTime lastWriteTime)
        {
            DirectoryInfo jsDirectory = new DirectoryInfo(directoryInfo.fullName() + "js");
            Directory directory = new Directory();
            if (jsDirectory.Exists)
            {
                DirectoryInfo[] directoryInfos = jsDirectory.GetDirectories();
                if (directoryInfos.Length != 0)
                {
                    LeftArray<Directory> directorys = new LeftArray<Directory>(directoryInfos.Length);
                    foreach (DirectoryInfo nextDirectoryInfo in directoryInfos)
                    {
                        Directory nextDirectory;
                        switch (nextDirectoryInfo.Name)
                        {
                            case "ace": nextDirectory = js(nextDirectoryInfo, lastWriteTime, new string[] { "ace.js" }); break;
                            case "mathJax": nextDirectory = js(nextDirectoryInfo, lastWriteTime, new string[] { "MathJax.js" }); break;
                            case "highcharts": nextDirectory = js(nextDirectoryInfo, false, lastWriteTime); break;
                            default: AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, "未知的js文件夹 " + nextDirectoryInfo.fullName()); nextDirectory = new Directory(); break;
                        }
                        if (nextDirectory.Name != null) directorys.Add(nextDirectory);
                    }
                    if (directorys.Length != 0) directory.Directorys = directorys.ToArray();
                }
                FileInfo[] files = jsDirectory.GetFiles("*.js");
                if (files.Length != 0)
                {
                    LeftArray<FileTime> fileTimes = new LeftArray<FileTime>(files.Length);
                    FileTime loadFileTime = new FileTime(), loadPageFileTime = new FileTime();
                    foreach (FileInfo file in files)
                    {
                        if (file.LastWriteTimeUtc > lastWriteTime)
                        {
                            if (file.Name == "load.js") loadFileTime = new FileTime(file);
                            else if (file.Name == "loadPage.js") loadPageFileTime = new FileTime(file);
                            else fileTimes.Add(new FileTime(file));
                        }
                    }
                    if (loadFileTime.FileName != null) fileTimes.Add(loadFileTime);
                    if (loadPageFileTime.FileName != null) fileTimes.Add(loadPageFileTime);
                    if (fileTimes.Length != 0) directory.Files = fileTimes.ToArray();
                }
                if (directory.Files != null || directory.Directorys != null)
                {
                    directory.Name = jsDirectory.Name;
                    directory = new Directory { Name = directoryInfo.Name, Directorys = new Directory[] { directory } };
                }
            }
            return directory;
        }
        /// <summary>
        /// 创建JS脚本文件目录信息
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="lastWriteTime"></param>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        private static Directory js(DirectoryInfo directoryInfo, DateTime lastWriteTime, string[] fileNames)
        {
            Directory directory = new Directory();
            LeftArray<FileTime> fileTimes = new LeftArray<FileTime>(fileNames.Length);
            string path = directoryInfo.fullName();
            foreach (string fileName in fileNames)
            {
                FileInfo file = new FileInfo(path + fileName);
                if (file.Exists && file.LastWriteTimeUtc > lastWriteTime) fileTimes.Add(new FileTime(file));
            }
            if (fileTimes.Length != 0)
            {
                directory.Files = fileTimes.ToArray();
                directory.Name = directoryInfo.Name;
            }
            return directory;
        }
        /// <summary>
        /// 图片目录信息
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="lastWriteTime"></param>
        /// <returns></returns>
        private static Directory image(DirectoryInfo directoryInfo, DateTime lastWriteTime)
        {
            DirectoryInfo imageDirectory = new DirectoryInfo(directoryInfo.fullName() + "images");
            Directory directory = new Directory();
            if (imageDirectory.Exists)
            {
                directory = page(imageDirectory, false, "*.*", null, lastWriteTime);
                if (directory.Name != null) directory = new Directory { Name = directoryInfo.Name, Directorys = new Directory[] { directory } };
            }
            return directory;
        }
    }
}

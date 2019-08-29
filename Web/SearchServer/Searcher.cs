using System;
using System.IO;
using System.IO.Compression;
using AutoCSer.Extension;
using AutoCSer.Threading;

namespace AutoCSer.Web.SearchServer
{
    /// <summary>
    /// 搜索
    /// </summary>
    internal static class Searcher
    {
        /// <summary>
        /// Trie 图
        /// </summary>
        private static readonly AutoCSer.Search.StaticStringTrieGraph staticTrieGraph;
        /// <summary>
        /// 搜索器
        /// </summary>
        internal static readonly AutoCSer.Search.StaticSearcher<DataKey> Default;
        /// <summary>
        /// 搜索器线程参数
        /// </summary>
        internal static readonly ThreadParameter DefaultThreadParameter;
        /// <summary>
        /// 搜索处理队列
        /// </summary>
        internal static readonly TaskQueue SearchTaskQueue = new TaskQueue();
        unsafe static Searcher()
        {
            if (File.Exists(AutoCSer.Web.Config.Search.WordFileName))
            {
                byte[] data = new byte[4 << 10];
                using (FileStream fileStream = new FileStream(AutoCSer.Web.Config.Search.WordFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (DeflateStream compressStream = new DeflateStream(fileStream, CompressionMode.Decompress, false))
                using (UnmanagedStream writeStream = new UnmanagedStream(4 << 10))
                {
                    do
                    {
                        int length = compressStream.Read(data, 0, 4 << 10);
                        if (length == 0)
                        {
                            data = writeStream.GetArray();
                            break;
                        }
                        writeStream.Write(new SubArray<byte>(data, 0, length));
                    }
                    while (true);

                }
                string[] words = null;
                fixed (byte* dataFixed = data)
                {
                    words = new string[*(int*)dataFixed];
                    int index = 0, length;
                    for (char* start = (char*)(dataFixed + sizeof(int)), read = start; index != words.Length;)
                    {
                        while (*(short*)read != 0) ++read;
                        AutoCSer.Search.Simplified.Format(start, length = (int)(read - start));
                        words[index++] = new string(start, 0, length);
                        start = ++read;
                    }
                }
                Console.WriteLine("Word Count " + words.Length.toString());
                using (AutoCSer.Search.StringTrieGraph trieGraph = new AutoCSer.Search.StringTrieGraph(words))
                {
                    staticTrieGraph = trieGraph.CreateStaticGraph(false);
                }
            }
            else Console.WriteLine("未找到文件 " + AutoCSer.Web.Config.Search.WordFileName);
            Default = new AutoCSer.Search.StaticSearcher<DataKey>(staticTrieGraph);
            DefaultThreadParameter = new ThreadParameter(Default);
        }
    }
}

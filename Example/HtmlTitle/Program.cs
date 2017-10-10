using System;
using System.Threading;

namespace AutoCSer.Example.HtmlTitle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/Index.html
");
            using (AutoCSer.Net.HtmlTitle.HttpTask task = new AutoCSer.Net.HtmlTitle.HttpTask(4, 15, SubBuffer.Size.Kilobyte16))
            {
#if DotNetStandard
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
                UrlTitle.Crawl(task, "http://www.AutoCSer.com/");

                UrlTitle.Crawl(task, "http://www.51nod.com/");
                UrlTitle.Crawl(task, "http://www.cnblogs.com/asxinyu/");
                UrlTitle.Crawl(task, "http://www.baidu.com/");
                UrlTitle.Crawl(task, "http://www.163.com/");
                UrlTitle.Crawl(task, "http://mail.163.com/");
                UrlTitle.Crawl(task, "http://www.qq.com/");
                UrlTitle.Crawl(task, "http://www.zhihu.com/");
                UrlTitle.Crawl(task, "http://www.cnblogs.com/");
                UrlTitle.Crawl(task, "http://www.csdn.net/");
                UrlTitle.Crawl(task, "http://bbs.csdn.net/");
                UrlTitle.Crawl(task, "http://www.csharpkit.com/");

                UrlTitle.Crawl(task, "https://www.51nod.com/");
                UrlTitle.Crawl(task, "https://www.163.com/");
                UrlTitle.Crawl(task, "https://mail.163.com/");
                UrlTitle.Crawl(task, "https://www.zhihu.com/");
                UrlTitle.Crawl(task, "https://www.cnblogs.com/");

                Console.WriteLine(@"Press quit to exit.");
                Console.WriteLine(@"Press url to crawl title.");
                do
                {
                    string url = Console.ReadLine();
                    if (url == "quit") break;
                    UrlTitle.Crawl(task, url);
                }
                while (true);
            }
        }
    }
}

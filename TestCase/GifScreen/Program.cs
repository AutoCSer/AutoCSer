using System;
using AutoCSer.Extension;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace AutoCSer.TestCase.GifScreen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/
");
            Screen screen = Screen.PrimaryScreen;
            int width = screen.Bounds.Width, height = screen.Bounds.Height;
            FileInfo file = new FileInfo(Path.Combine(AutoCSer.PubPath.ApplicationPath, AutoCSer.IO.File.BakPrefix + ((ulong)Date.Now.Ticks).toHex() + ".gif"));
            using (FileStream fileStream = new FileStream(file.FullName, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1, FileOptions.WriteThrough))
            using (AutoCSer.Drawing.Gif.TimerWriter gif = new AutoCSer.Drawing.Gif.TimerWriter(fileStream, () =>
            {
                Bitmap bitmap = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(bitmap)) graphics.CopyFromScreen(0, 0, 0, 0, new Size(width, height));
                return bitmap;
            }, width, height))
            {
                Console.WriteLine("Copy screen start...");
                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
            }
            Process.Start(file.FullName);
        }
    }
}

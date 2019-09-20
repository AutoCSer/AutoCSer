using System;
using System.Windows.Forms;
using System.IO;
using AutoCSer.Extension;
using System.Diagnostics;
using System.Text;

namespace AutoCSer.Web.DeployClient
{
    public partial class DeployForm : Form
    {

        /// <summary>
        /// UI线程上下文
        /// </summary>
        private readonly System.Threading.SynchronizationContext context;
        /// <summary>
        /// 发布工具客户端
        /// </summary>
        private readonly AutoCSer.Deploy.Client client;

        public DeployForm()
        {
            InitializeComponent();
            context = System.Threading.SynchronizationContext.Current;

            AutoCSer.Net.TcpInternalServer.ServerAttribute serverAttribute = AutoCSer.Web.Config.Pub.GetVerifyTcpServerAttribute(typeof(AutoCSer.Deploy.Server));
            serverAttribute.Host = AutoCSer.Web.Config.Deploy.ServerIp;
            DirectoryInfo webReleaseDirectory = new DirectoryInfo(@"..\..\..\www.AutoCSer.com\bin\Release\");
            string serverWebPath = AutoCSer.Web.Config.Deploy.ServerPath + @"www.AutoCSer.com\";

            AutoCSer.Deploy.ClientTask.Task[] htmlTasks = new AutoCSer.Deploy.ClientTask.Task[]
            {
                new AutoCSer.Deploy.ClientTask.WebFile
                {
                    ClientPath = webReleaseDirectory.Parent.Parent.FullName,
                    ServerPath = serverWebPath,
                },
                new AutoCSer.Deploy.ClientTask.File
                {
                    ClientPath = webReleaseDirectory.Parent.Parent.FullName,
                    ServerPath = serverWebPath,
                    SearchPatterns = new string[] { "*.png" }
                }
            };
            AutoCSer.Deploy.ClientTask.Task[] webTasks = new AutoCSer.Deploy.ClientTask.Task[]
            {
                htmlTasks[0],
                htmlTasks[1],
                new AutoCSer.Deploy.ClientTask.Run
                {
                    ClientPath = webReleaseDirectory.FullName,
                    ServerPath = AutoCSer.Web.Config.Deploy.ServerPath + @"www.AutoCSer.com\bin\Release\",
                    FileName = "AutoCSer.Web.exe",
                }
            };
            AutoCSer.Deploy.ClientTask.Task[] httpTasks = new AutoCSer.Deploy.ClientTask.Task[]
            {
                webTasks[0],
                webTasks[1],
                webTasks[2],
                new AutoCSer.Deploy.ClientTask.AssemblyFile
                {
                    ClientPath = webReleaseDirectory.FullName,
                    ServerPath = AutoCSer.Web.Config.Deploy.ServerPath + @"www.AutoCSer.com\bin\Release\"
                },
                new AutoCSer.Deploy.ClientTask.Run
                {
                    ClientPath = new DirectoryInfo(@"..\..\..\HttpServer\bin\Release\").FullName,
                    ServerPath = AutoCSer.Web.Config.Deploy.ServerPath + @"HttpServer\bin\Release\",
                    FileName = "AutoCSer.Web.HttpServer.exe",
                }
            };
            AutoCSer.Deploy.ClientTask.Task[] searchTasks = new AutoCSer.Deploy.ClientTask.Task[]
            {
                new AutoCSer.Deploy.ClientTask.Run
                {
                    ClientPath = new DirectoryInfo(@"..\..\..\SearchServer\bin\Release\").FullName,
                    ServerPath = AutoCSer.Web.Config.Deploy.ServerPath + @"SearchServer\bin\Release\",
                    FileName = "AutoCSer.Web.SearchServer.exe",
                }
            };
            AutoCSer.Deploy.ClientTask.Task[] exampleTasks = new AutoCSer.Deploy.ClientTask.Task[]
            {
                new AutoCSer.Deploy.ClientTask.File
                {
                    ClientPath = new DirectoryInfo(@"..\..\..\www.AutoCSer.com\Download\").FullName,
                    ServerPath = AutoCSer.Web.Config.Deploy.ServerPath + @"www.AutoCSer.com\Download\",
                },
                new AutoCSer.Deploy.ClientTask.File
                {
                    ClientPath = new DirectoryInfo(@"..\..\..\..\Example\").FullName,
                    ServerPath = AutoCSer.Web.Config.Deploy.ServerPath + @"Example\",
                    SearchPatterns = new string[] { "*.cs", "*.html", "*.ts", "*.js", "*.css", "*.json" },
                },
                new AutoCSer.Deploy.ClientTask.File
                {
                    ClientPath = new DirectoryInfo(@"..\..\..\..\TestCase\").FullName,
                    ServerPath = AutoCSer.Web.Config.Deploy.ServerPath + @"TestCase\",
                    SearchPatterns = new string[] { "*.cs", "*.html", "*.ts", "*.js" },
                },
                new AutoCSer.Deploy.ClientTask.Custom
                {
                    CallName = "OnExample"
                }
            };
            AutoCSer.Deploy.ClientTask.Task[] updateDeployServerTasks = new AutoCSer.Deploy.ClientTask.Task[]
            {
                new AutoCSer.Deploy.ClientTask.File
                {
                    ClientPath = new DirectoryInfo(@"..\..\..\DeployServer\bin\Release\").FullName,
                    ServerPath = AutoCSer.Web.Config.Deploy.ServerPath +@"DeployServer\bin\Release\" + AutoCSer.Deploy.Server.DefaultUpdateDirectoryName,
                    SearchPatterns = new string[]{ "*.*" }
                },
                new AutoCSer.Deploy.ClientTask.Custom
                {
                    CallName = "OnDeployServerUpdated",
                }
            };
            AutoCSer.Deploy.ClientTask.Task[] gameWebTasks = new AutoCSer.Deploy.ClientTask.Task[]
            {
                new AutoCSer.Deploy.ClientTask.File
                {
                    ClientPath = @"C:\showjim\CardGame\CardGame\WebPage\",
                    ServerPath = serverWebPath,
                    SearchPatterns = new string[]{ "*.*" }
                }
            };
            AutoCSer.Deploy.ClientTask.Task[] gameServerTasks = new AutoCSer.Deploy.ClientTask.Task[]
            {
                new AutoCSer.Deploy.ClientTask.File
                {
                    ClientPath = @"C:\showjim\CardGame\CardGame\Data\",
                    ServerPath = AutoCSer.Web.Config.GameServer.Path,
                    SearchPatterns = new string[]{ "*.*" }
                },
                new AutoCSer.Deploy.ClientTask.File
                {
                    ClientPath = @"C:\showjim\CardGame\GameServer\bin\Release\",
                    ServerPath = AutoCSer.Web.Config.GameServer.Path + AutoCSer.Deploy.Server.DefaultUpdateDirectoryName,
                    SearchPatterns = new string[]{ "*.*" }
                },
                new AutoCSer.Deploy.ClientTask.Custom
                {
                    CallName = "OnGameServerUpdated",
                }
            };
            AutoCSer.Deploy.ClientDeploy[] deploys = new AutoCSer.Deploy.ClientDeploy[]
            {
                new AutoCSer.Deploy.ClientDeploy
                {
                    Name = "HTML",
                    ServerName = AutoCSer.Deploy.Server.ServerName,
                    Tasks = htmlTasks
                },
                new AutoCSer.Deploy.ClientDeploy
                {
                    Name = "Web",
                    ServerName = AutoCSer.Deploy.Server.ServerName,
                    Tasks = webTasks
                },
                new AutoCSer.Deploy.ClientDeploy
                {
                    Name = "Web/Http",
                    ServerName = AutoCSer.Deploy.Server.ServerName,
                    Tasks = httpTasks
                },
                new AutoCSer.Deploy.ClientDeploy
                {
                    Name = "Search",
                    ServerName = AutoCSer.Deploy.Server.ServerName,
                    Tasks = searchTasks
                },
                new AutoCSer.Deploy.ClientDeploy
                {
                    Name = "Example",
                    ServerName = AutoCSer.Deploy.Server.ServerName,
                    Tasks = exampleTasks
                },
                new AutoCSer.Deploy.ClientDeploy
                {
                    Name = "DeployServer",
                    ServerName = AutoCSer.Deploy.Server.ServerName,
                    Tasks = updateDeployServerTasks
                },
                new AutoCSer.Deploy.ClientDeploy
                {
                    Name = "GameWeb",
                    ServerName = AutoCSer.Deploy.Server.ServerName,
                    Tasks = gameWebTasks
                },
                new AutoCSer.Deploy.ClientDeploy
                {
                    Name = "GameServer",
                    ServerName = AutoCSer.Deploy.Server.ServerName,
                    Tasks = gameServerTasks
                }
            };
            AutoCSer.Deploy.ClientConfig config = new Deploy.ClientConfig
            {
                //RunFilePaths = new string[] { webReleaseDirectory.FullName },
                ServerAttributes = new KeyValue<string, Net.TcpInternalServer.ServerAttribute>[] { new KeyValue<string, Net.TcpInternalServer.ServerAttribute>(AutoCSer.Deploy.Server.ServerName, serverAttribute) },
                Deploys = deploys
            };
            client = new AutoCSer.Deploy.Client(config, context.GetPost<AutoCSer.Deploy.OnClientParameter>(onClient));
            //do
            //{
            //    int index = 0;
            //    foreach (AutoCSer.Deploy.ClientDeploy deployInfo in deploys)
            //    {
            //        Console.ForegroundColor = (index & 1) == 0 ? ConsoleColor.Red : ConsoleColor.White;
            //        Console.WriteLine((index++).toString() + " -> " + toString(deployInfo));
            //    }
            //    Console.ForegroundColor = (index & 1) == 0 ? ConsoleColor.Red : ConsoleColor.White;
            //    Console.WriteLine((index++).toString() + " -> create Open Example");
            //    Console.ForegroundColor = (index & 1) == 0 ? ConsoleColor.Red : ConsoleColor.White;
            //    Console.WriteLine((index++).toString() + " -> create AutoCSer.zip");
            //    Console.ForegroundColor = (index & 1) == 0 ? ConsoleColor.Red : ConsoleColor.White;
            //    Console.WriteLine((index++).toString() + " -> Push Nuget");

            //    Console.ResetColor();
            //    Console.WriteLine("press quit to exit.");
            //    string command = Console.ReadLine();
            //    if (command == "quit") return;
            //    if (int.TryParse(command, out index))
            //    {
            //        if ((uint)index < (uint)deploys.Length)
            //        {
            //            try
            //            {
            //                Console.WriteLine("正在启动部署 " + toString(deploys[index]));
            //                AutoCSer.Deploy.DeployResult result = client.Deploy(index);
            //                if (result.State == AutoCSer.Deploy.DeployState.Success) Console.WriteLine("部署启动完毕 " + toString(deploys[index]) + " => " + result.Index.toString());
            //                else Console.WriteLine("部署启动失败 [" + result.State.ToString() + "] " + toString(deploys[index]));
            //            }
            //            catch (Exception error)
            //            {
            //                Console.WriteLine(error.ToString());
            //            }
            //        }
            //        else
            //        {
            //            switch (index - deploys.Length)
            //            {
            //                case 0: openExample(); break;
            //                case 1: openProcess(@"..\..\..\..\Web\Pack\bin\Release\AutoCSer.Web.Pack.exe"); break;
            //                case 2: pushNuget(); break;
            //                default: index = -1; break;
            //            }
            //        }
            //    }
            //    else index = -1;
            //    if (index == -1) Console.WriteLine("Error Command");
            //    Console.WriteLine();
            //}
            //while (true);
        }
        /// <summary>
        /// 发布客户端套接字事件
        /// </summary>
        /// <param name="ServerName"></param>
        private void onClient(AutoCSer.Deploy.OnClientParameter parameter)
        {
            switch (parameter.Type)
            {
                case AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SetSocket:
                    context.Post(onClient, true);
                    return;
                case AutoCSer.Net.TcpServer.ClientSocketEventParameter.EventType.SocketDisposed: 
                    context.Post(onClient, false);
                    return;
            }
        }
        /// <summary>
        /// 发布客户端套接字事件
        /// </summary>
        /// <param name="isClient"></param>
        private void onClient(bool isClient)
        {
            Enabled = isClient;
        }

        /// <summary>
        /// 清除消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearMessageButton_Click(object sender, EventArgs e)
        {
            MessageTextBox.Clear();
        }
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="message"></param>
        private void appendMessage(string message)
        {
            if (MessageTextBox.Text.Length != 0) MessageTextBox.AppendText("\r\n");
            MessageTextBox.AppendText(message);
        }
        /// <summary>
        /// HTML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deploy(object sender, EventArgs e)
        {
            string name = ((Button)sender).Text;
            if (MessageBox.Show("是否确定发布 " + name + " ？", "发布", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
            {
                foreach (int index in client.GetDeployIndex(name))
                {
                    AutoCSer.Deploy.DeployResult result = client.Deploy(index);
                    if (result.State == AutoCSer.Deploy.DeployState.Success) appendMessage(name + " 部署成功 " + result.Index.toString());
                    else appendMessage(name + " 部署失败 " + result.State.ToString());
                }
            }
        }

        /// <summary>
        /// 测试用例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openExampleButton_Click(object sender, EventArgs e)
        {
            openDirectory(@"..\..\..\..\TestCase\TcpServerPerformance\bin\Release\");
            openDirectory(@"..\..\..\..\TestCase\TcpSimpleServerPerformance\bin\Release\");
            openDirectory(@"..\..\..\..\TestCase\WebPerformance\bin\Release\");

            openProcess(@"..\..\..\..\Example\BinarySerialize\bin\Release\AutoCSer.Example.BinarySerialize.exe");
            openProcess(@"..\..\..\..\Example\Json\bin\Release\AutoCSer.Example.Json.exe");
            openProcess(@"..\..\..\..\Example\Xml\bin\Release\AutoCSer.Example.Xml.exe");

            openProcess(@"..\..\..\..\Example\TcpInterfaceOpenServer\bin\Release\AutoCSer.Example.TcpInterfaceOpenServer.exe");
            openProcess(@"..\..\..\..\Example\TcpInterfaceServer\bin\Release\AutoCSer.Example.TcpInterfaceServer.exe");
            openProcess(@"..\..\..\..\Example\TcpInternalServer\bin\Release\AutoCSer.Example.TcpInternalServer.exe");
            openProcess(@"..\..\..\..\Example\TcpOpenServer\bin\Release\AutoCSer.Example.TcpOpenServer.exe");
            openProcess(@"..\..\..\..\Example\TcpStaticServer\bin\Release\AutoCSer.Example.TcpStaticServer.exe");
            openProcess(@"..\..\..\..\Example\TcpRegisterServer\bin\Release\AutoCSer.Example.TcpRegisterServer.exe");

            openProcess(@"..\..\..\..\Example\TcpInterfaceOpenSimpleServer\bin\Release\AutoCSer.Example.TcpInterfaceOpenSimpleServer.exe");
            openProcess(@"..\..\..\..\Example\TcpInterfaceSimpleServer\bin\Release\AutoCSer.Example.TcpInterfaceSimpleServer.exe");
            openProcess(@"..\..\..\..\Example\TcpInternalSimpleServer\bin\Release\AutoCSer.Example.TcpInternalSimpleServer.exe");
            openProcess(@"..\..\..\..\Example\TcpOpenSimpleServer\bin\Release\AutoCSer.Example.TcpOpenSimpleServer.exe");
            openProcess(@"..\..\..\..\Example\TcpStaticSimpleServer\bin\Release\AutoCSer.Example.TcpStaticSimpleServer.exe");

            openProcess(@"..\..\..\..\Example\WebView\bin\Release\AutoCSer.Example.WebView.exe");

            openProcess(@"..\..\..\..\TestCase\TestCase\bin\Release\AutoCSer.TestCase.exe");

            //openProcess(@"..\..\..\..\TestCase\TcpServerPerformance\bin\Release\AutoCSer.TestCase.TcpInternalServerPerformance.Emit.exe");
            //openProcess(@"..\..\..\..\TestCase\TcpServerPerformance\bin\Release\AutoCSer.TestCase.TcpInternalServerPerformance.exe");
            //openProcess(@"..\..\..\..\TestCase\TcpServerPerformance\bin\Release\AutoCSer.TestCase.TcpOpenServerPerformance.Emit.exe");
            //openProcess(@"..\..\..\..\TestCase\TcpServerPerformance\bin\Release\AutoCSer.TestCase.TcpOpenServerPerformance.exe");

            //openProcess(@"..\..\..\..\TestCase\WebPerformance\bin\Release\AutoCSer.TestCase.HttpFilePerformance.exe");
            //openProcess(@"..\..\..\..\TestCase\WebPerformance\bin\Release\AutoCSer.TestCase.WebPerformance.exe");
        }

        /// <summary>
        /// AutoCSer.zip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCSerZipButton_Click(object sender, EventArgs e)
        {
            openProcess(@"..\..\..\..\Web\Pack\bin\Release\AutoCSer.Web.Pack.exe");
        }
        /// <summary>
        /// 获取 Nuget 版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getNugetVersionButton_Click(object sender, EventArgs e)
        {
            Nuget.Metadata metadata = Nuget.Project.Projects[0].Metadata;
            if (metadata != null) appendMessage(metadata.id + " " + metadata.version);
        }
        /// <summary>
        /// 重算 Nuget 版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setNugetVersionButton_Click(object sender, EventArgs e)
        {
            Nuget.Metadata metadata = Nuget.Project.Projects[0].Metadata;
            if (metadata != null)
            {
                int index = metadata.version.LastIndexOf('.') + 1;
                string newVersion = metadata.version.Substring(0, index) + (int.Parse(metadata.version.Substring(index)) + 1).toString();
                foreach (Nuget.Project project in Nuget.Project.Projects)
                {
                    File.WriteAllText(project.MetadataFile, Nuget.Metadata.VersionRegex.Replace(File.ReadAllText(project.MetadataFile, Encoding.UTF8), @"<version>" + newVersion + @"</version>"), Encoding.UTF8);
                    metadata = project.Metadata;
                    appendMessage(metadata.id + " 更新版本 " + metadata.version);
                }
            }
        }
        /// <summary>
        /// Nuget 打包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nugetPackButton_Click(object sender, EventArgs e)
        {
            //https://docs.microsoft.com/zh-cn/nuget/reference/cli-reference/cli-ref-pack
            //nuget spec
            foreach (Nuget.Project project in Nuget.Project.Projects)
            {
                Nuget.Metadata metadata = project.Metadata;
                if (metadata != null)
                {
                    deleteDirectory(project.Path + @"bin");
                    deleteDirectory(project.Path + @"obj");
                    deleteDirectory(project.Path + @".vs");

                    FileInfo packFile = new FileInfo(project.PackageFile);
                    if (packFile.Exists) packFile.Delete();

                    string arguments = "pack " + project.File + " -Build -Version " + metadata.version + @" -Properties Configuration=Release";
                    string output = waitProcessDirectory(AutoCSer.Web.Config.Pub.NugetFile, arguments);
                    if (new FileInfo(packFile.FullName).Exists)
                    {
                        arguments = "push " + packFile.FullName + " " + AutoCSer.Web.Config.Pub.NugetKey + " -Source https://api.nuget.org/v3/index.json";
                        output = waitProcessDirectory(AutoCSer.Web.Config.Pub.NugetFile, arguments);
                        if (output.IndexOf("Your package was pushed.") >= 0) appendMessage(packFile.FullName + " push 成功");
                        else
                        {
                            appendMessage(packFile.FullName + @" push 失败
" + output);
                        }
                    }
                    else
                    {
                        appendMessage(metadata.id + " Nuget 打包失败");
                        if (!string.IsNullOrEmpty(output)) appendMessage(output);
                    }
                }
                else appendMessage("配置文件 " + project.MetadataFile + " 读取失败");
            }
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        private void deleteDirectory(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            while (directory.Exists)
            {
                try
                {
                    directory.Delete(true);
                    return;
                }
                catch (Exception error)
                {
                    MessageBox.Show("目录 " + path + " 删除失败，请关闭相关程序以后重试");
                }
            }
        }
        /// <summary>
        /// 发布 .NET Standard Nuget 包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nugetStandardPushButton_Click(object sender, EventArgs e)
        {
            foreach (Project.Project project in Project.Project.Projects)
            {
                Project.PropertyGroup propertyGroup = project.PropertyGroup;
                if (propertyGroup != null)
                {
                    FileInfo packFile = new FileInfo(project.PackagePath + propertyGroup.PackageId + "." + propertyGroup.Version + ".nupkg");
                    if (packFile.Exists)
                    {
                        string arguments = " nuget push " + packFile.FullName + " -k " + AutoCSer.Web.Config.Pub.NugetKey + " -s https://api.nuget.org/v3/index.json";
                        string output = waitProcessDirectory(AutoCSer.Web.Config.Pub.DotnetExeFile, arguments);
                        appendMessage(output);
                    }
                    else appendMessage("没有找到 Nuget 包文件 " + packFile.FullName);
                }
                else appendMessage("项目文件 " + project.File + " 读取失败");
            }
        }

        /// <summary>
        /// 在文件当前目录启动进程并等待结束
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="arguments">执行参数</param>
        /// <returns>是否成功</returns>
        private static string waitProcessDirectory(FileInfo file, string arguments = null)
        {
            using (Process process = new Process())
            {
                ProcessStartInfo info = new ProcessStartInfo(file.FullName, arguments);
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                info.WorkingDirectory = file.DirectoryName;
                info.RedirectStandardOutput = true;
                process.StartInfo = info;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output;
            }
        }
        /// <summary>
        /// 打开进程文件
        /// </summary>
        /// <param name="file"></param>
        private static void openProcess(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Exists)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(fileInfo.FullName);
                startInfo.WorkingDirectory = fileInfo.DirectoryName;
                Process.Start(startInfo);
            }
            else Console.WriteLine("未找到 " + fileInfo.FullName);
        }
        /// <summary>
        /// 打开测试目录
        /// </summary>
        /// <param name="path"></param>
        private static void openDirectory(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists) Process.Start(directory.FullName);
            else Console.WriteLine("未找到 " + directory.FullName);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="deploy">客户端部署信息</param>
        ///// <returns></returns>
        //private static string toString(AutoCSer.Deploy.ClientDeploy deploy)
        //{
        //    return deploy.ServerName + " + " + deploy.Name;
        //}
    }
}

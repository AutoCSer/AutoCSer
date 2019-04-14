using System;
using System.Threading;
using System.IO;
using AutoCSer.Extension;
using System.Diagnostics;

namespace AutoCSer.Web.DeployClient
{
    class Program
    {
        static void Main(string[] args)
        {
            bool createdProcessWait;
            EventWaitHandle processWait = new EventWaitHandle(false, EventResetMode.ManualReset, "AutoCSer.Web.DeployClient", out createdProcessWait);
            if (createdProcessWait)
            {
                using (processWait)
                {
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
                            SearchPatterns = new string[] { "*.png" },
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
                    AutoCSer.Deploy.Client client = new AutoCSer.Deploy.Client(config, serverName => Console.WriteLine(serverName + " Ready."));
                    do
                    {
                        int index = 0;
                        foreach (AutoCSer.Deploy.ClientDeploy deployInfo in deploys)
                        {
                            Console.ForegroundColor = (index & 1) == 0 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.WriteLine((index++).toString() + " -> " + toString(deployInfo));
                        }
                        Console.ForegroundColor = (index & 1) == 0 ? ConsoleColor.Red : ConsoleColor.White;
                        Console.WriteLine((index++).toString() + " -> create Open Example");
                        Console.ForegroundColor = (index & 1) == 0 ? ConsoleColor.Red : ConsoleColor.White;
                        Console.WriteLine((index++).toString() + " -> create AutoCSer.zip");

                        Console.ResetColor();
                        Console.WriteLine("press quit to exit.");
                        string command = Console.ReadLine();
                        if (command == "quit") return;
                        if (int.TryParse(command, out index))
                        {
                            if ((uint)index < (uint)deploys.Length)
                            {
                                try
                                {
                                    Console.WriteLine("正在启动部署 " + toString(deploys[index]));
                                    AutoCSer.Deploy.DeployResult result = client.Deploy(index);
                                    if (result.State == AutoCSer.Deploy.DeployState.Success) Console.WriteLine("部署启动完毕 " + toString(deploys[index]) + " => " + result.Index.toString());
                                    else Console.WriteLine("部署启动失败 [" + result.State.ToString() + "] " + toString(deploys[index]));
                                }
                                catch (Exception error)
                                {
                                    Console.WriteLine(error.ToString());
                                }
                            }
                            else if (index == deploys.Length)
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
                            else if (index == deploys.Length + 1)
                            {
                                openProcess(@"..\..\..\..\Web\Pack\bin\Release\AutoCSer.Web.Pack.exe");
                            }
                            else index = -1;
                        }
                        else index = -1;
                        if (index == -1) Console.WriteLine("Error Command");
                        Console.WriteLine();
                    }
                    while (true);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deploy">客户端部署信息</param>
        /// <returns></returns>
        private static string toString(AutoCSer.Deploy.ClientDeploy deploy)
        {
            return deploy.ServerName + " + " + deploy.Name;
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
    }
}

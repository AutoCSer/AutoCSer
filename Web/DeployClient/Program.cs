using System;
using System.Windows.Forms;

namespace AutoCSer.Web.DeployClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 2: AutoCSer.Deploy.AssemblyEnvironment.CheckClient.Check(args); return;
                default:
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new DeployForm());
                    return;
            }
        }
    }
}

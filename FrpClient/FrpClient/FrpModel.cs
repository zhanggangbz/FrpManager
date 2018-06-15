using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrpClient
{
    class FrpModel
    {
        public FrpSettingModel Config { get; set; }

        public Process RunProcess { get; set; }

        public ListViewItem ListItem { get; set; }

        public bool IsRun { get; set; }

        public FrpModel(FrpSettingModel _config)
        {
            Config = _config;

            ListItem = null;

            IsRun = false;
        }

        public void Run()
        {
            RunProcess = new System.Diagnostics.Process();
            RunProcess.StartInfo.UseShellExecute = false;
            RunProcess.StartInfo.FileName =Application.StartupPath + @"\frp\frpc.exe";
            RunProcess.StartInfo.Arguments = "-c \"" + Config.ConfigFile + "\"";
            RunProcess.StartInfo.CreateNoWindow = true;
            RunProcess.EnableRaisingEvents = true;
            RunProcess.StartInfo.WorkingDirectory =Application.StartupPath + @"\frp\";
            RunProcess.Exited += new EventHandler(exep_Exited);
            RunProcess.Start();
            SetListViewInfo("已启动");
            IsRun = true;
        }

        private void exep_Exited(object sender, EventArgs e)
        {
            SetListViewInfo("未运行");
            RunProcess = null;
            IsRun = false;
        }

        private void SetListViewInfo(String info)
        {
            if (ListItem != null && ListItem.SubItems.Count >1)
            {
                if (ListItem.ListView.InvokeRequired)
                {
                    Action<String> actionDelegate = new Action<String>(SetListViewInfo);
                    ListItem.ListView.BeginInvoke(actionDelegate, info);
                }
                else
                {
                    ListItem.SubItems[1].Text = info;
                }
            }
        }

        public void Exit()
        {
            if (RunProcess != null && RunProcess.HasExited == false)
            {
                try
                {
                    RunProcess.Kill();
                    RunProcess.Close();
                }
                catch
                {

                }
            }
            SetListViewInfo("未运行");
            IsRun = false;
            RunProcess = null;
        }
    }
}

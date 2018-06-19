using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrpClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //注册EventWaitHandle事件处理体
            ThreadPool.RegisterWaitForSingleObject(Program.ProgramStarted, OnProgramStarted, null, -1, false);  
        }

        //显示已存在的窗口
        private void OnProgramStarted(object state, bool timedOut)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        List<FrpModel> FrpList = new List<FrpModel>();

        /// <summary>
        /// 增加配置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {    
            FrpSetForm dlg = new FrpSetForm();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FrpModel tt = new FrpModel(dlg.FrpObj);
                FrpList.Add(tt);
                AddFrpToList(tt);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitAllFrp();

            foreach (FrpModel item in FrpList)
            {
                AddFrpToList(item);
                if (item.Config.AutoRun)
                {
                    item.Run();
                }
            }

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(aTimer_Elapsed);
            // 设置引发时间的时间间隔 此处设置为１秒
            aTimer.Interval = 60000;
            aTimer.Enabled = true;
        }

        private void InitAllFrp()
        {
            List<FrpSettingModel> allFrp = FrpSettingAction.GetAllFrpSetting();
            foreach (FrpSettingModel item in allFrp)
            {
                FrpList.Add(new FrpModel(item));
            }
        }

        private void AddFrpToList(FrpModel item)
        {
            ListViewItem lvi = new ListViewItem();

            lvi.Text = item.Config.Name;

            lvi.SubItems.Add("未运行");

            lvi.Tag = item;

            item.ListItem = this.listView1.Items.Add(lvi);
        }

        /// <summary>
        /// 单独启动按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem select = this.listView1.SelectedItems[0];

                FrpModel frpobj = select.Tag as FrpModel;
                if (frpobj != null)
                {
                    frpobj.Run();

                    this.button2.Enabled = !frpobj.IsRun;
                    this.button3.Enabled = frpobj.IsRun;
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (FrpModel item in FrpList)
            {
                item.Exit();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem select = this.listView1.SelectedItems[0];

                FrpModel frpobj = select.Tag as FrpModel;
                if (frpobj != null)
                {
                    this.button2.Enabled = !frpobj.IsRun;
                    this.button3.Enabled = frpobj.IsRun;
                    this.button4.Enabled = true;
                    this.button5.Enabled = true;
                    this.button6.Enabled = true;
                }
                else
                {
                    this.button2.Enabled = false;
                    this.button3.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 单独关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem select = this.listView1.SelectedItems[0];

                FrpModel frpobj = select.Tag as FrpModel;
                if (frpobj != null)
                {
                    frpobj.Exit();

                    this.button2.Enabled = !frpobj.IsRun;
                    this.button3.Enabled = frpobj.IsRun;
                }
            }
        }

        /// <summary>
        /// 打开日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem select = this.listView1.SelectedItems[0];

                FrpModel frpobj = select.Tag as FrpModel;
                if (frpobj != null)
                {
                    if (string.IsNullOrEmpty(frpobj.Config.LogFile) == false)
                    {
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                        psi.Arguments = "/e,/select," + frpobj.Config.LogFile;
                        System.Diagnostics.Process.Start(psi);
                    }
                    else
                    {
                        MessageBox.Show("未指定日志文件");
                    }
                }
            }
        }

        /// <summary>
        /// 删除配置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem select = this.listView1.SelectedItems[0];

                FrpModel frpobj = select.Tag as FrpModel;
                if (frpobj != null)
                {
                    string info = "确定要删除" + frpobj.Config.Name + "吗？删除时将关闭其当前正在运行的进程。";
                    if (MessageBox.Show(info, "警告", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        FrpSettingAction.DelFrpSettingFromConfig(frpobj.Config);
                        frpobj.Exit();
                        frpobj.ListItem = null;
                        this.listView1.Items.Remove(select);
                    }
                }
            }
        }

        /// <summary>
        /// 修改配置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem select = this.listView1.SelectedItems[0];

                FrpModel frpobj = select.Tag as FrpModel;
                if (frpobj != null)
                {
                    FrpSetForm dlg = new FrpSetForm(frpobj.Config);
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        MessageBox.Show("修改成功，当前配置对应的进程将在下次启动时使用新配置");
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true; // 取消关闭窗体 

                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                this.mainNotifyIcon.Visible = true;
                //this.m_cartoonForm.CartoonClose();
                this.Hide();
                return;
            }
        }

        private void mainNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要退出？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.mainNotifyIcon.Visible = false;
                this.Close();
                this.Dispose();
                System.Environment.Exit(System.Environment.ExitCode);
            }
        }


        private void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (isReFresh)
            {
                // 得到 hour minute second  如果等于某个值就开始执行
                int intHour = e.SignalTime.Hour;
                int intMinute = e.SignalTime.Minute;

                if (intHour == freshHour && intMinute == freshmit)
                {
                    ReStartAllFrp();
                }
            }
        }

        private void ReStartAllFrp()
        {
            foreach (FrpModel item in FrpList)
            {
                if (item.IsRun)
                {
                    try
                    {
                        item.Exit();
                        System.Threading.Thread.Sleep(5000);
                        item.Run();
                    }
                    catch
                    {

                    }
                }
            }
        }

        bool isReFresh = false;

        int freshHour = 1;
        int freshmit = 0;

        private void button7_Click(object sender, EventArgs e)
        {
            SysSet dlg = new SysSet(isReFresh, freshHour, freshmit);

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                isReFresh = dlg.AutoRefesh;
                freshHour = dlg.RefeshHour;
                freshmit = dlg.RefeshMinite;
            }
        }
    }
}

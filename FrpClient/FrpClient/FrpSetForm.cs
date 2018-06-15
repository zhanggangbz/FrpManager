using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrpClient
{
    public partial class FrpSetForm : Form
    {
        bool isAdd = false;

        FrpSettingModel frpObj = null;

        public FrpSettingModel FrpObj
        {
            get { return frpObj; }
        }

        public FrpSetForm()
        {
            InitializeComponent();

            isAdd = true;
        }

        public FrpSetForm(FrpSettingModel _obj)
        {
            InitializeComponent();

            frpObj = _obj;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frpObj.AutoRun = this.checkBox1.Checked;
            frpObj.Name = this.textBox_name.Text;
            frpObj.ConfigFile = this.textBox_cf.Text;
            frpObj.LogFile = this.textBox_lf.Text;

            if (isAdd)
            {
                FrpSettingAction.AddFrpSettingToConfig(frpObj);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                FrpSettingAction.EditFrpSettingToConfig(frpObj);
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择frp配置文件";
            dialog.Filter = "配置文件(*.ini)|*.ini";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_cf.Text = dialog.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择frp配置对应的日志文件";
            dialog.Filter = "日志文件(*.log)|*.log";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_lf.Text = dialog.FileName;
            }
        }

        private void FrpSetForm_Load(object sender, EventArgs e)
        {
            if (frpObj != null)
            {
                this.checkBox1.Checked = frpObj.AutoRun;
                this.textBox_name.Text = frpObj.Name;
                this.textBox_cf.Text = frpObj.ConfigFile;
                this.textBox_lf.Text = frpObj.LogFile;
            }
            else
            {
                frpObj = new FrpSettingModel();
            }
        }
    }
}

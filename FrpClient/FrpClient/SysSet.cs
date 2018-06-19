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
    public partial class SysSet : Form
    {
        public SysSet(bool isCheck,int hourint,int minitint)
        {
            InitializeComponent();
            numericUpDown1.Value = hourint;
            numericUpDown2.Value = minitint;
            checkBox1.Checked = isCheck;

            SetEnableY();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetEnableY();
        }

        private void SetEnableY()
        {
            numericUpDown1.Enabled = checkBox1.Checked;
            numericUpDown2.Enabled = checkBox1.Checked;
        }

        public bool AutoRefesh
        {
            get {return checkBox1.Checked; }
        }

        public int RefeshHour
        {
            get { return (int)numericUpDown1.Value; }
        }

        public int RefeshMinite
        {
            get { return (int)numericUpDown2.Value; }
        }
    }
}

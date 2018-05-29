using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPoint
{
    public partial class FriendlyMatchSettings : Form
    {
        public FriendlyMatchSettings()
        {
            InitializeComponent();
        }

        private void button_var1_Click(object sender, EventArgs e)
        {
            numericUpDown_set.Value = 1;
            numericUpDown_point.Value = 6;
            numericUpDown_serve.Value = 2;
        }

        private void button_var2_Click(object sender, EventArgs e)
        {
            numericUpDown_set.Value = 3;
            numericUpDown_point.Value = 11;
            numericUpDown_serve.Value = 2;
        }

        private void button_var3_Click(object sender, EventArgs e)
        {
            numericUpDown_set.Value = 5;
            numericUpDown_point.Value = 21;
            numericUpDown_serve.Value = 5;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            PingPoint_main.point_max = numericUpDown_point.Value;
            PingPoint_main.set_max = numericUpDown_set.Value;
            PingPoint_main.serve_number = numericUpDown_serve.Value;
            PingPoint_main.choosed = true;
            this.Close();
        }
    }
}

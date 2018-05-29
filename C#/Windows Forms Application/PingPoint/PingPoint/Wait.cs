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
    public partial class Wait : Form
    {
        string winner;
        public Wait(string win)
        {
            InitializeComponent();
            
            winner = win;
            label_info_win.Text = "Set wygrywa: " + winner;
        }

        private void label_change_Click(object sender, EventArgs e)
        {
            PingPoint_main.accept_set = true;
            this.Close();
        }

        private void label_next_set_Click(object sender, EventArgs e)
        {
            PingPoint_main.accept_set = false;
            this.Close();
        }
    }
}

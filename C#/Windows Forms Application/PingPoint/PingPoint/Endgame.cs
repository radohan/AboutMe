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
    public partial class Endgame : Form
    {
        string winner;
        public Endgame(string win, bool turniej)
        {
            InitializeComponent();
            winner = win;
            label_winner.Text = winner;
            if(turniej == true)
            {
                button_rematch.Enabled = false;
            }
            else
            {
                button_rematch.Enabled = true;
            }
        }

        private void button_rematch_Click(object sender, EventArgs e)
        {
            PingPoint_main.rematch = true;
            this.Close();
        }

        private void button_endgame_Click(object sender, EventArgs e)
        {
            PingPoint_main.rematch = false;
            this.Close();
        }
    }
}

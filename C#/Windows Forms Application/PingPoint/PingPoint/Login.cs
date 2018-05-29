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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button_sign_in_Click(object sender, EventArgs e)
        {
            if(textBox_login.Text != "" && textBox_password.Text != "")
            {
                PingPoint_main.my_login = textBox_login.Text;
                PingPoint_main.my_password = textBox_password.Text;
                this.Close();
            }
        }

        private void textBox_password_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button_sign_in_Click(sender, e);
            }
        }
    }
}

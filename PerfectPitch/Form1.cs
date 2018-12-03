using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerfectPitch
{
    public partial class Form1 : Form
    {
        int num_buttons = 2;
        public Form1()
        {
            InitializeComponent();

        }
        private void button_Click(int level)
        {
            this.Hide();
            var location = this.Location;
            var form = new Form2(level);
            form.FormClosed += (s, args) => this.Close();
            form.Show();
            form.Location = location;
            return;
        }


        private void menu_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button_Click(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button_Click(2);
        }
    }
}

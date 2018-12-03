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
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var location = this.Location;

            this.Hide();
            var form2 = new Form2(1);
            form2.FormClosed += (s, args) => this.Close();
            form2.Show();
            form2.Location = location;
            return;
        }


        private void menu_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}

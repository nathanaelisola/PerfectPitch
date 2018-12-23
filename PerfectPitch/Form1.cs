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

        HashSet<int> LEVELS = new HashSet<int>();
        public Form1(HashSet<int> levels)
        {


            int X = 0;
            int Y = 100;
            int WIDTH = 185;
            int HEIGHT = 70;
            int LEVELS_COUNT = 7;

            InitializeComponent();
            LEVELS = levels;

            for (int i = 1; i < LEVELS_COUNT + 1; i++)
            {
                Button b = new Button()
                {
                    Text = "Level " + i,
                    Size = new Size(WIDTH, HEIGHT),
                    Location = new Point(X, Y),
                    Anchor = AnchorStyles.Top,
                };

                b.Click += button_Click;
                Controls.Add(b);

                Y += HEIGHT + 5;

            }
            
            // title label
            Label l = new Label()
            {
                AutoSize = false,
                Text = "Perfect Pitch",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(FontFamily.GenericSerif, 18, FontStyle.Regular),
                Size = new Size(WIDTH, HEIGHT),
                Location = new Point(0, 25),
                Anchor = AnchorStyles.Top
            };

            Controls.Add(l);

            // form size
            Size = new Size(WIDTH + 75, Y + 50);
        }
        private void startLevel(int level)
        {
            this.Hide();
            var location = this.Location;
            var form = new Form2(LEVELS, level);
            form.FormClosed += (s, args) => this.Close();
            form.Show();
            form.Location = location;
            return;
        }


        private void menu_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            string s = b.Text;

            int level = Convert.ToInt32(s.Substring(s.Length - 1));            


            startLevel(level);
        }
    }
}

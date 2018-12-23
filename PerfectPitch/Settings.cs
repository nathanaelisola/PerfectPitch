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
    public partial class Settings : Form
    {
        object caller;

        int difficulty = 0; // easy default

        public Settings(object form)
        {
            caller = form;

            int X = 160;
            int Y = 25;
            int WIDTH = 185;
            int HEIGHT = 70;

            InitializeComponent();

            Label l = new Label()
            {
                Text = "Difficulty",
                Size = new Size(WIDTH, 25),
                Location = new Point(X, Y),
                Name = "difficultyLabel",

            };

            Controls.Add(l);

            Y += 25;
            List<string> difficulties = new List<string>(){ "Easy", "Hard" };

            ComboBox box = new ComboBox()
            {
                DataSource = difficulties,
                Size = new Size(WIDTH, HEIGHT),
                Location = new Point(X, Y),
                Name = "difficultyBox",
            };

            box.SelectedIndexChanged += new System.EventHandler(box_IndexChanged);

            Controls.Add(box);
            

        }
        private void box_IndexChanged(object sender, EventArgs e)
        {
            difficulty = (sender as ComboBox).SelectedIndex;
        }

        private void button_Click(object sender)
        {
            if ((sender as Button).Name.Equals("backButton"))
            {
                saveSettings();
                this.Close();
            }
        }

        private void saveSettings()
        {
            var result = MessageBox.Show("Settings were changed. Save changes?", "Confirm Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            (caller as Form2).Difficulty = difficulty;
            (caller as Form2).RefreshDisplay();
            return;
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            (caller as Form).Show();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            button_Click(sender);
        }
    }
}

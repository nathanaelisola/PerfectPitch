using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace PerfectPitch
{
    public partial class Form2 : Form
    {
        string sounds = @"../../sounds/";
        int _level;
        List<Button> buttons = new List<Button>();
        List<string> notes = new List<string>();
        int counter = 0;
        int score = 0;
        int note_check = -1;
        bool feedback; // toggle feedback
        Random r = new Random();
        SoundPlayer p = new SoundPlayer();
        Stopwatch sw = new Stopwatch();
        //System.Timers.Timer t = new System.Timers.Timer();

        public Form2(int level)
        {
            InitializeComponent();
            //t.Elapsed += new ElapsedEventHandler(timeElapsed);
            label_level.Text = String.Format("Level {0}",level);
            _level = level;
            label_feedback.Text = "";
            label_question.Text = "";

            if (level % 2 == 0)
                feedback = false;
            else
                feedback = true;

            return;

        }
        private void timeElapsed(object sender, EventArgs e)
        {
            if (sw.Elapsed >= new TimeSpan(3))
            {
                feedBack(0);
                playSound();
            }
            return;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var location = this.Location;
            var form1 = new Form1();
            form1.FormClosed += (s, args) => this.Close();
            form1.Show();
            form1.Location = location;
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_level == 1 || _level == 2)
                Level1();

            return;
        }

        private void answerClick(object sender, EventArgs e)
        {
            string answer = (sender as Button).Name;

            if (notes[note_check].Equals(answer))
            {
                score++;
                feedBack(1);
            }
            else
                feedBack(0);


            if (counter == 10)
                gameOver();
            else
                playSound();

            return;
        }
        private void feedBack(int correct)
        {
            // no feedback sometimes
            if (!feedback)
                return;

            if (correct == 1)
            {
                label_feedback.Text = ":)";
                label_feedback.BackColor = Color.Green;
                label_feedback.ForeColor = Color.White;
            }
            else
            {
                label_feedback.Text = ":(";
                label_feedback.BackColor = Color.Red;
                label_feedback.ForeColor = Color.White;
            }
            return;
        }

        private void gameOver()
        {
            float total = (float)score / counter;
            MessageBox.Show(String.Format("Test concluded.\nResults: {0: 00%} accuracy.", total));
            toggleButtons();
            return;
        }

        private void makeButtons()
        {
            int Y = 5;
            int x0;
            int X = x0 = 5;
            int y;
            int x = y = 52;

            foreach (string s in notes)
            {
                
                string text = s.Split('-')[1];
                int end = text.IndexOf(".");
                text = text.Substring(1, end - 1);
                text = text.Replace('+', '#');

                Button b = new Button();
                b.Location = new Point(X, Y);
                b.Size = new Size(x, y);

                b.Name = s;
                b.Text = text;
                b.Font = new Font(b.Font.FontFamily, (float)15.75);
                b.Click += new EventHandler(this.answerClick);
                group_buttons.Controls.Add(b);
                buttons.Add(b);

                X += x;
            }
            return;

        }
        private void playSound()
        {
            toggleTimer(0);

            int next = r.Next(0, notes.Count);
            if (note_check == next)
            {
                int change = r.Next(0, 10);

                if (change <= 8)
                    while (note_check == next)
                        next = r.Next(0, notes.Count);
            }

            Debug.WriteLine(notes[next]);
            note_check = next;
            counter++;
            p.SoundLocation = notes[next];
            p.Load();
            p.Play();
            toggleTimer(1);
            return;
        }
        private void toggleButtons()
        {
            foreach (Button b in buttons)
                b.Visible = !b.Visible;

            return;
        }
        private void toggleTimer(int enable)
        {
            if (enable == 0)
            {
                timer.Stop();
                sw.Stop();
                sw.Reset();
            }
            else
            {
                sw.Start();
                timer.Start();
            }

            return;
        }
        private void Level1()
        {
            notes.Clear();
            buttons.Clear();
            score = 0;
            counter = 0;

            string E = String.Format(@"{0}{1}", sounds, @"Piano/17 - E.wav");
            string F = String.Format(@"{0}{1}", sounds, @"Piano/18 - F.wav");
            string Fs = String.Format(@"{0}{1}", sounds, @"Piano/19 - F+.wav");

            notes.Add(E);
            notes.Add(F);
            notes.Add(Fs);

            makeButtons();
            playSound();
            return;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Debug.WriteLine(sw.Elapsed);
            if (sw.Elapsed > new TimeSpan(0,0,3))
            {
                //sw.Reset();
                feedBack(0);
                playSound();

                if (counter == 10)
                    gameOver();
            }
            return;
        }

        private void menu_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

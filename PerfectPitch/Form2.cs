using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        string folder_sounds = @"../../sounds/";
    
        string low;
        string med;
        string high;

        string sound_correct;
        string sound_incorrect;

        enum difficulties { Normal, Hard };

        int difficulty = 0; // easy default
        int _level;
        int pitches = 3;

        string instr = "Piano"; // this needs to be passed at some point probably
        List<Button> buttons = new List<Button>();
        List<string> notes = new List<string>();
        List<string> button_texts = new List<string>();
        List<int> played_sounds = new List<int>();

        int counter = 0;
        int score = 0;
        int note_check = -1;
        bool feedback; // toggle feedback
        Random r = new Random();
        SoundPlayer p = new SoundPlayer();
        SoundPlayer q = new SoundPlayer();
        Stopwatch sw = new Stopwatch();

        HashSet<int> LEVELS = new HashSet<int>();

        public int Difficulty
        {
            get { return difficulty; }
            set
            {
                difficulty = value;
            }
        }

        public Form2(HashSet<int> levels, int level)
        {
            InitializeComponent();

            LEVELS = levels;
            label_level.Text = String.Format("Level {0} - {1}",level, (difficulties)difficulty);
            _level = level;
            label_feedback.Text = "";
            label_question.Text = "";

            if (level % 2 == 0)
                feedback = false;
            else
                feedback = true;

            low = folder_sounds + instr + @"/Low/";
            med = folder_sounds + instr + @"/Middle/";
            high = folder_sounds + instr + @"/High/";

            sound_correct = folder_sounds + "correct.wav";
            sound_incorrect = folder_sounds + "incorrect.wav";
            return;


        }

        public void RefreshDisplay()
        {
            label_level.Text = String.Format("Level {0} - {1}", _level, (difficulties)difficulty);

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
            var form1 = new Form1(LEVELS);
            form1.FormClosed += (s, args) => this.Close();
            form1.Show();
            form1.Location = location;
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_level == 1 || _level == 2)
                Level1();
            if (_level == 3 || _level == 4)
                Level3();
            return;
        }

        private void answerClick(object sender, EventArgs e)
        {
            string answer = (sender as Button).Name;
            bool correct = false;

            if (notes[note_check].Contains(answer.Replace("#","+") + "."))
                correct = true;

            if (correct)
            {
                score++;
                feedBack(1);
            }
            else
                feedBack(0);

            if (counter == 10)
            {
                toggleTimer(0);
                gameOver();
            }
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
                q.SoundLocation = sound_correct;
                q.Load();
                q.PlaySync();
            }
            else
            {
                label_feedback.Text = ":(";
                q.SoundLocation = sound_incorrect;
                q.Load();
                q.PlaySync();
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
            int col_count = 0;
            foreach (string s in button_texts)
            {
                Button b = new Button();
                b.Location = new Point(X, Y);
                b.Size = new Size(x, y);

                b.Name = s;
                b.Text = s;
                b.Font = new Font(b.Font.FontFamily, (float)15.75);
                b.Click += new EventHandler(this.answerClick);
                group_buttons.Controls.Add(b);
                buttons.Add(b);

                X += x;
                col_count++;

                if (col_count == 6)
                    Y += y;
            }
            return;

        }

        int getNextSound()
        {
            int next = r.Next(0, notes.Count);

            // this needs some work
            if (played_sounds.Contains(next))
            {
                int change = r.Next(0, 10);

                if (change <= 8)
                {
                    int cnt = 0;
                    while (played_sounds.Contains(next))
                    {
                        next = r.Next(0, notes.Count);
                        cnt++;

                        if (cnt > 10)
                            break;
                    }
                }
            }

            played_sounds.Add(next);
            return next;
        }

        private void playSound()
        {
            toggleTimer(0);

            int next = getNextSound();

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
            Reset();

            notes.Add(med + "E.wav");
            notes.Add(med + "F.wav");
            notes.Add(med + "F+.wav");

            button_texts.Add("E");
            button_texts.Add("F");
            button_texts.Add("F#");

            makeButtons();
            playSound();
            return;
        }
        private void Level3()
        {
            Reset();
            notes.Add(low + "E.wav");
            notes.Add(low + "F.wav");
            notes.Add(low + "F+.wav");
            notes.Add(med + "E.wav");
            notes.Add(med + "F.wav");
            notes.Add(med + "F+.wav"); 
            notes.Add(high + "E.wav");
            notes.Add(high + "F.wav");
            notes.Add(high + "F+.wav");

            button_texts.Add("E");
            button_texts.Add("F");
            button_texts.Add("F#");
            makeButtons();
            playSound();
            return;
        }

        private void Reset()
        {
            notes.Clear();
            buttons.Clear();
            button_texts.Clear();
            score = 0;
            counter = 0;
            toggleTimer(0);
            played_sounds.Clear();
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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var location = this.Location;
            var settings = new Settings(this);
            //settings.FormClosed += (s, args) => this.Close();
            settings.Show();
            settings.Location = location;
            return;
        }
    }
}

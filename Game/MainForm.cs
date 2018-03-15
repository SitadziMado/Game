using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class MainForm : Form
    {
        private const float Fps = 60.0f;
        private const long Frame = (long)(1000.0f / Fps);

        private bool running = true;
        private Grid mGrid;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var particles = new Particle[]
            {
                new Particle(new Vector3(5.0f, 5.0f, 0.0f), 1.0f),
                new Particle(new Vector3(7.0f, 7.0f, 0.0f), 1.0f),
                new Particle(new Vector3(-2.5f, 9.5f, 0.5f), 1.0f),
            };

            var constraints = new LengthConstraint[]
            {
                new LengthConstraint(new Tuple<int, int>(0, 1), 5.0f),
                new LengthConstraint(new Tuple<int, int>(0, 2), 5.0f),
                new LengthConstraint(new Tuple<int, int>(1, 2), 5.0f),
            };

            mGrid = new Grid(particles, constraints);

            Task.Factory.StartNew(() =>
            {
                long previous = 0;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                long current;

                while (running)
                {
                    current = sw.ElapsedMilliseconds;
                    var elapsed = current - previous;

                    if (elapsed >= Frame)
                    {
                        previous = current;
                        mGrid.TimeStep(elapsed);
                        Invalidate();
                    }
                }

                sw.Stop();
            });
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            foreach (var v in mGrid.Points)
            {
                e.Graphics.FillEllipse(Brushes.Bisque, v.X - 2.0f, v.Y - 2.0f, 4.0f, 4.0f);
            }
        }
    }
}

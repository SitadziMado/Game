using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
            Task.Factory.StartNew(() =>
            {
                long previous = 0;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                long current = sw.ElapsedMilliseconds;

                while (running)
                {
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
            foreach (var v in mGrid.Points)
            {
            }
        }
    }
}

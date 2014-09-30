using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class MainWindow : Form, IMainView
    {
        public event Action NewAgent;
        public event Action ResetCounter;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void newAgentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewAgent != null)
            {
                NewAgent();
            }
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.Close();
            }

            if (ResetCounter != null)
            {
                ResetCounter();
            }
        }

        private void floatAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.WindowState = FormWindowState.Maximized;
            }
        }
    }
}

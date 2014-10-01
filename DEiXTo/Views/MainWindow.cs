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
        public event Action CascadeAgentWindows;
        public event Action CloseAgentWindows;
        public event Action FloatAgentWindows;

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
            if (CascadeAgentWindows != null)
            {
                CascadeAgentWindows();
            }
        }

        public void CascadeAgents()
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        public void FloatAgents()
        {
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.WindowState = FormWindowState.Maximized;
            }
        }

        public void CloseAgents()
        {
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.Close();
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseAgentWindows != null)
            {
                CloseAgentWindows();
            }
        }

        private void floatAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FloatAgentWindows != null)
            {
                FloatAgentWindows();
            }
        }
    }
}

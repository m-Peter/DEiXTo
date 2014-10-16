using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    /// <summary>
    /// This Form is a multiple-document inteface (MDI) parent for 
    /// the DeixtoAgentWindow child forms. It can create, close,
    /// select and change the state of its DeixtoAgentWindow child
    /// forms.
    /// </summary>
    public partial class MainWindow : Form, IMainView
    {
        #region Public Events
        // Fires when the NewAgent menu item gets clicked
        public event Action CreateNewAgent;
        // Fires when the CascadeAgents menu item gets clicked
        public event Action CascadeAgentWindows;
        // Fires when the CloseAgents menu item gets clicked
        public event Action CloseAgentWindows;
        // Fires when the FloatAgents menu item gets clicked
        public event Action FloatAgentWindows;
        // Fires when the user attempts to close the window
        public event Action<FormClosingEventArgs> WindowClosing;
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Cascades all the DeixtoAgentWindows within the region of
        /// the MainWindow MDI parent form.
        /// </summary>
        public void CascadeAgents()
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        /// <summary>
        /// Maximizes the WindowState of the DeixtoAgentWindows contained
        /// in the MainWindow form.
        /// </summary>
        public void FloatAgents()
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.WindowState = FormWindowState.Maximized;
            }
        }

        /// <summary>
        /// Closes all the DeixtoAgentsWindows contained in the MainWindow
        /// form.
        /// </summary>
        public void CloseAgents()
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        /// <summary>
        /// Ask the user if he really wants to close this window. Returns true
        /// if the answer is yes, false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool AskUserToConfirmClosing()
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "DEiXTo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return result == DialogResult.Yes;
        }
        #endregion

        #region Private Events
        private void newAgentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateNewAgent != null)
            {
                CreateNewAgent();
            }
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CascadeAgentWindows != null)
            {
                CascadeAgentWindows();
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

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowClosing != null)
            {
                WindowClosing(e);
            }
        }
        #endregion
    }
}

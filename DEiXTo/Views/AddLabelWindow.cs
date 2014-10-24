using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class AddLabelWindow : Form, IAddLabelView
    {
        #region Public Events
        // Fires when the user presses the OK button
        public event Action AddLabel;
        // Fires when the user presses a keyboard
        public event Action<KeyEventArgs> KeyDownPress;
        #endregion

        #region Constructors
        public AddLabelWindow()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += AddLabelWindow_KeyDown;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLabelText()
        {
            return AddLabelTextBox.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowInvalidLabelMessage()
        {
            MessageBox.Show("Invalid empty label!", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Exit()
        {
            this.Close();
        }
        #endregion

        #region Private Events
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (AddLabel != null)
            {
                AddLabel();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddLabelWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDownPress != null)
            {
                KeyDownPress(e);
            }
        }
        #endregion
    }
}

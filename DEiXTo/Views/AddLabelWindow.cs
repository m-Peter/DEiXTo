using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class AddLabelWindow : Form, IAddLabelView
    {
        public event Action AddLabel;
        public event Action<KeyEventArgs> KeyDownPress;

        public AddLabelWindow()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += AddLabelWindow_KeyDown;
        }

        void AddLabelWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDownPress != null)
            {
                KeyDownPress(e);
            }
        }

        public string GetLabelText()
        {
            return AddLabelTextBox.Text;
        }

        public void ShowInvalidLabelMessage()
        {
            MessageBox.Show("Invalid empty label!", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void Exit()
        {
            this.Close();
        }

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
    }
}

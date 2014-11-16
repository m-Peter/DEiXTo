using DEiXTo.Presenters;
using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class AddLabelWindow : Form, IAddLabelView
    {
        public AddLabelWindow()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += AddLabelWindow_KeyDown;
        }

        public string LabelText
        {
            get { return AddLabelTextBox.Text; }
            set { AddLabelTextBox.Text = value; }
        }

        public AddLabelPresenter Presenter { get; set; }

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
            Presenter.AddLabel();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddLabelWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Presenter.KeyDownPress(e.KeyCode);
        }
    }
}

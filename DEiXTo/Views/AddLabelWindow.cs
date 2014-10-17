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
    public partial class AddLabelWindow : Form, IAddLabelView
    {
        public event Action AddLabel;

        public AddLabelWindow()
        {
            InitializeComponent();
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

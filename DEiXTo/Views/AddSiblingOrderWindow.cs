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
    public partial class AddSiblingOrderWindow : Form, IAddSiblingOrderView
    {
        public event Action<Boolean> SiblingOrderCheckboxChanged;
        public event Action AddSiblingOrder;

        public AddSiblingOrderWindow()
        {
            InitializeComponent();
        }

        public void ApplyVisibilityStateInOrdering(bool checkedState)
        {
            StartIndexNUD.Enabled = checkedState;
            StepValueNUD.Enabled = checkedState;
        }

        public int GetStartIndex
        {
            get { return (int)StartIndexNUD.Value; }
        }

        public int GetStepValue
        {
            get { return (int)StepValueNUD.Value; }
        }

        public bool CareAboutSiblingOrder
        {
            get { return SiblingOrderCheckBox.Checked; }
        }

        public void Exit()
        {
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (AddSiblingOrder != null)
            {
                AddSiblingOrder();
            }
        }

        private void SiblingOrderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = SiblingOrderCheckBox.Checked;
            
            if (SiblingOrderCheckboxChanged != null)
            {
                SiblingOrderCheckboxChanged(state);
            }
        }
    }
}

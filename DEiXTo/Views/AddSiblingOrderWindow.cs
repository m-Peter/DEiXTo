using DEiXTo.Presenters;
using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class AddSiblingOrderWindow : Form, IAddSiblingOrderView
    {
        public AddSiblingOrderWindow()
        {
            InitializeComponent();
        }

        public AddSiblingOrderPresenter Presenter { get; set; }

        public void ApplyVisibilityStateInOrdering(bool checkedState)
        {
            StartIndexNUD.Enabled = checkedState;
            StepValueNUD.Enabled = checkedState;
        }

        public int StartIndex
        {
            get { return (int)StartIndexNUD.Value; }
            set { StartIndexNUD.Value = value; }
        }

        public int StepValue
        {
            get { return (int)StepValueNUD.Value; }
            set { StepValueNUD.Value = value; }
        }

        public bool CareAboutSiblingOrder
        {
            get { return SiblingOrderCheckBox.Checked; }
            set { SiblingOrderCheckBox.Checked = value; }
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
            Presenter.AddSiblingOrder();
        }

        private void SiblingOrderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = SiblingOrderCheckBox.Checked;
            
            Presenter.ChangeSiblingOrderVisibility(state);
        }
    }
}

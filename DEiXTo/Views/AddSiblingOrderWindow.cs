using DEiXTo.Presenters;
using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class AddSiblingOrderWindow : Form, IAddSiblingOrderView
    {
        #region Constructors
        public AddSiblingOrderWindow()
        {
            InitializeComponent();
        }
        #endregion

        public AddSiblingOrderPresenter Presenter { get; set; }

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkedState"></param>
        public void ApplyVisibilityStateInOrdering(bool checkedState)
        {
            StartIndexNUD.Enabled = checkedState;
            StepValueNUD.Enabled = checkedState;
        }

        /// <summary>
        /// 
        /// </summary>
        public int StartIndex
        {
            get { return (int)StartIndexNUD.Value; }
            set { StartIndexNUD.Value = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int StepValue
        {
            get { return (int)StepValueNUD.Value; }
            set { StepValueNUD.Value = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CareAboutSiblingOrder
        {
            get { return SiblingOrderCheckBox.Checked; }
            set { SiblingOrderCheckBox.Checked = value; }
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
        #endregion
    }
}

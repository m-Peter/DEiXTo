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
        #region Public Events
        // Fires when the user changes the checked state of CareAboutSiblingOrder checkbox's
        public event Action<Boolean> SiblingOrderCheckboxChanged;
        // Fires when the user presses the OK button
        public event Action AddSiblingOrder;
        #endregion

        #region Constructors
        public AddSiblingOrderWindow()
        {
            InitializeComponent();
        }
        #endregion

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
        public int GetStartIndex
        {
            get { return (int)StartIndexNUD.Value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetStepValue
        {
            get { return (int)StepValueNUD.Value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CareAboutSiblingOrder
        {
            get { return SiblingOrderCheckBox.Checked; }
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
        #endregion
    }
}

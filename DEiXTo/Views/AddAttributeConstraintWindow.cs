using DEiXTo.Models;
using DEiXTo.Presenters;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class AddAttributeConstraintWindow : Form, IAddAttributeConstraintView
    {
        public AddAttributeConstraintWindow()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += AddAttributeConstraintWindow_KeyDown;
        }

        void AddAttributeConstraintWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var name = AttributeNameTextBox.Text;
            Presenter.KeyDown(e.KeyCode, name);
        }

        public AddAttributeConstraintPresenter Presenter { get; set; }

        public void LoadAttribute(TagAttribute attribute)
        {
            //AttributesComboBox.DataSource = attributes;
            //var attribute = attributes[0];
            AttributeNameTextBox.Text = attribute.Name;
            AddConstraintTextBox.Text = attribute.Value;
        }

        public string Constraint
        {
            get { return AddConstraintTextBox.Text; }
            set { AddConstraintTextBox.Text = value; }
        }

        public void Exit()
        {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            var name = AttributeNameTextBox.Text;
            Presenter.AddConstraint(name);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

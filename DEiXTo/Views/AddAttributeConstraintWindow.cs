using DEiXTo.Models;
using DEiXTo.Presenters;
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
    public partial class AddAttributeConstraintWindow : Form, IAddAttributeConstraintView
    {
        public AddAttributeConstraintWindow()
        {
            InitializeComponent();
            var attributes = new List<TagAttribute>();
            attributes.Add(new TagAttribute { Name = "id", Value = "github-link" });
            attributes.Add(new TagAttribute { Name = "href", Value = "www.github.com" });
            attributes.Add(new TagAttribute { Name = "alt", Value = "Move to GitHub" });
            AttributesComboBox.DataSource = attributes;
            AttributesComboBox.DisplayMember = "Name";
            AttributesComboBox.ValueMember = "Value";
        }

        public AddAttributeConstraintPresenter Presenter { get; set; }

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
            Presenter.AddConstraint();
        }
    }
}

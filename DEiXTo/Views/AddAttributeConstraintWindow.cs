﻿using DEiXTo.Models;
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
            AttributesComboBox.DisplayMember = "Name";
            AttributesComboBox.ValueMember = "Value";
        }

        public AddAttributeConstraintPresenter Presenter { get; set; }

        public void LoadAttributes(List<TagAttribute> attributes)
        {
            AttributesComboBox.DataSource = attributes;
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
            var tag = AttributesComboBox.SelectedItem as TagAttribute;
            Presenter.AddConstraint(tag.Name);
        }

        private void AttributesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tag = AttributesComboBox.SelectedItem as TagAttribute;
            Presenter.AttributeChanged(tag);
        }
    }
}
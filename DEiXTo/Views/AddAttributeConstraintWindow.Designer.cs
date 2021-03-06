﻿namespace DEiXTo.Views
{
    partial class AddAttributeConstraintWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.AddConstraintLabel = new System.Windows.Forms.Label();
            this.AddConstraintTextBox = new System.Windows.Forms.TextBox();
            this.AttributeNameLabel = new System.Windows.Forms.Label();
            this.AttributeNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(202, 104);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(303, 104);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // AddConstraintLabel
            // 
            this.AddConstraintLabel.AutoSize = true;
            this.AddConstraintLabel.Location = new System.Drawing.Point(21, 76);
            this.AddConstraintLabel.Name = "AddConstraintLabel";
            this.AddConstraintLabel.Size = new System.Drawing.Size(127, 13);
            this.AddConstraintLabel.TabIndex = 2;
            this.AddConstraintLabel.Text = "Please enter a constraint:";
            // 
            // AddConstraintTextBox
            // 
            this.AddConstraintTextBox.Location = new System.Drawing.Point(166, 69);
            this.AddConstraintTextBox.Name = "AddConstraintTextBox";
            this.AddConstraintTextBox.Size = new System.Drawing.Size(212, 20);
            this.AddConstraintTextBox.TabIndex = 3;
            // 
            // AttributeNameLabel
            // 
            this.AttributeNameLabel.AutoSize = true;
            this.AttributeNameLabel.Location = new System.Drawing.Point(21, 23);
            this.AttributeNameLabel.Name = "AttributeNameLabel";
            this.AttributeNameLabel.Size = new System.Drawing.Size(136, 13);
            this.AttributeNameLabel.TabIndex = 5;
            this.AttributeNameLabel.Text = "Please choose an attribute:";
            // 
            // AttributeNameTextBox
            // 
            this.AttributeNameTextBox.Location = new System.Drawing.Point(166, 23);
            this.AttributeNameTextBox.Name = "AttributeNameTextBox";
            this.AttributeNameTextBox.ReadOnly = true;
            this.AttributeNameTextBox.Size = new System.Drawing.Size(212, 20);
            this.AttributeNameTextBox.TabIndex = 6;
            // 
            // AddAttributeConstraintWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 139);
            this.Controls.Add(this.AttributeNameTextBox);
            this.Controls.Add(this.AttributeNameLabel);
            this.Controls.Add(this.AddConstraintTextBox);
            this.Controls.Add(this.AddConstraintLabel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddAttributeConstraintWindow";
            this.Text = "AddAttributeConstraint";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label AddConstraintLabel;
        public System.Windows.Forms.TextBox AddConstraintTextBox;
        private System.Windows.Forms.Label AttributeNameLabel;
        private System.Windows.Forms.TextBox AttributeNameTextBox;
    }
}
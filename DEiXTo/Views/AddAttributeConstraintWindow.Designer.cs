namespace DEiXTo.Views
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
            this.AttributesComboBox = new System.Windows.Forms.ComboBox();
            this.ChooseAttributeLabel = new System.Windows.Forms.Label();
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
            // AttributesComboBox
            // 
            this.AttributesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AttributesComboBox.FormattingEnabled = true;
            this.AttributesComboBox.Location = new System.Drawing.Point(257, 20);
            this.AttributesComboBox.Name = "AttributesComboBox";
            this.AttributesComboBox.Size = new System.Drawing.Size(121, 21);
            this.AttributesComboBox.TabIndex = 4;
            // 
            // ChooseAttributeLabel
            // 
            this.ChooseAttributeLabel.AutoSize = true;
            this.ChooseAttributeLabel.Location = new System.Drawing.Point(21, 23);
            this.ChooseAttributeLabel.Name = "ChooseAttributeLabel";
            this.ChooseAttributeLabel.Size = new System.Drawing.Size(136, 13);
            this.ChooseAttributeLabel.TabIndex = 5;
            this.ChooseAttributeLabel.Text = "Please choose an attribute:";
            // 
            // AddAttributeConstraintWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 142);
            this.Controls.Add(this.ChooseAttributeLabel);
            this.Controls.Add(this.AttributesComboBox);
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

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label AddConstraintLabel;
        private System.Windows.Forms.TextBox AddConstraintTextBox;
        private System.Windows.Forms.ComboBox AttributesComboBox;
        private System.Windows.Forms.Label ChooseAttributeLabel;
    }
}
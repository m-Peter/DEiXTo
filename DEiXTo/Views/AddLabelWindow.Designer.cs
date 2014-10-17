namespace DEiXTo.Views
{
    partial class AddLabelWindow
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
            this.EnterLabelLabel = new System.Windows.Forms.Label();
            this.AddLabelTextBox = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EnterLabelLabel
            // 
            this.EnterLabelLabel.AutoSize = true;
            this.EnterLabelLabel.Location = new System.Drawing.Point(12, 19);
            this.EnterLabelLabel.Name = "EnterLabelLabel";
            this.EnterLabelLabel.Size = new System.Drawing.Size(103, 13);
            this.EnterLabelLabel.TabIndex = 0;
            this.EnterLabelLabel.Text = "Please enter a label:";
            // 
            // AddLabelTextBox
            // 
            this.AddLabelTextBox.Location = new System.Drawing.Point(121, 19);
            this.AddLabelTextBox.Name = "AddLabelTextBox";
            this.AddLabelTextBox.Size = new System.Drawing.Size(232, 20);
            this.AddLabelTextBox.TabIndex = 1;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(197, 57);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(278, 57);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // AddLabelWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 95);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.AddLabelTextBox);
            this.Controls.Add(this.EnterLabelLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddLabelWindow";
            this.Text = "DEiXTo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EnterLabelLabel;
        private System.Windows.Forms.TextBox AddLabelTextBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
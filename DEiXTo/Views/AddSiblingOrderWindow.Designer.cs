namespace DEiXTo.Views
{
    partial class AddSiblingOrderWindow
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
            this.SiblingOrderCheckBox = new System.Windows.Forms.CheckBox();
            this.StartIndexLabel = new System.Windows.Forms.Label();
            this.StepValueLabel = new System.Windows.Forms.Label();
            this.StartIndexNUD = new System.Windows.Forms.NumericUpDown();
            this.StepValueNUD = new System.Windows.Forms.NumericUpDown();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.StartIndexNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepValueNUD)).BeginInit();
            this.SuspendLayout();
            // 
            // SiblingOrderCheckBox
            // 
            this.SiblingOrderCheckBox.AutoSize = true;
            this.SiblingOrderCheckBox.Location = new System.Drawing.Point(75, 37);
            this.SiblingOrderCheckBox.Name = "SiblingOrderCheckBox";
            this.SiblingOrderCheckBox.Size = new System.Drawing.Size(137, 17);
            this.SiblingOrderCheckBox.TabIndex = 0;
            this.SiblingOrderCheckBox.Text = "Care about sibling order";
            this.SiblingOrderCheckBox.UseVisualStyleBackColor = true;
            this.SiblingOrderCheckBox.CheckedChanged += new System.EventHandler(this.SiblingOrderCheckBox_CheckedChanged);
            // 
            // StartIndexLabel
            // 
            this.StartIndexLabel.AutoSize = true;
            this.StartIndexLabel.Location = new System.Drawing.Point(72, 86);
            this.StartIndexLabel.Name = "StartIndexLabel";
            this.StartIndexLabel.Size = new System.Drawing.Size(117, 13);
            this.StartIndexLabel.TabIndex = 1;
            this.StartIndexLabel.Text = "Please enter start index";
            // 
            // StepValueLabel
            // 
            this.StepValueLabel.AutoSize = true;
            this.StepValueLabel.Location = new System.Drawing.Point(71, 134);
            this.StepValueLabel.Name = "StepValueLabel";
            this.StepValueLabel.Size = new System.Drawing.Size(118, 13);
            this.StepValueLabel.TabIndex = 2;
            this.StepValueLabel.Text = "Please enter step value";
            // 
            // StartIndexNUD
            // 
            this.StartIndexNUD.Enabled = false;
            this.StartIndexNUD.Location = new System.Drawing.Point(231, 79);
            this.StartIndexNUD.Name = "StartIndexNUD";
            this.StartIndexNUD.Size = new System.Drawing.Size(60, 20);
            this.StartIndexNUD.TabIndex = 3;
            // 
            // StepValueNUD
            // 
            this.StepValueNUD.Enabled = false;
            this.StepValueNUD.Location = new System.Drawing.Point(231, 132);
            this.StepValueNUD.Name = "StepValueNUD";
            this.StepValueNUD.Size = new System.Drawing.Size(60, 20);
            this.StepValueNUD.TabIndex = 4;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(74, 182);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(216, 182);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // AddSiblingOrderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 242);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.StepValueNUD);
            this.Controls.Add(this.StartIndexNUD);
            this.Controls.Add(this.StepValueLabel);
            this.Controls.Add(this.StartIndexLabel);
            this.Controls.Add(this.SiblingOrderCheckBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddSiblingOrderWindow";
            this.Text = "Sibling Order Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.StartIndexNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepValueNUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox SiblingOrderCheckBox;
        private System.Windows.Forms.Label StartIndexLabel;
        private System.Windows.Forms.Label StepValueLabel;
        private System.Windows.Forms.NumericUpDown StartIndexNUD;
        private System.Windows.Forms.NumericUpDown StepValueNUD;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
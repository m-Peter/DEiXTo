namespace DEiXTo.Views
{
    partial class RegexBuilderWindow
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
            this.PatternsTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PatternsListView = new System.Windows.Forms.ListView();
            this.PatternColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ExampleColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.AddRegexLabel = new System.Windows.Forms.Label();
            this.AddRegexTextBox = new System.Windows.Forms.TextBox();
            this.InverseEvaluationCheckBox = new System.Windows.Forms.CheckBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ConstraintActionComboBox = new System.Windows.Forms.ComboBox();
            this.PatternsTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PatternsTabControl
            // 
            this.PatternsTabControl.Controls.Add(this.tabPage1);
            this.PatternsTabControl.Controls.Add(this.tabPage2);
            this.PatternsTabControl.Location = new System.Drawing.Point(12, 12);
            this.PatternsTabControl.Name = "PatternsTabControl";
            this.PatternsTabControl.SelectedIndex = 0;
            this.PatternsTabControl.Size = new System.Drawing.Size(453, 296);
            this.PatternsTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PatternsListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(445, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Patterns";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // PatternsListView
            // 
            this.PatternsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PatternColumn,
            this.DescriptionColumn,
            this.ExampleColumn});
            this.PatternsListView.FullRowSelect = true;
            this.PatternsListView.Location = new System.Drawing.Point(6, 15);
            this.PatternsListView.Name = "PatternsListView";
            this.PatternsListView.Size = new System.Drawing.Size(433, 249);
            this.PatternsListView.TabIndex = 0;
            this.PatternsListView.UseCompatibleStateImageBehavior = false;
            this.PatternsListView.View = System.Windows.Forms.View.Details;
            this.PatternsListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.PatternsListView_ItemSelectionChanged);
            // 
            // PatternColumn
            // 
            this.PatternColumn.Text = "Pattern";
            this.PatternColumn.Width = 100;
            // 
            // DescriptionColumn
            // 
            this.DescriptionColumn.Text = "Description";
            this.DescriptionColumn.Width = 150;
            // 
            // ExampleColumn
            // 
            this.ExampleColumn.Text = "Example";
            this.ExampleColumn.Width = 175;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(445, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Syntax";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // AddRegexLabel
            // 
            this.AddRegexLabel.AutoSize = true;
            this.AddRegexLabel.Location = new System.Drawing.Point(13, 325);
            this.AddRegexLabel.Name = "AddRegexLabel";
            this.AddRegexLabel.Size = new System.Drawing.Size(153, 13);
            this.AddRegexLabel.TabIndex = 1;
            this.AddRegexLabel.Text = "Please give regular expression:";
            // 
            // AddRegexTextBox
            // 
            this.AddRegexTextBox.HideSelection = false;
            this.AddRegexTextBox.Location = new System.Drawing.Point(172, 322);
            this.AddRegexTextBox.Name = "AddRegexTextBox";
            this.AddRegexTextBox.Size = new System.Drawing.Size(289, 20);
            this.AddRegexTextBox.TabIndex = 2;
            // 
            // InverseEvaluationCheckBox
            // 
            this.InverseEvaluationCheckBox.AutoSize = true;
            this.InverseEvaluationCheckBox.Location = new System.Drawing.Point(16, 389);
            this.InverseEvaluationCheckBox.Name = "InverseEvaluationCheckBox";
            this.InverseEvaluationCheckBox.Size = new System.Drawing.Size(151, 17);
            this.InverseEvaluationCheckBox.TabIndex = 3;
            this.InverseEvaluationCheckBox.Text = "Perform inverse evaluation";
            this.InverseEvaluationCheckBox.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(16, 423);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(106, 423);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 356);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Please select constraint action:";
            // 
            // ConstraintActionComboBox
            // 
            this.ConstraintActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConstraintActionComboBox.FormattingEnabled = true;
            this.ConstraintActionComboBox.Location = new System.Drawing.Point(173, 353);
            this.ConstraintActionComboBox.Name = "ConstraintActionComboBox";
            this.ConstraintActionComboBox.Size = new System.Drawing.Size(121, 21);
            this.ConstraintActionComboBox.TabIndex = 7;
            // 
            // RegexBuilderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 458);
            this.Controls.Add(this.ConstraintActionComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.InverseEvaluationCheckBox);
            this.Controls.Add(this.AddRegexTextBox);
            this.Controls.Add(this.AddRegexLabel);
            this.Controls.Add(this.PatternsTabControl);
            this.Name = "RegexBuilderWindow";
            this.Text = "RegexBuilderWindow";
            this.PatternsTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl PatternsTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView PatternsListView;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label AddRegexLabel;
        public System.Windows.Forms.TextBox AddRegexTextBox;
        public System.Windows.Forms.CheckBox InverseEvaluationCheckBox;
        public System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.ColumnHeader PatternColumn;
        private System.Windows.Forms.ColumnHeader DescriptionColumn;
        private System.Windows.Forms.ColumnHeader ExampleColumn;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox ConstraintActionComboBox;
    }
}
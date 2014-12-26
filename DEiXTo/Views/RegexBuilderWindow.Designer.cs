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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RegexTb = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.InputTb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.MatchRtb = new System.Windows.Forms.RichTextBox();
            this.GroupsRtb = new System.Windows.Forms.RichTextBox();
            this.PatternsTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PatternsTabControl
            // 
            this.PatternsTabControl.Controls.Add(this.tabPage1);
            this.PatternsTabControl.Controls.Add(this.tabPage2);
            this.PatternsTabControl.Location = new System.Drawing.Point(12, 238);
            this.PatternsTabControl.Name = "PatternsTabControl";
            this.PatternsTabControl.SelectedIndex = 0;
            this.PatternsTabControl.Size = new System.Drawing.Size(456, 272);
            this.PatternsTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PatternsListView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(448, 246);
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
            this.PatternsListView.Size = new System.Drawing.Size(435, 227);
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
            this.tabPage2.Size = new System.Drawing.Size(448, 246);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Syntax";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // AddRegexLabel
            // 
            this.AddRegexLabel.AutoSize = true;
            this.AddRegexLabel.Location = new System.Drawing.Point(744, 497);
            this.AddRegexLabel.Name = "AddRegexLabel";
            this.AddRegexLabel.Size = new System.Drawing.Size(153, 13);
            this.AddRegexLabel.TabIndex = 1;
            this.AddRegexLabel.Text = "Please give regular expression:";
            // 
            // AddRegexTextBox
            // 
            this.AddRegexTextBox.HideSelection = false;
            this.AddRegexTextBox.Location = new System.Drawing.Point(608, 461);
            this.AddRegexTextBox.Name = "AddRegexTextBox";
            this.AddRegexTextBox.Size = new System.Drawing.Size(289, 20);
            this.AddRegexTextBox.TabIndex = 2;
            // 
            // InverseEvaluationCheckBox
            // 
            this.InverseEvaluationCheckBox.AutoSize = true;
            this.InverseEvaluationCheckBox.Location = new System.Drawing.Point(827, 435);
            this.InverseEvaluationCheckBox.Name = "InverseEvaluationCheckBox";
            this.InverseEvaluationCheckBox.Size = new System.Drawing.Size(151, 17);
            this.InverseEvaluationCheckBox.TabIndex = 3;
            this.InverseEvaluationCheckBox.Text = "Perform inverse evaluation";
            this.InverseEvaluationCheckBox.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(903, 458);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(903, 487);
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
            this.label1.Location = new System.Drawing.Point(60, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Your regular expression:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(43, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(554, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "/";
            // 
            // RegexTb
            // 
            this.RegexTb.Location = new System.Drawing.Point(63, 25);
            this.RegexTb.Name = "RegexTb";
            this.RegexTb.Size = new System.Drawing.Size(485, 20);
            this.RegexTb.TabIndex = 9;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(574, 25);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Your test string:";
            // 
            // InputTb
            // 
            this.InputTb.Location = new System.Drawing.Point(63, 73);
            this.InputTb.Multiline = true;
            this.InputTb.Name = "InputTb";
            this.InputTb.Size = new System.Drawing.Size(308, 131);
            this.InputTb.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(433, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Match result:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(645, 332);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MatchRtb
            // 
            this.MatchRtb.Location = new System.Drawing.Point(436, 73);
            this.MatchRtb.Name = "MatchRtb";
            this.MatchRtb.Size = new System.Drawing.Size(171, 56);
            this.MatchRtb.TabIndex = 16;
            this.MatchRtb.Text = "";
            // 
            // GroupsRtb
            // 
            this.GroupsRtb.Location = new System.Drawing.Point(436, 136);
            this.GroupsRtb.Name = "GroupsRtb";
            this.GroupsRtb.Size = new System.Drawing.Size(171, 109);
            this.GroupsRtb.TabIndex = 17;
            this.GroupsRtb.Text = "";
            // 
            // RegexBuilderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 522);
            this.Controls.Add(this.GroupsRtb);
            this.Controls.Add(this.MatchRtb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.InputTb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.RegexTb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RegexTb;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox InputTb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox MatchRtb;
        private System.Windows.Forms.RichTextBox GroupsRtb;
    }
}
namespace DEiXTo.Views
{
    partial class MainWindow
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
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.AgentsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newAgentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floatAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateBrowserVersionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResetToDefaultMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TopMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopMenu
            // 
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AgentsMenuItem,
            this.WindowsMenuItem,
            this.SettingsMenuItem});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.MdiWindowListItem = this.WindowsMenuItem;
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(992, 24);
            this.TopMenu.TabIndex = 1;
            this.TopMenu.Text = "menuStrip1";
            // 
            // AgentsMenuItem
            // 
            this.AgentsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newAgentToolStripMenuItem});
            this.AgentsMenuItem.Name = "AgentsMenuItem";
            this.AgentsMenuItem.Size = new System.Drawing.Size(56, 20);
            this.AgentsMenuItem.Text = "Agents";
            // 
            // newAgentToolStripMenuItem
            // 
            this.newAgentToolStripMenuItem.Name = "newAgentToolStripMenuItem";
            this.newAgentToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.newAgentToolStripMenuItem.Text = "New Agent";
            this.newAgentToolStripMenuItem.Click += new System.EventHandler(this.newAgentToolStripMenuItem_Click);
            // 
            // WindowsMenuItem
            // 
            this.WindowsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.closeAllToolStripMenuItem,
            this.floatAllToolStripMenuItem});
            this.WindowsMenuItem.Name = "WindowsMenuItem";
            this.WindowsMenuItem.Size = new System.Drawing.Size(68, 20);
            this.WindowsMenuItem.Text = "&Windows";
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.cascadeToolStripMenuItem.Text = "Cascade";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.cascadeToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // floatAllToolStripMenuItem
            // 
            this.floatAllToolStripMenuItem.Name = "floatAllToolStripMenuItem";
            this.floatAllToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.floatAllToolStripMenuItem.Text = "Float All";
            this.floatAllToolStripMenuItem.Click += new System.EventHandler(this.floatAllToolStripMenuItem_Click);
            // 
            // SettingsMenuItem
            // 
            this.SettingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateBrowserVersionMenuItem,
            this.ResetToDefaultMenuItem});
            this.SettingsMenuItem.Name = "SettingsMenuItem";
            this.SettingsMenuItem.Size = new System.Drawing.Size(61, 20);
            this.SettingsMenuItem.Text = "Settings";
            // 
            // UpdateBrowserVersionMenuItem
            // 
            this.UpdateBrowserVersionMenuItem.Name = "UpdateBrowserVersionMenuItem";
            this.UpdateBrowserVersionMenuItem.Size = new System.Drawing.Size(198, 22);
            this.UpdateBrowserVersionMenuItem.Text = "Update browser version";
            this.UpdateBrowserVersionMenuItem.Click += new System.EventHandler(this.UpdateBrowserVersionMenuItem_Click);
            // 
            // ResetToDefaultMenuItem
            // 
            this.ResetToDefaultMenuItem.Name = "ResetToDefaultMenuItem";
            this.ResetToDefaultMenuItem.Size = new System.Drawing.Size(198, 22);
            this.ResetToDefaultMenuItem.Text = "Reset to default";
            this.ResetToDefaultMenuItem.Click += new System.EventHandler(this.ResetToDefaultMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 602);
            this.Controls.Add(this.TopMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.TopMenu;
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip TopMenu;
        private System.Windows.Forms.ToolStripMenuItem AgentsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newAgentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem floatAllToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem WindowsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateBrowserVersionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResetToDefaultMenuItem;
    }
}
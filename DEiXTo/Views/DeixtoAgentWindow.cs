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
    public partial class DeixtoAgentWindow : Form, IDeixtoAgentView
    {
        public event Action BrowseToUrl;

        public DeixtoAgentWindow()
        {
            InitializeComponent();
            DeixtoAgentTooltip.SetToolTip(this.URLComboBox, "Please specify URL to navigate");
            DeixtoAgentTooltip.SetToolTip(this.SilentlyCheckBox, "If checked, JavaScript errors are supressed.");
            DeixtoAgentTooltip.SetToolTip(this.HighlightModeCheckBox, "If checked, HTML elements are highlighted when mouse passes over them");
            DeixtoAgentTooltip.SetToolTip(this.RebuildDOMButton, "Rebuilds DOM Tree. Useful when DOM is altered by AJAX calls.");
            DeixtoAgentTooltip.SetToolTip(this.SimplifyDOMButton, "Simplify DOM Tree");
            DeixtoAgentTooltip.SetToolTip(this.StopExecutionButton, "Stop execution");
            DeixtoAgentTooltip.SetToolTip(this.CreateSnapshotButton, "Create Snapshot");
            DeixtoAgentTooltip.SetToolTip(this.ClearTreeViewsButton, "Clear All TreeViews");
            DeixtoAgentTooltip.SetToolTip(this.SavePatternButton, "Save record instance as pattern...");
            DeixtoAgentTooltip.SetToolTip(this.LevelUpButton, "Up one level");
            DeixtoAgentTooltip.SetToolTip(this.LevelDownButton, "Down one level");
            DeixtoAgentTooltip.SetToolTip(this.ExecuteButton, "Execute Rule");
            DeixtoAgentTooltip.SetToolTip(this.TargetURLSTextBox, "Target url container - double click to open site");
            DeixtoAgentTooltip.SetToolTip(this.AddURLTextBox, "Use this to add a URL to the list");
            DeixtoAgentTooltip.SetToolTip(this.URLsFileTextBox, "Use this to add URLs from file");
            DeixtoAgentTooltip.SetToolTip(this.AddURLButton, "Add to list");
            DeixtoAgentTooltip.SetToolTip(this.SelectOutputFileButton, "Select output file");
            DeixtoAgentTooltip.SetToolTip(this.DelayNUD, "Delay in sec between successive http calls");
            DeixtoAgentTooltip.SetToolTip(this.RemoveURLButton, "Delete selected");
            DeixtoAgentTooltip.SetToolTip(this.BrowseURLsFileButton, "Define file with additional URLs");
            DeixtoAgentTooltip.SetToolTip(this.IgnoreHTMLTagsGroupBox, "Select tags to ignore and press the 'Simplify' button above");
            DeixtoAgentTooltip.SetToolTip(this.TunePatternButton, "Create a working pattern");
            DeixtoAgentTooltip.SetToolTip(this.LoadPatternButton, "Load a previously saved pattern...");
            DeixtoAgentTooltip.SetToolTip(this.AutoFillCheckBox, "Check this to automatically submit a search form");
            DeixtoAgentTooltip.SetToolTip(this.OutputFileNameTextBox, "Use this to give output filename");
            DeixtoAgentTooltip.SetToolTip(this.OutputFileFormatComboBox, "Select output format");
            DeixtoAgentTooltip.SetToolTip(this.BrowseURLsFileButton, "Define file with additional URLs");
            DeixtoAgentTooltip.SetToolTip(this.OutputModeGroupBox, "Output file mode");
            DeixtoAgentTooltip.SetToolTip(this.HitsNUD, "Considered only if > 0");
            DeixtoAgentTooltip.SetToolTip(this.CrawlingCheckBox, "Check this to handle records that span in many pages");
            DeixtoAgentTooltip.SetToolTip(this.GoButton, "Run in auto mode");
            DeixtoAgentTooltip.SetToolTip(this.OpenButton, "Open Project File");
            DeixtoAgentTooltip.SetToolTip(this.SaveButton, "Open Project File");
        }

        public string Url
        {
            get { return URLComboBox.Text; }
        }

        public void ShowWarningMessage()
        {
            MessageBox.Show("Please specify URL!");
        }

        public void NavigateTo(string url)
        {
            WebBrowser.Navigate(url);
        }

        private void BrowseToURLButton_Click(object sender, EventArgs e)
        {
            if (BrowseToUrl != null)
            {
                BrowseToUrl();
            }
        }
    }
}

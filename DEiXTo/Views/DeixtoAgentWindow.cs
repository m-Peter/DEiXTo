using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DEiXTo.Services;

namespace DEiXTo.Views
{
    public partial class DeixtoAgentWindow : Form, IDeixtoAgentView
    {
        // Fires when the user clicks the Browse button
        public event Action BrowseToUrl;
        // Fires when the user enters a keyboard key
        public event Action<KeyEventArgs> KeyDownPress;
        // Fires when the user checks the Auto Fill Check Box
        public event Action<Boolean> AutoFillChanged;
        // Fires when the user checks the Crawling Check Box
        public event Action<Boolean> CrawlingChanged;
        // Fires when the Document specified by the URL has completed downloading
        public event Action BrowserCompleted;
        // Fires when the user's mouse passes over an element in WebBrowser's Document
        public event Action<HtmlElement> DocumentMouseOver;
        // Fires when the user's mouse leaves an element of WebBrowser's Document
        public event Action<HtmlElement> DocumentMouseLeave;
        // Fires when the user clicks a Node of the DOM Tree
        public event Action<int> DOMNodeClick;

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
            DeixtoAgentTooltip.SetToolTip(this.TargetURLsListBox, "Target url container - double click to open site");
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

            this.KeyPreview = true;
            this.KeyDown += DeixtoAgentWindow_KeyDown;

            OutputFileFormatComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get the URL specified by the user
        /// </summary>
        public string Url
        {
            get { return URLComboBox.Text; }
        }

        /// <summary>
        /// Shows a warning message when the URL is empty
        /// </summary>
        public void ShowWarningMessage()
        {
            MessageBox.Show("Please specify URL!");
        }

        /// <summary>
        /// Returns whether the Highlight Mode checkbox is enabled.
        /// </summary>
        /// <returns></returns>
        public bool HighlightModeEnabled()
        {
            return HighlightModeCheckBox.Checked;
        }

        /// <summary>
        /// Returns whether the AutoScroll option is enabled.
        /// </summary>
        /// <returns></returns>
        public bool CanAutoScroll()
        {
            return AutoScrollCheckBox.Checked;
        }

        /// <summary>
        /// Fill the element info tab page with the element's information.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="path"></param>
        public void FillElementInfo(HtmlElement element, string path)
        {
            OuterHtmlTextBox.Text = element.OuterHtml;
            InnerTextTextBox.Text = element.InnerText;
            HtmlPathTextBox.Text = path;
        }

        /// <summary>
        /// Clear the element info tab page.
        /// </summary>
        public void ClearElementInfo()
        {
            OuterHtmlTextBox.Text = String.Empty;
            InnerTextTextBox.Text = String.Empty;
            HtmlPathTextBox.Text = String.Empty;
        }

        /// <summary>
        /// Navigates the WebBrowser to the given URL
        /// </summary>
        /// <param name="url"></param>
        public void NavigateTo(string url)
        {
            WebBrowser.Navigate(url);
        }

        /// <summary>
        /// Navigates the WebBrowser to the next URL
        /// </summary>
        public void NavigateForward()
        {
            WebBrowser.GoForward();
        }

        /// <summary>
        /// Navigates the WebBrowser to the previous URL
        /// </summary>
        public void NavigateBack()
        {
            WebBrowser.GoBack();
        }

        /// <summary>
        /// Changes the visibility of the AutoFill fields according
        /// to the CheckBox state
        /// </summary>
        /// <param name="state"></param>
        public void ApplyVisibilityStateInAutoFill(bool state)
        {
            FormNameTextBox.Enabled = state;
            InputNameTextBox.Enabled = state;
            SearchQueryTextBox.Enabled = state;
        }

        /// <summary>
        /// Changes the visibility of the Crawling fields according
        /// to the CheckBox state
        /// </summary>
        /// <param name="state"></param>
        public void ApplyVisibilityStateInCrawling(bool state)
        {
            CrawlingDepthNUD.Enabled = state;
            HTMLLinkTextBox.Enabled = state;
        }

        /// <summary>
        /// Gets the HtmlDocument of the current page
        /// </summary>
        /// <returns></returns>
        public HtmlDocument GetHTMLDocument()
        {
            return WebBrowser.Document;
        }

        /// <summary>
        /// Fill the DOM TreeView with the given node
        /// </summary>
        /// <param name="node"></param>
        public void FillDomTree(TreeNode node)
        {
            HtmlTreeView.BeginUpdate();
            HtmlTreeView.Nodes.Add(node);
            HtmlTreeView.ExpandAll();
            HtmlTreeView.EndUpdate();
        }

        /// <summary>
        /// Append the given URL to the collection of TargetURLs
        /// </summary>
        /// <param name="url"></param>
        public void AppendTargetUrl(string url)
        {
            TargetURLsListBox.Items.Clear();
            TargetURLsListBox.Items.Add(url);
        }

        /// <summary>
        /// Select the given TreeNode and scroll the TreeView to its
        /// position
        /// </summary>
        /// <param name="node"></param>
        public void SelectDOMNode(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

            node.EnsureVisible();
            HtmlTreeView.SelectedNode = node;
            HtmlTreeView.Focus();
        }

        private void BrowseToURLButton_Click(object sender, EventArgs e)
        {
            if (BrowseToUrl != null)
            {
                BrowseToUrl();
            }
        }

        private void AutoFillCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = AutoFillCheckBox.Checked;
            if (AutoFillChanged != null)
            {
                AutoFillChanged(state);
            }
        }

        private void CrawlingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = CrawlingCheckBox.Checked;
            if (CrawlingChanged != null)
            {
                CrawlingChanged(state);
            }
        }

        private void DeixtoAgentWindow_ClientSizeChanged(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance += 500;
            splitContainer3.SplitterDistance += 200;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (BrowserCompleted != null && e.Url.Equals(WebBrowser.Url))
            {
                HtmlTreeView.BeginUpdate();
                HtmlTreeView.Nodes.Clear();
                HtmlTreeView.EndUpdate();
                WebBrowser.Document.MouseOver += Document_MouseOver;
                WebBrowser.Document.MouseLeave += Document_MouseLeave;
                BrowserCompleted();
            }
        }

        void Document_MouseLeave(object sender, HtmlElementEventArgs e)
        {
            DocumentMouseLeave(e.FromElement);
        }

        void Document_MouseOver(object sender, HtmlElementEventArgs e)
        {
            DocumentMouseOver(e.ToElement);
        }

        void DeixtoAgentWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDownPress != null)
            {
                KeyDownPress(e);
            }
        }

        private void HtmlTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (DOMNodeClick != null)
            {
                DOMNodeClick(e.Node.SourceIndex());
            }
        }
    }
}

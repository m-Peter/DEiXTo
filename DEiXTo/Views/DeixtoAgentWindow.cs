﻿using System;
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
        public event Action<TreeNode, MouseButtons> DOMNodeClick;
        public event Action<TreeNode> CreateWorkingPattern;
        public event Action<TreeNode> CreateAuxiliaryPattern;
        public event Action<TreeNode, MouseButtons> WorkingPatternNodeClick;
        public event Action<HtmlElement> CreateWorkingPatternFromDocument;
        public event Action<HtmlElement> CreateAuxiliaryPatternFromDocument;
        public event HtmlElementEventHandler ShowBrowserContextMenu;
        public event Action<TreeNode> AuxiliaryPatternNodeClick;
        public event Action SimplifyDOMTree;
        public event Action<TreeNode> CreateSnapshot;
        public event Action<TreeNode> MakeWorkingPatternFromSnapshot;
        public event Action<TreeNode> DeleteSnapshot;
        public event Action<int> ClearTreeViews;
        public event Action RebuildDOM;
        public event Action ExecuteRule;
        
        private HtmlElement _currentElement;

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
            get { return "http://" + URLComboBox.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateDocumentUrl()
        {
            URLComboBox.Text = WebBrowser.Document.Url.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public HtmlElement CurrentElement
        {
            get { return _currentElement; }
            set { _currentElement = value; }
        }

        /// <summary>
        /// Shows a warning message when the URL is empty
        /// </summary>
        public void ShowWarningMessage()
        {
            MessageBox.Show("Please specify URL!");
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowRequestNotFoundMessage()
        {
            MessageBox.Show("Request resource is not found", "Microsoft Internet Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowNoTagSelectedMessage()
        {
            MessageBox.Show("At least one HTML tag type should be selected", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        public void FillElementInfo(TreeNode node, string outerHtml)
        {
            OuterHtmlTextBox.Text = outerHtml;
            InnerTextTextBox.Text = node.GetContent();
            HtmlPathTextBox.Text = node.GetPath();
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
        /// 
        /// </summary>
        /// <param name="imageList"></param>
        public void AddWorkingPatterImages(ImageList imageList)
        {
            WorkingPatternTreeView.ImageList = imageList;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearSnapshotTree()
        {
            SnapshotsTreeView.Nodes.Clear();
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
        public HtmlDocument GetHtmlDocument()
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

        /// <summary>
        /// 
        /// </summary>
        public void ClearPatternTree()
        {
            WorkingPatternTreeView.BeginUpdate();
            WorkingPatternTreeView.Nodes.Clear();
            WorkingPatternTreeView.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearAuxiliaryTree()
        {
            AuxiliaryTreeView.BeginUpdate();
            AuxiliaryTreeView.Nodes.Clear();
            AuxiliaryTreeView.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearDOMTree()
        {
            HtmlTreeView.BeginUpdate();
            HtmlTreeView.Nodes.Clear();
            HtmlTreeView.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void FillPatternTree(TreeNode node)
        {
            node.NodeFont = new Font(WorkingPatternTreeView.Font, FontStyle.Bold);
            WorkingPatternTreeView.Nodes.Add(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void FillAuxiliaryTree(TreeNode node)
        {
            AuxiliaryTreeView.Nodes.Add(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void FillSnapshotTree(TreeNode node)
        {
            node.ContextMenuStrip = SnapshotsMenuStrip;
            SnapshotsTreeView.Nodes.Insert(0, node);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExpandPatternTree()
        {
            WorkingPatternTreeView.BeginUpdate();
            WorkingPatternTreeView.ExpandAll();
            WorkingPatternTreeView.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExpandAuxiliaryTree()
        {
            AuxiliaryTreeView.BeginUpdate();
            AuxiliaryTreeView.ExpandAll();
            AuxiliaryTreeView.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void SetContextMenuFor(TreeNode node)
        {
            node.ContextMenuStrip = CreatePatternsMenuStrip;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowBrowserMenu()
        {
            BrowserMenuStrip.Show(Cursor.Position);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool BrowserContextMenuEnabled()
        {
            return BrowserMenuStrip.Enabled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] IgnoredTags()
        {
            var items = HTMLTagsListBox.CheckedItems;
            string[] ignoredTags = new string[items.Count];
            int i = 0;
            foreach (var item in items)
            {
                var tag = item as string;
                ignoredTags[i] = tag;
                i++;
            }

            return ignoredTags;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AttachDocumentEvents()
        {
            WebBrowser.Document.MouseOver += Document_MouseOver;
            WebBrowser.Document.MouseLeave += Document_MouseLeave;
            WebBrowser.Document.ContextMenuShowing += Document_ContextMenuShowing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void DeleteSnapshotInstance(TreeNode node)
        {
            SnapshotsTreeView.Nodes.Remove(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool AskUserToClearTreeViews()
        {
            var result = MessageBox.Show("Are you sure you want to clear the treeviews?", "DEiXTo", MessageBoxButtons.YesNo);

            return result == DialogResult.Yes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreeNode GetWorkingPattern()
        {
            return WorkingPatternTreeView.Nodes[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreeNodeCollection GetDOMTreeNodes()
        {
            return HtmlTreeView.Nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnHeader"></param>
        public void AddOutputColumn(string columnHeader)
        {
            OutputListView.Columns.Add(columnHeader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        public void AddOutputItem(string[] contents)
        {
            ListViewItem item = new ListViewItem(contents);
            OutputListView.Items.Add(item);
        }

        public void SetExtractedResults(int count)
        {
            ExtractionResultsLabel.Text += count;
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
                BrowserCompleted();
            }
        }

        void Document_ContextMenuShowing(object sender, HtmlElementEventArgs e)
        {
            if (ShowBrowserContextMenu != null)
            {
                ShowBrowserContextMenu(sender, e);
            }
        }

        void Document_MouseLeave(object sender, HtmlElementEventArgs e)
        {
            if (DocumentMouseLeave != null)
            {
                DocumentMouseLeave(e.FromElement);
            }
        }

        void Document_MouseOver(object sender, HtmlElementEventArgs e)
        {
            if (DocumentMouseOver != null)
            {
                DocumentMouseOver(e.ToElement);
            }
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
                DOMNodeClick(e.Node, e.Button);
            }
        }

        private void WorkingPatternMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateWorkingPattern != null)
            {
                var node = HtmlTreeView.SelectedNode;
                CreateWorkingPattern(node);
            }
        }

        private void AuxiliaryPatternMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateAuxiliaryPattern != null)
            {
                var node = HtmlTreeView.SelectedNode;
                CreateAuxiliaryPattern(node);
            }
        }

        private void WorkingPatternTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (WorkingPatternNodeClick != null)
            {
                WorkingPatternNodeClick(e.Node, e.Button);
            }
        }

        private void UseAsWorkingPatternMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateWorkingPatternFromDocument != null)
            {
                CreateWorkingPatternFromDocument(CurrentElement);
            }
        }

        private void UseAsAuxiliaryPatternMenuItem_Click(object sender, EventArgs e)
        {
            if (CreateAuxiliaryPatternFromDocument != null)
            {
                CreateAuxiliaryPatternFromDocument(CurrentElement);
            }
        }

        private void AuxiliaryTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (AuxiliaryPatternNodeClick != null)
            {
                AuxiliaryPatternNodeClick(e.Node);
            }
        }

        private void SimplifyDOMButton_Click(object sender, EventArgs e)
        {
            if (SimplifyDOMTree != null)
            {
                SimplifyDOMTree();
            }
        }

        private void CreateSnapshotButton_Click(object sender, EventArgs e)
        {
            if (CreateSnapshot != null)
            {
                var node = WorkingPatternTreeView.Nodes[0];
                CreateSnapshot(node);
            }
        }

        private void MakeItWorkingPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MakeWorkingPatternFromSnapshot != null)
            {
                var node = SnapshotsTreeView.SelectedNode.FirstNode;
                MakeWorkingPatternFromSnapshot(node);
            }
        }

        private void DeleteSnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DeleteSnapshot != null)
            {
                var node = SnapshotsTreeView.SelectedNode;
                DeleteSnapshot(node);
            }
        }

        private void ClearTreeViewsButton_Click(object sender, EventArgs e)
        {
            if (ClearTreeViews != null)
            {
                int count = WorkingPatternTreeView.Nodes.Count + AuxiliaryTreeView.Nodes.Count + SnapshotsTreeView.Nodes.Count;
                ClearTreeViews(count);
            }
        }

        private void RebuildDOMButton_Click(object sender, EventArgs e)
        {
            if (RebuildDOM != null)
            {
                RebuildDOM();
            }
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (ExecuteRule != null)
            {
                ExecuteRule();
            }
        }
    }
}

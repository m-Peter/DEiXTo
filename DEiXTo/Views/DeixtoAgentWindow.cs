﻿using System.Drawing;
using DEiXTo.Services;
using System;
using System.Windows.Forms;
using DEiXTo.Models;
using DEiXTo.Presenters;

namespace DEiXTo.Views
{
    public partial class DeixtoAgentWindow : Form, IDeixtoAgentView
    {

        #region Instance Variables
        private HtmlElement _currentElement;
        #endregion

        #region Constructors
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
            DeixtoAgentTooltip.SetToolTip(this.OpenWrapperButton, "Open Project File");
            DeixtoAgentTooltip.SetToolTip(this.SaveWrapperButton, "Open Project File");
            ListViewItem item1 = new ListViewItem(new string[] { "title", "Channel name", "The name of the channel. It's how people refer to your service. If you have an HTML website that contains the same information as your RSS file, the title of your channel should be the same as the title of your website.", "GoUpstate.com News Headlines" });
            ListViewItem item2 = new ListViewItem(new string[] { "link", "http://mycomputer.mydomain.com", "The URL to the HTML website corresponding to the channel.", "http://goupstate.com" });
            ListViewItem item3 = new ListViewItem(new string[] { "description", "ΔEiXTo generated RSS feed", "Phrase or sentence describing the channel.", "The latest news from GoUpstate.com, a Spartanburn Herald-Journal Web site." });
            ListViewItem item4 = new ListViewItem(new string[] { "language", "en-us", "The language the channel is written in. This allows aggregators to group all Italian language sites, for example, on a single page. A list of allowable values for this element, as provided by Netscape, is here.  You may also use values defined by the W3C.", "en-us" });
            ListViewItem item5 = new ListViewItem(new string[] { "copyright", "Copyright 2007-2008, ΔEiXTo", "Copyright notice for content in the channel.", "Copyright 2002, Spartanburg Herald-Journal" });
            ListViewItem item6 = new ListViewItem(new string[] { "managingEditor", "myaddress@mydomain.com", "Email address for person responsible for editorial content.", "geo@herald.com (George Matesky)" });
            ListViewItem item7 = new ListViewItem(new string[] { "webMaster", "myaddress@mydomain.com", "Email address for person responsible for technical issues relating to channel.", "betty@herald.com (Betty Guernsey" });
            ListViewItem item8 = new ListViewItem(new string[] { "generator", "ΔEiXTo", "A string indicating the program used to generate the channel.", "MightyInHouse Content System v2.3" });
            ListViewItem item9 = new ListViewItem(new string[] { "category", "", "Specify one or more categories that the channel belongs to. Follows the same rules as the <item>-level category element.", "<category>Newspapers</category>" });
            ListViewItem item10 = new ListViewItem(new string[] { "docs", "", "A URL that points to the documentation for the format used in the RSS file. It's probably a pointer to this page. It's for people who might stumble across an RSS file on a Web server 25 years from now and wonder what it is.", "http://blogs.law.harvard.edu/tech/rss" });
            ListViewItem item11 = new ListViewItem(new string[] { "ttl", "", "ttl stands for time to live. It's a number of minutes that indicates how long a channel can be cached before refreshing from the source.", "<ttl>60</ttl>" });
            ListViewItem item12 = new ListViewItem(new string[] { "rating", "", "The PICS rating for the channel.", "<rating>(PICS-1.1 \"http://www.classify.org/safeurf/\" | r (SS~~0001))</rating>" });
            ListViewItem item13 = new ListViewItem(new string[] { "skipHours", "", "A hint for aggregators telling them which hours they can skip.", "<skipHours>19</skipHours>" });
            ListViewItem item14 = new ListViewItem(new string[] { "skipDays", "", "A hint for aggregators telling them which days they can skip.", "<skipDays>Saturday</skipDays>" });
            RSSListView.Items.Add(item1);
            RSSListView.Items.Add(item2);
            RSSListView.Items.Add(item3);
            RSSListView.Items.Add(item4);
            RSSListView.Items.Add(item5);
            RSSListView.Items.Add(item6);
            RSSListView.Items.Add(item7);
            RSSListView.Items.Add(item8);
            RSSListView.Items.Add(item9);
            RSSListView.Items.Add(item10);
            RSSListView.Items.Add(item11);
            RSSListView.Items.Add(item12);
            RSSListView.Items.Add(item13);
            RSSListView.Items.Add(item14);

            this.KeyPreview = true;
            this.KeyDown += DeixtoAgentWindow_KeyDown;

            OutputFileFormatComboBox.Items.AddRange(GetOutputFormats());
            OutputFileFormatComboBox.SelectedIndex = 0;
        }
        #endregion

        #region Public Methods

        public DeixtoWrapper Wrapper
        {
            get
            {
                DeixtoWrapper wrapper = new DeixtoWrapper();

                //wrapper.AutoFill = AutoFillCheckBox.Checked;
                //wrapper.HtmlNextLink = HTMLLinkTextBox.Text;
                //wrapper.Delay = (int)DelayNUD.Value;
                //wrapper.ExtractionPattern = ExtractionPatternTreeView.Nodes[0];
                //wrapper.ExtractNativeUrl = ExtractURLCheckBox.Checked;
                //wrapper.FormName = FormNameTextBox.Text;
                //wrapper.FormInputName = InputNameTextBox.Text;
                //wrapper.FormTerm = SearchQueryTextBox.Text;
                //wrapper.IgnoredTags = IgnoredTags();
                //wrapper.InputFile = URLsFileTextBox.Text;
                //wrapper.MaxCrawlingDepth = CrawlingDepth();
                //wrapper.MultiPageCrawling = CrawlingEnabled;
                //wrapper.NumberOfHits = (int)HitsNUD.Value;
                //wrapper.OutputFileName = OutputFileNameTextBox.Text;
                //wrapper.OutputFormat = OutputFileFormat;
                //wrapper.OutputMode = OverwriteRadioBtn.Checked ? OutputMode.Overwrite : OutputMode.Append;
                //wrapper.TargetUrls = TargetUrls();

                return wrapper;
            }
            set
            {

            }
        }

        public string[] TargetUrls
        {
            get
            {
                var count = TargetURLsListBox.Items.Count;
                string[] targetUrls = new string[count];

                int i = 0;
                foreach (string url in TargetURLsListBox.Items)
                {
                    targetUrls[i] = url;
                    i++;
                }

                return targetUrls;
            }
            set
            {
                TargetURLsListBox.Items.AddRange(value);
            }
        }

        /// <summary>
        /// Collects all the tags from the ListBox that are checked.
        /// </summary>
        /// <returns>An string array with the checked tags.</returns>
        public string[] IgnoredTags
        {
            get
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
            set
            {
                foreach (string tag in value)
                {
                    var index = HTMLTagsListBox.Items.IndexOf(tag);
                    HTMLTagsListBox.SetItemChecked(index, true);
                }
            }
        }

        public OutputMode OutputMode
        {
            get { return OverwriteRadioBtn.Checked ? OutputMode.Overwrite : OutputMode.Append; }
            set
            {
                if (value == global::OutputMode.Overwrite)
                {
                    OverwriteRadioBtn.Checked = true;
                }
                else if (value == global::OutputMode.Append)
                {
                    AppendRadioBtn.Checked = true;
                }
            }
        }

        public Format OutputFormat
        {
            get { return OutputFileFormat; }
            set
            {
                foreach (var item in OutputFileFormatComboBox.Items)
                {
                    var fitem = (OutputFormat)item;
                    if (fitem.Format == value)
                    {
                        OutputFileFormatComboBox.SelectedItem = item;
                    }
                }
            }
        }

        public Format OutputFileFormat
        {
            get
            {
                OutputFormat type = (OutputFormat)OutputFileFormatComboBox.SelectedItem;
                return type.Format;
            }
        }

        public string OutputFileName
        {
            get { return OutputFileNameTextBox.Text; }
            set { OutputFileNameTextBox.Text = value; }
        }

        public int NumberOfHits
        {
            get { return (int)HitsNUD.Value; }
            set { HitsNUD.Value = value; }
        }

        public bool MultiPageCrawling
        {
            get { return CrawlingEnabled; }
            set
            {
                CrawlingCheckBox.Checked = value;
                ApplyVisibilityStateInCrawling(value);
            }
        }

        public int MaxCrawlingDepth
        {
            get { return CrawlingDepth(); }
            set
            {
                CrawlingDepthNUD.Value = value;
            }
        }

        public string InputFile
        {
            get { return URLsFileTextBox.Text; }
            set { URLsFileTextBox.Text = value; }
        }

        public string FormTerm
        {
            get { return SearchQueryTextBox.Text; }
            set { SearchQueryTextBox.Text = value; }
        }

        public string FormInputName
        {
            get { return InputNameTextBox.Text; }
            set { InputNameTextBox.Text = value; }
        }

        public string FormName
        {
            get { return FormNameTextBox.Text; }
            set { FormNameTextBox.Text = value; }
        }

        public bool ExtractNativeUrl
        {
            get { return ExtractURLCheckBox.Checked; }
            set { ExtractURLCheckBox.Checked = value; }
        }

        public TreeNode ExtractionPattern
        {
            get { return ExtractionPatternTreeView.Nodes[0]; }
            set { ExtractionPatternTreeView.Nodes.Add(value); }
        }

        public int Delay
        {
            get { return (int)DelayNUD.Value; }
            set { DelayNUD.Value = value; }
        }

        public bool AutoFill
        {
            get { return AutoFillCheckBox.Checked; }
            set
            {
                AutoFillCheckBox.Checked = value;
                ApplyVisibilityStateInAutoFill(value);
            }
        }

        public string HtmlNextLink
        {
            get { return HTMLLinkTextBox.Text; }
            set { HTMLLinkTextBox.Text = value; }
        }

        /// <summary>
        /// Get the URL specified by the user
        /// </summary>
        public string Url
        {
            get { return URLComboBox.Text; }
        }

        public DeixtoAgentPresenter Presenter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TargetURLsFile
        {
            get { return URLsFileTextBox.Text; }
            set { URLsFileTextBox.Text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExtractionPatternSpecified
        {
            get { return ExtractionPatternTreeView.Nodes.Count > 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TargetURLSpecified
        {
            get { return TargetURLsListBox.Items.Count > 0; }
        }

        /// <summary>
        /// Return the URL of the WebBrowser's HtmlDocument.
        /// </summary>
        /// <returns></returns>
        public String GetDocumentUrl()
        {
            return WebBrowser.Document.Url.ToString();
        }

        /// <summary>
        /// Set the WebBrowser's document Url as the URLComboBox text.
        /// </summary>
        public void UpdateDocumentUrl()
        {
            URLComboBox.Text = WebBrowser.Document.Url.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearExtractedOutputs()
        {
            OutputListView.Clear();
            ExtractedResultsCountLabel.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FocusAuxiliaryTabPage()
        {
            AttributesTabControl.SelectedTab = Auxiliary;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FocusOutputTabPage()
        {
            SettingsTabControl.SelectedTab = Output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="imageIndex"></param>
        public void ApplyStateToNode(TreeNode node, int imageIndex)
        {
            node.ImageIndex = imageIndex;
            node.SelectedImageIndex = imageIndex;
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
        /// Shows a warning message when the URL is empty.
        /// </summary>
        public void ShowSpecifyURLMessage()
        {
            MessageBox.Show("Please specify URL!", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowSpecifyInputSourceMessage()
        {
            MessageBox.Show("Please specify target URLs or input file.", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ShowSelectOneInputSourceMessage()
        {
            MessageBox.Show("Please select just one input source", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowSpecifyExtractionPatternMessage()
        {
            MessageBox.Show("Please specify extraction pattern.", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowSpecifyTargetURLMessage()
        {
            MessageBox.Show("Please specify a URL", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowSpecifyPatternMessage()
        {
            MessageBox.Show("Please specify pattern to search for.", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowEmptyLinkMessage()
        {
            MessageBox.Show("Please specify text contained by link.", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowInvalidDepthMessage()
        {
            MessageBox.Show("Max crawl depth is a positive integer!", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows an error message when the URL is not found.
        /// </summary>
        public void ShowRequestNotFoundMessage()
        {
            MessageBox.Show("Request resource is not found", "Microsoft Internet Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowEnterURLToAddMessage()
        {
            MessageBox.Show("Please enter URL to add.", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowSelectURLMessage()
        {
            MessageBox.Show("Please select URL entry to remove.", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows a warning message when no tag is selected from the ListBox.
        /// </summary>
        public void ShowNoTagSelectedMessage()
        {
            MessageBox.Show("At least one HTML tag type should be selected", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowCannotDeleteRootMessage()
        {
            MessageBox.Show("Cannot remove the root of the pattern", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Returns whether the Highlight Mode checkbox is enabled.
        /// </summary>
        /// <returns></returns>
        public bool HighlightModeEnabled
        {
            get { return HighlightModeCheckBox.Checked; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CrawlingEnabled
        {
            get { return CrawlingCheckBox.Checked; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CrawlingDepth()
        {
            int depth = (int)CrawlingDepthNUD.Value;
            return depth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string HtmlLink()
        {
            return HTMLLinkTextBox.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearAddURLInput()
        {
            AddURLTextBox.Clear();
        }

        /// <summary>
        /// Returns whether the AutoScroll option is enabled.
        /// </summary>
        /// <returns></returns>
        public bool CanAutoScroll
        {
            get { return AutoScrollCheckBox.Checked; }
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
            RegularExpressionTextBox.Text = node.GetRegex();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void FillTextNodeElementInfo(TreeNode node)
        {
            OuterHtmlTextBox.Text = "This is an artificial text node.";
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
        public void AddWorkingPatternImages(ImageList imageList)
        {
            WorkingPatternTreeView.ImageList = imageList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageList"></param>
        public void AddExtractionTreeImages(ImageList imageList)
        {
            ExtractionPatternTreeView.ImageList = imageList;
        }

        /// <summary>
        /// Clear the TreeNodes of the Snapshot TreeView.
        /// </summary>
        public void ClearSnapshotTree()
        {
            SnapshotsTreeView.Nodes.Clear();
        }

        /// <summary>
        /// Navigates the WebBrowser to the given URL.
        /// </summary>
        /// <param name="url"></param>
        public void NavigateTo(string url)
        {
            WebBrowser.Navigate(url);
            while (WebBrowser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Navigates forward the WebBrowser's document.
        /// </summary>
        public void NavigateForward()
        {
            WebBrowser.GoForward();
        }

        /// <summary>
        /// Navigates back the WebBrowser's document.
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
        /// Retrieve the HtmlDocument of the current page.
        /// </summary>
        /// <returns></returns>
        public HtmlDocument GetHtmlDocument()
        {
            return WebBrowser.Document;
        }

        /// <summary>
        /// Fill the Html TreeView with the given node.
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
        /// Append the given URL to the collection of TargetURLs.
        /// </summary>
        /// <param name="url"></param>
        public void AppendTargetUrl(string url)
        {
            TargetURLsListBox.Items.Add(url);
        }

        public void AppendTargetUrls(string[] urls)
        {
            TargetURLsListBox.Items.AddRange(urls);
        }

        

        /// <summary>
        /// Clear all the URLs from the TargetURLs collection.
        /// </summary>
        public void ClearTargetURLs()
        {
            TargetURLsListBox.Items.Clear();
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
        /// Clear the TreeNodes of the WorkingPattern TreeView.
        /// </summary>
        public void ClearPatternTree()
        {
            WorkingPatternTreeView.BeginUpdate();
            WorkingPatternTreeView.Nodes.Clear();
            WorkingPatternTreeView.EndUpdate();
        }

        /// <summary>
        /// Clear the TreeNodes of the Auxiliary TreeView.
        /// </summary>
        public void ClearAuxiliaryTree()
        {
            AuxiliaryTreeView.BeginUpdate();
            AuxiliaryTreeView.Nodes.Clear();
            AuxiliaryTreeView.EndUpdate();
        }

        /// <summary>
        /// Clear the TreeNodes of the Html TreeView.
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
            WorkingPatternTreeView.Nodes.Add(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void SetNodeFont(TreeNode node)
        {
            node.NodeFont = new Font(WorkingPatternTreeView.Font, FontStyle.Bold);
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
        /// <param name="node"></param>
        public void SetAdjustContextMenuFor(TreeNode node)
        {
            node.ContextMenuStrip = AdjustpatternMenuStrip;

            EnterFSONMenuItem.Enabled = false;
            AddPreviousSiblingMenuItem.Enabled = true;
            AddNextSiblingMenuItem.Enabled = true;
            SetAsRootMenuItem.Enabled = false;
            RemoveLabelMenuItem.Enabled = node.HasLabel();
            RemoveRegExMenuItem.Enabled = node.HasRegex();

            if (node.IsRoot())
            {
                UncheckedMenuItem.Enabled = false;
                GrayedImpliedMenuItem.Enabled = false;
                CheckedImpliedMenuItem.Enabled = false;
                DeleteNodeMenuItem.Enabled = false;
                return;
            }
            else
            {
                CheckedMenuItem.Enabled = true;
                CheckedSourceMenuItem.Enabled = true;
                GrayedMenuItem.Enabled = true;
                UncheckedMenuItem.Enabled = true;
                GrayedImpliedMenuItem.Enabled = true;
                CheckedImpliedMenuItem.Enabled = true;
                DeleteNodeMenuItem.Enabled = true;
            }

            if (node.CanBeVRoot())
            {
                SetAsRootMenuItem.Enabled = true;
            }

            if (node.IsTextNode())
            {
                CheckedSourceMenuItem.Enabled = false;
                DeleteNodeMenuItem.Enabled = true;

                if (node.CanBeVRoot())
                {
                    SetAsRootMenuItem.Enabled = true;
                }
            }
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
        public bool BrowserContextMenuEnabled
        {
            get { return BrowserMenuStrip.Enabled; }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void SetURLInput(string url)
        {
            AddURLTextBox.Text = url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void RemoveTargetURL(string url)
        {
            TargetURLsListBox.Items.Remove(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string TargetURLToAdd()
        {
            return AddURLTextBox.Text;
        }

        /// <summary>
        /// Attach the necessary events to the WebBrowser's document.
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
        public bool AskUserToRemoveURL()
        {
            var result = MessageBox.Show("Are you sure you want to remove the selected URL(s)?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            return result == DialogResult.Yes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreeNode GetWorkingPattern()
        {
            if (WorkingPatternTreeView.GetNodeCount(false) > 0)
            {
                return WorkingPatternTreeView.Nodes[0];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreeNode GetAuxiliaryInstance()
        {
            return AuxiliaryTreeView.Nodes[0];
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
        /// <param name="node"></param>
        public void FillExtractionPattern(TreeNode node)
        {
            ExtractionPatternTreeView.Nodes.Add(node);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExpandExtractionTree()
        {
            ExtractionPatternTreeView.BeginUpdate();
            ExtractionPatternTreeView.ExpandAll();
            ExtractionPatternTreeView.EndUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreeNodeCollection GetBodyTreeNodes()
        {
            return HtmlTreeView.Nodes[0].Nodes[1].Nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void DeletePatternNode(TreeNode node)
        {
            WorkingPatternTreeView.Nodes.Remove(node);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool OutputFileSpecified
        {
            get
            {
                string outputFile = OutputFileNameTextBox.Text;
                return !String.IsNullOrWhiteSpace(outputFile);
            }
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
        public void AddOutputItem(string[] contents, TreeNode node)
        {
            ListViewItem item = new ListViewItem(contents);
            item.Tag = node;
            OutputListView.Items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public void WritePageResults(string message)
        {
            ExtractionResultsLabel.Enabled = true;
            ExtractionResultsLabel.Text = message;
        }

        /// <summary>
        /// 
        /// </summary>
        public string FirstTargetURL
        {
            get
            {
                var url = (string)TargetURLsListBox.Items[0];
                return url;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreeNodeCollection GetPatternTreeNodes()
        {
            return WorkingPatternTreeView.Nodes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TreeNode GetExtractionPattern()
        {
            return ExtractionPatternTreeView.Nodes[0];
        }

        public TreeNodeCollection GetExtractionPatternNodes()
        {
            return ExtractionPatternTreeView.Nodes;
        }
        #endregion

        #region Private Methods
        private object[] GetOutputFormats()
        {
            return new object[]
            {
                new OutputFormat(Format.Text, "Text (*.txt)"),
                new OutputFormat(Format.XML, "XML (*.xml)"),
                new OutputFormat(Format.RSS, "RSS (*.rss)")
            };
        }
        #endregion

        #region Private Events
        private void BrowseToURLButton_Click(object sender, EventArgs e)
        {
            Presenter.BrowseToUrl();
        }

        private void AutoFillCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = AutoFillCheckBox.Checked;
            Presenter.AutoFillChanged(state);
        }

        private void CrawlingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = CrawlingCheckBox.Checked;
            Presenter.CrawlingChanged(state);
        }

        private void DeixtoAgentWindow_ClientSizeChanged(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance += 500;
            splitContainer3.SplitterDistance += 200;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.Equals(WebBrowser.Url))
            {
                Presenter.BrowserCompleted();
            }
        }

        void Document_ContextMenuShowing(object sender, HtmlElementEventArgs e)
        {
            Presenter.ShowBrowserContextMenu(sender, e);
        }

        void Document_MouseLeave(object sender, HtmlElementEventArgs e)
        {
            Presenter.DocumentMouseLeave(e.FromElement);
        }

        void Document_MouseOver(object sender, HtmlElementEventArgs e)
        {
            Presenter.DocumentMouseOver(e.ToElement);
        }

        void DeixtoAgentWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Presenter.KeyDownPress(e);
        }

        private void HtmlTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Presenter.DOMNodeClick(e.Node, e.Button);
        }

        private void WorkingPatternMenuItem_Click(object sender, EventArgs e)
        {
            var node = HtmlTreeView.SelectedNode;
            Presenter.CreateWorkingPattern(node);
        }

        private void AuxiliaryPatternMenuItem_Click(object sender, EventArgs e)
        {
            var node = HtmlTreeView.SelectedNode;
            Presenter.CreateAuxiliaryPattern(node);
        }

        private void WorkingPatternTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Presenter.WorkingPatternNodeClick(e.Node, e.Button);
        }

        private void UseAsWorkingPatternMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.CreateWorkingPatternFromDocument(CurrentElement);
        }

        private void UseAsAuxiliaryPatternMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.CreateAuxiliaryPatternFromDocument(CurrentElement);
        }

        private void AuxiliaryTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Presenter.AuxiliaryPatternNodeClick(e.Node);
        }

        private void SimplifyDOMButton_Click(object sender, EventArgs e)
        {
            Presenter.SimplifyDOMTree();
        }

        private void CreateSnapshotButton_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.Nodes[0];
            Presenter.CreateSnapshot(node);
        }

        private void MakeItWorkingPatternToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = SnapshotsTreeView.SelectedNode.FirstNode;
            Presenter.MakeWorkingPatternFromSnapshot(node);
        }

        private void DeleteSnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = SnapshotsTreeView.SelectedNode;
            Presenter.DeleteSnapshot(node);
        }

        private void ClearTreeViewsButton_Click(object sender, EventArgs e)
        {
            int count = WorkingPatternTreeView.Nodes.Count + AuxiliaryTreeView.Nodes.Count + SnapshotsTreeView.Nodes.Count;
            Presenter.ClearTreeViews(count);
        }

        private void RebuildDOMButton_Click(object sender, EventArgs e)
        {
            Presenter.RebuildDOM();
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            Presenter.ExecuteRule();
        }

        private void LevelUpButton_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.Nodes[0];
            Presenter.LevelUpWorkingPattern(node);
        }

        private void LevelDownButton_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.Nodes[0];
            Presenter.LevelDownWorkingPattern(node);
        }

        private void matchAndExtractContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.NodeStateChanged(node, NodeState.Checked);
        }

        private void matchAndExtractSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.NodeStateChanged(node, NodeState.CheckedSource);
        }

        private void matchNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.NodeStateChanged(node, NodeState.Grayed);
        }

        private void dontCareAboutThisNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.NodeStateChanged(node, NodeState.Unchecked);
        }

        private void matchNodeOPTIONALToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.NodeStateChanged(node, NodeState.GrayedImplied);
        }

        private void matchAndExtractOPTIONALToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.NodeStateChanged(node, NodeState.CheckedImplied);
        }

        private void OutputListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var node = e.Item.Tag as TreeNode;
            bool selected = e.IsSelected;
            Presenter.OutputResultSelected(selected, node);
        }

        private void NewLabelMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.AddNewLabel(node);
        }

        private void AddRegexMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.AddRegex(node);
        }

        private void DeixtoAgentWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Presenter.windowClosing();
        }

        private void RemoveLabelMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.RemoveLabel(node);
        }

        private void RemoveRegExMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.RemoveRegex(node);
        }

        private void AddURLButton_Click(object sender, EventArgs e)
        {
            Presenter.AddURLToTargetURLs();
        }

        private void RemoveURLButton_Click(object sender, EventArgs e)
        {
            Presenter.RemoveURLFromTargetURLs();
        }

        private void TargetURLsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TargetURLsListBox.SelectedItem == null)
            {
                return;
            }

            string selectedURL = TargetURLsListBox.SelectedItem.ToString();
            Presenter.TargetURLSelected(selectedURL);
        }

        private void RSSListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            bool selected = e.IsSelected;

            if (!selected)
            {
                return;
            }

            textBox1.Text = e.Item.SubItems[1].Text;
        }

        private void SavePatternButton_Click(object sender, EventArgs e)
        {
            Presenter.SaveExtractionPattern();
        }

        private void LoadPatternButton_Click(object sender, EventArgs e)
        {
            Presenter.LoadExtractionPattern();
        }

        private void DeleteNodeMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.DeleteNode(node);
        }

        private void AddPreviousSiblingMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.AddPreviousSibling(node);
        }

        private void AddNextSiblingMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.AddNextSibling(node);
        }

        private void EnterSiblingOrderMenuItem_Click(object sender, EventArgs e)
        {
            var node = WorkingPatternTreeView.SelectedNode;
            Presenter.AddSiblingOrder(node);
        }

        private void SaveToDiskButton_Click(object sender, EventArgs e)
        {
            Presenter.SaveToDisk();
        }

        private void SelectOutputFileButton_Click(object sender, EventArgs e)
        {
            Presenter.SelectOutputFile();
        }

        private void TunePatternButton_Click(object sender, EventArgs e)
        {
            Presenter.TunePattern();
        }

        private void BrowseURLsFileButton_Click(object sender, EventArgs e)
        {
            Presenter.LoadURLsFromFile();
        }
        #endregion

        private void SaveWrapperButton_Click(object sender, EventArgs e)
        {
            Presenter.SaveWrapper();
        }

        private void OpenWrapperButton_Click(object sender, EventArgs e)
        {
            Presenter.LoadWrapper();
        }
    }
}

using DEiXTo.Views;
using System;
using System.Linq;
using System.Windows.Forms;
using mshtml;
using DEiXTo.Services;
using DEiXTo.Models;
using System.Drawing;
using System.Collections.Generic;

namespace DEiXTo.Presenters
{

    public delegate void DOMBuilted();

    /// <summary>
    /// 
    /// </summary>
    public class DeixtoAgentPresenter : ISubscriber<RegexAdded>
    {
        #region Instance Variables
        //private readonly IDeixtoAgentView _view;
        private ElementStyling _styling;
        private TreeBuilder _builder;
        private DocumentQuery _document;
        private StatesImageLoader _imageLoader;
        private DOMTreeStructure _domTree;
        private IViewLoader _loader;
        private EventHub _eventHub;
        private ISaveFileDialog _saveFileDialog;
        private IOpenFileDialog _openFileDialog;
        private PatternExtraction _executor;
        #endregion

        #region Public Events
        public event DOMBuilted DomBuilted;
        #endregion

        public IDeixtoAgentView View { get; set; }

        #region Constructors
        public DeixtoAgentPresenter(IDeixtoAgentView view, ISaveFileDialog saveFileDialog)
        {
            View = view;
            View.Presenter = this;
            _styling = new ElementStyling();
            _builder = new TreeBuilder();
            _imageLoader = new StatesImageLoader();
            _loader = new WindowsViewLoader();
            _saveFileDialog = saveFileDialog;
            _openFileDialog = new OpenFileDialogWrapper();
            _eventHub = EventHub.Instance;

            // ATTACH THE EVENTS OF THE VIEW TO LOCAL METHODS
            View.BrowseToUrl += browseToUrl;
            View.KeyDownPress += keyDownPress;
            View.AutoFillChanged += autoFillChanged;
            View.CrawlingChanged += crawlingChanged;
            View.BrowserCompleted += browserCompleted;
            View.DocumentMouseOver += documentMouseOver;
            View.DocumentMouseLeave += documentMouseLeave;
            View.DOMNodeClick += DOMNodeClick;
            View.CreateWorkingPattern += createWorkingPattern;
            View.CreateAuxiliaryPattern += createAuxiliaryPattern;
            View.WorkingPatternNodeClick += workingPatternNodeClick;
            View.CreateWorkingPatternFromDocument += createWorkingPatternFromDocument;
            View.CreateAuxiliaryPatternFromDocument += createAuxiliaryPatternFromDocument;
            View.ShowBrowserContextMenu += showBrowserContextMenu;
            View.AuxiliaryPatternNodeClick += auxiliaryPatternNodeClick;
            View.SimplifyDOMTree += simplifyDOMTree;
            View.CreateSnapshot += createSnapshot;
            View.DeleteSnapshot += deleteSnapshot;
            View.MakeWorkingPatternFromSnapshot += makeWorkingPatternFromSnapshot;
            View.ClearTreeViews += clearTreeViews;
            View.RebuildDOM += rebuildDOM;
            View.ExecuteRule += executeRule;
            View.LevelUpWorkingPattern += levelUpWorkingPattern;
            View.LevelDownWorkingPattern += levelDownWorkingPattern;
            View.NodeStateChanged += nodeStateChanged;
            View.OutputResultSelected += outputResultSelected;
            View.AddNewLabel += addNewLabel;
            View.AddRegex += addRegex;
            View.RemoveLabel += removeLabel;
            View.RemoveRegex += removeRegex;
            View.WindowClosing += windowClosing;
            View.AddURLToTargetURLs += addURLToTargetURLs;
            View.RemoveURLFromTargetURLs += removeURLFromTargetURLs;
            View.TargetURLSelected += targetURLSelected;
            View.SaveExtractionPattern += saveExtractionPattern;
            View.LoadExtractionPattern += loadExtractionPattern;
            View.DeleteNode += deleteNode;
            View.AddPreviousSibling += addPreviousSibling;
            View.AddNextSibling += addNextSibling;
            View.AddSiblingOrder += addSiblingOrder;
            View.SaveToDisk += saveToDisk;
            View.TunePattern += tunePattern;
            View.LoadURLsFromFile += loadURLsFromFile;

            _eventHub.Subscribe<RegexAdded>(this);

            var imagesList = _imageLoader.LoadImages();
            View.AddWorkingPatternImages(imagesList);
            View.AddExtractionTreeImages(imagesList);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        public void Receive(RegexAdded subject)
        {
            TreeNode node = subject.Node;
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            View.FillElementInfo(node, element.OuterHtml);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="mylink"></param>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        private void CrawlPages(int depth, string mylink, TreeNode pattern, ref int count)
        {
            for (int i = 0; i < depth; i++)
            {
                FollowNextLink(mylink);

                var executor = CreateExecutor(pattern);

                executor.FindMatches();

                count += executor.Count;

                AddOutputColumns(executor);

                AddOutputResults(executor);

                View.WritePageResults(count.ToString() + " results!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mylink"></param>
        private void FollowNextLink(string mylink)
        {
            var elem = _document.GetLinkToFollow(mylink);
            string href = elem.GetAttribute("href");
            View.NavigateTo(href);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executor"></param>
        private void AddOutputResults(PatternExtraction executor)
        {
            foreach (var item in executor.ExtractedResults())
            {
                View.AddOutputItem(item.ToStringArray(), item.Node);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executor"></param>
        private void AddOutputColumns(PatternExtraction executor)
        {
            var labels = executor.OutputVariableLabels();

            if (labels.Count == 0)
            {
                int columns = executor.CountOutputVariables();
                AddDefaultColumns(columns);

                return;
            }

            AddLabeledColumns(labels);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        private void AddDefaultColumns(int columns)
        {
            var columnFormat = "VAR";

            for (int i = 0; i < columns; i++)
            {
                View.AddOutputColumn(columnFormat + (i + 1));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="labels"></param>
        private void AddLabeledColumns(List<string> labels)
        {
            int columns = labels.Count;

            for (int i = 0; i < columns; i++)
            {
                View.AddOutputColumn(labels[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private PatternExtraction CreateExecutor(TreeNode pattern)
        {
            var domNodes = View.GetBodyTreeNodes();
            _executor = new PatternExtraction(pattern, domNodes);
            return _executor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        private void applyUncheckedToSubtree(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                View.ApplyStateToNode(node, 5);
                node.SetState(NodeState.Unchecked);
                applyUncheckedToSubtree(node.Nodes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CustomMenuCanBeShown(HtmlElementEventArgs e)
        {
            return e.CtrlKeyPressed && View.BrowserContextMenuEnabled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool AltPressed(KeyEventArgs e)
        {
            return e.Alt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool EnterPressed(KeyEventArgs e)
        {
            return e.KeyCode == Keys.Enter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private bool RightButtonPressed(MouseButtons button)
        {
            return button == MouseButtons.Right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        private bool Negative(DialogResult answer)
        {
            return answer != DialogResult.OK;
        }
        #endregion

        #region Private Events
        /// <summary>
        /// 
        /// </summary>
        void loadURLsFromFile()
        {
            _openFileDialog.Filter = "Text Files (*.txt)|";
            var answer = _openFileDialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }
            
            ReadTargetUrls reader = new ReadTargetUrls();
            string filename = _openFileDialog.Filename;
            var urls = reader.Read(filename);
            View.AppendTargetUrls(urls);
            View.TargetURLsFile = filename;
        }

        /// <summary>
        /// 
        /// </summary>
        void tunePattern()
        {
            // Check if the Extraction Pattern is specified
            if (!View.ExtractionPatternSpecified)
            {
                View.ShowSpecifyExtractionPatternMessage();
                return;
            }

            // Check if the Target URL is specified
            if (!View.TargetURLSpecified)
            {
                View.ShowSpecifyTargetURLMessage();
                return;
            }

            // Grab the first URL in the target URLs
            string url = View.FirstTargetURL;

            DomBuilted += DeixtoAgentPresenter_DomBuilted;
            
            // Navigate to that URL
            View.NavigateTo(url);
        }

        /// <summary>
        /// 
        /// </summary>
        void DeixtoAgentPresenter_DomBuilted()
        {
            // Grab the Extraction Pattern
            var pattern = View.GetExtractionPattern();

            // Search for the tree structure in the Extraction Pattern
            _executor = new PatternExtraction(pattern, View.GetDOMTreeNodes());
            var matchedNode = _executor.ScanTree(View.GetDOMTreeNodes(), pattern);
            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
            matchedNode.NodeFont = new Font(font, FontStyle.Bold);

            // If you find one, make it Working Pattern
            View.FillPatternTree(matchedNode.GetClone());
            View.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SelectOutputFile()
        {
            var format = View.OutputFileFormat;

            if (format == Format.Text)
            {
                _saveFileDialog.Filter = "Text Files (*.txt)|";
                _saveFileDialog.Extension = "txt";
            }
            
            if (format == Format.XML)
            {
                _saveFileDialog.Filter = "XML Files (*.xml)|";
                _saveFileDialog.Extension = "xml";
            }
            
            if (format == Format.RSS)
            {
                _saveFileDialog.Filter = "RSS Files (*.rss)|";
                _saveFileDialog.Extension = "rss";
            }

            var answer = _saveFileDialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = _saveFileDialog.Filename;
            View.OutputFileName = filename;
        }

        /// <summary>
        /// 
        /// </summary>
        void saveToDisk()
        {
            _saveFileDialog.Filter = "Text Files (*.txt)|";
            _saveFileDialog.Extension = "txt";
            var answer = _saveFileDialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = _saveFileDialog.Filename;
            TextRecordsWriter writer = new TextRecordsWriter(filename);
            var records = _executor.ExtractedResults();
            writer.Write(records);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void addSiblingOrder(TreeNode node)
        {
            _loader.LoadAddSiblingOrderView(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void addNextSibling(TreeNode node)
        {
            var parent = node.Parent;
            int index = node.SourceIndex();
            var tmpElem = _document.GetElementByIndex(index);
            var tmpNode = _domTree.GetNodeFor(tmpElem);
            var nextNode = tmpNode.NextNode;

            if (nextNode == null)
            {
                return;
            }

            int indx = node.Index;
            parent.Nodes.Insert(index++, nextNode.GetClone());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void addPreviousSibling(TreeNode node)
        {
            var parent = node.Parent;
            int index = node.SourceIndex();
            var tmpElem = _document.GetElementByIndex(index);
            var tmpNode = _domTree.GetNodeFor(tmpElem);
            var prevNode = tmpNode.PrevNode;

            if (prevNode == null)
            {
                return;
            }

            parent.Nodes.Insert(0, prevNode.GetClone());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void deleteNode(TreeNode node)
        {
            View.DeletePatternNode(node);
        }

        /// <summary>
        /// 
        /// </summary>
        void loadExtractionPattern()
        {
            _openFileDialog.Filter = "XML Files (*.xml)|";
            var answer = _openFileDialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = string.Empty;
            ReadExtractionPattern reader = new ReadExtractionPattern();

            filename = _openFileDialog.Filename;
            var node = reader.read(filename);
            View.FillExtractionPattern(node);
            View.ExpandExtractionTree();
        }

        /// <summary>
        /// 
        /// </summary>
        void saveExtractionPattern()
        {
            _saveFileDialog.Filter = "XML Files (*.xml)|";
            _saveFileDialog.Extension = "xml";
            var answer = _saveFileDialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = string.Empty;
            WriteExtractionPattern writer = new WriteExtractionPattern();

            filename = _saveFileDialog.Filename;
            writer.write(filename, View.GetPatternTreeNodes());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        void targetURLSelected(string url)
        {
            View.SetURLInput(url);
        }

        /// <summary>
        /// 
        /// </summary>
        void removeURLFromTargetURLs()
        {
            // retrieve the select URL from the TargetURLs list
            string url = View.TargetURLToAdd();

            if (String.IsNullOrWhiteSpace(url))
            {
                View.ShowSelectURLMessage();
                return;
            }

            bool confirm = View.AskUserToRemoveURL();

            if (confirm)
            {
                View.RemoveTargetURL(url);
                View.ClearAddURLInput();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void addURLToTargetURLs()
        {
            // fetch the user-entered url
            string url = View.TargetURLToAdd();

            // if nothing was entered show a warning message
            if (String.IsNullOrWhiteSpace(url))
            {
                View.ShowEnterURLToAddMessage();
                return;
            }

            // at this point we can add the url
            View.AppendTargetUrl(url);
            // clear the entered value from the input text box
            View.ClearAddURLInput();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void removeRegex(TreeNode node)
        {
            var font = node.NodeFont;
            node.NodeFont = new System.Drawing.Font(font, FontStyle.Regular);
            node.SetRegex(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void removeLabel(TreeNode node)
        {
            string text = node.Text;
            int index = text.IndexOf(":");
            node.Text = text.Substring(0, index);
            node.SetLabel(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void windowClosing(FormClosingEventArgs e)
        {
            _eventHub.Publish(new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void addRegex(TreeNode node)
        {
            _loader.LoadRegexBuilderView(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void addNewLabel(TreeNode node)
        {
            _loader.LoadAddLabelView(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="node"></param>
        void outputResultSelected(bool selected, TreeNode node)
        {
            if (!selected)
            {
                return;
            }

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            if (View.HighlightModeEnabled)
            {
                _styling.UnstyleElements();
                _styling.Style(element);
            }

            var path = node.GetPath();

            View.FillElementInfo(node, element.OuterHtml);
            View.SelectDOMNode(_domTree.GetNodeFor(element));

            if (View.CanAutoScroll)
            {
                element.ScrollIntoView(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="state"></param>
        void nodeStateChanged(TreeNode node, NodeState state)
        {
            int imageIndex = new StateToImageMapper().GetImageFromState(state);
            View.ApplyStateToNode(node, imageIndex);
            node.SetState(state);

            if (state == NodeState.Unchecked)
            {
                applyUncheckedToSubtree(node.Nodes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void levelDownWorkingPattern(TreeNode node)
        {
            if (node.IsRoot())
            {
                View.ShowCannotDeleteRootMessage();
                return;
            }

            var newNode = node.FirstNode.GetClone();

            View.ClearPatternTree();
            View.FillPatternTree(newNode);
            View.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void levelUpWorkingPattern(TreeNode node)
        {
            int indx = node.SourceIndex();
            var tmpElem = _document.GetElementByIndex(indx);
            var tmpNode = _domTree.GetNodeFor(tmpElem);
            var parentNode = tmpNode.Parent;

            if (parentNode == null && parentNode.Tag == null)
            {
                return;
            }

            int index = parentNode.SourceIndex();
            var element = _document.GetElementByIndex(index);
            var newNode = new TreeNode(element.TagName);
            var domElem = (IHTMLElement)element.DomElement;

            newNode.Tag = parentNode.Tag;
            newNode.SetState(NodeState.Grayed);
            newNode.SelectedImageIndex = 3;
            newNode.ImageIndex = 3;
            newNode.AddNode(node.GetClone());

            View.ClearPatternTree();
            View.FillPatternTree(newNode);
            View.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        void executeRule()
        {
            var pattern = View.GetWorkingPattern();

            if (pattern == null)
            {
                View.ShowSpecifyPatternMessage();
                return;
            }

            if (View.CrawlingEnabled)
            {
                string mylink = View.HtmlLink();
                int depth = View.CrawlingDepth();
                int count = 0;

                if (String.IsNullOrWhiteSpace(mylink))
                {
                    View.ShowEmptyLinkMessage();
                    return;
                }

                if (depth <= 0)
                {
                    View.ShowInvalidDepthMessage();
                    return;
                }

                View.FocusOutputTabPage();

                CrawlPages(depth, mylink, pattern, ref count);

                View.WritePageResults("Extraction Completed: " + count.ToString() + " results!");
            }
            else
            {
                View.FocusOutputTabPage();
                View.ClearExtractedOutputs();

                var executor = CreateExecutor(pattern);

                executor.FindMatches();

                AddOutputColumns(executor);

                AddOutputResults(executor);

                View.WritePageResults("Extraction Completed: " + executor.Count.ToString() + " results!");

                if (View.OutputFileSpecified)
                {
                    var format = View.OutputFileFormat;
                    string filename = View.OutputFileName;
                    var writer = RecordsWriterFactory.GetWriterFor(format, filename);
                    var records = _executor.ExtractedResults();
                    writer.Write(records);
                }

                var rootNode = executor.TrimUncheckedNodes(pattern);
                View.FillExtractionPattern(rootNode);
                View.ExpandExtractionTree();
            }
        }

        /// <summary>
        /// Reconstructs the DOM tree structure.
        /// </summary>
        void rebuildDOM()
        {
            browserCompleted();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodesCount"></param>
        void clearTreeViews(int nodesCount)
        {
            if (nodesCount == 0)
            {
                return;
            }

            bool confirm = View.AskUserToClearTreeViews();

            if (confirm)
            {
                View.ClearAuxiliaryTree();
                View.ClearPatternTree();
                View.ClearSnapshotTree();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void makeWorkingPatternFromSnapshot(TreeNode node)
        {
            View.ClearPatternTree();
            View.FillPatternTree(node.GetClone());
            View.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void deleteSnapshot(TreeNode node)
        {
            View.DeleteSnapshotInstance(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void createSnapshot(TreeNode node)
        {
            TreeNode root = new TreeNode("SNAP " + string.Format("{0:hh:mm:ss tt}", DateTime.Now));
            root.AddNode(node.GetClone());
            View.FillSnapshotTree(root);
        }

        /// <summary>
        /// Simplifies the DOM tree structure by ignoring the user-specified tags.
        /// 
        /// PRECONDITIONS: At least one tag is selected.
        /// POSTCONDITIONS: Clears all the TreeViews and the TargetURLs list.
        /// </summary>
        void simplifyDOMTree()
        {
            var ignoredTags = View.IgnoredTags();

            if (ignoredTags.Count() == 0)
            {
                View.ShowNoTagSelectedMessage();
                return;
            }

            View.ClearPatternTree();
            View.ClearSnapshotTree();
            View.ClearAuxiliaryTree();
            View.ClearDOMTree();

            _document = new DocumentQuery(View.GetHtmlDocument());
            var elem = _document.GetHtmlElement();
            _domTree = _builder.BuildSimplifiedDOMTree(elem, ignoredTags);
            View.FillDomTree(_domTree.RootNode);
            View.ClearTargetURLs();
            View.AppendTargetUrl(View.Url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void auxiliaryPatternNodeClick(TreeNode node)
        {
            if (node.IsTextNode())
            {
                View.FillTextNodeElementInfo(node);
                return;
            }

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _styling.UnstyleElements();
            _styling.Style(element);

            View.FillElementInfo(node, element.OuterHtml);
            View.SelectDOMNode(_domTree.GetNodeFor(element));

            if (View.CanAutoScroll)
            {
                element.ScrollIntoView(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        void createAuxiliaryPatternFromDocument(HtmlElement element)
        {
            View.FocusAuxiliaryTabPage();
            View.ClearAuxiliaryTree();

            var node = _domTree.GetNodeFor(element);

            View.FillAuxiliaryTree(node.GetClone());

            View.ExpandAuxiliaryTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void showBrowserContextMenu(object sender, HtmlElementEventArgs e)
        {
            // specify that the event was handled
            e.ReturnValue = e.CtrlKeyPressed;

            if (CustomMenuCanBeShown(e))
            {
                return;
            }

            View.CurrentElement = _document.GetElementFromPoint(e.ClientMousePosition);
            View.ShowBrowserMenu();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        void createWorkingPatternFromDocument(HtmlElement element)
        {
            View.ClearPatternTree();

            var node = _domTree.GetNodeFor(element);
            var newNode = _domTree.GetNodeFor(element).GetClone();
            newNode.SetAsRoot();

            View.SetNodeFont(newNode);
            View.FillPatternTree(newNode);

            View.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="button"></param>
        void workingPatternNodeClick(TreeNode node, MouseButtons button)
        {
            if (RightButtonPressed(button))
            {
                View.SetAdjustContextMenuFor(node);
            }

            if (node.IsTextNode())
            {
                View.FillTextNodeElementInfo(node);
                return;
            }

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            if (View.HighlightModeEnabled)
            {
                _styling.UnstyleElements();
                _styling.Style(element);
            }

            View.FillElementInfo(node, element.OuterHtml);
            View.SelectDOMNode(_domTree.GetNodeFor(element));

            if (View.CanAutoScroll)
            {
                element.ScrollIntoView(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void createAuxiliaryPattern(TreeNode node)
        {
            View.FocusAuxiliaryTabPage();
            View.ClearAuxiliaryTree();

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            View.FillAuxiliaryTree(node.GetClone());

            View.ExpandAuxiliaryTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void createWorkingPattern(TreeNode node)
        {
            View.ClearPatternTree();

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);
            var newNode = _domTree.GetNodeFor(element).GetClone();
            newNode.SetAsRoot();

            View.SetNodeFont(newNode);
            View.FillPatternTree(newNode);

            View.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="button"></param>
        void DOMNodeClick(TreeNode node, MouseButtons button)
        {
            if (RightButtonPressed(button))
            {
                View.SetContextMenuFor(node);
            }

            if (node.IsTextNode())
            {
                View.FillTextNodeElementInfo(node);
                return;
            }

            _styling.UnstyleElements();
            // Retrieve the HTML element that corresponds to the DOM TreeNode
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);
            // Style the HTML element in the WebBrowser
            _styling.Style(element);

            View.FillElementInfo(node, element.OuterHtml);

            if (View.CanAutoScroll)
            {
                element.ScrollIntoView(false);
            }
        }

        /// <summary>
        /// Removes the styling of the HtmlDocument's element, when the mouse
        /// moves away from it.
        /// 
        /// PRECONDITION: Highlight mode is enabled.
        /// POSTCONDITION: The ElementInfo TabPage is cleared.
        /// </summary>
        /// <param name="element">The HtmlElement the mouse is leaving from.</param>
        void documentMouseLeave(HtmlElement element)
        {
            if (View.HighlightModeEnabled)
            {
                _styling.Unstyle(element);
            }

            View.ClearElementInfo();
        }

        /// <summary>
        /// Highlights the HtmlDocument's element, when the mouse is over it.
        /// 
        /// PRECONDITION: Highlight mode is enabled.
        /// POSTCONDITION: The ElementInfo TabPage is populated and corresponding
        /// DOM TreeNode is selected.
        /// </summary>
        /// <param name="element"></param>
        void documentMouseOver(HtmlElement element)
        {
            var node = _domTree.GetNodeFor(element);

            if (node == null || !View.HighlightModeEnabled)
            {
                return;
            }

            _styling.UnstyleElements();
            _styling.Style(element);

            View.SelectDOMNode(node);

            View.FillElementInfo(node, element.OuterHtml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void crawlingChanged(bool state)
        {
            View.ApplyVisibilityStateInCrawling(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void autoFillChanged(bool state)
        {
            View.ApplyVisibilityStateInAutoFill(state);
        }

        /// <summary>
        /// Analyzes the keys pressed by the user. If the combination of
        /// Alt+Right Arrow is detected, the WebBrowser's document is navigated
        /// forward. If the detected combination is Alt+Left Arrow, the document
        /// is navigated back. If the user simply pressed Enter, the WebBrowser
        /// navigates to the specified URL.
        /// </summary>
        /// <param name="e">Information arguments from the KeyDown event.</param>
        void keyDownPress(KeyEventArgs e)
        {
            if (EnterPressed(e))
            {
                browseToUrl();
                return;
            }

            if (AltPressed(e))
            {
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        View.NavigateForward();
                        break;
                    case Keys.Left:
                        View.NavigateBack();
                        break;
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Navigates the WebBrowser to the user specified URL. The URL can also
        /// refer to a local HTML document. If the navigation to document fails for
        /// some reason, a message describing the error is shown.
        /// 
        /// PRECONDITIONS: The URL cannot be null or whitespace and it has to refer
        /// to a resource that exists.
        /// POSTCONDITIONS: All the TreeViews and TargetURLs are cleared and the WebBrowser is
        /// navigated to the HTML document.
        /// </summary>
        void browseToUrl()
        {
            var url = View.Url;

            if (String.IsNullOrWhiteSpace(url))
            {
                View.ShowSpecifyURLMessage();
                return;
            }

            var documentValidator = new DocumentValidatorFactory().CreateValidator(url);

            if (!documentValidator.IsValid())
            {
                View.ShowRequestNotFoundMessage();
                return;
            }

            // If the resource described by the URL exists, then proceed with the navigation
            View.ClearAuxiliaryTree();

            if (!View.CrawlingEnabled)
            {
                View.ClearPatternTree();
            }

            View.ClearSnapshotTree();
            View.NavigateTo(documentValidator.Url());
        }

        /// <summary>
        /// Builds the DOM tree structure for the WebBrowser's current HtmlDocument.
        /// 
        /// PRECONDITIONS: The HtmlDocument has been builted.
        /// POSTCONDITIONS: The document's URL is inserted to the TargetURLs list. Clears
        /// all the TreeViews.
        /// </summary>
        void browserCompleted()
        {
            _styling.Clear();
            View.ClearSnapshotTree();

            if (!View.CrawlingEnabled)
            {
                View.ClearPatternTree();
            }

            View.ClearAuxiliaryTree();
            View.ClearDOMTree();

            _document = new DocumentQuery(View.GetHtmlDocument());
            var elem = _document.GetHtmlElement();

            View.ClearTargetURLs();
            View.AppendTargetUrl(View.GetDocumentUrl());
            View.UpdateDocumentUrl();

            _domTree = _builder.BuildDOMTree(elem);
            View.FillDomTree(_domTree.RootNode);
            View.AttachDocumentEvents();

            if (DomBuilted != null)
            {
                DomBuilted();
            }
        }
        #endregion
    }
}

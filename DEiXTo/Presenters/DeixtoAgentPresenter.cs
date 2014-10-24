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
    public class DeixtoAgentPresenter : ISubscriber<RegexAdded>, ISubscriber<SiblingOrderAdded>
    {
        private readonly IDeixtoAgentView _view;
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

        public event DOMBuilted DomBuilted;

        public DeixtoAgentPresenter(IDeixtoAgentView view)
        {
            _view = view;
            _styling = new ElementStyling();
            _builder = new TreeBuilder();
            _imageLoader = new StatesImageLoader();
            _loader = new WindowsViewLoader();
            _saveFileDialog = new SaveFileDialogWrapper();
            _openFileDialog = new OpenFileDialogWrapper();
            _eventHub = EventHub.Instance;

            // ATTACH THE EVENTS OF THE VIEW TO LOCAL METHODS
            _view.BrowseToUrl += browseToUrl;
            _view.KeyDownPress += keyDownPress;
            _view.AutoFillChanged += autoFillChanged;
            _view.CrawlingChanged += crawlingChanged;
            _view.BrowserCompleted += browserCompleted;
            _view.DocumentMouseOver += documentMouseOver;
            _view.DocumentMouseLeave += documentMouseLeave;
            _view.DOMNodeClick += DOMNodeClick;
            _view.CreateWorkingPattern += createWorkingPattern;
            _view.CreateAuxiliaryPattern += createAuxiliaryPattern;
            _view.WorkingPatternNodeClick += workingPatternNodeClick;
            _view.CreateWorkingPatternFromDocument += createWorkingPatternFromDocument;
            _view.CreateAuxiliaryPatternFromDocument += createAuxiliaryPatternFromDocument;
            _view.ShowBrowserContextMenu += showBrowserContextMenu;
            _view.AuxiliaryPatternNodeClick += auxiliaryPatternNodeClick;
            _view.SimplifyDOMTree += simplifyDOMTree;
            _view.CreateSnapshot += createSnapshot;
            _view.DeleteSnapshot += deleteSnapshot;
            _view.MakeWorkingPatternFromSnapshot += makeWorkingPatternFromSnapshot;
            _view.ClearTreeViews += clearTreeViews;
            _view.RebuildDOM += rebuildDOM;
            _view.ExecuteRule += executeRule;
            _view.LevelUpWorkingPattern += levelUpWorkingPattern;
            _view.LevelDownWorkingPattern += levelDownWorkingPattern;
            _view.NodeStateChanged += nodeStateChanged;
            _view.OutputResultSelected += outputResultSelected;
            _view.AddNewLabel += addNewLabel;
            _view.AddRegex += addRegex;
            _view.RemoveLabel += removeLabel;
            _view.RemoveRegex += removeRegex;
            _view.WindowClosing += windowClosing;
            _view.AddURLToTargetURLs += addURLToTargetURLs;
            _view.RemoveURLFromTargetURLs += removeURLFromTargetURLs;
            _view.TargetURLSelected += targetURLSelected;
            _view.SaveExtractionPattern += saveExtractionPattern;
            _view.LoadExtractionPattern += loadExtractionPattern;
            _view.DeleteNode += deleteNode;
            _view.AddPreviousSibling += addPreviousSibling;
            _view.AddNextSibling += addNextSibling;
            _view.AddSiblingOrder += addSiblingOrder;
            _view.SaveToDisk += saveToDisk;
            _view.SelectOutputFile += selectOutputFile;
            _view.TunePattern += tunePattern;
            _view.LoadURLsFromFile += loadURLsFromFile;

            _eventHub.Subscribe<RegexAdded>(this);
            _eventHub.Subscribe<SiblingOrderAdded>(this);

            var imagesList = _imageLoader.LoadImages();
            _view.AddWorkingPatternImages(imagesList);
            _view.AddExtractionTreeImages(imagesList);
        }

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
            _view.AppendTargetUrls(urls);
            _view.TargetURLsFile = filename;
        }

        void tunePattern()
        {
            // Check if the Extraction Pattern is specified
            if (!_view.ExtractionPatternSpecified)
            {
                _view.ShowSpecifyExtractionPatternMessage();
                return;
            }

            // Check if the Target URL is specified
            if (!_view.TargetURLSpecified)
            {
                _view.ShowSpecifyTargetURLMessage();
                return;
            }

            // Grab the first URL in the target URLs
            string url = _view.FirstTargetURL;

            DomBuilted += DeixtoAgentPresenter_DomBuilted;
            
            // Navigate to that URL
            _view.NavigateTo(url);
        }

        void DeixtoAgentPresenter_DomBuilted()
        {
            // Grab the Extraction Pattern
            var pattern = _view.GetExtractionPattern();

            // Search for the tree structure in the Extraction Pattern
            _executor = new PatternExtraction(pattern, _view.GetDOMTreeNodes());
            var matchedNode = _executor.ScanTree(_view.GetDOMTreeNodes(), pattern);
            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
            matchedNode.NodeFont = new Font(font, FontStyle.Bold);

            // If you find one, make it Working Pattern
            _view.FillPatternTree(matchedNode.GetClone());
            _view.ExpandPatternTree();
        }

        void selectOutputFile()
        {
            var format = _view.OutputFileFormat;

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
            _view.OutputFileName = filename;
        }

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

        void addSiblingOrder(TreeNode node)
        {
            _loader.LoadAddSiblingOrderView(node);
        }

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
            _view.DeletePatternNode(node);
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
            _view.FillExtractionPattern(node);
            _view.ExpandExtractionTree();
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
            writer.write(filename, _view.GetPatternTreeNodes());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        void targetURLSelected(string url)
        {
            _view.SetURLInput(url);
        }

        /// <summary>
        /// 
        /// </summary>
        void removeURLFromTargetURLs()
        {
            // retrieve the select URL from the TargetURLs list
            string url = _view.TargetURLToAdd();

            if (String.IsNullOrWhiteSpace(url))
            {
                _view.ShowSelectURLMessage();
                return;
            }

            bool confirm = _view.AskUserToRemoveURL();

            if (confirm)
            {
                _view.RemoveTargetURL(url);
                _view.ClearAddURLInput();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void addURLToTargetURLs()
        {
            // fetch the user-entered url
            string url = _view.TargetURLToAdd();

            // if nothing was entered show a warning message
            if (String.IsNullOrWhiteSpace(url))
            {
                _view.ShowEnterURLToAddMessage();
                return;
            }

            // at this point we can add the url
            _view.AppendTargetUrl(url);
            // clear the entered value from the input text box
            _view.ClearAddURLInput();
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
        /// <param name="subject"></param>
        public void Receive(RegexAdded subject)
        {
            TreeNode node = subject.Node;
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _view.FillElementInfo(node, element.OuterHtml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        public void Receive(SiblingOrderAdded subject)
        {
            int startIndex = subject.StartIndex;
            int stepValue = subject.StepValue;
            TreeNode node = subject.Node;

            node.SetCareAboutSiblingOrder(true);
            node.SetStartIndex(startIndex);
            node.SetStepValue(stepValue);

            node.ForeColor = Color.CadetBlue;
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

            if (_view.HighlightModeEnabled)
            {
                _styling.UnstyleElements();
                _styling.Style(element);
            }

            var path = node.GetPath();

            _view.FillElementInfo(node, element.OuterHtml);
            _view.SelectDOMNode(_domTree.GetNodeFor(element));

            if (_view.CanAutoScroll)
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
            _view.ApplyStateToNode(node, imageIndex);
            node.SetState(state);

            if (state == NodeState.Unchecked)
            {
                applyUncheckedToSubtree(node.Nodes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        private void applyUncheckedToSubtree(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                _view.ApplyStateToNode(node, 5);
                node.SetState(NodeState.Unchecked);
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
                _view.ShowCannotDeleteRootMessage();
                return;
            }

            var newNode = node.FirstNode.GetClone();

            _view.ClearPatternTree();
            _view.FillPatternTree(newNode);
            _view.ExpandPatternTree();
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
            
            _view.ClearPatternTree();
            _view.FillPatternTree(newNode);
            _view.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        void executeRule()
        {
            var pattern = _view.GetWorkingPattern();

            if (pattern == null)
            {
                _view.ShowSpecifyPatternMessage();
                return;
            }

            if (_view.CrawlingEnabled)
            {
                string mylink = _view.HtmlLink();
                int depth = _view.CrawlingDepth();
                int count = 0;

                if (String.IsNullOrWhiteSpace(mylink))
                {
                    _view.ShowEmptyLinkMessage();
                    return;
                }

                if (depth <= 0)
                {
                    _view.ShowInvalidDepthMessage();
                    return;
                }

                _view.FocusOutputTabPage();

                CrawlPages(depth, mylink, pattern, ref count);

                _view.WritePageResults("Extraction Completed: " + count.ToString() + " results!");
            }
            else
            {
                _view.FocusOutputTabPage();
                _view.ClearExtractedOutputs();

                var executor = CreateExecutor(pattern);

                executor.FindMatches();

                AddOutputColumns(executor);

                AddOutputResults(executor);

                _view.WritePageResults("Extraction Completed: " + executor.Count.ToString() + " results!");

                if (_view.OutputFileSpecified)
                {
                    var format = _view.OutputFileFormat;
                    string filename = _view.OutputFileName;
                    var writer = RecordsWriterFactory.GetWriterFor(format, filename);
                    var records = _executor.ExtractedResults();
                    writer.Write(records);
                }

                var rootNode = executor.TrimUncheckedNodes(pattern);
                _view.FillExtractionPattern(rootNode);
                _view.ExpandExtractionTree();
            }
        }

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

                _view.WritePageResults(count.ToString() + " results!");
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
            _view.NavigateTo(href);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executor"></param>
        private void AddOutputResults(PatternExtraction executor)
        {
            foreach (var item in executor.ExtractedResults())
            {
                _view.AddOutputItem(item.ToStringArray(), item.Node);
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
                _view.AddOutputColumn(columnFormat + (i + 1));
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
                _view.AddOutputColumn(labels[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private PatternExtraction CreateExecutor(TreeNode pattern)
        {
            var domNodes = _view.GetBodyTreeNodes();
            _executor = new PatternExtraction(pattern, domNodes);
            return _executor;
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

            bool confirm = _view.AskUserToClearTreeViews();

            if (confirm)
            {
                _view.ClearAuxiliaryTree();
                _view.ClearPatternTree();
                _view.ClearSnapshotTree();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void makeWorkingPatternFromSnapshot(TreeNode node)
        {
            _view.ClearPatternTree();
            _view.FillPatternTree(node.GetClone());
            _view.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void deleteSnapshot(TreeNode node)
        {
            _view.DeleteSnapshotInstance(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void createSnapshot(TreeNode node)
        {
            TreeNode root = new TreeNode("SNAP " + string.Format("{0:hh:mm:ss tt}", DateTime.Now));
            root.AddNode(node.GetClone());
            _view.FillSnapshotTree(root);
        }

        /// <summary>
        /// Simplifies the DOM tree structure by ignoring the user-specified tags.
        /// 
        /// PRECONDITIONS: At least one tag is selected.
        /// POSTCONDITIONS: Clears all the TreeViews and the TargetURLs list.
        /// </summary>
        void simplifyDOMTree()
        {
            var ignoredTags = _view.IgnoredTags();

            if (ignoredTags.Count() == 0)
            {
                _view.ShowNoTagSelectedMessage();
                return;
            }

            _view.ClearPatternTree();
            _view.ClearSnapshotTree();
            _view.ClearAuxiliaryTree();
            _view.ClearDOMTree();

            _document = new DocumentQuery(_view.GetHtmlDocument());
            var elem = _document.GetHtmlElement();
            _domTree = _builder.BuildSimplifiedDOMTree(elem, ignoredTags);
            _view.FillDomTree(_domTree.RootNode);
            _view.ClearTargetURLs();
            _view.AppendTargetUrl(_view.Url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void auxiliaryPatternNodeClick(TreeNode node)
        {
            if (node.IsTextNode())
            {
                _view.FillTextNodeElementInfo(node);
                return;
            }

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _styling.UnstyleElements();
            _styling.Style(element);

            _view.FillElementInfo(node, element.OuterHtml);
            _view.SelectDOMNode(_domTree.GetNodeFor(element));

            if (_view.CanAutoScroll)
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
            _view.FocusAuxiliaryTabPage();
            _view.ClearAuxiliaryTree();

            var node = _domTree.GetNodeFor(element);

            _view.FillAuxiliaryTree(node.GetClone());

            _view.ExpandAuxiliaryTree();
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

            _view.CurrentElement = _document.GetElementFromPoint(e.ClientMousePosition);
            _view.ShowBrowserMenu();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool CustomMenuCanBeShown(HtmlElementEventArgs e)
        {
            return e.CtrlKeyPressed && _view.BrowserContextMenuEnabled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        void createWorkingPatternFromDocument(HtmlElement element)
        {
            _view.ClearPatternTree();

            var node = _domTree.GetNodeFor(element);
            var newNode = _domTree.GetNodeFor(element).GetClone();
            newNode.SetAsRoot();

            _view.SetNodeFont(newNode);
            _view.FillPatternTree(newNode);

            _view.ExpandPatternTree();
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
                _view.SetAdjustContextMenuFor(node);
            }

            if (node.IsTextNode())
            {
                _view.FillTextNodeElementInfo(node);
                return;
            }

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            if (_view.HighlightModeEnabled)
            {
                _styling.UnstyleElements();
                _styling.Style(element);
            }

            _view.FillElementInfo(node, element.OuterHtml);
            _view.SelectDOMNode(_domTree.GetNodeFor(element));

            if (_view.CanAutoScroll)
            {
                element.ScrollIntoView(false);
            }
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
        /// <param name="node"></param>
        void createAuxiliaryPattern(TreeNode node)
        {
            _view.FocusAuxiliaryTabPage();
            _view.ClearAuxiliaryTree();

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            _view.FillAuxiliaryTree(node.GetClone());

            _view.ExpandAuxiliaryTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        void createWorkingPattern(TreeNode node)
        {
            _view.ClearPatternTree();

            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);
            var newNode = _domTree.GetNodeFor(element).GetClone();
            newNode.SetAsRoot();

            _view.SetNodeFont(newNode);
            _view.FillPatternTree(newNode);

            _view.ExpandPatternTree();
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
                _view.SetContextMenuFor(node);
            }

            if (node.IsTextNode())
            {
                _view.FillTextNodeElementInfo(node);
                return;
            }

            _styling.UnstyleElements();
            // Retrieve the HTML element that corresponds to the DOM TreeNode
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);
            // Style the HTML element in the WebBrowser
            _styling.Style(element);

            _view.FillElementInfo(node, element.OuterHtml);

            if (_view.CanAutoScroll)
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
            if (_view.HighlightModeEnabled)
            {
                _styling.Unstyle(element);
            }

            _view.ClearElementInfo();
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

            if (node == null || !_view.HighlightModeEnabled)
            {
                return;
            }

            _styling.UnstyleElements();
            _styling.Style(element);

            _view.SelectDOMNode(node);

            _view.FillElementInfo(node, element.OuterHtml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void crawlingChanged(bool state)
        {
            _view.ApplyVisibilityStateInCrawling(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void autoFillChanged(bool state)
        {
            _view.ApplyVisibilityStateInAutoFill(state);
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
                        _view.NavigateForward();
                        break;
                    case Keys.Left:
                        _view.NavigateBack();
                        break;
                }
                e.Handled = true;
            }
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
            var url = _view.Url;

            if (String.IsNullOrWhiteSpace(url))
            {
                _view.ShowSpecifyURLMessage();
                return;
            }

            var documentValidator = new DocumentValidatorFactory().CreateValidator(url);

            if (!documentValidator.IsValid())
            {
                _view.ShowRequestNotFoundMessage();
                return;
            }

            // If the resource described by the URL exists, then proceed with the navigation
            _view.ClearAuxiliaryTree();

            if (!_view.CrawlingEnabled)
            {
                _view.ClearPatternTree();
            }

            _view.ClearSnapshotTree();
            _view.NavigateTo(documentValidator.Url());
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
            _view.ClearSnapshotTree();

            if (!_view.CrawlingEnabled)
            {
                _view.ClearPatternTree();
            }

            _view.ClearAuxiliaryTree();
            _view.ClearDOMTree();

            _document = new DocumentQuery(_view.GetHtmlDocument());
            var elem = _document.GetHtmlElement();

            _view.ClearTargetURLs();
            _view.AppendTargetUrl(_view.GetDocumentUrl());
            _view.UpdateDocumentUrl();

            _domTree = _builder.BuildDOMTree(elem);
            _view.FillDomTree(_domTree.RootNode);
            _view.AttachDocumentEvents();

            if (DomBuilted != null)
            {
                DomBuilted();
            }
        }
    }
}

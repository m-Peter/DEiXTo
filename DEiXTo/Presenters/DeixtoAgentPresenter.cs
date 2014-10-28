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
        private ElementStyling _styling;
        private TreeBuilder _builder;
        private DocumentQuery _document;
        private StatesImageLoader _imageLoader;
        private DOMTreeStructure _domTree;
        private IViewLoader _loader;
        private IEventHub _eventHub;
        private ISaveFileDialog _saveFileDialog;
        private IOpenFileDialog _openFileDialog;
        private PatternExtraction _executor;
        private IDeixtoAgentScreen _screen;
        #endregion

        #region Public Events
        public event DOMBuilted DomBuilted;
        #endregion

        public IDeixtoAgentView View { get; set; }

        #region Constructors
        public DeixtoAgentPresenter(IDeixtoAgentView view, ISaveFileDialog saveFileDialog, IViewLoader loader, IEventHub eventHub, IDeixtoAgentScreen screen)
        {
            View = view;
            View.Presenter = this;
            _screen = screen;
            _styling = new ElementStyling();
            _builder = new TreeBuilder();
            _imageLoader = new StatesImageLoader();
            _loader = loader;
            _saveFileDialog = saveFileDialog;
            _openFileDialog = new OpenFileDialogWrapper();
            _eventHub = eventHub;

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
            //int index = node.SourceIndex();
            //var element = _document.GetElementByIndex(index);
            var element = _screen.GetElementFromNode(node);

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
        public void LoadURLsFromFile()
        {
            var openFileDialog = _screen.GetTextFileDialog();
            var answer = openFileDialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            var filename = openFileDialog.Filename;
            var urls = _screen.LoadUrlsFromFile(filename);

            View.AppendTargetUrls(urls);
            View.TargetURLsFile = filename;
        }

        /// <summary>
        /// 
        /// </summary>
        public void TunePattern()
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
            // TODO REMOVE DEPENDENCIES
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
            var factory = new DialogBuilderFactory();
            var builder = factory.CreateBuilder(format);
            builder.Build(_saveFileDialog);

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
        public void SaveToDisk()
        {
            var saveFileDialog = _screen.GetTextSaveFileDialog();

            var answer = saveFileDialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = saveFileDialog.Filename;
            _screen.WriteExtractedRecords(filename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void AddSiblingOrder(TreeNode node)
        {
            _loader.LoadAddSiblingOrderView(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void AddNextSibling(TreeNode node)
        {
            var parent = node.Parent;
            //int index = node.SourceIndex();
            //var tmpElem = _document.GetElementByIndex(index);
            //var tmpNode = _domTree.GetNodeFor(tmpElem);
            var tmpNode = _screen.GetDomNode(node);
            var nextNode = tmpNode.NextNode;

            if (nextNode == null)
            {
                return;
            }

            int indx = node.Index;
            parent.Nodes.Insert(++indx, nextNode.GetClone());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void AddPreviousSibling(TreeNode node)
        {
            var parent = node.Parent;
            //int index = node.SourceIndex();
            //var tmpElem = _document.GetElementByIndex(index);
            //var tmpNode = _domTree.GetNodeFor(tmpElem);
            var tmpNode = _screen.GetDomNode(node);
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
        public void DeleteNode(TreeNode node)
        {
            View.DeletePatternNode(node);
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadExtractionPattern()
        {
            var dialog = _screen.GetOpenFileDialog("XML Files (*.xml)|");
            var answer = dialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = dialog.Filename;
            var node = _screen.LoadExtractionPattern(filename);
            View.FillExtractionPattern(node);
            View.ExpandExtractionTree();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveExtractionPattern()
        {
            string filter = "XML Files (*.xml)|";
            string extension = "xml";
            var dialog = _screen.GetSaveFileDialog(filter, extension);
            var answer = dialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = dialog.Filename;
            _screen.SaveExtractionPattern(filename, View.GetPatternTreeNodes());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public void TargetURLSelected(string url)
        {
            View.SetURLInput(url);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveURLFromTargetURLs()
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
        public void AddURLToTargetURLs()
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
        public void RemoveRegex(TreeNode node)
        {
            var font = node.NodeFont;
            node.NodeFont = new Font(font, FontStyle.Regular);
            node.SetRegex(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void RemoveLabel(TreeNode node)
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
        public void windowClosing()
        {
            _eventHub.Publish(new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void AddRegex(TreeNode node)
        {
            _loader.LoadRegexBuilderView(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void AddNewLabel(TreeNode node)
        {
            _loader.LoadAddLabelView(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="node"></param>
        public void OutputResultSelected(bool selected, TreeNode node)
        {
            // TODO REMOVE DEPENDENCIEs
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
        public void NodeStateChanged(TreeNode node, NodeState state)
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
        public void LevelDownWorkingPattern(TreeNode node)
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
        public void LevelUpWorkingPattern(TreeNode node)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void ExecuteRule()
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
        public void RebuildDOM()
        {
            BrowserCompleted();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodesCount"></param>
        public void ClearTreeViews(int nodesCount)
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
        public void MakeWorkingPatternFromSnapshot(TreeNode node)
        {
            View.ClearPatternTree();
            View.FillPatternTree(node.GetClone());
            View.ExpandPatternTree();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void DeleteSnapshot(TreeNode node)
        {
            View.DeleteSnapshotInstance(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void CreateSnapshot(TreeNode node)
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
        public void SimplifyDOMTree()
        {
            // TODO REMOVE DEPENDENCIES
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
        public void AuxiliaryPatternNodeClick(TreeNode node)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void CreateAuxiliaryPatternFromDocument(HtmlElement element)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void ShowBrowserContextMenu(object sender, HtmlElementEventArgs e)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void CreateWorkingPatternFromDocument(HtmlElement element)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void WorkingPatternNodeClick(TreeNode node, MouseButtons button)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void CreateAuxiliaryPattern(TreeNode node)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void CreateWorkingPattern(TreeNode node)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void DOMNodeClick(TreeNode node, MouseButtons button)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void DocumentMouseLeave(HtmlElement element)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void DocumentMouseOver(HtmlElement element)
        {
            // TODO REMOVE DEPENDENCIES
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
        public void CrawlingChanged(bool state)
        {
            View.ApplyVisibilityStateInCrawling(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void AutoFillChanged(bool state)
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
        public void KeyDownPress(KeyEventArgs e)
        {
            if (EnterPressed(e))
            {
                BrowseToUrl();
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
        public void BrowseToUrl()
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
        public void BrowserCompleted()
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

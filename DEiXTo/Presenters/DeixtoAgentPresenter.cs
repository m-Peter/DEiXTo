using DEiXTo.Views;
using System;
using System.Linq;
using System.Windows.Forms;
using mshtml;
using DEiXTo.Services;
using DEiXTo.Models;

namespace DEiXTo.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class DeixtoAgentPresenter
    {
        private readonly IDeixtoAgentView _view;
        private ElementStyling _styling;
        private TreeBuilder _builder;
        private DocumentQuery _document;
        private StatesImageLoader _imageLoader;
        private DOMTreeStructure _domTree;

        public DeixtoAgentPresenter(IDeixtoAgentView view)
        {
            _view = view;
            _styling = new ElementStyling();
            _builder = new TreeBuilder();
            _imageLoader = new StatesImageLoader();

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

            var imagesList = _imageLoader.LoadImages();

            _view.AddWorkingPatterImages(imagesList);
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

            if (parentNode != null && parentNode.Tag != null)
            {
                int index = parentNode.SourceIndex();
                var element = _document.GetElementByIndex(index);
                var newNode = new TreeNode(element.TagName);
                var domElem = (IHTMLElement)element.DomElement;

                PointerInfo pInfo = new PointerInfo();
                pInfo.ElementSourceIndex = domElem.sourceIndex;

                newNode.Tag = pInfo;
                newNode.SelectedImageIndex = 3;
                newNode.ImageIndex = 3;
                newNode.AddNode(node.GetClone());
                _view.ClearPatternTree();
                _view.FillPatternTree(newNode);
                _view.ExpandPatternTree();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="filtered"></param>
        private void FilterUncheckedNodes(TreeNodeCollection nodes, TreeNode filtered)
        {
            foreach (TreeNode node in nodes)
            {
                var state = node.GetState();

                if (state != NodeState.Unchecked)
                {
                    var newNode = new TreeNode(node.Text);
                    newNode.Tag = node.Tag;
                    filtered.Nodes.Add(newNode);
                    FilterUncheckedNodes(node.Nodes, newNode);
                }
                else
                {
                    FilterUncheckedNodes(node.Nodes, filtered);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void executeRule()
        {
            _view.ClearExtractedOutputs();

            var pattern = _view.GetWorkingPattern();
            var bodyNodes = _view.GetBodyTreeNodes();
            var copiedPattern = new TreeNode(pattern.Text);
            copiedPattern.Tag = pattern.Tag;
            FilterUncheckedNodes(pattern.Nodes, copiedPattern);
            PatternExtraction extraction = new PatternExtraction(copiedPattern, bodyNodes);
            
            extraction.FindMatches();
            
            var results = extraction.ExtractedResults();
            var columnFormat = "VAR";

            for (int i = 0; i < extraction.CountOutputVariables(); i++)
            {
                _view.AddOutputColumn(columnFormat + i);
            }

            foreach (var item in results)
            {
                _view.AddOutputItem(item.ToStringArray());
            }
            
            _view.SetExtractedResults(results.Count);
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

            bool answer = _view.AskUserToClearTreeViews();
            if (answer)
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

            var path = node.GetPath();

            _view.FillElementInfo(node, element.OuterHtml);
            _view.SelectDOMNode(_domTree.GetNodeFor(element));

            if (_view.CanAutoScroll())
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
            e.ReturnValue = e.CtrlKeyPressed;

            if (e.CtrlKeyPressed)
            {
                return;
            }

            if (!e.ReturnValue)
            {
                if (_view.BrowserContextMenuEnabled())
                {
                    _view.CurrentElement = _document.GetElementFromPoint(e.ClientMousePosition);
                    _view.ShowBrowserMenu();
                }
            }
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
            if (button == MouseButtons.Right)
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

            if (_view.HighlightModeEnabled())
            {
                _styling.UnstyleElements();
                _styling.Style(element);
            }

            var path = node.GetPath();

            _view.FillElementInfo(node, element.OuterHtml);
            _view.SelectDOMNode(_domTree.GetNodeFor(element));

            if (_view.CanAutoScroll())
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
            if (button == MouseButtons.Right)
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
            
            var path = node.GetPath();

            _view.FillElementInfo(node, element.OuterHtml);

            if (_view.CanAutoScroll())
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
            if (_view.HighlightModeEnabled())
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
            if (_view.HighlightModeEnabled())
            {
                _styling.UnstyleElements();
                _styling.Style(element);

                var node = _domTree.GetNodeFor(element);

                if (node == null)
                {
                    return;
                }

                _view.SelectDOMNode(node);

                var path = node.GetPath();
                _view.FillElementInfo(node, element.OuterHtml);
            }
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
            if (e.Alt)
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
            else if (e.KeyCode == Keys.Enter)
            {
                browseToUrl();
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
            var url = _view.Url;

            if (String.IsNullOrWhiteSpace(url))
            {
                _view.ShowSpecifyURLMessage();
                return;
            }

            var documentValidator = new DocumentValidatorFactory().CreateValidator(url);

            // If the resource described by the URL exists, then proceed with the navigation
            if (documentValidator.IsValid())
            {
                _view.ClearAuxiliaryTree();
                _view.ClearPatternTree();
                _view.ClearSnapshotTree();
                _view.NavigateTo(documentValidator.Url());
            }
            else // Otherwise show the error message
            {
                _view.ShowRequestNotFoundMessage();
            }
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
            _view.ClearPatternTree();
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
        }
    }
}

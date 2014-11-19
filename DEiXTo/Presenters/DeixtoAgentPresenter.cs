using DEiXTo.Views;
using System;
using System.Linq;
using System.Windows.Forms;
using DEiXTo.Services;
using DEiXTo.Models;
using System.Drawing;
using System.Collections.Generic;
using mshtml;

namespace DEiXTo.Presenters
{

    public delegate void DOMBuilted();
    public delegate void FormSubmitted();

    public class DeixtoAgentPresenter : ISubscriber<RegexAdded>
    {
        private IViewLoader _loader;
        private IEventHub _eventHub;
        private PatternExecutor _executor;
        private IDeixtoAgentScreen _screen;

        public event DOMBuilted DomBuilted;
        public event FormSubmitted FormSubmitted;

        public IDeixtoAgentView View { get; set; }

        public DeixtoAgentPresenter(IDeixtoAgentView view, IViewLoader loader, IEventHub eventHub, IDeixtoAgentScreen screen)
        {
            View = view;
            View.Presenter = this;
            _screen = screen;
            _loader = loader;
            _eventHub = eventHub;

            _eventHub.Subscribe<RegexAdded>(this);

            var imagesList = _screen.LoadStateImages();
            View.AddWorkingPatternImages(imagesList);
            View.AddExtractionTreeImages(imagesList);
        }

        public void Receive(RegexAdded subject)
        {
            TreeNode node = subject.Node;
            var element = _screen.GetElementFromNode(node);

            View.FillElementInfo(node, element.OuterHtml);
        }

        private void applyUncheckedToSubtree(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                View.ApplyStateToNode(node, 5);
                node.SetState(NodeState.Unchecked);
                applyUncheckedToSubtree(node.Nodes);
            }
        }

        private bool CustomMenuCanBeShown(HtmlElementEventArgs e)
        {
            return e.CtrlKeyPressed && View.BrowserContextMenuEnabled;
        }

        private bool AltPressed(KeyEventArgs e)
        {
            return e.Alt;
        }

        private bool EnterPressed(KeyEventArgs e)
        {
            return e.KeyCode == Keys.Enter;
        }

        private bool RightButtonPressed(MouseButtons button)
        {
            return button == MouseButtons.Right;
        }

        private bool Negative(DialogResult answer)
        {
            return answer != DialogResult.OK;
        }

        public void LoadURLsFromFile()
        {
            string filter = "Text Files (*.txt)|";
            var openFileDialog = _screen.GetOpenFileDialog(filter);
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

        void DeixtoAgentPresenter_DomBuilted()
        {
            // TODO REMOVE DEPENDENCIES
            // Grab the Extraction Pattern
            var pattern = View.GetExtractionPattern();
            var extraction = new ExtractionPattern(pattern);

            // Search for the tree structure in the Extraction Pattern
            _executor = new PatternExecutor(extraction, View.GetDOMTreeNodes());
            var matchedNode = _screen.ScanDomTree(pattern);
            matchedNode.Tag = pattern.Tag;
            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
            matchedNode.NodeFont = new Font(font, FontStyle.Bold);

            // If you find one, make it Working Pattern
            View.FillPatternTree(matchedNode.GetClone());
            View.ExpandPatternTree();
        }

        public void SelectOutputFile()
        {
            var format = View.OutputFileFormat;
            var dialog = _screen.GetSaveFileDialog(format);

            var answer = dialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = dialog.Filename;
            View.OutputFileName = filename;
        }

        public void DisableHighlighting()
        {
            View.DisableHighlighting();
        }

        public void EnableHighlighting()
        {
            View.EnableHighlighting();
        }

        public void SaveToDisk()
        {
            string filter = "Text Files (*.txt)|";
            string extension = "txt";
            var saveFileDialog = _screen.GetSaveFileDialog(filter, extension);

            var answer = saveFileDialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = saveFileDialog.Filename;
            _screen.WriteExtractedRecords(filename);
        }

        public void AddSiblingOrder(TreeNode node)
        {
            _loader.LoadAddSiblingOrderView(node);
        }

        public void AddAttributeConstraint(TreeNode node)
        {
            _loader.LoadAddAttributeConstraintView(node);
        }

        public void AddNextSibling(TreeNode node)
        {
            var parent = node.Parent;
            var tmpNode = _screen.GetDomNode(node);
            var nextNode = tmpNode.NextNode;

            if (nextNode == null)
            {
                return;
            }

            int indx = node.Index;
            parent.Nodes.Insert(++indx, nextNode.GetClone());
        }

        public void AddPreviousSibling(TreeNode node)
        {
            var parent = node.Parent;
            var tmpNode = _screen.GetDomNode(node);
            var prevNode = tmpNode.PrevNode;

            if (prevNode == null)
            {
                return;
            }

            parent.Nodes.Insert(0, prevNode.GetClone());
        }

        public void DeleteNode(TreeNode node)
        {
            View.DeletePatternNode(node);
        }

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

        public void TargetURLSelected(string url)
        {
            View.SetURLInput(url);
        }

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

        public void RemoveRegex(TreeNode node)
        {
            var font = node.NodeFont;
            node.NodeFont = new Font(font, FontStyle.Regular);
            node.SetRegex(null);
        }

        public void RemoveLabel(TreeNode node)
        {
            string text = node.Text;
            int index = text.IndexOf(":");
            node.Text = text.Substring(0, index);
            node.SetLabel(null);
        }

        public void windowClosing()
        {
            _eventHub.Publish(new DeixtoAgentClosed());
        }

        public void AddRegex(TreeNode node)
        {
            _loader.LoadRegexBuilderView(node);
        }

        public void AddNewLabel(TreeNode node)
        {
            _loader.LoadAddLabelView(node);
        }

        public void OutputResultSelected(bool selected, TreeNode node)
        {
            if (!selected)
            {
                return;
            }

            var element = _screen.GetElementFromNode(node);

            if (View.HighlightModeEnabled)
            {
                _screen.HighlightElement(element);
            }

            View.FillElementInfo(node, element.OuterHtml);
            
            var domNode = _screen.GetDomNode(node);
            View.SelectDOMNode(domNode);

            if (View.CanAutoScroll)
            {
                element.ScrollIntoView(true);
            }
        }

        public void NodeStateChanged(TreeNode node, NodeState state)
        {
            int imageIndex = NodeStateTranslator.StateToImageIndex(state);
            View.ApplyStateToNode(node, imageIndex);
            node.SetState(state);

            if (state == NodeState.Unchecked)
            {
                applyUncheckedToSubtree(node.Nodes);
            }
        }

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

        public void LevelUpWorkingPattern(TreeNode node)
        {
            // Translate the given node to its opposite DOM node
            var domNode = _screen.GetDomNode(node);
            // Retrieve the parent of the DOM node
            var parentNode = domNode.Parent;

            if (parentNode == null && parentNode.Tag == null)
            {
                return;
            }

            var element = _screen.GetElementFromNode(parentNode);
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

        public void ExecuteRule()
        {
            var pattern = View.GetWorkingPattern();
            View.ClearExtractionPattern();

            if (pattern == null)
            {
                View.ShowSpecifyPatternMessage();
                return;
            }

            View.FocusOutputTabPage();

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

                CrawlPages(depth, mylink, pattern, ref count);

                View.WritePageResults("Extraction Completed: " + count.ToString() + " results!");
            }
            else
            {
                View.ClearExtractedOutputs();

                var domNodes = View.GetBodyTreeNodes();
                var extractionResult = _screen.Execute(pattern, domNodes);

                AddOutputColumns(extractionResult);
                AddOutputResults(extractionResult);

                View.WritePageResults("Extraction Completed: " + extractionResult.RecordsCount.ToString() + " results!");

                View.FillExtractionPattern(pattern.GetClone());
                View.ExpandExtractionTree();
            }
        }

        private void CrawlPages(int depth, string mylink, TreeNode pattern, ref int count)
        {
            var domNodes = View.GetBodyTreeNodes();
            var extractionResult = _screen.Execute(pattern, domNodes);
            AddOutputColumns(extractionResult);

            for (int i = 0; i < depth; i++)
            {
                FollowNextLink(mylink);

                domNodes = View.GetBodyTreeNodes();
                extractionResult = _screen.Execute(pattern, domNodes);
                count += extractionResult.RecordsCount;
                
                AddOutputResults(extractionResult);

                View.WritePageResults(count.ToString() + " results!");
            }
        }

        private void FollowNextLink(string nextLink)
        {
            var elem = _screen.GetLinkToFollow(nextLink);
            string href = elem.GetAttribute("href");
            View.NavigateTo(href);
        }

        private void AddOutputColumns(IExtraction extraction)
        {
            var labels = extraction.OutputVariableLabels;

            foreach (string label in labels)
            {
                View.AddOutputColumn(label);
            }
        }

        private void AddOutputResults(IExtraction extraction)
        {
            foreach (var item in extraction.ExtractedRecords)
            {
                View.AddOutputItem(item.ToStringArray(), item.Node);
            }
        }

        public void RebuildDOM()
        {
            BrowserCompleted();
        }

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

        public void MakeWorkingPatternFromSnapshot(TreeNode node)
        {
            View.ClearPatternTree();
            View.FillPatternTree(node.GetClone());
            View.ExpandPatternTree();
        }

        public void DeleteSnapshot(TreeNode node)
        {
            View.DeleteSnapshotInstance(node);
        }

        public void CreateSnapshot(TreeNode node)
        {
            TreeNode root = new TreeNode("SNAP " + string.Format("{0:hh:mm:ss tt}", DateTime.Now));
            root.AddNode(node.GetClone());
            View.FillSnapshotTree(root);
        }

        public void SimplifyDOMTree()
        {
            var ignoredTags = View.IgnoredTags;

            if (ignoredTags.Count() == 0)
            {
                View.ShowNoTagSelectedMessage();
                return;
            }

            View.ClearPatternTree();
            View.ClearSnapshotTree();
            View.ClearAuxiliaryTree();
            View.ClearDOMTree();

            var document = View.GetHtmlDocument();
            _screen.CreateDocument(document);
            var rootNode = _screen.BuildSimplifiedDOM(ignoredTags);

            View.FillDomTree(rootNode);
            View.ClearTargetURLs();
            View.AppendTargetUrl(View.Url);
        }

        public void AuxiliaryPatternNodeClick(TreeNode node)
        {
            if (node.IsTextNode())
            {
                View.FillTextNodeElementInfo(node);
                return;
            }

            var element = _screen.GetElementFromNode(node);

            _screen.HighlightElement(element);

            View.FillElementInfo(node, element.OuterHtml);
            var domNode = _screen.GetDomNode(node);
            View.SelectDOMNode(domNode);

            if (View.CanAutoScroll)
            {
                element.ScrollIntoView(true);
            }
        }

        public void CreateAuxiliaryPatternFromDocument(HtmlElement element)
        {
            View.FocusAuxiliaryTabPage();
            View.ClearAuxiliaryTree();

            var domNode = _screen.GetNodeFromElement(element);

            View.FillAuxiliaryTree(domNode.GetClone());
            View.ExpandAuxiliaryTree();
        }

        public void ShowBrowserContextMenu(object sender, HtmlElementEventArgs e)
        {
            // TODO REMOVE DEPENDENCIES
            // specify that the event was handled
            e.ReturnValue = e.CtrlKeyPressed;

            if (CustomMenuCanBeShown(e))
            {
                return;
            }

            if (!View.BrowserContextMenuEnabled)
            {
                View.ShowHighlightContextMenu();
                return;
            }

            View.CurrentElement = _screen.GetElementFromPosition(e.ClientMousePosition);
            View.ShowBrowserMenu();
        }

        public void CreateWorkingPatternFromDocument(HtmlElement element)
        {
            View.ClearPatternTree();

            var newNode = _screen.GetNodeFromElement(element).GetClone();
            newNode.SetAsRoot();

            View.SetNodeFont(newNode);
            View.FillPatternTree(newNode);
            View.ExpandPatternTree();
        }

        public void WorkingPatternNodeClick(TreeNode node, MouseButtons button)
        {
            if (node.IsTextNode())
            {
                View.ClearAttributes();
            }
            else
            {
                View.ClearAttributes();
                var attributes = node.GetAttributes().All;
                View.LoadNodeAttributes(attributes);
            }

            if (RightButtonPressed(button))
            {
                View.SetAdjustContextMenuFor(node);
            }

            if (node.IsTextNode())
            {
                View.FillTextNodeElementInfo(node);
                return;
            }

            var element = _screen.GetElementFromNode(node);

            if (View.HighlightModeEnabled)
            {
                _screen.HighlightElement(element);
            }

            View.FillElementInfo(node, element.OuterHtml);
            var domNode = _screen.GetNodeFromElement(element);
            View.SelectDOMNode(domNode);

            if (View.CanAutoScroll)
            {
                element.ScrollIntoView(true);
            }
        }

        public void CreateAuxiliaryPattern(TreeNode node)
        {
            View.FocusAuxiliaryTabPage();
            View.ClearAuxiliaryTree();

            View.FillAuxiliaryTree(node.GetClone());

            View.ExpandAuxiliaryTree();
        }

        public void CreateWorkingPattern(TreeNode node)
        {
            View.ClearPatternTree();

            var domNode = _screen.GetDomNode(node).GetClone();
            domNode.SetAsRoot();

            View.SetNodeFont(domNode);
            View.FillPatternTree(domNode);
            View.ExpandPatternTree();
        }

        public void DOMNodeClick(TreeNode node, MouseButtons button)
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

            var element = _screen.GetElementFromNode(node);

            _screen.HighlightElement(element);
            View.FillElementInfo(node, element.OuterHtml);

            if (View.CanAutoScroll)
            {
                element.ScrollIntoView(true);
            }
        }

        public void DocumentMouseLeave(HtmlElement element)
        {
            if (View.HighlightModeEnabled)
            {
                _screen.RemoveHighlighting(element);
            }

            View.ClearElementInfo();
        }

        public void DocumentMouseOver(HtmlElement element)
        {
            var node = _screen.GetNodeFromElement(element);

            if (node == null || !View.HighlightModeEnabled)
            {
                return;
            }

            _screen.HighlightElement(element);
            View.SelectDOMNode(node);
            View.FillElementInfo(node, element.OuterHtml);
        }

        public void CrawlingChanged(bool state)
        {
            View.ApplyVisibilityStateInCrawling(state);
        }

        public void AutoFillChanged(bool state)
        {
            View.ApplyVisibilityStateInAutoFill(state);
        }

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

        public void NavigationFailed(object StatusCode)
        {
            string status = StatusCode.ToString();
            string message = string.Empty;

            if (status == "404")
            {
                message = "Object not found.";
            }
            else if (status == "-2146697211")
            {
                message = "Requested resource not found.";
            }
            else
            {
                message = status;
            }

            View.ShowNavigationErrorMessage(message);
        }

        public void BrowseToUrl()
        {
            var url = View.Url;

            if (String.IsNullOrWhiteSpace(url))
            {
                View.ShowSpecifyURLMessage();
                return;
            }

            View.ClearAuxiliaryTree();

            if (!View.CrawlingEnabled)
            {
                View.ClearPatternTree();
            }

            View.ClearSnapshotTree();
            View.NavigateTo(url);
        }

        public void BrowserCompleted()
        {
            _screen.ClearStyling();
            View.ClearSnapshotTree();

            if (!View.CrawlingEnabled)
            {
                View.ClearPatternTree();
            }

            View.ClearAuxiliaryTree();
            View.ClearDOMTree();

            var document = View.GetHtmlDocument();
            _screen.CreateDocument(document);

            View.ClearTargetURLs();
            View.AppendTargetUrl(View.GetDocumentUrl());
            View.UpdateDocumentUrl();

            var rootNode = _screen.BuildDom();
            View.FillDomTree(rootNode);
            View.AttachDocumentEvents();

            if (DomBuilted != null)
            {
                DomBuilted();
            }

            if (FormSubmitted != null)
            {
                FormSubmitted();
            }
        }

        public void SaveWrapper()
        {
            var targetUrls = View.TargetUrls;
            var inputFile = View.TargetURLsFile;

            if (targetUrls.Count() == 0 && String.IsNullOrWhiteSpace(inputFile))
            {
                View.ShowSpecifyInputSourceMessage();
                return;
            }

            if (targetUrls.Count() > 0 && !string.IsNullOrWhiteSpace(inputFile))
            {
                View.ShowSelectOneInputSourceMessage();
                return;
            }

            var pattern = View.GetExtractionPatternNodes();

            if (pattern.Count == 0)
            {
                View.ShowSpecifyExtractionPatternMessage();
                return;
            }

            var wrapper = new DeixtoWrapper();

            if (String.IsNullOrWhiteSpace(inputFile))
            {
                wrapper.TargetUrls = targetUrls;
            }
            else
            {
                wrapper.InputFile = inputFile;
            }

            wrapper.ExtractionPattern = View.ExtractionPattern;
            wrapper.AutoFill = View.AutoFill;
            wrapper.Delay = View.Delay;
            wrapper.ExtractNativeUrl = View.ExtractNativeUrl;
            wrapper.FormInputName = View.FormInputName;
            wrapper.FormName = View.FormName;
            wrapper.FormTerm = View.FormTerm;
            wrapper.HtmlNextLink = View.HtmlNextLink;
            wrapper.IgnoredTags = View.IgnoredTags;
            wrapper.MaxCrawlingDepth = View.MaxCrawlingDepth;
            wrapper.MultiPageCrawling = View.MultiPageCrawling;
            wrapper.NumberOfHits = View.NumberOfHits;
            wrapper.OutputFileName = View.OutputFileName;
            wrapper.OutputFormat = View.OutputFormat;
            wrapper.OutputMode = View.OutputMode;


            string filter = "Wrapper Project Files (*.wpf)|";
            string extension = "wpf";
            var dialog = _screen.GetSaveFileDialog(filter, extension);

            var answer = dialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = dialog.Filename;

            _screen.SaveWrapper(wrapper, pattern, filename);
        }

        public void LoadWrapper()
        {
            string filter = "Wrapper Project Files (*.wpf)|";
            var dialog = _screen.GetOpenFileDialog(filter);

            var answer = dialog.ShowDialog();

            if (Negative(answer))
            {
                return;
            }

            string filename = dialog.Filename;

            var wrapper = _screen.LoadWrapper(filename);
            View.AutoFill = wrapper.AutoFill;
            View.Delay = wrapper.Delay;
            View.ExtractionPattern = wrapper.ExtractionPattern;
            View.ExtractNativeUrl = wrapper.ExtractNativeUrl;
            View.FormInputName = wrapper.FormInputName;
            View.FormName = wrapper.FormName;
            View.FormTerm = wrapper.FormTerm;
            View.HtmlNextLink = wrapper.HtmlNextLink;
            View.IgnoredTags = wrapper.IgnoredTags;
            View.InputFile = wrapper.InputFile;
            View.MaxCrawlingDepth = wrapper.MaxCrawlingDepth;
            View.MultiPageCrawling = wrapper.MultiPageCrawling;
            View.NumberOfHits = wrapper.NumberOfHits;
            View.OutputFileName = wrapper.OutputFileName;
            View.OutputFormat = wrapper.OutputFormat;
            View.OutputMode = wrapper.OutputMode;
            View.TargetUrls = wrapper.TargetUrls;
            View.ExpandExtractionTree();
        }

        public void RunInAutoMode()
        {
            // navigate to the url
            var url = View.FirstTargetURL;
            View.NavigateTo(url);

            // check if there is a submit form
            if (View.AutoFill)
            {
                var form = View.FormName;
                var input = View.FormInputName;
                var term = View.FormTerm;

                _screen.SubmitForm(form, input, term);
                FormSubmitted += DeixtoAgentPresenter_FormSubmitted;
                return;
            }

            ExecutePattern();
        }

        private void ExecutePattern()
        {
            // search for the extraction pattern in dom
            var pattern = View.ExtractionPattern;
            var domNodes = View.GetBodyTreeNodes();
            var extractionResult = _screen.Execute(pattern, domNodes);

            // list the results
            View.FocusOutputTabPage();
            AddOutputColumns(extractionResult);
            AddOutputResults(extractionResult);

            View.WritePageResults("Extraction Completed: " + extractionResult.RecordsCount.ToString() + " results!");
        }

        void DeixtoAgentPresenter_FormSubmitted()
        {
            ExecutePattern();
        }
    }
}

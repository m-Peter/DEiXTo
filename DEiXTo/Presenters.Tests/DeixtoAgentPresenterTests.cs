﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using DEiXTo.Services;
using System.Windows.Forms;
using DEiXTo.Models;
using System.Drawing;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class DeixtoAgentPresenterTests
    {
        private Mock<IDeixtoAgentView> view;
        private Mock<ISaveFileDialog> saveFileDialog;
        private Mock<IViewLoader> loader;
        private Mock<IEventHub> eventHub;
        private Mock<IDeixtoAgentScreen> screen;
        private DeixtoAgentPresenter presenter;

        [TestInitialize]
        public void SetUp()
        {
            view = new Mock<IDeixtoAgentView>();
            saveFileDialog = new Mock<ISaveFileDialog>();
            loader = new Mock<IViewLoader>();
            eventHub = new Mock<IEventHub>();
            screen = new Mock<IDeixtoAgentScreen>();

            presenter = new DeixtoAgentPresenter(view.Object, loader.Object, eventHub.Object, screen.Object);
        }

        [TestMethod]
        public void TestSubscribesForTheRegexAddedEvent()
        {
            // Assert
            eventHub.Verify(e => e.Subscribe<RegexAdded>(presenter));
        }

        [TestMethod]
        public void TestLoadStateImages()
        {
            // Assert
            screen.Verify(s => s.LoadStateImages());
            view.Verify(v => v.AddWorkingPatternImages(It.IsAny<ImageList>()));
            view.Verify(v => v.AddExtractionTreeImages(It.IsAny<ImageList>()));
        }

        [TestMethod]
        public void TestReceiveRegexAdded()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            nInfo.SourceIndex = 12;
            node.Tag = nInfo;
            var regexAdded = new RegexAdded(node);
            var element = CreateHtmlElement();
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);

            // Act
            presenter.Receive(regexAdded);

            // Assert
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
        }

        [TestMethod]
        public void TestLoadUrlsFromFile()
        {
            // Arrange
            var filter = "Text Files (*.txt)|";
            var urls = new string[] { "http://www.google.gr", "http://www.cs.teilar.gr" };
            var openFileDialog = new Mock<IOpenFileDialog>();
            screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(openFileDialog.Object);
            openFileDialog.Setup(o => o.ShowDialog()).Returns(DialogResult.OK);
            openFileDialog.Setup(o => o.Filename).Returns("urls_file");
            screen.Setup(s => s.LoadUrlsFromFile("urls_file")).Returns(urls);

            // Act
            presenter.LoadURLsFromFile();

            // Assert
            view.Verify(v => v.AppendTargetUrls(urls));
            view.VerifySet(v => v.TargetURLsFile = "urls_file");
        }

        [TestMethod]
        public void TestCancelLoadingUrlsFromFile()
        {
            // Arrange
            var filter = "Text Files (*.txt)|";
            var openFileDialog = new Mock<IOpenFileDialog>();
            screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(openFileDialog.Object);
            openFileDialog.Setup(o => o.ShowDialog()).Returns(DialogResult.Cancel);

            // Act
            presenter.LoadURLsFromFile();

            // Assert
            view.Verify(v => v.AppendTargetUrls(It.IsAny<string[]>()), Times.Never);
            view.VerifySet(v => v.TargetURLsFile = "", Times.Never);
        }

        [TestMethod]
        public void TestTuneExtractionPattern()
        {
            // Arrange
            view.Setup(v => v.ExtractionPatternSpecified).Returns(true);
            view.Setup(v => v.TargetURLSpecified).Returns(true);
            view.Setup(v => v.FirstTargetURL).Returns("http://www.google.gr/");

            // Act
            presenter.TunePattern();

            // Assert
            view.Verify(v => v.NavigateTo("http://www.google.gr/"));
        }

        [TestMethod]
        public void TestPatternMatchFound()
        {
            // Arrange
            view.Setup(v => v.ExtractionPatternSpecified).Returns(true);
            view.Setup(v => v.TargetURLSpecified).Returns(true);
            view.Setup(v => v.FirstTargetURL).Returns("http://www.google.gr/");
            var pattern = new TreeNode("H2");
            var domNodes = new TreeNode("BODY");
            domNodes.AddNode(new TreeNode("SECTION"));
            domNodes.AddNode(new TreeNode("H2"));
            view.Setup(v => v.GetExtractionPattern()).Returns(pattern);
            view.Setup(v => v.GetDOMTreeNodes()).Returns(domNodes.Nodes);
            var matchNode = new TreeNode("H2");
            screen.Setup(s => s.ScanDomTree(pattern)).Returns(matchNode);

            // Act
            presenter.TunePattern();
            presenter.BrowserCompleted();

            // Assert
            view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == matchNode.Text)));
            view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestPatternMatchNotFound()
        {
            // Arrange
            view.Setup(v => v.ExtractionPatternSpecified).Returns(true);
            view.Setup(v => v.TargetURLSpecified).Returns(true);
            view.Setup(v => v.FirstTargetURL).Returns("http://www.google.gr/");
            var pattern = new TreeNode("H2");
            var domNodes = new TreeNode("BODY");
            domNodes.AddNode(new TreeNode("SECTION"));
            domNodes.AddNode(new TreeNode("H1"));
            view.Setup(v => v.GetExtractionPattern()).Returns(pattern);
            view.Setup(v => v.GetDOMTreeNodes()).Returns(domNodes.Nodes);
            TreeNode matchNode = null;
            screen.Setup(s => s.ScanDomTree(pattern)).Returns(matchNode);

            // Act
            presenter.TunePattern();
            presenter.BrowserCompleted();

            // Assert
            view.Verify(v => v.ShowPatternMatchNotFoundMessage());
        }

        [TestMethod]
        public void TestTuningRequiresExtractionPattern()
        {
            // Arrange
            view.Setup(v => v.ExtractionPatternSpecified).Returns(false);

            // Act
            presenter.TunePattern();

            // Assert
            view.Verify(v => v.ShowSpecifyExtractionPatternMessage());
        }

        [TestMethod]
        public void TestTuningRequiresTargetUrl()
        {
            // Arrange
            view.Setup(v => v.ExtractionPatternSpecified).Returns(true);
            view.Setup(v => v.TargetURLSpecified).Returns(false);

            // Act
            presenter.TunePattern();

            // Assert
            view.Verify(v => v.ShowSpecifyTargetURLMessage());
        }


        
        [TestMethod]
        public void TestSelectTextFormatOutputFile()
        {
            // Arrange
            string filename = "output_file";
            var dialog = new Mock<ISaveFileDialog>();
            var format = Format.Text;
            view.Setup(v => v.OutputFileFormat).Returns(format);
            screen.Setup(s => s.GetSaveFileDialog(format)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);

            // Act
            presenter.SelectOutputFile();

            // Assert
            view.VerifySet(v => v.OutputFileName = filename);
        }

        [TestMethod]
        public void TestSelectXmlFormatOutputFile()
        {
            // Arrange
            string filename = "output_file";
            var dialog = new Mock<ISaveFileDialog>();
            var format = Format.XML;
            view.Setup(v => v.OutputFileFormat).Returns(format);
            screen.Setup(s => s.GetSaveFileDialog(format)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);

            // Act
            presenter.SelectOutputFile();

            // Assert
            view.VerifySet(v => v.OutputFileName = filename);
        }

        [TestMethod]
        public void TestSelectRssFormatOutputFile()
        {
            // Arrange
            string filename = "output_file";
            var dialog = new Mock<ISaveFileDialog>();
            var format = Format.RSS;
            view.Setup(v => v.OutputFileFormat).Returns(format);
            screen.Setup(s => s.GetSaveFileDialog(format)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);

            // Act
            presenter.SelectOutputFile();
            
            // Assert
            view.VerifySet(v => v.OutputFileName = filename);
        }

        [TestMethod]
        public void TestOutputFileSelectionAbortsForNegativeAnswer()
        {
            // Arrange
            string filename = "output_file";
            var dialog = new Mock<ISaveFileDialog>();
            var format = Format.XML;
            view.Setup(v => v.OutputFileFormat).Returns(format);
            screen.Setup(s => s.GetSaveFileDialog(format)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.Abort);
            dialog.Setup(d => d.Filename).Returns(filename);

            // Act
            presenter.SelectOutputFile();
            
            // Assert
            view.VerifyGet(v => v.OutputFileName, Times.Never);
        }

        [TestMethod]
        public void TestAddSiblingOrder()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            presenter.AddSiblingOrder(node);

            // Assert
            loader.Verify(l => l.LoadAddSiblingOrderView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestDeleteNodeFromWorkingPattern()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            presenter.DeleteNode(node);

            // Assert
            view.Verify(v => v.DeletePatternNode(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestUrlInputIsFilledUponTargetUrlSelection()
        {
            // Arrange
            string url = "http://www.google.gr";

            // Act
            presenter.TargetURLSelected(url);

            // Assert
            view.Verify(v => v.SetURLInput(It.Is<string>(s => s == url)));
        }

        [TestMethod]
        public void TestRemoveUrlFromTargetUrls()
        {
            // Arrange
            string url = "http://www.google.gr";
            view.Setup(v => v.TargetURLToAdd()).Returns(url);
            view.Setup(v => v.AskUserToRemoveURL()).Returns(true);

            // Act
            presenter.RemoveURLFromTargetURLs();

            // Assert
            view.Verify(v => v.RemoveTargetURL(It.Is<string>(s => s == url)));
            view.Verify(v => v.ClearAddURLInput());
        }

        [TestMethod]
        public void TestRemovingEmptyUrlFromTargetUrlsShowsWarningMessage()
        {
            // Arrange
            string url = "";
            view.Setup(v => v.TargetURLToAdd()).Returns(url);

            // Act
            presenter.RemoveURLFromTargetURLs();

            // Assert
            view.Verify(v => v.ShowSelectURLMessage());
        }

        [TestMethod]
        public void TestUrlDeletionIsAbortedIfUserDoesNotConfirm()
        {
            // Arrange
            string url = "http://www.google.gr";
            view.Setup(v => v.TargetURLToAdd()).Returns(url);
            view.Setup(v => v.AskUserToRemoveURL()).Returns(false);

            // Act
            presenter.RemoveURLFromTargetURLs();

            // Assert
            view.Verify(v => v.RemoveTargetURL(url), Times.Never);
        }

        [TestMethod]
        public void TestAddUrlToTargetUrls()
        {
            // Arrange
            string url = "http://www.google.gr";
            view.Setup(v => v.TargetURLToAdd()).Returns(url);

            // Act
            presenter.AddURLToTargetURLs();

            // Assert
            view.Verify(v => v.AppendTargetUrl(It.Is<string>(s => s == url)));
            view.Verify(v => v.ClearAddURLInput());
        }

        [TestMethod]
        public void TestEmptyUrlIsNotAddedToTargetUrls()
        {
            // Arrange
            string url = "";
            view.Setup(v => v.TargetURLToAdd()).Returns(url);

            // Act
            presenter.AddURLToTargetURLs();

            // Assert
            view.Verify(v => v.ShowEnterURLToAddMessage());
        }

        [TestMethod]
        public void TestRemoveRegexFromNode()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");
            node.NodeFont = new Font(FontFamily.GenericSerif, 8.25f, FontStyle.Bold);
            NodeInfo nInfo = new NodeInfo();
            nInfo.Regex = "/[a-z]{3}/";
            node.Tag = nInfo;

            // Act
            presenter.RemoveRegex(node);

            // Assert
            Assert.IsNull(node.GetRegex());
            var font = node.NodeFont;
            Assert.AreEqual(FontStyle.Regular, font.Style);
        }

        [TestMethod]
        public void TestRemoveLabelFromNode()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV:CONTAINER");
            NodeInfo nInfo = new NodeInfo();
            nInfo.Label = "CONTAINER";
            node.Tag = nInfo;

            // Act
            presenter.RemoveLabel(node);

            // Assert
            Assert.IsNull(node.GetLabel());
            Assert.AreEqual("DIV", node.Text);
        }

        [TestMethod]
        public void TestAddRegexToNode()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            presenter.AddRegex(node);

            // Assert
            loader.Verify(l => l.LoadRegexBuilderView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestAddLabelToNode()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            presenter.AddNewLabel(node);

            // Assert
            loader.Verify(l => l.LoadAddLabelView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestWindowClosingPublishesEvent()
        {
            // Act
            presenter.windowClosing();
            
            // Assert
            eventHub.Verify(e => e.Publish(It.IsAny<DeixtoAgentClosed>()));
        }

        [TestMethod]
        public void TestNodeStateChanges()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            presenter.NodeStateChanged(node, NodeState.Grayed);

            // Assert
            view.Verify(v => v.ApplyStateToNode(node, 3));
            Assert.AreEqual(NodeState.Grayed, node.GetState());
        }

        [TestMethod]
        public void TestNodeStateChangesToUnchecked()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");
            TreeNode n1 = new TreeNode("H1");
            TreeNode n2 = new TreeNode("P");
            node.Nodes.Add(n1);
            node.Nodes.Add(n2);
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            presenter.NodeStateChanged(node, NodeState.Unchecked);

            // Assert
            view.Verify(v => v.ApplyStateToNode(node, 5));
            view.Verify(v => v.ApplyStateToNode(n1, 5));
            view.Verify(v => v.ApplyStateToNode(n2, 5));
            Assert.AreEqual(NodeState.Unchecked, node.GetState());
        }

        [TestMethod]
        public void TestLevelDownWorkingPattern()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");
            TreeNode n1 = new TreeNode("H1");
            node.Nodes.Add(n1);
            NodeInfo nInfo = new NodeInfo();
            nInfo.IsRoot = false;
            node.Tag = nInfo;

            // Act
            presenter.LevelDownWorkingPattern(node);

            // Assert
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == "H1")));
            view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestCannotLevelDownFromRootNode()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            nInfo.IsRoot = true;
            node.Tag = nInfo;

            // Act
            presenter.LevelDownWorkingPattern(node);

            // Assert
            view.Verify(v => v.ShowCannotDeleteRootMessage());
        }

        [TestMethod]
        public void TestClearTreeViews()
        {
            // Arrange
            view.Setup(v => v.AskUserToClearTreeViews()).Returns(true);

            // Act
            presenter.ClearTreeViews(2);

            // Assert
            view.Verify(v => v.ClearAuxiliaryTree());
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.ClearSnapshotTree());
        }

        [TestMethod]
        public void TestClearTreeViewsAbortsIfUserDoesNotConfirm()
        {
            // Arrange
            view.Setup(v => v.AskUserToClearTreeViews()).Returns(false);

            // Act
            presenter.ClearTreeViews(2);

            // Assert
            view.Verify(v => v.ClearAuxiliaryTree(), Times.Never);
            view.Verify(v => v.ClearPatternTree(), Times.Never);
            view.Verify(v => v.ClearSnapshotTree(), Times.Never);
        }

        [TestMethod]
        public void TestClearTreeViewsDoesNotProceedIfCountIsZero()
        {
            // Act
            presenter.ClearTreeViews(0);

            // Assert
            view.Verify(v => v.ClearAuxiliaryTree(), Times.Never);
            view.Verify(v => v.ClearPatternTree(), Times.Never);
            view.Verify(v => v.ClearSnapshotTree(), Times.Never);
        }

        [TestMethod]
        public void TestMakeWorkingPatternFromSnapshot()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            presenter.MakeWorkingPatternFromSnapshot(node);

            // Assert
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == node.Text)));
            view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestDeleteNodeFromSnapshots()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            presenter.DeleteSnapshot(node);

            // Assert
            view.Verify(v => v.DeleteSnapshotInstance(node));
        }

        [TestMethod]
        public void TestCreateSnapshots()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            presenter.CreateSnapshot(node);

            // Assert
            view.Verify(v => v.FillSnapshotTree(It.Is<TreeNode>(n => n.Text.StartsWith("SNAP"))));
        }

        [TestMethod]
        public void TestCrawlingChanged()
        {
            // Act
            presenter.CrawlingChanged(true);

            // Assert
            view.Verify(v => v.ApplyVisibilityStateInCrawling(true));

            // Act
            presenter.CrawlingChanged(false);

            // Assert
            view.Verify(v => v.ApplyVisibilityStateInCrawling(false));
        }

        [TestMethod]
        public void TestAutoFillChanged()
        {
            // Act
            presenter.AutoFillChanged(true);

            // Assert
            view.Verify(v => v.ApplyVisibilityStateInAutoFill(true));

            // Act
            presenter.AutoFillChanged(false);

            // Assert
            view.Verify(v => v.ApplyVisibilityStateInAutoFill(false));
        }

        [TestMethod]
        public void TestBrowsesToUrlWhenEnterPressed()
        {
            // Arrange
            var args = new KeyEventArgs(Keys.Enter);
            var url = "http://www.google.gr/";
            view.Setup(v => v.Url).Returns(url);

            // Act
            presenter.KeyDownPress(args);

            // Assert
            view.Verify(v => v.NavigateTo("http://www.google.gr/"));
        }

        [TestMethod]
        public void TestNavigatesBackWhenAltAndLeftArePressed()
        {
            // Arrange
            var args = new KeyEventArgs(Keys.Alt | Keys.Left);

            // Act
            presenter.KeyDownPress(args);

            // Assert
            view.Verify(v => v.NavigateBack());
        }

        [TestMethod]
        public void TestNavigatesForwardWhenAltAndRightArePressed()
        {
            // Arrange
            var args = new KeyEventArgs(Keys.Alt | Keys.Right);

            // Act
            presenter.KeyDownPress(args);

            // Assert
            view.Verify(v => v.NavigateForward());
        }

        [TestMethod]
        public void TestLoadingOfUrlsIsAbortedIfUserDoesNotSelectFile()
        {
            // Arrange
            string filter = "Text Files (*.txt)|";
            var dialog = new Mock<IOpenFileDialog>();
            screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.Abort);

            // Act
            presenter.LoadURLsFromFile();

            // Assert
            screen.Verify(s => s.LoadUrlsFromFile("output_file"), Times.Never);
        }

        [TestMethod]
        public void TestSaveToDisk()
        {
            // Arrange
            string filter = "Text Files (*.txt)|";
            string extension = "txt";
            string filename = "extracted_records";
            var dialog = new Mock<ISaveFileDialog>();
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);

            // Act
            presenter.SaveToDisk();

            // Assert
            screen.Verify(s => s.WriteExtractedRecords(filename));
        }

        [TestMethod]
        public void TestSaveToDiskAbortsIfUserDoesNotSelectOutputFile()
        {
            // Arrange
            string filter = "Text Files (*.txt)|";
            string extension = "txt";
            var dialog = new Mock<ISaveFileDialog>();
            screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.Abort);

            // Act
            presenter.SaveToDisk();

            // Assert
            screen.Verify(v => v.WriteExtractedRecords(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void TestAddNextSiblingNode()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("h1");
            node.Nodes.Add(n1);

            var dom = new TreeNode("DIV");
            var d1 = new TreeNode("h1");
            var d2 = new TreeNode("p");
            dom.AddNode(d1);
            dom.AddNode(d2);

            screen.Setup(s => s.GetDomNode(n1)).Returns(d1);

            // Act
            presenter.AddNextSibling(n1);

            // Assert
            Assert.AreEqual(2, node.Nodes.Count);
            Assert.AreEqual(d2.Text, node.Nodes[1].Text);
        }

        [TestMethod]
        public void TestAddPreviousSiblingNode()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("p");
            node.Nodes.Add(n1);

            var dom = new TreeNode("DIV");
            var d1 = new TreeNode("h1");
            var d2 = new TreeNode("p");
            dom.AddNode(d1);
            dom.AddNode(d2);

            screen.Setup(s => s.GetDomNode(n1)).Returns(d2);

            // Act
            presenter.AddPreviousSibling(n1);

            // Assert
            Assert.AreEqual(2, node.Nodes.Count);
            Assert.AreEqual(d1.Text, node.Nodes[0].Text);
        }

        [TestMethod]
        public void TestLoadExtractionPattern()
        {
            // Arrange
            string filter = "XML Files (*.xml)|";
            string filename = "extraction_pattern";
            var node = new TreeNode("DIV");
            var dialog = new Mock<IOpenFileDialog>();
            screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            screen.Setup(s => s.LoadExtractionPattern(filename)).Returns(node);

            // Act
            presenter.LoadExtractionPattern();
            
            // Assert
            view.Verify(v => v.FillExtractionPattern(node));
            view.Verify(v => v.ExpandExtractionTree());
        }

        [TestMethod]
        public void TestSaveExtractionPattern()
        {
            // Arrange
            string filter = "XML Files (*.xml)|";
            string extension = "xml";
            string filename = "extraction_pattern";
            var dom = new TreeNode("DIV");
            var d1 = new TreeNode("h1");
            var d2 = new TreeNode("p");
            dom.AddNode(d1);
            dom.AddNode(d2);
            var dialog = new Mock<ISaveFileDialog>();
            screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            view.Setup(v => v.GetPatternTreeNodes()).Returns(dom.Nodes);

            // Act
            presenter.SaveExtractionPattern();

            // Assert
            screen.Verify(s => s.SaveExtractionPattern(filename, dom.Nodes));
        }

        [TestMethod]
        public void TestOutputResultSelected()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("H1");
            var domNode = new TreeNode("H1");
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            view.Setup(v => v.HighlightModeEnabled).Returns(true);
            screen.Setup(s => s.GetDomNode(node)).Returns(domNode);
            view.Setup(v => v.CanAutoScroll).Returns(true);

            // Act
            presenter.OutputResultSelected(true, node);

            // Assert
            screen.Setup(s => s.HighlightElement(element));
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
            view.Verify(v => v.SelectDOMNode(domNode));
        }

        [TestMethod]
        public void TestLevelUpWorkingPattern()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("LI");
            var dom = new TreeNode("DIV");
            var d1 = new TreeNode("LI");
            dom.Nodes.Add(d1);
            screen.Setup(s => s.GetDomNode(node)).Returns(d1);
            screen.Setup(s => s.GetElementFromNode(dom)).Returns(element);

            // Act
            presenter.LevelUpWorkingPattern(node);

            // Assert
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == "LI")));
            view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestSimplifyDomTree()
        {
            // Arrange
            string url = "htt://www.google.gr";
            var document = CreateHtmlDocument();
            var node = new TreeNode("HTML");
            string[] tags = new string[] { "em" };
            view.Setup(v => v.IgnoredTags).Returns(tags);
            view.Setup(v => v.GetHtmlDocument()).Returns(document);
            screen.Setup(s => s.BuildSimplifiedDOM(tags)).Returns(node);
            view.Setup(v => v.Url).Returns(url);

            // Act
            presenter.SimplifyDOMTree();

            // Assert
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.ClearSnapshotTree());
            view.Verify(v => v.ClearAuxiliaryTree());
            view.Verify(v => v.ClearDOMTree());
            screen.Verify(s => s.CreateDocument(document));
            view.Verify(v => v.FillDomTree(node));
            view.Verify(v => v.ClearTargetURLs());
            view.Verify(v => v.AppendTargetUrl(url));
        }

        [TestMethod]
        public void TestSimplifyDomTreeNoTagSelected()
        {
            // Arrange
            view.Setup(v => v.IgnoredTags).Returns(new string[0]);

            // Act
            presenter.SimplifyDOMTree();

            // Assert
            view.Verify(v => v.ShowNoTagSelectedMessage());
        }

        [TestMethod]
        public void TestAuxiliaryPatternTextNodeClick()
        {
            // Arrange
            var node = new TreeNode("TEXT");
            NodeInfo nInfo = new NodeInfo();
            nInfo.IsTextNode = true;
            node.Tag = nInfo;
            
            // Act
            presenter.AuxiliaryPatternNodeClick(node);
            
            // Assert
            view.Verify(v => v.FillTextNodeElementInfo(node));
        }

        [TestMethod]
        public void TestAuxiliaryPatternNodeClick()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("DIV");
            var domNode = new TreeNode("DIV");
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            screen.Setup(s => s.GetDomNode(node)).Returns(domNode);

            // Act
            presenter.AuxiliaryPatternNodeClick(node);

            // Assert
            screen.Verify(s => s.HighlightElement(element));
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
            view.Verify(v => v.SelectDOMNode(domNode));
        }

        [TestMethod]
        public void TestCreateAuxiliaryPatternFromDocument()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("DIV");
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(node);

            // Act
            presenter.CreateAuxiliaryPatternFromDocument(element);

            // Assert
            view.Verify(v => v.FocusAuxiliaryTabPage());
            view.Verify(v => v.ClearAuxiliaryTree());
            view.Verify(v => v.FillAuxiliaryTree(It.Is<TreeNode>(n => n.Text == node.Text)));
            view.Verify(v => v.ExpandAuxiliaryTree());
        }

        [TestMethod]
        public void TestCreateWorkingPatternFromDocument()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("DIV");
            var domNode = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            domNode.Tag = nInfo;
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(domNode);

            // Act
            presenter.CreateWorkingPatternFromDocument(element);

            // Assert
            view.Verify(v => v.ClearPatternTree());
            Assert.IsTrue(domNode.IsRoot());
            view.Verify(v => v.SetNodeFont(It.Is<TreeNode>(n => n.Text == "DIV")));
            view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == "DIV")));
            view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestWorkingPatternRightNodeClick()
        {
            // Arrange
            var node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            var attributes = new TagAttributeCollection();
            attributes.Add(new TagAttribute { Name = "id", Value = "main-wrapper" });
            nInfo.Attributes = attributes;
            node.Tag = nInfo;
            var domNode = new TreeNode("DIV");
            var element = CreateHtmlElement();
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            view.Setup(v => v.HighlightModeEnabled).Returns(true);
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(domNode);

            // Act
            presenter.WorkingPatternNodeClick(node, MouseButtons.Right);

            // Assert
            view.Verify(v => v.SetAdjustContextMenuFor(node));
            screen.Verify(s => s.HighlightElement(element));
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
            view.Verify(v => v.SelectDOMNode(domNode));
        }

        [TestMethod]
        public void TestWorkingPatternNodeClick()
        {
            // Arrange
            var node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            var attributes = new TagAttributeCollection();
            attributes.Add(new TagAttribute { Name = "id", Value = "main-wrapper" });
            nInfo.Attributes = attributes;
            node.Tag = nInfo;
            var domNode = new TreeNode("DIV");
            var element = CreateHtmlElement();
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            view.Setup(v => v.HighlightModeEnabled).Returns(true);
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(domNode);

            // Act
            presenter.WorkingPatternNodeClick(node, MouseButtons.Left);

            // Assert
            screen.Verify(s => s.HighlightElement(element));
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
            view.Verify(v => v.SelectDOMNode(domNode));
        }

        [TestMethod]
        public void TestWorkingPatternTextNodeClick()
        {
            // Arrange
            var node = new TreeNode("TEXT");
            NodeInfo nInfo = new NodeInfo();
            nInfo.IsTextNode = true;
            node.Tag = nInfo;

            // Act
            presenter.WorkingPatternNodeClick(node, MouseButtons.Left);

            //Assert
            view.Verify(v => v.FillTextNodeElementInfo(node));
        }

        [TestMethod]
        public void TestCreateAuxiliaryPattern()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            presenter.CreateAuxiliaryPattern(node);

            // Assert
            view.Verify(v => v.FocusAuxiliaryTabPage());
            view.Verify(v => v.ClearAuxiliaryTree());
            view.Verify(v => v.FillAuxiliaryTree(It.Is<TreeNode>(n => n.Text == node.Text)));
            view.Verify(v => v.ExpandAuxiliaryTree());
        }

        [TestMethod]
        public void TestCreateWorkingPattern()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var domNode = new TreeNode("DIV");
            screen.Setup(s => s.GetDomNode(node)).Returns(domNode);

            // Act
            presenter.CreateWorkingPattern(node);

            // Assert
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.SetNodeFont(It.Is<TreeNode>(n => n.Text == "DIV")));
            view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == "DIV")));
            view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestDOMNodeClick()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("DIV");
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            view.Setup(v => v.CanAutoScroll).Returns(true);

            // Act
            presenter.DOMNodeClick(node, MouseButtons.Left);
            
            // Assert
            screen.Verify(s => s.HighlightElement(element));
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
        }

        [TestMethod]
        public void TestDOMNodeRightClick()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("DIV");
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            view.Setup(v => v.CanAutoScroll).Returns(true);

            // Act
            presenter.DOMNodeClick(node, MouseButtons.Right);

            // Assert
            view.Verify(v => v.SetContextMenuFor(node));
            screen.Verify(s => s.HighlightElement(element));
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
        }

        [TestMethod]
        public void TestDOMTextNodeClick()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("TEXT");
            NodeInfo nInfo = new NodeInfo();
            nInfo.IsTextNode = true;
            node.Tag = nInfo;
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            view.Setup(v => v.CanAutoScroll).Returns(true);

            // Act
            presenter.DOMNodeClick(node, MouseButtons.Left);

            // Assert
            view.Verify(v => v.FillTextNodeElementInfo(node));
        }

        [TestMethod]
        public void TestDocumentMouseLeave()
        {
            // Arrange
            var element = CreateHtmlElement();
            view.Setup(v => v.HighlightModeEnabled).Returns(true);

            // Act
            presenter.DocumentMouseLeave(element);

            // Assert
            screen.Verify(s => s.RemoveHighlighting(element));
            view.Verify(v => v.ClearElementInfo());
        }

        [TestMethod]
        public void TestDocumentMouseOver()
        {
            // Arrange
            var element = CreateHtmlElement();
            var node = new TreeNode("DIV");
            view.Setup(v => v.HighlightModeEnabled).Returns(true);
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(node);

            // Act
            presenter.DocumentMouseOver(element);

            // Assert
            screen.Verify(s => s.HighlightElement(element));
            view.Verify(v => v.SelectDOMNode(node));
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
        }

        [TestMethod]
        public void TestBrowserCompleted()
        {
            // Arrange
            string url = "http://www.google.gr";
            var node = new TreeNode("HTML");
            var document = CreateHtmlDocument();
            view.Setup(v => v.CrawlingEnabled).Returns(false);
            view.Setup(v => v.GetHtmlDocument()).Returns(document);
            view.Setup(v => v.GetDocumentUrl()).Returns(url);
            screen.Setup(s => s.BuildDom()).Returns(node);

            // Act
            presenter.BrowserCompleted();

            // Assert
            screen.Verify(s => s.ClearStyling());
            view.Verify(v => v.ClearSnapshotTree());
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.ClearAuxiliaryTree());
            view.Verify(v => v.ClearDOMTree());
            screen.Verify(s => s.CreateDocument(document));
            view.Verify(v => v.ClearTargetURLs());
            view.Verify(v => v.AppendTargetUrl(url));
            view.Verify(v => v.UpdateDocumentUrl());
            view.Verify(v => v.FillDomTree(node));
            view.Verify(v => v.AttachDocumentEvents());
        }

        [TestMethod]
        public void TestExecuteRuleWithNoWorkingPattern()
        {
            // Arrange
            TreeNode node = null;
            view.Setup(v => v.GetWorkingPattern()).Returns(node);

            // Act
            presenter.ExecuteRule();

            // Assert
            view.Verify(v => v.ShowSpecifyPatternMessage());
        }

        [TestMethod]
        public void TestExecuteRuleSingePage()
        {
            // Arrange
            IList<Result> results = new List<Result>();
            var extraction = new Mock<IExtraction>();
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("H1");
            var n2 = new TreeNode("P");
            node.AddNode(n1);
            node.AddNode(n2);
            var domNodes = node.Nodes;
            view.Setup(v => v.GetWorkingPattern()).Returns(node);
            view.Setup(v => v.GetBodyTreeNodes()).Returns(domNodes);
            screen.Setup(s => s.Execute(node, domNodes)).Returns(extraction.Object);
            extraction.Setup(e => e.RecordsCount).Returns(4);
            extraction.Setup(e => e.VariablesCount).Returns(3);
            extraction.Setup(e => e.OutputVariableLabels).Returns(new List<string>());
            extraction.Setup(e => e.ExtractedRecords).Returns(results);

            // Act
            presenter.ExecuteRule();

            // Assert
            string message = "Extraction Completed: 4 results!";
            view.Verify(v => v.WritePageResults(message));
            view.Verify(v => v.FillExtractionPattern(It.Is<TreeNode>(n => n.Text == "DIV")));
            view.Verify(v => v.ExpandExtractionTree());
        }

        [TestMethod]
        public void TestSaveWrapper()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("H1");
            var n2 = new TreeNode("P");
            node.AddNode(n1);
            node.AddNode(n2);
            var nodes = node.Nodes;
            string filter = "Wrapper Project Files (*.wpf)|";
            string extension = "wpf";
            string filename = "deixto_wrapper";
            var dialog = new Mock<ISaveFileDialog>();
            screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            view.Setup(v => v.TargetUrls).Returns(new string[] { "http://www.teilar.gr" });
            view.Setup(v => v.TargetURLsFile).Returns("");
            view.Setup(v => v.GetExtractionPatternNodes()).Returns(nodes);

            // Act
            presenter.SaveWrapper();

            // Assert
            screen.Verify(s => s.SaveWrapper(It.IsAny<DeixtoWrapper>(), nodes, filename));
        }

        [TestMethod]
        public void TestSavingWrapperRequiresOneInputSource()
        {
            // Arrange
            view.Setup(v => v.TargetUrls).Returns(new string[0]);
            view.Setup(v => v.TargetURLsFile).Returns(String.Empty);

            // Act
            presenter.SaveWrapper();

            // Assert
            view.Verify(v => v.ShowSpecifyInputSourceMessage());
        }

        [TestMethod]
        public void TestSavingWrapperAcceptsOnlyOneInputSource()
        {
            // Arrange
            view.Setup(v => v.TargetUrls).Returns(new string[] { "http://www.teilar.gr" });
            view.Setup(v => v.TargetURLsFile).Returns("some_file");

            // Act
            presenter.SaveWrapper();

            // Assert
            view.Verify(v => v.ShowSelectOneInputSourceMessage());
        }

        [TestMethod]
        public void TestSavingWrapperRequiresExtractionPattern()
        {
            // Arrange
            var node = new TreeNode("DIV");
            view.Setup(v => v.TargetURLsFile).Returns("some_file");
            view.Setup(v => v.GetExtractionPatternNodes()).Returns(node.Nodes);

            // Act
            presenter.SaveWrapper();

            // Assert
            view.Verify(v => v.ShowSpecifyExtractionPatternMessage());
        }

        [TestMethod]
        public void TestLoadWrapper()
        {
            // Arrange
            var wrapper = CreateWrapper();
            var filter = "Wrapper Project Files (*.wpf)|";
            var filename = "wrapper_file";
            var dialog = new Mock<IOpenFileDialog>();
            screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            screen.Setup(s => s.LoadWrapper(filename)).Returns(wrapper);

            // Act
            presenter.LoadWrapper();

            // Assert
            view.VerifySet(v => v.AutoFill = wrapper.AutoSubmitForm);
            view.VerifySet(v => v.Delay = wrapper.Delay);
            view.VerifySet(v => v.ExtractionPattern = wrapper.ExtractionPattern.RootNode);
            view.VerifySet(v => v.ExtractNativeUrl = wrapper.ExtractNativeUrl);
            view.VerifySet(v => v.FormInputName = wrapper.InputName);
            view.VerifySet(v => v.FormName = wrapper.FormName);
            view.VerifySet(v => v.FormTerm = wrapper.SearchQuery);
            view.VerifySet(v => v.HtmlNextLink = wrapper.HtmlNextLink);
            view.VerifySet(v => v.IgnoredTags = wrapper.IgnoredHtmlTags);
            view.VerifySet(v => v.InputFile = wrapper.UrlsInputFile);
            view.VerifySet(v => v.MaxCrawlingDepth = wrapper.MaxCrawlingDepth);
            view.VerifySet(v => v.MultiPageCrawling = wrapper.MultiPageCrawling);
            view.VerifySet(v => v.NumberOfHits = wrapper.NumberOfHits);
            view.VerifySet(v => v.OutputFileName = wrapper.OutputFileName);
            view.VerifySet(v => v.OutputFormat = wrapper.OutputFileFormat);
            view.VerifySet(v => v.OutputMode = wrapper.OutputFileMode);
            view.VerifySet(v => v.TargetUrls = wrapper.TargetUrls);
            view.Verify(v => v.ExpandExtractionTree());
        }

        // HELPER METHODS
        private DeixtoWrapper CreateWrapper()
        {
            var wrapper = new DeixtoWrapper();

            wrapper.AutoSubmitForm = true;
            wrapper.Delay = 2;
            wrapper.ExtractionPattern = new ExtractionPattern(new TreeNode("DIV"));
            wrapper.ExtractNativeUrl = true;
            wrapper.InputName = "repo";
            wrapper.FormName = "q";
            wrapper.SearchQuery = "rails";
            wrapper.HtmlNextLink = "Next";
            wrapper.IgnoredHtmlTags = new string[] { "<B>", "<I>", "<EM>" };
            wrapper.UrlsInputFile = "some_input_file";
            wrapper.MaxCrawlingDepth = 3;
            wrapper.MultiPageCrawling = true;
            wrapper.NumberOfHits = 4;
            wrapper.OutputFileName = "some_output_file";
            wrapper.OutputFileFormat = Format.XML;
            wrapper.OutputFileMode = OutputMode.Append;
            wrapper.TargetUrls = new string[] { "url1", "url2" };

            return wrapper;
        }

        private HtmlElement CreateHtmlElement()
        {
            WebBrowser browser = new WebBrowser();
            browser.DocumentText = "<li></li>";
            browser.Show();
            var doc = browser.Document;
            doc.Write("<li></li>");
            var element = doc.GetElementsByTagName("li");
            var elem = element[0];

            return elem;
        }

        private HtmlElement CreateHtmlElementWithAttribute()
        {
            WebBrowser browser = new WebBrowser();
            browser.DocumentText = "<a href='projects' id='projects_link'>Projects</a>";
            browser.Show();
            var doc = browser.Document;
            doc.Write("<a href='projects' id='projects_link'>Projects</a>");
            var element = doc.GetElementsByTagName("a");
            var elem = element[0];

            return elem;
        }

        private HtmlDocument CreateHtmlDocument()
        {
            WebBrowser browser = new WebBrowser();
            browser.DocumentText = "<html></html>";
            browser.Show();
            var doc = browser.Document;
            doc.Write("<html></html>");

            return browser.Document;
        }
    }
}

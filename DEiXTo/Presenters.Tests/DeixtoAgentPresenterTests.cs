using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using DEiXTo.Services;
using System.Windows.Forms;
using DEiXTo.Models;
using System.Drawing;
using mshtml;
using DEiXTo.TestHelpers;

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
            var element = TestUtils.CreateHtmlElement();
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
            var filename = "output_file";
            var openFileDialog = new Mock<ISaveFileDialog>();
            var format = Format.Text;
            view.Setup(v => v.OutputFileFormat).Returns(format);
            screen.Setup(s => s.GetSaveFileDialog(format)).Returns(openFileDialog.Object);
            openFileDialog.Setup(o => o.ShowDialog()).Returns(DialogResult.OK);
            openFileDialog.Setup(o => o.Filename).Returns(filename);

            // Act
            presenter.SelectOutputFile();

            // Assert
            view.VerifySet(v => v.OutputFileName = filename);
        }

        [TestMethod]
        public void TestSelectXmlFormatOutputFile()
        {
            // Arrange
            var filename = "output_file";
            var openFileDialog = new Mock<ISaveFileDialog>();
            var format = Format.XML;
            view.Setup(v => v.OutputFileFormat).Returns(format);
            screen.Setup(s => s.GetSaveFileDialog(format)).Returns(openFileDialog.Object);
            openFileDialog.Setup(o => o.ShowDialog()).Returns(DialogResult.OK);
            openFileDialog.Setup(o => o.Filename).Returns(filename);

            // Act
            presenter.SelectOutputFile();

            // Assert
            view.VerifySet(v => v.OutputFileName = filename);
        }

        [TestMethod]
        public void TestSelectRssFormatOutputFile()
        {
            // Arrange
            var filename = "output_file";
            var openFileDialog = new Mock<ISaveFileDialog>();
            var format = Format.RSS;
            view.Setup(v => v.OutputFileFormat).Returns(format);
            screen.Setup(s => s.GetSaveFileDialog(format)).Returns(openFileDialog.Object);
            openFileDialog.Setup(o => o.ShowDialog()).Returns(DialogResult.OK);
            openFileDialog.Setup(o => o.Filename).Returns(filename);

            // Act
            presenter.SelectOutputFile();

            // Assert
            view.VerifySet(v => v.OutputFileName = filename);
        }

        [TestMethod]
        public void TestOutputFileSelectionAbortsForNegativeAnswer()
        {
            // Arrange
            var filename = "output_file";
            var openFileDialog = new Mock<ISaveFileDialog>();
            var format = Format.XML;
            view.Setup(v => v.OutputFileFormat).Returns(format);
            screen.Setup(s => s.GetSaveFileDialog(format)).Returns(openFileDialog.Object);
            openFileDialog.Setup(o => o.ShowDialog()).Returns(DialogResult.Abort);
            openFileDialog.Setup(o => o.Filename).Returns(filename);

            // Act
            presenter.SelectOutputFile();

            // Assert
            view.VerifyGet(v => v.OutputFileName, Times.Never);
        }

        [TestMethod]
        public void TestEnableHighlighting()
        {
            // Act
            presenter.EnableHighlighting();

            // Assert
            view.Verify(v => v.EnableHighlighting());
        }

        [TestMethod]
        public void TestDisableHighlighting()
        {
            // Act
            presenter.DisableHighlighting();

            // Assert
            view.Verify(v => v.DisableHighlighting());
        }

        [TestMethod]
        public void TestSaveToDisk()
        {
            // Arrange
            var filter = "Text Files (*.txt)|";
            var extension = "txt";
            var filename = "extracted_records";
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
        public void TestSavingToDiskAbortsIfUserDoesNotSelectOutputFile()
        {
            // Arrange
            var filter = "Text Files (*.txt)|";
            var extension = "txt";
            var dialog = new Mock<ISaveFileDialog>();
            screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.Cancel);

            // Act
            presenter.SaveToDisk();

            // Assert
            screen.Verify(v => v.WriteExtractedRecords(""), Times.Never);
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
        public void TestAddAttributeConstraint()
        {
            // Arrange
            var node = new TreeNode("SECTION");

            // Act
            presenter.AddAttributeConstraint(node);

            // Assert
            loader.Verify(l => l.LoadAddAttributeConstraintView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestAddNextSiblingNode()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("H1");
            node.Nodes.Add(n1);
            var dom = new TreeNode("DIV");
            var d1 = new TreeNode("H1");
            var d2 = new TreeNode("P");
            dom.AddNode(d1);
            dom.AddNode(d2);

            screen.Setup(s => s.GetDomNode(n1)).Returns(d1);

            // Act
            presenter.AddNextSibling(n1);

            // Assert
            Assert.AreEqual(2, node.Nodes.Count);
            Assert.AreEqual("P", node.Nodes[1].Text);
        }

        [TestMethod]
        public void TestAddPreviousSiblingNode()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("P");
            node.Nodes.Add(n1);

            var dom = new TreeNode("DIV");
            var d1 = new TreeNode("H1");
            var d2 = new TreeNode("P");
            dom.AddNode(d1);
            dom.AddNode(d2);

            screen.Setup(s => s.GetDomNode(n1)).Returns(d2);

            // Act
            presenter.AddPreviousSibling(n1);

            // Assert
            Assert.AreEqual(2, node.Nodes.Count);
            Assert.AreEqual("H1", node.Nodes[0].Text);
        }

        [TestMethod]
        public void TestDeleteNodeFromWorkingPattern()
        {
            // Arrange
            var pattern = new TreeNode("DIV");
            var n1 = new TreeNode("H1");
            var n2 = new TreeNode("P");
            pattern.AddNode(n1);
            pattern.AddNode(n2);
            view.Setup(v => v.GetWorkingPattern()).Returns(pattern);

            // Act
            presenter.DeleteNode(n2);

            // Assert
            Assert.AreEqual(1, pattern.Nodes.Count);
            Assert.AreEqual("H1", pattern.Nodes[0].Text);
        }

        [TestMethod]
        public void TestLoadExtractionPattern()
        {
            // Arrange
            var filter = "XML Files (*.xml)|";
            var filename = "extraction_pattern";
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
        public void TestLoadingExtractionPatternAbortsWithNegativeAnswer()
        {
            // Arrange
            var filter = "XML Files (*.xml)|";
            var dialog = new Mock<IOpenFileDialog>();
            screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.Cancel);

            // Act
            presenter.LoadExtractionPattern();

            // Assert
            view.Verify(v => v.FillExtractionPattern(new TreeNode()), Times.Never);
            view.Verify(v => v.ExpandExtractionTree(), Times.Never);
        }

        [TestMethod]
        public void TestSaveExtractionPattern()
        {
            // Arrange
            var filter = "XML Files (*.xml)|";
            var extension = "xml";
            var filename = "extraction_pattern";
            var pattern = new TreeNode("DIV");
            var n1 = new TreeNode("h1");
            var n2 = new TreeNode("p");
            pattern.AddNode(n1);
            pattern.AddNode(n2);
            var dialog = new Mock<ISaveFileDialog>();
            screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            view.Setup(v => v.GetWorkingPattern()).Returns(pattern);

            // Act
            presenter.SaveExtractionPattern();

            // Assert
            screen.Verify(s => s.SaveExtractionPattern(filename, pattern));
        }

        [TestMethod]
        public void TestSavingExtractionPatternAbortsWithNegativeAnswer()
        {
            // Arrange
            var filter = "XML Files (*.xml)|";
            var extension = "xml";
            var dialog = new Mock<ISaveFileDialog>();
            screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.Cancel);

            // Act
            presenter.SaveExtractionPattern();

            // Assert
            screen.Verify(s => s.SaveExtractionPattern("", null), Times.Never);
        }

        [TestMethod]
        public void TestTargetUrlSelected()
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
            var url = "http://www.google.gr";
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
            var url = string.Empty;
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
            var url = "http://www.google.gr";
            view.Setup(v => v.TargetURLToAdd()).Returns(url);
            view.Setup(v => v.AskUserToRemoveURL()).Returns(false);

            // Act
            presenter.RemoveURLFromTargetURLs();

            // Assert
            view.Verify(v => v.RemoveTargetURL(url), Times.Never);
            view.Verify(v => v.ClearAddURLInput(), Times.Never);
        }

        [TestMethod]
        public void TestAddUrlToTargetUrls()
        {
            // Arrange
            var url = "http://www.google.gr";
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
            var url = string.Empty;
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
            var node = new TreeNode("DIV");
            node.NodeFont = new Font(FontFamily.GenericSerif, 8.25f, FontStyle.Bold);
            var nInfo = new NodeInfo();
            nInfo.Regex = "/[a-z]{3}/";
            node.Tag = nInfo;

            // Act
            presenter.RemoveRegex(node);

            // Assert
            Assert.IsNull(node.GetRegex());
        }

        [TestMethod]
        public void TestRemoveLabelFromNode()
        {
            // Arrange
            var node = new TreeNode("DIV:CONTAINER");
            var nInfo = new NodeInfo();
            nInfo.Label = "CONTAINER";
            node.Tag = nInfo;

            // Act
            presenter.RemoveLabel(node);

            // Assert
            Assert.IsNull(node.GetLabel());
            Assert.AreEqual("DIV", node.Text);
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
        public void TestAddRegexToNode()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            presenter.AddRegex(node);

            // Assert
            loader.Verify(l => l.LoadRegexBuilderView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestAddLabelToNode()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            presenter.AddNewLabel(node);

            // Assert
            loader.Verify(l => l.LoadAddLabelView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestOutputResultSelected()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var domNode = new TreeNode("DIV");
            var element = TestUtils.CreateHtmlElement();
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            screen.Setup(s => s.GetDomNode(node)).Returns(domNode);

            // Act
            presenter.OutputResultSelected(true, node);

            // Assert
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
            view.Verify(v => v.SelectDOMNode(domNode));
        }

        [TestMethod]
        public void TestOutputResultHighlightsElementIfEnabled()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var domNode = new TreeNode("DIV");
            var element = TestUtils.CreateHtmlElement();
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            screen.Setup(s => s.GetDomNode(node)).Returns(domNode);
            view.Setup(v => v.HighlightModeEnabled).Returns(true);

            // Act
            presenter.OutputResultSelected(true, node);

            // Assert
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
            screen.Verify(s => s.HighlightElement(element));
            view.Verify(v => v.SelectDOMNode(domNode));
        }

        [TestMethod]
        public void TestChangeNodeStateToGrayed()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            presenter.NodeStateChanged(node, NodeState.Grayed);

            // Assert
            view.Verify(v => v.ApplyStateToNode(node, 3));
            Assert.AreEqual(NodeState.Grayed, node.GetState());
        }

        [TestMethod]
        public void TestChangeNodeStateToChecked()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            presenter.NodeStateChanged(node, NodeState.Checked);
            view.Verify(v => v.ApplyStateToNode(node, 0));
            Assert.AreEqual(NodeState.Checked, node.GetState());
        }

        [TestMethod]
        public void TestChangeNodeStateToCheckedImplied()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            presenter.NodeStateChanged(node, NodeState.CheckedImplied);
            view.Verify(v => v.ApplyStateToNode(node, 1));
            Assert.AreEqual(NodeState.CheckedImplied, node.GetState());
        }

        [TestMethod]
        public void TestChangeNodeStateToCheckedSource()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            presenter.NodeStateChanged(node, NodeState.CheckedSource);
            view.Verify(v => v.ApplyStateToNode(node, 2));
            Assert.AreEqual(NodeState.CheckedSource, node.GetState());
        }

        [TestMethod]
        public void TestChangeNodeStateToGrayedImplied()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            presenter.NodeStateChanged(node, NodeState.GrayedImplied);
            view.Verify(v => v.ApplyStateToNode(node, 4));
            Assert.AreEqual(NodeState.GrayedImplied, node.GetState());
        }

        [TestMethod]
        public void TestChangeNodeStateToUnchecked()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            presenter.NodeStateChanged(node, NodeState.Unchecked);
            view.Verify(v => v.ApplyStateToNode(node, 5));
            Assert.AreEqual(NodeState.Unchecked, node.GetState());
        }

        [TestMethod]
        public void TestNodeStateChangesToUnchecked()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("H1");
            var n2 = new TreeNode("P");
            node.Nodes.Add(n1);
            node.Nodes.Add(n2);
            var nInfo = new NodeInfo();
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
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("H1");
            node.Nodes.Add(n1);
            var nInfo = new NodeInfo();
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
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            nInfo.IsRoot = true;
            node.Tag = nInfo;

            // Act
            presenter.LevelDownWorkingPattern(node);

            // Assert
            view.Verify(v => v.ShowCannotDeleteRootMessage());
        }

        [TestMethod]
        public void TestLevelUpWorkingPattern()
        {
            // Arrange
            var element = TestUtils.CreateUlElement();
            var node = new TreeNode("LI");
            var dom = new TreeNode("UL");
            var d1 = new TreeNode("LI");
            dom.Nodes.Add(d1);
            screen.Setup(s => s.GetDomNode(node)).Returns(d1);
            screen.Setup(s => s.GetElementFromNode(dom)).Returns(element);

            // Act
            presenter.LevelUpWorkingPattern(node);

            // Assert
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == "UL")));
            view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestExecuteRuleRequiresWorkingPattern()
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
            var message = "Extraction Completed: 4 results!";
            view.Verify(v => v.ClearExtractionPattern());
            view.Verify(v => v.FocusOutputTabPage());
            view.Verify(v => v.WritePageResults(message));
            view.Verify(v => v.FillExtractionPattern(It.Is<TreeNode>(n => n.Text == "DIV")));
            view.Verify(v => v.ExpandExtractionTree());
        }

        [TestMethod]
        public void TestExecuteRuleMultiPageRequiresHtmlLink()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("H1");
            var n2 = new TreeNode("P");
            node.AddNode(n1);
            node.AddNode(n2);
            view.Setup(v => v.GetWorkingPattern()).Returns(node);
            view.Setup(v => v.CrawlingEnabled).Returns(true);
            view.Setup(v => v.HtmlLink()).Returns("");
            view.Setup(v => v.CrawlingDepth()).Returns(2);

            // Act
            presenter.ExecuteRule();

            // Assert
            view.Verify(v => v.ClearExtractionPattern());
            view.Verify(v => v.FocusOutputTabPage());
            view.Verify(v => v.ShowEmptyLinkMessage());
        }

        [TestMethod]
        public void TestExecuteRuleMultiPageRequiresPositiveDepth()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var n1 = new TreeNode("H1");
            var n2 = new TreeNode("P");
            node.AddNode(n1);
            node.AddNode(n2);
            view.Setup(v => v.GetWorkingPattern()).Returns(node);
            view.Setup(v => v.CrawlingEnabled).Returns(true);
            view.Setup(v => v.HtmlLink()).Returns("Next");
            view.Setup(v => v.CrawlingDepth()).Returns(-2);

            // Act
            presenter.ExecuteRule();

            // Assert
            view.Verify(v => v.ClearExtractionPattern());
            view.Verify(v => v.FocusOutputTabPage());
            view.Verify(v => v.ShowInvalidDepthMessage());
        }

        [TestMethod]
        public void TestExecuteRuleMultiPage()
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
            var linkElement = TestUtils.CreateLinkElement();
            view.Setup(v => v.GetWorkingPattern()).Returns(node);
            view.Setup(v => v.CrawlingEnabled).Returns(true);
            view.Setup(v => v.HtmlLink()).Returns("Next");
            view.Setup(v => v.CrawlingDepth()).Returns(1);
            view.Setup(v => v.GetBodyTreeNodes()).Returns(domNodes);
            screen.Setup(s => s.Execute(node, domNodes)).Returns(extraction.Object);
            screen.Setup(s => s.GetLinkToFollow("Next")).Returns(linkElement);
            extraction.Setup(e => e.RecordsCount).Returns(4);
            extraction.Setup(e => e.VariablesCount).Returns(3);
            extraction.Setup(e => e.OutputVariableLabels).Returns(new List<string>());
            extraction.Setup(e => e.ExtractedRecords).Returns(results);

            // Act
            presenter.ExecuteRule();

            // Assert
            view.Verify(v => v.WritePageResults("Extraction Completed: 4 results!"));
            view.Verify(v => v.GetBodyTreeNodes());
            screen.Verify(s => s.Execute(node, domNodes));
            extraction.Verify(e => e.RecordsCount);
            view.Verify(v => v.WritePageResults("4 results!"));
        }

        [TestMethod]
        public void TestRebuildDOM()
        {
            // Arrange
            var url = "http://www.google.gr";
            var node = new TreeNode("HTML");
            var document = TestUtils.CreateHtmlDocument();
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
            var node = new TreeNode("DIV");

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
            var node = new TreeNode("DIV");

            // Act
            presenter.DeleteSnapshot(node);

            // Assert
            view.Verify(v => v.DeleteSnapshotInstance(node));
        }

        [TestMethod]
        public void TestCreateSnapshots()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            presenter.CreateSnapshot(node);

            // Assert
            view.Verify(v => v.FillSnapshotTree(It.Is<TreeNode>(n => n.Text.StartsWith("SNAP"))));
        }

        [TestMethod]
        public void TestSimplifyDomTree()
        {
            // Arrange
            var url = "htt://www.google.gr";
            var document = TestUtils.CreateHtmlDocument();
            var node = new TreeNode("HTML");
            var tags = new string[] { "em" };
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
        public void TestAuxiliaryPatternNodeClick()
        {
            // Arrange
            var element = TestUtils.CreateHtmlElement();
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
        public void TestAuxiliaryPatternTextNodeClick()
        {
            // Arrange
            var node = new TreeNode("TEXT");
            var nInfo = new NodeInfo();
            nInfo.IsTextNode = true;
            node.Tag = nInfo;

            // Act
            presenter.AuxiliaryPatternNodeClick(node);

            // Assert
            view.Verify(v => v.FillTextNodeElementInfo(node));
        }

        [TestMethod]
        public void TestCreateAuxiliaryPatternFromDocument()
        {
            // Arrange
            var element = TestUtils.CreateHtmlElement();
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
            var element = TestUtils.CreateHtmlElement();
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            node.Tag = nInfo;
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(node);

            // Act
            presenter.CreateWorkingPatternFromDocument(element);

            // Assert
            view.Verify(v => v.ClearPatternTree());
            Assert.IsTrue(node.IsRoot());
            view.Verify(v => v.SetNodeFont(It.Is<TreeNode>(n => n.Text == "DIV")));
            view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == "DIV")));
            view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestWorkingPatternRightNodeClick()
        {
            // Arrange
            var node = new TreeNode("DIV");
            var nInfo = new NodeInfo();
            var attributes = new TagAttributeCollection();
            attributes.Add(new TagAttribute { Name = "id", Value = "main-wrapper" });
            nInfo.Attributes = attributes;
            node.Tag = nInfo;
            var domNode = new TreeNode("DIV");
            var element = TestUtils.CreateHtmlElement();
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            view.Setup(v => v.HighlightModeEnabled).Returns(true);
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(domNode);

            // Act
            presenter.WorkingPatternNodeClick(node, MouseButtons.Right);

            // Assert
            view.Verify(v => v.ClearAttributes());
            view.Verify(v => v.LoadNodeAttributes(attributes.All));
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
            var nInfo = new NodeInfo();
            var attributes = new TagAttributeCollection();
            attributes.Add(new TagAttribute { Name = "id", Value = "main-wrapper" });
            nInfo.Attributes = attributes;
            node.Tag = nInfo;
            var domNode = new TreeNode("DIV");
            var element = TestUtils.CreateHtmlElement();
            screen.Setup(s => s.GetElementFromNode(node)).Returns(element);
            view.Setup(v => v.HighlightModeEnabled).Returns(true);
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(domNode);

            // Act
            presenter.WorkingPatternNodeClick(node, MouseButtons.Left);

            // Assert
            view.Verify(v => v.ClearAttributes());
            view.Verify(v => v.LoadNodeAttributes(attributes.All));
            screen.Verify(s => s.HighlightElement(element));
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
            view.Verify(v => v.SelectDOMNode(domNode));
        }

        [TestMethod]
        public void TestWorkingPatternTextNodeClick()
        {
            // Arrange
            var node = new TreeNode("TEXT");
            var nInfo = new NodeInfo();
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
            var element = TestUtils.CreateHtmlElement();
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
            var element = TestUtils.CreateHtmlElement();
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
            var element = TestUtils.CreateHtmlElement();
            var node = new TreeNode("TEXT");
            var nInfo = new NodeInfo();
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
            var element = TestUtils.CreateHtmlElement();
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
            var element = TestUtils.CreateHtmlElement();
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
        public void TestDoesNotHighlightOnMouseOverIfHighlightIsDisabled()
        {
            // Arrange
            var element = TestUtils.CreateHtmlElement();
            var node = new TreeNode("DIV");
            view.Setup(v => v.HighlightModeEnabled).Returns(false);
            screen.Setup(s => s.GetNodeFromElement(element)).Returns(node);

            // Act
            presenter.DocumentMouseOver(element);

            // Assert
            screen.Verify(s => s.HighlightElement(element), Times.Never);
            view.Verify(v => v.SelectDOMNode(node), Times.Never);
            view.Verify(v => v.FillElementInfo(node, element.OuterHtml), Times.Never);
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
        public void TestBrowseToUrl()
        {
            // Arrange
            view.Setup(v => v.Url).Returns("http://www.google.gr/");
            view.Setup(v => v.CrawlingEnabled).Returns(false);

            // Act
            presenter.BrowseToUrl();

            // Assert
            view.Verify(v => v.ClearAuxiliaryTree());
            view.Verify(v => v.ClearSnapshotTree());
            view.Verify(v => v.ClearPatternTree());
            view.Verify(v => v.NavigateTo("http://www.google.gr/"));
        }

        [TestMethod]
        public void TestBrowseToUrlRequiresNonEmptyUrl()
        {
            // Arrange
            view.Setup(v => v.Url).Returns(string.Empty);

            // Act
            presenter.BrowseToUrl();

            // Assert
            view.Verify(v => v.ShowSpecifyURLMessage());
        }

        [TestMethod]
        public void TestBrowseToUrlPreservesPatternWhenMultiPageCrawlingIsEnabled()
        {
            // Arrange
            view.Setup(v => v.Url).Returns("http://www.google.gr/");
            view.Setup(v => v.CrawlingEnabled).Returns(true);

            // Act
            presenter.BrowseToUrl();

            // Assert
            view.Verify(v => v.ClearAuxiliaryTree());
            view.Verify(v => v.ClearSnapshotTree());
            view.Verify(v => v.ClearPatternTree(), Times.Never);
            view.Verify(v => v.NavigateTo("http://www.google.gr/"));
        }

        [TestMethod]
        public void TestBrowserCompleted()
        {
            // Arrange
            string url = "http://www.google.gr";
            var node = new TreeNode("HTML");
            var document = TestUtils.CreateHtmlDocument();
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

        [TestMethod]
        public void TestRunInAutoMode()
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
            var url = "http://www.google.gr/";
            view.Setup(v => v.FirstTargetURL).Returns(url);
            view.Setup(v => v.ExtractionPattern).Returns(node);
            view.Setup(v => v.GetBodyTreeNodes()).Returns(domNodes);
            screen.Setup(s => s.Execute(node, domNodes)).Returns(extraction.Object);
            extraction.Setup(e => e.RecordsCount).Returns(4);
            extraction.Setup(e => e.VariablesCount).Returns(3);
            extraction.Setup(e => e.OutputVariableLabels).Returns(new List<string>());
            extraction.Setup(e => e.ExtractedRecords).Returns(results);

            // Act
            presenter.RunInAutoMode();

            // Assert
            view.Verify(v => v.NavigateTo(url));
            view.Verify(v => v.FocusOutputTabPage());
            var message = "Extraction Completed: 4 results!";
            view.Verify(v => v.WritePageResults(message));
        }

        [TestMethod]
        public void TestRunInAutoModeWithSubmitForm()
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
            var url = "http://www.google.gr/";
            view.Setup(v => v.FirstTargetURL).Returns(url);
            view.Setup(v => v.ExtractionPattern).Returns(node);
            view.Setup(v => v.GetBodyTreeNodes()).Returns(domNodes);
            view.Setup(v => v.AutoFill).Returns(true);
            view.Setup(v => v.FormName).Returns("search-form");
            view.Setup(v => v.FormInputName).Returns("query");
            view.Setup(v => v.FormTerm).Returns("JS");
            screen.Setup(s => s.Execute(node, domNodes)).Returns(extraction.Object);
            extraction.Setup(e => e.RecordsCount).Returns(4);
            extraction.Setup(e => e.VariablesCount).Returns(3);
            extraction.Setup(e => e.OutputVariableLabels).Returns(new List<string>());
            extraction.Setup(e => e.ExtractedRecords).Returns(results);

            // Act
            presenter.RunInAutoMode();
            presenter.BrowserCompleted();

            // Assert
            view.Verify(v => v.NavigateTo(url));
            view.Verify(v => v.FocusOutputTabPage());
            screen.Verify(s => s.SubmitForm("search-form", "query", "JS"));
            var message = "Extraction Completed: 4 results!";
            view.Verify(v => v.WritePageResults(message));
        }

        /// <summary>
        /// NOT DONE
        /// </summary>

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
    }
}

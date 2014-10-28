using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Mock<IDeixtoAgentView> _view;
        private Mock<ISaveFileDialog> _saveFileDialog;
        private Mock<IViewLoader> _loader;
        private Mock<IEventHub> _eventHub;
        private Mock<IDeixtoAgentScreen> _screen;
        private DeixtoAgentPresenter _presenter;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IDeixtoAgentView>();
            _saveFileDialog = new Mock<ISaveFileDialog>();
            _loader = new Mock<IViewLoader>();
            _eventHub = new Mock<IEventHub>();
            _screen = new Mock<IDeixtoAgentScreen>();

            _presenter = new DeixtoAgentPresenter(_view.Object, _saveFileDialog.Object, _loader.Object, _eventHub.Object, _screen.Object);
        }
        
        [TestMethod]
        public void TestForTextOutputFormatDialogAddsTextFilterAndExtesion()
        {
            // Arrange
            _view.Setup(v => v.OutputFileFormat).Returns(Format.Text);

            // Act
            _presenter.SelectOutputFile();

            // Assert
            _saveFileDialog.VerifySet(s => s.Filter = "Text Files (*.txt)|");
            _saveFileDialog.VerifySet(s => s.Extension = "txt");
        }

        [TestMethod]
        public void TestForXmlOutputFormatDialogAddsXmlFilterAndExtension()
        {
            // Arrange
            _view.Setup(v => v.OutputFileFormat).Returns(Format.XML);

            // Act
            _presenter.SelectOutputFile();

            // Assert
            _saveFileDialog.VerifySet(s => s.Filter = "XML Files (*.xml)|");
            _saveFileDialog.VerifySet(s => s.Extension = "xml");
        }

        [TestMethod]
        public void TestForRssOutputFormatDialogAddsRssFilterAndExtesion()
        {
            // Arrange
            _view.Setup(v => v.OutputFileFormat).Returns(Format.RSS);

            // Act
            _presenter.SelectOutputFile();
            
            // Assert
            _saveFileDialog.VerifySet(s => s.Filter = "RSS Files (*.rss)|");
            _saveFileDialog.VerifySet(s => s.Extension = "rss");
        }

        [TestMethod]
        public void TestOutputFileSelectionAbortsForNegativeAnswer()
        {
            // Arrange
            _view.Setup(v => v.OutputFileFormat).Returns(Format.XML);
            _saveFileDialog.Setup(s => s.ShowDialog()).Returns(DialogResult.Abort);

            // Act
            _presenter.SelectOutputFile();
            
            // Assert
            _view.VerifyGet(v => v.OutputFileName, Times.Never);
        }

        [TestMethod]
        public void TestOutputFileNameIsSetAfterSelectingFile()
        {
            // Arrange
            _view.Setup(m => m.OutputFileFormat).Returns(Format.Text);
            _saveFileDialog.Setup(m => m.ShowDialog()).Returns(DialogResult.OK);
            _saveFileDialog.Setup(m => m.Filename).Returns("output_file");

            // Act
            _presenter.SelectOutputFile();

            // Assert
            _saveFileDialog.VerifySet(m => m.Filter = "Text Files (*.txt)|");
            _saveFileDialog.VerifySet(m => m.Extension = "txt");
            _view.VerifySet(m => m.OutputFileName = "output_file");
        }

        [TestMethod]
        public void TestAddSiblingOrder()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            _presenter.AddSiblingOrder(node);

            // Assert
            _loader.Verify(l => l.LoadAddSiblingOrderView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestDeleteNodeFromWorkingPattern()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            _presenter.DeleteNode(node);

            // Assert
            _view.Verify(v => v.DeletePatternNode(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestUrlInputIsFilledUponTargetUrlSelection()
        {
            // Arrange
            string url = "http://www.google.gr";

            // Act
            _presenter.TargetURLSelected(url);

            // Assert
            _view.Verify(v => v.SetURLInput(It.Is<string>(s => s == url)));
        }

        [TestMethod]
        public void TestRemoveUrlFromTargetUrls()
        {
            // Arrange
            string url = "http://www.google.gr";
            _view.Setup(v => v.TargetURLToAdd()).Returns(url);
            _view.Setup(v => v.AskUserToRemoveURL()).Returns(true);

            // Act
            _presenter.RemoveURLFromTargetURLs();

            // Assert
            _view.Verify(v => v.RemoveTargetURL(It.Is<string>(s => s == url)));
            _view.Verify(v => v.ClearAddURLInput());
        }

        [TestMethod]
        public void TestRemovingEmptyUrlFromTargetUrlsShowsWarningMessage()
        {
            // Arrange
            string url = "";
            _view.Setup(v => v.TargetURLToAdd()).Returns(url);

            // Act
            _presenter.RemoveURLFromTargetURLs();

            // Assert
            _view.Verify(v => v.ShowSelectURLMessage());
        }

        [TestMethod]
        public void TestUrlDeletionIsAbortedIfUserDoesNotConfirm()
        {
            // Arrange
            string url = "http://www.google.gr";
            _view.Setup(v => v.TargetURLToAdd()).Returns(url);
            _view.Setup(v => v.AskUserToRemoveURL()).Returns(false);

            // Act
            _presenter.RemoveURLFromTargetURLs();

            // Assert
            _view.Verify(v => v.RemoveTargetURL(url), Times.Never);
        }

        [TestMethod]
        public void TestAddUrlToTargetUrls()
        {
            // Arrange
            string url = "http://www.google.gr";
            _view.Setup(v => v.TargetURLToAdd()).Returns(url);

            // Act
            _presenter.AddURLToTargetURLs();

            // Assert
            _view.Verify(v => v.AppendTargetUrl(It.Is<string>(s => s == url)));
            _view.Verify(v => v.ClearAddURLInput());
        }

        [TestMethod]
        public void TestEmptyUrlIsNotAddedToTargetUrls()
        {
            // Arrange
            string url = "";
            _view.Setup(v => v.TargetURLToAdd()).Returns(url);

            // Act
            _presenter.AddURLToTargetURLs();

            // Assert
            _view.Verify(v => v.ShowEnterURLToAddMessage());
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
            _presenter.RemoveRegex(node);

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
            _presenter.RemoveLabel(node);

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
            _presenter.AddRegex(node);

            // Assert
            _loader.Verify(l => l.LoadRegexBuilderView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestAddLabelToNode()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            _presenter.AddNewLabel(node);

            // Assert
            _loader.Verify(l => l.LoadAddLabelView(It.Is<TreeNode>(n => n == node)));
        }

        [TestMethod]
        public void TestWindowClosingPublishesEvent()
        {
            // Act
            _presenter.windowClosing();
            
            // Assert
            _eventHub.Verify(e => e.Publish(It.IsAny<EventArgs>()));
        }

        [TestMethod]
        public void TestNodeStateChanges()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;

            // Act
            _presenter.NodeStateChanged(node, NodeState.Grayed);

            // Assert
            _view.Verify(v => v.ApplyStateToNode(node, 3));
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
            _presenter.NodeStateChanged(node, NodeState.Unchecked);

            // Assert
            _view.Verify(v => v.ApplyStateToNode(node, 5));
            _view.Verify(v => v.ApplyStateToNode(n1, 5));
            _view.Verify(v => v.ApplyStateToNode(n2, 5));
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
            _presenter.LevelDownWorkingPattern(node);

            // Assert
            _view.Verify(v => v.ClearPatternTree());
            _view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == "H1")));
            _view.Verify(v => v.ExpandPatternTree());
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
            _presenter.LevelDownWorkingPattern(node);

            // Assert
            _view.Verify(v => v.ShowCannotDeleteRootMessage());
        }

        [TestMethod]
        public void TestClearTreeViews()
        {
            // Arrange
            _view.Setup(v => v.AskUserToClearTreeViews()).Returns(true);

            // Act
            _presenter.ClearTreeViews(2);

            // Assert
            _view.Verify(v => v.ClearAuxiliaryTree());
            _view.Verify(v => v.ClearPatternTree());
            _view.Verify(v => v.ClearSnapshotTree());
        }

        [TestMethod]
        public void TestClearTreeViewsAbortsIfUserDoesNotConfirm()
        {
            // Arrange
            _view.Setup(v => v.AskUserToClearTreeViews()).Returns(false);

            // Act
            _presenter.ClearTreeViews(2);

            // Assert
            _view.Verify(v => v.ClearAuxiliaryTree(), Times.Never);
            _view.Verify(v => v.ClearPatternTree(), Times.Never);
            _view.Verify(v => v.ClearSnapshotTree(), Times.Never);
        }

        [TestMethod]
        public void TestClearTreeViewsDoesNotProceedIfCountIsZero()
        {
            // Act
            _presenter.ClearTreeViews(0);

            // Assert
            _view.Verify(v => v.ClearAuxiliaryTree(), Times.Never);
            _view.Verify(v => v.ClearPatternTree(), Times.Never);
            _view.Verify(v => v.ClearSnapshotTree(), Times.Never);
        }

        [TestMethod]
        public void TestMakeWorkingPatternFromSnapshot()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            _presenter.MakeWorkingPatternFromSnapshot(node);

            // Assert
            _view.Verify(v => v.ClearPatternTree());
            _view.Verify(v => v.FillPatternTree(It.Is<TreeNode>(n => n.Text == node.Text)));
            _view.Verify(v => v.ExpandPatternTree());
        }

        [TestMethod]
        public void TestDeleteNodeFromSnapshots()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            _presenter.DeleteSnapshot(node);

            // Assert
            _view.Verify(v => v.DeleteSnapshotInstance(node));
        }

        [TestMethod]
        public void TestCreateSnapshots()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");

            // Act
            _presenter.CreateSnapshot(node);

            // Assert
            _view.Verify(v => v.FillSnapshotTree(It.Is<TreeNode>(n => n.Text.StartsWith("SNAP"))));
        }

        [TestMethod]
        public void TestCrawlingChanged()
        {
            // Act
            _presenter.CrawlingChanged(true);

            // Assert
            _view.Verify(v => v.ApplyVisibilityStateInCrawling(true));

            // Act
            _presenter.CrawlingChanged(false);

            // Assert
            _view.Verify(v => v.ApplyVisibilityStateInCrawling(false));
        }

        [TestMethod]
        public void TestAutoFillChanged()
        {
            // Act
            _presenter.AutoFillChanged(true);

            // Assert
            _view.Verify(v => v.ApplyVisibilityStateInAutoFill(true));

            // Act
            _presenter.AutoFillChanged(false);

            // Assert
            _view.Verify(v => v.ApplyVisibilityStateInAutoFill(false));
        }

        [TestMethod]
        public void TestBrowsesToUrlWhenEnterPressed()
        {
            // Arrange
            var args = new KeyEventArgs(Keys.Enter);
            var url = "http://www.google.gr/";
            _view.Setup(v => v.Url).Returns(url);

            // Act
            _presenter.KeyDownPress(args);

            // Assert
            _view.Verify(v => v.NavigateTo("http://www.google.gr/"));
        }

        [TestMethod]
        public void TestNavigatesBackWhenAltAndLeftArePressed()
        {
            // Arrange
            var args = new KeyEventArgs(Keys.Alt | Keys.Left);

            // Act
            _presenter.KeyDownPress(args);

            // Assert
            _view.Verify(v => v.NavigateBack());
        }

        [TestMethod]
        public void TestNavigatesForwardWhenAltAndRightArePressed()
        {
            // Arrange
            var args = new KeyEventArgs(Keys.Alt | Keys.Right);

            // Act
            _presenter.KeyDownPress(args);

            // Assert
            _view.Verify(v => v.NavigateForward());
        }

        [TestMethod]
        public void TestReceiveRegexAdded()
        {
            // Arrange
            TreeNode node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            nInfo.ElementSourceIndex = 12;
            node.Tag = nInfo;
            RegexAdded ra = new RegexAdded(node);
            var element = CreateHtmlElement();
            _screen.Setup(s => s.GetElementFromNode(node)).Returns(element);

            // Act
            _presenter.Receive(ra);
            
            // Assert
            _view.Verify(v => v.FillElementInfo(node, element.OuterHtml));
        }

        [TestMethod]
        public void TestLoadUrlsFromFile()
        {
            // Arrange
            string filter = "Text Files (*.txt)|";
            string filename = "output_file";
            string[] urls = new string[] { "http://www.google.gr", "http://www.cs.teilar.gr" };
            var dialog = new Mock<IOpenFileDialog>();
            _screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            _screen.Setup(s => s.LoadUrlsFromFile(filename)).Returns(urls);

            // Act
            _presenter.LoadURLsFromFile();

            // Assert
            _view.Verify(v => v.AppendTargetUrls(urls));
            _view.VerifySet(v => v.TargetURLsFile = filename);
        }

        [TestMethod]
        public void TestLoadingOfUrlsIsAbortedIfUserDoesNotSelectFile()
        {
            // Arrange
            string filter = "Text Files (*.txt)|";
            var dialog = new Mock<IOpenFileDialog>();
            _screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.Abort);

            // Act
            _presenter.LoadURLsFromFile();

            // Assert
            _screen.Verify(s => s.LoadUrlsFromFile("output_file"), Times.Never);
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
            _screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);

            // Act
            _presenter.SaveToDisk();

            // Assert
            _screen.Verify(s => s.WriteExtractedRecords(filename));
        }

        [TestMethod]
        public void TestSaveToDiskAbortsIfUserDoesNotSelectOutputFile()
        {
            // Arrange
            string filter = "Text Files (*.txt)|";
            string extension = "txt";
            var dialog = new Mock<ISaveFileDialog>();
            _screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.Abort);

            // Act
            _presenter.SaveToDisk();

            // Assert
            _screen.Verify(v => v.WriteExtractedRecords(It.IsAny<string>()), Times.Never);
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

            _screen.Setup(s => s.GetDomNode(n1)).Returns(d1);

            // Act
            _presenter.AddNextSibling(n1);

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

            _screen.Setup(s => s.GetDomNode(n1)).Returns(d2);

            // Act
            _presenter.AddPreviousSibling(n1);

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
            _screen.Setup(s => s.GetOpenFileDialog(filter)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            _screen.Setup(s => s.LoadExtractionPattern(filename)).Returns(node);

            // Act
            _presenter.LoadExtractionPattern();
            
            // Assert
            _view.Verify(v => v.FillExtractionPattern(node));
            _view.Verify(v => v.ExpandExtractionTree());
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
            _screen.Setup(s => s.GetSaveFileDialog(filter, extension)).Returns(dialog.Object);
            dialog.Setup(d => d.ShowDialog()).Returns(DialogResult.OK);
            dialog.Setup(d => d.Filename).Returns(filename);
            _view.Setup(v => v.GetPatternTreeNodes()).Returns(dom.Nodes);

            // Act
            _presenter.SaveExtractionPattern();

            // Assert
            _screen.Verify(s => s.SaveExtractionPattern(filename, dom.Nodes));
        }

        // HELPER METHODS
        private HtmlElement CreateHtmlElement()
        {
            WebBrowser browser = new WebBrowser();
            browser.DocumentText = "<html></html>";
            browser.Show();
            var doc = browser.Document;
            doc.Write("<html></html>");
            var element = doc.GetElementsByTagName("html");
            var elem = element[0];

            return elem;
        }
    }
}

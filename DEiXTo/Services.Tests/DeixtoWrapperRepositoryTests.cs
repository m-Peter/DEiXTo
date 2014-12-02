using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class DeixtoWrapperRepositoryTests
    {
        private string _filename = "wrapper.xml";
        private MemoryStream _stream;
        private DeixtoWrapper _wrapper;
        private DeixtoWrapperFileRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            _stream = new MemoryStream();
            _wrapper = new DeixtoWrapper();
            _repository = new DeixtoWrapperFileRepository(_filename);
        }

        [TestCleanup]
        public void TearDown()
        {
            _stream.Close();
        }

        [TestMethod]
        public void TestSaveAndLoadSubmitFormFields()
        {
            // Arrange
            _wrapper.AutoSubmitForm = true;
            _wrapper.FormName = "search-repo";
            _wrapper.InputName = "query";
            _wrapper.SearchQuery = "rails";

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.IsTrue(loaded.AutoSubmitForm);
            Assert.AreEqual("search-repo", loaded.FormName);
            Assert.AreEqual("query", loaded.InputName);
            Assert.AreEqual("rails", loaded.SearchQuery);
        }

        [TestMethod]
        public void TestSaveAndLoadExtractUrl()
        {
            // Arrange
            _wrapper.ExtractNativeUrl = true;
            
            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);
            
            // Assert
            Assert.IsTrue(loaded.ExtractNativeUrl);
        }

        [TestMethod]
        public void TestSaveAndLoadMaxHits()
        {
            // Arrange
            _wrapper.NumberOfHits = 3;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual(3, loaded.NumberOfHits);
        }

        [TestMethod]
        public void TestSaveAndLoadMultiPageFields()
        {
            // Arrange
            _wrapper.MultiPageCrawling = true;
            _wrapper.HtmlNextLink = "Next";
            _wrapper.MaxCrawlingDepth = 5;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.IsTrue(loaded.MultiPageCrawling);
            Assert.AreEqual("Next", loaded.HtmlNextLink);
            Assert.AreEqual(5, loaded.MaxCrawlingDepth);
        }

        [TestMethod]
        public void TestSaveAndLoadInputFile()
        {
            // Arrange
            _wrapper.UrlsInputFile = "input_file.txt";

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual("input_file.txt", loaded.UrlsInputFile);
        }

        [TestMethod]
        public void TestSaveAndLoadXmlOutputFile()
        {
            // Arrange
            _wrapper.OutputFileName = "output_file.xml";
            _wrapper.OutputFileFormat = Format.XML;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual("output_file.xml", loaded.OutputFileName);
            Assert.AreEqual(Format.XML, loaded.OutputFileFormat);
        }

        [TestMethod]
        public void TestSaveAndLoadTextOutputFile()
        {
            // Arrange
            _wrapper.OutputFileName = "output_file.txt";
            _wrapper.OutputFileFormat = Format.Text;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual("output_file.txt", loaded.OutputFileName);
            Assert.AreEqual(Format.Text, loaded.OutputFileFormat);
        }

        [TestMethod]
        public void TestSaveAndLoadRssOutputFile()
        {
            // Arrange
            _wrapper.OutputFileName = "output_file.rss";
            _wrapper.OutputFileFormat = Format.RSS;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual("output_file.rss", loaded.OutputFileName);
            Assert.AreEqual(Format.RSS, loaded.OutputFileFormat);
        }

        [TestMethod]
        public void TestSaveAndLoadAppendMode()
        {
            // Arrange
            _wrapper.OutputFileMode = OutputMode.Append;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual(OutputMode.Append, loaded.OutputFileMode);
        }

        [TestMethod]
        public void TestSaveAndLoadOverwriteMode()
        {
            // Arrange
            _wrapper.OutputFileMode = OutputMode.Overwrite;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual(OutputMode.Overwrite, loaded.OutputFileMode);
        }

        [TestMethod]
        public void TestSaveAndLoadTargetUrls()
        {
            // Arrange
            string[] urls = new string[3];
            urls[0] = "http://www.cs.teilar.gr";
            urls[1] = "http://www.teilar.gr";
            urls[2] = "http://www.petrmarkou.com";
            _wrapper.TargetUrls = urls;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            var loadedUrls = loaded.TargetUrls;
            Assert.AreEqual(3, loadedUrls.Length);
            Assert.AreEqual("http://www.cs.teilar.gr", loadedUrls[0]);
            Assert.AreEqual("http://www.teilar.gr", loadedUrls[1]);
            Assert.AreEqual("http://www.petrmarkou.com", loadedUrls[2]);
        }

        [TestMethod]
        public void TestSaveAndLoadIgnoredTags()
        {
            // Arrange
            string[] tags = new string[3];
            tags[0] = "<B>";
            tags[1] = "<SPAN>";
            tags[2] = "<EM>";
            _wrapper.IgnoredHtmlTags = tags;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            var loadedTags = loaded.IgnoredHtmlTags;
            Assert.AreEqual(3, loadedTags.Length);
            Assert.AreEqual("<B>", loadedTags[0]);
            Assert.AreEqual("<SPAN>", loadedTags[1]);
            Assert.AreEqual("<EM>", loadedTags[2]);
        }

        [TestMethod]
        public void TestSaveAndLoadExtractionPattern()
        {
            // Arrange
            var div = CreateRootNode("DIV");
            var h2 = CreateNode("H2", NodeState.Checked);
            var p = CreateNode("P", NodeState.CheckedImplied);
            p.SetRegex("[a|b]");
            var p1 = CreateNode("P", NodeState.CheckedSource);
            p1.SetCareAboutSiblingOrder(true);
            p1.SetStartIndex(2);
            p1.SetStepValue(4);
            AddNodesTo(div, h2, p, p1);
            var pattern = new ExtractionPattern(div);
            _wrapper.ExtractionPattern = pattern;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            var loadedPattern = loaded.ExtractionPattern.RootNode;
            Assert.AreEqual("DIV", loadedPattern.Text);
            Assert.IsTrue(loadedPattern.IsRoot());
            Assert.AreEqual(NodeState.Grayed, loadedPattern.GetState());

            Assert.AreEqual(3, loadedPattern.Nodes.Count);

            var loadedH2 = loadedPattern.Nodes[0];
            Assert.AreEqual("H2", loadedH2.Text);
            Assert.AreEqual(NodeState.Checked, loadedH2.GetState());

            var loadedP = loadedPattern.Nodes[1];
            Assert.AreEqual("P", loadedP.Text);
            Assert.AreEqual("[a|b]", loadedP.GetRegex());
            Assert.AreEqual(NodeState.CheckedImplied, loadedP.GetState());

            var loadedP1 = loadedPattern.Nodes[2];
            Assert.AreEqual("P", loadedP1.Text);
            Assert.IsTrue(loadedP1.GetCareAboutSiblingOrder());
            Assert.AreEqual(2, loadedP1.GetStartIndex());
            Assert.AreEqual(4, loadedP1.GetStepValue());
            Assert.AreEqual(NodeState.CheckedSource, loadedP1.GetState());
        }

        [TestMethod]
        public void TestSaveAndLoadTargetUrlsAndExtractionPattern()
        {
            // Arrange
            var div = CreateRootNode("DIV");
            var h2 = CreateNode("H2", NodeState.Grayed);
            var p = CreateNode("P", NodeState.Grayed);
            var p1 = CreateNode("P", NodeState.Grayed);
            AddNodesTo(div, h2, p, p1);
            var pattern = new ExtractionPattern(div);
            var targetUrls = new string[] { "http://www.cs.teilar.gr" };
            _wrapper.ExtractionPattern = pattern;
            _wrapper.TargetUrls = targetUrls;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            var loadedUrls = loaded.TargetUrls;
            var loadedPattern = loaded.ExtractionPattern.RootNode;

            Assert.AreEqual(1, loadedUrls.Length);
            Assert.AreEqual("http://www.cs.teilar.gr", loadedUrls[0]);

            Assert.AreEqual("DIV", loadedPattern.Text);
            Assert.AreEqual(3, loadedPattern.Nodes.Count);

            var loadedH2 = loadedPattern.Nodes[0];
            Assert.AreEqual("H2", loadedH2.Text);
            Assert.AreEqual(NodeState.Grayed, loadedH2.GetState());

            var loadedP = loadedPattern.Nodes[1];
            Assert.AreEqual("P", loadedP.Text);
            Assert.AreEqual(NodeState.Grayed, loadedP.GetState());

            var loadedP1 = loadedPattern.Nodes[2];
            Assert.AreEqual("P", loadedP1.Text);
            Assert.AreEqual(NodeState.Grayed, loadedP1.GetState());
        }

        [TestMethod]
        public void TestSaveAndLoadInputFileAndExtractionPattern()
        {
            // Arrange
            var div = CreateRootNode("DIV");
            var h2 = CreateNode("H2", NodeState.Grayed);
            var p = CreateNode("P", NodeState.Grayed);
            var p1 = CreateNode("P", NodeState.Grayed);
            AddNodesTo(div, h2, p, p1);
            var pattern = new ExtractionPattern(div);
            var inputFile = "input_file.txt";
            _wrapper.UrlsInputFile = inputFile;
            _wrapper.ExtractionPattern = pattern;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual("input_file.txt", loaded.UrlsInputFile);
            var loadedPattern = loaded.ExtractionPattern.RootNode;

            Assert.AreEqual("DIV", loadedPattern.Text);
            Assert.AreEqual(3, loadedPattern.Nodes.Count);

            var loadedH2 = loadedPattern.Nodes[0];
            Assert.AreEqual("H2", loadedH2.Text);
            Assert.AreEqual(NodeState.Grayed, loadedH2.GetState());

            var loadedP = loadedPattern.Nodes[1];
            Assert.AreEqual("P", loadedP.Text);
            Assert.AreEqual(NodeState.Grayed, loadedP.GetState());

            var loadedP1 = loadedPattern.Nodes[2];
            Assert.AreEqual("P", loadedP1.Text);
            Assert.AreEqual(NodeState.Grayed, loadedP1.GetState());
        }

        private void AddNodesTo(TreeNode node, params TreeNode[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                node.Nodes.Add(nodes[i]);
            }
        }

        private TreeNode CreateRootNode(string text)
        {
            var root = new TreeNode(text);
            root.Tag = new NodeInfo.Builder().SetRoot(true).SetState(NodeState.Grayed).Build();

            return root;
        }

        private TreeNode CreateNode(string text, NodeState state, string label = null)
        {
            var node = new TreeNode(text);
            node.Tag = new NodeInfo.Builder().SetLabel(label).SetState(state).Build();

            return node;
        }
    }
}

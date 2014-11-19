using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Moq;
using System.IO;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class ExtractionPatternRepositoryTests
    {
        private string _filename = "pattern.xml";
        private Mock<IFileLoader> _loader;
        private MemoryStream _stream;

        [TestInitialize]
        public void SetUp()
        {
            _loader = new Mock<IFileLoader>();
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void TestSaveAndLoadCheckedNode()
        {
            // Arrange
            var div = CreateNode("DIV", NodeState.Checked);
            var pattern = new ExtractionPattern(div);
            _loader.Setup(l => l.Load(_filename, It.IsAny<FileMode>())).Returns(_stream);
            var repository = new ExtractionPatternFileRepository(_filename, _loader.Object);

            // Act
            repository.Save(pattern);
            _stream.Position = 0;
            var loaded = repository.Load();

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("DIV", root.Text);
            Assert.AreEqual(NodeState.Checked, root.GetState());
        }

        [TestMethod]
        public void TestSaveAndLoadCheckedImpliedNode()
        {
            // Arrange
            var a = CreateNode("A", NodeState.CheckedImplied);
            var pattern = new ExtractionPattern(a);
            _loader.Setup(l => l.Load(_filename, It.IsAny<FileMode>())).Returns(_stream);
            var repository = new ExtractionPatternFileRepository(_filename, _loader.Object);

            // Act
            repository.Save(pattern);
            _stream.Position = 0;
            var loaded = repository.Load();

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("A", root.Text);
            Assert.AreEqual(NodeState.CheckedImplied, root.GetState());
        }

        [TestMethod]
        public void TestSaveAndLoadCheckedSourceNode()
        {
            // Arrange
            var img = CreateNode("IMG", NodeState.CheckedSource);
            var pattern = new ExtractionPattern(img);
            _loader.Setup(l => l.Load(_filename, It.IsAny<FileMode>())).Returns(_stream);
            var repository = new ExtractionPatternFileRepository(_filename, _loader.Object);

            // Act
            repository.Save(pattern);
            _stream.Position = 0;
            var loaded = repository.Load();

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("IMG", root.Text);
            Assert.AreEqual(NodeState.CheckedSource, root.GetState());
        }

        [TestMethod]
        public void TestSaveAndLoadGrayedNode()
        {
            // Arrange
            var p = CreateNode("P", NodeState.Grayed);
            var pattern = new ExtractionPattern(p);
            _loader.Setup(l => l.Load(_filename, It.IsAny<FileMode>())).Returns(_stream);
            var repository = new ExtractionPatternFileRepository(_filename, _loader.Object);

            // Act
            repository.Save(pattern);
            _stream.Position = 0;
            var loaded = repository.Load();

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("P", root.Text);
            Assert.AreEqual(NodeState.Grayed, root.GetState());
        }

        [TestMethod]
        public void TestSaveAndLoadGrayedImpliedNode()
        {
            // Arrange
            var h1 = CreateNode("H1", NodeState.GrayedImplied);
            var pattern = new ExtractionPattern(h1);
            _loader.Setup(l => l.Load(_filename, It.IsAny<FileMode>())).Returns(_stream);
            var repository = new ExtractionPatternFileRepository(_filename, _loader.Object);

            // Act
            repository.Save(pattern);
            _stream.Position = 0;
            var loaded = repository.Load();

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("H1", root.Text);
            Assert.AreEqual(NodeState.GrayedImplied, root.GetState());
        }

        [TestMethod]
        public void TestDoesNotSaveUncheckedNode()
        {
            // Arrange
            var div = CreateNode("DIV", NodeState.Grayed);
            var input = CreateNode("INPUT", NodeState.Unchecked);
            div.AddNode(input);
            var pattern = new ExtractionPattern(div);
            _loader.Setup(l => l.Load(_filename, It.IsAny<FileMode>())).Returns(_stream);
            var repository = new ExtractionPatternFileRepository(_filename, _loader.Object);

            // Act
            repository.Save(pattern);
            _stream.Position = 0;
            var loaded = repository.Load();

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("DIV", root.Text);
            Assert.AreEqual(NodeState.Grayed, root.GetState());
            Assert.AreEqual(0, root.Nodes.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadExtractionPattern()
        {
            // Arrange
            string filename = "pattern.xml";
            var div = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Checked);
            var p = CreateNode("P", NodeState.CheckedImplied);
            p.SetRegex("[a|b]");
            var p1 = CreateNode("P", NodeState.CheckedSource);
            p1.SetCareAboutSiblingOrder(true);
            p1.SetStartIndex(2);
            p1.SetStepValue(4);
            AddNodesTo(div, h2, p, p1);
            var pattern = new ExtractionPattern(div);
            var loader = new Mock<IFileLoader>();
            var ms = new MemoryStream();
            loader.Setup(l => l.Load(filename, It.IsAny<FileMode>())).Returns(ms);
            var repository = new ExtractionPatternFileRepository(filename, loader.Object);

            // Act
            repository.Save(pattern);
            ms.Position = 0;
            var loaded = repository.Load();

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("DIV", root.Text);
            Assert.IsTrue(root.IsRoot());
            Assert.AreEqual(NodeState.Grayed, root.GetState());

            Assert.AreEqual(3, root.Nodes.Count);

            var loadedH2 = root.Nodes[0];
            Assert.AreEqual("H2", loadedH2.Text);
            Assert.AreEqual(NodeState.Checked, loadedH2.GetState());

            var loadedP = root.Nodes[1];
            Assert.AreEqual("P", loadedP.Text);
            Assert.AreEqual("[a|b]", loadedP.GetRegex());
            Assert.AreEqual(NodeState.CheckedImplied, loadedP.GetState());

            var loadedP1 = root.Nodes[2];
            Assert.AreEqual("P", loadedP1.Text);
            Assert.IsTrue(loadedP1.GetCareAboutSiblingOrder());
            Assert.AreEqual(2, loadedP1.GetStartIndex());
            Assert.AreEqual(4, loadedP1.GetStepValue());
            Assert.AreEqual(NodeState.CheckedSource, loadedP1.GetState());
        }


        private void AddNodesTo(TreeNode node, params TreeNode[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                node.Nodes.Add(nodes[i]);
            }
        }

        private TreeNode CreateRootNode()
        {
            var root = new TreeNode("DIV");
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

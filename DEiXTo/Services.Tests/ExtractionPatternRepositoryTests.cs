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
        private MemoryStream _stream;

        [TestInitialize]
        public void SetUp()
        {
            _stream = new MemoryStream();
        }

        [TestCleanup]
        public void TearDown()
        {
            _stream.Close();
        }

        [TestMethod]
        public void TestSaveAndLoadCheckedNode()
        {
            // Arrange
            var div = CreateNode("DIV", NodeState.Checked);
            var pattern = new ExtractionPattern(div);
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

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
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

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
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

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
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

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
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

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
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("DIV", root.Text);
            Assert.AreEqual(NodeState.Grayed, root.GetState());
            Assert.AreEqual(0, root.Nodes.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadRootNode()
        {
            // Arrange
            var div = CreateRootNode("DIV");
            var pattern = new ExtractionPattern(div);
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

            // Assert
            var root = loaded.RootNode;
            Assert.IsTrue(root.IsRoot());
        }

        [TestMethod]
        public void TestSaveAndLoadNodeWithRegex()
        {
            // Arrange
            var p = CreateNode("P", NodeState.Checked);
            p.SetRegex("[a|b]");
            var pattern = new ExtractionPattern(p);
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("[a|b]", root.GetRegex());
        }

        [TestMethod]
        public void TestSaveAndLoadNodeWithCustomLabel()
        {
            // Arrange
            var img = CreateNode("IMG:ProductImage", NodeState.CheckedSource);
            img.SetLabel("ProductImage");
            var pattern = new ExtractionPattern(img);
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

            // Assert
            var root = loaded.RootNode;
            Assert.AreEqual("IMG:ProductImage", root.Text);
            Assert.AreEqual("ProductImage", root.GetLabel());
        }

        [TestMethod]
        public void TestSaveAndLoadNodeWithSiblingOrder()
        {
            // Arrange
            var div = CreateNode("DIV", NodeState.Checked);
            div.SetCareAboutSiblingOrder(true);
            div.SetStartIndex(1);
            div.SetStepValue(3);
            var pattern = new ExtractionPattern(div);
            var repository = new ExtractionPatternFileRepository(_filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

            // Assert
            var root = loaded.RootNode;
            Assert.IsTrue(root.GetCareAboutSiblingOrder());
            Assert.AreEqual(1, root.GetStartIndex());
            Assert.AreEqual(3, root.GetStepValue());
        }

        [TestMethod]
        public void TestSaveAndLoadExtractionPattern()
        {
            // Arrange
            string filename = "pattern.xml";
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
            var repository = new ExtractionPatternFileRepository(filename);

            // Act
            repository.Save(pattern, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

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

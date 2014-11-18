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
        [TestMethod]
        public void TestSaveAndLoadExtractionPattern()
        {
            // Arrange
            string filename = "pattern.xml";
            var div = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Checked);
            var p = CreateNode("P", NodeState.CheckedImplied);
            var p1 = CreateNode("P", NodeState.CheckedSource);
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
            Assert.AreEqual(3, root.Nodes.Count);
            Assert.AreEqual("H2", root.Nodes[0].Text);
            Assert.AreEqual("P", root.Nodes[1].Text);
            Assert.AreEqual("P", root.Nodes[2].Text);
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

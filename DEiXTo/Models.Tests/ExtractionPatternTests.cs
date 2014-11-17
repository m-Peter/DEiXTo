using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using DEiXTo.Services;

namespace DEiXTo.Models.Tests
{
    [TestClass]
    public class ExtractionPatternTests
    {
        [TestMethod]
        public void TestCreateNewExtractionPattern()
        {
            // Arrange
            var node = CreateRootNode();
            
            // Act
            var pattern = new ExtractionPattern(node);

            // Assert
            Assert.IsInstanceOfType(pattern, typeof(ExtractionPattern));
        }

        [TestMethod]
        public void TestRetrieveRootNode()
        {
            // Arrange
            var node = CreateRootNode();

            // Act
            var pattern = new ExtractionPattern(node);

            // Assert
            Assert.AreEqual(node, pattern.RootNode);
        }

        [TestMethod]
        public void TestCountOutputVariables()
        {
            // Arrange
            var node = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Grayed);
            var h2Text = CreateNode("TEXT", NodeState.Checked);
            h2.Nodes.Add(h2Text);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked);
            p.Nodes.Add(pText);
            var p1 = CreateNode("P", NodeState.Grayed);
            var p1Text = CreateNode("TEXT", NodeState.Checked);
            p1.Nodes.Add(p1Text);
            AddNodesTo(node, h2, p, p1);
            var pattern = new ExtractionPattern(node);

            // Act
            int variables = pattern.CountOutputVariables();

            // Assert
            Assert.AreEqual(3, variables);
        }

        [TestMethod]
        public void TestCollectOutputVariableLabes()
        {
            // Arrange
            var node = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Grayed);
            var h2Text = CreateNode("TEXT", NodeState.Checked, "HEADER");
            h2.Nodes.Add(h2Text);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked, "SYNOPSIS");
            p.Nodes.Add(pText);
            var p1 = CreateNode("P", NodeState.Grayed);
            var p1Text = CreateNode("TEXT", NodeState.Checked, "CONTENT");
            p1.Nodes.Add(p1Text);
            AddNodesTo(node, h2, p, p1);
            var pattern = new ExtractionPattern(node);

            // Act
            var labels = pattern.OutputVariableLabels();

            // Assert
            Assert.AreEqual(3, labels.Count);
            Assert.IsTrue(labels.Contains("HEADER"));
            Assert.IsTrue(labels.Contains("SYNOPSIS"));
            Assert.IsTrue(labels.Contains("CONTENT"));
        }

        [TestMethod]
        public void TestCollectVariableLabels()
        {
            // Arrange
            var node = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Grayed);
            var h2Text = CreateNode("TEXT", NodeState.Checked, "HEADER");
            h2.Nodes.Add(h2Text);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked);
            p.Nodes.Add(pText);
            var p1 = CreateNode("P", NodeState.Grayed);
            var p1Text = CreateNode("TEXT", NodeState.Checked, "CONTENT");
            p1.Nodes.Add(p1Text);
            AddNodesTo(node, h2, p, p1);
            var pattern = new ExtractionPattern(node);

            // Act
            var labels = pattern.OutputVariableLabels();

            // Assert
            Assert.AreEqual(3, labels.Count);
            Assert.IsTrue(labels.Contains("HEADER"));
            Assert.IsTrue(labels.Contains("VAR2"));
            Assert.IsTrue(labels.Contains("CONTENT"));
        }

        [TestMethod]
        public void TestTrimUncheckedNodesFromPattern()
        {
            // Arrange
            var node = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Grayed);
            var h2Text = CreateNode("TEXT", NodeState.Checked, "HEADER");
            h2.Nodes.Add(h2Text);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked, "SYNOPSIS");
            p.Nodes.Add(pText);
            var p1 = CreateNode("P", NodeState.Unchecked);
            var p1Text = CreateNode("TEXT", NodeState.Checked, "CONTENT");
            p1.Nodes.Add(p1Text);
            AddNodesTo(node, h2, p, p1);
            var pattern = new ExtractionPattern(node);

            // Act
            pattern.TrimUncheckedNodes();

            // Assert
            Assert.AreEqual(2, node.Nodes.Count);
            Assert.AreEqual(h2, node.Nodes[0]);
            Assert.AreEqual(p, node.Nodes[1]);
        }

        [TestMethod]
        public void TestCreatePatternWithVirtualRoot()
        {
            // Arrange
            var node = CreateNode("NAV", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            a.SetAsRoot();
            var aText = CreateNode("TEXT", NodeState.Checked);
            a.Nodes.Add(aText);
            AddNodesTo(node, a);
            var pattern = new ExtractionPattern(node);

            // Act
            var vRoot = pattern.FindVirtualRoot();

            // Assert
            Assert.IsFalse(node.IsRoot());
            Assert.IsTrue(vRoot.IsRoot());
            Assert.AreEqual("A", vRoot.Text);
            Assert.AreEqual(NodeState.Grayed, vRoot.GetState());
            Assert.AreEqual(1, vRoot.Nodes.Count);
        }

        [TestMethod]
        public void TestRetrieveUpperTreeWhenHavingVirtualRoot()
        {
            // Arrange
            var div = CreateNode("DIV", NodeState.Grayed);
            var nav = CreateNode("NAV", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            a.SetAsRoot();
            var aText = CreateNode("TEXT", NodeState.Checked);
            a.Nodes.Add(aText);
            div.Nodes.Add(nav);
            nav.Nodes.Add(a);
            var pattern = new ExtractionPattern(div);

            // Act
            var upperTree = pattern.GetUpperTree();

            // Assert
            Assert.AreEqual("A", upperTree.Text);
            Assert.AreEqual(NodeState.Grayed, upperTree.GetState());
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

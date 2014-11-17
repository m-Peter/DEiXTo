using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using DEiXTo.Services;

namespace DEiXTo.Models.Tests
{
    [TestClass]
    public class ExtractionPatternTests
    {
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
            var div = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Grayed);
            var h2Text = CreateNode("TEXT", NodeState.Checked);
            h2.Nodes.Add(h2Text);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.CheckedImplied);
            p.Nodes.Add(pText);
            var p1 = CreateNode("P", NodeState.Grayed);
            var p1Text = CreateNode("TEXT", NodeState.CheckedSource);
            p1.Nodes.Add(p1Text);
            AddNodesTo(div, h2, p, p1);
            var pattern = new ExtractionPattern(div);

            // Act
            int variables = pattern.CountOutputVariables();

            // Assert
            Assert.AreEqual(3, variables);
        }

        [TestMethod]
        public void TestCollectOutputVariablesWithDefaultFormat()
        {
            // Arrange
            var node = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Checked);
            var p = CreateNode("P", NodeState.CheckedImplied);
            var p1 = CreateNode("P", NodeState.CheckedSource);
            AddNodesTo(node, h2, p, p1);
            var pattern = new ExtractionPattern(node);

            // Act
            var labels = pattern.OutputVariableLabels();

            // Assert
            Assert.AreEqual(3, labels.Count);
            Assert.AreEqual("VAR1", labels[0]);
            Assert.AreEqual("VAR2", labels[1]);
            Assert.AreEqual("VAR3", labels[2]);
        }

        [TestMethod]
        public void TestCollectOutputVariablesWithCustomLabels()
        {
            // Arrange
            var div = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Checked, "HEADER");
            var p = CreateNode("P", NodeState.CheckedImplied, "SYNOPSIS");
            var p1 = CreateNode("P", NodeState.CheckedSource, "CONTENT");
            AddNodesTo(div, h2, p, p1);
            var pattern = new ExtractionPattern(div);

            // Act
            var labels = pattern.OutputVariableLabels();

            // Assert
            Assert.AreEqual(3, labels.Count);
            Assert.AreEqual("HEADER", labels[0]);
            Assert.AreEqual("SYNOPSIS", labels[1]);
            Assert.AreEqual("CONTENT", labels[2]);
        }

        [TestMethod]
        public void TestCollectOutputVariablesWithDefaultAndCustomLabels()
        {
            // Arrange
            var div = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Checked, "HEADER");
            var p = CreateNode("P", NodeState.CheckedImplied);
            var p1 = CreateNode("P", NodeState.CheckedSource, "CONTENT");
            AddNodesTo(div, h2, p, p1);
            var pattern = new ExtractionPattern(div);

            // Act
            var labels = pattern.OutputVariableLabels();

            // Assert
            Assert.AreEqual(3, labels.Count);
            Assert.AreEqual("HEADER", labels[0]);
            Assert.AreEqual("VAR2", labels[1]);
            Assert.AreEqual("CONTENT", labels[2]);
        }

        [TestMethod]
        public void TestTrimUncheckedNodesFromPattern()
        {
            // Arrange
            var div = CreateRootNode();
            var h2 = CreateNode("H2", NodeState.Grayed);
            var p = CreateNode("P", NodeState.Grayed);
            var p1 = CreateNode("P", NodeState.Unchecked);
            AddNodesTo(div, h2, p, p1);
            var pattern = new ExtractionPattern(div);

            // Act
            pattern.TrimUncheckedNodes();

            // Assert
            Assert.AreEqual(2, div.Nodes.Count);
            Assert.AreEqual(h2, div.Nodes[0]);
            Assert.AreEqual(p, div.Nodes[1]);
        }

        [TestMethod]
        public void TestRetrieveVirtualRootFromExtractionPattern()
        {
            // Arrange
            var nav = CreateNode("NAV", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            a.SetAsRoot();
            var aText = CreateNode("TEXT", NodeState.Checked);
            a.Nodes.Add(aText);
            AddNodesTo(nav, a);
            var pattern = new ExtractionPattern(nav);

            // Act
            var vRoot = pattern.FindVirtualRoot();

            // Assert
            Assert.IsFalse(nav.IsRoot());
            Assert.IsTrue(vRoot.IsRoot());
            Assert.AreEqual("A", vRoot.Text);
            Assert.AreEqual(NodeState.Grayed, vRoot.GetState());
            Assert.AreEqual(1, vRoot.Nodes.Count);
            Assert.AreEqual("TEXT", vRoot.Nodes[0].Text);
        }

        [TestMethod]
        public void TestRetrieveUpperTreeFromExtractionPattern()
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
            Assert.AreEqual("NAV", upperTree.Parent.Text);
            Assert.AreEqual("DIV", upperTree.Parent.Parent.Text);
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

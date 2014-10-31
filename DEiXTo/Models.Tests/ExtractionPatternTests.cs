using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void TestCountOutputVariables()
        {
            // Arrange
            var node = new TreeNode("DIV");
            NodeInfo gInfo = new NodeInfo();
            gInfo .State = NodeState.Grayed;
            node.Tag = gInfo ;
            var h2 = new TreeNode("H2");
            h2.Tag = gInfo ;
            var text = new TreeNode("TEXT");
            NodeInfo cInfo = new NodeInfo();
            text.Tag = cInfo;
            var p = new TreeNode("P");
            p.Tag = gInfo;
            var p1 = new TreeNode("P");
            p1.Tag = gInfo;
            h2.Nodes.Add(text);
            p.Nodes.Add(text);
            p1.Nodes.Add(text);
            node.Nodes.Add(h2);
            node.Nodes.Add(p);
            node.Nodes.Add(p1);
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

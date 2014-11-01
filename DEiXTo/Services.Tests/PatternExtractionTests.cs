using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class PatternExtractionTests
    {
        private TreeNode getRootTree()
        {
            // section
            //    a
            //       img
            //    h2
            //       a
            //           text
            //    p
            //       text
            var section = CreateRootNode("SECTION");
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            var h2 = CreateNode("H2", NodeState.Grayed);
            var text = CreateNode("TEXT", NodeState.Checked);
            var p = CreateNode("P", NodeState.Grayed);

            AddNodesTo(section, a, h2, p);
            a.AddNode(img);
            h2.AddNode(a);
            a.AddNode(text);
            p.AddNode(text);

            return section;
        }

        private TreeNode getInstanceTree()
        {
            // section
            //    a
            //       img
            //    h2
            //       a
            //          text
            //    p
            //       text
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            var h2 = CreateNode("H2", NodeState.Grayed);
            var text = CreateNode("TEXT", NodeState.Checked);
            var p = CreateNode("P", NodeState.Grayed);

            AddNodesTo(section, a, h2, p);
            a.AddNode(img);
            h2.AddNode(a);
            a.AddNode(text);
            p.AddNode(text);

            return section;
        }

        private TreeNode getTreeWithOptional()
        {
            // section
            //    a
            //       img
            //    h2
            //       a
            //          text
            //    p
            //       text
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            var h2 = CreateNode("H2", NodeState.Grayed);
            var text = CreateNode("TEXT", NodeState.Checked);
            var p = CreateNode("P", NodeState.GrayedImplied);

            AddNodesTo(section, a, h2, p);
            a.AddNode(img);
            h2.AddNode(a);
            a.AddNode(text);
            p.AddNode(text);

            return section;
        }

        private TreeNode GetTreeWithUnchecked()
        {
            // section
            //    header
            //       h2
            //          text
            //    p
            //       text
            //    p
            //       text
            var section = CreateNode("SECTION", NodeState.Grayed);
            var header = CreateNode("HEADER", NodeState.Grayed);
            var h2 = CreateNode("H2", NodeState.Grayed);
            var text = CreateNode("TEXT", NodeState.Checked);
            var p = CreateNode("P", NodeState.Unchecked);
            var text1 = CreateNode("TEXT", NodeState.Unchecked);
            var p1 = CreateNode("P", NodeState.Unchecked);
            var text2 = CreateNode("TEXT", NodeState.Unchecked);

            AddNodesTo(section, header, p, p1);
            header.AddNode(h2);
            h2.AddNode(text);
            p.AddNode(text1);
            p1.AddNode(text2);

            return section;
        }

        private TreeNode GetRootWithChecked()
        {
            // section
            //    header
            //       h2
            //          text
            //    p
            //       text
            //    p
            //       text
            var section = CreateRootNode("SECTION");
            var header = CreateNode("HEADER", NodeState.Grayed);
            var h2 = CreateNode("H2", NodeState.Grayed);
            var text = CreateNode("TEXT", NodeState.Checked);
            var p = CreateNode("P", NodeState.Grayed);
            var text1 = CreateNode("TEXT", NodeState.Checked);
            var p1 = CreateNode("P", NodeState.Grayed);
            var text2 = CreateNode("TEXT", NodeState.Checked);

            AddNodesTo(section, header, p, p1);
            header.AddNode(h2);
            h2.AddNode(text);
            p.AddNode(text1);
            p1.AddNode(text2);

            return section;
        }

        private TreeNodeCollection getDOMNodes(int instances, bool isOptional=false)
        {
            var body = new TreeNode("BODY");
            var div = new TreeNode("DIV");
            body.AddNode(div);
            
            for (int i = 0; i < instances; i++)
            {
                div.AddNode(getInstanceTree());
            }

            if (isOptional)
            {
                div.AddNode(getTreeWithOptional());
            }

            return body.Nodes;
        }

        [TestMethod]
        public void TestFindSingleMatch()
        {
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(1);

            PatternExecutor pattern = new PatternExecutor(extraction, domNodes);
            pattern.FindMatches();
            Assert.AreEqual(1, pattern.Count);
        }

        [TestMethod]
        public void TestTwoMatches()
        {
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(2);

            PatternExecutor pattern = new PatternExecutor(extraction, domNodes);
            pattern.FindMatches();
            Assert.AreEqual(2, pattern.Count);
        }

        [TestMethod]
        public void TestManyMatches()
        {
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(11);

            PatternExecutor pattern = new PatternExecutor(extraction, domNodes);
            pattern.FindMatches();
            Assert.AreEqual(11, pattern.Count);
        }

        [TestMethod]
        public void TestManyMatchesWithOneOptionalNode()
        {
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(10, true);

            PatternExecutor pattern = new PatternExecutor(extraction, domNodes);
            pattern.FindMatches();
            Assert.AreEqual(11, pattern.Count);
        }

        [TestMethod]
        public void TestDOMWithUnchecked()
        {
            var root = GetRootWithChecked();
            var extraction = new ExtractionPattern(root);
            var body = new TreeNode("BODY");
            var div = new TreeNode("DIV");
            div.AddNode(root);
            div.AddNode(GetTreeWithUnchecked());
            div.AddNode(root);
            body.AddNode(div);

            PatternExecutor pattern = new PatternExecutor(extraction, body.Nodes);
            pattern.FindMatches();
            Assert.AreEqual(3, pattern.Count);
        }

        private void AddNodesTo(TreeNode node, params TreeNode[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                node.Nodes.Add(nodes[i]);
            }
        }

        private TreeNode CreateRootNode(string tag)
        {
            var root = new TreeNode(tag);
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

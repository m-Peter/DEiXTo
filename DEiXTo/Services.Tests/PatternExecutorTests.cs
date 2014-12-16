using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class PatternExecutorTests
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

        private TreeNodeCollection getDOMNodes(int instances, bool isOptional = false)
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
            // Arrange
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(1);
            Executor pattern = new Executor(extraction, domNodes);

            // Act
            pattern.FindMatches();

            // Assert
            Assert.AreEqual(1, pattern.Count);
        }

        [TestMethod]
        public void TestTwoMatches()
        {
            // Arrange
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(2);
            Executor pattern = new Executor(extraction, domNodes);

            // Act
            pattern.FindMatches();

            // Assert
            Assert.AreEqual(2, pattern.Count);
        }

        [TestMethod]
        public void TestManyMatches()
        {
            // Arrange
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(11);
            Executor pattern = new Executor(extraction, domNodes);

            // Act
            pattern.FindMatches();

            // Assert
            Assert.AreEqual(11, pattern.Count);
        }

        [TestMethod]
        public void TestManyMatchesWithOneOptionalNode()
        {
            // Arrange
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(10, true);
            Executor pattern = new Executor(extraction, domNodes);

            // Act
            pattern.FindMatches();

            // Assert
            Assert.AreEqual(11, pattern.Count);
        }

        [TestMethod]
        public void TestDOMWithUnchecked()
        {
            // Arrange
            var root = GetRootWithChecked();
            var extraction = new ExtractionPattern(root);
            var body = new TreeNode("BODY");
            var div = new TreeNode("DIV");
            div.AddNode(root);
            div.AddNode(GetTreeWithUnchecked());
            div.AddNode(root);
            body.AddNode(div);
            Executor pattern = new Executor(extraction, body.Nodes);

            // Act
            pattern.FindMatches();

            // Assert
            Assert.AreEqual(3, pattern.Count);
        }

        [TestMethod]
        public void TestMathingWithVirtualRoot()
        {
            // Arrange
            var nav = CreateNode("NAV", NodeState.Grayed);
            var a = CreateRootNode("A");
            var text = CreateNode("TEXT", NodeState.Checked);
            a.Nodes.Add(text);
            nav.Nodes.Add(a);
            var extraction = new ExtractionPattern(nav);
            var nodes = CreateDomNodes();
            Executor pattern = new Executor(extraction, nodes);

            // Act
            pattern.FindMatches();

            // Assert
            Assert.AreEqual(3, pattern.Count);
        }

        [TestMethod]
        public void TestFilterAttributeWithRegex()
        {
            // Arrange
            var img = CreateRootNode("IMG");
            var constraint = new TagAttributeConstraint("src", "/img/main/post.png");
            img.SetAttrConstraint(constraint);
            var div = CreateNode("DIV", NodeState.Grayed);
            var attributes = new TagAttributeCollection();
            attributes.Add(new TagAttribute { Name = "src", Value = "/img/main/post.png" });
            var jpeg = new TagAttributeCollection();
            jpeg.Add(new TagAttribute{Name = "src", Value = "img/main/header.jpeg"});
            var gif = new TagAttributeCollection();
            gif.Add(new TagAttribute { Name = "src", Value = "img/main/footer.gif" });
            var img1 = CreateNode("IMG", NodeState.Grayed);
            img1.SetContent("/img/main/post.png");
            img1.SetAttributes(attributes);
            var img2 = CreateNode("IMG", NodeState.Grayed);
            img2.SetContent("/img/main/post.png");
            img2.SetAttributes(attributes);
            var img3 = CreateNode("IMG", NodeState.Grayed);
            img3.SetContent("/img/main/post.png");
            img3.SetAttributes(attributes);
            var img4 = CreateNode("IMG", NodeState.Grayed);
            img4.SetContent("/img/main/header.jpeg");
            img4.SetAttributes(jpeg);
            var img5 = CreateNode("IMG", NodeState.Grayed);
            img5.SetContent("/img/main/footer.gif");
            img5.SetAttributes(gif);
            AddNodesTo(div, img1, img2, img3, img4, img5);
            var extraction = new ExtractionPattern(img);
            var pattern = new Executor(extraction, div.Nodes);

            // Act
            pattern.FindMatches();

            // Assert
            Assert.AreEqual(5, div.Nodes.Count);
            Assert.AreEqual(3, pattern.Count);
        }

        [TestMethod]
        public void TestSplitTreeWithVirtualRoot()
        {
            // Arrange
            // section
            //    a
            //       img
            //    p
            //       text
            //    h2
            //       a
            //          text
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            a.AddNode(img);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked);
            p.AddNode(pText);
            var h2 = CreateRootNode("H2");
            var h2A = CreateNode("A", NodeState.Grayed);
            h2.AddNode(h2A);
            var aText = CreateNode("TEXT", NodeState.Checked);
            h2A.AddNode(aText);

            AddNodesTo(section, a, p, h2);
            var pattern = new ExtractionPattern(section);

            // Act
            var vRoot = pattern.FindVirtualRoot();
            var upperTree = pattern.GetUpperTree();

            // Assert
            Assert.AreEqual("H2", vRoot.Text);
            Assert.AreEqual(NodeState.Grayed, vRoot.GetState());
            Assert.AreEqual("H2", upperTree.Text);
            Assert.AreEqual(NodeState.Grayed, upperTree.GetState());
        }

        [TestMethod]
        public void TestCompareUpperTreeInc()
        {
            // Arrange
            // section
            //    a
            //       img
            //    p
            //       text
            //    h2
            //       a
            //          text
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            a.AddNode(img);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked);
            p.AddNode(pText);
            var h2 = CreateRootNode("H2");
            var h2A = CreateNode("A", NodeState.Grayed);
            h2.AddNode(h2A);
            var aText = CreateNode("TEXT", NodeState.Checked);
            h2A.AddNode(aText);

            AddNodesTo(section, a, p, h2);
            var pattern = new ExtractionPattern(section);

            // Act
            var vRoot = pattern.FindVirtualRoot();
            var leaf = pattern.GetUpperTree();

            // Assert
            Assert.AreEqual("H2", vRoot.Text);
            Assert.AreEqual("H2", leaf.Text);
            Assert.AreEqual("SECTION", leaf.Parent.Text);
        }

        [TestMethod]
        public void TestCompareTreeWithVirtualRootInIt()
        {
            // Arrange
            // section
            //    a
            //       img
            //    p
            //       text
            //    h2
            //       a
            //          text
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            a.AddNode(img);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked);
            p.AddNode(pText);
            var h2 = CreateRootNode("H2");
            var h2A = CreateNode("A", NodeState.Grayed);
            h2.AddNode(h2A);
            var aText = CreateNode("TEXT", NodeState.Checked);
            h2A.AddNode(aText);

            AddNodesTo(section, a, p, h2);
            var pattern = new ExtractionPattern(section);

            // Act
            var vRoot = pattern.FindVirtualRoot();
            var leafNode = pattern.GetUpperTree();

            // Assert
            Assert.AreEqual("H2", leafNode.Text);
            Assert.AreEqual("SECTION", leafNode.Parent.Text);
        }

        [TestMethod]
        public void TestCheckUpperReturnsFalse()
        {
            // Arrange
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            a.AddNode(img);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked);
            p.AddNode(pText);
            var h2 = CreateRootNode("H2");
            var h2A = CreateNode("A", NodeState.Grayed);
            h2.AddNode(h2A);
            var aText = CreateNode("TEXT", NodeState.Checked);
            h2A.AddNode(aText);

            AddNodesTo(section, a, p, h2);
            var pattern = new ExtractionPattern(section);

            // Act
            var vRoot = pattern.FindVirtualRoot();
            var leafNode = pattern.GetUpperTree();
        }

        [TestMethod]
        public void TestRegexConstraint()
        {
            // Arrange
            var p = CreateRootNode("P");
            p.SetRegexConstraint(new RegexConstraint(@"\d+"));
            var div = CreateNode("DIV", NodeState.Grayed);
            var p1 = CreateNode("P", NodeState.Grayed);
            p1.Tag = new NodeInfo();
            p1.SetContent("price 50");
            var p2 = CreateNode("P", NodeState.Grayed);
            p2.Tag = new NodeInfo();
            p2.SetContent("price None");
            var p3 = CreateNode("P", NodeState.Grayed);
            p3.Tag = new NodeInfo();
            p3.SetContent("price 60");
            var p4 = CreateNode("P", NodeState.Grayed);
            p4.Tag = new NodeInfo();
            p4.SetContent("price None");
            AddNodesTo(div, p1, p2, p3, p4);
            var extraction = new ExtractionPattern(p);
            var pattern = new Executor(extraction, div.Nodes);

            // Act
            pattern.FindMatches();

            // Assert
            Assert.AreEqual(2, pattern.Count);
        }

        private TreeNode GetUnmatchingInstance()
        {
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            a.AddNode(img);
            var p = CreateNode("P", NodeState.Grayed);
            var h2 = CreateRootNode("H2");
            var h2A = CreateNode("A", NodeState.Grayed);
            h2.AddNode(h2A);
            var aText = CreateNode("TEXT", NodeState.Checked);
            h2A.AddNode(aText);

            AddNodesTo(section, a, p, h2);

            return h2;
        }

        private TreeNode GetLeftTree()
        {
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            a.AddNode(img);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked);
            p.AddNode(pText);
            var h2 = CreateRootNode("H2");
            var h2A = CreateNode("A", NodeState.Grayed);
            h2.AddNode(h2A);
            var aText = CreateNode("TEXT", NodeState.Checked);
            h2A.AddNode(aText);

            AddNodesTo(section, a, p, h2);

            return section;
        }

        private TreeNode GetRightTree()
        {
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            a.AddNode(img);
            var p = CreateNode("P", NodeState.Grayed);
            var pText = CreateNode("TEXT", NodeState.Checked);
            p.AddNode(pText);
            var h2 = CreateRootNode("H2");
            var h2A = CreateNode("A", NodeState.Grayed);
            h2.AddNode(h2A);
            var aText = CreateNode("TEXT", NodeState.Checked);
            h2A.AddNode(aText);

            AddNodesTo(section, a, p, h2);

            return section;
        }

        public TreeNodeCollection CreateDomNodes()
        {
            var body = CreateNode("BODY", NodeState.Grayed);
            var div = CreateNode("DIV", NodeState.Grayed);
            var resources = CreateNode("DIV", NodeState.Grayed);
            var nav = CreateNode("NAV", NodeState.Grayed);
            var a1 = CreateNode("A", NodeState.Grayed);
            var a1Text = CreateNode("TEXT", NodeState.Checked);
            a1.Nodes.Add(a1Text);
            var a2 = CreateNode("A", NodeState.Grayed);
            var a2Text = CreateNode("TEXT", NodeState.Checked);
            a2.Nodes.Add(a2Text);
            var a3 = CreateNode("A", NodeState.Grayed);
            var a3Text = CreateNode("TEXT", NodeState.Checked);
            a3.Nodes.Add(a3Text);

            body.Nodes.Add(div);
            div.Nodes.Add(resources);
            resources.Nodes.Add(nav);
            AddNodesTo(nav, a1, a2, a3);

            var a4 = CreateNode("A", NodeState.Grayed);
            var a4Text = CreateNode("TEXT", NodeState.Checked);
            a4.Nodes.Add(a4Text);
            var a5 = CreateNode("A", NodeState.Grayed);
            var a5Text = CreateNode("TEXT", NodeState.Checked);
            a5.Nodes.Add(a5Text);
            var a6 = CreateNode("A", NodeState.Grayed);
            var a6Text = CreateNode("TEXT", NodeState.Checked);
            a6.Nodes.Add(a6Text);
            var a7 = CreateNode("A", NodeState.Grayed);
            var a7Text = CreateNode("TEXT", NodeState.Checked);
            a7.Nodes.Add(a7Text);
            AddNodesTo(div, a4, a5, a6);

            return body.Nodes;
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

using DEiXTo.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
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
            var section = new TreeNode("section");
            section.Tag = new PointerInfo();
            section.SetAsRoot();
            section.SetState(NodeState.Grayed);
            
            var a = new TreeNode("A");
            a.Tag = new PointerInfo();
            a.SetState(NodeState.Grayed);

            var img = new TreeNode("IMG");
            img.Tag = new PointerInfo();
            img.SetState(NodeState.Grayed);
            
            var h2 = new TreeNode("H2");
            h2.Tag = new PointerInfo();
            h2.SetState(NodeState.Grayed);
            
            var text = new TreeNode("TEXT");
            text.Tag = new PointerInfo();
            text.SetState(NodeState.Checked);
            
            var p = new TreeNode("P");
            p.Tag = new PointerInfo();
            p.SetState(NodeState.Grayed);
            
            section.AddNode(a);
            a.AddNode(img);
            section.AddNode(h2);
            h2.AddNode(a);
            a.AddNode(text);
            section.AddNode(p);
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
            var section = new TreeNode("section");
            section.Tag = new PointerInfo();
            section.SetState(NodeState.Grayed);
            
            var a = new TreeNode("A");
            a.Tag = new PointerInfo();
            a.SetState(NodeState.Grayed);
            
            var img = new TreeNode("IMG");
            img.Tag = new PointerInfo();
            img.SetState(NodeState.Grayed);
            
            var h2 = new TreeNode("H2");
            h2.Tag = new PointerInfo();
            h2.SetState(NodeState.Grayed);
            
            var text = new TreeNode("TEXT");
            text.Tag = new PointerInfo();
            text.SetState(NodeState.Checked);
            
            var p = new TreeNode("P");
            p.Tag = new PointerInfo();
            p.SetState(NodeState.Grayed);

            section.AddNode(a);
            a.AddNode(img);
            section.AddNode(h2);
            h2.AddNode(a);
            a.AddNode(text);
            section.AddNode(p);
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
            var section = new TreeNode("section");
            section.Tag = new PointerInfo();
            section.SetState(NodeState.Grayed);

            var a = new TreeNode("A");
            a.Tag = new PointerInfo();
            a.SetState(NodeState.Grayed);

            var img = new TreeNode("IMG");
            img.Tag = new PointerInfo();
            img.SetState(NodeState.Grayed);

            var h2 = new TreeNode("H2");
            h2.Tag = new PointerInfo();
            h2.SetState(NodeState.Grayed);

            var text = new TreeNode("TEXT");
            text.Tag = new PointerInfo();
            text.SetState(NodeState.Grayed);

            var p = new TreeNode("P");
            p.Tag = new PointerInfo();
            p.SetState(NodeState.GrayedImplied);

            section.AddNode(a);
            a.AddNode(img);
            section.AddNode(h2);
            h2.AddNode(a);
            a.AddNode(text);
            section.AddNode(p);
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
            var section = new TreeNode("section");
            section.Tag = new PointerInfo();
            section.SetState(NodeState.Grayed);
            
            var header = new TreeNode("header");
            header.Tag = new PointerInfo();
            header.SetState(NodeState.Grayed);

            var h2 = new TreeNode("h2");
            h2.Tag = new PointerInfo();
            h2.SetState(NodeState.Grayed);
            
            var text = new TreeNode("text");
            text.Tag = new PointerInfo();
            text.SetState(NodeState.Checked);

            var p = new TreeNode("p");
            p.Tag = new PointerInfo();
            p.SetState(NodeState.Unchecked);

            var text1 = new TreeNode("text");
            text1.Tag = new PointerInfo();
            text1.SetState(NodeState.Unchecked);
            
            var p1 = new TreeNode("p");
            p1.Tag = new PointerInfo();
            p1.SetState(NodeState.Unchecked);

            var text2 = new TreeNode("text");
            text2.Tag = new PointerInfo();
            text2.SetState(NodeState.Unchecked);

            section.AddNode(header);
            header.AddNode(h2);
            h2.AddNode(text);
            section.AddNode(p);
            p.AddNode(text1);
            section.AddNode(p1);
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
            var section = new TreeNode("section");
            section.Tag = new PointerInfo();
            section.SetAsRoot();
            section.SetState(NodeState.Grayed);

            var header = new TreeNode("header");
            header.Tag = new PointerInfo();
            header.SetState(NodeState.Grayed);

            var h2 = new TreeNode("h2");
            h2.Tag = new PointerInfo();
            h2.SetState(NodeState.Grayed);

            var text = new TreeNode("text");
            text.Tag = new PointerInfo();
            text.SetState(NodeState.Checked);

            var p = new TreeNode("p");
            p.Tag = new PointerInfo();
            p.SetState(NodeState.Grayed);

            var text1 = new TreeNode("text");
            text1.Tag = new PointerInfo();
            text1.SetState(NodeState.Checked);

            var p1 = new TreeNode("p");
            p1.Tag = new PointerInfo();
            p1.SetState(NodeState.Grayed);

            var text2 = new TreeNode("text");
            text2.Tag = new PointerInfo();
            text2.SetState(NodeState.Checked);

            section.AddNode(header);
            header.AddNode(h2);
            h2.AddNode(text);
            section.AddNode(p);
            p.AddNode(text1);
            section.AddNode(p1);
            p1.AddNode(text2);

            return section;
        }

        private TreeNodeCollection getDOMNodes(int instances, bool isOptional=false)
        {
            var body = new TreeNode("body");
            var div = new TreeNode("div");
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
            var domNodes = getDOMNodes(1);

            PatternExtraction pattern = new PatternExtraction(root, domNodes);
            pattern.FindMatches();
            Assert.AreEqual(1, pattern.Results);
        }

        [TestMethod]
        public void TestTwoMatches()
        {
            var root = getRootTree();
            var domNodes = getDOMNodes(2);

            PatternExtraction pattern = new PatternExtraction(root, domNodes);
            pattern.FindMatches();
            Assert.AreEqual(2, pattern.Results);
        }

        [TestMethod]
        public void TestManyMatches()
        {
            var root = getRootTree();
            var domNodes = getDOMNodes(11);

            PatternExtraction pattern = new PatternExtraction(root, domNodes);
            pattern.FindMatches();
            Assert.AreEqual(11, pattern.Results);
        }

        [TestMethod]
        public void TestManyMatchesWithOneOptionalNode()
        {
            var root = getRootTree();
            var domNodes = getDOMNodes(10, true);

            PatternExtraction pattern = new PatternExtraction(root, domNodes);
            pattern.FindMatches();
            Assert.AreEqual(11, pattern.Results);
        }

        [TestMethod]
        public void TestDOMWithUnchecked()
        {
            var root = GetRootWithChecked();
            var body = new TreeNode("BODY");
            var div = new TreeNode("DIV");
            div.AddNode(root);
            div.AddNode(GetTreeWithUnchecked());
            div.AddNode(root);
            body.AddNode(div);

            PatternExtraction pattern = new PatternExtraction(root, body.Nodes);
            pattern.FindMatches();
            Assert.AreEqual(3, pattern.Results);
        }
    }
}

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
    }
}

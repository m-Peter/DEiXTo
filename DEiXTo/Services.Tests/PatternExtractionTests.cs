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
            var a = new TreeNode("A");
            var img = new TreeNode("IMG");
            var h2 = new TreeNode("H2");
            var text = new TreeNode("TEXT");
            var p = new TreeNode("P");
            
            section.AddNode(a);
            a.AddNode(img);
            section.AddNode(h2);
            h2.AddNode(a);
            a.AddNode(text);
            section.AddNode(p);
            p.AddNode(text);
            section.Tag = new PointerInfo();
            section.SetAsRoot();

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
            var a = new TreeNode("A");
            var img = new TreeNode("IMG");
            var h2 = new TreeNode("H2");
            var text = new TreeNode("TEXT");
            var p = new TreeNode("P");

            section.AddNode(a);
            a.AddNode(img);
            section.AddNode(h2);
            h2.AddNode(a);
            a.AddNode(text);
            section.AddNode(p);
            p.AddNode(text);

            return section;
        }

        private TreeNodeCollection getDOMNodes(int instances)
        {
            var body = new TreeNode("body");
            var div = new TreeNode("div");
            body.AddNode(div);
            
            for (int i = 0; i < instances; i++)
            {
                div.AddNode(getInstanceTree());
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
    }
}

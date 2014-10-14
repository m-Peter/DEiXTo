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
            // div
            //    section
            //       a
            //          img
            //       h2
            //          a
            //              text
            //       p
            //          text
            var div = new TreeNode("DIV");
            var section = new TreeNode("section");
            var a = new TreeNode("A");
            var img = new TreeNode("IMG");
            var h2 = new TreeNode("H2");
            var text = new TreeNode("TEXT");
            var p = new TreeNode("P");

            div.AddNode(section);
            section.AddNode(a);
            a.AddNode(img);
            section.AddNode(h2);
            h2.AddNode(a);
            a.AddNode(text);
            section.AddNode(p);
            p.AddNode(text);

            return div;
        }

        [TestMethod]
        public void TestFindSingleMatch()
        {
            var root = getRootTree();
            var instance = getInstanceTree();

            PatternExtraction pattern = new PatternExtraction(root, instance.Nodes);
            pattern.FindMatches();
            Assert.AreEqual(1, pattern.Results);
        }
    }
}

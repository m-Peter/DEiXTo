﻿using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
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
            // Arrange
            var root = getRootTree();
            var extraction = new ExtractionPattern(root);
            var domNodes = getDOMNodes(1);
            PatternExecutor pattern = new PatternExecutor(extraction, domNodes);
            
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
            PatternExecutor pattern = new PatternExecutor(extraction, domNodes);
            
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
            PatternExecutor pattern = new PatternExecutor(extraction, domNodes);
            
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
            PatternExecutor pattern = new PatternExecutor(extraction, domNodes);
            
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
            PatternExecutor pattern = new PatternExecutor(extraction, body.Nodes);
            
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
            PatternExecutor pattern = new PatternExecutor(extraction, nodes);

            // Act
            pattern.FindMatches();

            // Assert
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
            Assert.AreEqual("SECTION", upperTree.Text);
            Assert.AreEqual(NodeState.Grayed, upperTree.GetState());

            // Act
            string backwards = "";
            traverseBackwards(upperTree, ref backwards);
            Assert.AreEqual("-TEXT-P-IMG-A-SECTION", backwards);
        }

        [TestMethod]
        public void TestCompareTrees()
        {
            // Arrange
            var left = GetLeftTree();
            var right = GetRightTree();

            // Act
            var match = CompareTrees(left, right);

            // Assert
            Assert.IsTrue(match);
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
            var leaf = pattern.GetUpperTreeInc();

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
            var leafNode = pattern.GetUpperTreeInc();

            // Assert
            Assert.IsTrue(CompareTrees(h2, vRoot));
            Assert.AreEqual("H2", leafNode.Text);
            Assert.AreEqual("SECTION", leafNode.Parent.Text);
            Assert.IsTrue(CheckUpper(leafNode, h2));
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
            var leafNode = pattern.GetUpperTreeInc();

            // Assert
            var instance = GetUnmatchingInstance();
            var match = CheckUpper(leafNode, instance);
            Assert.IsFalse(match);
        }

        private TreeNode GetUnmatchingInstance()
        {
            var section = CreateNode("SECTION", NodeState.Grayed);
            var a = CreateNode("A", NodeState.Grayed);
            var img = CreateNode("IMG", NodeState.Grayed);
            a.AddNode(img);
            var p = CreateNode("P", NodeState.Grayed);
            //var pText = CreateNode("TEXT", NodeState.Checked);
            //p.AddNode(pText);
            var h2 = CreateRootNode("H2");
            var h2A = CreateNode("A", NodeState.Grayed);
            h2.AddNode(h2A);
            var aText = CreateNode("TEXT", NodeState.Checked);
            h2A.AddNode(aText);

            AddNodesTo(section, a, p, h2);

            return h2;
        }

        private bool CheckUpper(TreeNode pattern, TreeNode instance)
        {
            if (pattern.Text != instance.Text)
            {
                return false;
            }

            var leftParent = pattern.Parent;
            var rightParent = instance.Parent;

            if (leftParent == null)
            {
                return true;
            }

            if (rightParent == null)
            {
                return false;
            }

            if (!CompareTrees(leftParent, rightParent))
            {
                return false;
            }

            CheckUpper(leftParent, rightParent);

            return true;
        }

        private bool CompareTrees(TreeNode left, TreeNode right)
        {
            if (left.Text != right.Text)
            {
                return false;
            }

            for (int i = 0; i < left.Nodes.Count; i++)
            {
                var nextLeft = left.Nodes[i];
                bool hasNode = HasNextNode(right, i);

                if (hasNode)
                {
                    return false;
                }

                var nextRight = right.Nodes[i];

                if (!CompareTrees(nextLeft, nextRight))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HasNextNode(TreeNode node, int index)
        {
            return node.Nodes.Count <= index;
        }

        private void traverse(TreeNode node, Stack<TreeNode> stack)
        {
            stack.Push(node);

            foreach (TreeNode n in node.Nodes)
            {
                traverse(n, stack);
            }
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

        private void traverseBackwards(TreeNode node, Stack<TreeNode> stack)
        {
            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                traverseBackwards(node.Nodes[i], stack);
            }
            stack.Push(node);
        }

        private void traverseBackwards(TreeNode root, ref string format)
        {
            for (int i = root.Nodes.Count - 1; i >= 0; i--)
            {
                traverseBackwards(root.Nodes[i], ref format);
            }

            format += string.Format("-{0}", root.Text);
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
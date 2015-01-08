using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using DEiXTo.Models;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class TestTreeComparison
    {
        // Testing Strategy for Comparing Trees.
        // - Pattern and Instance with a single same node. X
        // - Pattern and Instance with a different single node. X
        // - Pattern with single node and Instance of level one. X
        // - Pattern of level one and Instance with single node. X
        // - Pattern and Instance of level one matching. X
        // - Pattern and Instance of level one mismatch. X
        // - Pattern with level one and instance of level one with two nodes. X

        [TestMethod]
        public void TestTwoUnitTrees()
        {
            var pattern = new TreeNode("DIV");
            var instance = new TreeNode("DIV");

            var result = CompareTrees(pattern, instance);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestTwoDifferentTrees()
        {
            var pattern = new TreeNode("DIV");
            var instance = new TreeNode("P");

            var result = CompareTrees(pattern, instance);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestLevelOneInstance()
        {
            var pattern = new TreeNode("DIV");
            var instance = new TreeNode("DIV");
            instance.Nodes.Add("P");

            var result = CompareTrees(pattern, instance);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLevelOnePattern()
        {
            var pattern = new TreeNode("DIV");
            pattern.Nodes.Add("P");
            var instance = new TreeNode("DIV");

            var result = CompareTrees(pattern, instance);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestLevelOnePatternAndInstance()
        {
            var pattern = new TreeNode("DIV");
            pattern.Nodes.Add("P");
            var instance = new TreeNode("DIV");
            instance.Nodes.Add("P");

            var result = CompareTrees(pattern, instance);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLevelOnePatternAndInstanceDifferent()
        {
            var pattern = new TreeNode("DIV");
            pattern.Nodes.Add("P");
            var instance = new TreeNode("DIV");
            instance.Nodes.Add("H1");

            var result = CompareTrees(pattern, instance);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestLevelOnePatternAndInstanceWithTwoChildNodes()
        {
            var pattern = new TreeNode("DIV");
            pattern.Nodes.Add("P");
            var instance = new TreeNode("DIV");
            instance.Nodes.Add("H1");
            instance.Nodes.Add("P");

            var result = CompareTrees(pattern, instance);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestInstanceWithOneMoreNode()
        {
            var pattern = new TreeNode("DIV");
            var pa = pattern.Nodes.Add("A");
            pa.Nodes.Add("TEXT");
            var pd = pattern.Nodes.Add("DIV");
            pd.Nodes.Add("TEXT");
            pd.Nodes.Add("DIV");

            var instance = new TreeNode("DIV");
            var ia = instance.Nodes.Add("A");
            ia.Nodes.Add("TEXT");
            var id = instance.Nodes.Add("DIV");
            var id1 = id.Nodes.Add("DIV");
            var ibtn = id1.Nodes.Add("BUTTON");
            ibtn.Nodes.Add("TEXT");
            id.Nodes.Add("TEXT");
            id.Nodes.Add("DIV");

            var result = CompareTrees(pattern, instance);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDoNotMatch()
        {
            var pattern = new TreeNode("DIV");
            var pa = pattern.Nodes.Add("A");
            pa.Nodes.Add("TEXT");
            var pd = pattern.Nodes.Add("DIV");
            pd.Nodes.Add("TEXT");
            pd.Nodes.Add("DIV");

            var instance = new TreeNode("DIV");
            var ia = instance.Nodes.Add("A");
            ia.Nodes.Add("TEXT");

            var result = CompareTrees(pattern, instance);

            Assert.IsFalse(result);
        }

        private bool CompareTrees(TreeNode left, TreeNode right)
        {
            // Check for tag matching
            if (left.Text != right.Text)
            {
                return false;
            }

            var childNodes = left.Nodes.Count;

            for (int i = 0; i < childNodes; i++)
            {
                var nextLeft = left.Nodes[i];
                
                if (right.Nodes.Count <= i)
                {
                    return false;
                }
                
                var index = i;
                var nextRight = right.Nodes[index];

                if (!CompareTrees(nextLeft, nextRight))
                {
                    index += 1;
                    if (right.Nodes.Count > index)
                    {
                        nextRight = right.Nodes[index];
                        return CompareTrees(nextLeft, nextRight);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}

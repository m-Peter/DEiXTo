using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEiXTo.Models;
using System.Windows.Forms;

namespace DEiXTo.Models.Tests
{
    [TestClass]
    public class TreeNodeTests
    {
        [TestMethod]
        public void TestAddRegexConstraintToTreeNode()
        {
            // Arrange
            var pattern = @"\d+";
            var constraint = new RegexConstraint(pattern);

            // Act
            var node = new TreeNode();
            node.Tag = new NodeInfo();
            node.SetRegexConstraint(constraint);

            // Assert
            Assert.IsTrue(node.HasRegexConstraint());
        }

        [TestMethod]
        public void TestGetRegexConstraintFromTreeNode()
        {
            // Arrange
            var pattern = @"\d+";
            var constraint = new RegexConstraint(pattern);

            // Act
            var node = new TreeNode();
            node.Tag = new NodeInfo();
            node.SetRegexConstraint(constraint);

            // Assert
            var result = node.GetRegexConstraint();
            Assert.IsNotNull(result);
            Assert.AreEqual(pattern, result.Pattern);
            Assert.AreEqual(ConstraintAction.MatchAndExtract, result.Action);
        }
    }
}

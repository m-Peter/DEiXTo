using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEiXTo.Models.Tests
{
    [TestClass]
    public class ConstraintsTests
    {
        [TestMethod]
        public void TestAddRegexConstraintToNodeInfo()
        {
            // Arrange
            var pattern = @"\d+";
            var constraint = new RegexConstraint(pattern, NodeState.Grayed);
            var node = new NodeInfo();

            // Act
            node.AddConstraint(constraint);

            // Assert
            Assert.AreEqual(1, node.NumOfConstraints());
        }

        [TestMethod]
        public void TestAddAttributeConstraintToNodeInfo()
        {
            // Arrange
            var attribute = "src";
            var pattern = ".png";
            var constraint = new TagAttributeConstraint(attribute, pattern, NodeState.Grayed);
            var node = new NodeInfo();

            // Act
            node.AddConstraint(constraint);

            // Assert
            Assert.AreEqual(1, node.NumOfConstraints());
        }

        [TestMethod]
        public void TestAddRegexAndAttributeConstraint()
        {
            // Arrange
            var regex = new RegexConstraint("a+", NodeState.Grayed);
            var attribute = new TagAttributeConstraint("src", ".png", NodeState.Grayed);
            var node = new NodeInfo();

            // Act
            node.AddConstraint(regex);
            node.AddConstraint(attribute);

            // Assert
            Assert.AreEqual(2, node.NumOfConstraints());
        }

        [TestMethod]
        public void TestEvaluateRegexConstraint()
        {
            // Arrange
            var pattern = new NodeInfo();
            var instance = new NodeInfo();
            instance.Content = "23";
            var regex = new RegexConstraint("\\d+", NodeState.Grayed);

            // Act
            pattern.AddConstraint(regex);

            // Assert
            var match = pattern.EvaluateConstraints(instance);
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void TestDontEvaluateRegexConstraint()
        {
            // Arrange
            var pattern = new NodeInfo();
            var instance = new NodeInfo();
            instance.Content = "abc";
            var regex = new RegexConstraint("dd", NodeState.Grayed);

            // Act
            pattern.AddConstraint(regex);

            // Assert
            var match = pattern.EvaluateConstraints(instance);
            Assert.IsFalse(match);
        }

        [TestMethod]
        public void TestEvaluateAttributeConstraint()
        {
            // Arrange
            var pattern = new NodeInfo();
            var instance = new NodeInfo();
            var attributes = new TagAttributeCollection();
            attributes.Add(new TagAttribute { Name = "src", Value = "/src/github/images/octokit.png" });
            instance.Attributes = attributes;
            var attribute = new TagAttributeConstraint("src", ".png", NodeState.Grayed);

            // Act
            pattern.AddConstraint(attribute);

            // Assert
            var match = pattern.EvaluateConstraints(instance);
            Assert.IsTrue(match);
        }
    }
}

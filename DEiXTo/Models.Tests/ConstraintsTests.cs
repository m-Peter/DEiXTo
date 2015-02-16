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
            node.Constraint = constraint;

            // Assert
            Assert.IsInstanceOfType(node.Constraint, typeof (RegexConstraint));
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
            node.Constraint = constraint;

            // Assert
            Assert.IsInstanceOfType(node.Constraint, typeof(TagAttributeConstraint));
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
            pattern.Constraint = regex;

            // Assert
            var evaluation = pattern.EvaluateConstraints(instance);
            Assert.IsTrue(evaluation.Match);
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
            pattern.Constraint = regex;

            // Assert
            var evaluation = pattern.EvaluateConstraints(instance);
            Assert.IsFalse(evaluation.Match);
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
            pattern.Constraint = attribute;

            // Assert
            var evaluation = pattern.EvaluateConstraints(instance);
            Assert.IsTrue(evaluation.Match);
        }
    }
}

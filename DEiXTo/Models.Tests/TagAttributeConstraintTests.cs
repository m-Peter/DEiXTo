using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEiXTo.Models.Tests
{
    [TestClass]
    public class TagAttributeConstraintTests
    {
        [TestMethod]
        public void TestCreateNewTagAttributeConstraint()
        {
            // Arrange
            var attribute = "src";
            var pattern = ".png";

            // Act
            var constraint = new TagAttributeConstraint(attribute, pattern);

            // Assert
            Assert.AreEqual("src", constraint.Attribute);
            Assert.AreEqual(".png", constraint.Pattern);
        }

        [TestMethod]
        public void TestEvaluateTagAttributeConstraint()
        {
            // Arrange
            var attribute = "src";
            var pattern = ".png";
            var input = "/images/main.png";

            // Act
            var constraint = new TagAttributeConstraint(attribute, pattern);

            // Assert
            Assert.IsTrue(constraint.Evaluate(input));
        }

        [TestMethod]
        public void TestDoesNotEvaluateTagAttributeConstraint()
        {
            // Arrange
            var attribute = "src";
            var pattern = ".jpeg";
            var input = "/images/main.png";

            // Act
            var constraint = new TagAttributeConstraint(attribute, pattern);

            // Assert
            Assert.IsFalse(constraint.Evaluate(input));
        }

        [TestMethod]
        public void TestMatchContentFromTagAttributeConstraint()
        {
            // Arrange
            var attribute = "src";
            var pattern = ".png";
            var input = "/images/main.png";

            // Act
            var constraint = new TagAttributeConstraint(attribute, pattern, ConstraintAction.Match);
            constraint.Evaluate(input);

            // Assert
            Assert.AreEqual("/images/main.png", constraint.Value);
        }

        [TestMethod]
        public void TestMatchAndExtractContentFromTagAttributeConstraint()
        {
            // Arrange
            var attribute = "src";
            var pattern = ".png";
            var input = "/images/main.png";
            
            // Act
            var constraint = new TagAttributeConstraint(attribute, pattern);
            constraint.Evaluate(input);

            // Assert
            Assert.AreEqual(".png", constraint.Value);
        }
    }
}

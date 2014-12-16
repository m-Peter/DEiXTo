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
            var attr = "src";
            var val = ".png";

            // Act
            var constraint = new TagAttributeConstraint(attr, val);

            // Assert
            Assert.AreEqual("src", constraint.Attribute);
            Assert.AreEqual(".png", constraint.Value);
        }

        [TestMethod]
        public void TestEvaluateTagAttributeConstraint()
        {
            // Arrange
            var attr = "src";
            var val = ".png";
            var input = "/images/main.png";

            // Act
            var constraint = new TagAttributeConstraint(attr, val);

            // Assert
            Assert.IsTrue(constraint.Evaluate(input));
        }

        [TestMethod]
        public void TestDoesNotEvaluateTagAttributeConstraint()
        {
            // Arrange
            var attr = "src";
            var val = ".jpeg";
            var input = "/images/main.png";

            // Act
            var constraint = new TagAttributeConstraint(attr, val);

            // Assert
            Assert.IsFalse(constraint.Evaluate(input));
        }
    }
}

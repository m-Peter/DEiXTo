using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DEiXTo.Models.Tests
{
    [TestClass]
    public class RegexContraintTests
    {
        [TestMethod]
        public void TestCreateNewRegexConstraint()
        {
            // Arrange
            var input = "[#1] International Trade";
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(pattern);

            // Assert
            Assert.AreEqual(pattern, constraint.Pattern);
            Assert.AreEqual(ConstraintAction.MatchAndExtract, constraint.Action);
        }

        [TestMethod]
        public void TestEvaluateRegexConstraint()
        {
            // Arrange
            var input = "[#1] International Trade";
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(pattern);

            // Assert
            Assert.IsTrue(constraint.Evaluate(input));
        }

        [TestMethod]
        public void TestDoesNotEvaluateRegexConstraint()
        {
            // Arrange
            var input = "International Trade";
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(pattern);

            // Assert
            Assert.IsFalse(constraint.Evaluate(input));
        }

        [TestMethod]
        public void TestMatchContentFromRegexConstraint()
        {
            // Arrange
            var input = "[#1] International Trade";
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(pattern, ConstraintAction.Match);
            constraint.Evaluate(input);

            // Assert
            Assert.AreEqual("[#1] International Trade", constraint.Value);
            Assert.AreEqual(ConstraintAction.Match, constraint.Action);
        }

        [TestMethod]
        public void TestMatchAndExtractContentFromRegexConstraint()
        {
            // Arrange
            var input = "[#1] International Trade";
            var pattern = "[a-zA-Z]+";

            // Act
            var constraint = new RegexConstraint(pattern);
            constraint.Evaluate(input);

            // Assert
            Assert.AreEqual("International Trade", constraint.Value);
        }
    }
}

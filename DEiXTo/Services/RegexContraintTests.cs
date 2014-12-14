using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DEiXTo.Services
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
            var constraint = new RegexConstraint(input, pattern);

            // Assert
            Assert.AreEqual(input, constraint.Input);
            Assert.AreEqual(pattern, constraint.Pattern);
        }

        [TestMethod]
        public void TestEvaluateRegexConstraint()
        {
            // Arrange
            var input = "[#1] International Trade";
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(input, pattern);

            // Assert
            Assert.IsTrue(constraint.Evaluate());
        }

        [TestMethod]
        public void TestDoesNotEvaluateRegexConstraint()
        {
            // Arrange
            var input = "International Trade";
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(input, pattern);

            // Assert
            Assert.IsFalse(constraint.Evaluate());
        }

        [TestMethod]
        public void TestMatchContentFromRegexConstraint()
        {
            // Arrange
            var input = "[#1] International Trade";
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(input, pattern, ConstraintAction.Match);
            constraint.Evaluate();

            // Assert
            Assert.AreEqual("[#1] International Trade", constraint.Value);
        }

        [TestMethod]
        public void TestMatchAndExtractContentFromRegexConstraint()
        {
            // Arrange
            var input = "[#1] International Trade";
            var pattern = "[a-zA-Z]+";

            // Act
            var constraint = new RegexConstraint(input, pattern);
            constraint.Evaluate();

            // Assert
            var match = Regex.Match(input, pattern);
            Assert.IsTrue(match.Success);
            Assert.AreEqual("International", match.Value);
            var nextMatch = match.NextMatch();
            Assert.AreEqual("Trade", nextMatch.Value);
            Assert.AreEqual("International Trade", constraint.Value);
        }
    }
}

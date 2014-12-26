using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DEiXTo.Models.Tests
{
    [TestClass]
    public class RegexConstraintTests
    {
        [TestMethod]
        public void TestCreateNewRegexConstraint()
        {
            // Arrange
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(pattern, NodeState.Grayed);

            // Assert
            Assert.AreEqual(pattern, constraint.Pattern);
            Assert.AreEqual(NodeState.Grayed, constraint.State);
        }

        [TestMethod]
        public void TestEvaluateRegexConstraint()
        {
            // Arrange
            var input = "[#1] International Trade";
            var pattern = @"\d+";

            // Act
            var constraint = new RegexConstraint(pattern, NodeState.Grayed);

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
            var constraint = new RegexConstraint(pattern, NodeState.Grayed);

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
            var constraint = new RegexConstraint(pattern, NodeState.Grayed);
            constraint.Evaluate(input);

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
            var constraint = new RegexConstraint(pattern, NodeState.Checked);
            constraint.Evaluate(input);

            // Assert
            Assert.AreEqual("International Trade", constraint.Value);
        }
    }
}

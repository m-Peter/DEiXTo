﻿using DEiXTo.Models;
using DEiXTo.Presenters;
using DEiXTo.Services;
using NUnit.Framework;
using System.Windows.Forms;
using Moq;
using System.Drawing;

namespace DEiXTo.Views.Tests
{
    [TestFixture]
    public class RegexBuilderWindowTests
    {
        private RegexBuilderWindow window;
        private RegexBuilderPresenter presenter;
        private TreeNode node;
        private Mock<IEventHub> eventHub;

        [Test]
        public void TestStartingState()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Assert
            Assert.AreEqual("$11.5", window.AddRegexTextBox.Text);
            Assert.IsFalse(window.InverseEvaluationCheckBox.Checked);
        }

        [Test]
        public void TestGetAndSetRegexFields()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);
            // Act
            window.AddRegexTextBox.Text = "[0-9]{2}";
            window.InverseEvaluationCheckBox.Checked = true;
            // Assert
            Assert.AreEqual("[0-9]{2}", window.RegexText);
            Assert.IsTrue(window.InverseRegex);

            // Reset
            window.AddRegexTextBox.Text = string.Empty;
            window.InverseEvaluationCheckBox.Checked = false;

            // Act
            window.RegexText = "[a-z]?";
            window.InverseRegex = true;
            // Assert
            Assert.AreEqual("[a-z]?", window.AddRegexTextBox.Text);
            Assert.IsTrue(window.InverseEvaluationCheckBox.Checked);
        }

        [Test]
        public void TestAddRegex()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);
            window.AddRegexTextBox.Text = "[0-9]{2}";

            // Act
            window.AddRegexTextBox.Text = "[0-9]{2}";
            presenter.AddRegex();

            // Assert
            var constraint = node.GetRegexConstraint();
            Assert.AreEqual("[0-9]{2}", constraint.Pattern);
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(sub => sub.Node == node)));
            Assert.AreEqual(FontStyle.Underline, node.NodeFont.Style);
        }

        [Test]
        public void TestAddRegexInBoldTreeNode()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);
            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
            var boldFont = new Font(font, FontStyle.Bold);
            node.NodeFont = boldFont;

            // Act
            window.AddRegexTextBox.Text = "[0-9]{2}";
            presenter.AddRegex();

            // Assert
            var constraint = node.GetRegexConstraint();
            Assert.AreEqual("[0-9]{2}", constraint.Pattern);
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(sub => sub.Node == node)));
            Assert.AreEqual(FontStyle.Bold | FontStyle.Underline, node.NodeFont.Style);
        }

        [Test]
        public void TestLoadExistingRegex()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            var constraint = new RegexConstraint("[0-9]{2}", NodeState.Grayed);
            node.SetRegexConstraint(constraint);
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Assert
            Assert.AreEqual("[0-9]{2}", window.RegexText);
        }

        [Test]
        public void TestChangeExistingRegex()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            var constraint = new RegexConstraint("[0-9]{2}", NodeState.Grayed);
            node.SetRegexConstraint(constraint);
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Act
            window.RegexText = "[a-z]?";
            presenter.AddRegex();

            // Assert
            var result = node.GetRegexConstraint();
            Assert.AreEqual("[a-z]?", result.Pattern);
        }
    }
}

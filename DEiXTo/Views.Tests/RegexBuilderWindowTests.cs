using DEiXTo.Models;
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

        [SetUp]
        public void SetUp()
        {
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
        }

        [Test]
        public void TestStartingState()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Assert
            Assert.AreEqual("$11.5", window.RegexTb.Text);
        }

        [Test]
        public void TestGetAndSetRegexFields()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Act
            window.RegexTb.Text = "[0-9]{2}";
            // Assert
            Assert.AreEqual("[0-9]{2}", window.RegexText);

            //Reset
            window.RegexTb.Text = string.Empty;

            // Act
            window.RegexText = "[a-z]?";
            // Assert
            Assert.AreEqual("[a-z]?", window.RegexTb.Text);
        }

        [Test]
        public void TestAddRegex()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Act
            window.RegexTb.Text = "[0-9]{2}";
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
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);
            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
            var boldFont = new Font(font, FontStyle.Bold);
            node.NodeFont = boldFont;

            // Act
            window.RegexTb.Text = "[0-9]{2}";
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

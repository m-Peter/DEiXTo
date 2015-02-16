using DEiXTo.Models;
using DEiXTo.Presenters;
using DEiXTo.Services;
using System.Windows.Forms;
using Moq;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEiXTo.Views.Tests
{
    [TestClass]
    public class RegexBuilderWindowTests
    {
        private RegexBuilderWindow window;
        private RegexBuilderPresenter presenter;
        private TreeNode node;
        private Mock<IEventHub> eventHub;

        [TestInitialize]
        public void SetUp()
        {
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
        }

        [TestMethod]
        public void TestStartingState()
        {
            // Arrange
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Assert
            Assert.AreEqual("$11.5", window.InputTb.Text);
        }

        [TestMethod]
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

        [TestMethod]
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
            //var constraint = node.GetRegexConstraint();
            Assert.IsInstanceOfType(node.GetConstraint(), typeof(RegexConstraint));
            //Assert.AreEqual("[0-9]{2}", constraint.Pattern);
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(sub => sub.Node == node)));
            Assert.AreEqual(FontStyle.Underline, node.NodeFont.Style);
        }

        [TestMethod]
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
            //var constraint = node.GetRegexConstraint();
            //Assert.AreEqual("[0-9]{2}", constraint.Pattern);
            Assert.IsInstanceOfType(node.GetConstraint(), typeof(RegexConstraint));
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(sub => sub.Node == node)));
            Assert.AreEqual(FontStyle.Bold | FontStyle.Underline, node.NodeFont.Style);
        }

        [TestMethod]
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

        [TestMethod]
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
            //var result = node.GetRegexConstraint();
            //Assert.AreEqual("[a-z]?", result.Pattern);
            Assert.IsInstanceOfType(node.GetConstraint(), typeof(RegexConstraint));
        }
    }
}

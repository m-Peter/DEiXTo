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
        public void Init()
        {
            node = new TreeNode("SPAN");
            node.Tag = new NodeInfo();
            node.SetContent("$11.5");
            eventHub = new Mock<IEventHub>();
            window = new RegexBuilderWindow();
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            window.Show();
        }

        [TearDown]
        public void Cleanup()
        {
            window.Close();
        }

        [Test]
        public void TestStartingState()
        {
            Assert.AreEqual("$11.5", window.AddRegexTextBox.Text);
            Assert.IsFalse(window.InverseEvaluationCheckBox.Checked);
        }

        [Test]
        public void TestGetAndSetRegexFields()
        {
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
            window.AddRegexTextBox.Text = "[0-9]{2}";

            // Act
            window.OKButton.PerformClick();

            // Assert
            Assert.AreEqual("[0-9]{2}", node.GetRegex());
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(sub => sub.Node == node)));
            Assert.AreEqual(FontStyle.Underline, node.NodeFont.Style);
        }

        [Test]
        public void TestAddRegexInBoldTreeNode()
        {
            // Arrange
            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
            var boldFont = new Font(font, FontStyle.Bold);
            node.NodeFont = boldFont;
            window.AddRegexTextBox.Text = "[0-9]{2}";

            // Act
            window.OKButton.PerformClick();

            // Assert
            Assert.AreEqual("[0-9]{2}", node.GetRegex());
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(sub => sub.Node == node)));
            Assert.AreEqual(FontStyle.Bold | FontStyle.Underline, node.NodeFont.Style);
        }

        [Test]
        public void TestLoadExistingRegex()
        {
            // Arrange
            node.SetRegex("[0-9]{2}");
            node.SetInverse(true);
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Act
            window.Show();

            // Assert
            Assert.AreEqual("[0-9]{2}", window.RegexText);
            Assert.IsTrue(window.InverseRegex);
        }

        [Test]
        public void TestChangeExistingRegex()
        {
            // Arrange
            node.SetRegex("[0-9]{2}");
            presenter = new RegexBuilderPresenter(window, node, eventHub.Object);

            // Act
            window.Show();
            window.RegexText = "[a-z]?";
            window.InverseRegex = true;
            window.OKButton.PerformClick();

            // Assert
            Assert.AreEqual("[a-z]?", node.GetRegex());
            Assert.IsTrue(node.InverseRegex());
        }
    }
}

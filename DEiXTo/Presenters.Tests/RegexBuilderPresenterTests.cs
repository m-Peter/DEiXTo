using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using System.Windows.Forms;
using DEiXTo.Services;
using DEiXTo.Models;
using System.Drawing;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class RegexBuilderPresenterTests
    {
        private Mock<IRegexBuilderView> _view;
        private TreeNode _node;
        private RegexBuilderPresenter _presenter;
        private Mock<IEventHub> _eventHub;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IRegexBuilderView>();
            _node = new TreeNode("H1");
            NodeInfo nInfo = new NodeInfo();
            nInfo.Content = "This is the header";
            _node.Tag = nInfo;
            _eventHub = new Mock<IEventHub>();
            _presenter = new RegexBuilderPresenter(_view.Object, _node, _eventHub.Object);
        }

        [TestMethod]
        public void TestRegexTextIsPopulatedWithNodesContent()
        {
            // Assert
            _view.VerifySet(m => m.RegexText = "This is the header");
        }

        [TestMethod]
        public void TestAddRegex()
        {
            // Arrange
            _view.Setup(m => m.RegexText).Returns("some regex");

            // Act
            _presenter.AddRegex();

            // Assert
            Assert.AreEqual("some regex", _node.GetRegex());
            _eventHub.Verify(m => m.Publish(It.IsAny<RegexAdded>()));
            var nodeFont = _node.NodeFont;
            Assert.AreEqual(FontStyle.Underline, nodeFont.Style);
            _view.Verify(m => m.Exit());
        }

        [TestMethod]
        public void TestAddInvalidRegex()
        {
            // Arrange
            _view.Setup(m => m.RegexText).Returns("");

            // Act
            _presenter.AddRegex();

            // Assert
            _view.Verify(m => m.ShowInvalidRegexMessage());
            _view.Verify(m => m.Exit(), Times.Never);
        }

        [TestMethod]
        public void TestPressingEnterAddsRegex()
        {
            // Arrange
            _view.Setup(m => m.RegexText).Returns("some regex");

            // Act
            _presenter.KeyDownPress(Keys.Enter);

            // Assert
            Assert.AreEqual("some regex", _node.GetRegex());
            _eventHub.Verify(m => m.Publish(It.IsAny<RegexAdded>()));
            var nodeFont = _node.NodeFont;
            Assert.AreEqual(FontStyle.Underline, nodeFont.Style);
            _view.Verify(m => m.Exit());
        }
    }
}

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
        private Mock<IRegexBuilderView> view;
        private TreeNode node;
        private RegexBuilderPresenter presenter;
        private Mock<IEventHub> eventHub;

        [TestInitialize]
        public void SetUp()
        {
            view = new Mock<IRegexBuilderView>();
            node = new TreeNode("H1");
            NodeInfo nInfo = new NodeInfo();
            nInfo.Content = "This is the header";
            node.Tag = nInfo;
            eventHub = new Mock<IEventHub>();
            presenter = new RegexBuilderPresenter(view.Object, node, eventHub.Object);
        }

        [TestMethod]
        public void TestRegexTextIsPopulatedWithNodeContent()
        {
            // Assert
            view.VerifySet(v => v.RegexText = "This is the header");
        }

        [TestMethod]
        public void TestAddRegex()
        {
            // Arrange
            view.Setup(v => v.RegexText).Returns("some regex");

            // Act
            presenter.AddRegex();

            // Assert
            Assert.AreEqual("some regex", node.GetRegex());
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(ra => ra.Node == node)));
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestAddInvalidRegex()
        {
            // Arrange
            view.Setup(v => v.RegexText).Returns("");

            // Act
            presenter.AddRegex();

            // Assert
            view.Verify(v => v.ShowInvalidRegexMessage());
            view.Verify(v => v.Exit(), Times.Never);
        }

        [TestMethod]
        public void TestPressingEnterAddsRegex()
        {
            // Arrange
            view.Setup(v => v.RegexText).Returns("some regex");

            // Act
            presenter.KeyDownPress(Keys.Enter);

            // Assert
            Assert.AreEqual("some regex", node.GetRegex());
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(ra => ra.Node == node )));
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestLoadExistingRegex()
        {
            // Arrange
            node.SetRegex("[0-9]{2}");
            node.SetInverse(true);

            // Act
            presenter = new RegexBuilderPresenter(view.Object, node, eventHub.Object);

            // Assert
            view.VerifySet(v => v.RegexText = "[0-9]{2}");
            view.VerifySet(v => v.InverseRegex = true);
        }

        [TestMethod]
        public void TestChangeExistingRegex()
        {
            // Arrange
            node.SetRegex("[a-z]{4}");
            node.SetInverse(false);

            // Act
            presenter = new RegexBuilderPresenter(view.Object, node, eventHub.Object);
            view.Setup(v => v.RegexText).Returns("[0-9]{2}");
            view.Setup(v => v.InverseRegex).Returns(true);
            presenter.AddRegex();

            // Assert
            Assert.AreEqual("[0-9]{2}", node.GetRegex());
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(ra => ra.Node == node)));
            view.Verify(v => v.Exit());
        }
    }
}

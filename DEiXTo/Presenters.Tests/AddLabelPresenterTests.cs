using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using DEiXTo.Models;
using System.Windows.Forms;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class AddLabelPresenterTests
    {
        private Mock<IAddLabelView> view;
        private AddLabelPresenter presenter;
        private TreeNode node;

        [TestInitialize]
        public void SetUp()
        {
            view = new Mock<IAddLabelView>();
            node = new TreeNode("H1");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;
            presenter = new AddLabelPresenter(view.Object, node);
        }

        [TestMethod]
        public void TestAddLabel()
        {
            // Arrange
            view.Setup(v => v.LabelText).Returns("HEADER");

            // Act
            presenter.AddLabel();

            // Assert
            Assert.AreEqual("H1:HEADER", node.Text);
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestLoadNodeWithLabel()
        {
            // Arrange
            node.SetLabel("HEAD");

            // Act
            presenter = new AddLabelPresenter(view.Object, node);

            // Assert
            view.VerifySet(v => v.LabelText = "HEAD");
        }

        [TestMethod]
        public void TestReplaceExistingLabelWithNew()
        {
            // Arrange
            node.SetLabel("HEAD");
            presenter = new AddLabelPresenter(view.Object, node);

            // Act
            view.Setup(v => v.LabelText).Returns("HEADER");
            presenter.AddLabel();

            // Assert
            Assert.AreEqual("H1:HEADER", node.Text);
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestInvalidLabelDoesNotGetAdded()
        {
            // Arrange
            view.Setup(v => v.LabelText).Returns("");

            // Act
            presenter.AddLabel();

            // Assert
            Assert.AreEqual("H1", node.Text);
            view.Verify(v => v.ShowInvalidLabelMessage());
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestPressingEnterAddsLabel()
        {
            // Arrange
            view.Setup(v => v.LabelText).Returns("HEADER");

            // Act
            presenter.KeyDownPress(Keys.Enter);

            // Assert
            Assert.AreEqual("H1:HEADER", node.Text);
            view.Verify(v => v.Exit());
        }
    }
}

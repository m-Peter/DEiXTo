using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using System.Windows.Forms;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class AddLabelPresenterTests
    {
        private Mock<IAddLabelView> _view;
        private AddLabelPresenter _presenter;
        private TreeNode _node;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IAddLabelView>();
            _node = new TreeNode("H1");
            _presenter = new AddLabelPresenter(_view.Object, _node);
        }

        [TestMethod]
        public void TestAddLabel()
        {
            // Arrange
            _view.Setup(m => m.LabelText).Returns("HEADER");

            // Act
            _presenter.AddLabel();

            // Assert
            Assert.AreEqual("H1:HEADER", _node.Text);
        }

        [TestMethod]
        public void TestWindowClosesAfterLabelInsertion()
        {
            // Arrange
            _view.Setup(m => m.LabelText).Returns("HEADER");

            // Act
            _presenter.AddLabel();

            // Assert
            Assert.AreEqual("H1:HEADER", _node.Text);
            _view.Verify(m => m.Exit());
        }

        [TestMethod]
        public void TestAddInvalidLabel()
        {
            // Arrange
            _view.Setup(m => m.LabelText).Returns("");

            // Act
            _presenter.AddLabel();

            // Assert
            _view.Verify(m => m.ShowInvalidLabelMessage());
            _view.Verify(m => m.Exit());
        }

        [TestMethod]
        public void TestPressingEnterAddsLabel()
        {
            // Arrange
            _view.Setup(m => m.LabelText).Returns("HEADER");

            // Act
            _presenter.KeyDownPress(Keys.Enter);

            // Assert
            Assert.AreEqual("H1:HEADER", _node.Text);
        }
    }
}

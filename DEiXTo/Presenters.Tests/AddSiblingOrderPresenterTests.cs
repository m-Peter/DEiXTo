using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using System.Windows.Forms;
using System.Drawing;
using DEiXTo.Models;
using DEiXTo.Services;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class AddSiblingOrderPresenterTests
    {
        private Mock<IAddSiblingOrderView> view;
        private TreeNode node;
        private AddSiblingOrderPresenter presenter;

        [TestInitialize]
        public void SetUp()
        {
            view = new Mock<IAddSiblingOrderView>();
            node = new TreeNode("H1");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;
            presenter = new AddSiblingOrderPresenter(view.Object, node);
        }

        [TestMethod]
        public void TestAddSiblingOrder()
        {
            // Arrange
            view.Setup(v => v.CareAboutSiblingOrder).Returns(true);
            view.Setup(v => v.StartIndex).Returns(0);
            view.Setup(v => v.StepValue).Returns(2);

            // Act
            presenter.AddSiblingOrder();

            // Assert
            Assert.IsTrue(node.GetCareAboutSiblingOrder());
            Assert.AreEqual(0, node.GetStartIndex());
            Assert.AreEqual(2, node.GetStepValue());
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestLoadExistingSiblingOrder()
        {
            // Arrange
            node.SetCareAboutSiblingOrder(true);
            node.SetStartIndex(1);
            node.SetStepValue(3);

            // Act
            presenter = new AddSiblingOrderPresenter(view.Object, node);

            // Assert
            view.VerifySet(v => v.CareAboutSiblingOrder = true);
            view.VerifySet(v => v.StartIndex = 1);
            view.VerifySet(v => v.StepValue = 3);
        }

        [TestMethod]
        public void TestChangeExistingSiblingOrder()
        {
            // Arrange
            node.SetCareAboutSiblingOrder(true);
            node.SetStartIndex(1);
            node.SetStepValue(3);

            // Act
            presenter = new AddSiblingOrderPresenter(view.Object, node);
            view.Setup(v => v.CareAboutSiblingOrder).Returns(true);
            view.Setup(v => v.StartIndex).Returns(2);
            view.Setup(v => v.StepValue).Returns(4);
            presenter.AddSiblingOrder();

            // Assert
            Assert.IsTrue(node.GetCareAboutSiblingOrder());
            Assert.AreEqual(2, node.GetStartIndex());
            Assert.AreEqual(4, node.GetStepValue());
        }

        [TestMethod]
        public void TestRemoveExistingSiblingOrder()
        {
            // Arrange
            node.SetCareAboutSiblingOrder(true);
            node.SetStartIndex(1);
            node.SetStepValue(3);

            // Act
            presenter = new AddSiblingOrderPresenter(view.Object, node);
            view.Setup(v => v.CareAboutSiblingOrder).Returns(false);
            presenter.AddSiblingOrder();

            // Assert
            Assert.IsFalse(node.GetCareAboutSiblingOrder());
            Assert.AreEqual(0, node.GetStartIndex());
            Assert.AreEqual(0, node.GetStepValue());
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestSiblingOrderFieldsAreEnabledUponCheckBoxChecking()
        {
            // Act
            presenter.ChangeSiblingOrderVisibility(true);

            // Assert
            view.Verify(v => v.EnableSiblingOrderFields());
        }

        [TestMethod]
        public void TestSiblingOrderFieldsAreDisabledUponCheckBoxUnchecking()
        {
            // Act
            presenter.ChangeSiblingOrderVisibility(false);

            // Assert
            view.Verify(v => v.DisableSiblingOrderFields());
        }
    }
}

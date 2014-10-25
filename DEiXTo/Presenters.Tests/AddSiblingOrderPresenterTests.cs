using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Mock<IAddSiblingOrderView> _view;
        private TreeNode _node;
        private AddSiblingOrderPresenter _presenter;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IAddSiblingOrderView>();
            _node = new TreeNode("H1");
            NodeInfo nInfo = new NodeInfo();
            _node.Tag = nInfo;
            _presenter = new AddSiblingOrderPresenter(_view.Object, _node);
        }

        [TestMethod]
        public void TestAddSiblingOrder()
        {
            // Arrange
            _view.Setup(m => m.CareAboutSiblingOrder).Returns(true);
            _view.Setup(m => m.GetStartIndex).Returns(0);
            _view.Setup(m => m.GetStepValue).Returns(2);

            // Act
            _presenter.AddSiblingOrder();

            // Assert
            Assert.IsTrue(_node.GetCareAboutSiblingOrder());
            Assert.AreEqual(2, _node.GetStepValue());
            Assert.AreEqual(Color.CadetBlue, _node.ForeColor);
            _view.Verify(m => m.Exit());
        }

        [TestMethod]
        public void TestReturnsWhenCheckBoxIsDisabled()
        {
            // Arrange
            _view.Setup(m => m.CareAboutSiblingOrder).Returns(false);

            // Act
            _presenter.AddSiblingOrder();

            // Assert
            _view.Verify(m => m.Exit());
        }

        [TestMethod]
        public void TestSiblingOrderFieldsAreEnabledUponCheckBoxChecking()
        {
            // Act
            _presenter.ChangeSiblingOrderVisibility(true);

            // Assert
            _view.Verify(m => m.ApplyVisibilityStateInOrdering(true));
        }

        [TestMethod]
        public void TestSiblingOrderFieldsAreDisabledUponCheckBoxUnchecking()
        {
            // Act
            _presenter.ChangeSiblingOrderVisibility(false);

            // Assert
            _view.Verify(m => m.ApplyVisibilityStateInOrdering(false));
        }
    }
}

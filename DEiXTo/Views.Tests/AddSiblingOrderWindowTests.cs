using DEiXTo.Models;
using DEiXTo.Presenters;
using NUnit.Framework;
using System.Windows.Forms;
using DEiXTo.Services;

namespace DEiXTo.Views.Tests
{
    [TestFixture]
    public class AddSiblingOrderWindowTests
    {
        private AddSiblingOrderWindow window;
        private AddSiblingOrderPresenter presenter;

        [SetUp]
        public void Init()
        {
            window = new AddSiblingOrderWindow();
        }

        [TearDown]
        public void Cleanup()
        {
            window.Close();
        }

        [Test]
        public void TestStartingState()
        {
            // Arrange
            presenter = new AddSiblingOrderPresenter(window, new TreeNode());
            presenter.View = window;

            // Act
            window.Show();

            // Assert
            Assert.IsFalse(window.SiblingOrderCheckBox.Checked);
            Assert.IsFalse(window.StartIndexNUD.Enabled);
            Assert.IsFalse(window.StepValueNUD.Enabled);
        }

        [Test]
        public void TestCheckSiblingOrderCheckBox()
        {
            // Arrange
            presenter = new AddSiblingOrderPresenter(window, new TreeNode());
            presenter.View = window;

            // Act
            window.Show();
            window.SiblingOrderCheckBox.Checked = true;

            // Assert
            Assert.IsTrue(window.SiblingOrderCheckBox.Checked);
            Assert.IsTrue(window.StartIndexNUD.Enabled);
            Assert.IsTrue(window.StepValueNUD.Enabled);
        }

        [Test]
        public void TestAssignSiblingOrderFields()
        {
            // Arrange
            presenter = new AddSiblingOrderPresenter(window, new TreeNode());
            presenter.View = window;

            // Act
            window.Show();
            window.CareAboutSiblingOrder = true;
            window.StartIndex = 2;
            window.StepValue = 4;

            // Assert
            Assert.IsTrue(window.SiblingOrderCheckBox.Checked);
            Assert.AreEqual(2, window.StartIndexNUD.Value);
            Assert.AreEqual(4, window.StepValueNUD.Value);
        }

        [Test]
        public void TestAddSiblingOrderToNode()
        {
            // Arrange
            var node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;
            presenter = new AddSiblingOrderPresenter(window, node);
            presenter.View = window;

            // Act
            window.Show();
            window.CareAboutSiblingOrder = true;
            window.StartIndex = 1;
            window.StepValue = 3;
            window.OKButton.PerformClick();

            // Assert
            Assert.IsTrue(node.GetCareAboutSiblingOrder());
            Assert.AreEqual(1, node.GetStartIndex());
            Assert.AreEqual(3, node.GetStepValue());
        }

        [Test]
        public void TestLoadExistingSiblingOrderFromNode()
        {
            // Arrange
            var node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            nInfo.CareAboutSiblingOrder = true;
            nInfo.SiblingOrderStart = 1;
            nInfo.SiblingOrderStep = 3;
            node.Tag = nInfo;
            presenter = new AddSiblingOrderPresenter(window, node);
            presenter.View = window;

            // Act
            window.Show();

            // Assert
            Assert.IsTrue(window.SiblingOrderCheckBox.Checked);
            Assert.AreEqual(1, window.StartIndexNUD.Value);
            Assert.AreEqual(3, window.StepValueNUD.Value);
        }
    }
}

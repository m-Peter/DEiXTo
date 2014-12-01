using DEiXTo.Models;
using DEiXTo.Presenters;
using NUnit.Framework;
using System.Windows.Forms;
using DEiXTo.Services;
using System.Drawing;

namespace DEiXTo.Views.Tests
{
    [TestFixture]
    public class AddSiblingOrderWindowTests
    {
        private AddSiblingOrderWindow window;
        private AddSiblingOrderPresenter presenter;
        private TreeNode node;

        [Test]
        public void TestStartingState()
        {
            // Arrange
            node = new TreeNode("DIV");
            node.Tag = new NodeInfo();
            window = new AddSiblingOrderWindow();
            presenter = new AddSiblingOrderPresenter(window, node);

            // Act

            // Assert
            Assert.IsFalse(window.SiblingOrderCheckBox.Checked);
            Assert.IsFalse(window.StartIndexNUD.Enabled);
            Assert.IsFalse(window.StepValueNUD.Enabled);
        }

        [Test]
        public void TestGetAndSetSiblingOrderFields()
        {
            // Arrange
            node = new TreeNode("DIV");
            node.Tag = new NodeInfo();
            window = new AddSiblingOrderWindow();
            presenter = new AddSiblingOrderPresenter(window, node);

            // Act
            window.SiblingOrderCheckBox.Checked = true;
            window.StartIndexNUD.Value = 1;
            window.StepValueNUD.Value = 3;
            // Assert
            Assert.IsTrue(window.CareAboutSiblingOrder);
            Assert.AreEqual(1, window.StartIndex);
            Assert.AreEqual(3, window.StepValue);

            // Reset
            window.SiblingOrderCheckBox.Checked = false;

            // Act
            window.CareAboutSiblingOrder = true;
            window.StartIndex = 2;
            window.StepValue = 4;
            // Assert
            Assert.IsTrue(window.SiblingOrderCheckBox.Checked);
            Assert.AreEqual(2, window.StartIndexNUD.Value);
            Assert.AreEqual(4, window.StepValueNUD.Value);
        }

        [Test]
        public void TestEnableSiblingOrderFields()
        {
            // Arrange
            node = new TreeNode("DIV");
            node.Tag = new NodeInfo();
            window = new AddSiblingOrderWindow();
            presenter = new AddSiblingOrderPresenter(window, node);

            // Act
            window.SiblingOrderCheckBox.Checked = true;
            
            // Assert
            Assert.IsTrue(window.StartIndexNUD.Enabled);
            Assert.IsTrue(window.StepValueNUD.Enabled);
        }

        [Test]
        public void TestDisableSiblingOrderFields()
        {
            // Arrange
            node = new TreeNode("DIV");
            node.Tag = new NodeInfo();
            window = new AddSiblingOrderWindow();
            presenter = new AddSiblingOrderPresenter(window, node);
            window.SiblingOrderCheckBox.Checked = true;
            window.StartIndexNUD.Value = 1;
            window.StepValueNUD.Value = 3;

            // Act
            window.SiblingOrderCheckBox.Checked = false;

            // Assert
            Assert.IsFalse(window.StartIndexNUD.Enabled);
            Assert.AreEqual(0, window.StartIndexNUD.Value);
            Assert.IsFalse(window.StepValueNUD.Enabled);
            Assert.AreEqual(0, window.StepValueNUD.Value);
        }

        [Test]
        public void TestAddSiblingOrderToNode()
        {
            // Arrange
            node = new TreeNode("DIV");
            node.Tag = new NodeInfo();
            window = new AddSiblingOrderWindow();
            presenter = new AddSiblingOrderPresenter(window, node);
            window.CareAboutSiblingOrder = true;
            window.StartIndex = 1;
            window.StepValue = 3;

            // Act
            window.Show();
            window.OKButton.PerformClick();

            // Assert
            Assert.IsTrue(node.GetCareAboutSiblingOrder());
            Assert.AreEqual(1, node.GetStartIndex());
            Assert.AreEqual(3, node.GetStepValue());
            Assert.AreEqual(Color.CadetBlue, node.ForeColor);
        }

        [Test]
        public void TestLoadExistingSiblingOrderFromNode()
        {
            // Arrange
            node = new TreeNode("DIV");
            node.Tag = new NodeInfo();
            window = new AddSiblingOrderWindow();
            presenter = new AddSiblingOrderPresenter(window, node);
            node.SetCareAboutSiblingOrder(true);
            node.SetStartIndex(1);
            node.SetStepValue(3);
            presenter = new AddSiblingOrderPresenter(window, node);

            // Act

            // Assert
            Assert.IsTrue(window.CareAboutSiblingOrder);
            Assert.AreEqual(1, window.StartIndex);
            Assert.AreEqual(3, window.StepValue);
        }

        [Test]
        public void TestChangeExistingSiblingOrder()
        {
            // Arrange
            node = new TreeNode("DIV");
            node.Tag = new NodeInfo();
            window = new AddSiblingOrderWindow();
            presenter = new AddSiblingOrderPresenter(window, node);
            node.SetCareAboutSiblingOrder(true);
            node.SetStartIndex(1);
            node.SetStepValue(3);
            presenter = new AddSiblingOrderPresenter(window, node);

            // Act
            window.Show();
            window.StartIndex = 2;
            window.StepValue = 4;
            window.OKButton.PerformClick();

            // Assert
            Assert.IsTrue(node.GetCareAboutSiblingOrder());
            Assert.AreEqual(2, node.GetStartIndex());
            Assert.AreEqual(4, node.GetStepValue());
        }

        [Test]
        public void TestRemoveExistingSiblingOrder()
        {
            // Arrange
            node = new TreeNode("DIV");
            node.Tag = new NodeInfo();
            window = new AddSiblingOrderWindow();
            presenter = new AddSiblingOrderPresenter(window, node);
            node.SetCareAboutSiblingOrder(true);
            node.SetStartIndex(1);
            node.SetStepValue(3);
            presenter = new AddSiblingOrderPresenter(window, node);

            // Act
            window.Show();
            window.CareAboutSiblingOrder = false;
            window.OKButton.PerformClick();

            // Assert
            Assert.IsFalse(node.GetCareAboutSiblingOrder());
            Assert.AreEqual(0, node.GetStartIndex());
            Assert.AreEqual(0, node.GetStepValue());
            Assert.AreEqual(Color.Black, node.ForeColor);
        }
    }
}

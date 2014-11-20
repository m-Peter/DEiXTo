using DEiXTo.Models;
using DEiXTo.Presenters;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Extensions.Forms;
using System.Windows.Forms;
using NUnit;
using NUnit.Framework;

namespace DEiXTo.Views.Tests
{
    [TestFixture]
    public class AddLabelWindowTests : NUnitFormTest
    {
        private AddLabelWindow window;
        private AddLabelPresenter presenter;

        [SetUp]
        public void SetUp()
        {
            window = new AddLabelWindow();
        }

        [TearDown]
        public override void TearDown()
        {
            window.Close();
        }

        public override bool UseHidden
        {
            get
            {
                return false;
            }
        }

        [Test]
        public void TestStartingState()
        {
            // Arrange
            presenter = new AddLabelPresenter(window, new TreeNode());
            presenter.View = window;
            
            // Act
            window.Show();

            // Assert
            Assert.AreSame(presenter, window.Presenter);
            Assert.AreEqual(string.Empty, window.AddLabelTextBox.Text);
        }

        [Test]
        public void TestGetAndSetLabelText()
        {
            // Arrange
            presenter = new AddLabelPresenter(window, new TreeNode());
            presenter.View = window;

            // Act
            window.Show();
            window.LabelText = "Container";

            // Assert
            Assert.AreEqual("Container", window.AddLabelTextBox.Text);
        }

        [Test]
        public void TestAddLabelToNode()
        {
            // Arrange
            var node = new TreeNode("DIV");
            presenter = new AddLabelPresenter(window, node);
            presenter.View = window;

            // Act
            window.Show();
            window.AddLabelTextBox.Text = "Container";
            window.OkButton.PerformClick();

            // Assert
            Assert.AreEqual("DIV:Container", node.Text);
        }

        [Test]
        public void TestAddInvalidLabelToNode()
        {
            // Arrange
            AddLabelWindow form = new AddLabelWindow();
            var node = new TreeNode("DIV");
            presenter = new AddLabelPresenter(form, node);
            presenter.View = form;

            // Act
            base.ExpectModal("DEiXTo", MessageBoxTestHandler);
            form.Show();
            form.AddLabelTextBox.Text = string.Empty;
            
            form.OkButton.PerformClick();

            // Assert
            Assert.AreEqual("DIV", node.Text);
        }

        public void MessageBoxTestHandler()
        {
            MessageBoxTester messageBox = new MessageBoxTester("DEiXTo");
            Assert.AreEqual("DEiXTo", messageBox.Title);
            Assert.AreEqual("Invalid empty label!", messageBox.Text);
            messageBox.ClickOk();
        }

        [Test]
        public void TestLoadExistingLabelFromNode()
        {
            // Arrange
            var node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            nInfo.Label = "Container";
            node.Tag = nInfo;
            presenter = new AddLabelPresenter(window, node);
            presenter.View = window;

            // Act
            window.Show();

            // Assert
            Assert.AreEqual("Container", window.AddLabelTextBox.Text);
        }
    }
}

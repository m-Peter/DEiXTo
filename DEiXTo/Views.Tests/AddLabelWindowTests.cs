using DEiXTo.Models;
using DEiXTo.Presenters;
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
        private TreeNode node;

        [SetUp]
        public void Init()
        {
            node = new TreeNode("TEXT");
            node.Tag = new NodeInfo();
            window = new AddLabelWindow();
            presenter = new AddLabelPresenter(window, node);

            window.Show();
        }

        [TearDown]
        public void Cleanup()
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
            Assert.AreSame(presenter, window.Presenter);
            Assert.AreEqual(string.Empty, window.AddLabelTextBox.Text);
        }

        [Test]
        public void TestGetAndSetLabelText()
        {
            // Act
            window.AddLabelTextBox.Text = "Container";
            // Assert
            Assert.AreEqual("Container", window.LabelText);

            // Act
            window.LabelText = "Content";
            // Assert
            Assert.AreEqual("Content", window.AddLabelTextBox.Text);
        }

        [Test]
        public void TestAddLabelToNode()
        {
            // Arrange
            window.AddLabelTextBox.Text = "Container";
            
            // Act
            window.OkButton.PerformClick();

            // Assert
            Assert.AreEqual("TEXT:Container", node.Text);
            Assert.AreEqual("Container", node.GetLabel());
        }

        [Test]
        public void TestAddInvalidLabelToNode()
        {
            // Arrange
            window.AddLabelTextBox.Text = string.Empty;

            // Act
            ExpectModal("DEiXTo", MessageBoxTestHandler);
            window.OkButton.PerformClick();

            // Assert
            Assert.AreEqual("TEXT", node.Text);
            Assert.Null(node.GetLabel());
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
            node.SetLabel("Container");
            presenter = new AddLabelPresenter(window, node);

            // Act
            window.Show();

            // Assert
            Assert.AreEqual("Container", window.AddLabelTextBox.Text);
        }

        [Test]
        public void TestChangeExistingLabel()
        {
            // Arrange
            node.SetLabel("Container");
            node.Text = "TEXT:Container";
            presenter = new AddLabelPresenter(window, node);

            // Act
            window.Show();
            window.AddLabelTextBox.Text = "Content";
            window.OkButton.PerformClick();

            // Assert
            Assert.AreEqual("TEXT:Content", node.Text);
            Assert.AreEqual("Content", node.GetLabel());
        }
    }
}

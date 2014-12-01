using DEiXTo.Presenters;
using DEiXTo.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Moq;

namespace DEiXTo.Views.Tests
{
    [TestClass]
    public class MainWindowTests
    {
        private MainWindow window;
        private MainPresenter presenter;
        private IViewLoader loader;
        private Mock<IEventHub> eventHub;

        [TestInitialize]
        public void SetUp()
        {
            window = new MainWindow();
            eventHub = new Mock<IEventHub>();
            loader = new WindowsViewLoader();
            presenter = new MainPresenter(window, loader, eventHub.Object);
        }

        [TestMethod]
        public void TestStartingState()
        {
            // Assert
            Assert.IsTrue(window.HasChildren);
        }

        [TestMethod]
        public void TestCreateNewAgent()
        {
            // Act
            presenter.CreateNewAgent();

            // Assert
            Assert.AreEqual(1, window.MdiChildren.Length);
            var child = window.MdiChildren[0];
            Assert.AreEqual("Agent 1", child.Text);
        }

        [TestMethod]
        public void TestCascadeAgents()
        {
            // Act
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();
            window.CascadeAgents();

            // Assert
            var childs = window.MdiChildren;
            foreach (Form form in childs)
            {
                Assert.AreEqual(FormWindowState.Normal, form.WindowState);
            }
        }

        [TestMethod]
        public void TestFloatAgents()
        {
            // Act
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();
            window.FloatAgents();

            // Assert
            var childs = window.MdiChildren;
            foreach (Form form in childs)
            {
                Assert.AreEqual(FormWindowState.Maximized, form.WindowState);
            }
        }

        [TestMethod]
        public void TestCloseAgents()
        {
            // Act
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();
            window.CloseAgents();

            // Assert
            Assert.AreEqual(0, window.MdiChildren.Length);
        }

        [TestMethod]
        public void TestCloseAgent()
        {
            // Act
            presenter.CreateNewAgent();

            // Assert
            Assert.AreEqual(1, presenter.FormCounter);
            presenter.Receive(new DeixtoAgentClosed());
            Assert.AreEqual(0, presenter.FormCounter);
        }
    }
}

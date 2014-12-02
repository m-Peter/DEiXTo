using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEiXTo.Views;
using DEiXTo.Presenters;
using DEiXTo.Services;
using System.Windows.Forms;

namespace DEiXTo.IntegrationTests
{
    [TestClass]
    public class MainWindowIntegrationTests
    {
        private MainWindow window;
        private IViewLoader loader;
        private IEventHub eventHub;
        private IBrowserVersionManager browserManager;
        private MainWindowPresenter presenter;

        [TestInitialize]
        public void SetUp()
        {
            window = new MainWindow();
            loader = new WindowsViewLoader();
            eventHub = EventHub.Instance;
            browserManager = new BrowserVersionManager();
            presenter = new MainWindowPresenter(window, loader, eventHub, browserManager);
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
            // Arrange
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Act
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
            // Arrange
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Act
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
            // Arrange
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Act
            window.CloseAgents();

            // Assert
            Assert.AreEqual(0, window.MdiChildren.Length);
        }

        [TestMethod]
        public void TestCloseAgent()
        {
            // Arrange
            presenter.CreateNewAgent();

            // Act
            presenter.Receive(new DeixtoAgentClosed());

            // Assert
            Assert.AreEqual(0, presenter.FormCounter);
        }
    }
}

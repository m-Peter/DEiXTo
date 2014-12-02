using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using DEiXTo.Services;
using System.Windows.Forms;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class MainWindowPresenterTests
    {
        private Mock<IMainView> view;
        private Mock<IViewLoader> loader;
        private Mock<IEventHub> eventHub;
        private MainWindowPresenter presenter;

        [TestInitialize]
        public void SetUp()
        {
            view = new Mock<IMainView>();
            loader = new Mock<IViewLoader>();
            eventHub = new Mock<IEventHub>();
            presenter = new MainWindowPresenter(view.Object, loader.Object, eventHub.Object);
        }

        [TestMethod]
        public void TestCreateNewAgent()
        {
            // Act
            presenter.CreateNewAgent();
            
            // Assert
            Assert.AreEqual(1, presenter.FormCounter);
            loader.Verify(l => l.LoadAgentView("Agent 1", view.Object));
        }

        [TestMethod]
        public void TestCreategManyAgents()
        {
            // Act
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Assert
            Assert.AreEqual(2, presenter.FormCounter);
            loader.Verify(l => l.LoadAgentView("Agent 1", view.Object));
            loader.Verify(l => l.LoadAgentView("Agent 2", view.Object));
        }

        [TestMethod]
        public void TestFormCounterIsResetWhenAllAgentWindowsAreClosed()
        {
            // Arrange
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Act
            presenter.CloseAgentWindows();

            // Assert
            Assert.AreEqual(0, presenter.FormCounter);
        }

        [TestMethod]
        public void TestFormCounterIsReducedByOneWhenSingleAgentIsClose()
        {
            // Arrange
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Act
            presenter.Receive(new DeixtoAgentClosed());

            // Assert
            Assert.AreEqual(1, presenter.FormCounter);
        }

        [TestMethod]
        public void TestCascadeAgents()
        {
            // Arrange
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Act
            presenter.CascadeAgentWindows();

            // Assert
            view.Verify(v => v.CascadeAgents());
        }

        [TestMethod]
        public void TestCloseAgents()
        {
            // Arrange
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Act
            presenter.CloseAgentWindows();

            // Assert
            view.Verify(v => v.CloseAgents());
        }

        [TestMethod]
        public void TestFloatAgents()
        {
            // Arrange
            presenter.CreateNewAgent();
            presenter.CreateNewAgent();

            // Act
            presenter.FloatAgentWindows();

            // Assert
            view.Verify(v => v.FloatAgents());
        }

        [TestMethod]
        public void TestCloseMainWindowPromptsUser()
        {
            // Arrange
            var args = new FormClosingEventArgs(CloseReason.UserClosing, true);
            
            // Act
            presenter.WindowClosing(args);

            // Assert
            view.Verify(v => v.AskUserToConfirmClosing());
        }

        [TestMethod]
        public void TestMainWindowRemainsOpenWithNegativeResponse()
        {
            // Arrange
            var args = new FormClosingEventArgs(CloseReason.UserClosing, false);
            view.Setup(v => v.AskUserToConfirmClosing()).Returns(false);
            
            // Act
            presenter.WindowClosing(args);

            // Assert
            view.Verify(v => v.AskUserToConfirmClosing());
            Assert.IsTrue(args.Cancel);
        }

        [TestMethod]
        public void TestMainWindowClosesWithPositiveResponse()
        {
            // Arrange
            var args = new FormClosingEventArgs(CloseReason.UserClosing, false);
            view.Setup(v => v.AskUserToConfirmClosing()).Returns(true);

            // Act
            presenter.WindowClosing(args);

            // Assert
            view.Verify(v => v.AskUserToConfirmClosing());
            Assert.IsFalse(args.Cancel);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using DEiXTo.Services;
using System.Windows.Forms;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class MainPresenterTests
    {
        private Mock<IMainView> _view;
        private Mock<IViewLoader> _loader;
        private Mock<IEventHub> _eventHub;
        private MainPresenter _presenter;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IMainView>();
            _loader = new Mock<IViewLoader>();
            _eventHub = new Mock<IEventHub>();
            _presenter = new MainPresenter(_view.Object, _loader.Object, _eventHub.Object);
        }

        [TestMethod]
        public void TestCreateNewAgent()
        {
            // Act
            _presenter.CreateNewAgent();
            
            // Assert
            _loader.Verify(m => m.LoadAgentView("Agent 1", _view.Object));
        }

        [TestMethod]
        public void TestCreatingNewAgentsIncreasesFormCounter()
        {
            // Act
            _presenter.CreateNewAgent();
            _presenter.CreateNewAgent();

            // Assert
            _loader.Verify(m => m.LoadAgentView("Agent 2", _view.Object));
            Assert.AreEqual(2, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestFormCounterIsResetWhenAllAgentWindowsAreClosed()
        {
            // Act
            _presenter.CreateNewAgent();

            // Assert
            _loader.Verify(m => m.LoadAgentView("Agent 1", _view.Object));
            Assert.AreEqual(1, _presenter.FormCounter);

            // Act
            _presenter.CloseAgentWindows();

            // Assert
            Assert.AreEqual(0, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestFormCounterIsReducedByOneWhenSingleAgentIsClose()
        {
            // Act
            _presenter.CreateNewAgent();
            _presenter.CreateNewAgent();

            // Assert
            _loader.Verify(m => m.LoadAgentView("Agent 2", _view.Object));
            Assert.AreEqual(2, _presenter.FormCounter);

            // Act
            _presenter.Receive(null);

            // Assert
            Assert.AreEqual(1, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestCascadeAgents()
        {
            // Act
            _presenter.CascadeAgentWindows();

            // Assert
            _view.Verify(m => m.CascadeAgents());
        }

        [TestMethod]
        public void TestCloseAgents()
        {
            // Act
            _presenter.CloseAgentWindows();

            // Assert
            _view.Verify(m => m.CloseAgents());
        }

        [TestMethod]
        public void TestFloatAgents()
        {
            // Act
            _presenter.FloatAgentWindows();

            // Assert
            _view.Verify(m => m.FloatAgents());
        }

        [TestMethod]
        public void TestCloseMainWindowPromptsUser()
        {
            // Arrange
            var args = new FormClosingEventArgs(CloseReason.UserClosing, true);
            
            // Act
            _presenter.WindowClosing(args);

            // Assert
            _view.Verify(m => m.AskUserToConfirmClosing());
        }

        [TestMethod]
        public void TestMainWindowRemainsOpenWithNegativeResponse()
        {
            // Arrange
            var args = new FormClosingEventArgs(CloseReason.UserClosing, false);
            _view.Setup(m => m.AskUserToConfirmClosing()).Returns(false);
            
            // Act
            _presenter.WindowClosing(args);

            // Assert
            _view.Verify(m => m.AskUserToConfirmClosing());
            Assert.IsTrue(args.Cancel);
        }

        [TestMethod]
        public void TestMainWindowClosesWithPositiveResponse()
        {
            // Arrange
            var args = new FormClosingEventArgs(CloseReason.UserClosing, false);
            _view.Setup(m => m.AskUserToConfirmClosing()).Returns(true);

            // Act
            _presenter.WindowClosing(args);

            // Assert
            _view.Verify(m => m.AskUserToConfirmClosing());
            Assert.IsFalse(args.Cancel);
        }
    }
}

using System;
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
            _presenter.CreateNewAgent();

            _loader.Verify(m => m.LoadAgentView("Agent 1", _view.Object));
        }

        [TestMethod]
        public void TestCreatingNewAgentsIncreasesFormCounter()
        {
            _presenter.CreateNewAgent();
            _presenter.CreateNewAgent();

            _loader.Verify(m => m.LoadAgentView("Agent 2", _view.Object));
            Assert.AreEqual(2, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestFormCounterIsResetWhenAllAgentWindowsAreClosed()
        {
            _presenter.CreateNewAgent();

            _loader.Verify(m => m.LoadAgentView("Agent 1", _view.Object));
            Assert.AreEqual(1, _presenter.FormCounter);

            _presenter.CloseAgentWindows();

            Assert.AreEqual(0, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestFormCounterIsReducedByOneWhenSingleAgentIsClose()
        {
            _presenter.CreateNewAgent();
            _presenter.CreateNewAgent();

            _loader.Verify(m => m.LoadAgentView("Agent 2", _view.Object));
            Assert.AreEqual(2, _presenter.FormCounter);

            _presenter.Receive(null);

            Assert.AreEqual(1, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestCascadeAgents()
        {
            _presenter.CascadeAgentWindows();

            _view.Verify(m => m.CascadeAgents());
        }

        [TestMethod]
        public void TestCloseAgents()
        {
            _presenter.CloseAgentWindows();

            _view.Verify(m => m.CloseAgents());
        }

        [TestMethod]
        public void TestFloatAgents()
        {
            _presenter.FloatAgentWindows();

            _view.Verify(m => m.FloatAgents());
        }

        [TestMethod]
        public void TestCloseMainWindowPromptsUser()
        {
            _presenter.WindowClosing(new FormClosingEventArgs(CloseReason.UserClosing, true));

            _view.Verify(m => m.AskUserToConfirmClosing());
        }

        [TestMethod]
        public void TestMainWindowRemainsOpenWithNegativeResponse()
        {
            var args = new FormClosingEventArgs(CloseReason.UserClosing, false);
            _view.Setup(m => m.AskUserToConfirmClosing()).Returns(false);
            
            _presenter.WindowClosing(args);

            _view.Verify(m => m.AskUserToConfirmClosing());
            Assert.IsTrue(args.Cancel);
        }

        [TestMethod]
        public void TestMainWindowClosesWithPositiveResponse()
        {
            var args = new FormClosingEventArgs(CloseReason.UserClosing, false);
            _view.Setup(m => m.AskUserToConfirmClosing()).Returns(true);

            _presenter.WindowClosing(args);

            _view.Verify(m => m.AskUserToConfirmClosing());
            Assert.IsFalse(args.Cancel);
        }
    }
}

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
        private MainPresenter _presenter;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IMainView>();
            _loader = new Mock<IViewLoader>();
            _presenter = new MainPresenter(_view.Object, _loader.Object);
        }

        [TestMethod]
        public void TestCreateNewAgent()
        {
            _view.Raise(m => m.CreateNewAgent += null);

            _loader.Verify(m => m.LoadAgentView("Agent 1", _view.Object));
        }

        [TestMethod]
        public void TestCreatingNewAgentsIncreasesFormCounter()
        {
            _view.Raise(m => m.CreateNewAgent += null);
            _view.Raise(m => m.CreateNewAgent += null);

            _loader.Verify(m => m.LoadAgentView("Agent 2", _view.Object));
            Assert.AreEqual(2, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestFormCounterIsResetWhenAllAgentWindowsAreClosed()
        {
            _view.Raise(m => m.CreateNewAgent += null);

            _loader.Verify(m => m.LoadAgentView("Agent 1", _view.Object));
            Assert.AreEqual(1, _presenter.FormCounter);

            _view.Raise(m => m.CloseAgentWindows += null);

            Assert.AreEqual(0, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestFormCounterIsReducedByOneWhenSingleAgentIsClose()
        {
            _view.Raise(m => m.CreateNewAgent += null);
            _view.Raise(m => m.CreateNewAgent += null);

            _loader.Verify(m => m.LoadAgentView("Agent 2", _view.Object));
            Assert.AreEqual(2, _presenter.FormCounter);

            _presenter.Receive(null);

            Assert.AreEqual(1, _presenter.FormCounter);
        }

        [TestMethod]
        public void TestCascadeAgents()
        {
            _view.Raise(m => m.CascadeAgentWindows += null);

            _view.Verify(m => m.CascadeAgents());
        }

        [TestMethod]
        public void TestCloseAgents()
        {
            _view.Raise(m => m.CloseAgentWindows += null);

            _view.Verify(m => m.CloseAgents());
        }

        [TestMethod]
        public void TestFloatAgents()
        {
            _view.Raise(m => m.FloatAgentWindows += null);

            _view.Verify(m => m.FloatAgents());
        }
    }
}

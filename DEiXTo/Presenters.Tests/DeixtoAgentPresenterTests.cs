using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using DEiXTo.Services;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class DeixtoAgentPresenterTests
    {
        private Mock<IDeixtoAgentView> _view;
        private Mock<IViewLoader> _loader;
        private DeixtoAgentPresenter _presenter;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IDeixtoAgentView>();
            _loader = new Mock<IViewLoader>();
            _presenter = new DeixtoAgentPresenter(_view.Object);
        }
    }
}

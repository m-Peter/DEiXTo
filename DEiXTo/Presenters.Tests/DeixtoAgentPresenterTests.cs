using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using DEiXTo.Services;
using System.Windows.Forms;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class DeixtoAgentPresenterTests
    {
        private Mock<IDeixtoAgentView> _view;
        private Mock<ISaveFileDialog> _saveFileDialog;
        private DeixtoAgentPresenter _presenter;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IDeixtoAgentView>();
            _saveFileDialog = new Mock<ISaveFileDialog>();
            _presenter = new DeixtoAgentPresenter(_view.Object, _saveFileDialog.Object);
        }

        [TestMethod]
        public void TestMethod()
        {
            // Arrange
            _view.Setup(m => m.OutputFileFormat).Returns(Format.Text);
            _saveFileDialog.Setup(m => m.ShowDialog()).Returns(DialogResult.OK);
            _saveFileDialog.Setup(m => m.Filename).Returns("output_file");

            // Act
            _presenter.SelectOutputFile();

            // Assert
            _saveFileDialog.VerifySet(m => m.Filter = "Text Files (*.txt)|");
            _saveFileDialog.VerifySet(m => m.Extension = "txt");
            _view.VerifySet(m => m.OutputFileName = "output_file");
        }
    }
}

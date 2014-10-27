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
        private Mock<IViewLoader> _loader;
        private DeixtoAgentPresenter _presenter;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IDeixtoAgentView>();
            _saveFileDialog = new Mock<ISaveFileDialog>();
            _loader = new Mock<IViewLoader>();
            _presenter = new DeixtoAgentPresenter(_view.Object, _saveFileDialog.Object, _loader.Object);
        }
        
        [TestMethod]
        public void TestForTextOutputFormatDialogAddsTextFilterAndExtesion()
        {
            // Arrange
            _view.Setup(v => v.OutputFileFormat).Returns(Format.Text);

            // Act
            _presenter.SelectOutputFile();

            // Assert
            _saveFileDialog.VerifySet(s => s.Filter = "Text Files (*.txt)|");
            _saveFileDialog.VerifySet(s => s.Extension = "txt");
        }

        [TestMethod]
        public void TestForXmlOutputFormatDialogAddsXmlFilterAndExtension()
        {
            // Arrange
            _view.Setup(v => v.OutputFileFormat).Returns(Format.XML);

            // Act
            _presenter.SelectOutputFile();

            // Assert
            _saveFileDialog.VerifySet(s => s.Filter = "XML Files (*.xml)|");
            _saveFileDialog.VerifySet(s => s.Extension = "xml");
        }

        [TestMethod]
        public void TestForRssOutputFormatDialogAddsRssFilterAndExtesion()
        {
            // Arrange
            _view.Setup(v => v.OutputFileFormat).Returns(Format.RSS);

            // Act
            _presenter.SelectOutputFile();
            
            // Assert
            _saveFileDialog.VerifySet(s => s.Filter = "RSS Files (*.rss)|");
            _saveFileDialog.VerifySet(s => s.Extension = "rss");
        }

        [TestMethod]
        public void TestOutputFileSelectionAbortsForNegativeAnswer()
        {
            // Arrange
            _view.Setup(v => v.OutputFileFormat).Returns(Format.XML);
            _saveFileDialog.Setup(s => s.ShowDialog()).Returns(DialogResult.Abort);

            // Act
            _presenter.SelectOutputFile();
            
            // Assert
            _view.VerifyGet(v => v.OutputFileName, Times.Never);
        }

        [TestMethod]
        public void TestOutputFileNameIsSetAfterSelectingFile()
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

        [TestMethod]
        public void TestAddSiblingOrder()
        {
            // Arrange
            var node = new TreeNode("DIV");

            // Act
            _presenter.AddSiblingOrder(node);

            // Assert
            _loader.Verify(l => l.LoadAddSiblingOrderView(It.Is<TreeNode>(n => n == node)));
        }
    }
}

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
        private Mock<IMainWindowPresenter> presenter;

        [TestInitialize]
        public void SetUp()
        {
            window = new MainWindow();
            presenter = new Mock<IMainWindowPresenter>();
            window.Presenter = presenter.Object;
        }

        [TestMethod]
        public void TestCreateNewAgent()
        {
            // Act
            window.NewAgentMenuItem.PerformClick();
            
            // Assert
            presenter.Verify(p => p.CreateNewAgent());
        }

        [TestMethod]
        public void TestCascadeAgents()
        {
            // Arrange
            window.NewAgentMenuItem.PerformClick();
            
            // Act
            window.CascadeAgentsMenuItem.PerformClick();

            // Assert
            presenter.Verify(p => p.CascadeAgentWindows());
        }

        [TestMethod]
        public void TestFloatAgents()
        {
            // Arrange
            window.NewAgentMenuItem.PerformClick();

            // Act
            window.FloatAgentsMenuItem.PerformClick();

            // Assert
            presenter.Verify(p => p.FloatAgentWindows());
        }

        [TestMethod]
        public void TestCloseAgents()
        {
            // Arrange
            window.NewAgentMenuItem.PerformClick();

            // Act
            window.CloseAgentsMenuItem.PerformClick();

            // Assert
            presenter.Verify(p => p.CloseAgentWindows());
        }

        [TestMethod]
        public void TestUpdateBrowserVersion()
        {
            // Act
            window.UpdateBrowserVersionMenuItem.PerformClick();

            // Assert
            presenter.Verify(p => p.UpdateBrowserVersion());
        }

        [TestMethod]
        public void TestResetBrowserVersion()
        {
            // Act
            window.ResetToDefaultMenuItem.PerformClick();

            // Assert
            presenter.Verify(p => p.ResetBrowserVersion());
        }

        [TestMethod]
        public void TestClosingWindowPromptsUser()
        {
            // Arrange
            window.Show();

            // Act
            window.Close();

            // Assert
            presenter.Verify(p => p.WindowClosing(It.IsAny<FormClosingEventArgs>()));
        }
    }
}

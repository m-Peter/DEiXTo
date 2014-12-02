using DEiXTo.Presenters;
using DEiXTo.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEiXTo.Views.Tests
{
    [TestClass]
    public class DeixtoAgentWindowTests
    {
        private DeixtoAgentWindow window;
        private IViewLoader loader;
        private IEventHub eventHub;
        private IDeixtoAgentScreen screen;
        private DeixtoAgentPresenter presenter;

        [TestInitialize]
        public void SetUp()
        {
            window = new DeixtoAgentWindow();
            loader = new WindowsViewLoader();
            eventHub = EventHub.Instance;
            screen = new DeixtoAgentScreen();
            presenter = new DeixtoAgentPresenter(window, loader, eventHub, screen);
        }

        [TestMethod]
        public void TestStartingState()
        {
            Assert.AreEqual(1, window.DelayNUD.Value);
            Assert.IsTrue(window.SilentlyCheckBox.Checked);
            Assert.IsTrue(window.AutoScrollCheckBox.Checked);
            Assert.IsTrue(window.HighlightModeCheckBox.Checked);
            Assert.IsFalse(window.AutoFillCheckBox.Checked);
            Assert.IsFalse(window.CrawlingCheckBox.Checked);
            Assert.IsFalse(window.ExtractURLCheckBox.Checked);
        }

        [TestMethod]
        public void TestEnableSubmitFormFields()
        {
            window.AutoFillCheckBox.Checked = true;

            Assert.IsTrue(window.AutoFillCheckBox.Checked);
            Assert.IsTrue(window.FormNameTextBox.Enabled);
            Assert.IsTrue(window.InputNameTextBox.Enabled);
            Assert.IsTrue(window.SearchQueryTextBox.Enabled);

            window.AutoFillCheckBox.Checked = false;

            Assert.IsFalse(window.AutoFillCheckBox.Checked);
            Assert.IsFalse(window.FormNameTextBox.Enabled);
            Assert.IsFalse(window.InputNameTextBox.Enabled);
            Assert.IsFalse(window.SearchQueryTextBox.Enabled);
        }

        [TestMethod]
        public void TestEnableMultiPageCrawlingFields()
        {
            window.CrawlingCheckBox.Checked = true;

            Assert.IsTrue(window.CrawlingCheckBox.Checked);
            Assert.IsTrue(window.CrawlingDepthNUD.Enabled);
            Assert.IsTrue(window.HTMLLinkTextBox.Enabled);

            window.CrawlingCheckBox.Checked = false;

            Assert.IsFalse(window.CrawlingCheckBox.Checked);
            Assert.IsFalse(window.CrawlingDepthNUD.Enabled);
            Assert.IsFalse(window.HTMLLinkTextBox.Enabled);
        }

        [TestMethod]
        public void TestGetAndSetAutoScroll()
        {
            window.AutoScrollCheckBox.Checked = false;
            Assert.IsFalse(window.CanAutoScroll);

            window.CanAutoScroll = true;
            Assert.IsTrue(window.AutoScrollCheckBox.Checked);
        }

        [TestMethod]
        public void TestGetAndSetAutoFill()
        {
            window.AutoFillCheckBox.Checked = true;
            Assert.IsTrue(window.AutoFill);

            window.AutoFill = false;
            Assert.IsFalse(window.AutoFillCheckBox.Checked);
        }

        [TestMethod]
        public void TestGetAndSetMultiPage()
        {
            window.CrawlingCheckBox.Checked = true;
            Assert.IsTrue(window.CrawlingEnabled);

            window.CrawlingEnabled = false;
            Assert.IsFalse(window.CrawlingCheckBox.Checked);
        }

        [TestMethod]
        public void TestGetAndSetExtractNativeUrl()
        {
            window.ExtractURLCheckBox.Checked = true;
            Assert.IsTrue(window.ExtractNativeUrl);

            window.ExtractNativeUrl = false;
            Assert.IsFalse(window.ExtractURLCheckBox.Checked);
        }

        [TestMethod]
        public void TestGetAndSetOutputMode()
        {
            window.AppendRadioBtn.Checked = true;
            Assert.AreEqual(OutputMode.Append, window.OutputMode);

            window.OverwriteRadioBtn.Checked = true;
            Assert.AreEqual(OutputMode.Overwrite, window.OutputMode);
        }
    }
}

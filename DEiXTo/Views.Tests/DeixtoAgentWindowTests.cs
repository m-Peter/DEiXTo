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

        [TestMethod]
        public void TestGetAndSetNumberOfHits()
        {
            window.HitsNUD.Value = 3;
            Assert.AreEqual(3, window.NumberOfHits);

            window.NumberOfHits = 2;
            Assert.AreEqual(2, window.HitsNUD.Value);
        }

        [TestMethod]
        public void TestGetAndSetDelayBetweenUrls()
        {
            window.DelayNUD.Value = 3;
            Assert.AreEqual(3, window.Delay);

            window.Delay = 1;
            Assert.AreEqual(1, window.DelayNUD.Value);
        }

        [TestMethod]
        public void TestGetAndSetAutoFillFields()
        {
            window.AutoFillCheckBox.Checked = true;
            window.FormNameTextBox.Text = "s-form";
            window.InputNameTextBox.Text = "q";
            window.SearchQueryTextBox.Text = "JavaScript";

            Assert.AreEqual("s-form", window.FormName);
            Assert.AreEqual("q", window.FormInputName);
            Assert.AreEqual("JavaScript", window.FormTerm);

            window.AutoFill = false;

            Assert.AreEqual("", window.FormNameTextBox.Text);
            Assert.AreEqual("", window.InputNameTextBox.Text);
            Assert.AreEqual("", window.SearchQueryTextBox.Text);
        }

        [TestMethod]
        public void TestGetAndSetCrawlingFields()
        {
            window.CrawlingCheckBox.Checked = true;
            window.CrawlingDepthNUD.Value = 4;
            window.HTMLLinkTextBox.Text = "Next";

            Assert.AreEqual(4, window.MaxCrawlingDepth);
            Assert.AreEqual("Next", window.HtmlNextLink);

            window.CrawlingEnabled = false;

            Assert.AreEqual(5, window.CrawlingDepthNUD.Value);
            Assert.AreEqual("", window.HTMLLinkTextBox.Text);
        }

        [TestMethod]
        public void TestGetAndSetOutputFileFields()
        {
            window.OutputFileNameTextBox.Text = "some_file";
            window.OutputFileFormatComboBox.SelectedIndex = 0;

            Assert.AreEqual("some_file", window.OutputFileName);
            Assert.AreEqual(Format.Text, window.OutputFormat);

            window.OutputFileName = "another_file";
            window.OutputFormat = Format.XML;

            Assert.AreEqual("another_file", window.OutputFileNameTextBox.Text);
            var selectedItem = (OutputFormat)window.OutputFileFormatComboBox.SelectedItem;
            Assert.AreEqual(Format.XML, selectedItem.Format);
        }

        [TestMethod]
        public void TestGetAndSetIgnoredTags()
        {
            window.HTMLTagsListBox.SetItemChecked(0, true);
            window.HTMLTagsListBox.SetItemChecked(2, true);

            var ignoredTags = window.IgnoredTags;
            Assert.AreEqual(2, ignoredTags.Length);
            Assert.AreEqual("<B>", ignoredTags[0]);
            Assert.AreEqual("<I>", ignoredTags[1]);

            window.HTMLTagsListBox.SetItemChecked(0, false);
            window.HTMLTagsListBox.SetItemChecked(2, false);
            window.IgnoredTags = new string[] { "<EM>" };

            Assert.AreEqual(1, window.HTMLTagsListBox.CheckedItems.Count);
            Assert.AreEqual("<EM>", window.HTMLTagsListBox.CheckedItems[0]);
        }

        [TestMethod]
        public void TestGetAndSetTargetUrls()
        {
            window.TargetURLsListBox.Items.Add("http://www.google.gr/");
            window.TargetURLsListBox.Items.Add("http://www.teilar.gr/");

            var targetUrls = window.TargetUrls;
            Assert.AreEqual(2, targetUrls.Length);
            Assert.AreEqual("http://www.google.gr/", targetUrls[0]);
            Assert.AreEqual("http://www.teilar.gr/", targetUrls[1]);

            window.TargetURLsListBox.Items.Clear();
            window.TargetUrls = new string[] { "http://www.skai-news.gr/" };

            Assert.AreEqual(1, window.TargetURLsListBox.Items.Count);
            Assert.AreEqual("http://www.skai-news.gr/", window.TargetURLsListBox.Items[0]);
        }
    }
}

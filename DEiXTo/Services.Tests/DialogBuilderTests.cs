using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class DialogBuilderTests
    {
        private DialogBuilderFactory builderFactory;
        private SaveFileDialogWrapper dialog;

        [TestInitialize]
        public void SetUp()
        {
            builderFactory = new DialogBuilderFactory();
            dialog = new SaveFileDialogWrapper();
        }

        [TestCleanup]
        public void TearDown()
        {
            dialog.Reset();
        }

        [TestMethod]
        public void TestTextDialogBuilder()
        {
            // Arrange
            var format = Format.Text;
            var builder = builderFactory.CreateBuilder(format);

            // Act
            builder.Build(dialog);

            // Assert
            Assert.AreEqual("Text Files (*.txt)|", dialog.Filter);
            Assert.AreEqual("txt", dialog.Extension);
        }

        [TestMethod]
        public void TestXmlDialogBuilder()
        {
            // Arrange
            var format = Format.XML;
            var builder = builderFactory.CreateBuilder(format);

            // Act
            builder.Build(dialog);

            // Assert
            Assert.AreEqual("XML Files (*.xml)|", dialog.Filter);
            Assert.AreEqual("xml", dialog.Extension);
        }

        [TestMethod]
        public void TestRssDialogBuilder()
        {
            // Arrange
            var format = Format.RSS;
            var builder = builderFactory.CreateBuilder(format);

            // Act
            builder.Build(dialog);

            // Assert
            Assert.AreEqual("RSS Files (*.rss)|", dialog.Filter);
            Assert.AreEqual("rss", dialog.Extension);
        }
    }
}

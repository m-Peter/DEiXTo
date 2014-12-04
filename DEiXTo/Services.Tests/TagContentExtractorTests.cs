using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using mshtml;
using DEiXTo.TestHelpers;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class TagContentExtractorTests
    {
        [TestMethod]
        public void TestExtractContentFromLinkTag()
        {
            // Arrange
            var link = TestUtils.CreateLinkElement();
            var extractor = ContentExtractionFactory.GetExtractorFor(link);

            // Act
            var content = extractor.ExtractContent();

            // Assert
            Assert.AreEqual("http://www.google.gr/?page=1", content);
        }

        [TestMethod]
        public void TestExtractContentFromImageTag()
        {
            // Arrange
            var image = TestUtils.CreateImageElement();
            var extractor = ContentExtractionFactory.GetExtractorFor(image);

            // Act
            var content = extractor.ExtractContent();

            // Assert
            Assert.AreEqual("http://www.images.com/img/main/thumb-small.png", content);
        }

        [TestMethod]
        public void TestExtractContentFromFormTag()
        {
            // Arrange
            var form = TestUtils.CreateFormElement();
            var extractor = ContentExtractionFactory.GetExtractorFor(form);

            // Act
            var content = extractor.ExtractContent();

            // Assert
            Assert.AreEqual("query", content);
        }

        [TestMethod]
        public void TestExtractContentFromInputTag()
        {
            // Arrange

            var input = TestUtils.CreateInputElement();
            var extractor = ContentExtractionFactory.GetExtractorFor(input);

            // Act
            var content = extractor.ExtractContent();

            // Assert
            Assert.AreEqual("s", content);
        }

        [TestMethod]
        public void TestExtractContentFromGenericTag()
        {
            // Arrange
            var element = TestUtils.CreateGenericElement();
            var extractor = ContentExtractionFactory.GetExtractorFor(element);

            // Act
            var content = extractor.ExtractContent();

            // Assert
            Assert.AreEqual("Some text in here", content);
        }
    }
}

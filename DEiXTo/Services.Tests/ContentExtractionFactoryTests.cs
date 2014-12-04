using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using mshtml;
using System.Text.RegularExpressions;
using DEiXTo.TestHelpers;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class ContentExtractionFactoryTests
    {
        public void TestReturnsLinkExtractorForLinkElement()
        {
            // Arrange
            var link = TestUtils.CreateLinkElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(link);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(LinkContentExtractor));
        }

        [TestMethod]
        public void TestReturnsImageExtractorForImageElement()
        {
            // Arrange
            var image = TestUtils.CreateImageElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(image);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(ImageContentExtractor));
        }

        [TestMethod]
        public void TestReturnsFormExtractorForFormElement()
        {
            // Arrange
            var form = TestUtils.CreateFormElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(form);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(FormContentExtractor));
        }

        [TestMethod]
        public void TestReturnsInputExtractorForInputElement()
        {
            // Arrange
            var input = TestUtils.CreateInputElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(input);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(InputContentExtractor));
        }

        [TestMethod]
        public void TestReturnsGenericExtractorForGenericElement()
        {
            // Arrange
            var element = TestUtils.CreateGenericElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(element);
            
            // Assert
            Assert.IsInstanceOfType(extractor, typeof(GenericContentExtractor));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using mshtml;
using System.Text.RegularExpressions;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class ContentExtractionFactoryTests
    {
        private WebBrowser browser = new WebBrowser();

        [TestMethod]
        public void TestReturnsLinkExtractorForLinkElement()
        {
            // Arrange
            var link = CreateLinkElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(link);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(LinkContentExtractor));
        }

        [TestMethod]
        public void TestReturnsImageExtractorForImageElement()
        {
            // Arrange
            var image = CreateImageElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(image);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(ImageContentExtractor));
        }

        [TestMethod]
        public void TestReturnsFormExtractorForFormElement()
        {
            // Arrange
            var form = CreateFormElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(form);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(FormContentExtractor));
        }

        [TestMethod]
        public void TestReturnsInputExtractorForInputElement()
        {
            // Arrange
            var input = CreateInputElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(input);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(InputContentExtractor));
        }

        [TestMethod]
        public void TestReturnsGenericExtractorForGenericElement()
        {
            // Arrange
            var element = CreateGenericElement();

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(element);
            
            // Assert
            Assert.IsInstanceOfType(extractor, typeof(GenericContentExtractor));
        }

        private IHTMLElement CreateGenericElement()
        {
            var doc = CreateDocument();
            doc.Write("<p>Some text in here</p>");
            var element = doc.GetElementsByTagName("p")[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        private IHTMLElement CreateInputElement()
        {
            var doc = CreateDocument();
            doc.Write("<input name='s' type='text' />");
            var element = doc.GetElementsByTagName("input")[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        private IHTMLElement CreateFormElement()
        {
            var doc = CreateDocument();
            doc.Write("<form method='get' action='http://www.sitepoint.com'></form>");
            var element = doc.Forms[0];
            var domElement = (IHTMLElement)element.DomElement;
            
            return domElement;
        }

        private IHTMLElement CreateImageElement()
        {
            var doc = CreateDocument();
            doc.Write("<img src='/img/main/thumb-small.png' alt='Image' />");
            var element = doc.GetElementsByTagName("img")[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        private IHTMLElement CreateLinkElement()
        {
            var doc = CreateDocument();
            doc.Write("<a href='http://www.google.gr/'>Google</a>");
            var element = doc.GetElementsByTagName("a")[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        private HtmlDocument CreateDocument()
        {
            browser.DocumentText = "some text";
            browser.Show();

            return browser.Document;
        }
    }
}

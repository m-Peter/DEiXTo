using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using mshtml;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class TagContentExtractorTests
    {
        private WebBrowser browser = new WebBrowser();

        [TestMethod]
        public void TestExtractContentFromLinkTag()
        {
            // Arrange
            var link = CreateLinkElement();
            var extractor = ContentExtractionFactory.GetExtractorFor(link);

            // Act
            var content = extractor.ExtractContent();

            // Assert
            Assert.AreEqual("http://www.google.gr/", content);
        }

        [TestMethod]
        public void TestExtractContentFromImageTag()
        {
            // Arrange
            var image = CreateImageElement();
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
            var form = CreateFormElement();
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
            var input = CreateInputElement();
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
            var element = CreateGenericElement();
            var extractor = ContentExtractionFactory.GetExtractorFor(element);

            // Act
            var content = extractor.ExtractContent();

            // Assert
            Assert.AreEqual("Some text in here", content);
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
            doc.Write("<form name='query' method='get' action='http://www.sitepoint.com'></form>");
            var element = doc.Forms[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        private IHTMLElement CreateImageElement()
        {
            var doc = CreateDocument();
            doc.Write("<img src='http://www.images.com/img/main/thumb-small.png' alt='Image' />");
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

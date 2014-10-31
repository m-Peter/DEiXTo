using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using mshtml;

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
            var domElement = link.DomElement as IHTMLElement;

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(LinkContentExtractor));
        }

        [TestMethod]
        public void TestReturnsImageExtractorForImageElement()
        {
            // Arrange
            var image = CreateImageElement();
            var domElement = image.DomElement as IHTMLElement;

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(ImageContentExtractor));
        }

        [TestMethod]
        public void TestReturnsFormExtractorForFormElement()
        {
            // Arrange
            var form = CreateFormElement();
            var domElement = form.DomElement as IHTMLElement;

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(FormContentExtractor));
        }

        [TestMethod]
        public void TestReturnsInputExtractorForInputElement()
        {
            // Arrange
            var input = CreateInputElement();
            var domElement = input.DomElement as IHTMLElement;

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(InputContentExtractor));
        }

        [TestMethod]
        public void TestReturnsGenericExtractorForGenericElement()
        {
            // Arrange
            var element = CreateGenericElement();
            var domElement = element.DomElement as IHTMLElement;

            // Act
            var extractor = ContentExtractionFactory.GetExtractorFor(domElement);
            
            // Assert
            Assert.IsInstanceOfType(extractor, typeof(GenericContentExtractor));
        }

        private HtmlElement CreateGenericElement()
        {
            var doc = CreateDocument();
            doc.Write("<p>Some text in here</p>");
            var element = doc.GetElementsByTagName("p")[0];

            return element;
        }

        private HtmlElement CreateInputElement()
        {
            var doc = CreateDocument();
            doc.Write("<input name='s' type='text' />");
            var element = doc.GetElementsByTagName("input")[0];

            return element;
        }

        private HtmlElement CreateFormElement()
        {
            var doc = CreateDocument();
            doc.Write("<form method='get' action='http://www.sitepoint.com'></form>");
            var element = doc.Forms[0];
            
            return element;
        }

        private HtmlElement CreateImageElement()
        {
            var doc = CreateDocument();
            doc.Write("<img src='/img/main/thumb-small.png' alt='Image' />");
            var element = doc.GetElementsByTagName("img")[0];

            return element;
        }

        private HtmlElement CreateLinkElement()
        {
            var doc = CreateDocument();
            doc.Write("<a href='http://www.google.gr/'>Google</a>");
            var element = doc.GetElementsByTagName("a")[0];

            return element;
        }

        private HtmlDocument CreateDocument()
        {
            browser.DocumentText = "some text";
            browser.Show();

            return browser.Document;
        }
    }
}

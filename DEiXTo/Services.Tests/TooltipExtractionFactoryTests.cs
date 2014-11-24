using Microsoft.VisualStudio.TestTools.UnitTesting;
using mshtml;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class TooltipExtractionFactoryTests
    {
        private WebBrowser browser = new WebBrowser();

        [TestMethod]
        public void TestReturnsLinkExtractorForLinkElement()
        {
            // Arrange
            var link = CreateLinkElement();
            var domElement = link.DomElement as IHTMLElement;

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(LinkTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsImageExtractorForImageElement()
        {
            // Arrange
            var image = CreateImageElement();
            var domElement = image.DomElement as IHTMLElement;

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(ImageTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsFormExtractorForFormElement()
        {
            // Arrange
            var form = CreateFormElement();
            var domElement = form.DomElement as IHTMLElement;

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(FormTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsInputExtractorForInputElement()
        {
            // Arrange
            var input = CreateInputElement();
            var domElement = input.DomElement as IHTMLElement;

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(InputTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsNullExtractorForGenericElement()
        {
            // Arrange
            var element = CreateGenericElement();
            element.ToString();
            var domElement = element.DomElement as IHTMLElement;

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(domElement);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(NullTooltipExtractor));
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

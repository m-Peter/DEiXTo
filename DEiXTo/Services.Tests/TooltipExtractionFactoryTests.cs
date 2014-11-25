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

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(link);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(LinkTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsImageExtractorForImageElement()
        {
            // Arrange
            var image = CreateImageElement();

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(image);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(ImageTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsFormExtractorForFormElement()
        {
            // Arrange
            var form = CreateFormElement();

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(form);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(FormTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsInputExtractorForInputElement()
        {
            // Arrange
            var input = CreateInputElement();

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(input);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(InputTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsNullExtractorForGenericElement()
        {
            // Arrange
            var element = CreateGenericElement();

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(element);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(NullTooltipExtractor));
        }

        [TestMethod]
        public void TestReturnsTextExtractorForTextElement()
        {
            // Arrange
            var element = CreateTextElement();

            // Act
            var extractor = TooltipExtractionFactory.GetTooltipFor(element);

            // Assert
            Assert.IsInstanceOfType(extractor, typeof(TextTooltipExtractor));
        }

        private IHTMLDOMNode CreateGenericElement()
        {
            var doc = CreateDocument();
            doc.Write("<p>Some text in here</p>");
            var element = doc.GetElementsByTagName("p")[0];
            var domElement = (IHTMLDOMNode)element.DomElement;

            return domElement;
        }

        private IHTMLDOMNode CreateTextElement()
        {
            var doc = CreateDocument();
            doc.Write("<p>Some text in here</p>");
            var element = doc.GetElementsByTagName("p")[0];
            var domNode = (IHTMLDOMNode)element.DomElement;
            var textNode = domNode.firstChild;
            return textNode;
        }

        private IHTMLDOMNode CreateInputElement()
        {
            var doc = CreateDocument();
            doc.Write("<input name='s' type='text' />");
            var element = doc.GetElementsByTagName("input")[0];
            var domElement = (IHTMLDOMNode)element.DomElement;

            return domElement;
        }

        private IHTMLDOMNode CreateFormElement()
        {
            var doc = CreateDocument();
            doc.Write("<form method='get' action='http://www.sitepoint.com'></form>");
            var element = doc.Forms[0];
            var domElement = (IHTMLDOMNode)element.DomElement;

            return domElement;
        }

        private IHTMLDOMNode CreateImageElement()
        {
            var doc = CreateDocument();
            doc.Write("<img src='/img/main/thumb-small.png' alt='Image' />");
            var element = doc.GetElementsByTagName("img")[0];
            var domElement = (IHTMLDOMNode)element.DomElement;

            return domElement;
        }

        private IHTMLDOMNode CreateLinkElement()
        {
            var doc = CreateDocument();
            doc.Write("<a href='http://www.google.gr/'>Google</a>");
            var element = doc.GetElementsByTagName("a")[0];
            var domElement = (IHTMLDOMNode)element.DomElement;

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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using mshtml;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class TagTooltipExtractorTests
    {
        private WebBrowser browser = new WebBrowser();

        [TestMethod]
        public void TestExtractTooltipFromLinkTag()
        {
            // Arrange
            var link = CreateLinkElement();
            var extractor = TooltipExtractionFactory.GetTooltipFor(link);

            // Act
            var tooltip = extractor.ExtractTooltip();

            // Assert
            Assert.AreEqual("http://www.google.gr/", tooltip);
        }

        [TestMethod]
        public void TestExtractTooltipFromImageTag()
        {
            // Arrange
            var image = CreateImageElement();
            var extractor = TooltipExtractionFactory.GetTooltipFor(image);

            // Act
            var tooltip = extractor.ExtractTooltip();

            // Assert
            Assert.AreEqual("http://www.images.com/img/main/thumb-small.png", tooltip);
        }

        [TestMethod]
        public void TestExtractTooltipFromFormTag()
        {
            // Arrange
            var form = CreateFormElement();
            var extractor = TooltipExtractionFactory.GetTooltipFor(form);

            // Act
            var tooltip = extractor.ExtractTooltip();

            // Assert
            Assert.AreEqual("query", tooltip);
        }

        [TestMethod]
        public void TestExtractTooltipFromInputTag()
        {
            // Arrange
            var input = CreateInputElement();
            var extractor = TooltipExtractionFactory.GetTooltipFor(input);

            // Act
            var tooltip = extractor.ExtractTooltip();

            // Assert
            Assert.AreEqual("s", tooltip);
        }

        [TestMethod]
        public void TestExtractTooltipFromGenericTag()
        {
            // Arrange
            var element = CreateGenericElement();
            var extractor = TooltipExtractionFactory.GetTooltipFor(element);

            // Act
            var tooltip = extractor.ExtractTooltip();

            // Assert
            Assert.AreEqual(string.Empty, tooltip);
        }

        [TestMethod]
        public void TestExtractTooltipFromTextTag()
        {
            // Arrange
            var element = CreateTextElement();
            var extractor = TooltipExtractionFactory.GetTooltipFor(element);

            // Act
            var tooltip = extractor.ExtractTooltip();

            // Assert
            Assert.AreEqual("Some text in here", element.nodeValue);
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
            doc.Write("<form name='query' method='get' action='http://www.sitepoint.com'></form>");
            var element = doc.Forms[0];
            var domElement = (IHTMLDOMNode)element.DomElement;

            return domElement;
        }

        private IHTMLDOMNode CreateImageElement()
        {
            var doc = CreateDocument();
            doc.Write("<img src='http://www.images.com/img/main/thumb-small.png' alt='Image' />");
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class ElementStylingTests
    {
        private WebBrowser browser = new WebBrowser();
        private static string STYLE = "BORDER-TOP: red 2px solid; BORDER-RIGHT: red 2px solid; BORDER-BOTTOM: red 2px solid; BORDER-LEFT: red 2px solid; BACKGROUND-COLOR: yellow";

        [TestMethod]
        public void TestStyleElement()
        {
            // Arrange
            var styling = new ElementStyling();
            var element = CreateParagraphElement();

            // Act
            styling.Style(element);

            // Assert
            Assert.AreEqual(STYLE, element.Style);
        }

        [TestMethod]
        public void TestStylingElementMaintainsPreviousStyle()
        {
            // Arrange
            var styling = new ElementStyling();
            var p = CreateLinkElement();
            p.Style = "align: center";

            // Act
            styling.Style(p);

            // Assert
            Assert.AreEqual(STYLE + "; align: center", p.Style);
        }

        [TestMethod]
        public void TestUnstyleElement()
        {
            // Arrange
            var styling = new ElementStyling();
            var element = CreateParagraphElement();
            styling.Style(element);

            // Act
            styling.Unstyle(element);

            // Assert
            Assert.AreEqual(null, element.Style);
        }

        [TestMethod]
        public void TestUnstylingElementMaintainsPreviousStyle()
        {
            // Arrange
            var styling = new ElementStyling();
            var p = CreateLinkElement();
            p.Style = "align: center";
            styling.Style(p);

            // Act
            styling.Unstyle(p);

            // Assert
            Assert.AreEqual("align: center", p.Style);
        }

        [TestMethod]
        public void TestUnstyleAllElements()
        {
            // Arrange
            var styling = new ElementStyling();
            var p = CreateParagraphElement();
            var link = CreateLinkElement();
            styling.Style(p);
            styling.Style(link);

            // Act
            styling.UnstyleElements();

            // Assert
            Assert.AreEqual(null, p.Style);
            Assert.AreEqual(null, link.Style);
        }

        [TestMethod]
        public void TestStyleAlreadyStyledElement()
        {
            // Arrange
            var styling = new ElementStyling();
            var element = CreateParagraphElement();
            styling.Style(element);

            // Act
            styling.Style(element);

            // Assert
            Assert.AreEqual(STYLE, element.Style);
        }

        [TestMethod]
        public void TestOnlyOneInstanceOfElementIsKept()
        {
            // Arrange
            var styling = new ElementStyling();
            var element = CreateParagraphElement();

            // Act
            styling.Style(element);

            // Assert
            Assert.AreEqual(STYLE, element.Style);

            // Act
            styling.Style(element);

            // Assert
            Assert.AreEqual(1, styling.Count());
        }

        [TestMethod]
        public void TestClearRemovesAllTheElements()
        {
            // Arrange
            var styling = new ElementStyling();
            var p = CreateParagraphElement();
            var link = CreateLinkElement();

            // Act
            styling.Style(p);
            styling.Style(link);

            // Assert
            Assert.AreEqual(2, styling.Count());

            // Act
            styling.Clear();

            // Assert
            Assert.AreEqual(0, styling.Count());
        }

        private HtmlElement CreateParagraphElement()
        {
            var doc = CreateDocument();
            doc.Write("<p>Some text in here</p>");
            var element = doc.GetElementsByTagName("p")[0];

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

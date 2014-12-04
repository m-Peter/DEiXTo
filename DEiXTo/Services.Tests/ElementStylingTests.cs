using DEiXTo.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class ElementStylingTests
    {
        private static string STYLE = "BORDER-TOP: red 2px solid; BORDER-RIGHT: red 2px solid; BORDER-BOTTOM: red 2px solid; BORDER-LEFT: red 2px solid; BACKGROUND-COLOR: yellow";

        [TestMethod]
        public void TestStyleElement()
        {
            // Arrange
            var styling = new ElementStyling();
            var element = TestUtils.CreateParagraphElement();

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
            var element = TestUtils.CreateHtmlLinkElement();
            element.Style = "align: center";

            // Act
            styling.Style(element);

            // Assert
            Assert.AreEqual(STYLE + "; align: center", element.Style);
        }

        [TestMethod]
        public void TestUnstyleElement()
        {
            // Arrange
            var styling = new ElementStyling();
            var element = TestUtils.CreateParagraphElement();
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
            var element = TestUtils.CreateHtmlLinkElement();
            element.Style = "align: center";
            styling.Style(element);

            // Act
            styling.Unstyle(element);

            // Assert
            Assert.AreEqual("align: center", element.Style);
        }

        [TestMethod]
        public void TestUnstyleAllElements()
        {
            // Arrange
            var styling = new ElementStyling();
            var paragraph = TestUtils.CreateParagraphElement();
            var link = TestUtils.CreateHtmlLinkElement();
            styling.Style(paragraph);
            styling.Style(link);

            // Act
            styling.UnstyleElements();

            // Assert
            Assert.AreEqual(null, paragraph.Style);
            Assert.AreEqual(null, link.Style);
        }

        [TestMethod]
        public void TestStyleAlreadyStyledElement()
        {
            // Arrange
            var styling = new ElementStyling();
            var element = TestUtils.CreateParagraphElement();
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
            var element = TestUtils.CreateParagraphElement();

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
            var paragraph = TestUtils.CreateParagraphElement();
            var link = TestUtils.CreateHtmlLinkElement();

            // Act
            styling.Style(paragraph);
            styling.Style(link);

            // Assert
            Assert.AreEqual(2, styling.Count());

            // Act
            styling.Clear();

            // Assert
            Assert.AreEqual(0, styling.Count());
        }
    }
}

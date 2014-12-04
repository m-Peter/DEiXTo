using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Drawing;
using mshtml;
using DEiXTo.TestHelpers;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class DocumentQueryTests
    {
        private DocumentQuery query;

        [TestInitialize]
        public void SetUp()
        {
            var document = TestUtils.CreateFilledHtmlDocument();
            query = new DocumentQuery(document);
        }

        [TestMethod]
        public void TestGetElementByIndex()
        {
            // Act
            var htmlElement = query.GetElementByIndex(0);

            // Assert
            Assert.AreEqual("HTML", htmlElement.TagName);

            // Act
            var titleElement = query.GetElementByIndex(2);

            // Assert
            Assert.AreEqual("TITLE", titleElement.TagName);
            Assert.AreEqual("My Web Page", titleElement.InnerText);
        }

        [TestMethod]
        public void TestGetHTMLElement()
        {
            // Act
            var htmlElement = query.GetHtmlElement();

            // Assert
            Assert.AreEqual("HTML", htmlElement.TagName);
        }

        [TestMethod]
        public void TestGetElementFromPoint()
        {
            // Act
            var element = query.GetElementFromPoint(new Point(0, 0));

            // Assert
            Assert.IsInstanceOfType(element, typeof(HtmlElement));
            Assert.AreEqual("BODY", element.TagName);
        }

        [TestMethod]
        public void TestGetLinkToFollow()
        {
            // Act
            var linkElement = query.GetLinkToFollow("Next");

            // Assert
            Assert.AreEqual("A", linkElement.TagName);
            Assert.AreEqual("Next", linkElement.InnerText);
        }

        [TestMethod]
        public void TestGetFormElement()
        {
            // Act
            var formElement = query.GetForm("search-form");

            // Assert
            Assert.AreEqual("FORM", formElement.TagName);
            Assert.AreEqual("search-form", formElement.Name);
        }

        [TestMethod]
        public void TestGetInputForForm()
        {
            // Arrange
            var formElement = query.GetForm("search-form");

            // Act
            var inputElement = query.GetInputFor(formElement, "s");

            // Assert
            Assert.AreEqual("INPUT", inputElement.TagName);
            Assert.AreEqual("s", inputElement.Name);
        }

        [TestMethod]
        public void TestFillInput()
        {
            // Arrange
            var formElement = query.GetForm("search-form");
            var inputElement = query.GetInputFor(formElement, "s");

            // Act
            query.FillInput(inputElement, "About");

            // Assert
            var domElem = inputElement.DomElement as IHTMLInputElement;
            Assert.AreEqual("About", domElem.value);
        }

        [TestMethod]
        public void TestSubmitForm()
        {
            // Arrange
            var formElement = query.GetForm("search-form");
            var inputElement = query.GetInputFor(formElement, "s");

            // Act
            query.FillInput(inputElement, "about");
            query.SubmitForm(formElement);
        }
    }
}

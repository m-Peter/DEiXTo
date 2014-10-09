using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class DocumentValidatorFactoryTests
    {
        // What are the different cases we should check?
        // VALID CASES for web document:
        // - insert http without www (http://github.com)
        // - insert only www (www.github.com)
        // - omit http and www (github.com)
        // - with http and www (http://www.github.com)
        // VALID CASES for local document:
        // - insert the file part (file:///C:/Users/Peter/Desktop/brendan%20forster.htm)
        // - omit the file part (C:\Users\Peter\Desktop\brendan%20forster.htm)

        [TestMethod]
        public void TestReturnsWebValidatorForURLWithHttpComponentOnly()
        {
            // Arrange
            DocumentValidatorFactory factory = new DocumentValidatorFactory();
            string url = "http://github.com";

            // Act
            var validator = factory.CreateValidator(url);

            // Assert
            Assert.IsInstanceOfType(validator, typeof(WebDocumentValidator));
        }

        [TestMethod]
        public void TestReturnsWebValidatorForURLWithWWWComponentOnly()
        {
            // Arrange
            DocumentValidatorFactory factory = new DocumentValidatorFactory();
            string url = "www.github.com";

            // Act
            var validator = factory.CreateValidator(url);

            // Assert
            Assert.IsInstanceOfType(validator, typeof(WebDocumentValidator));
        }

        [TestMethod]
        public void TestReturnsWebValidatorForURLWithHttpAndWWWComponents()
        {
            // Arrange
            DocumentValidatorFactory factory = new DocumentValidatorFactory();
            string url = "http://www.github.com";

            // Act
            var validator = factory.CreateValidator(url);

            // Assert
            Assert.IsInstanceOfType(validator, typeof(WebDocumentValidator));
        }

        [TestMethod]
        public void TestReturnsWebValidatorForURLWithoutHttpAndWWWComponents()
        {
            // Arrange
            DocumentValidatorFactory factory = new DocumentValidatorFactory();
            string url = "github.com";

            // Act
            var validator = factory.CreateValidator(url);
            
            // Assert
            Assert.IsInstanceOfType(validator, typeof(WebDocumentValidator));
        }

        [TestMethod]
        public void TestReturnsLocalValidatorForURLWithFilePart()
        {
            // Arrange
            DocumentValidatorFactory factory = new DocumentValidatorFactory();
            string url = "file:///C:/Users/Peter/Desktop/brendan%20forster.htm";

            // Act
            var validator = factory.CreateValidator(url);

            // Assert
            Assert.IsInstanceOfType(validator, typeof(LocalDocumentValidator));
        }

        [TestMethod]
        public void TestReturnsLocalValidatorForURLWithBackslash()
        {
            // Arrange
            DocumentValidatorFactory factory = new DocumentValidatorFactory();
            string url = "file:///C:/Users\\Peter/Desktop/brendan%20forster.htm";

            // Act
            var validator = factory.CreateValidator(url);

            // Assert
            Assert.IsInstanceOfType(validator, typeof(LocalDocumentValidator));
        }

        [TestMethod]
        public void TestReturnsLocalValidatorForURLWithoutFilePart()
        {
            // Arrange
            DocumentValidatorFactory factory = new DocumentValidatorFactory();
            string url = "C:/Users/Peter/Desktop/brendan%20forster.htm";

            // Act
            var validator = factory.CreateValidator(url);

            // Assert
            Assert.IsInstanceOfType(validator, typeof(LocalDocumentValidator));
        }
    }
}

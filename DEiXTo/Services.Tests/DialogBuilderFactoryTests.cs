using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class DialogBuilderFactoryTests
    {
        private DialogBuilderFactory builderFactory;

        [TestInitialize]
        public void SetUp()
        {
            builderFactory = new DialogBuilderFactory();
        }

        [TestMethod]
        public void TestReturnsTextDialogForTextFormat()
        {
            // Arrange
            var format = Format.Text;

            // Act
            var dialogBuilder = builderFactory.CreateBuilder(format);

            // Assert
            Assert.IsInstanceOfType(dialogBuilder, typeof(TextDialogBuilder));
        }

        [TestMethod]
        public void TestReturnsXmlDialogForXmlFormat()
        {
            // Arrange
            var format = Format.XML;

            // Act
            var dialogBuilder = builderFactory.CreateBuilder(format);

            // Assert
            Assert.IsInstanceOfType(dialogBuilder, typeof(XmlDialogBuilder));
        }

        [TestMethod]
        public void TestReturnsRssDialogForRssFormat()
        {
            // Arrange
            var format = Format.RSS;

            // Act
            var dialogBuilder = builderFactory.CreateBuilder(format);

            // Assert
            Assert.IsInstanceOfType(dialogBuilder, typeof(RssDialogBuilder));
        }
    }
}

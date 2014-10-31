using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class RecordsWriterFactoryTests
    {
        [TestMethod]
        public void TestReturnsTextWriterForTextFormat()
        {
            // Arrange
            var format = Format.Text;
            string filename = "some_file";

            // Act
            var writer = RecordsWriterFactory.GetWriterFor(format, filename);

            // Assert
            Assert.IsInstanceOfType(writer, typeof(TextRecordsWriter));
        }

        [TestMethod]
        public void TestReturnsXmlWriterForXmlFormat()
        {
            // Arrange
            var format = Format.XML;
            string filename = "some_file";

            // Act
            var writer = RecordsWriterFactory.GetWriterFor(format, filename);

            // Assert
            Assert.IsInstanceOfType(writer, typeof(XmlRecordsWriter));
        }
    }
}

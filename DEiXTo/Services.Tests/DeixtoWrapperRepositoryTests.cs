using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class DeixtoWrapperRepositoryTests
    {
        private string _filename = "wrapper.xml";
        private MemoryStream _stream;

        [TestInitialize]
        public void SetUp()
        {
            _stream = new MemoryStream();
        }

        [TestCleanup]
        public void TearDown()
        {
            _stream.Close();
        }

        [TestMethod]
        public void TestSaveAndLoadSubmitFormFields()
        {
            // Arrange
            var wrapper = new DeixtoWrapper();
            wrapper.AutoFill = true;
            wrapper.FormName = "search-repo";
            wrapper.FormInputName = "query";
            wrapper.FormTerm = "rails";
            var repository = new DeixtoWrapperFileRepository(_filename);

            // Act
            repository.Save(wrapper, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);

            // Assert
            Assert.IsTrue(loaded.AutoFill);
            Assert.AreEqual("search-repo", loaded.FormName);
            Assert.AreEqual("query", loaded.FormInputName);
            Assert.AreEqual("rails", loaded.FormTerm);
        }

        [TestMethod]
        public void TestSaveAndLoadExtractUrl()
        {
            // Arrange
            var wrapper = new DeixtoWrapper();
            wrapper.ExtractNativeUrl = true;
            var repository = new DeixtoWrapperFileRepository(_filename);
            
            // Act
            repository.Save(wrapper, _stream);
            _stream.Position = 0;
            var loaded = repository.Load(_stream);
            
            // Assert
            Assert.IsTrue(loaded.ExtractNativeUrl);
        }
    }
}

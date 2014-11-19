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
        private DeixtoWrapper _wrapper;
        private DeixtoWrapperFileRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            _stream = new MemoryStream();
            _wrapper = new DeixtoWrapper();
            _repository = new DeixtoWrapperFileRepository(_filename);
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
            _wrapper.AutoFill = true;
            _wrapper.FormName = "search-repo";
            _wrapper.FormInputName = "query";
            _wrapper.FormTerm = "rails";

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

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
            _wrapper.ExtractNativeUrl = true;
            
            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);
            
            // Assert
            Assert.IsTrue(loaded.ExtractNativeUrl);
        }

        [TestMethod]
        public void TestSaveAndLoadMaxHits()
        {
            // Arrange
            _wrapper.NumberOfHits = 3;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual(3, loaded.NumberOfHits);
        }
    }
}

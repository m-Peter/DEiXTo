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

        [TestMethod]
        public void TestSaveAndLoadMultiPageFields()
        {
            // Arrange
            _wrapper.MultiPageCrawling = true;
            _wrapper.HtmlNextLink = "Next";
            _wrapper.MaxCrawlingDepth = 5;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.IsTrue(loaded.MultiPageCrawling);
            Assert.AreEqual("Next", loaded.HtmlNextLink);
            Assert.AreEqual(5, loaded.MaxCrawlingDepth);
        }

        [TestMethod]
        public void TestSaveAndLoadInputFile()
        {
            // Arrange
            _wrapper.InputFile = "input_file.txt";

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual("input_file.txt", loaded.InputFile);
        }

        [TestMethod]
        public void TestSaveAndLoadOutputFileFields()
        {
            // Arrange
            _wrapper.OutputFileName = "output_file.xml";
            _wrapper.OutputFormat = Format.XML;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual("output_file.xml", loaded.OutputFileName);
            Assert.AreEqual(Format.XML, loaded.OutputFormat);
        }

        [TestMethod]
        public void TestSaveAndLoadAppendMode()
        {
            // Arrange
            _wrapper.OutputMode = OutputMode.Append;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual(OutputMode.Append, loaded.OutputMode);
        }

        [TestMethod]
        public void TestSaveAndLoadOverwriteMode()
        {
            // Arrange
            _wrapper.OutputMode = OutputMode.Overwrite;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            Assert.AreEqual(OutputMode.Overwrite, loaded.OutputMode);
        }

        [TestMethod]
        public void TestSaveAndLoadTargetUrls()
        {
            // Arrange
            string[] urls = new string[3];
            urls[0] = "http://www.cs.teilar.gr";
            urls[1] = "http://www.teilar.gr";
            urls[2] = "http://www.petrmarkou.com";
            _wrapper.TargetUrls = urls;

            // Act
            _repository.Save(_wrapper, _stream);
            _stream.Position = 0;
            var loaded = _repository.Load(_stream);

            // Assert
            var loadedUrls = loaded.TargetUrls;
            Assert.AreEqual(3, loadedUrls.Length);
            Assert.AreEqual("http://www.cs.teilar.gr", loadedUrls[0]);
            Assert.AreEqual("http://www.teilar.gr", loadedUrls[1]);
            Assert.AreEqual("http://www.petrmarkou.com", loadedUrls[2]);
        }
    }
}

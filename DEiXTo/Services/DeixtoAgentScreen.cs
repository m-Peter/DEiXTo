﻿using DEiXTo.Models;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class DeixtoAgentScreen : IDeixtoAgentScreen
    {
        private ElementStyling _styling;
        private IDOMBuilder _builder;
        private DocumentQuery _document;
        private StatesImageLoader _imageLoader;
        private DOMTree _domTree;
        private IViewLoader _loader;
        private ISaveFileDialog _saveFileDialog;
        private IOpenFileDialog _openFileDialog;
        private Executor _executor;
        private ReadTargetUrls _readTargetUrls;
        private TextRecordsWriter _recordsWriter;
        private IExtractionPatternRepository _patternRepository;

        public DeixtoAgentScreen()
        {
            _styling = new ElementStyling();
            _imageLoader = new StatesImageLoader();
            _loader = new WindowsViewLoader();
            _readTargetUrls = new ReadTargetUrls();
            _domTree = new DOMTree();
            _openFileDialog = new OpenFileDialogWrapper();
            _saveFileDialog = new SaveFileDialogWrapper();
        }

        public HtmlElement GetElementFromNode(TreeNode node)
        {
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            return element;
        }

        public HtmlElement GetElementFromPosition(Point position)
        {
            var element = _document.GetElementFromPoint(position);

            return element;
        }

        public TreeNode ScanDomTree(TreeNode pattern)
        {
            var node = _domTree.ScanTree(pattern);

            return node;
        }

        public TreeNode GetNodeFromElement(HtmlElement element)
        {
            var node = _domTree.GetNodeFor(element);

            return node;
        }

        public TreeNode GetDomNode(TreeNode node)
        {
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);
            var domNode = _domTree.GetNodeFor(element);

            return domNode;
        }

        public TreeNode LoadExtractionPattern(string filename)
        {
            _patternRepository = new ExtractionPatternFileRepository(filename);
            var loader = new FileLoader();

            using (var stream = loader.Load(filename, FileMode.Open))
            {
                return _patternRepository.Load(stream).RootNode;
            }
        }

        public IOpenFileDialog GetOpenFileDialog(string filter)
        {
            resetOpenFileDialog();
            _openFileDialog.Filter = filter;

            return _openFileDialog;
        }

        public ISaveFileDialog GetSaveFileDialog(string filter, string extension)
        {
            resetSaveFileDialog();
            _saveFileDialog.Filter = filter;
            _saveFileDialog.Extension = extension;

            return _saveFileDialog;
        }

        public ISaveFileDialog GetSaveFileDialog(Format format)
        {
            resetSaveFileDialog();
            var factory = new DialogBuilderFactory();
            var builder = factory.CreateBuilder(format);
            builder.Build(_saveFileDialog);

            return _saveFileDialog;
        }

        public string[] LoadUrlsFromFile(string filename)
        {
            var urls = _readTargetUrls.Read(filename);
            return urls;
        }

        public void WriteExtractedRecords(string filename)
        {
            _recordsWriter = new TextRecordsWriter(filename);
            var records = _executor.ExtractedResults();
            _recordsWriter.Write(records);
        }

        public void SaveExtractionPattern(string filename, TreeNode node)
        {
            _patternRepository = new ExtractionPatternFileRepository(filename);
            ExtractionPattern pattern = new ExtractionPattern(node);
            var loader = new FileLoader();
            
            using (var stream = loader.Load(filename, FileMode.Create))
            {
                _patternRepository.Save(pattern, stream);
            }
        }

        public void HighlightElement(HtmlElement element)
        {
            _styling.UnstyleElements();
            _styling.Style(element);
        }

        public void RemoveHighlighting(HtmlElement element)
        {
            _styling.Unstyle(element);
        }

        public void ClearStyling()
        {
            _styling.Clear();
        }

        public void CreateDocument(HtmlDocument document)
        {
            _document = new DocumentQuery(document);
        }

        public TreeNode BuildSimplifiedDOM(string[] ignoredTags)
        {
            var element = _document.GetHtmlElement();
            _builder = new SimplifiedDOMBuilder(element, ignoredTags);
            _domTree = _builder.Build();

            return _domTree.RootNode;
        }

        public TreeNode BuildDom()
        {
            var element = _document.GetHtmlElement();
            _builder = new DOMBuilder(element);
            _domTree = _builder.Build();

            return _domTree.RootNode;
        }

        public ImageList LoadStateImages()
        {
            return _imageLoader.LoadImages();
        }

        public IExtraction Execute(TreeNode pattern, TreeNodeCollection domNodes)
        {
            var extractionPattern = new ExtractionPattern(pattern);
            _executor = new Executor(extractionPattern, domNodes);
            _executor.FindMatches();

            var result = new ExtractionResult();
            result.VariablesCount = _executor.CountOutputVariables();
            result.OutputVariableLabels = _executor.OutputVariableLabels();
            result.ExtractedRecords = _executor.ExtractedResults();

            return result;
        }

        public void SaveWrapper(DeixtoWrapper wrapper, TreeNodeCollection nodes, string filename)
        {
            var wrapperRepository = new DeixtoWrapperFileRepository(filename);
            var loader = new FileLoader();

            using (var stream = loader.Load(filename, FileMode.Create))
            {
                wrapperRepository.Save(wrapper, stream);
            }
        }

        public DeixtoWrapper LoadWrapper(string filename)
        {
            var wrapperRepository = new DeixtoWrapperFileRepository(filename);
            var loader = new FileLoader();

            using (var stream = loader.Load(filename, FileMode.Open))
            {
                return wrapperRepository.Load(stream);
            }
        }

        public void SubmitForm(string formName, string inputName, string term)
        {
            var form = _document.GetForm(formName);
            var input = _document.GetInputFor(form, inputName);
            _document.FillInput(input, term);
            _document.SubmitForm(form);
        }

        public HtmlElement GetLinkToFollow(string link)
        {
            return _document.GetLinkToFollow(link);
        }

        private void resetOpenFileDialog()
        {
            _openFileDialog.Reset();
        }

        private void resetSaveFileDialog()
        {
            _saveFileDialog.Reset();
        }
    }
}

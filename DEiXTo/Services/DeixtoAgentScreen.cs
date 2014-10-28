using DEiXTo.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class DeixtoAgentScreen : IDeixtoAgentScreen
    {
        private ElementStyling _styling;
        private TreeBuilder _builder;
        private DocumentQuery _document;
        private StatesImageLoader _imageLoader;
        private DOMTreeStructure _domTree;
        private IViewLoader _loader;
        private ISaveFileDialog _saveFileDialog;
        private IOpenFileDialog _openFileDialog;
        private PatternExecutor _executor;
        private ReadTargetUrls _readTargetUrls;
        private TextRecordsWriter _recordsWriter;

        public DeixtoAgentScreen()
        {
            _styling = new ElementStyling();
            _builder = new TreeBuilder();
            _imageLoader = new StatesImageLoader();
            _loader = new WindowsViewLoader();
            _readTargetUrls = new ReadTargetUrls();
            _domTree = new DOMTreeStructure();
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
            var reader = new ReadExtractionPattern();
            var node = reader.read(filename);

            return node;
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

        public void SaveExtractionPattern(string filename, TreeNodeCollection nodes)
        {
            var writer = new WriteExtractionPattern();
            writer.write(filename, nodes);
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
            _domTree = _builder.BuildSimplifiedDOMTree(element, ignoredTags);

            return _domTree.RootNode;
        }

        public TreeNode BuildDom()
        {
            var element = _document.GetHtmlElement();
            _domTree = _builder.BuildDOMTree(element);

            return _domTree.RootNode;
        }

        public ImageList LoadStateImages()
        {
            return _imageLoader.LoadImages();
        }

        public IExtraction Execute(TreeNode pattern, TreeNodeCollection domNodes)
        {
            _executor = new PatternExecutor(pattern, domNodes);
            _executor.FindMatches();

            var result = new ExtractionResult();
            result.VariablesCount = _executor.CountOutputVariables();
            result.OutputVariableLabels = _executor.OutputVariableLabels();
            result.ExtractedRecords = _executor.ExtractedResults();

            return result;
        }

        private void resetOpenFileDialog()
        {
            _openFileDialog.Filename = string.Empty;
            _openFileDialog.Filter = string.Empty;
        }

        private void resetSaveFileDialog()
        {
            _saveFileDialog.Filename = string.Empty;
            _saveFileDialog.Filter = string.Empty;
            _saveFileDialog.Extension = string.Empty;
        }
    }
}

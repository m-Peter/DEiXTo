using System;
using System.Collections.Generic;
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
        private IViewLoader _loader;
        private ISaveFileDialog _saveFileDialog;
        private IOpenFileDialog _openFileDialog;
        private PatternExtraction _executor;
        private ReadTargetUrls _readTargetUrls;
        private TextRecordsWriter _recordsWriter;

        public DeixtoAgentScreen()
        {
            _styling = new ElementStyling();
            _builder = new TreeBuilder();
            _imageLoader = new StatesImageLoader();
            _loader = new WindowsViewLoader();
            _readTargetUrls = new ReadTargetUrls();
        }

        public HtmlElement GetElementFromNode(TreeNode node)
        {
            int index = node.SourceIndex();
            var element = _document.GetElementByIndex(index);

            return element;
        }

        public IOpenFileDialog GetTextFileDialog()
        {
            _openFileDialog = new OpenFileDialogWrapper();
            _openFileDialog.Filter = "Text Files (*.txt)|";

            return _openFileDialog;
        }

        public ISaveFileDialog GetTextSaveFileDialog()
        {
            _saveFileDialog = new SaveFileDialogWrapper();
            _saveFileDialog.Filter = "Text Files (*.txt)|";
            _saveFileDialog.Extension = "txt";

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
    }
}

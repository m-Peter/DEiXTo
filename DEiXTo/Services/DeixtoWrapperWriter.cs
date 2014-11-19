using DEiXTo.Models;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class DeixtoWrapperWriter
    {
        private DeixtoWrapper _wrapper;
        private XmlWriter _writer;

        public DeixtoWrapperWriter(DeixtoWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        private void writeInputFile()
        {
            _writer.WriteStartElement("InputFile"); // Write InputFile
            _writer.WriteAttributeString("Filename", _wrapper.InputFile); // Write Filename attribute
            _writer.WriteEndElement(); // Close InputFile element
        }

        private void writeTargetUrls()
        {
            _writer.WriteStartElement("TargetUrls"); // Write TargetUrls element

            if (_wrapper.TargetUrls == null)
            {
                _writer.WriteEndElement(); // Close TargetUrls element
                return;
            }

            foreach (var url in _wrapper.TargetUrls)
            {
                _writer.WriteStartElement("URL"); // Write URL element
                _writer.WriteAttributeString("Address", url); // Write Address attribute
                _writer.WriteEndElement(); // Close URL element
            }

            _writer.WriteEndElement(); // Close TargetUrls element
        }

        private void writeMultiPage()
        {
            _writer.WriteStartElement("MultiplePage"); // Write MultiplePage element
            _writer.WriteAttributeString("Enabled", _wrapper.MultiPageCrawling ? "true" : "false"); // Write Enabled attribute
            _writer.WriteAttributeString("ContainsText", _wrapper.HtmlNextLink); // Write ContainsText attribute
            _writer.WriteAttributeString("MaxCrawlDepth", _wrapper.MaxCrawlingDepth.ToString()); // Write MaxCrawlDepth attribute
            _writer.WriteEndElement(); // Close MultiplePage element
        }

        private void writeMaxHits()
        {
            _writer.WriteStartElement("MaxHits"); // Write MaxHits element
            _writer.WriteAttributeString("Value", _wrapper.NumberOfHits.ToString()); // Write Value attribute
            _writer.WriteEndElement(); // Close MaxHits element
        }

        private void writeExtractPageUrl()
        {
            _writer.WriteStartElement("ExtractPageURL"); // Write ExtractPageUrl element
            _writer.WriteAttributeString("Enabled", _wrapper.ExtractNativeUrl ? "true" : "false"); // Write Enabled attribute
            _writer.WriteEndElement(); // Close ExtractpageUrl element
        }

        private void writeSubmitForm()
        {
            _writer.WriteStartElement("SubmitForm"); // Write SubmitForm element
            _writer.WriteAttributeString("Enabled", _wrapper.AutoFill ? "true" : "false"); // Write Enabled attribute
            _writer.WriteAttributeString("FormName", _wrapper.FormName); // Write FormName attribute
            _writer.WriteAttributeString("InputName", _wrapper.FormInputName); // Write InputName attribute
            _writer.WriteAttributeString("Term", _wrapper.FormTerm); // Write Term attribute
            _writer.WriteEndElement(); // Close SubmitForm element
        }

        private void writeExtractionPattern(TreeNodeCollection nodes)
        {
            _writer.WriteStartElement("ExtractionPattern"); // Write ExtractionPattern element
            writeNodes1(nodes, true);
            _writer.WriteEndElement(); // Close ExtractionPattern element
        }

        private void writeIgnoredTags()
        {
            _writer.WriteStartElement("IgnoredTagsList"); // Write IgnoredTagsList element

            foreach (var tag in _wrapper.IgnoredTags)
            {
                _writer.WriteStartElement("IgnoredTag"); // Write IgnoredTag element
                _writer.WriteAttributeString("Label", tag); // Write the Label attribute
                _writer.WriteEndElement(); // Close IgnoredTag element
            }

            _writer.WriteEndElement(); // Close IgnoredTagsList element
        }

        private void writeOutputFile()
        {
            _writer.WriteStartElement("OutputFile"); // Write OutputFile element
            _writer.WriteAttributeString("Filename", _wrapper.OutputFileName); // Write Filename attribute

            string format = null;
            
            if (_wrapper.OutputFormat == Format.Text)
            {
                format = "TabDelimited";
            }
            else if (_wrapper.OutputFormat == Format.XML)
            {
                format = "XML";
            }
            else if (_wrapper.OutputFormat == Format.RSS)
            {
                format = "RSS";
            }
            
            _writer.WriteAttributeString("Format", format); // Write Format attribute
            _writer.WriteAttributeString("FileMode", _wrapper.OutputMode.ToString()); // Write FileMode attribute
            _writer.WriteEndElement(); // Close OutputFile element
        }

        public void Write(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (_writer = XmlWriter.Create(stream, settings))
            {
                _writer.WriteStartDocument(); // Write the first line
                _writer.WriteDocType("Project", null, "wpf.dtd", null); // Write the DOCTYPE

                _writer.WriteStartElement("Project"); // Write Project element

                writeInputFile();
                //writeTargetUrls();
                writeMultiPage();
                writeMaxHits();
                writeExtractPageUrl();
                writeSubmitForm();
                //writeExtractionPattern(_wrapper.ExtractionPattern.Nodes);
                //writeIgnoredTags();
                writeOutputFile();

                _writer.WriteEndElement(); // Close Project element
                _writer.WriteEndDocument(); // Close the document
            }
        }

        public void write(string filename, TreeNodeCollection nodes)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (_writer = XmlWriter.Create(filename, settings))
            {
                _writer.WriteStartDocument(); // Write the first line
                _writer.WriteDocType("Project", null, "wpf.dtd", null); // Write the DOCTYPE

                _writer.WriteStartElement("Project"); // Write Project element

                writeInputFile();
                writeTargetUrls();
                writeMultiPage();
                writeMaxHits();
                writeExtractPageUrl();
                writeSubmitForm();
                writeExtractionPattern(nodes);
                writeIgnoredTags();
                writeOutputFile();

                _writer.WriteEndElement(); // Close Project element
                _writer.WriteEndDocument(); // Close the document
            }
        }

        private void writeNodes1(TreeNodeCollection nodes, bool isRoot)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.SelectedImageIndex == 5 || node.ImageIndex == 5)
                {
                    continue;
                }

                _writer.WriteStartElement("Node"); // Write Node element
                _writer.WriteAttributeString("tag", node.Text); // Write tag attribute
                string stateIndex = NodeStateTranslator.StateToString(node.GetState());
                _writer.WriteAttributeString("stateIndex", stateIndex); // Write stateIndex attribute

                if (isRoot)
                {
                    _writer.WriteAttributeString("IsRoot", "true");
                }

                writeNodes1(node.Nodes, false);
                _writer.WriteEndElement(); // Close Node Element
            }
        }
    }
}

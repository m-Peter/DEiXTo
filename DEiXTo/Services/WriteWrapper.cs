using DEiXTo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class WriteWrapper
    {
        private DeixtoWrapper _wrapper;

        public WriteWrapper(DeixtoWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public void write(string filename, TreeNodeCollection nodes)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument(); // Write the first line
                writer.WriteDocType("Project", null, "wpf.dtd", null); // Write the DOCTYPE

                writer.WriteStartElement("Project"); // Write Project element

                writer.WriteStartElement("InputFile"); // Write InputFile
                writer.WriteAttributeString("Filename", _wrapper.InputFile); // Write Filename attribute
                writer.WriteEndElement(); // Close InputFile element

                writer.WriteStartElement("TargetUrls"); // Write TargetUrls element

                foreach (var url in _wrapper.TargetUrls)
                {
                    writer.WriteStartElement("URL"); // Write URL element
                    writer.WriteAttributeString("Address", url); // Write Address attribute
                    writer.WriteEndElement(); // Close URL element
                }

                writer.WriteEndElement(); // Close TargetUrls element

                writer.WriteStartElement("MultiplePage"); // Write MultiplePage element
                writer.WriteAttributeString("Enabled", _wrapper.MultiPageCrawling ? "true" : "false"); // Write Enabled attribute
                writer.WriteAttributeString("ContainsText", _wrapper.HtmlNextLink); // Write ContainsText attribute
                writer.WriteAttributeString("MaxCrawlDepth", _wrapper.MaxCrawlingDepth.ToString()); // Write MaxCrawlDepth attribute
                writer.WriteEndElement(); // Close MultiplePage element

                writer.WriteStartElement("MaxHits"); // Write MaxHits element
                writer.WriteAttributeString("Value", _wrapper.NumberOfHits.ToString()); // Write Value attribute
                writer.WriteEndElement(); // Close MaxHits element

                writer.WriteStartElement("ExtractPageUrl"); // Write ExtractPageUrl element
                writer.WriteAttributeString("Enabled", _wrapper.ExtractNativeUrl ? "true" : "false"); // Write Enabled attribute
                writer.WriteEndElement(); // Close ExtractpageUrl element

                writer.WriteStartElement("SubmitForm"); // Write SubmitForm element
                writer.WriteAttributeString("Enabled", _wrapper.AutoFill ? "true" : "false"); // Write Enabled attribute
                writer.WriteAttributeString("FormName", _wrapper.FormName); // Write FormName attribute
                writer.WriteAttributeString("InputName", _wrapper.FormInputName); // Write InputName attribute
                writer.WriteAttributeString("Term", _wrapper.FormTerm); // Write Term attribute
                writer.WriteEndElement(); // Close SubmitForm element

                writer.WriteStartElement("ExtractionPattern"); // Write ExtractionPattern element
                writeNodes1(writer, nodes, true);
                writer.WriteEndElement(); // Close ExtractionPattern element

                writer.WriteStartElement("IgnoredTagsList"); // Write IgnoredTagsList element
                foreach (var tag in _wrapper.IgnoredTags)
                {
                    writer.WriteStartElement("IgnoredTag"); // Write IgnoredTag element
                    writer.WriteAttributeString("Label", tag); // Write the Label attribute
                    writer.WriteEndElement(); // Close IgnoredTag element
                }
                writer.WriteEndElement(); // Close IgnoredTagsList element

                writer.WriteStartElement("OutputFile"); // Write OutputFile element
                writer.WriteAttributeString("Filename", _wrapper.OutputFileName); // Write Filename attribute
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
                writer.WriteAttributeString("Format", format); // Write Format attribute
                writer.WriteAttributeString("FileMode", _wrapper.OutputMode.ToString()); // Write FileMode attribute
                writer.WriteEndElement(); // Close OutputFile element

                writer.WriteEndElement(); // Close Project element
                writer.WriteEndDocument(); // Close the document
            }
        }

        private void writeNodes1(XmlWriter writer, TreeNodeCollection nodes, bool isRoot)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.SelectedImageIndex == 5 || node.ImageIndex == 5)
                {
                    continue;
                }

                writer.WriteStartElement("Node"); // Write Node element
                writer.WriteAttributeString("tag", node.Text); // Write tag attribute
                string stateIndex = getStateIndex1(node.SelectedImageIndex);
                writer.WriteAttributeString("stateIndex", stateIndex); // Write stateIndex attribute

                if (isRoot)
                {
                    writer.WriteAttributeString("IsRoot", "true");
                }

                writeNodes1(writer, node.Nodes, false);
                writer.WriteEndElement(); // Close Node Element
            }
        }

        private string getStateIndex1(int imageKey)
        {
            var state = "";

            switch (imageKey)
            {
                case 0:
                    state = "checked";
                    break;
                case 1:
                    state = "checked_implied";
                    break;
                case 2:
                    state = "checked_source";
                    break;
                case 3:
                    state = "grayed";
                    break;
                case 4:
                    state = "grayed_implied";
                    break;
                case 5:
                    state = "dont_care";
                    break;
            }

            return state;
        }
    }
}

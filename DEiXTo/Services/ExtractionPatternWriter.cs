using DEiXTo.Models;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    // I want to create a repository for loading/saving extraction patterns
    // interface IExtractionPatternRepository
    // - ExtractionPattern Load()
    // - void Save(ExtractionPattern pattern)
    // Then implement this interface for loading/saving on XML files.
    // The Save(ExtractionPattern pattern) method will receive an object of
    // ExtractionPattern type and will save it to an XML file, using the
    // WriteExtractionPattern class.
    public class ExtractionPatternWriter
    {
        public void Write(Stream stream, ExtractionPattern pattern)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (var writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument(); // Write the first line
                writer.WriteStartElement("Pattern"); // Write Pattern element
                writeNodes(writer, pattern.RootNode.Nodes);
                writer.WriteEndElement(); // Close Pattern element
            }
        }

        public void write(string filename, TreeNodeCollection nodes)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument(); // Write the first line

                writer.WriteStartElement("Pattern"); // Write Pattern element

                writeNodes(writer, nodes);

                writer.WriteEndElement(); // Close Pattern element
            }
        }

        private void writeNodes(XmlWriter writer, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.IsSkipped())
                {
                    continue;
                }

                writer.WriteStartElement("Node"); // Write Node element
                writer.WriteAttributeString("tag", node.Text); // Write tag attribute
                string stateIndex = getStateIndex(node.SelectedImageIndex);
                writer.WriteAttributeString("stateIndex", stateIndex); // Write stateIndex attribute

                if (node.HasRegex())
                {
                    writer.WriteAttributeString("regexpr", node.GetRegex());
                }

                if (node.IsRoot())
                {
                    writer.WriteAttributeString("IsRoot", "true");
                }

                writeNodes(writer, node.Nodes);
                writer.WriteEndElement(); // Close Node Element
            }
        }

        private string getStateIndex(int imageKey)
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

using DEiXTo.Models;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
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
                writePattern(writer, pattern.RootNode);
                writer.WriteEndElement(); // Close Pattern element
            }
        }

        private void writePattern(XmlWriter writer, TreeNode node)
        {
            writer.WriteStartElement("Node"); // Write Node Element
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

            foreach (TreeNode n in node.Nodes)
            {
                writePattern(writer, n);
            }

            writer.WriteEndElement(); // Close Node Element
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

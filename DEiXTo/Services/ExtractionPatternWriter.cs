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
            //string stateIndex = getStateIndex(node.SelectedImageIndex);
            string stateIndex = getStringState(node.GetState());
            writer.WriteAttributeString("stateIndex", stateIndex); // Write stateIndex attribute

            if (node.IsRoot())
            {
                writer.WriteAttributeString("IsRoot", "true");
            }

            if (node.HasRegex())
            {
                writer.WriteAttributeString("regexpr", node.GetRegex());
            }

            if (node.GetCareAboutSiblingOrder())
            {
                writer.WriteAttributeString("CareAboutSO", "1");
                writer.WriteAttributeString("so_start", node.GetStartIndex().ToString());
                writer.WriteAttributeString("so_step", node.GetStepValue().ToString());
            }

            foreach (TreeNode n in node.Nodes)
            {
                writePattern(writer, n);
            }

            writer.WriteEndElement(); // Close Node Element
        }

        private string getStringState(NodeState state)
        {
            var stringState = "";

            switch (state)
            {
                case NodeState.Checked:
                    stringState = "checked";
                    break;
                case NodeState.CheckedImplied:
                    stringState = "checked_implied";
                    break;
                case NodeState.CheckedSource:
                    stringState = "checked_source";
                    break;
                case NodeState.Grayed:
                    stringState = "grayed";
                    break;
                case NodeState.GrayedImplied:
                    stringState = "grayed_implied";
                    break;
                case NodeState.Unchecked:
                    stringState = "dont_care";
                    break;
            }

            return stringState;
        }
    }
}

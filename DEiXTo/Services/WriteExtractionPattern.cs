using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class WriteExtractionPattern
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="nodes"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="nodes"></param>
        /// <param name="isRoot"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageKey"></param>
        /// <returns></returns>
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

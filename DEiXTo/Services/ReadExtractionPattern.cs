using DEiXTo.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class ReadExtractionPattern
    {
        public TreeNode read(string filename)
        {
            var rootNode = new TreeNode();

            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Pattern")
                    {

                        reader.ReadToFollowing("Node");
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader.ReadSubtree());
                        createPattern(doc.ChildNodes, rootNode);
                    }
                }
            }

            return rootNode;
        }

        private void createPattern(XmlNodeList nodes, TreeNode tNode)
        {
            foreach (XmlNode node in nodes)
            {
                TreeNode temp;
                if (node.NodeType == XmlNodeType.Element && node.Name == "Node")
                {
                    if (tNode.Text == String.Empty)
                    {
                        NodeInfo pInfo = new NodeInfo();
                        tNode.Text = node.Attributes["tag"].Value;
                        var isRoot = node.Attributes["IsRoot"];

                        if (isRoot != null && isRoot.Value == "true")
                        {
                            pInfo.IsRoot = true;
                            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
                            tNode.NodeFont = new Font(font, FontStyle.Bold);
                        }

                        tNode.Tag = pInfo;
                        var state = node.Attributes["stateIndex"].Value;

                        tNode.SelectedImageIndex = getStateIndex(state);
                        tNode.ImageIndex = getStateIndex(state);
                        createPattern(node.ChildNodes, tNode);
                    }
                    else
                    {
                        NodeInfo pInfo = new NodeInfo();
                        temp = tNode.Nodes.Add(node.Attributes["tag"].Value);
                        var isRoot = node.Attributes["IsRoot"];

                        if (isRoot != null && isRoot.Value == "true")
                        {
                            pInfo.IsRoot = true;
                            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
                            temp.NodeFont = new Font(font, FontStyle.Bold);
                        }

                        temp.Tag = pInfo;
                        var state = node.Attributes["stateIndex"].Value;

                        temp.SelectedImageIndex = getStateIndex(state);
                        temp.ImageIndex = getStateIndex(state);
                        createPattern(node.ChildNodes, temp);
                    }
                }
            }
        }

        private int getStateIndex(string state)
        {
            int index = -1;

            switch (state)
            {
                case "checked":
                    index = 0;
                    break;
                case "checked_implied":
                    index = 1;
                    break;
                case "checked_source":
                    index = 2;
                    break;
                case "grayed":
                    index = 3;
                    break;
                case "grayed_implied":
                    index = 4;
                    break;
                case "dont_care":
                    index = 5;
                    break;
            }

            return index;
        }
    }
}
